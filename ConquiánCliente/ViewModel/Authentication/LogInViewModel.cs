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
        private bool isLoading; 
        private readonly IMessageResolver messageResolver;

        private const string SPANISH_LANGUAGE_CODE = "es-MX";
        private const string ENGLISH_LANGUAGE_CODE = "en-US";

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
            isLoading = false;

            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);

            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp, CanExecuteNavigation);
            NavigateToForgotPasswordCommand = new RelayCommand(ExecuteNavigateToForgotPassword, CanExecuteNavigation);
            NavigateToGuestLogInCommand = new RelayCommand(ExecuteNavigateToGuestLogIn, CanExecuteNavigation);
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !isLoggingIn;
        }

        private bool CanExecuteNavigation(object parameter)
        {
            return !isLoading && !isLoggingIn;
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

                MessageBox.Show(localMessage, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ExecuteNavigateToSignUp(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;
            CommandManager.InvalidateRequerySuggested(); 

            var signUpWindow = new SignUp();
            signUpWindow.Show();
            (parameter as Window)?.Close();


        }

        private void ExecuteNavigateToForgotPassword(object parameter)
        {
            if (isLoading) return;

            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            var requestRecoveryWindow = new PasswordRecoveryMainFrame();
            requestRecoveryWindow.Show();
            (parameter as Window)?.Close();
        }

        private void ExecuteNavigateToGuestLogIn(object parameter)
        {
            if (isLoading) return;

            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            var guestLogInWindow = new GuestLogIn();
            guestLogInWindow.Show();
            (parameter as Window)?.Close();
        }

        private void ChangeLanguage()
        {
            if (SelectedLanguageIndex == 1)
            {
                Properties.Settings.Default.languageCode = SPANISH_LANGUAGE_CODE;
            }
            else if (SelectedLanguageIndex == 2)
            {
                Properties.Settings.Default.languageCode = ENGLISH_LANGUAGE_CODE;
            }
            Properties.Settings.Default.Save();
        }
    }
}