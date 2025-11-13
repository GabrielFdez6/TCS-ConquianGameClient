using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceGame; // Revisa tu namespace de referencia
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

        // --- Propiedades para Bindeo (UI) ---
        public ObservableCollection<CardViewModel> PlayerHand { get; set; }

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

        private string _turnStatusText;
        public string TurnStatusText
        {
            get { return _turnStatusText; }
            set { _turnStatusText = value; OnPropertyChanged(nameof(TurnStatusText)); }
        }

        public GameViewModel(string roomCode)
        {
            this.roomCode = roomCode;

            // Inicializar propiedades
            PlayerHand = new ObservableCollection<CardViewModel>();
            var sessionPlayer = PlayerSession.CurrentPlayer;

            // 2. Convertimos/Mapeamos al DTO del servicio de Game
            CurrentPlayer = new ServiceGame.PlayerDto
            {
                idPlayer = sessionPlayer.idPlayer,
                nickname = sessionPlayer.nickname,
                pathPhoto = sessionPlayer.pathPhoto
                // Asegúrate de copiar otras propiedades si las necesitas
            };
            // Conectar al servicio
            _ = InitializeGameConnectionAsync();
        }

        private async Task InitializeGameConnectionAsync()
        {
            try
            {
                callbackHandler = new GameCallbackHandler();

                // Suscribirse a eventos del callback
                callbackHandler.OnOpponentDiscarded += (card) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        TopDiscardCard = card;
                        TurnStatusText = Lang.GameTurn; // Es tu turno
                    });
                };

                // ... (suscribirse a otros eventos) ...

                var context = new InstanceContext(callbackHandler);
                client = new GameClient(context); // Esto fallaba antes

                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                GameStateDto gameState = await client.JoinGameAsync(roomCode, playerId);

                // Poblar el ViewModel con el estado inicial del juego
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

                        if (gameState.CurrentTurnPlayerId == playerId)
                        {
                            TurnStatusText = Lang.GameTurn; // "Es tu turno"
                        }
                        else
                        {
                            TurnStatusText = "Turno del oponente"; // "Turno del oponente"
                        }
                    });
                }
                else
                {
                    // Error: el juego no se encontró en el servidor
                    MessageBox.Show(Lang.ErrorGeneric, "Error de juego", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // ESTE ES EL ERROR QUE PROBABLEMENTE TENÍAS
                MessageBox.Show($"{Lang.ErrorConnectingToServer}: {ex.Message}",
                                Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                // Aquí deberías navegar de vuelta al menú principal
            }
        }

        // --- Comandos del Juego (Ej. Robar, Descartar) ---
        // (Los implementaremos después)
    }
}