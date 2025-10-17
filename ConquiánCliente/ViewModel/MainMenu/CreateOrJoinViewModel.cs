using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var client = new LobbyClient();
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
                catch (Exception ex)
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                }
                finally
                {
                    client.Close();
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
                var client = new LobbyClient();
                try
                {
                    bool joinedSuccessfully = await client.JoinLobbyAsync(RoomCode.ToUpper(), PlayerSession.CurrentPlayer.idPlayer);

                    if (joinedSuccessfully)
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
                catch (EndpointNotFoundException ex)
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError);
                }
                finally
                {
                    client.Close();
                }
            }
        }

        private void ExecuteClose(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }
}
