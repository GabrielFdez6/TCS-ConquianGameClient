using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceSignUp;
using ConquiánCliente.Utilities.Messages;
using ConquiánCliente.View;
using ConquiánCliente.ViewModel.Validation;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ConquiánCliente.ViewModel.Authentication
{
    public class SignUpViewModel : ViewModelBase
    {
        private string email;
        private string name;
        private string lastName;
        private string nickname;
        private string enteredVerificationCode;
        private bool isLoading;
        private readonly PlayerDto playerInProgress;
        private readonly IMessageResolver messageResolver;

        private const string DEFAULT_PATH_PHOTO = "/Resources/imageProfile/default.JPG";
        private const int MAX_VERIFICATION_CODE_LENGTH = 6;

        public bool IsVerificationSuccessful { get; set; } = false;

        public bool IsRegistrationCompleted { get; set; } = false;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(); }
        }
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; OnPropertyChanged(); }
        }
        public string EnteredVerificationCode
        {
            get { return enteredVerificationCode; }
            set { enteredVerificationCode = value; OnPropertyChanged(); }
        }

        public ICommand SendVerificationCodeCommand { get; }
        public ICommand VerifyCodeCommand { get; }
        public ICommand RegisterPlayerCommand { get; }
        public ICommand NavigateToLoginCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }

        public SignUpViewModel()
        {
            playerInProgress = new PlayerDto();
            this.messageResolver = new ResourceMessageResolver();
            isLoading = false;

            SendVerificationCodeCommand = new RelayCommand(ExecuteSendVerificationCode, CanExecuteSendVerificationCode);
            NavigateToLoginCommand = new RelayCommand(ExecuteNavigateToLogin, CanExecuteNavigation);

            VerifyCodeCommand = new RelayCommand(ExecuteVerifyCode, CanExecuteVerifyCode);
            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp, CanExecuteNavigation);

            RegisterPlayerCommand = new RelayCommand(ExecuteRegisterPlayer, CanExecuteRegisterPlayer);
        }

        private bool CanExecuteNavigation(object parameter)
        {
            return !isLoading;
        }

        private bool CanExecuteSendVerificationCode(object parameter)
        {
            return !isLoading;
        }

        private bool CanExecuteVerifyCode(object parameter)
        {
            return !isLoading;
        }

        private bool CanExecuteRegisterPlayer(object parameter)
        {
            return !isLoading;
        }

        private async void ExecuteSendVerificationCode(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            var passwords = GetPasswordsFromView(parameter);
            if (passwords.Item1 == null)
            {
                return;
            }

            if (!ValidateInitialData(passwords.Item1, passwords.Item2))
            {
                return;
            }

            await ProcessVerificationRequest(passwords.Item1, parameter);
        }

        private Tuple<string, string> GetPasswordsFromView(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null)
            {
                return new Tuple<string, string>(null, null);
            }

            string password = passwordBox.Password;
            var window = Window.GetWindow(passwordBox);
            var confirmPasswordBox = window?.FindName("pbConfirmPassowrd") as PasswordBox;
            string confirmPassword = confirmPasswordBox?.Password;

            return new Tuple<string, string>(password, confirmPassword);
        }

        private bool ValidateInitialData(string password, string confirmPassword)
        {
            bool isValid = false;
            string emailError = SignUpValidator.ValidateEmail(Email);

            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleValidation);
            }
            else
            {
                Email = Email.Trim();
                if (ValidatePasswords(password, confirmPassword))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private bool ValidatePasswords(string password, string confirmPassword)
        {
            bool areValid = false;
            string passwordError = SignUpValidator.ValidatePassword(password);

            if (!string.IsNullOrEmpty(passwordError))
            {
                MessageBox.Show(passwordError, Lang.TitleValidation);
            }
            else
            {
                string confirmError = SignUpValidator.ValidateConfirmPassword(password, confirmPassword);
                if (!string.IsNullOrEmpty(confirmError))
                {
                    MessageBox.Show(confirmError, Lang.TitleValidation);
                }
                else
                {
                    areValid = true;
                }
            }
            return areValid;
        }

        private async Task ProcessVerificationRequest(string password, object viewParameter)
        {
            SetLoading(true);

            try
            {
                var client = new SignUpClient();
                string verificationCode = await client.SendVerificationCodeAsync(Email);

                if (!string.IsNullOrEmpty(verificationCode))
                {
                    ProceedToVerification(password, viewParameter);
                }
                else
                {
                    MessageBox.Show(Lang.ErrorVerificationEmail, Lang.TitleError);
                }
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void ProceedToVerification(string password, object viewParameter)
        {
            playerInProgress.email = Email;
            playerInProgress.password = password;

            var verificationWindow = new VerificationCode();
            verificationWindow.DataContext = this;
            verificationWindow.Show();

            var passwordBox = viewParameter as PasswordBox;
            Window.GetWindow(passwordBox)?.Close();
        }

        private async void ExecuteVerifyCode(object parameter)
        {
            if (!ValidateVerificationCodeInput())
            {
                return;
            }

            SetLoading(true);

            try
            {
                var client = new SignUpClient();
                await client.VerifyCodeAsync(playerInProgress.email, EnteredVerificationCode);

                HandleVerificationSuccess(parameter);
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private bool ValidateVerificationCodeInput()
        {
            bool isValid = false;

            if (string.IsNullOrEmpty(EnteredVerificationCode))
            {
                MessageBox.Show(string.Format(Lang.ErrorVerificationCodeEmpty));
            }
            else if (EnteredVerificationCode.Length > MAX_VERIFICATION_CODE_LENGTH)
            {
                MessageBox.Show(string.Format(Lang.ErrorVerificationCode, MessageBoxImage.Information));
            }
            else
            {
                string formatError = SignUpValidator.ValidateCodeVerification(EnteredVerificationCode);
                if (!string.IsNullOrEmpty(formatError))
                {
                    MessageBox.Show(string.Format(Lang.ErrorVerificationCodeFormat));
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private void HandleVerificationSuccess(object parameter)
        {
            IsVerificationSuccessful = true;
            var signUpDataWindow = new SignUpData();
            signUpDataWindow.DataContext = this;
            signUpDataWindow.Show();
            (parameter as Window)?.Close();
        }

        private async void ExecuteRegisterPlayer(object parameter)
        {
            if (!ValidateRegistrationData())
            {
                return;
            }

            PopulatePlayerDto();
            SetLoading(true);

            try
            {
                var client = new SignUpClient();
                // El servidor ahora retorna Task (void), si no lanza excepción, es éxito.
                await client.RegisterPlayerAsync(playerInProgress);

                HandleRegistrationSuccess(parameter);
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private bool ValidateRegistrationData()
        {
            bool isValid = false;
            string nameError = SignUpValidator.ValidateName(Name);

            if (!string.IsNullOrEmpty(nameError))
            {
                MessageBox.Show(nameError, Lang.TitleValidation);
            }
            else if (!string.IsNullOrEmpty(SignUpValidator.ValidateLastName(LastName)))
            {
                MessageBox.Show(SignUpValidator.ValidateLastName(LastName), Lang.TitleValidation);
            }
            else if (!string.IsNullOrEmpty(SignUpValidator.ValidateNickname(Nickname)))
            {
                MessageBox.Show(SignUpValidator.ValidateNickname(Nickname), Lang.TitleValidation);
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }
        private void PopulatePlayerDto()
        {
            playerInProgress.name = Name.Trim();
            playerInProgress.lastName = LastName.Trim();
            playerInProgress.nickname = Nickname.Trim();
            playerInProgress.pathPhoto = DEFAULT_PATH_PHOTO;
            playerInProgress.Status = PlayerStatus.Offline;
        }

        private void HandleRegistrationSuccess(object parameter)
        {
            IsRegistrationCompleted = true;
            MessageBox.Show(Lang.SuccessAccountCreated, Lang.TitleRegistrationComplete);
            ExecuteNavigateToLogin(parameter);
        }

        public async Task CancelRegistrationOnServerAsync()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                return;
            }

            try
            {
                var client = new SignUpClient();
                await client.CancelRegistrationAsync(this.Email);
                client.Close();
            }
            catch (Exception ex)
            {
                HandleServiceException(ex);
            }
        }

        private void ExecuteNavigateToLogin(object parameter)
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;
            var loginWindow = new LogIn();
            loginWindow.Show();
            (parameter as Window)?.Close();
        }

        private void ExecuteNavigateToSignUp(object parameter)
        {
            EnteredVerificationCode = string.Empty;
            Name = string.Empty;
            LastName = string.Empty;
            Nickname = string.Empty;

            var signUpWindow = new SignUp();
            signUpWindow.DataContext = this;
            signUpWindow.Show();
            (parameter as Window)?.Close();
        }

        private void SetLoading(bool loading)
        {
            isLoading = loading;
            CommandManager.InvalidateRequerySuggested();
        }

        private void HandleServiceException(Exception ex)
        {
            if (ex is FaultException<ServiceFaultDto> fault)
            {
                var errorType = (ConquiánCliente.ServiceLogin.ServiceErrorType)(int)fault.Detail.ErrorType;
                string msg = messageResolver.GetMessage(errorType);
                MessageBox.Show(msg, Lang.TitleError, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ex is EndpointNotFoundException || ex is CommunicationException || ex is TimeoutException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            else
            {
                MessageBox.Show(string.Format(Lang.ErrorConnectingToServer), Lang.TitleConnectionError);
            }
        }
    }
}