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

            if (client != null && client.State != CommunicationState.Opened)
            {
                try
                {
                    client.Abort();
                }
                catch
                {
                }
                client = null;
            }

            if (client != null && client.State == CommunicationState.Opened)
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
            catch (Exception)
            {
                MessageBox.Show(Lang.ErrorConnectingToServer, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void Disconnect(int idPlayer)
        {
            if (client != null)
            {
                try
                {
                    if (client.State == CommunicationState.Opened)
                    {
                        client.Unsubscribe(idPlayer);
                        client.Close();
                    }
                    else
                    {
                        client.Abort();
                    }
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
        }

        public static async Task SendInvitationAsync(int idSender, string senderNickname, int idReceiver, string roomCode)
        {
            if (client == null || client.State != CommunicationState.Opened)
            {
                Connect(idSender);
            }

            if (client != null && client.State == CommunicationState.Opened)
            {
                await client.SendInvitationAsync(idSender, senderNickname, idReceiver, roomCode);
            }
            else
            {
                throw new CommunicationException(Lang.ErrorConnectingToServer);
            }
        }

        public static async Task ReconnectAsync(int idPlayer)
        {
            Disconnect(idPlayer);

            await Task.Run(() => Connect(idPlayer));
        }

    }
}