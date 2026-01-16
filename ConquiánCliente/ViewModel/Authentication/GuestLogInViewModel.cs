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
using ConquiánCliente.Utilities.Messages;

namespace ConquiánCliente.ViewModel.Authentication
{
    public class GuestLogInViewModel : ViewModelBase
    {
        private string email;
        private string roomCode;
        private readonly LobbyClient lobbyClient;
        private readonly Window currentWindow;
        private bool isLoading;
        private readonly IMessageResolver messageResolver;

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
            this.messageResolver = new ResourceMessageResolver();

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
            string validationMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(code))
            {
                validationMessage = Lang.ErrorRoomCodeEmpty;
            }
            else if (code.Length != 5)
            {
                validationMessage = Lang.ErrorRoomCodeLength;
            }

            return validationMessage;
        }

        private async Task ExecuteGuestLogin()
        {
            if (!ValidateInput())
            {
                return;
            }

            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            try
            {
                await ProcessLogin();
            }
            catch (Exception ex)
            {
                HandleLoginException(ex);
            }
            finally
            {
                isLoading = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool ValidateInput()
        {
            bool isValid = false;
            string emailError = LogInValidator.ValidateEmail(Email);

            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string roomCodeError = ValidateRoomCode(RoomCode);
                if (!string.IsNullOrEmpty(roomCodeError))
                {
                    MessageBox.Show(roomCodeError, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private async Task ProcessLogin()
        {
            PlayerDto guestPlayer = await lobbyClient.JoinAndSubscribeAsGuestAsync(Email, RoomCode);

            if (guestPlayer != null)
            {
                NavigateToLobby(guestPlayer);
            }
            else
            {
                MessageBox.Show(Lang.ErrorGuestInviteMismatch, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NavigateToLobby(PlayerDto guestPlayer)
        {
            PlayerSession.StartGuestSession(guestPlayer);
            var lobbyView = new LobbyGame(RoomCode);
            lobbyView.Show();
            currentWindow.Close();
        }

        private void HandleLoginException(Exception ex)
        {
            if (ex is FaultException<ServiceLobby.ServiceFaultDto> fault)
            {
                HandleFaultException(fault);
            }
            else if (IsCommunicationException(ex))
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsCommunicationException(Exception ex)
        {
            bool isCommError = false;
            if (ex is EndpointNotFoundException || ex is CommunicationException || ex is TimeoutException)
            {
                isCommError = true;
            }
            return isCommError;
        }

        private void HandleFaultException(FaultException<ServiceLobby.ServiceFaultDto> fault)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string message = messageResolver.GetMessage(errorType);

            if (errorType == ConquiánCliente.ServiceLogin.ServiceErrorType.RegisteredUserAsGuest)
            {
                MessageBox.Show(message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
                ExecuteNavigateBack(null);
            }
            else
            {
                MessageBox.Show(message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteNavigateBack(object parameter)
        {
            var loginView = new LogIn();
            loginView.Show();

            CloseCurrentWindow(parameter);
        }

        private void CloseCurrentWindow(object parameter)
        {
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