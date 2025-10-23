using ConquiánCliente.ViewModel;
using System.Windows;

namespace ConquiánCliente.View.FriendList
{
    public partial class FriendList : Window
    {
        private FriendListViewModel viewModel;

        public FriendList()
        {
            InitializeComponent();
            viewModel = new FriendListViewModel();
            DataContext = viewModel;
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.SearchPlayer(txtBXSearchFriend.Text);
            FriendsDataGrid.Visibility = Visibility.Collapsed;
            SearchDataGrid.Visibility = Visibility.Visible;
        }
    }
}