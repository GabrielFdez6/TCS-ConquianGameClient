using ConquiánCliente.ServicePresence;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Linq;
using ConquiánCliente.ViewModel.Lobby;

namespace ConquiánCliente.ViewModel
{
    public class PresenceClientManager
    {
        private static PresenceClientManager instance;
        private PresenceClient client;

        private bool isHandlingConnectionLoss = false;
        private DispatcherTimer heartbeatTimer;
        private int currentUserId;
        private bool isConnected;

        public PresenceClient Client
        {
            get
            {
                if (client == null ||
                    client.State == CommunicationState.Closed ||
                    client.State == CommunicationState.Faulted)
                {
                    InitializeClient();
                }
                return client;
            }
        }

        private PresenceClientManager()
        {
        }

        public static PresenceClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PresenceClientManager();
                }
                return instance;
            }
        }

        private void InitializeClient()
        {
            if (client != null)
            {
                try
                {
                    if (client.State == CommunicationState.Faulted)
                        client.Abort();
                    else
                        client.Close();
                }
                catch (Exception)
                {
                    client.Abort();
                }
            }

            var context = new InstanceContext(new PresenceCallbackHandler());
            client = new PresenceClient(context);
        }


        public void StartHeartbeat(int userId)
        {
            if (isConnected) return;

            currentUserId = userId;

            try
            {
                if (client == null || client.State != CommunicationState.Opened)
                {
                    InitializeClient();
                }

                client.Subscribe(userId);
                isConnected = true;

                heartbeatTimer = new DispatcherTimer();
                heartbeatTimer.Interval = TimeSpan.FromSeconds(5);
                heartbeatTimer.Tick += SendPing;
                heartbeatTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar heartbeat: " + ex.Message);
            }
        }

        public void StopHeartbeat()
        {
            heartbeatTimer?.Stop();
            isConnected = false;
            isHandlingConnectionLoss = false;

            try
            {
                if (client != null)
                {
                    client.Unsubscribe(currentUserId);
                    client.Close();
                }
            }
            catch (Exception)
            {
                client?.Abort();
            }
        }

        private async void SendPing(object sender, EventArgs e)
        {
            if (!isConnected || client == null) return;

            bool isConnectionSuccess = true;

            await Task.Run(() =>
            {
                try
                {
                    if (client.State == CommunicationState.Opened)
                    {
                        client.Ping(currentUserId);
                    }
                    else
                    {
                        isConnectionSuccess = false;
                    }
                }
                catch (Exception)
                {
                    isConnectionSuccess = false;
                }
            });

            if (!isConnectionSuccess)
            {
                HandleConnectionLoss();
            }
        }

        private void HandleConnectionLoss()
        {
            if (isHandlingConnectionLoss)
            {
                return;
            }

            isHandlingConnectionLoss = true;
            heartbeatTimer?.Stop();
            isConnected = false;

            try
            {
                client?.Abort();
            }
            catch
            {
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(ConquiánCliente.Properties.Langs.Lang.ErrorLostConnection,
                                ConquiánCliente.Properties.Langs.Lang.TitleError,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                NavigateToLogin();
                isHandlingConnectionLoss = false;
            });
        }
        private void NavigateToLogin()
        {
            PlayerSession.EndSession();
            var loginWindow = new LogIn();
            loginWindow.Show();

            Application.Current.MainWindow = loginWindow;

            var windowsToClose = Application.Current.Windows.OfType<Window>()
                                     .Where(w => w != loginWindow).ToList();

            foreach (var win in windowsToClose)
            {
                if (win.DataContext is LobbyGameViewModel lobbyVm)
                {
                    lobbyVm.IsNavigatingAway = true;
                }

                win.Close();
            }
        }
    }
}