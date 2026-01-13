using ConquiánCliente.ServiceLobby;
using System;
using System.ServiceModel;


namespace ConquiánCliente.ViewModel.Lobby
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class LobbyCallbackHandler : ILobbyCallback
    {
        private static readonly LobbyCallbackHandler instance = new LobbyCallbackHandler();

        private LobbyCallbackHandler() { }

        public static LobbyCallbackHandler Instance
        {
            get { return instance; }
        }

        public event Action<PlayerDto> OnPlayerJoined;
        public event Action<int> OnPlayerLeft;
        public event Action OnHostLeft;
        public event Action<MessageDto> OnMessageReceived;
        public event Action<int> OnGamemodeChanged;
        public event Action OnGameStarting;
        public event Action OnYouWereKicked;

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
        public void NotifyGamemodeChanged(int newGamemodeId)
        {
            OnGamemodeChanged?.Invoke(newGamemodeId);
        }

        public void NotifyGameStarting()
        {
            OnGameStarting?.Invoke();
        }

        public void YouWereKicked()
        {
            OnYouWereKicked?.Invoke();
        }

        public bool Ping()
        {
            return true;
        }
    }
}
