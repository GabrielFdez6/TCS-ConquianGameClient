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
        private string opponentName;
        private int opponentScore;
        private string resultTitle;
        private string resultDetails;

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

            PlayerName = amIPlayer1 ? result.Player1Name : result.Player2Name;
            OpponentName = amIPlayer1 ? result.Player2Name : result.Player1Name;

            bool palyerIsWinner = (result.WinnerId == myPlayerId);
            bool isDraw = result.IsDraw;

            if (isDraw)
            {
                ResultTitle = Lang.GlobalGameDraw;
                ResultDetails = string.Format(Lang.ResultDrawMessage, OpponentName);
                PlayerScore = 0;
                OpponentScore = 0;
            }
            else if (palyerIsWinner)
            {
                ResultTitle = Lang.GlobalGameVictory;
                ResultDetails = string.Format(Lang.ResultWinMessage, OpponentName);

                PlayerScore = result.PointsWon;
                OpponentScore = 0;
            }
            else
            {
                ResultTitle = Lang.GlobalGameDefeat;
                ResultDetails = string.Format(Lang.ResultLossMessage, OpponentName);

                PlayerScore = 0;
                OpponentScore = result.PointsWon;
            }
        }

        private void ReturnToMainMenu(object obj)
        {
            var mainMenu = new ConquiánCliente.View.MainMenu.MainMenu();
            mainMenu.Show();
            CloseWindow(obj);
        }

        private void CloseWindow(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
            else
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
}