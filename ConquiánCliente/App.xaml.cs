using ConquiánCliente.ServiceLogin;
using ConquiánCliente.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ConquiánCliente
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
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
            var langCode = ConquiánCliente.Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);
            base.OnStartup(e);
        }
    }
}
