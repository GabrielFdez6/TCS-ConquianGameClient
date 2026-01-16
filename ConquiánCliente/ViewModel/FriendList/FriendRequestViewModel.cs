using ConquiánCliente.Models;
using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages;

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

        private readonly FriendListClient friendListService;
        private readonly IMessageResolver messageResolver;

        private const int STATUS_ACCEPTED = 1;
        private const int STATUS_DECLINED = 2;

        public FriendRequestsViewModel()
        {
            friendListService = new FriendListClient();
            this.messageResolver = new ResourceMessageResolver();

            Requests = new ObservableCollection<FriendRequest>();
            AcceptRequestCommand = new RelayCommand(AcceptRequest);
            DeclineRequestCommand = new RelayCommand(DeclineRequest);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            PresenceCallbackHandler.FriendRequestReceived += OnFriendRequestReceived;
        }

        public async Task InitializeAsync()
        {
            await LoadFriendRequests();
        }

        public void Cleanup()
        {
            PresenceCallbackHandler.FriendRequestReceived -= OnFriendRequestReceived;
        }

        private void OnFriendRequestReceived()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                Requests.Clear();
                await LoadFriendRequests();
            });
        }

        private async Task LoadFriendRequests()
        {
            try
            {
                var requestsList = await friendListService.GetFriendRequestsAsync(PlayerSession.CurrentPlayer.idPlayer);
                Requests.Clear();

                if (requestsList != null)
                {
                    foreach (var req in requestsList)
                    {
                        Requests.Add(new FriendRequest { IdFriendship = req.IdFriendship, Nickname = req.Nickname });
                    }
                }
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private async void AcceptRequest(object parameter)
        {
            if (parameter is FriendRequest request)
            {
                await ProcessRequestUpdate(request, STATUS_ACCEPTED);
            }
        }

        private async void DeclineRequest(object parameter)
        {
            if (parameter is FriendRequest request)
            {
                await ProcessRequestUpdate(request, STATUS_DECLINED);
            }
        }

        private async Task ProcessRequestUpdate(FriendRequest request, int status)
        {
            try
            {
                await friendListService.UpdateFriendRequestStatusAsync(request.IdFriendship, status);
                Requests.Remove(request);
            }
            catch (FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                HandleFaultException(fault, request);
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private void HandleFaultException(FaultException<ServiceFriendList.ServiceFaultDto> fault, FriendRequest request)
        {
            var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
            string msg = messageResolver.GetMessage(errorType);
            MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);

            if (errorType == ConquiánCliente.ServiceLogin.ServiceErrorType.NotFound)
            {
                Requests.Remove(request);
            }
        }

        private void ExecuteBackCommand(object parameter)
        {
            Cleanup();

            if (parameter is Window currentWindow)
            {
                var friendListWindow = new View.FriendList.FriendList();
                friendListWindow.Show();
                currentWindow.Close();
            }
        }

        private void HandleServiceException(Exception ex)
        {
            if (ex is FaultException<ServiceFriendList.ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ex is EndpointNotFoundException || ex is CommunicationException || ex is TimeoutException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}