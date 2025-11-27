using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication;
using ConquiánCliente.View.Authentication.PasswordRecovery;
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
        private bool isLoggingIn;
        private readonly IMessageResolver messageResolver;

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
        public ICommand NavigateToGuestLogInCommand { get; }

        public LogInViewModel(IMessageResolver messageResolver)
        {
            this.messageResolver = messageResolver;
            isLoggingIn = false;
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);

            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp);
            NavigateToForgotPasswordCommand = new RelayCommand(ExecuteNavigateToForgotPassword);
            NavigateToGuestLogInCommand = new RelayCommand(ExecuteNavigateToGuestLogIn);
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !isLoggingIn;
        }
        private async void ExecuteLogin(object parameter)
        {
            if (isLoggingIn)
            {
                return;
            }

            try
            {
                isLoggingIn = true;
                CommandManager.InvalidateRequerySuggested();

                var passwordBox = parameter as PasswordBox;
                if (passwordBox == null)
                {
                    return;
                }

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

                var client = new LoginClient();

                PlayerDto authenticatedPlayer = await client.AuthenticatePlayerAsync(Email, password);

                if (authenticatedPlayer.idPlayer > 0)
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
            catch (FaultException<ServiceFaultDto> fault)
            {
                ServiceErrorType errorType = fault.Detail.ErrorType;
                string localMessage = messageResolver.GetMessage(errorType);

                if (errorType == ServiceErrorType.SessionActive)
                {
                    MessageBox.Show(localMessage, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(localMessage, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Error);
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
            finally
            {
                isLoggingIn = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private static void ExecuteNavigateToSignUp(object parameter)
        {
            var signUpWindow = new SignUp();
            signUpWindow.Show();
            (parameter as Window)?.Close();
        }

        private static void ExecuteNavigateToForgotPassword(object parameter)
        {
            var requestRecoveryWindow = new PasswordRecoveryMainFrame();
            requestRecoveryWindow.Show();
            (parameter as Window)?.Close();
        }

        private static void ExecuteNavigateToGuestLogIn(object parameter)
        {
            var guestLogInWindow = new GuestLogIn();
            guestLogInWindow.Show();
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