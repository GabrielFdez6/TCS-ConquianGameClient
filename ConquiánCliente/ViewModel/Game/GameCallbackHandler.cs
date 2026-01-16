using ConquiánCliente.ServiceGame;
using System;
using System.ServiceModel;

namespace ConquiánCliente.ViewModel.Game
{
    [CallbackBehavior(UseSynchronizationContext = true)]
    public class GameCallbackHandler : IGameCallback
    {

        public event Action<GameStateDto> OnGameUpdate;
        public event Action OnOpponentDrewDeck;
        public event Action<CardDto> OnOpponentDiscarded;
        public event Action<int, int, int> TimeStateUpdated;
        public event Action<int> OpponentHandUpdated;
        public event Action<CardDto[]> OnOpponentMeld;
        public event Action OnOpponentLeftEvent;
        public event Action<GameResultDto> OnGameEnded;
        public event Action<string> OnGameEndedByAfkEvent;

        public void OnTimeUpdated(int gameRemainingSeconds, int turnRemainingSeconds, int currentTurnPlayerId)
        {
            TimeStateUpdated?.Invoke(gameRemainingSeconds, turnRemainingSeconds, currentTurnPlayerId);
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

        public void OnOpponentHandUpdated(int newCardCount)
        {
            OpponentHandUpdated?.Invoke(newCardCount);
        }

        public void NotifyOpponentMeld(CardDto[] meldCards)
        {
            OnOpponentMeld?.Invoke(meldCards);
        }

        public void OnOpponentLeft()
        {
            OnOpponentLeftEvent?.Invoke();
        }
        public void NotifyGameEnded(GameResultDto result)
        {
            OnGameEnded?.Invoke(result);
        }

        public void NotifyGameEndedByAFK(string reasonKey)
        {
            OnGameEndedByAfkEvent?.Invoke(reasonKey);
        }
    }
}