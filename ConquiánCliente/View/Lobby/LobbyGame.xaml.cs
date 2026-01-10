using System.Windows;
using ConquiánCliente.ViewModel;
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

            if (PlayerSession.IsNetworkDown)
            {
                return;
            }

            if (DataContext is LobbyGameViewModel vm)
            {
                if (vm.IsNavigatingAway) return;
                vm.ShutdownApplicationCommand.Execute(this);
                e.Cancel = true;
            }
        }
    }
}