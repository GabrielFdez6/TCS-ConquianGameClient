using ConquiánCliente.ServicePresence;
using System;
using System.ServiceModel;
using System.Windows;
using ConquiánCliente.Properties.Langs;

namespace ConquiánCliente.ViewModel
{
    public class PresenceClientManager
    {
        private static PresenceClientManager instance;
        private PresenceClient client;

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
                    client.InnerChannel.Closed -= OnConnectionLost;
                    client.InnerChannel.Faulted -= OnConnectionLost;

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

            client.InnerChannel.Closed += OnConnectionLost;
            client.InnerChannel.Faulted += OnConnectionLost;
        }

        public void Reconnect(int idPlayer)
        {
            try
            {
                InitializeClient();
                client.Open();
                client.Subscribe(idPlayer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reconectar servicio de presencia: {ex.Message}");
                if (client != null) client.Abort();
            }
        }

        private static void OnConnectionLost(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (PlayerSession.IsNetworkDown)
                {
                    return;
                }

                if (!PlayerSession.IsLoggedIn)
                {
                    return;
                }

                PlayerSession.IsNetworkDown = true;

                MessageBox.Show(Lang.ErrorLostConnection, Lang.TitleError,
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                LogIn loginWindow = new LogIn();
                loginWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != loginWindow)
                    {
                        window.Close();
                    }
                }

                Application.Current.MainWindow = loginWindow;
                PlayerSession.EndSession();
                PlayerSession.IsNetworkDown = false;
            });
        }
    }
}