using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WFDTO.Converters
{
    public class RelicDropChanceToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double valueString)
            {
                if (valueString == 2) return Brushes.Gold;
                else if (valueString == 11) return Brushes.Silver;
                else if (valueString == 25.33) return Brushes.SaddleBrown;
                else return null;
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
