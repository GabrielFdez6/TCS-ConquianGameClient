using ConquiánCliente.ServiceInvitation;
using System;
using System.ServiceModel;
using System.Windows;
using System.Linq;
using GameWindow = ConquiánCliente.View.Game.Game;

namespace ConquiánCliente.ViewModel
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class InvitationCallbackHandler : IInvitationServiceCallback
    {
        public static event Action<string, string> OnGlobalInvitationReceived;

        public void OnInvitationReceived(string senderNickname, string roomCode)
        {
            bool isInGame = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                isInGame = Application.Current.Windows.OfType<GameWindow>().Any();
            });

            if (isInGame)
            {
                return;
            }

            OnGlobalInvitationReceived?.Invoke(senderNickname, roomCode);
        }
    }
}