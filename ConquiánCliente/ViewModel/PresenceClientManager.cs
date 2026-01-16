using ConquiánCliente.ServicePresence;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private bool IsClientInvalid()
        {
            bool isInvalid = client == null ||
                             client.State == CommunicationState.Closed ||
                             client.State == CommunicationState.Faulted;
            return isInvalid;
        }

        private void InitializeClient()
        {
            CleanupExistingClient();

            var context = new InstanceContext(new PresenceCallbackHandler());
            client = new PresenceClient(context);

            if (client.InnerChannel != null)
            {
                client.InnerChannel.Closed += OnConnectionLost;
                client.InnerChannel.Faulted += OnConnectionLost;
            }
        }

        private void CleanupExistingClient()
        {
            if (client == null)
            {
                return;
            }

            try
            {
                if (client.InnerChannel != null)
                {
                    client.InnerChannel.Closed -= OnConnectionLost;
                    client.InnerChannel.Faulted -= OnConnectionLost;
                }

                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
            }
        }

        private static void OnConnectionLost(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ShouldSkipDisconnectionHandling())
                {
                    return;
                }

                HandleDisconnectionSequence();
            });
        }

        private static bool ShouldSkipDisconnectionHandling()
        {
            bool skip = PlayerSession.IsNetworkDown || !PlayerSession.IsLoggedIn;
            return skip;
        }

        private static void HandleDisconnectionSequence()
        {
            PlayerSession.IsNetworkDown = true;

            MessageBox.Show(Lang.ErrorLostConnection, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);

            TransitionToLogin();

            PlayerSession.IsNetworkDown = false;
        }

        private static void TransitionToLogin()
        {
            LogIn loginWindow = new LogIn();
            loginWindow.Show();

            CloseOtherWindows(loginWindow);

            Application.Current.MainWindow = loginWindow;
            PlayerSession.EndSession();
        }

        private static void CloseOtherWindows(Window keepOpenWindow)
        {
            List<Window> windowsToClose = Application.Current.Windows.OfType<Window>()
                .Where(w => w != keepOpenWindow)
                .ToList();

            foreach (Window window in windowsToClose)
            {
                window.Close();
            }
        }
    }
}