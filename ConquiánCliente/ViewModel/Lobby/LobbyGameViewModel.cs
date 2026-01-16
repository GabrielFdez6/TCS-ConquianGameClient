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
        private const int MAX_PLAYERS = 2;
        private const int SPAM_TIME_WINDOW_SECONDS = 2;
        private const int COOLDOWN_DURATION_SECONDS = 5;
        private const int ONE_SECOND_DELAY_MS = 1000;
        private const int NO_PLAYER_ID = 0;
        private const int DIRECTION_NEXT = 1;
        private const int DIRECTION_PREVIOUS = -1;

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
        private int recentMessageCount = 0;
        private DateTime lastMessageTime = DateTime.MinValue;
        private bool isChatCooldownActive = false;
        private const int SPAM_THRESHOLD = 5;
        private const int SPAM_TIME_WINDOW = 2;
        private const int COOLDOWN_DURATION = 5;
        private string cooldownText;
        private bool isCooldownVisible;

        public bool IsHost
        {
            get { return isHostBool; }
            set { isHostBool = value; OnPropertyChanged(nameof(IsHost)); }
        }

        public ObservableCollection<PlayerLobbyItemViewModel> Players { get; }
        public ObservableCollection<MessageDto> ChatMessages { get; }

        public string CooldownText
        {
            get { return cooldownText; }
            set { cooldownText = value; OnPropertyChanged(nameof(CooldownText)); }
        }

        public bool IsCooldownVisible
        {
            get { return isCooldownVisible; }
            set { isCooldownVisible = value; OnPropertyChanged(nameof(IsCooldownVisible)); }
        }

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
            AssignCurrentPlayerId();

            try
            {
                SetupCallbacks();
                await JoinLobbyAndSyncState();
            }
            catch (EndpointNotFoundException ex)
            {
                await HandleConnectionError(ex);
            }
            catch (TimeoutException ex)
            {
                await HandleConnectionError(ex);
            }
            catch (CommunicationException ex)
            {
                await HandleConnectionError(ex);
            }
        }

        private void AssignCurrentPlayerId()
        {
            if (PlayerSession.CurrentPlayer != null)
            {
                this.myPlayerId = PlayerSession.CurrentPlayer.idPlayer;
            }
        }


        private void SetupCallbacks()
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
            SubscribeToChannelEvents();
        }

        private void SubscribeToChannelEvents()
        {
            if (client.InnerChannel != null)
            {
                client.InnerChannel.Closed += OnConnectionLost;
                client.InnerChannel.Faulted += OnConnectionLost;
            }
        }


        private void OnConnectionLost(object sender, EventArgs e)
        {
            if (PlayerSession.IsGuest)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    HandleGuestDisconnection();
                });
            }
        }

        private void HandleGuestDisconnection()
        {
            if (IsNavigatingAway)
            {
                return;
            }
            IsNavigatingAway = true;

            MessageBox.Show(Lang.ErrorLostConnection, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);

            CloseClientConnection(notifyServer: false);
            PlayerSession.EndSession();

            var loginWindow = new LogIn();
            loginWindow.Show();

            CloseCurrentWindow();
        }

        private async Task JoinLobbyAndSyncState()
        {
            var lobbyState = await client.GetLobbyStateAsync(this.RoomCode);

            if (lobbyState == null || string.IsNullOrEmpty(lobbyState.RoomCode))
            {
                HandleHostLeft();
                return;
            }

            SyncPlayerInfo(lobbyState);
            UpdateLobbyVisuals(lobbyState);
            await FinalizeLobbyJoin(lobbyState);
        }

        private void SyncPlayerInfo(LobbyDto lobbyState)
        {
            if (PlayerSession.CurrentPlayer != null)
            {
                var myUserOnServer = lobbyState.Players.FirstOrDefault(p => p.nickname == PlayerSession.CurrentPlayer.nickname);
                if (myUserOnServer != null)
                {
                    this.myPlayerId = myUserOnServer.idPlayer;
                    PlayerSession.CurrentPlayer.idPlayer = myUserOnServer.idPlayer;
                }
            }

            idHost = lobbyState.idHostPlayer;

            if (PlayerSession.CurrentPlayer != null)
            {
                this.IsHost = (PlayerSession.CurrentPlayer.idPlayer == idHost);
            }
        }

        private void UpdateLobbyVisuals(LobbyDto lobbyState)
        {
            UpdatePlayerList(lobbyState.Players);
            UpdateChat(lobbyState.ChatMessages);
        }

        private async Task FinalizeLobbyJoin(LobbyDto lobbyState)
        {
            if (lobbyState.idGamemode.HasValue)
            {
                UpdateSelectedGamemode(lobbyState.idGamemode.Value);
            }
            else if (this.IsHost)
            {
                await client.SelectGamemodeAsync(this.RoomCode, this.currentGameModeId);
            }

            if (!PlayerSession.IsGuest && PlayerSession.CurrentPlayer != null)
            {
                await client.JoinAndSubscribeAsync(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer);
            }
        }

        private async Task HandleConnectionError(Exception ex)
        {
            if (IsNavigatingAway)
            {
                return;
            }
            IsNavigatingAway = true;

            string errorMessage = Lang.ErrorConnectingToServer;
            string title = Lang.TitleError;
            bool isConnectionLost = true;

            if (ex is EndpointNotFoundException)
            {
                errorMessage = Lang.ErrorServerUnavailable;
                title = Lang.TitleConnectionError;
            }

            await ShowErrorAndNavigate(errorMessage, title, isConnectionLost);
        }

        private async Task ShowErrorAndNavigate(string message, string title, bool isConnectionLost)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                NavigateToLoginOrMainMenu(null, isConnectionLost);
            });
        }

        private bool CanExecuteKickPlayer(object parameter)
        {
            bool canKick = false;

            if (IsHost && parameter != null && PlayerSession.CurrentPlayer != null)
            {
                if (int.TryParse(parameter.ToString(), out int idTarget))
                {
                    canKick = idTarget != PlayerSession.CurrentPlayer.idPlayer;
                }
            }

            return canKick;
        }

        private void ExecuteKickPlayer(object parameter)
        {
            if (int.TryParse(parameter.ToString(), out int idPlayerToKick))
            {
                var result = MessageBox.Show(Lang.ConfirmKickPlayer, Lang.TitleConfirm, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    PerformKickPlayer(idPlayerToKick);
                }
            }
        }

        private void PerformKickPlayer(int idPlayerToKick)
        {
            Task.Run(async () =>
            {
                try
                {
                    await client.KickPlayerAsync(this.RoomCode, PlayerSession.CurrentPlayer.idPlayer, idPlayerToKick);
                }
                catch (FaultException<ServiceFaultDto> ex)
                {
                    HandleServiceFault(ex);
                }
                catch (CommunicationException)
                {
                    HandleCommunicationError();
                }
                catch (TimeoutException)
                {
                    HandleCommunicationError();
                }
            });
        }

        private void HandleServiceFault(FaultException<ServiceFaultDto> fault)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }

        private void HandleCommunicationError()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private void HandleYouWereKicked()
        {
            if (IsNavigatingAway)
            {
                return;
            }
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
            if (IsNavigatingAway)
            {
                return;
            }
            IsNavigatingAway = true;
            Task.Run(() => CloseClientConnection(notifyServer: true));
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
            if (IsNavigatingAway)
            {
                return;
            }
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
            if (IsHost)
            {
                return;
            }
            NavigateToGame();
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
            PlayerCountText = $"{Players.Count}/{MAX_PLAYERS}";
        }

        private bool CanExecuteSendMessage(object obj)
        {
            return !string.IsNullOrWhiteSpace(CurrentMessage) && !isChatCooldownActive;
        }

        private void ExecuteSendMessage(object obj)
        {
            CheckSpamStatus();

            var messageDto = new MessageDto
            {
                Nickname = PlayerSession.CurrentPlayer.nickname,
                Message = this.CurrentMessage,
                Timestamp = DateTime.UtcNow
            };

            SendMessageToServer(messageDto);

            CurrentMessage = string.Empty;

            if (recentMessageCount >= SPAM_THRESHOLD)
            {
                ActivateChatCooldown();
            }
        }

        private void CheckSpamStatus()
        {
            var currentTimestamp = DateTime.UtcNow;

            if ((currentTimestamp - lastMessageTime).TotalSeconds < SPAM_TIME_WINDOW_SECONDS)
            {
                recentMessageCount++;
            }
            else
            {
                recentMessageCount = 1;
            }

            lastMessageTime = currentTimestamp;
        }

        private void SendMessageToServer(MessageDto messageDto)
        {
            Task.Run(async () =>
            {
                try
                {
                    await client.SendMessageAsync(this.RoomCode, messageDto);
                }
                catch (EndpointNotFoundException)
                {
                    ShowMessageSendError(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
                }
                catch (TimeoutException)
                {
                    ShowMessageSendError(Lang.ErrorSendMessageFailed, Lang.TitleError);
                }
                catch (CommunicationException)
                {
                    ShowMessageSendError(Lang.ErrorSendMessageFailed, Lang.TitleError);
                }
            });
        }

        private void ShowMessageSendError(string message, string title)
        {
            if (!IsNavigatingAway)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        private void ActivateChatCooldown()
        {
            isChatCooldownActive = true;
            recentMessageCount = 0;
            IsCooldownVisible = true;

            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);

            Task.Run(async () =>
            {
                for (int i = COOLDOWN_DURATION_SECONDS; i > 0; i--)
                {
                    CooldownText = string.Format(Lang.ChatCooldownWarning, i);
                    await Task.Delay(ONE_SECOND_DELAY_MS);
                }

                ResetCooldownState();
            });
        }

        private void ResetCooldownState()
        {
            isChatCooldownActive = false;
            IsCooldownVisible = false;
            CooldownText = string.Empty;

            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        private void ExecuteGoBack(object parameter)
        {
            if (isNavigatingAway)
            {
                return;
            }
            isNavigatingAway = true;
            Task.Run(() => CloseClientConnection(notifyServer: true));
            NavigateToLoginOrMainMenu(parameter as Window);
        }

        public void CloseClientConnection(bool notifyServer)
        {
            if (client == null)
            {
                return;
            }

            if (notifyServer && this.myPlayerId != NO_PLAYER_ID)
            {
                AttemptNotifyServerLeave();
            }

            SafeCloseClient();
        }

        private void AttemptNotifyServerLeave()
        {
            try
            {
                client.LeaveAndUnsubscribe(this.RoomCode, this.myPlayerId);
            }
            catch (CommunicationException)
            {
                // Ignored: The server is not reachable to report the output. 
                // Since the client is closing, no retry is required.
            }
            catch (TimeoutException)
            {
                // Ignored: The request exceeded the timeout. 
                // The priority is to free up client resources, so no retry is attempted.
            }
        }

        private void SafeCloseClient()
        {
            try
            {
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
            var windows = Application.Current.Windows.OfType<Window>().ToList();
            foreach (Window window in windows)
            {
                if (window.DataContext == this)
                {
                    CloseWindowSafely(window);
                    break;
                }
            }
        }

        private void CloseWindowSafely(Window window)
        {
            try
            {
                window.Close();
            }
            catch (InvalidOperationException)
            {
                // Ignored: This exception occurs if the window is already closed or closing.
                // The goal is to ensure that the window does not exist, so this state is acceptable.
            }
        }

        private void NavigateToLoginOrMainMenu(Window currentWindow = null, bool isConnectionLost = false)
        {
            DisconnectInvitationServiceAsync(isConnectionLost);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Window newWindow = CreateNextWindow(isConnectionLost);
                TransitionToWindow(newWindow, currentWindow);
            });
        }

        private static void DisconnectInvitationServiceAsync(bool isConnectionLost)
        {
            if (isConnectionLost && PlayerSession.CurrentPlayer != null)
            {
                Task.Run(() =>
                {
                    try
                    {
                        InvitationClientManager.Disconnect(PlayerSession.CurrentPlayer.idPlayer);
                    }
                    catch (Exception)
                    {
                        // Ignored: This is a ‘best effort’ cleanup task.
                        // Failure here should not halt the main shutdown flow or cause a crash.
                    }
                });
            }
        }

        private static Window CreateNextWindow(bool isConnectionLost)
        {
            Window newWindow;
            if (PlayerSession.IsGuest || isConnectionLost)
            {
                PlayerSession.EndSession();
                newWindow = new LogIn();
            }
            else
            {
                newWindow = new View.MainMenu.MainMenu();
            }
            return newWindow;
        }

        private void TransitionToWindow(Window newWindow, Window currentWindow)
        {
            newWindow.Show();
            Application.Current.MainWindow = newWindow;

            var windowToClose = currentWindow;
            if (windowToClose == null)
            {
                windowToClose = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            }

            if (windowToClose != null)
            {
                CloseWindowSafely(windowToClose);
            }
        }

        private bool CanExecuteStartGame(object obj)
        {
            bool canStart = IsHost && Players.Count == MAX_PLAYERS && gameModes.ContainsKey(currentGameModeId);
            return canStart;
        }

        private void ExecuteStartGame(object parameter)
        {
            if (currentGameModeId == 0 || !gameModes.ContainsKey(currentGameModeId))
            {
                MessageBox.Show(Lang.ErrorGamemodeNotSelected, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (IsNavigatingAway)
            {
                return;
            }

            Task.Run(ProcessStartGameRequest);
        }

        private async Task ProcessStartGameRequest()
        {
            try
            {
                await client.StartGameAsync(this.RoomCode);
                NavigateToGame();
            }
            catch (FaultException<ServiceFaultDto> ex)
            {
                HandleStartGameFault(ex);
            }
            catch (CommunicationException)
            {
                ShowStartGameError(Lang.ErrorConnectingToServer);
            }
            catch (TimeoutException)
            {
                ShowStartGameError(Lang.ErrorConnectingToServer);
            }
        }

        private void ShowStartGameError(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        private void HandleStartGameFault(FaultException<ServiceFaultDto> fault)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (errorType == ConquiánCliente.ServiceLogin.ServiceErrorType.OpponentConnectionLost)
                {
                    MessageBox.Show(msg, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private bool CanExecuteShowInviteFriends(object obj)
        {
            return IsHost && !PlayerSession.IsGuest;
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

        private void UpdateSelectedGamemode(int id)
        {
            if (gameModes.ContainsKey(id))
            {
                currentGameModeId = id;
                OnPropertyChanged(nameof(SelectedGameType));
                Application.Current.Dispatcher.Invoke(() => { CommandManager.InvalidateRequerySuggested(); });
            }
        }

        private bool CanExecuteGameTypeChange(object obj)
        {
            return IsHost;
        }

        private void ExecuteNextGameType(object obj)
        {
            ChangeGameModeIndex(DIRECTION_NEXT);
        }

        private void ExecutePreviousGameType(object obj)
        {
            ChangeGameModeIndex(DIRECTION_PREVIOUS);
        }

        private void ChangeGameModeIndex(int direction)
        {
            int currentIndex = gameModeIds.IndexOf(currentGameModeId);
            int nextIndex = (currentIndex + direction + gameModeIds.Count) % gameModeIds.Count;
            int newId = gameModeIds[nextIndex];

            PerformGameModeUpdate(newId);
        }

        private void PerformGameModeUpdate(int newId)
        {
            Task.Run(async () =>
            {
                try
                {
                    await client.SelectGamemodeAsync(this.RoomCode, newId);
                }
                catch (FaultException<ServiceFaultDto> ex)
                {
                    HandleServiceFault(ex);
                }
                catch (CommunicationException)
                {
                    HandleCommunicationError();
                }
                catch (TimeoutException)
                {
                    HandleCommunicationError();
                }
            });
        }

        private void NavigateToGame()
        {
            if (IsNavigatingAway)
            {
                return;
            }
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