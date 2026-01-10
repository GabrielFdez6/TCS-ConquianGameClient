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

    }
}