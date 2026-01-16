using ConquiánCliente.Properties.Langs;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ServiceGuestInvitation;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class SendRoomCodeViewModel : ViewModelBase
    {
        private readonly string roomCode;
        private string email;
        private bool isLoading;
        private readonly IMessageResolver messageResolver;

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(IsControlEnabled));
            }
        }

        public bool IsControlEnabled => !IsLoading;

        public ICommand SendCommand { get; }
        public ICommand BackCommand { get; }

        public SendRoomCodeViewModel(string roomCode)
        {
            this.roomCode = roomCode;
            this.messageResolver = new ResourceMessageResolver();

            SendCommand = new RelayCommand(async (param) => await ExecuteSend(param));
            BackCommand = new RelayCommand(ExecuteBack);
        }

        private async Task ExecuteSend(object parameter)
        {
            bool isInputValid = ValidateInput();
            if (!isInputValid)
            {
                return;
            }

            Email = Email.Trim();
            IsLoading = true;

            await TrySendGuestInvitation();

            IsLoading = false;
        }

        private bool ValidateInput()
        {
            bool isValid = false;
            string emailError = SignUpValidator.ValidateEmail(Email);

            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleValidation, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        private async Task TrySendGuestInvitation()
        {
            try
            {
                await PerformInvitationRequest();
                HandleSuccess();
            }
            catch (FaultException<ServiceFaultDto> fault)
            {
                HandleServiceFault(fault);
            }
            catch (EndpointNotFoundException)
            {
                ShowConnectionError(Lang.ErrorServerUnavailable);
            }
            catch (TimeoutException)
            {
                ShowConnectionError(Lang.ErrorConnectingToServer);
            }
            catch (CommunicationException)
            {
                ShowConnectionError(Lang.ErrorConnectingToServer);
            }
        }

        private async Task PerformInvitationRequest()
        {
            using (var client = new GuestInvitationClient())
            {
                await client.SendGuestInviteAsync(roomCode, Email);
            }
        }

        private void HandleSuccess()
        {
            MessageBox.Show(Lang.LobbyGuestInviteSent, Lang.Lobby, MessageBoxButton.OK, MessageBoxImage.Information);
            CloseCurrentWindow();
        }

        private void HandleServiceFault(FaultException<ServiceFaultDto> fault)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);

            MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowConnectionError(string message)
        {
            MessageBox.Show(message, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseCurrentWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<SendRoomCode>()
                                .FirstOrDefault(w => w.DataContext == this);

            if (currentWindow != null)
            {
                ExecuteBack(currentWindow);
            }
        }

        private static void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }
}