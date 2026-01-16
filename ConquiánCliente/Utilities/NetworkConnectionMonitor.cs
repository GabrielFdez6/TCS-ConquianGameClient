using System;
using System.Net.NetworkInformation;

namespace ConquiánCliente.Utilities
{
    public class NetworkConnectionMonitor
    {
        public event Action OnNetworkStatusLost;
        public event Action OnNetworkStatusRestored;

        public NetworkConnectionMonitor()
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
            {
                OnNetworkStatusLost?.Invoke();
            }
            else
            {
                OnNetworkStatusRestored?.Invoke();
            }
        }
    }
}