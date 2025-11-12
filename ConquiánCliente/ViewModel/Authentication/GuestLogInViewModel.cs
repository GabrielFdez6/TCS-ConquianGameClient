using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Authentication
{
    public class GuestLogInViewModel : ViewModelBase
    {
        private string email;
        private string roomCode;
        private LobbyClient lobbyClient;
        private Window currentWindow;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string RoomCode
        {
            get { return roomCode; }
            set
            {
                roomCode = value;
                OnPropertyChanged(nameof(RoomCode));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand GuestLoginCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public GuestLogInViewModel(Window window)
        {
            this.currentWindow = window;

            var context = new InstanceContext(LobbyCallbackHandler.Instance);
            lobbyClient = new LobbyClient(context);

            GuestLoginCommand = new RelayCommand(async (param) => await ExecuteGuestLogin(), (param) => CanExecuteGuestLogin());
            NavigateBackCommand = new RelayCommand(ExecuteNavigateBack);
        }

        private bool CanExecuteGuestLogin()
        {
            return !string.IsNullOrWhiteSpace(RoomCode) && RoomCode.Length == 5;
        }

        private async Task ExecuteGuestLogin()
        {
            try
            {
                PlayerDto guestPlayer = await lobbyClient.JoinAndSubscribeAsGuestAsync(RoomCode);

                if (guestPlayer != null)
                {
                    PlayerSession.StartGuestSession(guestPlayer);

                    var lobbyView = new LobbyGame(RoomCode);
                    lobbyView.Show();

                    currentWindow.Close();
                }
                else
                {
                    MessageBox.Show(
                        Lang.ErrorJoinLobby,
                        Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}", Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteNavigateBack(object parameter)
        {
            var loginView = new LogIn();
            loginView.Show();

            if (parameter is Window window)
            {
                window.Close();
            }
            else
            {
                currentWindow.Close();
            }
        }
    }
}
