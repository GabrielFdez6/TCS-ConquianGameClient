using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.View.FriendList;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages;

namespace ConquiánCliente.ViewModel.FriendList
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

        private readonly FriendListClient friendListService;
        private readonly ConquiánCliente.ServiceUserProfile.UserProfileClient userProfileService;
        private readonly IMessageResolver messageResolver;

        public FriendListViewModel()
        {
            friendListService = new FriendListClient();
            userProfileService = new ConquiánCliente.ServiceUserProfile.UserProfileClient();
            this.messageResolver = new ResourceMessageResolver();

            Friends = new ObservableCollection<FriendInviteItemViewModel>();
            SearchResult = new ObservableCollection<FriendInviteItemViewModel>();

            ViewProfileCommand = new RelayCommand(ExecuteViewProfileCommand);
            AddFriendCommand = new RelayCommand(AddFriend);
            RequestsCommand = new RelayCommand(ExecuteRequestsCommand);
            DeleteFriendCommand = new RelayCommand(DeleteFriend);
            BackCommand = new RelayCommand(ExecuteBackCommand);

            _ = LoadFriends();

            PresenceCallbackHandler.FriendStatusChanged += OnFriendStatusChanged;
            PresenceCallbackHandler.FriendListUpdated += OnFriendListUpdated;
        }

        private void OnFriendListUpdated()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await LoadFriends();
            });
        }

        private void OnFriendStatusChanged(int friendId, int newStatusId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateFriendStatus(friendId, newStatusId);
            });
        }

        private void UpdateFriendStatus(int friendId, int newStatusId)
        {
            var newStatus = (PlayerStatus)newStatusId;
            bool isOnline = (newStatus == PlayerStatus.Online);

            UpdateCollectionStatus(Friends, friendId, newStatus);
            UpdateCollectionStatus(SearchResult, friendId, newStatus);
        }

        private void UpdateCollectionStatus(ObservableCollection<FriendInviteItemViewModel> collection, int friendId, PlayerStatus newStatus)
        {
            var item = collection.FirstOrDefault(f => f.IdPlayer == friendId);
            if (item != null)
            {
                bool isOnline = (newStatus == PlayerStatus.Online);
                item.IsOnline = isOnline;
                item.StatusText = isOnline ? Lang.StatusOnline : Lang.StatusOffline;
                item.PlayerDto.Status = newStatus;
            }
        }

        public void Cleanup()
        {
            PresenceCallbackHandler.FriendStatusChanged -= OnFriendStatusChanged;
            PresenceCallbackHandler.FriendListUpdated -= OnFriendListUpdated;
        }

        private async Task LoadFriends()
        {
            try
            {
                var friendsList = await friendListService.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);

                Friends.Clear();
                if (friendsList != null)
                {
                    foreach (var friendDto in friendsList.OrderBy(f => f.Status))
                    {
                        Friends.Add(new FriendInviteItemViewModel(friendDto));
                    }
                }
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        public async Task SearchPlayer(string nickname)
        {
            try
            {
                SearchResult.Clear();
                var player = await friendListService.GetPlayerByNicknameAsync(nickname, PlayerSession.CurrentPlayer.idPlayer);

                if (player != null)
                {
                    SearchResult.Add(new FriendInviteItemViewModel(player));
                }
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private async void AddFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                try
                {
                    await friendListService.SendFriendRequestAsync(PlayerSession.CurrentPlayer.idPlayer, friendVM.IdPlayer);
                    MessageBox.Show(Lang.FriendRequestSentSuccess, Lang.TitleSuccess);
                }
                catch (Exception ex)
                {
                    HandleServiceException(ex);
                }
            }
        }

        private void ExecuteRequestsCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                Cleanup();
                var requestsWindow = new View.FriendList.FriendRequests();
                requestsWindow.Show();
                currentWindow.Close();
            }
        }

        private void ExecuteBackCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                Cleanup();
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
                    var fullPlayerProfile = await userProfileService.GetPlayerByIdAsync(friendVM.IdPlayer);
                    var socials = await userProfileService.GetPlayerSocialsAsync(friendVM.IdPlayer);

                    if (fullPlayerProfile != null)
                    {
                        ShowProfileWindow(fullPlayerProfile, socials);
                    }
                }
                catch (Exception ex)
                {
                    HandleServiceException(ex);
                }
            }
        }

        private void ShowProfileWindow(ConquiánCliente.ServiceUserProfile.PlayerDto profile, ConquiánCliente.ServiceUserProfile.SocialDto[] socials)
        {
            var profileWindow = new FriendProfile(profile, new ObservableCollection<ConquiánCliente.ServiceUserProfile.SocialDto>(socials));
            profileWindow.ShowDialog();
        }

        private async void DeleteFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                if (UserConfirmsDeletion(friendVM.Nickname))
                {
                    await PerformDeletion(friendVM);
                }
            }
        }

        private bool UserConfirmsDeletion(string nickname)
        {
            MessageBoxResult result = MessageBox.Show(string.Format(Lang.FriendListDeleteConfirmation, nickname), Lang.TitleConfirmation, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private async Task PerformDeletion(FriendInviteItemViewModel friendVM)
        {
            try
            {
                await friendListService.DeleteFriendAsync(PlayerSession.CurrentPlayer.idPlayer, friendVM.IdPlayer);

                Friends.Remove(friendVM);
                MessageBox.Show(Lang.FriendListDeletedSuccess, Lang.TitleSuccess);
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private void HandleServiceException(Exception ex)
        {
            if (ex is FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ex is FaultException<ServiceUserProfile.ServiceFaultDto> userProfileFault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)userProfileFault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ex is EndpointNotFoundException || ex is CommunicationException || ex is TimeoutException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected), Lang.TitleError);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}