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
                switch (status.ToLower().Trim())
                {
                    case "victoria":
                    case "win":
                        return Lang.GlobalGameVictory;

                    case "derrota":
                    case "defeat":
                    case "loss":
                        return Lang.GlobalGameDefeat;

                    case "empate":
                    case "draw":
                        return Lang.GlobalGameDraw;

                    default:
                        return status;
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
