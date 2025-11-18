using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.MainMenu
{
    public class CreateOrJoinViewModel : ViewModelBase
    {
        private string roomCode;
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
                        MessageBox.Show(Lang.ErrorLobbyCreation, Lang.TitleError);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
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
                MessageBox.Show(Lang.ErrorEmptyRoomCode, Lang.TitleError);
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
                    else
                    {
                        MessageBox.Show(Lang.ErrorJoinLobby, Lang.TitleError);
                    }
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
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
