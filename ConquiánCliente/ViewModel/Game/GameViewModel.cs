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
        private string gameTimeDisplay;
        public string GameTimeDisplay
        {
            get { return gameTimeDisplay; }
            set { gameTimeDisplay = value; OnPropertyChanged(nameof(GameTimeDisplay)); }
        }

        private string turnTimeDisplay;
        public string TurnTimeDisplay
        {
            get { return turnTimeDisplay; }
            set { turnTimeDisplay = value; OnPropertyChanged(nameof(TurnTimeDisplay)); }
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

        public GameViewModel(string roomCode)
        {
            this.roomCode = roomCode;

            PlayerHand = new ObservableCollection<CardViewModel>();
            OpponentFaceDownCards = new ObservableCollection<object>();
            PlayerMelds = new ObservableCollection<MeldViewModel>();
            OpponentMelds = new ObservableCollection<MeldViewModel>();
            TemporaryMeld = new ObservableCollection<CardViewModel>();

            var sessionPlayer = PlayerSession.CurrentPlayer;

            CurrentPlayer = new ServiceGame.PlayerDto
            {
                idPlayer = sessionPlayer.idPlayer,
                nickname = sessionPlayer.nickname,
                pathPhoto = sessionPlayer.pathPhoto
            };
            _ = InitializeGameConnectionAsync();
        }

        private async Task InitializeGameConnectionAsync()
        {
            try
            {
                var callbackHandler = new GameCallbackHandler();
                callbackHandler.OnOpponentDiscarded += (card) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        TopDiscardCard = card;
                    });
                };


                callbackHandler.OnOpponentDrewDeck += () => {
                    Application.Current.Dispatcher.Invoke(() => {
                    });
                };

                callbackHandler.TimeStateUpdated += (gameSeconds, turnSeconds, newTurnPlayerId) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        UpdateTimerDisplay(gameSeconds);
                        UpdateTurnTimerDisplay(turnSeconds);
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
                            TurnStatusText = "Turno del oponente";
                        }

                        IsMyTurn = (gameState.CurrentTurnPlayerId == playerId);
                        UpdateTimerDisplay(gameState.TotalGameSeconds);
                    });
                }
                else
                {
                    MessageBox.Show(Lang.ErrorGeneric, "Error de juego", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}",
                                Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (client == null || CurrentPlayer == null || cardIds == null || cardIds.Count < 3) return;

            var cardsToPlay = PlayerHand.Where(vm => cardIds.Contains(vm.Id)).ToList();

            if (cardsToPlay.Count != cardIds.Count) return;

            if (IsValidMeld(cardsToPlay))
            {
                foreach (var cardVM in cardsToPlay)
                {
                    PlayerHand.Remove(cardVM);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}",
                                    Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);

                    await Application.Current.Dispatcher.InvokeAsync(() => {
                        TemporaryMeld.Clear();
                        foreach (var cardVM in cardsToPlay)
                        {
                            PlayerHand.Add(cardVM);
                        }
                    });
                    return;
                }

                await Task.Delay(1000);

                await Application.Current.Dispatcher.InvokeAsync(() => {
                    TemporaryMeld.Clear();
                    PlayerMelds.Add(new MeldViewModel(cardsToPlay));
                });
            }
            else
            {
                MessageBox.Show(Lang.GameInvalidMeld, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private static bool IsValidMeld(List<CardViewModel> cards)
        {
            if (cards == null || cards.Count < 3) return false;

            cards = cards.OrderBy(c => c.Rank).ToList();

            bool isTercia = cards.All(c => c.Rank == cards[0].Rank);
            bool distinctSuits = cards.Select(c => c.Suit).Distinct().Count() == cards.Count;
            if (isTercia && distinctSuits)
            {
                return true;
            }

            bool isCorrida = cards.All(c => c.Suit == cards[0].Suit);
            if (!isCorrida) return false;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                int currentRank = cards[i].Rank;
                int nextRank = cards[i + 1].Rank;

                if (currentRank == 7 && nextRank == 10)
                {
                    continue;
                }

                if (nextRank != currentRank + 1)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateTimerDisplay(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            GameTimeDisplay = time.ToString(@"mm\:ss");
        }

        private void UpdateTurnTimerDisplay(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            TurnTimeDisplay = time.ToString(@"ss"); 
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
                TurnStatusText = "Turno del oponente";
                IsMyTurn = false;
            }
        }

        public async Task DrawFromDeckAsync()
        {
            if (client == null || !IsMyTurn) return;

            try
            {
                await client.DrawFromDeckAsync(roomCode, CurrentPlayer.idPlayer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}",
                                Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task DrawFromDiscardAsync(CardDto card)
        {
            if (card == null || client == null || !IsMyTurn) return;

            try
            {
                CardDto drawnCard = await client.DrawFromDiscardAsync(roomCode, CurrentPlayer.idPlayer);

                if (drawnCard != null && drawnCard.Id == card.Id)
                {
                    PlayerHand.Add(new CardViewModel(drawnCard));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}",
                                Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task DiscardCardAsync(CardViewModel cardVM)
        {
            if (cardVM == null || client == null || !IsMyTurn) return;

            try
            {
                await client.DiscardCardAsync(roomCode, CurrentPlayer.idPlayer, cardVM.Id);

                PlayerHand.Remove(cardVM);
                TopDiscardCard = cardVM.Card;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}",
                                Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}