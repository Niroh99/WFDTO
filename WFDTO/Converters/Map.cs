using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WFDTO.Converters
{
    public class Map : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(Map));
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty DisplayProperty = DependencyProperty.Register("Display", typeof(object), typeof(Map));
        public object Display
        {
            get { return GetValue(DisplayProperty); }
            set { SetValue(DisplayProperty, value); }
        }
    }
}