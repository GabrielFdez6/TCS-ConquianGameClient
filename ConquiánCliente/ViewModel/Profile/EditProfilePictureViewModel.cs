using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceUserProfile;
using ConquiánCliente.ViewModel;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Profile
{
    public class EditProfilePictureViewModel : ViewModelBase
    {
        private string _selectedImagePath;
        public string SelectedImagePath
        {
            get { return _selectedImagePath; }
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged(nameof(SelectedImagePath));
            }
        }

        public string CurrentProfilePicturePath { get; }

        public ICommand SelectImageCommand { get; }
        public ICommand ChangeProfilePictureCommand { get; }
        public ICommand CloseWindowCommand { get; }

        public EditProfilePictureViewModel()
        {
            CurrentProfilePicturePath = PlayerSession.CurrentPlayer.pathPhoto;
            SelectImageCommand = new RelayCommand(ExecuteSelectImage);

            ChangeProfilePictureCommand = new RelayCommand(ExecuteChangeProfilePicture, CanExecuteChangeProfilePicture);

            CloseWindowCommand = new RelayCommand(ExecuteCloseWindow);
        }
        private void ExecuteSelectImage(object parameter)
        {
            SelectedImagePath = parameter as string;
        }

        private bool CanExecuteChangeProfilePicture(object obj)
        {
            return !string.IsNullOrEmpty(SelectedImagePath);
        }

        private async void ExecuteChangeProfilePicture(object obj)
        {
            PlayerSession.UpdateProfilePicture(SelectedImagePath);

            try
            {

                var userProfileClient = new UserProfileClient();

                int playerId = PlayerSession.CurrentPlayer.idPlayer;

                bool success = await userProfileClient.UpdateProfilePictureAsync(playerId, SelectedImagePath);

                if (success)
                {
                    ExecuteCloseWindow(obj);
                }
                else
                {
                    MessageBox.Show(Lang.ErrorUpdatePhoto, Lang.TitleError);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        private void ExecuteCloseWindow(object parameter)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}