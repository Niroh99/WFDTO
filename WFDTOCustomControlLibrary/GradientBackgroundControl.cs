using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WFDTOCustomControlLibrary
{
    public class GradientBackgroundControl : Border
    {
        static GradientBackgroundControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GradientBackgroundControl), new FrameworkPropertyMetadata(typeof(GradientBackgroundControl)));
        }

        public static readonly DependencyProperty GradientColorProperty = DependencyProperty.Register("GradientColor", typeof(Color), typeof(GradientBackgroundControl), new FrameworkPropertyMetadata(Colors.White));
        public Color GradientColor
        {
            get { return (Color)GetValue(GradientColorProperty); }
            set { SetValue(GradientColorProperty, value); }
        }

        public static readonly DependencyProperty ShowGradientProperty = DependencyProperty.Register("ShowGradient", typeof(bool), typeof(GradientBackgroundControl), new FrameworkPropertyMetadata(false, ShowGradientPropertyChanged));

        private static void ShowGradientPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GradientBackgroundControl)d).OnShowGradientPropertyChanged();
        }

        public bool ShowGradient
        {
            get { return (bool)GetValue(ShowGradientProperty); }
            set { SetValue(ShowGradientProperty, value); }
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

            var backgroundGradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

            backgroundGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            backgroundGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            Background = backgroundGradientBrush;

            ShowGradientStoryboard = new Storyboard()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            var showGradientAnimation = new ColorAnimation(GradientColor, ShowGradientStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(showGradientAnimation, this);
            Storyboard.SetTargetProperty(showGradientAnimation, new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[1].(GradientStop.Color)"));

            ShowGradientStoryboard.Children.Add(showGradientAnimation);

            HideGradientStoryboard = new Storyboard()
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            var hideGradientAnimation = new ColorAnimation(Colors.Transparent, HideGradientStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(hideGradientAnimation, this);
            Storyboard.SetTargetProperty(hideGradientAnimation, new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[1].(GradientStop.Color)"));

            HideGradientStoryboard.Children.Add(hideGradientAnimation);

            OnShowGradientPropertyChanged();
        }
    }
}
