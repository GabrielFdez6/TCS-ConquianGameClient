using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.MainMenu
{
    public class CreateOrJoinViewModel : ViewModelBase
    {
        private string roomCode;
        private bool isLoading;
        private readonly IMessageResolver messageResolver;

        public string RoomCode
        {
            get { return roomCode; }
            set
            {
                roomCode = value;
                OnPropertyChanged(nameof(RoomCode));
            }
        }
        public string CreatedRoomCode { get; private set; }

        public ICommand CreateRoomCommand { get; }
        public ICommand JoinRoomCommand { get; }
        public ICommand CloseCommand { get; }

        public CreateOrJoinViewModel()
        {
            this.messageResolver = new ResourceMessageResolver();
            isLoading = false;

            CreateRoomCommand = new RelayCommand(ExecuteCreateRoom, CanExecuteSubmit);
            JoinRoomCommand = new RelayCommand(ExecuteJoinRoom, CanExecuteSubmit);
            CloseCommand = new RelayCommand(ExecuteClose);
        }

        private bool CanExecuteSubmit(object parameter)
        {
            return !isLoading;
        }

        private async void ExecuteCreateRoom(object parameter)
        {
            if (isLoading || !(parameter is Window window))
            {
                return;
            }

            SetLoadingState(true);
            LobbyClient client = new LobbyClient(new InstanceContext(LobbyCallbackHandler.Instance));

            try
            {
                await AttemptCreateLobby(client, window);
            }
            catch (FaultException<ServiceFaultDto> ex)
            {
                HandleServiceFault(ex);
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
            finally
            {
                SafeCloseClient(client);
                SetLoadingState(false);
            }
        }



        private void HandleServiceException(Exception ex)
        {
            if (ex is FaultException<ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ex is EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (ex is CommunicationException)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AttemptCreateLobby(LobbyClient client, Window window)
        {
            CreatedRoomCode = await client.CreateLobbyAsync(PlayerSession.CurrentPlayer.idPlayer);

            if (!string.IsNullOrEmpty(CreatedRoomCode))
            {
                CloseWindowWithResult(window, true);
            }
            else
            {
                MessageBox.Show(Lang.ErrorLobbyCreation, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExecuteJoinRoom(object parameter)
        {
            if (string.IsNullOrWhiteSpace(RoomCode))
            {
                MessageBox.Show(Lang.ErrorEmptyRoomCode, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isLoading || !(parameter is Window window))
            {
                return;
            }

            SetLoadingState(true);
            LobbyClient client = new LobbyClient(new InstanceContext(LobbyCallbackHandler.Instance));

            try
            {
                await AttemptJoinLobby(client, window);
            }
            catch (FaultException<ServiceFaultDto> ex)
            {
                HandleServiceFault(ex, isInfo: true);
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
            finally
            {
                SafeCloseClient(client);
                SetLoadingState(false);
            }
        }

        private async Task AttemptJoinLobby(LobbyClient client, Window window)
        {
            var lobbyState = await client.GetLobbyStateAsync(RoomCode.ToUpper());

            if (lobbyState != null)
            {
                CreatedRoomCode = RoomCode.ToUpper();
                CloseWindowWithResult(window, true);
            }
        }

        private void SetLoadingState(bool loading)
        {
            isLoading = loading;
            CommandManager.InvalidateRequerySuggested();
        }

        private void SafeCloseClient(LobbyClient client)
        {
            if (client == null)
            {
                return;
            }

            try
            {
                if (client.State == CommunicationState.Opened)
                {
                    client.Close();
                }
                else
                {
                    client.Abort();
                }
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
            }
        }

        private void CloseWindowWithResult(Window window, bool result)
        {
            window.DialogResult = result;
            window.Close();
        }

        private void HandleServiceFault(FaultException<ServiceFaultDto> fault, bool isInfo = false)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);

            MessageBoxImage icon = isInfo ? MessageBoxImage.Information : MessageBoxImage.Warning;
            string title = isInfo ? Lang.TitleInfo : Lang.TitleError;

            MessageBox.Show(msg, title, MessageBoxButton.OK, icon);
        }

        private void ShowConnectionError(string message)
        {
            MessageBox.Show(message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void ExecuteClose(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }
}