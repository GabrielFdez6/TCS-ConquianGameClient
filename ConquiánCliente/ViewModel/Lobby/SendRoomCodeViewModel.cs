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

            SendCommand = new RelayCommand(async (param) => await ExecuteSend());
            BackCommand = new RelayCommand(ExecuteBack);
        }

        private async Task ExecuteSend()
        {
            string emailError = SignUpValidator.ValidateEmail(Email);

            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleValidation, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Email = Email.Trim();
            IsLoading = true;

            try
            {
                using (var client = new GuestInvitationClient())
                {
                    await client.SendGuestInviteAsync(roomCode, Email);
                }

                MessageBox.Show(Lang.LobbyGuestInviteSent, Lang.Lobby, MessageBoxButton.OK, MessageBoxImage.Information);
                ExecuteBack(Application.Current.Windows.OfType<SendRoomCode>().FirstOrDefault(w => w.DataContext == this));
            }
            catch (FaultException<ServiceGuestInvitation.ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);

                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
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