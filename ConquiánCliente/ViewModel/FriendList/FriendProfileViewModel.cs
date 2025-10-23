using ConquiánCliente.ServiceUserProfile;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.FriendList
{
    public class FriendProfileViewModel : ViewModelBase
    {
        private PlayerDto player;
        public PlayerDto Player
        {
            get => player;
            set { player = value; OnPropertyChanged(nameof(Player)); }
        }

        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }

       
        public ICommand BackCommand { get; }

        public FriendProfileViewModel(PlayerDto player, ObservableCollection<SocialDto> socials)
        {
            Player = player;
            BackCommand = new RelayCommand(ExecuteBackCommand);
            LoadSocials(socials);
        }

        private void LoadSocials(ObservableCollection<SocialDto> socials)
        {
            if (socials != null)
            {
                FacebookLink = socials.FirstOrDefault(s => s.IdSocialType == 2)?.UserLink;
                InstagramLink = socials.FirstOrDefault(s => s.IdSocialType == 1)?.UserLink;

                OnPropertyChanged(nameof(FacebookLink));
                OnPropertyChanged(nameof(InstagramLink));
            }
        }

        private void ExecuteBackCommand(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }
}