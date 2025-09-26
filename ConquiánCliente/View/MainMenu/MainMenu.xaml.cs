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
using ConquiánCliente.View.Lobby;

namespace ConquiánCliente.View.MainMenu
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void ClickPlay(object sender, RoutedEventArgs e)
        {
            LobbyGame lobby = new LobbyGame();
            lobby.Show();
            this.Close();
        }

        private void ClickChangeLanguage(object sender, RoutedEventArgs e)
        {
            var selector = new ChangeLanguage();
            selector.Owner = this;
            selector.ShowDialog();
        }
    }
}
