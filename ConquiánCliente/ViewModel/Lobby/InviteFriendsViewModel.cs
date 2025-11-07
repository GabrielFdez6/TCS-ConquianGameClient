using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class InviteFriendsViewModel : ViewModelBase
    {
        private readonly string roomCode;
        public ObservableCollection<FriendInviteItemViewModel> FriendsList { get; }
        public ICommand InviteFriendCommand { get; }
        public ICommand SendRoomCodeCommand { get; } 

        public InviteFriendsViewModel(string roomCode)
        {
            this.roomCode = roomCode;
            FriendsList = new ObservableCollection<FriendInviteItemViewModel>();
            InviteFriendCommand = new RelayCommand(async (param) => await ExecuteInviteFriend(param));
            PresenceCallbackHandler.FriendStatusChanged += OnFriendStatusChanged;
            SendRoomCodeCommand = new RelayCommand(ExecuteSendRoomCode); 
            _ =LoadFriends();
        }

        private void ExecuteSendRoomCode(object parameter)
        {
            var ownerWindow = parameter as Window;

            var sendCodeWindow = new SendRoomCode()
            {
                Owner = ownerWindow
            };

            sendCodeWindow.ShowDialog();
        }
        private void OnFriendStatusChanged(int friendId, int newStatusId)
        {
            var friendVM = FriendsList.FirstOrDefault(f => f.IdPlayer == friendId);
            if (friendVM != null)
            {
                bool isOnline = (newStatusId == 1);
                friendVM.IsOnline = isOnline;
                friendVM.StatusText = isOnline ? Lang.StatusOnline : Lang.StatusOffline;
            }
        }
        private async Task LoadFriends()
        {
            try
            {
                using (var client = new FriendListClient())
                {
                    var friends = await client.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);
                    FriendsList.Clear();
                    foreach (var friend in friends.OrderByDescending(f => f.idStatus)) 
                    {
                        FriendsList.Add(new FriendInviteItemViewModel(friend));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Lang.LobbyErrorLoadingFriends + $": {ex.Message}");
            }
        }

        private async Task ExecuteInviteFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                bool success = await InvitationClientManager.SendInvitation(
                    PlayerSession.CurrentPlayer.idPlayer,
                    PlayerSession.CurrentPlayer.nickname,
                    friendVM.IdPlayer,
                    this.roomCode
                );

                if (success)
                {
                    friendVM.StatusText = Lang.LobbyInvitationSent; 
                    friendVM.IsOnline = false; 
                }
                else
                {
                    System.Windows.MessageBox.Show(Lang.LobbyErrorInvitationFailed); 
                }
            }
        }
    }

    public class FriendInviteItemViewModel : ViewModelBase
    {
        private readonly PlayerDto friend;
        private string statusText;
        private bool isOnline;
        public string Level => friend.level;
        public FriendInviteItemViewModel(PlayerDto friend)
        {
            this.friend = friend;
            this.IsOnline = friend.idStatus == 1; 
            this.StatusText = this.IsOnline ? Lang.StatusOnline : Lang.StatusOffline;
        }

        public int IdPlayer => friend.idPlayer;
        public string Nickname => friend.nickname;
        public string ProfileImagePath => friend.pathPhoto;
        public bool IsOnline
        {
            get => isOnline;
            set { isOnline = value; OnPropertyChanged(nameof(IsOnline)); }
        }

        public string StatusText
        {
            get => statusText;
            set { statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public Brush StatusColor => IsOnline ? Brushes.Green : Brushes.Gray;
    }
}