using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConquiánCliente.View.MainMenu
{
    /// <summary>
    /// Lógica de interacción para ChangeLanguage.xaml
    /// </summary>
    public partial class ChangeLanguage : Window
    {
        public ChangeLanguage()
        {
            InitializeComponent();
        }
        private void ClickSpanish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "es-MX";
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }

        private void ClickEnglish(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }
    }
}
