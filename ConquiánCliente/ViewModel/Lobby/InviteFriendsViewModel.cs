using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.View.Lobby;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ConquiánCliente.Utilities.Messages; 

namespace ConquiánCliente.ViewModel.Lobby
{
    public class InviteFriendsViewModel : ViewModelBase
    {
        private readonly string roomCode;
        public ObservableCollection<FriendInviteItemViewModel> FriendsList { get; }
        public ICommand InviteFriendCommand { get; }
        public ICommand SendRoomCodeCommand { get; }
        private readonly IMessageResolver messageResolver; 

        public InviteFriendsViewModel(string roomCode)
        {
            this.roomCode = roomCode;
            this.messageResolver = new ResourceMessageResolver(); 

            FriendsList = new ObservableCollection<FriendInviteItemViewModel>();
            InviteFriendCommand = new RelayCommand(async (param) => await ExecuteInviteFriend(param));
            SendRoomCodeCommand = new RelayCommand(ExecuteSendRoomCode);

            PresenceCallbackHandler.FriendStatusChanged += OnFriendStatusChanged;
            _ = LoadFriends();
        }

        private void ExecuteSendRoomCode(object parameter)
        {
            var ownerWindow = parameter as Window;
            var viewModel = new SendRoomCodeViewModel(this.roomCode);
            var sendCodeWindow = new SendRoomCode()
            {
                Owner = ownerWindow,
                DataContext = viewModel
            };
            sendCodeWindow.ShowDialog();
        }

        private void OnFriendStatusChanged(int friendId, int newStatusId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var friendVM = FriendsList.FirstOrDefault(f => f.IdPlayer == friendId);
                if (friendVM != null)
                {
                    var newStatus = (PlayerStatus)newStatusId;

                    bool isOnline = (newStatus != PlayerStatus.Offline);

                    friendVM.IsOnline = isOnline;

                    switch (newStatus)
                    {
                        case PlayerStatus.Online:
                            friendVM.StatusText = Lang.StatusOnline;
                            break;
                        case PlayerStatus.InLobby:
                            friendVM.StatusText = Lang.StatusInLobby; 
                            break;
                        case PlayerStatus.InGame:
                            friendVM.StatusText = Lang.StatusInGame; 
                            break;
                        default:
                            friendVM.StatusText = Lang.StatusOffline;
                            break;
                    }
                }
            });
        }

        private async Task LoadFriends()
        {
            try
            {
                using (var client = new FriendListClient())
                {
                    var friends = await client.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);

                    FriendsList.Clear();

                    foreach (var friend in friends.OrderBy(f => f.Status))
                    {
                        FriendsList.Add(new FriendInviteItemViewModel(friend));
                    }
                }
            }
            catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lang.LobbyErrorLoadingFriends + $": {ex.Message}");
            }
        }

        private async Task ExecuteInviteFriend(object parameter)
        {
            if (parameter is FriendInviteItemViewModel friendVM)
            {
                try
                {
                    await InvitationClientManager.SendInvitationAsync(
                        PlayerSession.CurrentPlayer.idPlayer,
                        PlayerSession.CurrentPlayer.nickname,
                        friendVM.IdPlayer,
                        this.roomCode
                    );

                    friendVM.StatusText = Lang.LobbyInvitationSent;
                }
                catch (FaultException<ConquiánCliente.ServiceInvitation.ServiceFaultDto> fault)
                {
                    var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;

                    if (errorType == ConquiánCliente.ServiceLogin.ServiceErrorType.UserInGame)
                    {
                        MessageBox.Show(Lang.ErrorPlayerInGame, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (errorType == ConquiánCliente.ServiceLogin.ServiceErrorType.UserOffline)
                    {
                        friendVM.StatusText = Lang.StatusOffline;
                        friendVM.IsOnline = false;
                    }
                    else
                    {
                        string msg = messageResolver.GetMessage(errorType);
                        MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (CommunicationException)
                {
                    MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Lang.LobbyErrorInvitationFailed}: {ex.Message}", Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class FriendInviteItemViewModel : ViewModelBase
    {
        private readonly PlayerDto friend;
        public PlayerDto PlayerDto => friend;
        private string statusText;
        private bool isOnline;
        public int Level => friend.idLevel;

        public FriendInviteItemViewModel(PlayerDto friend)
        {
            this.friend = friend;

            this.IsOnline = friend.Status != PlayerStatus.Offline;

            if (friend.Status == PlayerStatus.InLobby)
            {
                this.StatusText = Lang.StatusInLobby;
            }
            else if (friend.Status == PlayerStatus.InGame)
            {
                this.StatusText = Lang.StatusInGame;
            }
            else
            {
                this.StatusText = this.IsOnline ? Lang.StatusOnline : Lang.StatusOffline;
            }
        }
        public int IdPlayer => friend.idPlayer;
        public string Nickname => friend.nickname;
        public string ProfileImagePath => friend.pathPhoto;
        public bool IsOnline
        {
            get => isOnline;
            set { isOnline = value; OnPropertyChanged(nameof(IsOnline)); OnPropertyChanged(nameof(StatusColor)); }
        }

        public string StatusText
        {
            get => statusText;
            set { statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public Brush StatusColor => IsOnline ? Brushes.Green : Brushes.Gray;
    }
}