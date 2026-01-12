using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;
using ConquiánCliente.View;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.View.MainMenu;
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

            if (PlayerSession.CurrentPlayer != null)
            {
                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                Task.Run(() => InitializeServerConnections(playerId));
            }
        }

        private void InitializeServerConnections(int playerId)
        {
            try
            {
                InvitationClientManager.Connect(playerId);
                if (PresenceClientManager.Instance.Client != null)
                {
                    PresenceClientManager.Instance.Client.Subscribe(PlayerSession.CurrentPlayer.idPlayer);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error connecting services in background: " + ex.Message);
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
            if (isLoading) return;
            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            ProfileMainFrame userProfileView = ProfileMainFrame.GetInstance();
            userProfileView.Show();
            (parameter as Window)?.Close();
        }

        private async void ExecuteLogoutCommand(object parameter)
        {
            if (isLoading) return;
            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            try
            {
                int playerId = PlayerSession.CurrentPlayer.idPlayer;
                await Task.Run(async () =>
                {
                    var loginClient = new LoginClient();
                    try
                    {
                        await loginClient.SignOutPlayerAsync(playerId);
                    }
                    catch (Exception) { }

                    try
                    {
                        if (PresenceClientManager.Instance.Client != null)
                        {
                            await PresenceClientManager.Instance.Client.UnsubscribeAsync(playerId);
                        }
                    }
                    catch (Exception) { }

                    try
                    {
                        InvitationClientManager.Disconnect(playerId);
                    }
                    catch (Exception) { }
                });
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorLogOutSession, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                PlayerSession.EndSession();
                var loginWindow = new LogIn();
                loginWindow.Show();
                (parameter as Window)?.Close();

            }
        }

        private void ExecuteFriendsCommand(object obj)
        {
            if (isLoading) return;
            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            if (obj is Window mainMenuWindow)
            {
                var friendListWindow = new View.FriendList.FriendList();
                friendListWindow.Show();
                mainMenuWindow.Close();
            }
        }

        private void ExecutePlay(object parameter)
        {
            if (isLoading) return;
            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

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
                        return;
                    }
                }

                isLoading = false;
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                isLoading = false;
            }
        }

        private void ExecuteOpenSettings(object parameter)
        {
            if (isLoading) return;
            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            if (parameter is Window currentWindow)
            {
                var settingsView = new Settings();
                settingsView.Owner = currentWindow;
                settingsView.ShowDialog();
            }

            isLoading = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}