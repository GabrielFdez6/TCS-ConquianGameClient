using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.ServiceUserProfile; 
using ConquiánCliente.View.FriendList;
using ConquiánCliente.View.MainMenu; 
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel
{
    public class FriendListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ServiceFriendList.PlayerDto> friends;

        private ObservableCollection<ServiceFriendList.PlayerDto> searchResult;


        public ICommand ViewProfileCommand { get; }
        public ICommand AddFriendCommand { get; }
        public ICommand RequestsCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand DeleteFriendCommand { get; }


        public ObservableCollection<ServiceFriendList.PlayerDto> Friends
        {
            get { return friends; }
            set { friends = value; OnPropertyChanged(nameof(Friends)); }
        }

        public ObservableCollection<ServiceFriendList.PlayerDto> SearchResult
        {
            get { return searchResult; }
            set { searchResult = value; OnPropertyChanged(nameof(SearchResult)); }
        }

        private readonly FriendListClient FriendListService;
        private readonly UserProfileClient UserProfileService;

        public FriendListViewModel()
        {
            FriendListService = new FriendListClient();
            UserProfileService = new UserProfileClient(); 
            Friends = new ObservableCollection<ServiceFriendList.PlayerDto>();
            SearchResult = new ObservableCollection<ServiceFriendList.PlayerDto>();
            ViewProfileCommand = new RelayCommand(ExecuteViewProfileCommand);
            AddFriendCommand = new RelayCommand(AddFriend);
            RequestsCommand = new RelayCommand(ExecuteRequestsCommand);
            DeleteFriendCommand = new RelayCommand(DeleteFriend);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            LoadFriends();
        }

        private async void LoadFriends()
        {
            var friendsList = await FriendListService.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);
            Friends = new ObservableCollection<ServiceFriendList.PlayerDto>(friendsList);
        }

        public async void SearchPlayer(string nickname)
        {
            var player = await FriendListService.GetPlayerByNicknameAsync(nickname, PlayerSession.CurrentPlayer.idPlayer);
            SearchResult.Clear();
            if (player != null)
            {
                SearchResult.Add(player);
            }
        }

        private async void AddFriend(object parameter)
        {
            if (parameter is ServiceFriendList.PlayerDto player)
            {
                var success = await FriendListService.SendFriendRequestAsync(PlayerSession.CurrentPlayer.idPlayer, player.idPlayer);
                if (success)
                {
                    MessageBox.Show(Lang.FriendRequestSentSuccess, Lang.TitleSuccess);
                } else
                {
                    MessageBox.Show(Lang.FriendRequestSentError, Lang.TitleError);
                }
            }
        }

        private void ExecuteRequestsCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var requestsWindow = new View.FriendList.FriendRequests();
                requestsWindow.Show();
                currentWindow.Close();
            }
        }

        private void ExecuteBackCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var mainMenu = new View.MainMenu.MainMenu();
                mainMenu.Show();
                currentWindow.Close();
            }
        }

        private async void ExecuteViewProfileCommand(object parameter)
        {
            if (parameter is ServiceFriendList.PlayerDto playerSummary)
            {
                try
                {
                    var fullPlayerProfile = await UserProfileService.GetPlayerByIdAsync(playerSummary.idPlayer);

                    var socials = await UserProfileService.GetPlayerSocialsAsync(playerSummary.idPlayer);

                    if (fullPlayerProfile != null)
                    {
                        var profileWindow = new FriendProfile(fullPlayerProfile, new ObservableCollection<ServiceUserProfile.SocialDto>(socials));
                        profileWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo cargar el perfil del jugador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al contactar el servicio.", "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void DeleteFriend(object parameter)
        {
            if (parameter is ServiceFriendList.PlayerDto player)
            {
                MessageBoxResult result = MessageBox.Show(string.Format(Lang.FriendListDeleteConfirmation, player.nickname), Lang.TitleConfirmation, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var success = await FriendListService.DeleteFriendAsync(PlayerSession.CurrentPlayer.idPlayer, player.idPlayer);
                    if (success)
                    {
                        Friends.Remove(player);
                        MessageBox.Show(Lang.FriendListDeletedSuccess, Lang.TitleSuccess);
                    }
                    else
                    {
                        MessageBox.Show(Lang.FriendListDeletedError, Lang.TitleError);
                    }
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}