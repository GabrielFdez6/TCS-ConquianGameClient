using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.ServiceInvitation;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.View.Lobby;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
                    friendVM.CurrentStatus = (PlayerStatus)newStatusId;
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
                    PopulateFriendsList(friends);
                }
            }
            catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                HandleServiceFault(fault);
            }
            catch (EndpointNotFoundException)
            {
                ShowConnectionError(Lang.ErrorServerUnavailable);
            }
            catch (TimeoutException)
            {
                ShowConnectionError(Lang.LobbyErrorLoadingFriends);
            }
            catch (CommunicationException)
            {
                ShowConnectionError(Lang.LobbyErrorLoadingFriends);
            }
        }

        private void PopulateFriendsList(PlayerDto[] friends)
        {
            FriendsList.Clear();
            foreach (var friend in friends.OrderBy(f => f.Status))
            {
                FriendsList.Add(new FriendInviteItemViewModel(friend));
            }
        }

        private async Task ExecuteInviteFriend(object parameter)
        {
            if (!(parameter is FriendInviteItemViewModel friendVM))
            {
                return;
            }

            if (!ValidatePlayerAvailability(friendVM))
            {
                return;
            }

            await AttemptSendInvitation(friendVM);
        }

        private bool ValidatePlayerAvailability(FriendInviteItemViewModel friendVM)
        {
            bool isAvailable = true;

            if (friendVM.CurrentStatus == PlayerStatus.InGame)
            {
                MessageBox.Show(Lang.ErrorPlayerInGame, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                isAvailable = false;
            }
            else if (friendVM.CurrentStatus == PlayerStatus.InLobby)
            {
                MessageBox.Show(Lang.ErrorPlayerInLobby, Lang.TitleInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                isAvailable = false;
            }

            return isAvailable;
        }

        private async Task AttemptSendInvitation(FriendInviteItemViewModel friendVM)
        {
            try
            {
                var senderDto = CreateSenderDto();
                await InvitationClientManager.SendInvitationAsync(senderDto, friendVM.IdPlayer, this.roomCode);
                friendVM.StatusText = Lang.LobbyInvitationSent;
            }
            catch (FaultException<ServiceInvitation.ServiceFaultDto> fault)
            {
                HandleInvitationFault(fault, friendVM);
            }
            catch (CommunicationException)
            {
                ShowConnectionError(Lang.ErrorConnectingToServer);
            }
            catch (TimeoutException)
            {
                ShowConnectionError(Lang.ErrorConnectingToServer);
            }
        }

        private InvitationSenderDto CreateSenderDto()
        {
            return new InvitationSenderDto
            {
                IdPlayer = PlayerSession.CurrentPlayer.idPlayer,
                Nickname = PlayerSession.CurrentPlayer.nickname
            };
        }

        private void HandleInvitationFault(FaultException<ServiceInvitation.ServiceFaultDto> fault, FriendInviteItemViewModel friendVM)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;

            if (errorType == ConquiánCliente.ServiceLogin.ServiceErrorType.UserOffline)
            {
                friendVM.StatusText = Lang.StatusOffline;
                friendVM.IsOnline = false;
            }
            else
            {
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void HandleServiceFault(FaultException<ServiceFriendList.ServiceFaultDto> fault)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);
            MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowConnectionError(string message)
        {
            MessageBox.Show(message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public class FriendInviteItemViewModel : ViewModelBase
    {
        private readonly PlayerDto friend;
        public PlayerDto PlayerDto => friend;
        private PlayerStatus currentStatus;
        private string statusText;
        private bool isOnline;
        public int Level => friend.idLevel;

        public FriendInviteItemViewModel(PlayerDto friend)
        {
            this.friend = friend;

            this.CurrentStatus = friend.Status;
        }

        public PlayerStatus CurrentStatus
        {
            get => currentStatus;
            set
            {
                currentStatus = value;
                OnPropertyChanged(nameof(CurrentStatus));
                UpdateVisuals();
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

        private void UpdateVisuals()
        {
            this.IsOnline = (this.CurrentStatus != PlayerStatus.Offline);

            if (this.CurrentStatus == PlayerStatus.InLobby)
            {
                this.StatusText = Lang.StatusInLobby;
            }
            else if (this.CurrentStatus == PlayerStatus.InGame)
            {
                this.StatusText = Lang.StatusInGame;
            }
            else
            {
                this.StatusText = this.IsOnline ? Lang.StatusOnline : Lang.StatusOffline;
            }
        }
    }
}