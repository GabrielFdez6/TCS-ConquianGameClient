using ConquiánCliente.ServiceInvitation;
using System;
using System.ServiceModel;

namespace ConquiánCliente.ViewModel
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class InvitationCallbackHandler : IInvitationServiceCallback
    {
        public static event Action<string, string> OnGlobalInvitationReceived;

        public void OnInvitationReceived(string senderNickname, string roomCode)
        {
            OnGlobalInvitationReceived?.Invoke(senderNickname, roomCode);
        }
    }
}