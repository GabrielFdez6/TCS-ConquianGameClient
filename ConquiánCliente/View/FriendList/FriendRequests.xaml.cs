using System.Collections.Generic; 
using System.Windows;

namespace ConquiánCliente.View.FriendList
{
    /// <summary>
    /// Interaction logic for FriendRequests.xaml
    /// </summary>
    public partial class FriendRequests : Window
    {
        public FriendRequests()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            List<FriendRequest> requests = new List<FriendRequest>
            {
                new FriendRequest() { Nickname = "Player123" },
                new FriendRequest() { Nickname = "xX_Gamer_Xx" },
                new FriendRequest() { Nickname = "LolaBunny" },
                new FriendRequest() { Nickname = "MasterChief" }
            };

            RequestsDataGrid.ItemsSource = requests;
        }
    }

    public class FriendRequest
    {
        public string Nickname { get; set; }
    }
}