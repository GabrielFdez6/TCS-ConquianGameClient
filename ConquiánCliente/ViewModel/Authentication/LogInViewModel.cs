using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;
using ConquiánCliente.ServiceSignUp;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.View.MainMenu;
using ConquiánCliente.ViewModel.Validation;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConquiánCliente.ViewModel.Authentication
{
    public class LogInViewModel : ViewModelBase
    {
        private string email;
        private int selectedLanguageIndex;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        public int SelectedLanguageIndex
        {
            get { return selectedLanguageIndex; }
            set
            {
                selectedLanguageIndex = value;
                OnPropertyChanged();
                ChangeLanguage();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }
        public ICommand NavigateToForgotPasswordCommand { get; }

        public LogInViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteCommand);
            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp);
            NavigateToForgotPasswordCommand = new RelayCommand(ExecuteNavigateToForgotPassword);
        }

        private bool CanExecuteCommand(object parameter)
        {
            return true;
        }

        private async void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            string password = passwordBox.Password;

            string emailError = LogInValidator.ValidateEmail(Email);
            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleValidation);
                return;
            }

            string passwordError = LogInValidator.ValidatePassword(password);
            if (!string.IsNullOrEmpty(passwordError))
            {
                MessageBox.Show(passwordError, Lang.TitleValidation);
                return;
            }

            try
            {
                var client = new LoginClient();

                ConquiánCliente.ServiceLogin.Player authenticatedPlayer = await client.AuthenticatePlayerAsync(Email, password);

                if (authenticatedPlayer != null)
                {
                    PlayerSession.StartSession(authenticatedPlayer);

                    var mainMenu = new View.MainMenu.MainMenu();
                    mainMenu.Show();
                    Window.GetWindow(passwordBox)?.Close();
                }
                else
                {
                    MessageBox.Show(Lang.ErrorInvalidCredentials, Lang.TitleAuthenticationError);
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

        private void ExecuteNavigateToSignUp(object parameter)
        {
            var signUpWindow = new SignUp();
            signUpWindow.Show();
            (parameter as Window)?.Close();
        }

        private void ExecuteNavigateToForgotPassword(object parameter)
        {
            var requestRecoveryWindow = new RequestRecovery();
            requestRecoveryWindow.Show();
            (parameter as Window)?.Close();
        }

        private void ChangeLanguage()
        {
            if (SelectedLanguageIndex == 1)
            {
                Properties.Settings.Default.languageCode = "es-MX";
            }
            else if (SelectedLanguageIndex == 2)
            {
                Properties.Settings.Default.languageCode = "en-US";
            }
            Properties.Settings.Default.Save();
        }
    }
}