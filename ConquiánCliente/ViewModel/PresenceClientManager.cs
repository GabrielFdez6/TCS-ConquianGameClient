using System.ServiceModel;
using ConquiánCliente.ServicePresence; 

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
            InitializeClient();
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
                catch
                {
                }
            }

            var context = new InstanceContext(new PresenceCallbackHandler());
            client = new PresenceClient(context);
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
    }
}