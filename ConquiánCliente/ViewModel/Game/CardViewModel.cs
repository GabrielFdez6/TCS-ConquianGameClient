using ConquiánCliente.ServiceGame; 

namespace ConquiánCliente.ViewModel.Game
{
    public class CardViewModel : ViewModelBase
    {
        public string Id { get; }
        public string Suit { get; }
        public int Rank { get; }
        public string ImagePath { get; }

        private bool isSelected;
        private bool isPlayable;
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

        public CardViewModel(CardDto cardDto)
        {
            Id = cardDto.Id;
            Suit = cardDto.Suit;
            Rank = cardDto.Rank;
            ImagePath = cardDto.ImagePath;

            isSelected = false;
            isPlayable = true; 
        }
    }
}