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
        private static InvitationCallbackHandler callbackHandler;
        private static InstanceContext context;

        public static void Connect(int idPlayer)
        {
            if (client != null && client.State == CommunicationState.Opened)
            {
                return; 
            }

            try
            {
                callbackHandler = new InvitationCallbackHandler();
                context = new InstanceContext(callbackHandler);
                client = new InvitationServiceClient(context);
                client.Subscribe(idPlayer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.ErrorUnexpected );
            }
        }

        public static void Disconnect(int idPlayer)
        {
            if (client != null && client.State == CommunicationState.Opened)
            {
                try
                {
                    client.Unsubscribe(idPlayer);
                    client.Close();
                }
                catch (Exception)
                {
                    client.Abort();
                }
            }
            client = null;
        }

        public static async Task<bool> SendInvitation(int idSender, string senderNickname, int idReceiver, string roomCode)
        {
            if (client == null || client.State != CommunicationState.Opened)
            {
                Connect(idSender); 
            }

            try
            {
                return await client.SendInvitationAsync(idSender, senderNickname, idReceiver, roomCode);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}