using ConquiánCliente.ViewModel.FriendList;
using System;
using System.Windows;

namespace ConquiánCliente.View.FriendList
{

    public partial class FriendRequests : Window
    {
        private readonly FriendRequestsViewModel viewModel;
        public FriendRequests()
        {
            InitializeComponent();
            viewModel = new FriendRequestsViewModel();
            DataContext = viewModel;
            Loaded += OnWindowLoaded;
            Closed += OnWindowClosed;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await viewModel.InitializeAsync();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            viewModel.Cleanup();
        }
    }
}