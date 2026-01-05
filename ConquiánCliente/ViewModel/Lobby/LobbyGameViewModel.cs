using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLobby;
using ConquiánCliente.View.Lobby;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class LobbyGameViewModel : ViewModelBase
    {
        private readonly Dictionary<int, string> gameModes = new Dictionary<int, string>
        {
            { 1, Lang.LobbyQuickGame },
            { 2, Lang.LobbyClassicGame }
        };
        private readonly List<int> gameModeIds;
        private int currentGameModeId;
        private string playerCountText;
        private string roomCode;
        private string currentMessage;
        private bool isNavigatingAway = false;
        private LobbyClient client;
        private int idHost;
        private bool isHostBool;
        private readonly IMessageResolver messageResolver;
        private int myPlayerId;

        public bool IsHost
        {
            get { return isHostBool; }
            set { isHostBool = value; OnPropertyChanged(nameof(IsHost)); }
        }

        public ObservableCollection<PlayerLobbyItemViewModel> Players { get; }
        public ObservableCollection<MessageDto> ChatMessages { get; }
        public string RoomCode
        {
            get { return roomCode; }
            set { roomCode = value; OnPropertyChanged(nameof(RoomCode)); }
        }
        public string SelectedGameType
        {
            get { return gameModes.ContainsKey(currentGameModeId) ? gameModes[currentGameModeId] : ""; }
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

        public bool IsNavigatingAway
        {
            get { return isNavigatingAway; }
            set { isNavigatingAway = value; OnPropertyChanged(nameof(IsNavigatingAway)); }
        }

        public ICommand NextGameTypeCommand { get; }
        public ICommand PreviousGameTypeCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand ShowInviteFriendsCommand { get; }
        public ICommand ShutdownApplicationCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand KickPlayerCommand { get; }

        public LobbyGameViewModel(string receivedRoomCode)
        {
            Players = new ObservableCollection<PlayerLobbyItemViewModel>();
            ChatMessages = new ObservableCollection<MessageDto>();
            this.messageResolver = new ResourceMessageResolver();

            this.RoomCode = receivedRoomCode;

            gameModeIds = gameModes.Keys.ToList();
            currentGameModeId = gameModeIds.FirstOrDefault();

            NextGameTypeCommand = new RelayCommand(ExecuteNextGameType, CanExecuteGameTypeChange);
            PreviousGameTypeCommand = new RelayCommand(ExecutePreviousGameType, CanExecuteGameTypeChange);

            GoBackCommand = new RelayCommand(ExecuteGoBack);
            SendMessageCommand = new RelayCommand(ExecuteSendMessage, CanExecuteSendMessage);
            ShowInviteFriendsCommand = new RelayCommand(ExecuteShowInviteFriends, CanExecuteShowInviteFriends);
            ShutdownApplicationCommand = new RelayCommand(ExecuteShutdownApplication);
            StartGameCommand = new RelayCommand(ExecuteStartGame, CanExecuteStartGame);
            KickPlayerCommand = new RelayCommand(ExecuteKickPlayer, CanExecuteKickPlayer);

            _ = InitializeConnectionAsync();
        }

        private async Task InitializeConnectionAsync()
        {
            if (PlayerSession.CurrentPlayer != null)
            {
                this.myPlayerId = PlayerSession.CurrentPlayer.idPlayer;
            }

            try
            {
                var callbackHandler = LobbyCallbackHandler.Instance;

                callbackHandler.OnPlayerJoined += HandlePlayerJoined;
                callbackHandler.OnPlayerLeft += HandlePlayerLeft;
                callbackHandler.OnHostLeft += HandleHostLeft;
                callbackHandler.OnMessageReceived += HandleMessageReceived;
                callbackHandler.OnGamemodeChanged += HandleGamemodeChanged;
                callbackHandler.OnGameStarting += HandleGameStarting;
                callbackHandler.OnYouWereKicked += HandleYouWereKicked;

                var context = new InstanceContext(callbackHandler);
                client = new LobbyClient(context);

                var lobbyState = await client.GetLobbyStateAsync(this.RoomCode);
                if (lobbyState == null || string.IsNullOrEmpty(lobbyState.RoomCode))
                {
                    HandleHostLeft();
                    return;
                }

                idHost = lobbyState.idHostPlayer;
                this.IsHost = (PlayerSession.CurrentPlayer.idPlayer == idHost);
                UpdatePlayerList(lobbyState.Players);
                UpdateChat(lobbyState.ChatMessages);

                if (lobbyState.idGamemode.HasValue)
                {
                    UpdateSelectedGamemode(lobbyState.idGamemode.Value);
                }
                else if (this.IsHost)
                {
                    await client.SelectGamemodeAsync(this.RoomCode, this.currentGameModeId);
                }

                if (!PlayerSession.IsGuest)
                {
                    await client.JoinAndSubscribeAsync(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer);
                }
            }
            catch (FaultException<ServiceFaultDto> fault)
            {
                IsNavigatingAway = true;

                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                });

                CloseClientConnection(notifyServer: false);
                NavigateToLoginOrMainMenu();
            }
            catch (CommunicationException)
            {
                IsNavigatingAway = true;

                PresenceClientManager.Instance.StopHeartbeat();

                InvitationClientManager.Disconnect(PlayerSession.CurrentPlayer.idPlayer);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                });

                NavigateToLoginOrMainMenu(isConnectionLost: true);
            }
            catch (Exception)
            {
                IsNavigatingAway = true;
                PresenceClientManager.Instance.StopHeartbeat();
                InvitationClientManager.Disconnect(PlayerSession.CurrentPlayer.idPlayer);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                });
                NavigateToLoginOrMainMenu(isConnectionLost: true);
            }
        }

        private bool CanExecuteKickPlayer(object parameter)
        {
            if (!IsHost || parameter == null || PlayerSession.CurrentPlayer == null) return false;
            if (int.TryParse(parameter.ToString(), out int idTarget))
            {
                return idTarget != PlayerSession.CurrentPlayer.idPlayer;
            }
            return false;
        }

        private void ExecuteKickPlayer(object parameter)
        {
            if (int.TryParse(parameter.ToString(), out int idPlayerToKick))
            {
                var result = MessageBox.Show(Lang.ConfirmKickPlayer, Lang.TitleConfirm, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes) return;

                Task.Run(async () =>
                {
                    try
                    {
                        await client.KickPlayerAsync(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer, idPlayerToKick);
                    }
                    catch (FaultException<ServiceFaultDto> fault)
                    {
                        var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                        string msg = messageResolver.GetMessage(errorType);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                        });
                    }
                    catch (Exception)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(Lang.ErrorLobbyAction, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                    }
                });
            }
        }

        private void HandleYouWereKicked()
        {
            if (IsNavigatingAway) return;
            IsNavigatingAway = true;

            Application.Current.Dispatcher.Invoke(() =>
            {
                CloseClientConnection(notifyServer: false);
                NavigateToLoginOrMainMenu();
                MessageBox.Show(Lang.InfoYouWereKicked, Lang.Lobby, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void ExecuteShutdownApplication(object obj)
        {
            if (IsNavigatingAway) return;
            IsNavigatingAway = true;
            CloseClientConnection(notifyServer: true);
            Application.Current.Shutdown();
        }
        private void HandlePlayerJoined(PlayerDto newPlayer)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!Players.Any(p => p.Id == newPlayer.idPlayer))
                {
                    Players.Add(CreatePlayerViewModel(newPlayer));
                    UpdatePlayerCount();
                    CommandManager.InvalidateRequerySuggested();
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
                    CommandManager.InvalidateRequerySuggested();
                }
            });
        }
        private void HandleHostLeft()
        {
            if (IsNavigatingAway) return;
            IsNavigatingAway = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Lang.InfoHostLeft, Lang.Lobby, MessageBoxButton.OK, MessageBoxImage.Information);
                CloseClientConnection(notifyServer: false);
                NavigateToLoginOrMainMenu();
            });
        }
        private void HandleMessageReceived(MessageDto message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatMessages.Add(message);
            });
        }
        private void HandleGamemodeChanged(int newGamemodeId)
        {
            Application.Current.Dispatcher.Invoke(() => { UpdateSelectedGamemode(newGamemodeId); });
        }
        private void HandleGameStarting()
        {
            if (IsHost) return;
            NavigateToGame();
        }
        private void UpdatePlayerList(PlayerDto[] players)
        {
            Players.Clear();
            foreach (var playerDto in players) Players.Add(CreatePlayerViewModel(playerDto));
            UpdatePlayerCount();
        }
        private void UpdateChat(MessageDto[] messages)
        {
            ChatMessages.Clear();
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    ChatMessages.Add(message);
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
        private bool CanExecuteSendMessage(object obj) => !string.IsNullOrWhiteSpace(CurrentMessage);

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
            NavigateToLoginOrMainMenu(parameter as Window);
        }
        public void CloseClientConnection(bool notifyServer)
        {
            if (client == null) return;
            try
            {
                if (notifyServer && client.State == CommunicationState.Opened)
                {
                    try
                    {
                        if (this.myPlayerId > 0)
                        {
                            client.LeaveAndUnsubscribe(this.RoomCode, this.myPlayerId);
                        }
                    }
                    catch (Exception) { }
                }
                client.Close();
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
        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows.OfType<Window>().ToList())
            {
                if (window.DataContext == this)
                {
                    try { window.Close(); }
                    catch (InvalidOperationException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Intento de cerrar ventana ya en proceso: {ex.Message}");
                    }
                    break;
                }
            }
        }
        private void NavigateToLoginOrMainMenu(Window currentWindow = null, bool isConnectionLost = false)
        {
            if (isConnectionLost)
            {
                PresenceClientManager.Instance.StopHeartbeat();
                if (PlayerSession.CurrentPlayer != null)
                    InvitationClientManager.Disconnect(PlayerSession.CurrentPlayer.idPlayer);
            }

            Window newWindow = null;

            if (PlayerSession.IsGuest || isConnectionLost)
            {
                PlayerSession.EndSession();
                newWindow = new LogIn();
            }
            else
            {
                newWindow = new View.MainMenu.MainMenu();
            }

            newWindow.Show();
            Application.Current.MainWindow = newWindow;

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
                try { windowToClose.Close(); }
                catch (InvalidOperationException ex) { System.Diagnostics.Debug.WriteLine($"Intento de cerrar ventana ya en proceso: {ex.Message}"); }
            }
        }

        private bool CanExecuteStartGame(object obj)
        {
            return IsHost && Players.Count == 2 && gameModes.ContainsKey(currentGameModeId);
        }

        private void ExecuteStartGame(object parameter)
        {
            if (currentGameModeId == 0 || !gameModes.ContainsKey(currentGameModeId))
            {
                MessageBox.Show(Lang.ErrorGamemodeNotSelected, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (IsNavigatingAway) return;

            Task.Run(async () =>
            {
                try
                {
                    await client.StartGameAsync(this.RoomCode);
                    NavigateToGame();
                }
                catch (FaultException<ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
                catch (CommunicationException)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    });

                }
                catch (Exception)
                {
                    if (!isNavigatingAway)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(Lang.ErrorStartingGame, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                    }
                }
            });
        }

        private bool CanExecuteShowInviteFriends(object obj) => IsHost && !PlayerSession.IsGuest;
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
        private void UpdateSelectedGamemode(int id)
        {
            if (gameModes.ContainsKey(id))
            {
                currentGameModeId = id;
                OnPropertyChanged(nameof(SelectedGameType));
                Application.Current.Dispatcher.Invoke(() => { CommandManager.InvalidateRequerySuggested(); });
            }
        }
        private bool CanExecuteGameTypeChange(object obj) => IsHost;

        private void ExecuteNextGameType(object obj)
        {
            int currentIndex = gameModeIds.IndexOf(currentGameModeId);
            int nextIndex = (currentIndex + 1) % gameModeIds.Count;
            int newId = gameModeIds[nextIndex];

            Task.Run(async () =>
            {
                try
                {
                    await client.SelectGamemodeAsync(this.RoomCode, newId);
                }
                catch (FaultException<ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
                catch (CommunicationException)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
                catch (Exception)
                {
                    MessageBox.Show(Lang.ErrorSelectGameMode, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void ExecutePreviousGameType(object obj)
        {
            int currentIndex = gameModeIds.IndexOf(currentGameModeId);
            int prevIndex = (currentIndex - 1 + gameModeIds.Count) % gameModeIds.Count;
            int newId = gameModeIds[prevIndex];

            Task.Run(async () =>
            {
                try
                {
                    await client.SelectGamemodeAsync(this.RoomCode, newId);
                }
                catch (FaultException<ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
                catch (CommunicationException)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
                catch (Exception)
                {
                    MessageBox.Show(Lang.ErrorSelectGameMode, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void NavigateToGame()
        {
            if (IsNavigatingAway) return;
            IsNavigatingAway = true;

            Application.Current.Dispatcher.Invoke(() =>
            {
                CloseClientConnection(notifyServer: false);
                var gameViewModel = new ConquiánCliente.ViewModel.Game.GameViewModel(this.RoomCode);
                var gameWindow = new ConquiánCliente.View.Game.Game(this.RoomCode);
                gameWindow.DataContext = gameViewModel;
                gameWindow.Show();
                CloseCurrentWindow();
            });
        }
    }
}