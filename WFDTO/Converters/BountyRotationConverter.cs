using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WFDTO.Converters
{
    public class BountyRotationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 4 && values[0] is SearchResultModels.Bounty bounty && values[1] is bool rotationARadioButtonisChecked && values[2] is bool rotationBRadioButtonisChecked && values[3] is bool rotationCRadioButtonisChecked)
            {
                if (rotationARadioButtonisChecked) return bounty.RotationA;
                else if (rotationBRadioButtonisChecked) return bounty.RotationB;
                else if (rotationCRadioButtonisChecked) return bounty.RotationC;
                return null;
            }
            else return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
