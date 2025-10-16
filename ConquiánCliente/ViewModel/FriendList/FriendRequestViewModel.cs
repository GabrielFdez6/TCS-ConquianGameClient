using ConquiánCliente.Models;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.View.FriendList;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.FriendList
{
    public class FriendRequestsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<FriendRequest> requests;
        public ObservableCollection<FriendRequest> Requests
        {
            get { return requests; }
            set { requests = value; OnPropertyChanged(nameof(Requests)); }
        }

        public ICommand AcceptRequestCommand { get; }
        public ICommand DeclineRequestCommand { get; }
        public ICommand BackCommand { get; }

        private readonly FriendListClient FriendListService;

        public FriendRequestsViewModel()
        {
            FriendListService = new FriendListClient();
            Requests = new ObservableCollection<FriendRequest>();
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            DeclineRequestCommand = new RelayCommand(DeclineRequest);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            LoadFriendRequests();
        }

        private async void LoadFriendRequests()
        {
            var requestsList = await FriendListService.GetFriendRequestsAsync(PlayerSession.CurrentPlayer.idPlayer);
            if (requestsList != null)
            {
                foreach (var req in requestsList)
                {
                    Requests.Add(new FriendRequest { IdFriendship = req.IdFriendship, Nickname = req.Nickname });
                }
            }
        }

        private async void AcceptRequest(object parameter)
        {
            if (parameter is FriendRequest request)
            {
                bool success = await FriendListService.UpdateFriendRequestStatusAsync(request.IdFriendship, 1); 
                if (success)
                {
                    Requests.Remove(request);
                }
            }
        }

        private async void DeclineRequest(object parameter)
        {
            if (parameter is FriendRequest request)
            {
                bool success = await FriendListService.UpdateFriendRequestStatusAsync(request.IdFriendship, 2);
                if (success)
                {
                    Requests.Remove(request);
                }
            }
        }

        private void ExecuteBackCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var friendListWindow = new View.FriendList.FriendList();
                friendListWindow.Show();
                currentWindow.Close();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}