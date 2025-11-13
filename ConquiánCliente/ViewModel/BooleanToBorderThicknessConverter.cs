using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConquiánCliente.ViewModel 
{
    public class BooleanToBorderThicknessConverter : IValueConverter
    {
        public double Thickness { get; set; } = 3.0; 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return new Thickness(Thickness);
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}