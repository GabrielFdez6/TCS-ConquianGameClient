using System;
using ConquiánCliente.ServicePresence;

namespace ConquiánCliente.ViewModel
{
    public class PresenceCallbackHandler : IPresenceCallback
    {
        public static event Action<int, int> FriendStatusChanged;
        public static event Action FriendRequestReceived;
        public static event Action FriendListUpdated;

        public void OnFriendStatusChanged(int friendId, int newStatusId)
        {
            FriendStatusChanged?.Invoke(friendId, newStatusId);
        }

        public void OnFriendRequestReceived()
        {
            FriendRequestReceived?.Invoke();
        }

        public void OnFriendListUpdated()
        {
            FriendListUpdated?.Invoke();
        }
    }
}
