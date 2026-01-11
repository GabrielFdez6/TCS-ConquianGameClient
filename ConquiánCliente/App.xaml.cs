using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;
using ConquiánCliente.Utilities;
using ConquiánCliente.View.Lobby;
using ConquiánCliente.ViewModel;
using ConquiánCliente.ViewModel.Lobby;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ConquiánCliente
{
    public partial class App : Application
    {
        private const int RECONNECTION_TIMEOUT_SECONDS = 60;

        private NetworkConnectionMonitor networkMonitor;
        private DispatcherTimer reconnectionTimer;
        private Window reconnectingWindow;

        public App()
        {
            this.Exit += AppExit;
        }

        private static void AppExit(object sender, ExitEventArgs e)
        {
            if (PlayerSession.IsLoggedIn && PlayerSession.CurrentPlayer != null)
            {
                try
                {
                    var loginClient = new LoginClient();
                    loginClient.SignOutPlayerAsync(PlayerSession.CurrentPlayer.idPlayer).GetAwaiter().GetResult();
                    PresenceClientManager.Instance.Client.Unsubscribe(PlayerSession.CurrentPlayer.idPlayer);
                    InvitationClientManager.Disconnect(PlayerSession.CurrentPlayer.idPlayer);
                    PlayerSession.EndSession();
                    AudioManager.Instance.StopMusic();
                }
                catch (CommunicationException commEx)
                {
                    Console.WriteLine($"Error de comunicación al desconectar: {commEx.Message}");
                }
                catch (TimeoutException timeoutEx)
                {
                    Console.WriteLine($"Tiempo de espera agotado al desconectar: {timeoutEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado al desconectar: {ex.Message}");
                }
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            reconnectionTimer = new DispatcherTimer();
            reconnectionTimer.Interval = TimeSpan.FromSeconds(RECONNECTION_TIMEOUT_SECONDS);
            reconnectionTimer.Tick += OnReconnectionTimeout;

            networkMonitor = new NetworkConnectionMonitor();
            networkMonitor.OnNetworkStatusLost += HandleNetworkLost;
            networkMonitor.OnNetworkStatusRestored += HandleNetworkRestored;

            var settings = ConquiánCliente.Properties.Settings.Default;

            if (string.IsNullOrEmpty(settings.languageCode))
            {
                var osLanguage = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;

                if (osLanguage == "es")
                {
                    settings.languageCode = "es-MX";
                }
                else
                {
                    settings.languageCode = "en-US";
                }

                settings.Save();
            }

            var langCode = settings.languageCode;
            var culture = new CultureInfo(langCode);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            try
            {
                double savedVolume = settings.MusicVolume;

                AudioManager.Instance.SetVolume(savedVolume);
                AudioManager.Instance.PlayMenuMusic();
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                Console.WriteLine($"No se encontró la configuración de volumen, usando valor por defecto: {ex.Message}");

                AudioManager.Instance.SetVolume(50);
                AudioManager.Instance.PlayMenuMusic();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar el sistema de audio: {ex.Message}");
            }
            InvitationCallbackHandler.OnGlobalInvitationReceived += ShowInvitationPopup;
            base.OnStartup(e);
        }

        private void HandleNetworkLost()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (!PlayerSession.IsLoggedIn)
                {
                    return;
                }

                PlayerSession.IsNetworkDown = true;

                if (reconnectingWindow == null)
                {
                    reconnectionTimer.Start();

                    reconnectingWindow = new ConquiánCliente.View.Utilities.ReconnectionWindow();
                    reconnectingWindow.Show();
                }
            });
        }

        private void HandleNetworkRestored()
        {
            this.Dispatcher.Invoke(async () =>
            {
                if (reconnectionTimer.IsEnabled)
                {
                    reconnectionTimer.Stop();
                    await System.Threading.Tasks.Task.Delay(5000);
                    PlayerSession.IsNetworkDown = false;

                    if (reconnectingWindow != null)
                    {
                        reconnectingWindow.Close();
                        reconnectingWindow = null;
                    }
                }
            });
        }

        private void OnReconnectionTimeout(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                reconnectionTimer.Stop();

                if (reconnectingWindow != null)
                {
                    reconnectingWindow.Close();
                    reconnectingWindow = null;
                }

                PerformLogoutAndNavigate();
            });
        }

        private void PerformLogoutAndNavigate()
        {
            if (!PlayerSession.IsLoggedIn) return;

            MessageBox.Show(Lang.ErrorLostConnection, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);

            PlayerSession.EndSession();
            NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            LogIn loginWindow = new LogIn();
            loginWindow.Show();

            List<Window> windowsToClose = new List<Window>();

            foreach (Window window in Application.Current.Windows)
            {
                if (window != loginWindow)
                {
                    windowsToClose.Add(window);
                }
            }

            foreach (Window window in windowsToClose)
            {
                window.Close();
            }

            Application.Current.MainWindow = loginWindow;
            PlayerSession.IsNetworkDown = false;

        }

        private void ShowInvitationPopup(string senderNickname, string roomCode)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is InvitationReceived)
                    {
                        return;
                    }
                }

                var vm = new InvitationReceivedViewModel(senderNickname, roomCode);

                var invitationWindow = new InvitationReceived();

                invitationWindow.DataContext = vm;

                invitationWindow.Show();
            });
        }
    }
}