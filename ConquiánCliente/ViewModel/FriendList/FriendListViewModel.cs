using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceFriendList;
using ConquiánCliente.View.MainMenu; 
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel
{
    public class FriendListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<PlayerDto> Friends;

        private ObservableCollection<PlayerDto> _searchResult;


        public ICommand ViewProfileCommand { get; }
        public ICommand AddFriendCommand { get; }
        public ICommand RequestsCommand { get; }
        public ICommand BackCommand { get; }
        public ObservableCollection<PlayerDto> friends
        {
            get { return Friends; }
            set { Friends = value; OnPropertyChanged(nameof(friends)); }
        }

        public ObservableCollection<PlayerDto> SearchResult
        {
            get { return _searchResult; }
            set { _searchResult = value; OnPropertyChanged(nameof(SearchResult)); }
        }

        private readonly FriendListClient FriendListService;

        public FriendListViewModel()
        {
            FriendListService = new FriendListClient();
            friends = new ObservableCollection<PlayerDto>();
            SearchResult = new ObservableCollection<PlayerDto>();
            ViewProfileCommand = new RelayCommand(p => { /* Lógica para ver perfil */ });
            AddFriendCommand = new RelayCommand(AddFriend);
            RequestsCommand = new RelayCommand(ExecuteRequestsCommand); 
            BackCommand = new RelayCommand(ExecuteBackCommand);
            LoadFriends();
        }

        private async void LoadFriends()
        {
            var friendsList = await FriendListService.GetFriendsAsync(PlayerSession.CurrentPlayer.idPlayer);
            friends = new ObservableCollection<PlayerDto>(friendsList);
        }

        public async void SearchPlayer(string nickname)
        {
            var player = await FriendListService.GetPlayerByNicknameAsync(nickname, PlayerSession.CurrentPlayer.idPlayer);
            SearchResult.Clear();
            if (player != null)
            {
                SearchResult.Add(player);
            }
        }

        private async void AddFriend(object parameter)
        {
            if (parameter is PlayerDto player)
            {
                var success = await FriendListService.SendFriendRequestAsync(PlayerSession.CurrentPlayer.idPlayer, player.idPlayer);
                if (success)
                {
                    MessageBox.Show(Lang.FriendRequestSentSuccess, Lang.TitleSuccess);
                } else
                {
                    MessageBox.Show(Lang.FriendRequestSentError, Lang.TitleError);
                }
            }
        }

        private void ExecuteRequestsCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var requestsWindow = new View.FriendList.FriendRequests();
                requestsWindow.Show();
                currentWindow.Close();
            }
        }

        private void ExecuteBackCommand(object parameter)
        {
            if (parameter is Window currentWindow)
            {
                var mainMenu = new View.MainMenu.MainMenu();
                mainMenu.Show();
                currentWindow.Close();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}