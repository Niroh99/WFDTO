using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WFDTOCustomControlLibrary
{
    public class GradientBackgroundBorder : Border
    {
        static GradientBackgroundBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GradientBackgroundBorder), new FrameworkPropertyMetadata(typeof(GradientBackgroundBorder)));
        }

        public static readonly DependencyProperty GradientColorProperty = DependencyProperty.Register("GradientColor", typeof(Brush), typeof(GradientBackgroundBorder), new FrameworkPropertyMetadata(Brushes.White, GradientColorPropertyChanged));

        private static void GradientColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GradientBackgroundBorder)d).OnGradientColorPropertyChanged();
        }

        public Brush GradientColor
        {
            get { return (Brush)GetValue(GradientColorProperty); }
            set { SetValue(GradientColorProperty, value); }
        }

        public static readonly DependencyProperty GradientTypeProperty = DependencyProperty.Register("GradientType", typeof(GradientType), typeof(GradientBackgroundBorder), new FrameworkPropertyMetadata(GradientType.Vertical));
        public GradientType GradientType
        {
            get { return (GradientType)GetValue(GradientTypeProperty); }
            set { SetValue(GradientTypeProperty, value); }
        }

        public static readonly DependencyProperty ShowGradientProperty = DependencyProperty.Register("ShowGradient", typeof(bool), typeof(GradientBackgroundBorder), new FrameworkPropertyMetadata(false, ShowGradientPropertyChanged));

        private static void ShowGradientPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GradientBackgroundBorder)d).OnShowGradientPropertyChanged();
        }

        public bool ShowGradient
        {
            get { return (bool)GetValue(ShowGradientProperty); }
            set { SetValue(ShowGradientProperty, value); }
        }

        private void OnGradientColorPropertyChanged()
        {
            SetStoryBoards();
        }

        private void OnShowGradientPropertyChanged()
        {
            if (ShowGradient)
            {
                if (ShowGradientStoryboard != null) ShowGradientStoryboard.Begin();
            }
            else
            {
                if (HideGradientStoryboard != null) HideGradientStoryboard.Begin();
            }
        }

        private Storyboard ShowGradientStoryboard;
        private Storyboard HideGradientStoryboard;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            SetStoryBoards();
        }

        private void SetStoryBoards()
        {
            if (!(GradientColor is SolidColorBrush)) throw new ArgumentException("Graident Color has to be of Type SolidColorBrush.");

            GradientBrush backgroundGradientBrush;

            ShowGradientStoryboard = new Storyboard()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            var showGradientAnimation = new ColorAnimation(((SolidColorBrush)GradientColor).Color, ShowGradientStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            ShowGradientStoryboard.Children.Add(showGradientAnimation);

            Storyboard.SetTarget(showGradientAnimation, this);

            HideGradientStoryboard = new Storyboard()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            var hideGradientAnimation = new ColorAnimation(Colors.Transparent, HideGradientStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(hideGradientAnimation, this);

            HideGradientStoryboard.Children.Add(hideGradientAnimation);

            if (GradientType == GradientType.Vertical)
            {
                backgroundGradientBrush = new LinearGradientBrush
                {
                    StartPoint = new Point(0.5, 0),
                    EndPoint = new Point(0.5, 1)
                };

                Storyboard.SetTargetProperty(showGradientAnimation, new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[1].(GradientStop.Color)"));

                Storyboard.SetTargetProperty(hideGradientAnimation, new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[1].(GradientStop.Color)"));
            }
            else if (GradientType == GradientType.Radial)
            {
                backgroundGradientBrush = new RadialGradientBrush();

                Storyboard.SetTargetProperty(showGradientAnimation, new PropertyPath("(Border.Background).(RadialGradientBrush.GradientStops)[1].(GradientStop.Color)"));

                Storyboard.SetTargetProperty(hideGradientAnimation, new PropertyPath("(Border.Background).(RadialGradientBrush.GradientStops)[1].(GradientStop.Color)"));
            }
            else backgroundGradientBrush = new LinearGradientBrush();

            Background = backgroundGradientBrush;

            backgroundGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            backgroundGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            OnShowGradientPropertyChanged();
        }
    }

    public enum GradientType
    {
        Vertical,
        Radial
    }
}
