using ConquiánCliente.ViewModel.FriendList;
using System.Windows;

namespace ConquiánCliente.View.FriendList
{
    /// <summary>
    /// Interaction logic for FriendRequests.xaml
    /// </summary>
    public partial class FriendRequests : Window
    {
        private readonly FriendRequestsViewModel viewModel;
        public FriendRequests()
        {
            InitializeComponent();
            viewModel = new FriendRequestsViewModel();
            DataContext = viewModel;
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await viewModel.InitializeAsync();
        }
    }
}