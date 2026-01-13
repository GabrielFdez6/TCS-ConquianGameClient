using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceGame;
using ConquiánCliente.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace ConquiánCliente.ViewModel.Game
{
    public class GameViewModel : ViewModelBase
    {
        private const int ANIMATION_DELAY_MS = 1000;
        private const int MINIMUM_MELD_SIZE = 3;
        private const int SINGLE_CARD_COUNT = 1;
        private const int MINIMUM_CARDS_FOR_MELD_FROM_HAND = 2;
        private const int RANK_INCREMENT = 1;

        private DispatcherTimer activityTimer;
        private readonly Stopwatch afkStopwatch;
        private bool isWarningShown;
        private const int INACTIVITY_LIMIT_SECONDS = 60;
        private const int GRACE_PERIOD_SECONDS = 60;

        private const int RANK_BEFORE_SKIP = 7;
        private const int RANK_AFTER_SKIP = 10;

        private readonly string roomCode;
        private GameClient client;
        private bool isGameEnded = false;
        private readonly IMessageResolver messageResolver;
        private bool isNavigatingAway = false;

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
            catch (FaultException<ServiceFaultDto> fault)
            {
                HandleGameFault(fault);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorGeneric);
            }
        }

        private async Task InitializeGameConnectionAsync()
        {
            try
            {
                var callbackHandler = ConfigureGameCallbacks();
                var context = new InstanceContext(callbackHandler);
                client = new GameClient(context);

                if (client.InnerChannel != null)
                {
                    client.InnerChannel.Closed += OnConnectionLost;
                    client.InnerChannel.Faulted += OnConnectionLost;
                }

                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                GameStateDto gameState = await client.JoinGameAsync(roomCode, playerId);

                if (gameState != null)
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

                        if (gameState.CurrentTurnPlayerId == playerId)
                        {
                            TurnStatusText = Lang.GameTurn;
                            IsMyTurn = true;
                            StartTurnTimer();
                        }
                        else
                        {
                            TurnStatusText = Lang.GameOpponentsturn;
                            IsMyTurn = false;
                            StopTurnTimer();
                        }
                        UpdateTimerDisplay(gameState.TotalGameSeconds);
                    });
                }
                else
                {
                    MessageBox.Show(Lang.ErrorGeneric, Lang.ErrorGame, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnConnectionLost(object sender, EventArgs e)
        {
            if (PlayerSession.IsGuest)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (isNavigatingAway) return;
                    isNavigatingAway = true;

                    StopTurnTimer();

                    MessageBox.Show(Lang.ErrorLostConnection, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);

                    var loginWindow = new LogIn();
                    loginWindow.Show();

                    CloseWindow();

                    PlayerSession.EndSession();
                });
            }
        }

        private GameCallbackHandler ConfigureGameCallbacks()
        {
            var callbackHandler = new GameCallbackHandler();

            callbackHandler.OnOpponentDiscarded += (card) => {
                Application.Current.Dispatcher.Invoke(() => TopDiscardCard = card);
            };

            callbackHandler.OnOpponentDrewDeck += () => {
                Application.Current.Dispatcher.Invoke(() => { });
            };

            callbackHandler.TimeStateUpdated += (gameSeconds, turnSeconds, newTurnPlayerId) => {
                Application.Current.Dispatcher.Invoke(() => {
                    UpdateTimerDisplay(gameSeconds);
                    UpdateTurnStatus(newTurnPlayerId);
                });
            };

            callbackHandler.OpponentHandUpdated += (newCardCount) => {
                Application.Current.Dispatcher.Invoke(() => UpdateOpponentCardCount(newCardCount));
            };

            callbackHandler.OnOpponentMeld += HandleOpponentMeld;
            callbackHandler.OnGameEnded += HandleGameEnded;
            callbackHandler.OnOpponentLeftEvent += HandleOpponentLeft;
            callbackHandler.OnGameEndedByAFKEvent += HandleGameEndedByAFK;

            if (client.InnerChannel != null)
            {
                client.InnerChannel.Closed += OnConnectionLost;
                client.InnerChannel.Faulted += OnConnectionLost;
            }

            return callbackHandler;
        }

        private async void HandleOpponentMeld(CardDto[] meldCardDtos)
        {
            var cardVMs = meldCardDtos.Select(dto => new CardViewModel(dto)).ToList();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                foreach (var cardVM in cardVMs)
                {
                    TemporaryMeld.Add(cardVM);
                }
            });
            await Task.Delay(ANIMATION_DELAY_MS);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                TemporaryMeld.Clear();
                OpponentMelds.Add(new MeldViewModel(cardVMs));
            });
        }

        private void HandleGameEnded(GameResultDto results)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!PlayerSession.IsLoggedIn || isNavigatingAway)
                {
                    return;
                }

                isNavigatingAway = true;
                isGameEnded = true;
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
                NavigateToMainMenu();
            });
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

                foreach (Window win in Application.Current.Windows)
                {
                    if (win.DataContext == this)
                    {
                        win.Close();
                        break;
                    }
                }
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

        public async Task<bool> PlayCardsAsync(List<string> cardIds)
        {
            bool executionResult = false;

            if (!CanPlayCards(cardIds) || cardIds.Count > MINIMUM_MELD_SIZE)
            {
                executionResult = false;
                return executionResult;
            }

            bool isUsingDiscardCard = TopDiscardCard != null && cardIds.Contains(TopDiscardCard.Id);

            var cardsToPlay = GetCardsToPlay(cardIds);

            if (cardsToPlay.Count != cardIds.Count)
            {
                executionResult = false;
                return executionResult;
            }

            if (!IsValidMeld(cardsToPlay))
            {
                MessageBox.Show(Lang.GameInvalidMeld, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                executionResult = false;
                return executionResult;
            }

            await MoveCardsToTemporaryMeldAsync(cardsToPlay);

            executionResult = await TryExecutePlayCardsAsync(cardsToPlay, cardIds, isUsingDiscardCard);
            return executionResult;
        }

        private bool CanPlayCards(List<string> cardIds)
        {
            bool isPlayable = true;
            if (client == null || CurrentPlayer == null || cardIds == null)
            {
                isPlayable = false;
            }
            return isPlayable;
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
            bool isSuccess = false;
            try
            {
                await client.PlayCardsAsync(roomCode, CurrentPlayer.idPlayer, cardIds.ToArray());
                await Task.Delay(ANIMATION_DELAY_MS);

                if (isGameEnded)
                {
                    isSuccess = true;
                    return isSuccess;
                }

                await FinalizeSuccessfulPlayAsync(cardsToPlay, isUsingDiscardCard);
                isSuccess = true;
            }
            catch (FaultException<ServiceFaultDto> fault)
            {
                HandleGameFault(fault);
                await RollbackPlayCards(cardsToPlay);
                isSuccess = false;
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
                await RollbackPlayCards(cardsToPlay);
                isSuccess = false;
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                await RollbackPlayCards(cardsToPlay);
                isSuccess = false;
            }
            return isSuccess;
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
            await Application.Current.Dispatcher.InvokeAsync(() => {
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

        private static bool IsValidMeld(List<CardViewModel> cards)
        {
            bool isValid = false;

            if (cards == null || cards.Count < MINIMUM_MELD_SIZE || cards.Count > MINIMUM_MELD_SIZE)
            {
                isValid = false;
                return isValid;
            }

            cards = cards.OrderBy(c => c.Rank).ToList();

            bool isTercia = cards.All(c => c.Rank == cards[0].Rank);
            bool distinctSuits = cards.Select(c => c.Suit).Distinct().Count() == cards.Count;

            if (isTercia && distinctSuits)
            {
                isValid = true;
                return isValid;
            }

            bool isCorrida = cards.All(c => c.Suit == cards[0].Suit);

            if (!isCorrida)
            {
                isValid = false;
                return isValid;
            }

            bool isSequential = true;
            for (int i = 0; i < cards.Count - 1; i++)
            {
                int currentRank = cards[i].Rank;
                int nextRank = cards[i + 1].Rank;

                if (currentRank == RANK_BEFORE_SKIP && nextRank == RANK_AFTER_SKIP)
                {
                    continue;
                }

                if (nextRank != currentRank + RANK_INCREMENT)
                {
                    isSequential = false;
                    break;
                }
            }

            isValid = isSequential;
            return isValid;
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
            catch (FaultException<ServiceFaultDto> fault)
            {
                HandleGameFault(fault);
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer);
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
                var cardToPay = selectedCards[0];
                try
                {
                    await client.SwapDrawnCardAsync(roomCode, CurrentPlayer.idPlayer, cardToPay.Id);

                    PlayerHand.Remove(cardToPay);
                    PlayerHand.Add(new CardViewModel(card));
                    TopDiscardCard = cardToPay.Card;

                    HasJustDrawnFromDeck = false;
                    CanDiscard = false;

                    foreach (var c in PlayerHand)
                    {
                        c.IsSelected = false;
                    }
                }
                catch (FaultException<ServiceFaultDto> fault)
                {
                    HandleGameFault(fault);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Lang.ErrorGeneric}: {ex.Message}");
                }
                return;
            }

            if (selectedCards.Count < MINIMUM_CARDS_FOR_MELD_FROM_HAND)
            {
                string msg = HasJustDrawnFromDeck ? Lang.GameSwapInstruction : Lang.GameInvalidMove;
                MessageBox.Show(msg, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var cardIdsToPlay = selectedCards.Select(c => c.Id).ToList();
            cardIdsToPlay.Add(card.Id);

            bool playSuccessful = await PlayCardsAsync(cardIdsToPlay);

            if (playSuccessful && !isGameEnded)
            {
                HasJustDrawnFromDeck = false;
                CanDiscard = true;
            }
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
            catch (FaultException<ServiceFaultDto> fault)
            {
                HandleGameFault(fault);
            }
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer);
            }
        }

        private void HandleGameFault(FaultException<ServiceFaultDto> fault)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);

            MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void InitializeAFKTimer()
        {
            activityTimer = new DispatcherTimer();
            activityTimer.Interval = TimeSpan.FromSeconds(1);
            activityTimer.Tick += ActivityTimerTick;
        }

        private void ActivityTimerTick(object sender, EventArgs e)
        {
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

        public void OnGameEndedByAFK(string reasonKey)
        {
            activityTimer.Stop();
            IsAFKWarningVisible = false;

            string message = Lang.ResourceManager.GetString(reasonKey);
            MessageBox.Show(message, Lang.TitleAuthenticationError, MessageBoxButton.OK, MessageBoxImage.Information);

            ReturnToMainMenu();
        }

        private static void ReturnToMainMenu()
        {
            Application.Current.Dispatcher.Invoke(() => {
                var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is View.MainMenu.MainMenu);
                if (mainWindow == null)
                {
                    var menu = new View.MainMenu.MainMenu();
                    menu.Show();
                }
                Application.Current.Windows.OfType<View.Game.Game>().FirstOrDefault()?.Close();
            });
        }
    }
}