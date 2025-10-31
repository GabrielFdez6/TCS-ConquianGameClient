using ConquiánCliente.ServiceLogin;
using ConquiánCliente.View;
using ConquiánCliente.View.FriendList;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.View.MainMenu;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.MainMenu
{
    public class MainMenuViewModel : ViewModelBase
    {
        public string Nickname { get; set; }
        public string ProfileImagePath { get; set; }

        public ICommand ViewProfileCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand FriendsCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand ChangeLanguageCommand { get; }

        public MainMenuViewModel()
        {
            LoadPlayerData();
            ViewProfileCommand = new RelayCommand(p => ExecuteViewProfileCommand(p));
            LogoutCommand = new RelayCommand(async (p) => await ExecuteLogoutCommand(p));
            FriendsCommand = new RelayCommand(ExecuteFriendsCommand);
            PlayCommand = new RelayCommand(ExecutePlay);
            ChangeLanguageCommand = new RelayCommand(ExecuteChangeLanguage);
            InvitationClientManager.Connect(PlayerSession.CurrentPlayer.idPlayer);
            InvitationCallbackHandler.OnGlobalInvitationReceived += HandleInvitation;
            PresenceClientManager.Instance.Client.Subscribe(PlayerSession.CurrentPlayer.idPlayer);
        }

        private void HandleInvitation(string senderNickname, string roomCode)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var vm = new InvitationReceivedViewModel(senderNickname, roomCode);
                var window = new View.Lobby.InvitationReceived { DataContext = vm };
                window.Show();
            });
        }

        public void OnWindowClosing()
        {
            InvitationCallbackHandler.OnGlobalInvitationReceived -= HandleInvitation;
        }

        private void LoadPlayerData()
        {
            if (PlayerSession.IsLoggedIn)
            {
                Nickname = PlayerSession.CurrentPlayer.nickname;
                ProfileImagePath = PlayerSession.CurrentPlayer.pathPhoto;
            }
        }

        private static void ExecuteViewProfileCommand(object parameter)
        {
            ProfileMainFrame userProfileView = ProfileMainFrame.GetInstance();
            userProfileView.Show();
            (parameter as Window)?.Close();
        }

        private static async Task ExecuteLogoutCommand(object parameter)
        {
            try
            {
                var loginClient = new LoginClient();
                await loginClient.SignOutPlayerAsync(PlayerSession.CurrentPlayer.idPlayer);
                await PresenceClientManager.Instance.Client.UnsubscribeAsync(PlayerSession.CurrentPlayer.idPlayer);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {

            }
            finally
            {
                PlayerSession.EndSession();
                var loginWindow = new LogIn();
                loginWindow.Show();
                (parameter as Window)?.Close();
            }
        }

        private static void ExecuteFriendsCommand(object obj)
        {
            if (obj is Window mainMenuWindow)
            {
                var friendListWindow = new View.FriendList.FriendList();
                friendListWindow.Show();
                mainMenuWindow.Close();
            }
        }

        private static void ExecutePlay(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                CreateOrJoin createOrJoinView = new CreateOrJoin();
                createOrJoinView.Owner = currentWindow;

                bool? result = createOrJoinView.ShowDialog();

                if (result == true)
                {
                    var createJoinViewModel = createOrJoinView.DataContext as CreateOrJoinViewModel;
                    string newRoomCode = createJoinViewModel.CreatedRoomCode;

                    if (!string.IsNullOrEmpty(newRoomCode))
                    {
                        LobbyGame lobby = new LobbyGame(newRoomCode);
                        lobby.Show();
                        currentWindow.Close();
                    }
                }
            }
        }

        private static void ExecuteChangeLanguage(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var selector = new ChangeLanguage();
                selector.Owner = currentWindow;
                selector.ShowDialog();
            }
        }
    }
}