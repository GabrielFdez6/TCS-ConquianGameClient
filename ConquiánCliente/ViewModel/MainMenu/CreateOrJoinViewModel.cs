using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages;

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
            if (isLoading) return;

            if (parameter is Window window)
            {
                isLoading = true;
                CommandManager.InvalidateRequerySuggested();

                var client = new LobbyClient(new InstanceContext(LobbyCallbackHandler.Instance));
                try
                {
                    CreatedRoomCode = await client.CreateLobbyAsync(PlayerSession.CurrentPlayer.idPlayer);

                    if (!string.IsNullOrEmpty(CreatedRoomCode))
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        MessageBox.Show(Lang.ErrorLobbyCreation, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    HandleServiceException(ex);
                }
                finally
                {
                    if (client.State == CommunicationState.Opened) client.Close();
                    else client.Abort();

                    isLoading = false;
                    CommandManager.InvalidateRequerySuggested();
                }
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

        private async void ExecuteJoinRoom(object parameter)
        {
            if (string.IsNullOrWhiteSpace(RoomCode))
            {
                MessageBox.Show(Lang.ErrorEmptyRoomCode, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isLoading) return;

            if (parameter is Window window)
            {
                isLoading = true;
                CommandManager.InvalidateRequerySuggested();

                var context = new InstanceContext(LobbyCallbackHandler.Instance);
                var client = new LobbyClient(context);
                try
                {
                    var lobbyState = await client.GetLobbyStateAsync(RoomCode.ToUpper());

                    if (lobbyState != null)
                    {
                        CreatedRoomCode = RoomCode.ToUpper();
                        window.DialogResult = true;
                        window.Close();
                    }
                }
                catch (Exception ex)
                {
                    HandleJoinRoomException(ex);
                }
                finally
                {
                    if (client.State == CommunicationState.Opened) client.Close();
                    else client.Abort();

                    isLoading = false;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private void HandleJoinRoomException(Exception ex)
        {
            if (ex is FaultException<ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show(Lang.ErrorJoinLobby, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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