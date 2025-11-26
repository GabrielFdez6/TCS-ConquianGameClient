using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceGame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace ConquiánCliente.ViewModel.Game
{
    public class GameViewModel : ViewModelBase
    {
        private readonly string roomCode;
        private GameClient client;
        private bool hasJustDrawnFromDeck = false;
        public RelayCommand PassTurnCommand { get; set; }
        private string gameTimeDisplay;
        public string GameTimeDisplay
        {
            get { return gameTimeDisplay; }
            set { gameTimeDisplay = value; OnPropertyChanged(nameof(GameTimeDisplay)); }
        }

        public ObservableCollection<CardViewModel> PlayerHand { get; set; }
        public ObservableCollection<object> OpponentFaceDownCards { get; set; }
        public ObservableCollection<MeldViewModel> PlayerMelds { get; set; }
        public ObservableCollection<MeldViewModel> OpponentMelds { get; set; }
        public ObservableCollection<CardViewModel> TemporaryMeld { get; set; }

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
        public GameViewModel(string roomCode)
        {
            this.roomCode = roomCode;

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
            _ = InitializeGameConnectionAsync();
        }

        public void LeaveGame()
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
                Console.WriteLine(Lang.ErrorGeneric);
            }
        }

        public async Task PassTurnAsync()
        {
            if (client == null || !IsMyTurn)
            {
                return;
            }

            try
            {
                await client.PassTurnAsync(roomCode, CurrentPlayer.idPlayer);

                hasJustDrawnFromDeck = false;

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
                MessageBox.Show(fault.Detail.Message, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
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
                        }
                        else
                        {
                            TurnStatusText = Lang.GameOpponentsturn;
                        }

                        IsMyTurn = (gameState.CurrentTurnPlayerId == playerId);
                        UpdateTimerDisplay(gameState.TotalGameSeconds);
                    });
                }
                else
                {
                    MessageBox.Show(Lang.ErrorGeneric, Lang.ErrorGame, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}", Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private GameCallbackHandler ConfigureGameCallbacks()
        {
            var callbackHandler = new GameCallbackHandler();

            callbackHandler.OnOpponentDiscarded += (card) => {
                Application.Current.Dispatcher.Invoke(() => {
                    TopDiscardCard = card;
                });
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
                Application.Current.Dispatcher.Invoke(() => {
                    UpdateOpponentCardCount(newCardCount);
                });
            };

            callbackHandler.OnOpponentMeld += async (meldCardDtos) => {
                var cardVMs = meldCardDtos.Select(dto => new CardViewModel(dto)).ToList();
                await Application.Current.Dispatcher.InvokeAsync(() => {
                    TemporaryMeld.Clear();
                    foreach (var cardVM in cardVMs)
                    {
                        TemporaryMeld.Add(cardVM);
                    }
                });
                await Task.Delay(1000);
                await Application.Current.Dispatcher.InvokeAsync(() => {
                    TemporaryMeld.Clear();
                    OpponentMelds.Add(new MeldViewModel(cardVMs));
                });
            };

            callbackHandler.OnGameEnded += (result) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ShowGameResults(result);
                });
            };

            callbackHandler.OnOpponentLeftEvent += () =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Lang.GameOpponentLeft, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigateToMainMenu();
                });
            };

            return callbackHandler;
        }

        private void NavigateToMainMenu()
        {
            var mainMenu = new ConquiánCliente.View.MainMenu.MainMenu();
            mainMenu.Show();

            foreach (Window win in Application.Current.Windows)
            {
                if (win.DataContext == this)
                {
                    win.Close();
                    break;
                }
            }
        }

        private void ShowGameResults(GameResultDto result)
        {
            string myName = CurrentPlayer.nickname;
            string opponentName = Opponent.nickname;

            int myScore = 0;
            int opponentScore = 0;

            if (result.IsDraw)
            {
                myScore = 0;
                opponentScore = 0;
            }
            else
            {
                if (result.WinnerId == CurrentPlayer.idPlayer)
                {
                    myScore = result.PointsWon; 
                    opponentScore = 0;
                }
                else
                {
                    myScore = 0;
                    opponentScore = result.PointsWon; 
                }
            }

            var resultsVM = new GameResultsViewModel(myName, myScore, opponentName, opponentScore);
            var resultsWindow = new ConquiánCliente.View.Game.GameResults();
            resultsWindow.DataContext = resultsVM;
            resultsWindow.Show();

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
        public async Task PlayCardsAsync(List<string> cardIds)
        {
            if (client == null || CurrentPlayer == null || cardIds == null)
            {
                return;
            }
            bool isUsingDiscardCard = (TopDiscardCard != null && cardIds.Contains(TopDiscardCard.Id));
            var cardsToPlay = PlayerHand.Where(vm => cardIds.Contains(vm.Id)).ToList();

            if (TopDiscardCard != null && cardIds.Contains(TopDiscardCard.Id))
            {
                var discardVM = new CardViewModel(TopDiscardCard);
                cardsToPlay.Add(discardVM);
            }

            if (cardsToPlay.Count != cardIds.Count)
            {
                return;
            }

            if (IsValidMeld(cardsToPlay))
            {
                foreach (var cardVM in cardsToPlay.ToList())
                {
                    if (PlayerHand.Contains(cardVM))
                    {
                        PlayerHand.Remove(cardVM);
                    }
                }

                await Application.Current.Dispatcher.InvokeAsync(() => {
                    TemporaryMeld.Clear();
                    foreach (var cardVM in cardsToPlay)
                    {
                        TemporaryMeld.Add(cardVM);
                    }
                });

                try
                {
                    await client.PlayCardsAsync(roomCode, CurrentPlayer.idPlayer, cardIds.ToArray());

                    await Task.Delay(1000);
                    await Application.Current.Dispatcher.InvokeAsync(() => {
                        TemporaryMeld.Clear();
                        PlayerMelds.Add(new MeldViewModel(cardsToPlay));
                    });

                    if (isUsingDiscardCard)
                    {
                        MessageBox.Show(Lang.GameMoveMade,Lang.TitlePay, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (FaultException<ServiceFaultDto> fault)
                {
                    MessageBox.Show(fault.Detail.Message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
                    await RollbackPlayCards(cardsToPlay);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}", Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    await RollbackPlayCards(cardsToPlay);
                }
            }
            else
            {
                MessageBox.Show(Lang.GameInvalidMeld, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (cards == null || cards.Count < 3) return false;

            cards = cards.OrderBy(c => c.Rank).ToList();

            bool isTercia = cards.All(c => c.Rank == cards[0].Rank);
            bool distinctSuits = cards.Select(c => c.Suit).Distinct().Count() == cards.Count;
            if (isTercia && distinctSuits) return true;

            bool isCorrida = cards.All(c => c.Suit == cards[0].Suit);
            if (!isCorrida) return false;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                int currentRank = cards[i].Rank;
                int nextRank = cards[i + 1].Rank;

                if (currentRank == 7 && nextRank == 10) continue;

                if (nextRank != currentRank + 1) return false;
            }

            return true;
        }

        private void UpdateTimerDisplay(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            GameTimeDisplay = time.ToString(@"mm\:ss");
        }

        private void UpdateTurnStatus(int newTurnPlayerId)
        {
            if (CurrentPlayer == null) return;

            if (newTurnPlayerId == CurrentPlayer.idPlayer)
            {
                TurnStatusText = Lang.GameTurn;
                IsMyTurn = true;
            }
            else
            {
                TurnStatusText = Lang.GameOpponentsturn;
                IsMyTurn = false;
                IsStockPileBlinking = false; 
            }
        }

        public async Task DrawFromDeckAsync()
        {
            if (client == null || !IsMyTurn)
            {
                return;
            }

            IsStockPileBlinking = false;

            try
            {
                await client.DrawFromDeckAsync(roomCode, CurrentPlayer.idPlayer);
                hasJustDrawnFromDeck = true;
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

            if (hasJustDrawnFromDeck && selectedCards.Count == 1)
            {
                var cardToPay = selectedCards.First();
                try
                {
                    await client.SwapDrawnCardAsync(roomCode, CurrentPlayer.idPlayer, cardToPay.Id);

                    PlayerHand.Remove(cardToPay);            
                    PlayerHand.Add(new CardViewModel(card)); 
                    TopDiscardCard = cardToPay.Card;        

                    hasJustDrawnFromDeck = false;
                    foreach (var c in PlayerHand) c.IsSelected = false;
                }
                catch (FaultException<ServiceFaultDto> fault)
                {
                    MessageBox.Show(fault.Detail.Message, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Lang.ErrorGeneric}: {ex.Message}");
                }
                return;
            }

            if (selectedCards.Count < 2)
            {
                string msg;
                if (hasJustDrawnFromDeck)
                {
                    msg = "Para cambiar la carta, selecciona 1 carta de tu mano.\nPara bajar juego, selecciona 2 o más.";
                }
                else
                {
                    msg = Lang.GameInvalidMove;
                }

                MessageBox.Show(msg, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var cardIdsToPlay = selectedCards.Select(c => c.Id).ToList();
            cardIdsToPlay.Add(card.Id);

            await PlayCardsAsync(cardIdsToPlay);

            hasJustDrawnFromDeck = false;
        }

        public async Task DiscardCardAsync(CardViewModel cardVM)
        {
            if (cardVM == null || client == null || !IsMyTurn) return;

            try
            {
                await client.DiscardCardAsync(roomCode, CurrentPlayer.idPlayer, cardVM.Id);

                PlayerHand.Remove(cardVM);
                TopDiscardCard = cardVM.Card;

                foreach (var c in PlayerHand) c.IsSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}");
            }
        }
    }
}