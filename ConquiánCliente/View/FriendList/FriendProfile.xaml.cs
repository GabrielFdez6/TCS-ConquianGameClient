using ConquiánCliente.ViewModel.FriendList;
using ConquiánCliente.ServiceUserProfile;
using System.Collections.ObjectModel;
using System.Windows;

namespace ConquiánCliente.View.FriendList
{
    public partial class FriendProfile : Window
    {
        public FriendProfile(PlayerDto player, ObservableCollection<SocialDto> socials)
        {
            InitializeComponent();
            DataContext = new FriendProfileViewModel(player, socials);
        }
    }
}