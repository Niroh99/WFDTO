using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WFDTOCustomControlLibrary
{
    public class ClosableTabItem : TabItem
    {
        static ClosableTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClosableTabItem), new FrameworkPropertyMetadata(typeof(ClosableTabItem)));
        }

        public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(ClosableTabItem));
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        private Grid MainGrid;
        private Rectangle SelectedHighlightRectangle;
        private Button CloseButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CloseButton = (Button)GetTemplateChild("CloseButton");
            SelectedHighlightRectangle = (Rectangle)GetTemplateChild("SelectedHighlightRectangle");
            MainGrid = (Grid)GetTemplateChild("MainGrid");

            CloseButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is TabControl parentTabControl)
            {
                if (parentTabControl.Items.Count == 1)
                {

                }
                else parentTabControl.Items.Remove(this);
            }
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            Dispatcher.BeginInvoke(new Action(() => SelectedHighlightRectangle.BeginAnimation(WidthProperty, new DoubleAnimation(MainGrid.ActualWidth, TimeSpan.FromSeconds(0.2)))), System.Windows.Threading.DispatcherPriority.Loaded);
        }
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);

            Dispatcher.BeginInvoke(new Action(() => SelectedHighlightRectangle.BeginAnimation(WidthProperty, new DoubleAnimation(SelectedHighlightRectangle.ActualWidth, 0, TimeSpan.FromSeconds(0.2)))), System.Windows.Threading.DispatcherPriority.Loaded);
        }
    }
}