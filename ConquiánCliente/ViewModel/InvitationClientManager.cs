using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceInvitation;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace ConquiánCliente.ViewModel
{
    public static class InvitationClientManager
    {
        private static InvitationServiceClient client;

        public static void Connect(int idPlayer)
        {
            if (IsClientConnected())
            {
                return;
            }

            try
            {
                var callbackHandler = new InvitationCallbackHandler();
                var context = new InstanceContext(callbackHandler);
                client = new InvitationServiceClient(context);
                client.Subscribe(idPlayer);
            }
            catch (EndpointNotFoundException)
            {
                ShowConnectionError(Lang.ErrorServerUnavailable);
            }
            catch (TimeoutException)
            {
                ShowConnectionError(Lang.ErrorConnectingToServer);
            }
            catch (CommunicationException)
            {
                ShowConnectionError(Lang.ErrorConnectingToServer);
            }
        }



        public static void Disconnect(int idPlayer)
        {
            if (client == null)
            {
                return;
            }

            try
            {
                if (client.State == CommunicationState.Opened)
                {
                    client.Unsubscribe(idPlayer);
                }
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {

            }
            finally
            {
                SafeCloseClient();
            }
        }

        public static async Task SendInvitationAsync(InvitationSenderDto sender, int idReceiver, string roomCode)
        {
            if (!IsClientConnected())
            {
                Connect(sender.IdPlayer);
            }

            if (IsClientConnected())
            {
                await client.SendInvitationAsync(sender, idReceiver, roomCode);
            }
            else
            {
                throw new CommunicationException(Lang.ErrorConnectingToServer);
            }
        }

        private static bool IsClientConnected()
        {
            bool isConnected = client != null && client.State == CommunicationState.Opened;
            return isConnected;
        }

        private static void SafeCloseClient()
        {
            if (client == null)
            {
                return;
            }

            try
            {
                if (client.State == CommunicationState.Opened)
                {
                    client.Close();
                }
                else
                {
                    client.Abort();
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
            finally
            {
                client = null;
            }
        }

        private static void ShowConnectionError(string message)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}