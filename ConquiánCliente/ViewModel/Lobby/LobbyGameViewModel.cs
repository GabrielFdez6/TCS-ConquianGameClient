using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.MainMenu;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class LobbyGameViewModel : ViewModelBase
    {
        private readonly string[] gameTypes = { Lang.LobbyQuickGame, Lang.LobbyClassicGame };
        private int currentGameIndex = 0;
        private string selectedGameType;
        private string playerCountText;
        private string roomCode;
        private string currentMessage;
        private DispatcherTimer pollingTimer;
        public bool isNavigatingAway = false;

        public ObservableCollection<PlayerLobbyItemViewModel> Players { get; }
        public ObservableCollection<string> ChatMessages { get; }

        public string RoomCode
        {
            get { return roomCode; }
            set { roomCode = value; OnPropertyChanged(nameof(RoomCode)); }
        }
        public string SelectedGameType
        {
            get { return selectedGameType; }
            set { selectedGameType = value; OnPropertyChanged(nameof(SelectedGameType)); }
        }
        public string PlayerCountText
        {
            get { return playerCountText; }
            set { playerCountText = value; OnPropertyChanged(nameof(PlayerCountText)); }
        }

        public string CurrentMessage
        {
            get { return currentMessage; }
            set { currentMessage = value; OnPropertyChanged(nameof(CurrentMessage)); }
        }

        public ICommand NextGameTypeCommand { get; }
        public ICommand PreviousGameTypeCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand SendMessageCommand { get; }

        public LobbyGameViewModel(string receivedRoomCode)
        {
            Players = new ObservableCollection<PlayerLobbyItemViewModel>();
            ChatMessages = new ObservableCollection<string>();
            this.RoomCode = receivedRoomCode;
            SelectedGameType = gameTypes[currentGameIndex];

            NextGameTypeCommand = new RelayCommand(ExecuteNextGameType);
            PreviousGameTypeCommand = new RelayCommand(ExecutePreviousGameType);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage, CanExecuteSendMessage);

            InitializePolling();
        }

        private void InitializePolling()
        {
            pollingTimer = new DispatcherTimer();
            pollingTimer.Interval = TimeSpan.FromSeconds(3);
            pollingTimer.Tick += async (sender, e) => await PollLobbyState();
            pollingTimer.Start();
            Task.Run(async () => await PollLobbyState());
        }

        private async Task PollLobbyState()
        {
            if (isNavigatingAway) return;

            var client = new LobbyClient();
            try
            {
                var lobbyState = await client.GetLobbyStateAsync(this.RoomCode);

                if (lobbyState == null || lobbyState.StatusLobby == "Finalizada")
                {
                    pollingTimer.Stop();
                    if (isNavigatingAway) return;
                    isNavigatingAway = true;

                    MessageBox.Show(Lang.InfoHostLeft, Lang.Lobby, MessageBoxButton.OK, MessageBoxImage.Information);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var mainMenu = new View.MainMenu.MainMenu();
                        mainMenu.Show();

                        foreach (Window window in Application.Current.Windows.OfType<Window>().ToList())
                        {
                            if (window.DataContext == this)
                            {
                                window.Close();
                                break;
                            }
                        }
                    });
                    return;
                }

                int maxPlayers = 2;
                PlayerCountText = $"{lobbyState.Players.Length}/{maxPlayers}";

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Players.Clear();
                    foreach (var playerDto in lobbyState.Players)
                    {
                        var playerItem = new PlayerLobbyItemViewModel
                        {
                            ProfileImagePath = playerDto.pathPhoto,
                            DisplayName = playerDto.nickname
                        };

                        if (playerDto.idPlayer == lobbyState.idHostPlayer)
                        {
                            playerItem.DisplayName = $"{Lang.LobbyHostPrefix} {playerDto.nickname}";
                        }
                        Players.Add(playerItem);
                    }

                    if (lobbyState.ChatMessages != null)
                    {
                        ChatMessages.Clear();
                        foreach (var message in lobbyState.ChatMessages)
                        {
                            ChatMessages.Add($"{message.Nickname}: {message.Message}");
                        }
                    }
                });
            }
            catch (Exception)
            {
                pollingTimer.Stop();
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (client.State == System.ServiceModel.CommunicationState.Opened)
                {
                    client.Close();
                }
            }
        }

        private bool CanExecuteSendMessage(object obj)
        {
            return !string.IsNullOrWhiteSpace(CurrentMessage);
        }

        private void ExecuteSendMessage(object obj)
        {
            var messageDto = new MessageDto
            {
                Nickname = PlayerSession.CurrentPlayer.nickname,
                Message = this.CurrentMessage,
                Timestamp = DateTime.UtcNow
            };

            Task.Run(async () =>
            {
                var client = new LobbyClient();
                try
                {
                    await client.SendMessageAsync(this.RoomCode, messageDto);
                }
                catch (Exception)
                {
                    MessageBox.Show(Lang.ErrorSendMessageFailed, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (client.State == System.ServiceModel.CommunicationState.Opened)
                    {
                        client.Close();
                    }
                }
            });

            CurrentMessage = string.Empty;
        }

        private void ExecuteGoBack(object parameter)
        {
            if (isNavigatingAway) return;
            isNavigatingAway = true;

            pollingTimer.Stop();

            Task.Run(async () =>
            {
                var client = new LobbyClient();
                try
                {
                    await client.LeaveLobbyAsync(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer);
                }
                catch
                {
                    // Se ignora el error intencionalmente, ya que el usuario está saliendo de la pantalla de todas formas.
                }
                finally
                {
                    if (client.State == System.ServiceModel.CommunicationState.Opened)
                    {
                        client.Close();
                    }
                }
            });

            var mainMenu = new View.MainMenu.MainMenu();
            mainMenu.Show();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteNextGameType(object obj)
        {
            currentGameIndex = (currentGameIndex + 1) % gameTypes.Length;
            SelectedGameType = gameTypes[currentGameIndex];
        }

        private void ExecutePreviousGameType(object obj)
        {
            currentGameIndex = (currentGameIndex - 1 + gameTypes.Length) % gameTypes.Length;
            SelectedGameType = gameTypes[currentGameIndex];
        }
    }
}