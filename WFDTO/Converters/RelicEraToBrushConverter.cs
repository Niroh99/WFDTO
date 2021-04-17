using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WFDTO.Converters
{
    public class RelicEraToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string valueString)
            {
                if (valueString?.ToLower() == "axi") return Brushes.Gold;
                else if (valueString?.ToLower() == "neo") return Brushes.Silver;
                else if (valueString?.ToLower() == "meso") return Brushes.DimGray;
                else if (valueString?.ToLower() == "lith") return Brushes.SaddleBrown;
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
