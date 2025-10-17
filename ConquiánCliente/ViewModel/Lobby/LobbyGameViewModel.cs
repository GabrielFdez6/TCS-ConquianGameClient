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
            pollingTimer.Interval = TimeSpan.FromSeconds(2); // Reducido para un chat más fluido
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
                var lobbyStateTask = client.GetLobbyStateAsync(this.RoomCode);
                var chatMessagesTask = client.GetChatMessagesAsync(this.RoomCode);

                await Task.WhenAll(lobbyStateTask, chatMessagesTask);

                var lobbyState = await lobbyStateTask;
                var chatMessages = await chatMessagesTask;

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

                        foreach (Window window in Application.Current.Windows)
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
                            playerItem.DisplayName = $"Host: {playerDto.nickname}";
                        }
                        Players.Add(playerItem);
                    }

                    if (chatMessages != null)
                    {
                        ChatMessages.Clear();
                        foreach (var message in chatMessages)
                        {
                            ChatMessages.Add($"{message.Nickname}: {message.Message}");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                pollingTimer.Stop();
            }
            finally
            {
                if (client.State == System.ServiceModel.CommunicationState.Opened) client.Close();
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
                catch (Exception ex)
                {
                    // Manejar error de envío
                }
                finally
                {
                    if (client.State == System.ServiceModel.CommunicationState.Opened) client.Close();
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
                }
                finally
                {
                    if (client.State == System.ServiceModel.CommunicationState.Opened) client.Close();
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
            currentGameIndex++;
            if (currentGameIndex >= gameTypes.Length) { currentGameIndex = 0; }
            SelectedGameType = gameTypes[currentGameIndex];
        }

        private void ExecutePreviousGameType(object obj)
        {
            currentGameIndex--;
            if (currentGameIndex < 0) { currentGameIndex = gameTypes.Length - 1; }
            SelectedGameType = gameTypes[currentGameIndex];
        }
    }
}