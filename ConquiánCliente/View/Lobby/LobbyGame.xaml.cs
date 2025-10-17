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
using ConquiánCliente.ViewModel.Lobby;

namespace ConquiánCliente.View.Lobby
{
    /// <summary>
    /// Lógica de interacción para LobbyGame.xaml
    /// </summary>
    public partial class LobbyGame : Window
    {
        public LobbyGame(string roomCode)
        {
            InitializeComponent();
            DataContext = new LobbyGameViewModel(roomCode);
            this.Closing += LobbyGame_Closing;
        }

        private void LobbyGame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is LobbyGameViewModel vm)
            {
                if (vm.isNavigatingAway) return;
                vm.GoBackCommand.Execute(this);
            }
        }
    }
}
