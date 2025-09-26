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
using ConquiánCliente.Properties.Langs;

namespace ConquiánCliente.View.Lobby
{
    /// <summary>
    /// Lógica de interacción para LobbyGame.xaml
    /// </summary>
    public partial class LobbyGame : Window
    {
        private readonly string[] gameTypes = { Lang.LobbyQuickGame, Lang.LobbyClassicGame };
        private int currentGameIndex = 0;
        public LobbyGame()
        {
            InitializeComponent();
            UpdateGameLabel();
        }

        private void UpdateGameLabel()
        {
            txtBxTypeGame.Text = gameTypes[currentGameIndex];
        }

        private void ClickLeftChangeGame(object sender, RoutedEventArgs e)
        {
            currentGameIndex--;
            if (currentGameIndex < 0)
                currentGameIndex = gameTypes.Length - 1;
            UpdateGameLabel();
        }

        private void ClickRighChangeGame(object sender, RoutedEventArgs e)
        {
            currentGameIndex++;
            if (currentGameIndex >= gameTypes.Length)
                currentGameIndex = 0;
            UpdateGameLabel();
        }
    }
}
