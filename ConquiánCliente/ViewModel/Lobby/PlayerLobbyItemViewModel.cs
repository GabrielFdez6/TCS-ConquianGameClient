using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquiánCliente.ViewModel.Lobby
{
    public class PlayerLobbyItemViewModel : ViewModelBase
    {
        private string displayName;
        private string profileImagePath;
        public int Id { get; set; }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; OnPropertyChanged(nameof(DisplayName)); }
        }

        public string ProfileImagePath
        {
            get { return profileImagePath; }
            set { profileImagePath = value; OnPropertyChanged(nameof(ProfileImagePath)); }
        }
    }
}
