﻿using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WFDTO.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (boolValue) return System.Windows.Visibility.Visible;
                else return System.Windows.Visibility.Collapsed;
            }
            else return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
