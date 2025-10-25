using System.ServiceModel;
using ConquiánCliente.ServicePresence; 
using ConquiánCliente.ViewModel; 

namespace ConquiánCliente.ViewModel
{
    public class PresenceClientManager
    {
        private static PresenceClientManager instance;
        public PresenceClient Client { get; private set; }

        private PresenceClientManager()
        {

            var context = new InstanceContext(new PresenceCallbackHandler());
            Client = new PresenceClient(context);
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