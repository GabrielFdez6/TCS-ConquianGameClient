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

        private int pointsToNextLevel;
        public int PointsToNextLevel
        {
            get => pointsToNextLevel;
            set
            {
                pointsToNextLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PointsDisplay));
            }
        }

        public string PointsDisplay
        {
            get
            {
                if (PointsToNextLevel <= 0 || (CurrentPoints >= PointsToNextLevel && PointsToNextLevel != 0))
                {
                    return $"{CurrentPoints} (MAX)";
                }

                return $"{CurrentPoints} / {PointsToNextLevel}";
            }
        }

        private string rankName;
        public string RankName
        {
            get => rankName;
            set { rankName = value; OnPropertyChanged(); }
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
            NavigateToEditCommand = new RelayCommand(ExecuteNavigateToEdit, CanExecuteNavigateToEdit);
            NavigateToEditProfilePictureCommand = new RelayCommand(ExecuteNavigateToEditProfilePicture);
            _ = LoadPlayerData();
        }

        private async Task LoadPlayerData()
        {
            if (!PlayerSession.IsLoggedIn) return;

            var sessionPlayer = PlayerSession.CurrentPlayer;
            Nickname = sessionPlayer.nickname;
            SetProfileImage(System.IO.Path.GetFileName(sessionPlayer.pathPhoto));

            try
            {
                var userProfileClient = new UserProfileClient();
                await LoadUserProfileAsync(userProfileClient, sessionPlayer.idPlayer);
                await LoadUserSocialsAsync(userProfileClient, sessionPlayer.idPlayer);
                await LoadMatchHistoryAsync(userProfileClient, sessionPlayer.idPlayer);
            }
            catch (System.Exception ex)
            {
                HandleLoadDataError(ex);
            }
        }

        private async Task LoadUserProfileAsync(UserProfileClient client, int playerId)
        {
            fullPlayerProfile = await client.GetPlayerByIdAsync(playerId);

            if (fullPlayerProfile.idPlayer > 0)
            {
                Email = fullPlayerProfile.email;
                Name = fullPlayerProfile.name;
                LastName = fullPlayerProfile.lastName;
                Level = fullPlayerProfile.idLevel;
                CurrentPoints = fullPlayerProfile.currentPoints;
                PointsToNextLevel = fullPlayerProfile.PointsToNextLevel;
                RankName = fullPlayerProfile.RankName;

                SetProfileImage(System.IO.Path.GetFileName(fullPlayerProfile.pathPhoto));
                PlayerSession.UpdateSession(fullPlayerProfile);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    CommandManager.InvalidateRequerySuggested();
                });
            }
        }

        private async Task LoadUserSocialsAsync(UserProfileClient client, int playerId)
        {
            var socials = await client.GetPlayerSocialsAsync(playerId);
            if (socials.Any())
            {
                Facebook = socials.FirstOrDefault(s => s.IdSocialType == 2)?.UserLink;
                Instagram = socials.FirstOrDefault(s => s.IdSocialType == 1)?.UserLink;
            }
        }

        private async Task LoadMatchHistoryAsync(UserProfileClient client, int playerId)
        {
            var history = await client.GetPlayerGameHistoryAsync(playerId);

            MatchHistoryList.Clear();
            if (history != null && history.Length > 0)
            {
                foreach (var game in history)
                {
                    if (string.IsNullOrEmpty(game.PlayerName) || game.PlayerName == "Unknown")
                        game.PlayerName = Lang.GlobalGuess;

                    if (string.IsNullOrEmpty(game.OpponentName) || game.OpponentName == "Unknown")
                        game.OpponentName = Lang.GlobalGuess;

                    MatchHistoryList.Add(game);
                }
                IsHistoryEmpty = false;
            }
            else
            {
                IsHistoryEmpty = true;
            }
        }

        private static void HandleLoadDataError(System.Exception ex)
        {
            if (ex is EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            else if (ex is FaultException<ServiceFaultDto> faultEx)
            {
                if (faultEx.Detail.ErrorType == ServiceErrorType.DatabaseError)
                {
                    MessageBox.Show(Lang.GlobalSqlError, Lang.TitleConnectionError);
                }
                else
                {
                    MessageBox.Show(Lang.ErrorUserNotFound, Lang.TitleError);
                }
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
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
        private bool CanExecuteNavigateToEdit(object parameter)
        {
            return fullPlayerProfile != null;
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