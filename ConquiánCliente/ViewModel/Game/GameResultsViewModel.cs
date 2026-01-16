using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceGame;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Game
{
    public class GameResultsViewModel : ViewModelBase
    {
        private string playerName;
        private int playerScore;
        private string playerImage; 
        private string opponentName;
        private int opponentScore;
        private string opponentImage; 
        private string resultTitle;
        private string resultDetails;

        private const int ZERO_SCORE = 0;

        public string ResultTitle
        {
            get { return resultTitle; }
            set { resultTitle = value; OnPropertyChanged(nameof(ResultTitle)); }
        }

        public string ResultDetails
        {
            get { return resultDetails; }
            set { resultDetails = value; OnPropertyChanged(nameof(ResultDetails)); }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; OnPropertyChanged(nameof(PlayerScore)); }
        }

        public string PlayerImage
        {
            get { return playerImage; }
            set { playerImage = value; OnPropertyChanged(nameof(PlayerImage)); }
        }

        public string OpponentName
        {
            get { return opponentName; }
            set { opponentName = value; OnPropertyChanged(nameof(OpponentName)); }
        }

        public int OpponentScore
        {
            get { return opponentScore; }
            set { opponentScore = value; OnPropertyChanged(nameof(OpponentScore)); }
        }
        public string OpponentImage
        {
            get { return opponentImage; }
            set { opponentImage = value; OnPropertyChanged(nameof(OpponentImage)); }
        }
        public ICommand ReturnToMainMenuCommand { get; private set; }

        public GameResultsViewModel()
        {
            ReturnToMainMenuCommand = new RelayCommand(ReturnToMainMenu);
        }

        public GameResultsViewModel(GameResultDto result, int myPlayerId) : this()
        {
            LoadResultData(result, myPlayerId);
        }

        private void LoadResultData(GameResultDto result, int myPlayerId)
        {
            bool amIPlayer1 = (result.Player1Id == myPlayerId);

            SetPlayerData(result, amIPlayer1);

            bool playerIsWinner = (result.WinnerId == myPlayerId);

            if (result.IsDraw)
            {
                SetDrawResult(result.PointsWon);
            }
            else if (playerIsWinner)
            {
                SetVictoryResult(result.PointsWon);
            }
            else
            {
                SetDefeatResult(result.PointsWon);
            }
        }

        private void SetPlayerData(GameResultDto result, bool amIPlayer1)
        {
            PlayerName = amIPlayer1 ? result.Player1Name : result.Player2Name;
            OpponentName = amIPlayer1 ? result.Player2Name : result.Player1Name;
            PlayerImage = amIPlayer1 ? result.Player1PathPhoto : result.Player2PathPhoto;
            OpponentImage = amIPlayer1 ? result.Player2PathPhoto : result.Player1PathPhoto;
        }

        private void SetDrawResult(int points)
        {
            ResultTitle = Lang.GlobalGameDraw;
            ResultDetails = string.Format(Lang.ResultDrawMessage, OpponentName);
            PlayerScore = points;
            OpponentScore = points;
        }

        private void SetVictoryResult(int points)
        {
            ResultTitle = Lang.GlobalGameVictory;
            ResultDetails = string.Format(Lang.ResultWinMessage, OpponentName);
            PlayerScore = points;
            OpponentScore = ZERO_SCORE;
        }

        private void SetDefeatResult(int points)
        {
            ResultTitle = Lang.GlobalGameDefeat;
            ResultDetails = string.Format(Lang.ResultLossMessage, OpponentName);
            PlayerScore = ZERO_SCORE;
            OpponentScore = points;
        }

        private void ReturnToMainMenu(object obj)
        {
            if (PlayerSession.IsGuest)
            {
                HandleGuestExit();
            }
            else
            {
                var mainMenu = new View.MainMenu.MainMenu();
                mainMenu.Show();
            }

            CloseWindow(obj);
        }

        private void HandleGuestExit()
        {
            PlayerSession.EndSession();
            var loginWindow = new LogIn();
            loginWindow.Show();
        }

        public static string ReturnButtonText
        {
            get
            {
                return PlayerSession.IsGuest ? Lang.GameExit : Lang.GameBackToMainMenu;
            }
        }

        private void CloseWindow(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
            else
            {
                FindAndCloseSelf();
            }
        }

        private void FindAndCloseSelf()
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
    }
}