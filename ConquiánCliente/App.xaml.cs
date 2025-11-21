using ConquiánCliente.ServiceLogin;
using ConquiánCliente.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ConquiánCliente
{

    public partial class App : Application
    {
        public App()
        {
            this.Exit += App_Exit;
        }

        private static void App_Exit(object sender, ExitEventArgs e)
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

            base.OnStartup(e);
        }
    }
}
