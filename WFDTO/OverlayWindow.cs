using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using WFDTO.Themes;

namespace WFDTO
{
    public class OverlayWindow : Window
    {
        static OverlayWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayWindow), new FrameworkPropertyMetadata(typeof(OverlayWindow)));
        }

        public static readonly DependencyProperty TargetHeightProperty = DependencyProperty.Register("TargetHeight", typeof(double), typeof(OverlayWindow), new FrameworkPropertyMetadata(400d));
        public double TargetHeight
        {
            get { return (double)GetValue(TargetHeightProperty); }
            set { SetValue(TargetHeightProperty, value); }
        }

        public static readonly DependencyProperty TargetWidthProperty = DependencyProperty.Register("TargetWidth", typeof(double), typeof(OverlayWindow), new FrameworkPropertyMetadata(600d));
        public double TargetWidth
        {
            get { return (double)GetValue(TargetWidthProperty); }
            set { SetValue(TargetWidthProperty, value); }
        }

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register("ExpandDirection", typeof(ExpandDirection), typeof(OverlayWindow), new FrameworkPropertyMetadata(ExpandDirection.Down, ExpandDirectionPropertyChanged));

        private static void ExpandDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OverlayWindow)d).OnExpandDirectionPropertyChanged();
        }

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(OverlayWindow), new PropertyMetadata(false, IsExpandedPropertyChanged));

        private static void IsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OverlayWindow)d).OnIsExpandedPropertyChanged();
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        private Border ContentPresenterBorder;
        private ContentPresenter ContentPresenter;
        private Button CloseButton;
        private StackPanel ButtonsStackPanel;
        private Button AlignmentPopupButton;
        private Thumb RepositionThumb;
        private Thumb TopResizeThumb;
        private Thumb BottomResizeThumb;

        public event EventHandler TemplateApplyed;

        public override void OnApplyTemplate()
        {
            ContentPresenterBorder = (Border)GetTemplateChild("ContentPresenterBorder");
            ContentPresenter = (ContentPresenter)GetTemplateChild("ContentPresenter");
            CloseButton = (Button)GetTemplateChild("CloseButton");
            ButtonsStackPanel = (StackPanel)GetTemplateChild("ButtonsStackPanel");
            AlignmentPopupButton = (Button)GetTemplateChild("AlignmentPopupButton");
            RepositionThumb = (Thumb)GetTemplateChild("RepositionThumb");
            TopResizeThumb = (Thumb)GetTemplateChild("TopRisizeThumb");
            BottomResizeThumb = (Thumb)GetTemplateChild("BottomResizeThumb");

            CloseButton.Click += CloseButton_Click;
            AlignmentPopupButton.Click += AlignmentPopupButton_Click;
            RepositionThumb.DragDelta += RepositionThumb_DragDelta;
            TopResizeThumb.DragDelta += TopResizeThumb_DragDelta;
            BottomResizeThumb.DragDelta += BottomResizeThumb_DragDelta;

            OnExpandDirectionPropertyChanged();

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() =>
            {
                Height = ButtonsStackPanel.RenderSize.Height;
                Width = ButtonsStackPanel.RenderSize.Width;
                MinHeight = ButtonsStackPanel.RenderSize.Height;
                MinWidth = ButtonsStackPanel.RenderSize.Width;
            }));

            TemplateApplyed?.Invoke(this, EventArgs.Empty);
        }

        private void TopResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Top += e.VerticalChange;

            if (Height - e.VerticalChange < 0) Height = 0;
            else Height -= e.VerticalChange;
        }

        private void BottomResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height + e.VerticalChange < 0) Height = 0;
            else Height += e.VerticalChange;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AlignmentPopupButton_Click(object sender, RoutedEventArgs e)
        {
            AlignmentPopupButton.ContextMenu.PlacementTarget = AlignmentPopupButton;
            AlignmentPopupButton.ContextMenu.Placement = PlacementMode.Bottom;
            AlignmentPopupButton.ContextMenu.IsOpen = true;
        }

        ICommand _changeAlignmentCommand;
        public ICommand ChangeAlignmentCommand
        {
            get
            {
                return _changeAlignmentCommand ?? (_changeAlignmentCommand = new WFDTOCustomControlLibrary.DefaultCommand(new Action<object>((parameter) =>
                {
                    switch ((string)parameter)
                    {
                        case "Top": ExpandDirection = ExpandDirection.Down; break;
                        case "Bottom": ExpandDirection = ExpandDirection.Up; break;
                        case "Left": ExpandDirection = ExpandDirection.Right; break;
                        case "Right": ExpandDirection = ExpandDirection.Left; break;
                    }
                })));
            }
        }

        private void RepositionThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Up:
                case ExpandDirection.Down:
                    {
                        var newLeft = Left + e.HorizontalChange;

                        if (newLeft < 0) newLeft = 0;
                        else if (newLeft + Width > SystemParameters.PrimaryScreenWidth) newLeft = SystemParameters.PrimaryScreenWidth - Width;

                        Left = newLeft;

                        break;
                    }
                case ExpandDirection.Left:
                case ExpandDirection.Right:
                    {
                        var newTop = Top + e.VerticalChange;

                        if (newTop < 0) newTop = 0;
                        else if (newTop + Height > SystemParameters.PrimaryScreenHeight) newTop = SystemParameters.PrimaryScreenHeight - Height;

                        Top = newTop;

                        break;
                    }
            }
        }

        private void OnExpandDirectionPropertyChanged()
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Up:
                    {
                        CloseButton.HorizontalAlignment = HorizontalAlignment.Right;
                        CloseButton.VerticalAlignment = VerticalAlignment.Center;
                        CloseButton.SetValue(Grid.ColumnProperty, 1);
                        CloseButton.SetValue(Grid.RowProperty, 2);
                        ButtonsStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                        ButtonsStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
                        ButtonsStackPanel.Orientation = Orientation.Horizontal;
                        ButtonsStackPanel.SetValue(Grid.ColumnProperty, 1);
                        ButtonsStackPanel.SetValue(Grid.RowProperty, 2);
                        RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeWE;

                        if (IsExpanded)
                        {
                            TopResizeThumb.Visibility = Visibility.Visible;
                            BottomResizeThumb.Visibility = Visibility.Collapsed;
                        }

                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                        {
                            if (!IsExpanded)
                            {
                                Height = ButtonsStackPanel.RenderSize.Height;
                                MinHeight = ButtonsStackPanel.RenderSize.Height;
                                Width = ButtonsStackPanel.RenderSize.Width;
                                MinWidth = ButtonsStackPanel.RenderSize.Width;
                            }

                            Top = SystemParameters.PrimaryScreenHeight - Height;

                            if (Left + Width > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - Width;
                        }));
                        break;
                    }
                case ExpandDirection.Down:
                    {
                        CloseButton.HorizontalAlignment = HorizontalAlignment.Right;
                        CloseButton.VerticalAlignment = VerticalAlignment.Center;
                        CloseButton.SetValue(Grid.ColumnProperty, 1);
                        CloseButton.SetValue(Grid.RowProperty, 0);
                        ButtonsStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                        ButtonsStackPanel.VerticalAlignment = VerticalAlignment.Top;
                        ButtonsStackPanel.Orientation = Orientation.Horizontal;
                        ButtonsStackPanel.SetValue(Grid.ColumnProperty, 1);
                        ButtonsStackPanel.SetValue(Grid.RowProperty, 0);
                        RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeWE;

                        if (IsExpanded)
                        {
                            TopResizeThumb.Visibility = Visibility.Collapsed;
                            BottomResizeThumb.Visibility = Visibility.Visible;
                        }

                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                        {
                            if (!IsExpanded)
                            {
                                Height = ButtonsStackPanel.RenderSize.Height;
                                MinHeight = ButtonsStackPanel.RenderSize.Height;
                                Width = ButtonsStackPanel.RenderSize.Width;
                                MinWidth = ButtonsStackPanel.RenderSize.Width;
                            }

                            Top = 0;

                            if (Left + Width > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - Width;
                        }));
                        break;
                    }
                case ExpandDirection.Left:
                    {
                        CloseButton.HorizontalAlignment = HorizontalAlignment.Center;
                        CloseButton.VerticalAlignment = VerticalAlignment.Top;
                        CloseButton.SetValue(Grid.ColumnProperty, 2);
                        CloseButton.SetValue(Grid.RowProperty, 1);
                        ButtonsStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
                        ButtonsStackPanel.VerticalAlignment = VerticalAlignment.Center;
                        ButtonsStackPanel.Orientation = Orientation.Vertical;
                        ButtonsStackPanel.SetValue(Grid.ColumnProperty, 2);
                        ButtonsStackPanel.SetValue(Grid.RowProperty, 1);
                        RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeNS;

                        if (IsExpanded)
                        {
                            TopResizeThumb.Visibility = Visibility.Visible;
                            BottomResizeThumb.Visibility = Visibility.Visible;
                        }

                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                        {
                            if (!IsExpanded)
                            {
                                Height = ButtonsStackPanel.RenderSize.Height;
                                MinHeight = ButtonsStackPanel.RenderSize.Height;
                                Width = ButtonsStackPanel.RenderSize.Width;
                                MinWidth = ButtonsStackPanel.RenderSize.Width;
                            }

                            Left = SystemParameters.PrimaryScreenWidth - Width;

                            if (Top + Height > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - Height;
                        }));
                        break;
                    }
                case ExpandDirection.Right:
                    {
                        CloseButton.HorizontalAlignment = HorizontalAlignment.Center;
                        CloseButton.VerticalAlignment = VerticalAlignment.Top;
                        CloseButton.SetValue(Grid.ColumnProperty, 0);
                        CloseButton.SetValue(Grid.RowProperty, 1);
                        ButtonsStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                        ButtonsStackPanel.VerticalAlignment = VerticalAlignment.Center;
                        ButtonsStackPanel.Orientation = Orientation.Vertical;
                        ButtonsStackPanel.SetValue(Grid.ColumnProperty, 0);
                        ButtonsStackPanel.SetValue(Grid.RowProperty, 1);
                        RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeNS;

                        if (IsExpanded)
                        {
                            TopResizeThumb.Visibility = Visibility.Visible;
                            BottomResizeThumb.Visibility = Visibility.Visible;
                        }

                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                        {
                            if (!IsExpanded)
                            {
                                Height = ButtonsStackPanel.RenderSize.Height;
                                MinHeight = ButtonsStackPanel.RenderSize.Height;
                                Width = ButtonsStackPanel.RenderSize.Width;
                                MinWidth = ButtonsStackPanel.RenderSize.Width;
                            }

                            Left = 0;

                            if (Top + Height > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - Height;
                        }));
                        break;
                    }
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }

        private void OnIsExpandedPropertyChanged()
        {
            if (IsExpanded)
            {
                Height = TargetHeight;
                Width = TargetWidth;
                MinWidth = TargetWidth;

                switch (ExpandDirection)
                {
                    case ExpandDirection.Up:
                        {
                            Left -= TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                            if (Left < 0) Left = 0;

                            if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                            Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                            TopResizeThumb.Visibility = Visibility.Visible;

                            break;
                        }
                    case ExpandDirection.Down:
                        {
                            Left -= TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                            if (Left < 0) Left = 0;

                            if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                            BottomResizeThumb.Visibility = Visibility.Visible;

                            break;
                        }
                    case ExpandDirection.Left:
                        {
                            Top -= TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                            if (Top < 0) Top = 0;

                            if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                            Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                            TopResizeThumb.Visibility = Visibility.Visible;
                            BottomResizeThumb.Visibility = Visibility.Visible;

                            break;
                        }
                    case ExpandDirection.Right:
                        {
                            Top -= TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                            if (Top < 0) Top = 0;

                            if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                            TopResizeThumb.Visibility = Visibility.Visible;
                            BottomResizeThumb.Visibility = Visibility.Visible;

                            break;
                        }
                }

                BindingOperations.SetBinding(this, BackgroundProperty, new ColorBinding("Background"));

                ContentPresenterBorder.Visibility = Visibility.Visible;
                ContentPresenter.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
            }
            else
            {
                Height = ButtonsStackPanel.ActualHeight;
                MinHeight = ButtonsStackPanel.ActualHeight;
                Width = ButtonsStackPanel.ActualWidth;
                MinWidth = ButtonsStackPanel.ActualWidth;

                TopResizeThumb.Visibility = Visibility.Collapsed;
                BottomResizeThumb.Visibility = Visibility.Collapsed;

                switch (ExpandDirection)
                {
                    case ExpandDirection.Up:
                        {
                            Left += TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                            if (Left < 0) Left = 0;

                            if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                            Top = SystemParameters.PrimaryScreenHeight - ButtonsStackPanel.ActualHeight;

                            break;
                        }
                    case ExpandDirection.Down:
                        {
                            Left += TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                            if (Left < 0) Left = 0;

                            if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                            break;
                        }
                    case ExpandDirection.Left:
                        {
                            Top += TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                            if (Top < 0) Top = 0;

                            if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                            Left = SystemParameters.PrimaryScreenWidth - ButtonsStackPanel.ActualWidth;

                            break;
                        }
                    case ExpandDirection.Right:
                        {
                            Top += TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                            if (Top < 0) Top = 0;

                            if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                            break;
                        }
                }

                Background = Brushes.Transparent;
                ContentPresenterBorder.Visibility = Visibility.Collapsed;
                ContentPresenter.Visibility = Visibility.Collapsed;
                CloseButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}