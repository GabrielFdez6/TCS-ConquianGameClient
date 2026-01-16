using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ViewModel.Game;
using System.Collections.Generic;
using System.Linq;

namespace ConquiánCliente.ViewModel.Validation
{
    public static class ConquianRulesValidator
    {
        private const int MINIMUM_MELD_SIZE = 3;
        private const int RANK_INCREMENT = 1;
        private const int RANK_BEFORE_SKIP = 7;
        private const int RANK_AFTER_SKIP = 10;

        public static string ValidateMeld(List<CardViewModel> cards)
        {
            string validationError = string.Empty;

            if (cards == null || cards.Count < MINIMUM_MELD_SIZE)
            {
                return Lang.GameInvalidMeld;
            }

            var orderedCards = cards.OrderBy(c => c.Rank).ToList();

            if (IsTercia(orderedCards))
            {
                return string.Empty;
            }

            if (!IsCorrida(orderedCards))
            {
                return Lang.GameInvalidMeld;
            }

            if (!IsSequential(orderedCards))
            {
                return Lang.GameInvalidMeld;
            }

            return string.Empty;
        }

        private static bool IsTercia(List<CardViewModel> cards)
        {
            bool sameRank = cards.All(c => c.Rank == cards[0].Rank);
            bool distinctSuits = cards.Select(c => c.Suit).Distinct().Count() == cards.Count;

            return sameRank && distinctSuits;
        }

        private static bool IsCorrida(List<CardViewModel> cards)
        {
            return cards.All(c => c.Suit == cards[0].Suit);
        }

        private static bool IsSequential(List<CardViewModel> cards)
        {
            bool isSequential = true;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                int currentRank = cards[i].Rank;
                int nextRank = cards[i + 1].Rank;

                if (currentRank == RANK_BEFORE_SKIP && nextRank == RANK_AFTER_SKIP)
                {
                    continue;
                }

                if (nextRank != currentRank + RANK_INCREMENT)
                {
                    isSequential = false;
                    break;
                }
            }

            return isSequential;
        }
    }
}
