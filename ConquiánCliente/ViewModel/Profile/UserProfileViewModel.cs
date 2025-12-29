using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.View;
using ConquiánCliente.View.Profile;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Profile
{
    public class UserProfileViewModel : ViewModelBase
    {
        private string profileImagePath;
        private PlayerDto fullPlayerProfile;

        private ObservableCollection<GameHistoryDto> matchHistoryList;
        public ObservableCollection<GameHistoryDto> MatchHistoryList
        {
            get => matchHistoryList;
            set { matchHistoryList = value; OnPropertyChanged(); }
        }

        private bool isHistoryEmpty;
        public bool IsHistoryEmpty
        {
            get => isHistoryEmpty;
            set { isHistoryEmpty = value; OnPropertyChanged(); }
        }

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

        private int currentPoints;
        public int CurrentPoints
        {
            get => currentPoints;
            set
            {
                currentPoints = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PointsDisplay));
            }
        }

        public string PointsDisplay
        {
            get
            {
                return $"{CurrentPoints} / {Level * 100}";
            }
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

        private int level;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PointsDisplay));
            }
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
            MatchHistoryList = new ObservableCollection<GameHistoryDto>();
            IsHistoryEmpty = true;

            NavigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            NavigateToEditCommand = new RelayCommand(ExecuteNavigateToEdit);
            NavigateToEditProfilePictureCommand = new RelayCommand(ExecuteNavigateToEditProfilePicture);
            _ = LoadPlayerData();
        }

        private async Task LoadPlayerData()
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
                        Level = fullPlayerProfile.idLevel;
                        CurrentPoints = fullPlayerProfile.currentPoints;

                        string serverImageName = System.IO.Path.GetFileName(fullPlayerProfile.pathPhoto);
                        SetProfileImage(serverImageName);

                        PlayerSession.UpdateSession(fullPlayerProfile);
                    }

                    var socials = await userProfileClient.GetPlayerSocialsAsync(sessionPlayer.idPlayer);
                    if (socials.Any())
                    {
                        Facebook = socials.FirstOrDefault(s => s.IdSocialType == 2)?.UserLink;
                        Instagram = socials.FirstOrDefault(s => s.IdSocialType == 1)?.UserLink;
                    }

                    var history = await userProfileClient.GetPlayerGameHistoryAsync(sessionPlayer.idPlayer);

                    MatchHistoryList.Clear();
                    if (history != null && history.Length > 0)
                    {
                        foreach (var game in history)
                        {
                            MatchHistoryList.Add(game);
                        }
                        IsHistoryEmpty = false;
                    }
                    else
                    {
                        IsHistoryEmpty = true;
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


        private static void ExecuteNavigateBack(object parameter)
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