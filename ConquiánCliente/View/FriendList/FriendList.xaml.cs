using ConquiánCliente.ViewModel;
using System;
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

        private async void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            await viewModel.SearchPlayer(txtBXSearchFriend.Text);
            FriendsDataGrid.Visibility = Visibility.Collapsed;
            SearchDataGrid.Visibility = Visibility.Visible;
        }
    }
}