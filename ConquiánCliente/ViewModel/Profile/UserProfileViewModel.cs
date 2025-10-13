using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.View;
using ConquiánCliente.View.MainMenu;
using ConquiánCliente.View.Profile;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConquiánCliente.Properties.Langs;
using System;
using System.Linq; 

namespace ConquiánCliente.ViewModel.Profile
{
    public class UserProfileViewModel : ViewModelBase
    {
        // Propiedades para enlazar con la Vista (XAML)
        private string _profileImagePath;
        private PlayerDto _fullPlayerProfile;

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

        private string _facebook;
        public string Facebook
        {
            get => _facebook;
            set { _facebook = value; OnPropertyChanged(); }
        }

        private string _instagram;
        public string Instagram
        {
            get => _instagram;
            set { _instagram = value; OnPropertyChanged(); }
        }


        public ICommand NavigateToEditCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand NavigateToEditProfilePictureCommand { get; }

        public UserProfileViewModel()
        {
            NavigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            NavigateToEditCommand = new RelayCommand(ExecuteNavigateToEdit);
            NavigateToEditProfilePictureCommand = new RelayCommand(ExecuteNavigateToEditProfilePicture);

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

                    _fullPlayerProfile = await userProfileClient.GetPlayerByIdAsync(sessionPlayer.idPlayer);

                    if (_fullPlayerProfile != null)
                    {
                        Email = _fullPlayerProfile.email;
                        Name = _fullPlayerProfile.name;
                        LastName = _fullPlayerProfile.lastName;
                        Level = _fullPlayerProfile.level?.ToString() ?? "1";

                        string serverImageName = System.IO.Path.GetFileName(_fullPlayerProfile.pathPhoto);
                        SetProfileImage(serverImageName);

                        PlayerSession.UpdateSession(_fullPlayerProfile);
                    }

                    var socials = await userProfileClient.GetPlayerSocialsAsync(sessionPlayer.idPlayer);
                    if (socials != null)
                    {
                        Facebook = socials.FirstOrDefault(s => s.IdSocialType == 2)?.UserLink;
                        Instagram = socials.FirstOrDefault(s => s.IdSocialType == 1)?.UserLink;
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
            var currentPlayerDto = PlayerSession.CurrentPlayer;

            var editInfoViewModel = new EditInfoViewModel(_fullPlayerProfile);

            var editInfoPage = new EditInfoPage
            {
                DataContext = editInfoViewModel
            };

            ProfileMainFrame.MainFrame.Navigate(editInfoPage);
        }
        private void ExecuteNavigateToEditProfilePicture(object obj)
        {
            EditProfilePicture editProfilePicture = new EditProfilePicture();
            editProfilePicture.ShowDialog(); 

            if (PlayerSession.IsLoggedIn)
            {
                string serverImageName = System.IO.Path.GetFileName(PlayerSession.CurrentPlayer.pathPhoto);
                SetProfileImage(serverImageName);
            }
        }
    }
}