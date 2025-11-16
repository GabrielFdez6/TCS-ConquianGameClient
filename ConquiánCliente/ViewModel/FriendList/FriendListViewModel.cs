using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.View.FriendList;
using ConquiánCliente.View.MainMenu;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel
{
    public class FriendListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<FriendInviteItemViewModel> friends;
        private ObservableCollection<FriendInviteItemViewModel> searchResult;

        public ICommand ViewProfileCommand { get; }
        public ICommand AddFriendCommand { get; }
        public ICommand RequestsCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand DeleteFriendCommand { get; }

        public ObservableCollection<FriendInviteItemViewModel> Friends
        {
            get { return friends; }
            set { friends = value; OnPropertyChanged(nameof(Friends)); }
        }

        public ObservableCollection<FriendInviteItemViewModel> SearchResult
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
            Friends = new ObservableCollection<FriendInviteItemViewModel>();
            SearchResult = new ObservableCollection<FriendInviteItemViewModel>();
            ViewProfileCommand = new RelayCommand(ExecuteViewProfileCommand);
            AddFriendCommand = new RelayCommand(AddFriend);
            RequestsCommand = new RelayCommand(ExecuteRequestsCommand); 
            DeleteFriendCommand = new RelayCommand(DeleteFriend);
            BackCommand = new RelayCommand(ExecuteBackCommand); 
            _ = LoadFriends();

            PresenceCallbackHandler.FriendStatusChanged += OnFriendStatusChanged;
        }

        private void OnFriendStatusChanged(int friendId, int newStatusId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var friendVM = Friends.FirstOrDefault(f => f.IdPlayer == friendId);
                if (friendVM != null)
                {
                    bool isOnline = (newStatusId == 1);
                    friendVM.IsOnline = isOnline;
                    friendVM.StatusText = isOnline ? Lang.StatusOnline : Lang.StatusOffline;
                }

                var searchVM = SearchResult.FirstOrDefault(f => f.IdPlayer == friendId);
                if (searchVM != null)
                {
                    bool isOnline = (newStatusId == 1);
                    searchVM.IsOnline = isOnline;
                    searchVM.StatusText = isOnline ? Lang.StatusOnline : Lang.StatusOffline;
                }
            });
        }

        public void Cleanup()
        {
            PresenceCallbackHandler.FriendStatusChanged -= OnFriendStatusChanged;
        }

        private async Task LoadFriends()
        {
            var friendsList = await FriendListService.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);

            Friends.Clear();
            if (friendsList != null)
            {
                foreach (var friendDto in friendsList.OrderByDescending(f => f.idStatus))
                {
                    Friends.Add(new FriendInviteItemViewModel(friendDto));
                }
            }
        }

        public async Task SearchPlayer(string nickname)
        {
            var player = await FriendListService.GetPlayerByNicknameAsync(nickname, PlayerSession.CurrentPlayer.idPlayer);
            SearchResult.Clear();
            if (player.idPlayer > 0)
            {
                SearchResult.Add(new FriendInviteItemViewModel(player));
            }
        }

        private async void AddFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                var success = await FriendListService.SendFriendRequestAsync(PlayerSession.CurrentPlayer.idPlayer, friendVM.IdPlayer);
                if (success)
                {
                    MessageBox.Show(Lang.FriendRequestSentSuccess, Lang.TitleSuccess);
                }
                else
                {
                    MessageBox.Show(Lang.FriendRequestSentError, Lang.TitleError);
                }
            }
        }

        private static void ExecuteRequestsCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var requestsWindow = new View.FriendList.FriendRequests();
                requestsWindow.Show();
                currentWindow.Close();
            }
        }

        private static void ExecuteBackCommand(object parameter)
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
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                try
                {
                    var fullPlayerProfile = await UserProfileService.GetPlayerByIdAsync(friendVM.IdPlayer);
                    var socials = await UserProfileService.GetPlayerSocialsAsync(friendVM.IdPlayer);

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
                catch (CommunicationException commEx)
                {
                    Console.WriteLine($"Error de comunicación al desconectar: {commEx.Message}");
                }
                catch (TimeoutException timeoutEx)
                {
                    Console.WriteLine($"Tiempo de espera agotado al desconectar: {timeoutEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado al desconectar: {ex.Message}");
                }
            }
        }

        private async void DeleteFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                MessageBoxResult result = MessageBox.Show(string.Format(Lang.FriendListDeleteConfirmation, friendVM.Nickname), Lang.TitleConfirmation, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var success = await FriendListService.DeleteFriendAsync(PlayerSession.CurrentPlayer.idPlayer, friendVM.IdPlayer);
                    if (success)
                    {
                        Friends.Remove(friendVM);
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