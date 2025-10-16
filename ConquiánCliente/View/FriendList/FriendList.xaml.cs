using ConquiánCliente.ViewModel;
using System.Windows;

namespace ConquiánCliente.View.FriendList
{
    public partial class FriendList : Window
    {
        private FriendListViewModel ViewModel;

        public FriendList()
        {
            InitializeComponent();
            ViewModel = new FriendListViewModel();
            DataContext = ViewModel;
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.SearchPlayer(txtBXSearchFriend.Text);
            FriendsDataGrid.Visibility = Visibility.Collapsed;
            SearchDataGrid.Visibility = Visibility.Visible;
        }
    }
}