using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WFDTO
{
    public class ExpandToggleButton : ToggleButton
    {
        static ExpandToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandToggleButton), new FrameworkPropertyMetadata(typeof(ExpandToggleButton)));
        }

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register("ExpandDirection", typeof(ExpandDirection), typeof(ExpandToggleButton), new FrameworkPropertyMetadata(ExpandDirection.Down, ExpandDirectionPropertyChanged));

        private static void ExpandDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ExpandToggleButton)d).OnExpandDirectionPropertyChanged();
        }

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        private Storyboard UpStoryboard;
        private Storyboard DownStoryboard;
        private Storyboard LeftStoryboard;
        private Storyboard RightStoryboard;

        private Path ArrowPath;

        public override void OnApplyTemplate()
        {
            ArrowPath = (Path)GetTemplateChild("ArrowPath");

            CreateUpStoryboard();
            CreateDownStoryboard();
            CreateLeftStoryboard();
            CreateRightStoryboard();

            OnExpandDirectionPropertyChanged();
        }

        private void CreateUpStoryboard()
        {
            UpStoryboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            var doubleAnimation = new DoubleAnimation(180, UpStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(doubleAnimation, ArrowPath);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            UpStoryboard.Children.Add(doubleAnimation);
        }

        private void CreateDownStoryboard()
        {
            DownStoryboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            var doubleAnimation = new DoubleAnimation(0, DownStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(doubleAnimation, ArrowPath);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            DownStoryboard.Children.Add(doubleAnimation);
        }

        private void CreateLeftStoryboard()
        {
            LeftStoryboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            var doubleAnimation = new DoubleAnimation(90, LeftStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(doubleAnimation, ArrowPath);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            LeftStoryboard.Children.Add(doubleAnimation);
        }

        private void CreateRightStoryboard()
        {
            RightStoryboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            var doubleAnimation = new DoubleAnimation(270, RightStoryboard.Duration)
            {
                BeginTime = new TimeSpan(0)
            };

            Storyboard.SetTarget(doubleAnimation, ArrowPath);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            RightStoryboard.Children.Add(doubleAnimation);
        }

        private void OnExpandDirectionPropertyChanged()
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Up:
                {
                    if (IsChecked == true) UpStoryboard?.Begin();
                    else DownStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Down:
                {
                    if (IsChecked == true) DownStoryboard.Begin();
                    else UpStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Left:
                {
                    if (IsChecked == true) LeftStoryboard.Begin();
                    else RightStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Right:
                {
                    if (IsChecked == true) RightStoryboard?.Begin();
                    else LeftStoryboard?.Begin();
                    break;
                }
            }
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Up:
                {
                    UpStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Down:
                {
                    DownStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Left:
                {
                    LeftStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Right:
                {
                    RightStoryboard?.Begin();
                    break;
                }
            }
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Up:
                {
                    DownStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Down:
                {
                    UpStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Left:
                {
                    RightStoryboard?.Begin();
                    break;
                }
                case ExpandDirection.Right:
                {
                    LeftStoryboard?.Begin();
                    break;
                }
            }
        }
    }
}