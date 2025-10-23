using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class InviteFriendsViewModel : ViewModelBase
    {
        private string roomCode;
        public ObservableCollection<FriendInviteItemViewModel> FriendsList { get; }
        public ICommand InviteFriendCommand { get; }

        public InviteFriendsViewModel(string roomCode)
        {
            this.roomCode = roomCode;
            FriendsList = new ObservableCollection<FriendInviteItemViewModel>();
            InviteFriendCommand = new RelayCommand(async (param) => await ExecuteInviteFriend(param));
            LoadFriends();
        }

        private async void LoadFriends()
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
        private PlayerDto friend;
        private string statusText;
        private bool isOnline;
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