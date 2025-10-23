using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.View.Lobby;
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
        public bool isNavigatingAway = false;
        private LobbyClient client;
        private LobbyCallbackHandler callbackHandler;
        private int idHost;
        private bool isHostBool;
        public bool IsHost
        {
            get { return isHostBool; }
            set
            {
                isHostBool = value;
                OnPropertyChanged(nameof(IsHost));
            }
        }

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

        public ICommand ShowInviteFriendsCommand { get; }

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
            ShowInviteFriendsCommand = new RelayCommand(ExecuteShowInviteFriends, CanExecuteShowInviteFriends);

            InitializeConnectionAsync();
        }

        private async void InitializeConnectionAsync()
        {
            try
            {
                callbackHandler = new LobbyCallbackHandler();

                callbackHandler.OnPlayerJoined += HandlePlayerJoined;
                callbackHandler.OnPlayerLeft += HandlePlayerLeft;
                callbackHandler.OnHostLeft += HandleHostLeft;
                callbackHandler.OnMessageReceived += HandleMessageReceived;

                var context = new InstanceContext(callbackHandler);
                client = new LobbyClient(context);

                var lobbyState = await client.GetLobbyStateAsync(this.RoomCode);
                if (lobbyState == null)
                {
                    HandleHostLeft();
                    return;
                }

                idHost = lobbyState.idHostPlayer;
                this.IsHost = (PlayerSession.CurrentPlayer.idPlayer == idHost);
                UpdatePlayerList(lobbyState.Players);
                UpdateChat(lobbyState.ChatMessages);

                await client.JoinAndSubscribeAsync(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer);
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                NavigateToMainMenu();
            }
        }

        private void HandlePlayerJoined(PlayerDto newPlayer)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!Players.Any(p => p.Id == newPlayer.idPlayer))
                {
                    Players.Add(CreatePlayerViewModel(newPlayer));
                    UpdatePlayerCount();
                }
            });
        }

        private void HandlePlayerLeft(int idPlayer)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var playerToRemove = Players.FirstOrDefault(p => p.Id == idPlayer);
                if (playerToRemove != null)
                {
                    Players.Remove(playerToRemove);
                    UpdatePlayerCount();
                }
            });
        }

        private void HandleHostLeft()
        {
            if (isNavigatingAway) return;
            isNavigatingAway = true;

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Lang.InfoHostLeft, Lang.Lobby, MessageBoxButton.OK, MessageBoxImage.Information);

                CloseClientConnection(notifyServer: false);
                NavigateToMainMenu();
            });
        }

        private void HandleMessageReceived(MessageDto message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatMessages.Add($"{message.Nickname}: {message.Message}");
            });
        }

        private void UpdatePlayerList(PlayerDto[] players)
        {
            Players.Clear();
            foreach (var playerDto in players)
            {
                Players.Add(CreatePlayerViewModel(playerDto));
            }
            UpdatePlayerCount();
        }

        private void UpdateChat(MessageDto[] messages)
        {
            ChatMessages.Clear();
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    ChatMessages.Add($"{message.Nickname}: {message.Message}");
                }
            }
        }

        private PlayerLobbyItemViewModel CreatePlayerViewModel(PlayerDto playerDto)
        {
            var playerItem = new PlayerLobbyItemViewModel
            {
                Id = playerDto.idPlayer,
                ProfileImagePath = playerDto.pathPhoto,
                DisplayName = playerDto.nickname
            };

            if (playerDto.idPlayer == this.idHost)
            {
                playerItem.DisplayName = $"{Lang.LobbyHostPrefix} {playerDto.nickname}";
            }
            return playerItem;
        }

        private void UpdatePlayerCount()
        {
            int maxPlayers = 2;
            PlayerCountText = $"{Players.Count}/{maxPlayers}";
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
                try
                {
                    await client.SendMessageAsync(this.RoomCode, messageDto);
                }
                catch (Exception)
                {
                    MessageBox.Show(Lang.ErrorSendMessageFailed, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            CurrentMessage = string.Empty;
        }

        private void ExecuteGoBack(object parameter)
        {
            if (isNavigatingAway) return;
            isNavigatingAway = true;

            CloseClientConnection(notifyServer: true);
            NavigateToMainMenu(parameter as Window);
        }

        private void CloseClientConnection(bool notifyServer)
        {
            if (client == null) return;

            try
            {
                if (client.State == CommunicationState.Opened)
                {
                    if (notifyServer)
                    {
                        client.LeaveAndUnsubscribe(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer);
                    }
                    client.Close();
                }
                else
                {
                    client.Abort();
                }
            }
            catch (Exception)
            {
                client.Abort();
            }
            finally
            {
                client = null;
            }
        }

        private void NavigateToMainMenu(Window currentWindow = null)
        {
            var mainMenu = new View.MainMenu.MainMenu();
            mainMenu.Show();

            var windowToClose = currentWindow;
            if (windowToClose == null)
            {
                foreach (Window window in Application.Current.Windows.OfType<Window>().ToList())
                {
                    if (window.DataContext == this)
                    {
                        windowToClose = window;
                        break;
                    }
                }
            }

            if (windowToClose != null)
            {
                try
                {
                    windowToClose.Close();
                }
                catch (InvalidOperationException) { }
            }
        }

        private bool CanExecuteShowInviteFriends(object obj)
        {
            return IsHost;
        }

        private void ExecuteShowInviteFriends(object obj)
        {
            var vm = new InviteFriendsViewModel(this.RoomCode);
            var window = new InviteFriendsWindow
            {
                DataContext = vm,
                Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this)
            };
            window.ShowDialog(); 
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