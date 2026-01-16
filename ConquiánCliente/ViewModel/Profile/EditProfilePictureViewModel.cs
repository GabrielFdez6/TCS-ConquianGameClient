using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceUserProfile;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using ConquiánCliente.Utilities.Messages; 

namespace ConquiánCliente.ViewModel.Profile
{
    public class EditProfilePictureViewModel : ViewModelBase
    {
        private string selectedImagePath;
        private readonly IMessageResolver messageResolver;

        public string SelectedImagePath
        {
            get { return selectedImagePath; }
            set
            {
                selectedImagePath = value;
                OnPropertyChanged(nameof(SelectedImagePath));
            }
        }

        public string CurrentProfilePicturePath { get; }

        public ICommand SelectImageCommand { get; }
        public ICommand ChangeProfilePictureCommand { get; }
        public ICommand CloseWindowCommand { get; }

        public EditProfilePictureViewModel()
        {
            this.messageResolver = new ResourceMessageResolver(); 
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
            try
            {
                var userProfileClient = new UserProfileClient();
                int playerId = PlayerSession.CurrentPlayer.idPlayer;

                await userProfileClient.UpdateProfilePictureAsync(playerId, SelectedImagePath);

                PlayerSession.UpdateProfilePicture(SelectedImagePath);
                ExecuteCloseWindow(obj);
            }
            catch (FaultException<ServiceFaultDto> fault)
            {
                var errorType = (ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);

                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Warning);
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