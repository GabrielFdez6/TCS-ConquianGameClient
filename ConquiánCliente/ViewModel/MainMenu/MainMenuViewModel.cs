using ConquiánCliente.ServiceLogin;
using ConquiánCliente.View;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.MainMenu
{
    public class MainMenuViewModel : ViewModelBase
    {
        public string Nickname { get; set; }
        public string ProfileImagePath { get; set; }

        public ICommand ViewProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainMenuViewModel()
        {
            LoadPlayerData();
            ViewProfileCommand = new RelayCommand(p => ExecuteViewProfileCommand(p));
            LogoutCommand = new RelayCommand(p => ExecuteLogoutCommand(p));
        }

        private void LoadPlayerData()
        {
            if (PlayerSession.IsLoggedIn)
            {
                Nickname = PlayerSession.CurrentPlayer.nickname;
                ProfileImagePath = PlayerSession.CurrentPlayer.pathPhoto;
            }
        }

        private void ExecuteViewProfileCommand(object parameter)
        {
            ProfileMainFrame userProfileView = new ProfileMainFrame();
            userProfileView.Show();
            (parameter as Window)?.Close();
        }

        private void ExecuteLogoutCommand(object parameter)
        {
            PlayerSession.EndSession();
            var loginWindow = new LogIn();
            loginWindow.Show();
            (parameter as Window)?.Close();
        }
    }
}