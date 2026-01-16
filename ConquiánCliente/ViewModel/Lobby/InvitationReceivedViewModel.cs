using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class InvitationReceivedViewModel : ViewModelBase
    {
        private const int MAX_LOBBY_CAPACITY = 2;
        private readonly string roomCode;
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
            Window currentInvitationWindow = parameter as Window;

            CloseActiveLobbySession();

            LobbyDto lobbyState = await FetchLobbyStateAsync(currentInvitationWindow);
            bool canJoin = ValidateLobbyAvailability(lobbyState, currentInvitationWindow);

            if (canJoin)
            {
                PerformLobbyTransition();
            }
        }

        private void CloseActiveLobbySession()
        {
            LobbyGame openLobbyWindow = Application.Current.Windows.OfType<LobbyGame>().FirstOrDefault();

            if (openLobbyWindow != null)
            {
                if (openLobbyWindow.DataContext is LobbyGameViewModel lobbyVM)
                {
                    lobbyVM.IsNavigatingAway = true;
                    lobbyVM.CloseClientConnection(true);
                }

                openLobbyWindow.Close();
            }
        }

        private async Task<LobbyDto> FetchLobbyStateAsync(Window window)
        {
            LobbyDto fetchedState = null;

            try
            {
                using (var lobbyClient = new LobbyClient(new InstanceContext(LobbyCallbackHandler.Instance)))
                {
                    fetchedState = await lobbyClient.GetLobbyStateAsync(this.roomCode);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
                CloseWindow(window);
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                CloseWindow(window);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                CloseWindow(window);
            }

            return fetchedState;
        }

        private bool ValidateLobbyAvailability(LobbyDto lobbyState, Window window)
        {
            bool isValid = true;

            if (lobbyState == null)
            {
                MessageBox.Show(Lang.InfoHostLeft, Lang.Lobby);
                CloseWindow(window);
                isValid = false;
            }
            else if (lobbyState.Players.Length >= MAX_LOBBY_CAPACITY)
            {
                MessageBox.Show(Lang.LobbyFull, Lang.Lobby);
                CloseWindow(window);
                isValid = false;
            }

            return isValid;
        }

        private void PerformLobbyTransition()
        {
            try
            {
                LobbyGame newLobbyWindow = new LobbyGame(this.roomCode);
                newLobbyWindow.Show();

                CloseAllOtherWindows(newLobbyWindow);
            }
            catch (CommunicationException)
            {
                ShowConnectionErrorMessage();
            }
            catch (TimeoutException)
            {
                ShowConnectionErrorMessage();
            }
            catch (Exception)
            {
                ShowConnectionErrorMessage();
            }
        }

        private void ShowConnectionErrorMessage()
        {
            string errorMessage = Lang.ErrorConnectingToServer;
            string errorTitle = Lang.TitleError;

            MessageBox.Show(errorMessage, errorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseAllOtherWindows(Window windowToKeep)
        {
            List<Window> windowsToClose = Application.Current.Windows.OfType<Window>()
                .Where(w => w != windowToKeep)
                .ToList();

            foreach (Window openWindow in windowsToClose)
            {
                openWindow.Close();
            }
        }

        private void ExecuteReject(object parameter)
        {
            Window window = parameter as Window;
            CloseWindow(window);
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}