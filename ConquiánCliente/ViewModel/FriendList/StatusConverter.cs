using System;
using System.Globalization;
using System.Windows.Data;

namespace ConquiánCliente.ViewModel.FriendList
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                switch (status)
                {
                    case 1:
                        return "Online";
                    case 2:
                        return "Offline";
                    default:
                        return "Desconocido";
                }
            }
            return "Desconocido";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}