using ConquiánCliente.ServiceGame; 

namespace ConquiánCliente.ViewModel.Game
{
    public class CardViewModel : ViewModelBase
    {
        public CardDto Card { get; }

        public string Id => Card.Id;
        public string Suit => Card.Suit;
        public int Rank => Card.Rank;
        public string ImagePath => Card.ImagePath;

        private bool isSelected;
        private bool isPlayable;
        private bool isBeingDragged;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));

            }
        }

        public bool IsPlayable
        {
            get { return isPlayable; }
            set
            {
                isPlayable = value;
                OnPropertyChanged(nameof(IsPlayable));
            }
        }

        public bool IsBeingDragged
        {
            get { return isBeingDragged; }
            set
            {
                isBeingDragged = value;
                OnPropertyChanged(nameof(IsBeingDragged));
            }
        }

        public CardViewModel(CardDto cardDto)
        {
            Card = cardDto;

            isSelected = false;
            isPlayable = true;
            IsBeingDragged = false;
        }
    }
}