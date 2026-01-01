using System;
using System.Globalization;
using System.Windows.Data;
using ConquiánCliente.ServiceFriendList;

namespace ConquiánCliente.ViewModel.FriendList
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayerStatus status)
            {
                switch (status)
                {
                    case PlayerStatus.Online:
                        return "Online";
                    case PlayerStatus.Offline:
                        return "Offline";
                    default:
                        return "Desconocido";
                }
            }

            if (value is int intStatus)
            {
                return intStatus == 1 ? "Online" : "Offline";
            }

            return "Desconocido";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}