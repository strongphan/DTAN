using System.Globalization;

namespace PetAdoption.Mobile.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && parameter is string parameterString)
            {
                int parameterValue = int.Parse(parameterString);
                return intValue == parameterValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue && parameter is string parameterString)
            {
                return int.Parse(parameterString);
            }
            return Binding.DoNothing;
        }
    }
}