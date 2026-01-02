using ConquiánCliente.Properties.Langs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConquiánCliente.ViewModel.Profile
{
    public class ResultStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {

                if (string.Equals(status, "Victory", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "Won", StringComparison.OrdinalIgnoreCase))
                {
                    return Lang.GlobalGameVictory; 
                }

                if (string.Equals(status, "Defeat", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "Lost", StringComparison.OrdinalIgnoreCase))
                {
                    return Lang.GlobalGameDefeat; 
                }

                if (string.Equals(status, "Draw", StringComparison.OrdinalIgnoreCase))
                {
                    return Lang.GlobalGameDraw; 
                }
            }

            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
