using ConquiánCliente.ServiceGame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConquiánCliente.ViewModel.Game
{
    public class MeldViewModel : ViewModelBase
    {
        public ObservableCollection<CardViewModel> Cards { get; set; }

        public MeldViewModel(List<CardViewModel> cards)
        {
            Cards = new ObservableCollection<CardViewModel>(cards.OrderBy(c => c.Rank));
        }

        public MeldViewModel(IEnumerable<CardDto> cardDtos)
        {
            Cards = new ObservableCollection<CardViewModel>(
                cardDtos.Select(dto => new CardViewModel(dto))
                        .OrderBy(c => c.Rank)
            );
        }
    }
}
