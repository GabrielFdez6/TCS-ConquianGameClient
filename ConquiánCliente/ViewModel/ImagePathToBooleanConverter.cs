using System;
using System.Globalization;
using System.Windows.Data;

namespace ConquiánCliente.ViewModel
{
    public class ImagePathToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is string) || !(values[1] is string))
            {
                return true;
            }

            string imagePath = values[0] as string;
            string currentImagePath = values[1] as string;

            return !string.Equals(imagePath, currentImagePath, StringComparison.OrdinalIgnoreCase);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}