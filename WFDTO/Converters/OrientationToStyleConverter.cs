using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WFDTO.Converters
{
    public class OrientationToStyleConverter : IValueConverter
    {
        public Style VerticalStyle { get; set; }

        public Style HorizontalStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Controls.Orientation orientationValue)
            {
                if (orientationValue == System.Windows.Controls.Orientation.Horizontal) return HorizontalStyle;
                else return VerticalStyle;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}