using System;
using System.Net.NetworkInformation;

namespace ConquianClient.Utilities
{
    public class NetworkConnectionMonitor
    {
        public event Action OnNetworkStatusLost;

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
        }

        public void StopMonitoring()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
        }
    }
}