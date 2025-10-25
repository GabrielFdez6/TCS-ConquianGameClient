using System;
using ConquiánCliente.ServicePresence;

namespace ConquiánCliente.ViewModel
{
    public class PresenceCallbackHandler : IPresenceCallback
    {
        public static event Action<int, int> FriendStatusChanged;

        public void OnFriendStatusChanged(int friendId, int newStatusId)
        {
            FriendStatusChanged?.Invoke(friendId, newStatusId);
        }
    }
}
