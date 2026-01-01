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
        private readonly ConquiánCliente.ServiceUserProfile.UserProfileClient UserProfileService;
        private readonly IMessageResolver messageResolver; 

        public FriendListViewModel()
        {
            FriendListService = new FriendListClient();
            UserProfileService = new ConquiánCliente.ServiceUserProfile.UserProfileClient();
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

                var newStatus = (PlayerStatus)newStatusId;
                bool isOnline = (newStatus == PlayerStatus.Online);

                var friendVM = Friends.FirstOrDefault(f => f.IdPlayer == friendId);
                if (friendVM != null)
                {
                    friendVM.IsOnline = isOnline;
                    friendVM.StatusText = isOnline ? Lang.StatusOnline : Lang.StatusOffline;
                    friendVM.PlayerDto.Status = newStatus;
                }

                var searchVM = SearchResult.FirstOrDefault(f => f.IdPlayer == friendId);
                if (searchVM != null)
                {
                    searchVM.IsOnline = isOnline;
                    searchVM.StatusText = isOnline ? Lang.StatusOnline : Lang.StatusOffline;
                    searchVM.PlayerDto.Status = newStatus;
                }
            });
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
                var friendsList = await FriendListService.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);

                Friends.Clear();
                if (friendsList != null)
                {
                    foreach (var friendDto in friendsList.OrderBy(f => f.Status))
                    {
                        Friends.Add(new FriendInviteItemViewModel(friendDto));
                    }
                }
            }
            catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        public async Task SearchPlayer(string nickname)
        {
            try
            {
                SearchResult.Clear();
                var player = await FriendListService.GetPlayerByNicknameAsync(nickname, PlayerSession.CurrentPlayer.idPlayer);

                if (player != null)
                {
                    SearchResult.Add(new FriendInviteItemViewModel(player));
                }
            }
            catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        private async void AddFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                try
                {
                    await FriendListService.SendFriendRequestAsync(PlayerSession.CurrentPlayer.idPlayer, friendVM.IdPlayer);
                    MessageBox.Show(Lang.FriendRequestSentSuccess, Lang.TitleSuccess);
                }
                catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);
                    MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
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
                    var fullPlayerProfile = await UserProfileService.GetPlayerByIdAsync(friendVM.IdPlayer);
                    var socials = await UserProfileService.GetPlayerSocialsAsync(friendVM.IdPlayer);

                    if (fullPlayerProfile != null)
                    {
                        var profileWindow = new FriendProfile(fullPlayerProfile, new ObservableCollection<ConquiánCliente.ServiceUserProfile.SocialDto>(socials));
                        profileWindow.ShowDialog();
                    }
                }
                catch (FaultException<ServiceUserProfile.ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                    string msg = messageResolver.GetMessage(errorType);
                    MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.ErrorConnectingToServer, ex.Message), Lang.TitleError);
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
                    try
                    {
                        await FriendListService.DeleteFriendAsync(PlayerSession.CurrentPlayer.idPlayer, friendVM.IdPlayer);

                        Friends.Remove(friendVM);
                        MessageBox.Show(Lang.FriendListDeletedSuccess, Lang.TitleSuccess);
                    }
                    catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
                    {
                        var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                        string msg = messageResolver.GetMessage(errorType);
                        MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
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