using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceGame;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ConquiánCliente.ViewModel.Game
{
    public class GameViewModel : ViewModelBase
    {
        private const int ANIMATION_DELAY_MS = 1000;
        private const int CATCHUP_THRESHOLD_MS = 200;
        private const int MINIMUM_MELD_SIZE = 3;
        private const int SINGLE_CARD_COUNT = 1;
        private const int MINIMUM_CARDS_FOR_MELD_FROM_HAND = 2;
        private const int INACTIVITY_LIMIT_SECONDS = 60;
        private const int GRACE_PERIOD_SECONDS = 60;

        private DispatcherTimer activityTimer;
        private readonly Stopwatch afkStopwatch;
        private bool isWarningShown;

        private readonly string roomCode;
        private GameClient client;
        private bool isGameEnded = false;
        private readonly IMessageResolver messageResolver;
        private bool isNavigatingAway = false;
        private bool isCatchingUp = false;
        private DateTime lastEventTime = DateTime.MinValue;
        private GameCallbackHandler gameCallbackHandler;

        private bool isAFKWarningVisible;
        public bool IsAFKWarningVisible
        {
            get { return isAFKWarningVisible; }
            set { isAFKWarningVisible = value; OnPropertyChanged(nameof(IsAFKWarningVisible)); }
        }

        public RelayCommand AcceptAFKCommand { get; set; }
        public RelayCommand PassTurnCommand { get; set; }
        public ObservableCollection<CardViewModel> PlayerHand { get; set; }
        public ObservableCollection<object> OpponentFaceDownCards { get; set; }
        public ObservableCollection<MeldViewModel> PlayerMelds { get; set; }
        public ObservableCollection<MeldViewModel> OpponentMelds { get; set; }
        public ObservableCollection<CardViewModel> TemporaryMeld { get; set; }

        private string gameTimeDisplay;
        public string GameTimeDisplay
        {
            get { return gameTimeDisplay; }
            set { gameTimeDisplay = value; OnPropertyChanged(nameof(GameTimeDisplay)); }
        }

        private CardDto topDiscardCard;
        public CardDto TopDiscardCard
        {
            get { return topDiscardCard; }
            set { topDiscardCard = value; OnPropertyChanged(nameof(TopDiscardCard)); }
        }

        private PlayerDto opponent;
        public PlayerDto Opponent
        {
            get { return opponent; }
            set { opponent = value; OnPropertyChanged(nameof(Opponent)); }
        }

        private PlayerDto currentPlayer;
        public PlayerDto CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; OnPropertyChanged(nameof(CurrentPlayer)); }
        }

        private string turnStatusText;
        public string TurnStatusText
        {
            get { return turnStatusText; }
            set { turnStatusText = value; OnPropertyChanged(nameof(TurnStatusText)); }
        }

        private bool isMyTurn;
        public bool IsMyTurn
        {
            get { return isMyTurn; }
            set { isMyTurn = value; OnPropertyChanged(nameof(IsMyTurn)); }
        }

        private bool isStockPileBlinking;
        public bool IsStockPileBlinking
        {
            get { return isStockPileBlinking; }
            set { isStockPileBlinking = value; OnPropertyChanged(nameof(IsStockPileBlinking)); }
        }

        private bool canDiscard;
        public bool CanDiscard
        {
            get { return canDiscard; }
            set { canDiscard = value; OnPropertyChanged(nameof(CanDiscard)); }
        }

        private bool hasJustDrawnFromDeck;
        public bool HasJustDrawnFromDeck
        {
            get { return hasJustDrawnFromDeck; }
            set { hasJustDrawnFromDeck = value; OnPropertyChanged(nameof(HasJustDrawnFromDeck)); }
        }

        public GameViewModel(string roomCode)
        {
            this.roomCode = roomCode;
            this.messageResolver = new ResourceMessageResolver();

            afkStopwatch = new Stopwatch();

            PlayerHand = new ObservableCollection<CardViewModel>();
            OpponentFaceDownCards = new ObservableCollection<object>();
            PlayerMelds = new ObservableCollection<MeldViewModel>();
            OpponentMelds = new ObservableCollection<MeldViewModel>();
            TemporaryMeld = new ObservableCollection<CardViewModel>();

            PassTurnCommand = new RelayCommand(async (o) => await PassTurnAsync());

            var sessionPlayer = PlayerSession.CurrentPlayer;
            CurrentPlayer = new ServiceGame.PlayerDto
            {
                idPlayer = sessionPlayer.idPlayer,
                nickname = sessionPlayer.nickname,
                pathPhoto = sessionPlayer.pathPhoto
            };

            InitializeAFKTimer();
            AcceptAFKCommand = new RelayCommand(OnAcceptAFK);

            _ = InitializeGameConnectionAsync();
        }

        public void Cleanup()
        {
            StopTurnTimer();
            CloseClientConnection();
            UnsubscribeCallbacks();
        }

        private void CloseClientConnection()
        {
            if (client != null)
            {
                try
                {
                    client.InnerChannel.Closed -= OnConnectionLost;
                    client.InnerChannel.Faulted -= OnConnectionLost;
                    client.Abort();
                }
                catch
                {
                    // Intentionally ignored: if the channel is already closed or in a failed state,
                    // Abort() could throw an exception that does not affect the cleanup of other resources.
                }
                finally
                {
                    client = null;
                }
            }
        }

        private void UnsubscribeCallbacks()
        {
            if (gameCallbackHandler != null)
            {
                gameCallbackHandler.OnGameEnded -= HandleGameEnded;
                gameCallbackHandler.OnOpponentMeld -= HandleOpponentMeld;
                gameCallbackHandler.OnOpponentLeftEvent -= HandleOpponentLeft;
                gameCallbackHandler.OnGameEndedByAfkEvent -= HandleGameEndedByAFK;
            }
        }

        public void LeaveGame()
        {
            if (isNavigatingAway)
            {
                return;
            }

            isNavigatingAway = true;

            Task.Run(() =>
            {
                try
                {
                    if (client != null && CurrentPlayer != null)
                    {
                        client.LeaveGame(roomCode, CurrentPlayer.idPlayer);
                    }
                }
                catch (Exception)
                {
                    // The exception is intentionally ignored so as not to block navigation if the connection fails upon exit. 
                }
            });
        }

        public async Task PassTurnAsync()
        {
            StopTurnTimer();

            if (client == null || !IsMyTurn)
            {
                return;
            }

            try
            {
                await client.PassTurnAsync(roomCode, CurrentPlayer.idPlayer);
                ResetTurnState();
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private void ResetTurnState()
        {
            HasJustDrawnFromDeck = false;
            CanDiscard = false;

            foreach (var card in PlayerHand)
            {
                card.IsSelected = false;
            }

            if (IsMyTurn)
            {
                IsStockPileBlinking = true;
            }
        }

        private async Task InitializeGameConnectionAsync()
        {
            try
            {
                gameCallbackHandler = ConfigureGameCallbacks();
                var context = new InstanceContext(gameCallbackHandler);
                client = new GameClient(context);

                MonitorConnection();

                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                GameStateDto gameState = await client.JoinGameAsync(roomCode, playerId);

                if (gameState != null)
                {
                    await LoadGameState(gameState);
                }
                else
                {
                    MessageBox.Show(Lang.ErrorGeneric, Lang.ErrorGame, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
        }

        private void MonitorConnection()
        {
            if (client.InnerChannel != null)
            {
                client.InnerChannel.Closed += OnConnectionLost;
                client.InnerChannel.Faulted += OnConnectionLost;
            }
        }

        private async Task LoadGameState(GameStateDto gameState)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                PlayerHand.Clear();
                foreach (var cardDto in gameState.PlayerHand)
                {
                    PlayerHand.Add(new CardViewModel(cardDto));
                }

                TopDiscardCard = gameState.TopDiscardCard;
                Opponent = gameState.Opponent;
                UpdateOpponentCardCount(gameState.OpponentCardCount);

                UpdateTurnStatus(gameState.CurrentTurnPlayerId);
                UpdateTimerDisplay(gameState.TotalGameSeconds);
            });
        }

        private void HandleConnectionError(Exception ex)
        {
            if (ex is EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Cleanup();
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnConnectionLost(object sender, EventArgs e)
        {
            StopTurnTimer();
            if (PlayerSession.IsGuest)
            {
                Application.Current.Dispatcher.Invoke(() => HandleGuestDisconnection());
            }
        }

        private void HandleGuestDisconnection()
        {
            if (isNavigatingAway)
            {
                return;
            }
            isNavigatingAway = true;
            StopTurnTimer();

            MessageBox.Show(Lang.ErrorLostConnection, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);

            var loginWindow = new LogIn();
            loginWindow.Show();
            CloseWindow();
            PlayerSession.EndSession();
        }

        private GameCallbackHandler ConfigureGameCallbacks()
        {
            var callbackHandler = new GameCallbackHandler();

            callbackHandler.OnOpponentDiscarded += (card) =>
            {
                Application.Current.Dispatcher.Invoke(() => TopDiscardCard = card);
            };

            callbackHandler.OnOpponentDrewDeck += () => { };

            callbackHandler.TimeStateUpdated += (gameSeconds, turnSeconds, newTurnPlayerId) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdateTimerDisplay(gameSeconds);
                    UpdateTurnStatus(newTurnPlayerId);
                });
            };

            callbackHandler.OpponentHandUpdated += (newCardCount) =>
            {
                Application.Current.Dispatcher.Invoke(() => UpdateOpponentCardCount(newCardCount));
            };

            callbackHandler.OnOpponentMeld += HandleOpponentMeld;
            callbackHandler.OnGameEnded += HandleGameEnded;
            callbackHandler.OnOpponentLeftEvent += HandleOpponentLeft;
            callbackHandler.OnGameEndedByAfkEvent += HandleGameEndedByAFK;

            return callbackHandler;
        }

        private async void HandleOpponentMeld(CardDto[] meldCardDtos)
        {
            CheckSyncStatus();
            var cardVMs = meldCardDtos.Select(dto => new CardViewModel(dto)).ToList();

            if (!isCatchingUp)
            {
                await ShowMeldAnimation(cardVMs);
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                OpponentMelds.Add(new MeldViewModel(cardVMs));
            });
        }

        private async Task ShowMeldAnimation(List<CardViewModel> cardVMs)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                foreach (var cardVM in cardVMs)
                {
                    TemporaryMeld.Add(cardVM);
                }
            });
            await Task.Delay(ANIMATION_DELAY_MS);
        }

        private void CheckSyncStatus()
        {
            if ((DateTime.UtcNow - lastEventTime).TotalMilliseconds < CATCHUP_THRESHOLD_MS)
            {
                isCatchingUp = true;
            }
            else
            {
                isCatchingUp = false;
            }
            lastEventTime = DateTime.UtcNow;
        }

        private void HandleGameEnded(GameResultDto results)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!PlayerSession.IsLoggedIn || isNavigatingAway || isGameEnded)
                {
                    return;
                }

                isNavigatingAway = true;
                isGameEnded = true;
                Cleanup();
                StopTurnTimer();
                ShowGameResults(results);

                if (results.ErrorSavingToDatabase)
                {
                    MessageBox.Show(Lang.GameResultErrorSQL, Lang.TitleError, MessageBoxButton.OK);
                }
            });
        }

        private void HandleOpponentLeft()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!PlayerSession.IsLoggedIn || isNavigatingAway)
                {
                    return;
                }

                isNavigatingAway = true;
                StopTurnTimer();
                MessageBox.Show(Lang.GameOpponentLeft, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                if (PlayerSession.IsGuest)
                {
                    HandleGuestExit();
                }
                else
                {
                    NavigateToMainMenu();
                }
            });
        }

        private void HandleGuestExit()
        {
            PlayerSession.EndSession();
            var loginView = new LogIn();
            loginView.Show();
            CloseWindow();
        }


        private void HandleGameEndedByAFK(string reasonKey)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!PlayerSession.IsLoggedIn || isNavigatingAway)
                {
                    return;
                }

                isNavigatingAway = true;
                activityTimer.Stop();
                IsAFKWarningVisible = false;

                string message = Lang.ResourceManager.GetString(reasonKey);
                MessageBox.Show(message, Lang.TitleAuthenticationError, MessageBoxButton.OK, MessageBoxImage.Information);

                NavigateToMainMenu();
            });
        }

        private void NavigateToMainMenu()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingMenu = Application.Current.Windows.OfType<ConquiánCliente.View.MainMenu.MainMenu>().FirstOrDefault();

                if (existingMenu != null)
                {
                    existingMenu.Activate();
                    if (existingMenu.WindowState == WindowState.Minimized)
                    {
                        existingMenu.WindowState = WindowState.Normal;
                    }
                }
                else
                {
                    var mainMenu = new ConquiánCliente.View.MainMenu.MainMenu();
                    mainMenu.Show();
                }

                CloseWindow();
            });
        }

        private void ShowGameResults(GameResultDto result)
        {
            StopTurnTimer();
            int myPlayerId = CurrentPlayer.idPlayer;
            var resultsVM = new GameResultsViewModel(result, myPlayerId);
            var resultsWindow = new ConquiánCliente.View.Game.GameResults();
            resultsWindow.DataContext = resultsVM;
            resultsWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.DataContext == this)
                {
                    win.Close();
                    break;
                }
            }
        }

        private void UpdateOpponentCardCount(int newCardCount)
        {
            OpponentFaceDownCards.Clear();
            for (int i = 0; i < newCardCount; i++)
            {
                OpponentFaceDownCards.Add(new object());
            }
        }

        public async Task PlayCardsAsync(List<string> cardIds, Action onSuccess = null)
        {
            if (!CanPlayCards(cardIds) || cardIds.Count > MINIMUM_MELD_SIZE)
            {
                return;
            }

            bool isUsingDiscardCard = TopDiscardCard != null && cardIds.Contains(TopDiscardCard.Id);
            var cardsToPlay = GetCardsToPlay(cardIds);

            if (cardsToPlay.Count != cardIds.Count)
            {
                return;
            }

            string validationError = ConquianRulesValidator.ValidateMeld(cardsToPlay);
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await MoveCardsToTemporaryMeldAsync(cardsToPlay);
            await TryExecutePlayCardsAsync(cardsToPlay, cardIds, isUsingDiscardCard, onSuccess);
        }

        private async Task TryExecutePlayCardsAsync(List<CardViewModel> cardsToPlay, List<string> cardIds, bool isUsingDiscardCard, Action onSuccess)
        {
            try
            {
                await client.PlayCardsAsync(roomCode, CurrentPlayer.idPlayer, cardIds.ToArray());
                await Task.Delay(ANIMATION_DELAY_MS);

                if (isGameEnded)
                {
                    return;
                }

                await FinalizeSuccessfulPlayAsync(cardsToPlay, isUsingDiscardCard);

                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
                await RollbackPlayCards(cardsToPlay);
            }
        }

        private bool CanPlayCards(List<string> cardIds)
        {
            return client != null && CurrentPlayer != null && cardIds != null;
        }

        private List<CardViewModel> GetCardsToPlay(List<string> cardIds)
        {
            var cards = PlayerHand.Where(vm => cardIds.Contains(vm.Id)).ToList();

            if (TopDiscardCard != null && cardIds.Contains(TopDiscardCard.Id))
            {
                var discardVM = new CardViewModel(TopDiscardCard);
                cards.Add(discardVM);
            }

            return cards;
        }

        private async Task MoveCardsToTemporaryMeldAsync(List<CardViewModel> cardsToPlay)
        {
            foreach (var cardVM in cardsToPlay.Where(c => PlayerHand.Contains(c)).ToList())
            {
                PlayerHand.Remove(cardVM);
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                foreach (var cardVM in cardsToPlay)
                {
                    TemporaryMeld.Add(cardVM);
                }
            });
        }

        private async Task<bool> TryExecutePlayCardsAsync(List<CardViewModel> cardsToPlay, List<string> cardIds, bool isUsingDiscardCard)
        {
            try
            {
                await client.PlayCardsAsync(roomCode, CurrentPlayer.idPlayer, cardIds.ToArray());
                await Task.Delay(ANIMATION_DELAY_MS);

                if (isGameEnded)
                {
                    return true;
                }

                await FinalizeSuccessfulPlayAsync(cardsToPlay, isUsingDiscardCard);
                return true;
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
                await RollbackPlayCards(cardsToPlay);
                return false;
            }
        }

        private async Task FinalizeSuccessfulPlayAsync(List<CardViewModel> cardsToPlay, bool isUsingDiscardCard)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                PlayerMelds.Add(new MeldViewModel(cardsToPlay));
            });

            if (isUsingDiscardCard)
            {
                MessageBox.Show(Lang.GameMoveMade, Lang.TitlePay, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task RollbackPlayCards(List<CardViewModel> cardsToPlay)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                foreach (var cardVM in cardsToPlay)
                {
                    if (TopDiscardCard == null || cardVM.Id != TopDiscardCard.Id)
                    {
                        PlayerHand.Add(cardVM);
                    }
                }
            });
        }

        private void UpdateTimerDisplay(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            GameTimeDisplay = time.ToString(@"mm\:ss");
        }

        private void UpdateTurnStatus(int newTurnPlayerId)
        {
            if (CurrentPlayer == null)
            {
                return;
            }

            bool isItMyTurnNow = (newTurnPlayerId == CurrentPlayer.idPlayer);

            if (isItMyTurnNow)
            {
                TurnStatusText = Lang.GameTurn;
                if (!IsMyTurn)
                {
                    IsMyTurn = true;
                    StartTurnTimer();
                }
            }
            else
            {
                TurnStatusText = Lang.GameOpponentsturn;
                if (IsMyTurn)
                {
                    IsMyTurn = false;
                    IsStockPileBlinking = false;
                    StopTurnTimer();
                }
            }
        }

        public async Task DrawFromDeckAsync()
        {
            if (client == null || !IsMyTurn || HasJustDrawnFromDeck)
            {
                return;
            }

            IsStockPileBlinking = false;
            CanDiscard = true;

            try
            {
                await client.DrawFromDeckAsync(roomCode, CurrentPlayer.idPlayer);
                HasJustDrawnFromDeck = true;
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        public async Task DrawFromDiscardAsync(CardDto card)
        {
            if (card == null || client == null || !IsMyTurn)
            {
                return;
            }

            var selectedCards = PlayerHand.Where(c => c.IsSelected).ToList();

            if (selectedCards.Count + 1 > MINIMUM_MELD_SIZE)
            {
                MessageBox.Show(Lang.GameInvalidMeld, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (HasJustDrawnFromDeck && selectedCards.Count == SINGLE_CARD_COUNT)
            {
                await ExecuteSwapDrawnCardAsync(card, selectedCards[0]);
            }
            else
            {
                await ExecuteMeldFromDiscardAsync(card, selectedCards);
            }
        }

        private async Task ExecuteSwapDrawnCardAsync(CardDto newCard, CardViewModel cardToPay)
        {
            try
            {
                await client.SwapDrawnCardAsync(roomCode, CurrentPlayer.idPlayer, cardToPay.Id);

                PlayerHand.Remove(cardToPay);
                PlayerHand.Add(new CardViewModel(newCard));
                TopDiscardCard = cardToPay.Card;

                ResetTurnState();
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private async Task ExecuteMeldFromDiscardAsync(CardDto card, List<CardViewModel> selectedCards)
        {
            if (selectedCards.Count < MINIMUM_CARDS_FOR_MELD_FROM_HAND)
            {
                string msg = HasJustDrawnFromDeck ? Lang.GameSwapInstruction : Lang.GameInvalidMove;
                MessageBox.Show(msg, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var cardIdsToPlay = selectedCards.Select(c => c.Id).ToList();
            cardIdsToPlay.Add(card.Id);

            await PlayCardsAsync(cardIdsToPlay, () =>
            {
                if (!isGameEnded)
                {
                    HasJustDrawnFromDeck = false;
                    CanDiscard = true;
                }
            });
        }

        public async Task DiscardCardAsync(CardViewModel cardVM)
        {
            StopTurnTimer();

            if (cardVM == null || client == null || !IsMyTurn || !CanDiscard)
            {
                return;
            }

            try
            {
                await client.DiscardCardAsync(roomCode, CurrentPlayer.idPlayer, cardVM.Id);

                PlayerHand.Remove(cardVM);
                TopDiscardCard = cardVM.Card;
                CanDiscard = false;

                foreach (var c in PlayerHand)
                {
                    c.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private void InitializeAFKTimer()
        {
            activityTimer = new DispatcherTimer();
            activityTimer.Interval = TimeSpan.FromSeconds(1);
            activityTimer.Tick += ActivityTimerTick;
        }

        private void ActivityTimerTick(object sender, EventArgs e)
        {
            if (isNavigatingAway || isGameEnded)
            {
                StopTurnTimer();
                return;
            }

            if (!IsMyTurn)
            {
                return;
            }

            var timeSinceActivity = afkStopwatch.Elapsed;

            if (!isWarningShown && timeSinceActivity.TotalSeconds >= INACTIVITY_LIMIT_SECONDS)
            {
                ShowAFKWarning();
            }

            if (isWarningShown && timeSinceActivity.TotalSeconds >= (INACTIVITY_LIMIT_SECONDS + GRACE_PERIOD_SECONDS))
            {
                activityTimer.Stop();
                ReportSelfAFK();
            }
        }

        public void OnUserActivity()
        {
            if (!isWarningShown)
            {
                afkStopwatch.Restart();
            }
        }

        private void OnAcceptAFK(object obj)
        {
            IsAFKWarningVisible = false;
        }

        private void ShowAFKWarning()
        {
            isWarningShown = true;
            IsAFKWarningVisible = true;
        }

        private void ReportSelfAFK()
        {
            IsAFKWarningVisible = false;
            try
            {
                client.ReportAFK(roomCode, CurrentPlayer.idPlayer);
            }
            catch (Exception)
            {
                ReturnToMainMenu();
            }
        }

        public void StartTurnTimer()
        {
            afkStopwatch.Restart();
            isWarningShown = false;
            IsAFKWarningVisible = false;
            activityTimer.Start();
        }

        public void StopTurnTimer()
        {
            activityTimer.Stop();
            IsAFKWarningVisible = false;
            isWarningShown = false;
        }

        private static void ReturnToMainMenu()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is View.MainMenu.MainMenu);
                if (mainWindow == null)
                {
                    var menu = new View.MainMenu.MainMenu();
                    menu.Show();
                }
                Application.Current.Windows.OfType<View.Game.Game>().FirstOrDefault()?.Close();
            });
        }

        private void HandleServiceException(Exception ex)
        {
            if (ex is FaultException<ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ex is EndpointNotFoundException || ex is CommunicationException || ex is TimeoutException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(Lang.ErrorGeneric);
            }
        }
    }
}