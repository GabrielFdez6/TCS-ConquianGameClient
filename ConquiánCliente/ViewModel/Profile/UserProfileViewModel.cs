using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.View;
using ConquiánCliente.View.MainMenu;
using ConquiánCliente.View.Profile;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConquiánCliente.Properties.Langs;

namespace ConquiánCliente.ViewModel.Profile
{
    public class UserProfileViewModel : ViewModelBase
    {
        // Propiedades para enlazar con la Vista (XAML)
        private string _profileImagePath;
        public string ProfileImagePath
        {
            get => _profileImagePath;
            set { _profileImagePath = value; OnPropertyChanged(); }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set { _nickname = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _level;
        public string Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged(); }
        }

        public ICommand NavigateToEditCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public UserProfileViewModel()
        {
            NavigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            NavigateToEditCommand = new RelayCommand(ExecuteNavigateToEdit);
            LoadPlayerData();
        }

        private async void LoadPlayerData()
        {
            if (PlayerSession.IsLoggedIn)
            {
                var sessionPlayer = PlayerSession.CurrentPlayer;
                Nickname = sessionPlayer.nickname;

                string initialImageName = System.IO.Path.GetFileName(sessionPlayer.pathPhoto);
                SetProfileImage(initialImageName);

                try
                {
                    var userProfileClient = new UserProfileClient();
                    Player fullPlayerProfile = await userProfileClient.GetPlayerProfileAsync(sessionPlayer.nickname);

                    if (fullPlayerProfile != null)
                    {
                        Email = fullPlayerProfile.email;
                        Name = fullPlayerProfile.name;
                        LastName = fullPlayerProfile.lastName;
                        Level = fullPlayerProfile.level?.ToString() ?? "1";

                        string serverImageName = System.IO.Path.GetFileName(fullPlayerProfile.pathPhoto);
                        SetProfileImage(serverImageName);

                        PlayerSession.UpdateSession(fullPlayerProfile);
                    }
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
                }
            }
        }

        private void SetProfileImage(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                string fullPath = $"pack://application:,,,/Resources/imageProfile/{imageName}";
                ProfileImagePath = fullPath;
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