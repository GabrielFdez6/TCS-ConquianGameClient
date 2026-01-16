using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;
using ConquiánCliente.View;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.View.MainMenu;
using System;
using System.ServiceModel;
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
        public ICommand OpenSettingsCommand { get; }

        private bool isLoading;

        public MainMenuViewModel()
        {
            LoadPlayerData();
            isLoading = false;

            ViewProfileCommand = new RelayCommand(ExecuteViewProfileCommand, CanExecuteNavigation);
            LogoutCommand = new RelayCommand(ExecuteLogoutCommand, CanExecuteNavigation);
            FriendsCommand = new RelayCommand(ExecuteFriendsCommand, CanExecuteNavigation);
            PlayCommand = new RelayCommand(ExecutePlay, CanExecuteNavigation);
            OpenSettingsCommand = new RelayCommand(ExecuteOpenSettings, CanExecuteNavigation);

            InitializeBackgroundConnections();
        }

        private void InitializeBackgroundConnections()
        {
            if (PlayerSession.CurrentPlayer != null)
            {
                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                Task.Run(() => AttemptConnectionSetup(playerId));
            }
        }

        private static void AttemptConnectionSetup(int playerId)
        {
            try
            {
                InvitationClientManager.Connect(playerId);
                if (PresenceClientManager.Instance.Client != null)
                {
                    PresenceClientManager.Instance.Client.Subscribe(PlayerSession.CurrentPlayer.idPlayer);
                }
            }
            catch (CommunicationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network error connecting services: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Timeout connecting services: {ex.Message}");
            }
        }

        private bool CanExecuteNavigation(object parameter)
        {
            return !isLoading;
        }

        private void LoadPlayerData()
        {
            if (PlayerSession.IsLoggedIn)
            {
                Nickname = PlayerSession.CurrentPlayer.nickname;
                ProfileImagePath = PlayerSession.CurrentPlayer.pathPhoto;
            }
        }

        private void ExecuteViewProfileCommand(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            SetLoadingState(true);

            ProfileMainFrame userProfileView = ProfileMainFrame.GetInstance();
            userProfileView.Show();

            CloseWindow(parameter as Window);
        }

        private async void ExecuteLogoutCommand(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            SetLoadingState(true);

            await PerformServerLogoutAsync();
            TransitionToLogin(parameter as Window);
        }

        private async Task PerformServerLogoutAsync()
        {
            int playerId = PlayerSession.CurrentPlayer.idPlayer;

            await Task.Run(async () =>
            {
                await SignOutLoginService(playerId);
                await UnsubscribePresenceService(playerId);
                DisconnectInvitationService(playerId);
            });
        }

        private async Task SignOutLoginService(int playerId)
        {
            var loginClient = new LoginClient();
            try
            {
                await loginClient.SignOutPlayerAsync(playerId);
            }
            catch (CommunicationException)
            {
                 // The server may be unavailable or the network may fail at this point.
                // Logout must still complete locally to avoid leaving the client in an inconsistent state.
            }
            catch (TimeoutException)
            {
                 // A delayed server response during logout is expected in unstable networks.
                 //Blocking the logout would negatively impact user experience, so the error is ignored.
            }
        }

        private async Task UnsubscribePresenceService(int playerId)
        {
            try
            {
                if (PresenceClientManager.Instance.Client != null)
                {
                    await PresenceClientManager.Instance.Client.UnsubscribeAsync(playerId);
                }
            }
            catch (CommunicationException)
            {
                   // Presence unsubscription is a best-effort operation.
                  // If it fails, the server will eventually clean up stale subscriptions automatically.
            }
            catch (TimeoutException)
            {
                // Presence status is not critical during logout.
                // The application prioritizes session termination over presence synchronization.
            }
        }

        private void DisconnectInvitationService(int playerId)
        {
            try
            {
                InvitationClientManager.Disconnect(playerId);
            }
            catch (Exception)
            {
                // This disconnection only releases local resources.
                // Any failure here should not prevent the user from logging out successfully.
            }
        }
        private void TransitionToLogin(Window currentWindow)
        {
            PlayerSession.EndSession();
            var loginWindow = new LogIn();
            loginWindow.Show();

            CloseWindow(currentWindow);
        }

        private void ExecuteFriendsCommand(object obj)
        {
            if (isLoading)
            {
                return;
            }

            SetLoadingState(true);

            if (obj is Window mainMenuWindow)
            {
                var friendListWindow = new View.FriendList.FriendList();
                friendListWindow.Show();
                mainMenuWindow.Close();
            }
        }
        private void ExecutePlay(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            SetLoadingState(true);

            if (parameter is Window currentWindow)
            {
                string newRoomCode = TryGetRoomCodeFromDialog(currentWindow);

                if (!string.IsNullOrEmpty(newRoomCode))
                {
                    NavigateToLobby(newRoomCode, currentWindow);
                    return;
                }
            }

            SetLoadingState(false);
        }

        private string TryGetRoomCodeFromDialog(Window owner)
        {
            string code = string.Empty;
            CreateOrJoin createOrJoinView = new CreateOrJoin();
            createOrJoinView.Owner = owner;

            bool? result = createOrJoinView.ShowDialog();

            if (result == true)
            {
                var createJoinViewModel = createOrJoinView.DataContext as CreateOrJoinViewModel;
                if (createJoinViewModel != null)
                {
                    code = createJoinViewModel.CreatedRoomCode;
                }
            }

            return code;
        }

        private void NavigateToLobby(string roomCode, Window currentWindow)
        {
            LobbyGame lobby = new LobbyGame(roomCode);
            lobby.Show();
            currentWindow.Close();
        }

        private void ExecuteOpenSettings(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            SetLoadingState(true);

            if (parameter is Window currentWindow)
            {
                var settingsView = new Settings();
                settingsView.Owner = currentWindow;
                settingsView.ShowDialog();
            }

            SetLoadingState(false);
        }

        private void SetLoadingState(bool loading)
        {
            isLoading = loading;
            CommandManager.InvalidateRequerySuggested();
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}