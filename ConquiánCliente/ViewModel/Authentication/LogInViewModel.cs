using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceLogin;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
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

        private const int LANGUAGE_INDEX_SPANISH = 1;
        private const int LANGUAGE_INDEX_ENGLISH = 2;
        private const int INVALID_PLAYER_ID = 0;

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

            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null)
            {
                return;
            }

            if (!ValidateCredentials(passwordBox.Password))
            {
                return;
            }

            isLoggingIn = true;
            CommandManager.InvalidateRequerySuggested();

            try
            {
                await PerformLogin(passwordBox.Password, passwordBox);
            }
            catch (Exception ex)
            {
                HandleLoginException(ex);
            }
            finally
            {
                isLoggingIn = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool ValidateCredentials(string password)
        {
            bool isValid = false;
            string emailError = LogInValidator.ValidateEmail(Email);

            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleValidation);
            }
            else
            {
                string passwordError = LogInValidator.ValidatePassword(password);
                if (!string.IsNullOrEmpty(passwordError))
                {
                    MessageBox.Show(passwordError, Lang.TitleValidation);
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private async Task PerformLogin(string password, PasswordBox passwordBox)
        {
            var client = new LoginClient();
            PlayerDto authenticatedPlayer = await client.AuthenticatePlayerAsync(Email, password);

            if (authenticatedPlayer.idPlayer > INVALID_PLAYER_ID)
            {
                HandleLoginSuccess(authenticatedPlayer, passwordBox);
            }
            else
            {
                MessageBox.Show(Lang.ErrorInvalidCredentials, Lang.TitleAuthenticationError);
            }
        }

        private void HandleLoginSuccess(PlayerDto player, PasswordBox passwordBox)
        {
            PlayerSession.StartSession(player);
            var mainMenu = new View.MainMenu.MainMenu();
            mainMenu.Show();

            if (passwordBox != null)
            {
                Window.GetWindow(passwordBox)?.Close();
            }
        }

        private void HandleLoginException(Exception ex)
        {
            if (ex is FaultException<ServiceFaultDto> fault)
            {
                ServiceErrorType errorType = fault.Detail.ErrorType;
                string localMessage = messageResolver.GetMessage(errorType);
                MessageBox.Show(localMessage, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ex is EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorUnexpected, ex.Message), Lang.TitleError);
            }
        }

        private void ExecuteNavigateToSignUp(object parameter)
        {
            var signUpWindow = new SignUp();
            NavigateAndClose(signUpWindow, parameter);
        }
        private void ExecuteNavigateToForgotPassword(object parameter)
        {
            var requestRecoveryWindow = new PasswordRecoveryMainFrame();
            NavigateAndClose(requestRecoveryWindow, parameter);
        }

        private void ExecuteNavigateToGuestLogIn(object parameter)
        {
            var guestLogInWindow = new GuestLogIn();
            NavigateAndClose(guestLogInWindow, parameter);
        }

        private void NavigateAndClose(Window newWindow, object parameter)
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;
            CommandManager.InvalidateRequerySuggested();

            newWindow.Show();
            (parameter as Window)?.Close();
        }

        private void ChangeLanguage()
        {
            switch (SelectedLanguageIndex)
            {
                case LANGUAGE_INDEX_SPANISH:
                    Properties.Settings.Default.languageCode = SPANISH_LANGUAGE_CODE;
                    break;
                case LANGUAGE_INDEX_ENGLISH:
                    Properties.Settings.Default.languageCode = ENGLISH_LANGUAGE_CODE;
                    break;
                default:
                    return;
            }

            Properties.Settings.Default.Save();
        }
    }
}