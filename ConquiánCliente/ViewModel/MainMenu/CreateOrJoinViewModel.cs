using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages; 

namespace ConquiánCliente.ViewModel.MainMenu
{
    public class CreateOrJoinViewModel : ViewModelBase
    {
        private string roomCode;
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

            CreateRoomCommand = new RelayCommand(async (p) => await ExecuteCreateRoom(p));
            JoinRoomCommand = new RelayCommand(async (p) => await ExecuteJoinRoom(p));
            CloseCommand = new RelayCommand(ExecuteClose);
        }

        private async Task ExecuteCreateRoom(object parameter)
        {
            if (parameter is Window window)
            {
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
                catch (FaultException<ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);

                    MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    if (client.State == CommunicationState.Opened) client.Close();
                    else client.Abort();
                }
            }
        }

        private async Task ExecuteJoinRoom(object parameter)
        {
            if (string.IsNullOrWhiteSpace(RoomCode))
            {
                MessageBox.Show(Lang.ErrorEmptyRoomCode, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (parameter is Window window)
            {
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
                catch (FaultException<ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);

                    MessageBox.Show(msg, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (CommunicationException)
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show(Lang.ErrorJoinLobby, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (client.State == CommunicationState.Opened) client.Close();
                    else client.Abort();
                }
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