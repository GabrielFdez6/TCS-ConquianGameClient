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
        private string profileImagePath;
        private PlayerDto fullPlayerProfile;

        public string ProfileImagePath
        {
            get => profileImagePath;
            set { profileImagePath = value; OnPropertyChanged(); }
        }

        private string nickname;
        public string Nickname
        {
            get => nickname;
            set { nickname = value; OnPropertyChanged(); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }

        private string level;
        public string Level
        {
            get => level;
            set { level = value; OnPropertyChanged(); }
        }

        private string facebook;
        public string Facebook
        {
            get => facebook;
            set { facebook = value; OnPropertyChanged(); }
        }

        private string instagram;
        public string Instagram
        {
            get => instagram;
            set { instagram = value; OnPropertyChanged(); }
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

                    fullPlayerProfile = await userProfileClient.GetPlayerByIdAsync(sessionPlayer.idPlayer);

                    if (fullPlayerProfile.idPlayer > 0)
                    {
                        Email = fullPlayerProfile.email;
                        Name = fullPlayerProfile.name;
                        LastName = fullPlayerProfile.lastName;
                        Level = fullPlayerProfile.level?.ToString() ?? "1";

                        string serverImageName = System.IO.Path.GetFileName(fullPlayerProfile.pathPhoto);
                        SetProfileImage(serverImageName);

                        PlayerSession.UpdateSession(fullPlayerProfile);
                    }

                    var socials = await userProfileClient.GetPlayerSocialsAsync(sessionPlayer.idPlayer);
                    if (socials.Count() > 0)
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

            var editInfoViewModel = new EditInfoViewModel(fullPlayerProfile);

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