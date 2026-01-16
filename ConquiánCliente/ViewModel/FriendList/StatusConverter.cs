using System;
using System.Globalization;
using System.Windows.Data;
using ConquiánCliente.ServiceFriendList;

namespace ConquiánCliente.ViewModel.FriendList
{
    public class StatusConverter : IValueConverter
    {
        private const string STATUS_ONLINE_TEXT = "Online";
        private const string STATUS_OFFLINE_TEXT = "Offline";
        private const string STATUS_UNKNOWN_TEXT = "Desconocido";
        private const int INT_STATUS_ONLINE = 1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string statusResult = STATUS_UNKNOWN_TEXT;

            if (value is PlayerStatus status)
            {
                statusResult = ConvertFromEnum(status);
            }
            else if (value is int intStatus)
            {
                statusResult = ConvertFromInt(intStatus);
            }

            return statusResult;
        }

        private string ConvertFromEnum(PlayerStatus status)
        {
            string result;

            switch (status)
            {
                case PlayerStatus.Online:
                    result = STATUS_ONLINE_TEXT;
                    break;
                case PlayerStatus.Offline:
                    result = STATUS_OFFLINE_TEXT;
                    break;
                default:
                    result = STATUS_UNKNOWN_TEXT;
                    break;
            }

            return result;
        }

        private string ConvertFromInt(int status)
        {
            string result;

            if (status == INT_STATUS_ONLINE)
            {
                result = STATUS_ONLINE_TEXT;
            }
            else
            {
                result = STATUS_OFFLINE_TEXT;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}