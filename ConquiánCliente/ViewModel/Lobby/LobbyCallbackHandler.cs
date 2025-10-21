using ConquiánCliente.ServiceLobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConquiánCliente.ViewModel.Lobby
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class LobbyCallbackHandler : ILobbyCallback
    {
        public event Action<PlayerDto> OnPlayerJoined;
        public event Action<int> OnPlayerLeft;
        public event Action OnHostLeft;
        public event Action<MessageDto> OnMessageReceived;

        public void HostLeft()
        {
            OnHostLeft?.Invoke();
        }

        public void MessageReceived(MessageDto message)
        {
            OnMessageReceived?.Invoke(message);
        }

        public void PlayerJoined(PlayerDto newPlayer)
        {
            OnPlayerJoined?.Invoke(newPlayer);
        }

        public void PlayerLeft(int idPlayer)
        {
            OnPlayerLeft?.Invoke(idPlayer);
        }
    }
}
