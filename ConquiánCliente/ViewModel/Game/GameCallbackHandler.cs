using ConquiánCliente.ServiceGame; 
using System;
using System.ServiceModel;

namespace ConquiánCliente.ViewModel.Game
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class GameCallbackHandler : IGameCallback
    {

        public event Action<GameStateDto> OnGameUpdate;
        public event Action OnOpponentDrewDeck;
        public event Action<CardDto> OnOpponentDiscarded;
        public event Action<int> TimeUpdated;

        public void OnTimeUpdated(int remainingSeconds)
        {
            TimeUpdated?.Invoke(remainingSeconds);
        }
        public void NotifyGameUpdate(GameStateDto newState)
        {
            OnGameUpdate?.Invoke(newState);
        }

        public void NotifyOpponentDrewDeck()
        {
            OnOpponentDrewDeck?.Invoke();
        }

        public void NotifyOpponentDiscarded(CardDto card)
        {
            OnOpponentDiscarded?.Invoke(card);
        }
    }
}