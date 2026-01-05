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
        private const int MAX_RETRY_ATTEMPTS = 3;
        private static PresenceClientManager instance;
        private PresenceClient client;

        private bool isHandlingConnectionLoss = false;
        private DispatcherTimer heartbeatTimer;
        private int currentUserId;
        private bool isConnected;
        private int failureCount = 0;

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
            failureCount = 0;

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
            failureCount = 0;

            try
            {
                if (client != null)
                {
                    if (client.State == CommunicationState.Opened)
                    {
                        client.Unsubscribe(currentUserId);
                        client.Close();
                    }
                    else
                    {
                        client.Abort();
                    }
                }
            }
            catch (Exception)
            {
                client?.Abort();
            }
        }

        private async void SendPing(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                return;
            }

            int timeoutMs = 3000;
            bool pingSuccess = false;

            await Task.Run(async () =>
            {
                try
                {
                    if (client == null || client.State == CommunicationState.Faulted || client.State == CommunicationState.Closed)
                    {
                        InitializeClient();
                    }

                    if (client.State == CommunicationState.Opened)
                    {
                        var pingTask = Task.Run(() => client.Ping(currentUserId));

                        if (await Task.WhenAny(pingTask, Task.Delay(timeoutMs)) == pingTask)
                        {
                            await pingTask;
                            pingSuccess = true;
                        }
                        else
                        {
                            Console.WriteLine("Heartbeat Timeout: El servidor no respondió en 3s.");
                            pingSuccess = false;
                        }
                    }
                    else
                    {
                        pingSuccess = false;
                    }
                }
                catch (Exception)
                {
                    pingSuccess = false;
                }
            });

            if (pingSuccess)
            {
                failureCount = 0;
            }
            else
            {
                failureCount++;
                Console.WriteLine($"Heartbeat fallido: {failureCount}/{MAX_RETRY_ATTEMPTS}");
            }

            if (failureCount >= MAX_RETRY_ATTEMPTS)
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
                if (ConquiánCliente.ViewModel.PlayerSession.IsLoggedIn)
                {

                    NavigateToLogin();

                    MessageBox.Show(ConquiánCliente.Properties.Langs.Lang.ErrorLostConnection,
                                    ConquiánCliente.Properties.Langs.Lang.TitleError,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }

                isHandlingConnectionLoss = false;
            });
        }
        private void NavigateToLogin()
        {
            var windowsToClose = Application.Current.Windows.OfType<Window>()
                                                         .Where(w => !(w is LogIn)).ToList();

            PlayerSession.EndSession();

            var loginWindow = new LogIn();
            loginWindow.Show();

            Application.Current.MainWindow = loginWindow;

            foreach (var win in windowsToClose)
            {
                if (win.DataContext is LobbyGameViewModel lobbyVm)
                {
                    lobbyVm.IsNavigatingAway = true;

                    lobbyVm.CloseClientConnection(notifyServer: true);
                }

                if (win.DataContext is ConquiánCliente.ViewModel.Game.GameViewModel gameVm)
                {
                    gameVm.LeaveGame();
                }

                win.Close();
            }
        }
    }
}