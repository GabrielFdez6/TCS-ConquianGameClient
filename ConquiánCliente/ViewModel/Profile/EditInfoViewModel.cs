using ConquiánCliente.View;
using ConquiánCliente.View.Profile;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Profile
{
    public class EditInfoViewModel : ViewModelBase
    {
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }


        public ICommand SaveChangesCommand { get; }
        public ICommand CancelCommand { get; }

        public EditInfoViewModel()
        {
            LoadEditablePlayerData();

            SaveChangesCommand = new RelayCommand(p => ExecuteSaveChanges());
            CancelCommand = new RelayCommand(p => ProfileMainFrame.MainFrame.Navigate(new UserProfilePage()));
        }

        private void LoadEditablePlayerData()
        {
            if (PlayerSession.IsLoggedIn)
            {
                var currentPlayer = PlayerSession.CurrentPlayer;
                Nickname = currentPlayer.nickname;
                Email = currentPlayer.email;
                Name = currentPlayer.name;
                LastName = currentPlayer.lastName;
            }
        }

        private void ExecuteSaveChanges()
        {
            // --- Lógica para guardar cambios (pendiente) ---
            // Aquí llamarías a un nuevo servicio para actualizar Name, LastName, etc.
            // Por ejemplo: client.UpdatePlayerInfo(Email, Name, LastName);

            // Una vez guardado, actualizamos la sesión local y navegamos de vuelta
            if (PlayerSession.IsLoggedIn)
            {
                PlayerSession.CurrentPlayer.name = this.Name;
                PlayerSession.CurrentPlayer.lastName = this.LastName;
                PlayerSession.CurrentPlayer.nickname = this.Nickname;
            }

            ProfileMainFrame.MainFrame.Navigate(new UserProfilePage());
        }
    }
}