using ConquiánCliente.View;
using ConquiánCliente.View.MainMenu;
using ConquiánCliente.View.Profile;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Profile
{
    public class UserProfileViewModel : ViewModelBase
    {
        // Propiedades para enlazar con la Vista (XAML)
        public string ProfileImagePath { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }

        public ICommand NavigateToEditCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public UserProfileViewModel()
        {
            LoadPlayerData();
            NavigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            NavigateToEditCommand = new RelayCommand(ExecuteNavigateToEdit);
        }

        private void LoadPlayerData()
        {
            if (PlayerSession.IsLoggedIn)
            {
                var currentPlayer = PlayerSession.CurrentPlayer;
                ProfileImagePath = currentPlayer.pathPhoto;
                Nickname = currentPlayer.nickname;
                Email = currentPlayer.email;
                Name = currentPlayer.name;
                LastName = currentPlayer.lastName;
                Level = currentPlayer.level?.ToString() ?? "1";
            }
        }

        private void ExecuteNavigateBack(object parameter)
        {
            var mainMenu = new View.MainMenu.MainMenu();
            mainMenu.Show();
            if (parameter is Page currentPage)
            {
                Window parentWindow = Window.GetWindow(currentPage);
                parentWindow?.Close();
            }
        }
        private void ExecuteNavigateToEdit(object parameter)
        {
            ProfileMainFrame.MainFrame.Navigate(new EditInfoPage());
        }
    }
}