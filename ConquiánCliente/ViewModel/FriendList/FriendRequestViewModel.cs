using ConquiánCliente.Models;
using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.View.FriendList;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
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
        }

        public async Task InitializeAsync()
        {
            await LoadFriendRequests();
        }
        private async Task LoadFriendRequests()
        {
            try
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
            catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                MessageBox.Show(fault.Detail.Message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        private async void AcceptRequest(object parameter)
        {
            if (parameter is FriendRequest request)
            {
                try
                {
                    await FriendListService.UpdateFriendRequestStatusAsync(request.IdFriendship, 1);

                    Requests.Remove(request);
                }
                catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
                {
                    MessageBox.Show(fault.Detail.Message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    if (fault.Detail.ErrorType == ServiceFriendList.ServiceErrorType.NotFound)
                    {
                        Requests.Remove(request);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
                }
            }
        }

        private async void DeclineRequest(object parameter)
        {
            if (parameter is FriendRequest request)
            {
                try
                {
                    await FriendListService.UpdateFriendRequestStatusAsync(request.IdFriendship, 2);

                    Requests.Remove(request);
                }
                catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
                {
                    MessageBox.Show(fault.Detail.Message, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
                    if (fault.Detail.ErrorType == ServiceFriendList.ServiceErrorType.NotFound)
                    {
                        Requests.Remove(request);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
                }
            }
        }

        private static void ExecuteBackCommand(object parameter)
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