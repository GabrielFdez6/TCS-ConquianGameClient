using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceGame;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;


namespace ConquiánCliente.ViewModel.Game
{
    public class GameViewModel : ViewModelBase
    {
        private string roomCode;
        private GameClient client;
        private GameCallbackHandler callbackHandler;

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

        public GameViewModel(string roomCode)
        {
            this.roomCode = roomCode;

            PlayerHand = new ObservableCollection<CardViewModel>();
            OpponentFaceDownCards = new ObservableCollection<object>();
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
                callbackHandler = new GameCallbackHandler();

                callbackHandler.OnOpponentDiscarded += (card) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        TopDiscardCard = card;
                        TurnStatusText = Lang.GameTurn;
                    });
                };

                callbackHandler.TimeStateUpdated += (gameSeconds, turnSeconds, newTurnPlayerId) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        UpdateTimerDisplay(gameSeconds);
                        UpdateTurnTimerDisplay(turnSeconds);
                        UpdateTurnStatus(newTurnPlayerId); 
                    });
                };

                var context = new InstanceContext(callbackHandler);
                client = new GameClient(context);

                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                GameStateDto gameState = await client.JoinGameAsync(roomCode, playerId);

                if (gameState != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PlayerHand.Clear();
                        foreach (var cardDto in gameState.PlayerHand)
                        {
                            PlayerHand.Add(new CardViewModel(cardDto));
                        }

                        TopDiscardCard = gameState.TopDiscardCard;
                        Opponent = gameState.Opponent;

                        OpponentFaceDownCards.Clear();
                        for (int i = 0; i < gameState.OpponentCardCount; i++)
                        {
                            OpponentFaceDownCards.Add(new object());
                        }

                        if (gameState.CurrentTurnPlayerId == playerId)
                        {
                            TurnStatusText = Lang.GameTurn;
                        }
                        else
                        {
                            TurnStatusText = "Turno del oponente";
                        }

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
            }
            else
            {
                TurnStatusText = "Turno del oponente";
            }
        }
    }
}