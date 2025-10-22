using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class InvitationReceivedViewModel : ViewModelBase
    {
        private string roomCode;
        public string InvitationText { get; }
        public ICommand AcceptCommand { get; }
        public ICommand RejectCommand { get; }

        public InvitationReceivedViewModel(string senderNickname, string roomCode)
        {
            this.roomCode = roomCode;
            this.InvitationText = $"{senderNickname} {Lang.LobbyInvitedYou}"; 
            AcceptCommand = new RelayCommand(ExecuteAccept);
            RejectCommand = new RelayCommand(ExecuteReject);
        }

        private async void ExecuteAccept(object parameter)
        {
            var window = parameter as Window;
            LobbyDto lobbyState = null;

            try
            {
                using (var lobbyClient = new LobbyClient(new InstanceContext(new LobbyCallbackHandler())))
                {
                    lobbyState = await lobbyClient.GetLobbyStateAsync(this.roomCode);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                window?.Close();
                return;
            }

            if (lobbyState == null)
            {
                MessageBox.Show(Lang.InfoHostLeft, Lang.Lobby); 
                window?.Close();
                return;
            }

            if (lobbyState.Players.Length >= 2)
            {
                MessageBox.Show(Lang.LobbyFull, Lang.Lobby); 
                window?.Close();
                return;
            }

 
            try
            {
                var lobbyGame = new LobbyGame(this.roomCode);
                lobbyGame.Show();

                foreach (Window openWindow in Application.Current.Windows.OfType<Window>().ToList())
                {
                    if (openWindow != lobbyGame)
                    {
                        openWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}", Lang.TitleError);
            }
        }

        private void ExecuteReject(object parameter)
        {
            (parameter as Window)?.Close();
        }
    }
}