using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WFDTOCustomControlLibrary
{
    public class AccentRectangle : Control
    {
        static AccentRectangle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccentRectangle), new FrameworkPropertyMetadata(typeof(AccentRectangle)));
        }

        public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(AccentRectangle), new FrameworkPropertyMetadata(Brushes.White));
        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        public static readonly DependencyProperty ShowHighlightProperty = DependencyProperty.Register("ShowHighlight", typeof(bool), typeof(AccentRectangle), new FrameworkPropertyMetadata(ShowHighlightPropertyChanged));

        private static void ShowHighlightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AccentRectangle)d).UpdateHighlightRectangle();
        }

        public bool ShowHighlight
        {
            get { return (bool)GetValue(ShowHighlightProperty); }
            set { SetValue(ShowHighlightProperty, value); }
        }

        Rectangle HighLightRectangle;

        public override void OnApplyTemplate()
        {
            HighLightRectangle = (Rectangle)GetTemplateChild("HighLightRectangle");
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateHighlightRectangle();
        }

        private void UpdateHighlightRectangle()
        {
            if (ShowHighlight) Dispatcher.BeginInvoke(new Action(() => HighLightRectangle.BeginAnimation(WidthProperty, new DoubleAnimation(ActualWidth, TimeSpan.FromSeconds(0.1)))), System.Windows.Threading.DispatcherPriority.Loaded);
            else Dispatcher.BeginInvoke(new Action(() => HighLightRectangle.BeginAnimation(WidthProperty, new DoubleAnimation(0, TimeSpan.FromSeconds(0.1)))), System.Windows.Threading.DispatcherPriority.Loaded);
        }
    }
}