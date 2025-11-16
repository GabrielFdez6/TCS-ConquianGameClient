using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.ViewModel.Validation;

namespace ConquiánCliente.ViewModel.Authentication
{
    public class GuestLogInViewModel : ViewModelBase
    {
        private string email;
        private string roomCode;
        private readonly LobbyClient lobbyClient;
        private readonly Window currentWindow;
        private bool isLoading;

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
            return !isLoading;
        }

        private static string ValidateRoomCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Lang.ErrorRoomCodeEmpty;
            }
            if (code.Length != 5)
            {
                return Lang.ErrorRoomCodeLength;
            }
            return string.Empty; 
        }

        private async Task ExecuteGuestLogin()
        {

            string emailError = LogInValidator.ValidateEmail(Email);
            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            string roomCodeError = ValidateRoomCode(RoomCode);
            if (!string.IsNullOrEmpty(roomCodeError))
            {
                MessageBox.Show(roomCodeError, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            try
            {
                PlayerDto guestPlayer = await lobbyClient.JoinAndSubscribeAsGuestAsync(Email, RoomCode);

                if (guestPlayer != null)
                {
                    PlayerSession.StartGuestSession(guestPlayer);
                    var lobbyView = new LobbyGame(RoomCode);
                    lobbyView.Show();
                    currentWindow.Close();
                }
                else
                {
                    MessageBox.Show(Lang.ErrorGuestInviteMismatch, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (FaultException<ServiceLobby.GuestInviteUsedFault> ex)
            {
                MessageBox.Show(ex.Detail.Message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FaultException<ServiceLobby.RegisteredUserAsGuestFault> ex)
            {
                MessageBox.Show(ex.Detail.Message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);

                ExecuteNavigateBack(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}", Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            finally
            {
                isLoading = false;
                CommandManager.InvalidateRequerySuggested();
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