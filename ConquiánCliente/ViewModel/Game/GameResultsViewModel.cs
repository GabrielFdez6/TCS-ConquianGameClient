using ConquiánCliente.Properties.Langs;
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

        public string ResultTitle
        {
            get { return resultTitle; }
            set { resultTitle = value; OnPropertyChanged(nameof(ResultTitle)); }
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

        public ICommand BackToLobbyCommand { get; private set; }
        public ICommand ExitGameCommand { get; private set; }

        public GameResultsViewModel()
        {
            BackToLobbyCommand = new RelayCommand(BackToLobby);
            ExitGameCommand = new RelayCommand(ExitGame);
        }

        public GameResultsViewModel(string pName, int pScore, string oName, int oScore) : this()
        {
            PlayerName = pName;
            PlayerScore = pScore;
            OpponentName = oName;
            OpponentScore = oScore;

            if (pScore > oScore)
            {

                ResultTitle = Lang.GameVictory; 
            }
            else if (pScore < oScore)
            {
                ResultTitle = Lang.GameDefeat; 
            }
            else
            {
                ResultTitle = "Empate";
            }
        }

        private void BackToLobby(object obj)
        {
            var mainMenu = new ConquiánCliente.View.MainMenu.MainMenu();
            mainMenu.Show();
            CloseWindow(obj);
        }

        private void ExitGame(object obj)
        {
            Application.Current.Shutdown();
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