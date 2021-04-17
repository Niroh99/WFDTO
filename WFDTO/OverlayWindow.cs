using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shell;

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
        private Popup AlignmentPopup;
        private Button AlignTopButton;
        private Button AlignBottomButton;
        private Button AlignLeftButton;
        private Button AlignRightButton;
        private Thumb RepositionThumb;

        public EventHandler TemplateApplyed;

        public override void OnApplyTemplate()
        {
            ContentPresenterBorder = (Border)GetTemplateChild("ContentPresenterBorder");
            ContentPresenter = (ContentPresenter)GetTemplateChild("ContentPresenter");
            CloseButton = (Button)GetTemplateChild("CloseButton");
            ButtonsStackPanel = (StackPanel)GetTemplateChild("ButtonsStackPanel");
            AlignmentPopupButton = (Button)GetTemplateChild("AlignmentPopupButton");
            AlignmentPopup = (Popup)GetTemplateChild("AlignmentPopup");
            AlignTopButton = (Button)GetTemplateChild("AlignTopButton");
            AlignBottomButton = (Button)GetTemplateChild("AlignBottomButton");
            AlignLeftButton = (Button)GetTemplateChild("AlignLeftButton");
            AlignRightButton = (Button)GetTemplateChild("AlignRightButton");
            RepositionThumb = (Thumb)GetTemplateChild("RepositionThumb");

            CloseButton.Click += CloseButton_Click;
            AlignmentPopupButton.Click += AlignmentPopupButton_Click;
            AlignTopButton.Click += AlignTopButton_Click;
            AlignBottomButton.Click += AlignBottomButton_Click;
            AlignLeftButton.Click += AlignLeftButton_Click;
            AlignRightButton.Click += AlignRightButton_Click;
            RepositionThumb.DragDelta += RepositionThumb_DragDelta;

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AlignmentPopupButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlignmentPopup != null) AlignmentPopup.IsOpen = true;
        }

        private void AlignTopButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandDirection = ExpandDirection.Down;
        }

        private void AlignBottomButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandDirection = ExpandDirection.Up;
        }

        private void AlignLeftButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandDirection = ExpandDirection.Right;
        }

        private void AlignRightButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandDirection = ExpandDirection.Left;
        }

        private void RepositionThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Up:
                case ExpandDirection.Down:
                {
                    if (Left + Width + e.HorizontalChange <= SystemParameters.PrimaryScreenWidth && Left + e.HorizontalChange >= 0) Left += e.HorizontalChange;
                    break;
                }
                case ExpandDirection.Left:
                case ExpandDirection.Right:
                {
                    if (Top + Height + e.VerticalChange <= SystemParameters.PrimaryScreenHeight && Top + e.VerticalChange >= 0) Top += e.VerticalChange;
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
                    ButtonsStackPanel.Margin = new Thickness(0, 2, 0, 0);
                    AlignmentPopup.Placement = PlacementMode.Top;
                    RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeWE;

                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        if (!IsExpanded)
                        {
                            Height = ButtonsStackPanel.RenderSize.Height;
                            Width = ButtonsStackPanel.RenderSize.Width;
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
                    ButtonsStackPanel.Margin = new Thickness(0, 0, 0, 2);
                    AlignmentPopup.Placement = PlacementMode.Bottom;
                    RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeWE;

                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        if (!IsExpanded)
                        {
                            Height = ButtonsStackPanel.RenderSize.Height;
                            Width = ButtonsStackPanel.RenderSize.Width;
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
                    AlignmentPopup.Placement = PlacementMode.Left;
                    ButtonsStackPanel.SetValue(Grid.ColumnProperty, 2);
                    ButtonsStackPanel.SetValue(Grid.RowProperty, 1);
                    ButtonsStackPanel.Margin = new Thickness(2, 0, 0, 0);
                    RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeNS;

                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        if (!IsExpanded)
                        {
                            Height = ButtonsStackPanel.RenderSize.Height;
                            Width = ButtonsStackPanel.RenderSize.Width;
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
                    AlignmentPopup.Placement = PlacementMode.Right;
                    ButtonsStackPanel.SetValue(Grid.ColumnProperty, 0);
                    ButtonsStackPanel.SetValue(Grid.RowProperty, 1);
                    ButtonsStackPanel.Margin = new Thickness(0, 0, 2, 0);
                    RepositionThumb.Cursor = System.Windows.Input.Cursors.SizeNS;

                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        if (!IsExpanded)
                        {
                            Height = ButtonsStackPanel.RenderSize.Height;
                            Width = ButtonsStackPanel.RenderSize.Width;
                        }

                        Left = 0;

                        if (Top + Height > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - Height;
                    }));
                    break;
                }
            }

            AlignmentPopup.IsOpen = false;
        }

        private void OnIsExpandedPropertyChanged()
        {
            if (IsExpanded)
            {
                Height = TargetHeight;
                Width = TargetWidth;

                switch (ExpandDirection)
                {
                    case System.Windows.Controls.ExpandDirection.Up:
                    {
                        Left -= TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                        if (Left < 0) Left = 0;

                        if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                        Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                        var windowChrome = WindowChrome.GetWindowChrome(this);
                        windowChrome.ResizeBorderThickness = new Thickness(5, 5, 5, 0);

                        break;
                    }
                    case System.Windows.Controls.ExpandDirection.Down:
                    {
                        Left -= TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                        if (Left < 0) Left = 0;

                        if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                        var windowChrome = WindowChrome.GetWindowChrome(this);
                        windowChrome.ResizeBorderThickness = new Thickness(5, 0, 5, 5);

                        break;
                    }
                    case System.Windows.Controls.ExpandDirection.Left:
                    {
                        Top -= TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                        if (Top < 0) Top = 0;

                        if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                        Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                        var windowChrome = WindowChrome.GetWindowChrome(this);
                        windowChrome.ResizeBorderThickness = new Thickness(5, 5, 0, 5);

                        break;
                    }
                    case System.Windows.Controls.ExpandDirection.Right:
                    {
                        Top -= TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                        if (Top < 0) Top = 0;

                        if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                        var windowChrome = WindowChrome.GetWindowChrome(this);
                        windowChrome.ResizeBorderThickness = new Thickness(0, 5, 5, 5);

                        break;
                    }
                }

                Background = (Brush)TryFindResource("ControlBackground");
                ContentPresenterBorder.Visibility = Visibility.Visible;
                ContentPresenter.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
            }
            else
            {
                Height = ButtonsStackPanel.ActualHeight;
                Width = ButtonsStackPanel.ActualWidth;

                switch (ExpandDirection)
                {
                    case System.Windows.Controls.ExpandDirection.Up:
                    {
                        Left += TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                        if (Left < 0) Left = 0;

                        if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                        Top = SystemParameters.PrimaryScreenHeight - ButtonsStackPanel.ActualHeight;

                        break;
                    }
                    case System.Windows.Controls.ExpandDirection.Down:
                    {
                        Left += TargetWidth / 2 - ButtonsStackPanel.ActualWidth / 2;

                        if (Left < 0) Left = 0;

                        if (Left + TargetWidth > SystemParameters.PrimaryScreenWidth) Left = SystemParameters.PrimaryScreenWidth - TargetWidth;

                        break;
                    }
                    case System.Windows.Controls.ExpandDirection.Left:
                    {
                        Top += TargetHeight / 2 - ButtonsStackPanel.ActualHeight / 2;

                        if (Top < 0) Top = 0;

                        if (Top + TargetHeight > SystemParameters.PrimaryScreenHeight) Top = SystemParameters.PrimaryScreenHeight - TargetHeight;

                        Left = SystemParameters.PrimaryScreenWidth - ButtonsStackPanel.ActualWidth;

                        break;
                    }
                    case System.Windows.Controls.ExpandDirection.Right:
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
                var windowChrome = WindowChrome.GetWindowChrome(this);
                windowChrome.ResizeBorderThickness = new Thickness(0);
            }
        }
    }
}