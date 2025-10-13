using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ServiceSignUp;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication;
using ConquiánCliente.ViewModel.Validation;
using System.ServiceModel;
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
        private readonly PlayerDto playerInProgress;

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

            SendVerificationCodeCommand = new RelayCommand(ExecuteSendVerificationCode, CanExecuteSendVerificationCode);
            NavigateToLoginCommand = new RelayCommand(ExecuteNavigateToLogin);

            VerifyCodeCommand = new RelayCommand(ExecuteVerifyCode, CanExecuteVerifyCode);
            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp);

            RegisterPlayerCommand = new RelayCommand(ExecuteRegisterPlayer, CanExecuteRegisterPlayer);
        }

        private bool CanExecuteSendVerificationCode(object parameter) => true;
        private bool CanExecuteVerifyCode(object parameter) => true;
        private bool CanExecuteRegisterPlayer(object parameter) => true;

        private async void ExecuteSendVerificationCode(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;

            var window = Window.GetWindow(passwordBox);
            var confirmPasswordBox = window?.FindName("pbConfirmPassowrd") as PasswordBox;
            string confirmPassword = confirmPasswordBox?.Password;

            Email = Email.Trim();
            string emailError = SignUpValidator.ValidateEmail(Email);
            if (!string.IsNullOrEmpty(emailError))
            {
                MessageBox.Show(emailError, Lang.TitleValidation);
                return;
            }
            string passwordError = SignUpValidator.ValidatePassword(password);
            if (!string.IsNullOrEmpty(passwordError))
            {
                MessageBox.Show(passwordError, Lang.TitleValidation);
                return;
            }
            string confirmPasswordError = SignUpValidator.ValidateConfirmPassword(password, confirmPassword);
            if (!string.IsNullOrEmpty(confirmPasswordError))
            {
                MessageBox.Show(confirmPasswordError, Lang.TitleValidation);
                return;
            }

            try
            {
                var client = new SignUpClient();
                string serverResponse = await client.SendVerificationCodeAsync(Email);

                if (serverResponse == "ERROR_EMAIL_EXISTS")
                {
                    MessageBox.Show(Lang.ErrorVerificationEmail, Lang.TitleError);
                }
                else if (!string.IsNullOrEmpty(serverResponse))
                {
                    playerInProgress.email = Email;
                    playerInProgress.password = password;

                    var verificationWindow = new VerificationCode();
                    verificationWindow.DataContext = this;
                    verificationWindow.Show();
                    Window.GetWindow(passwordBox)?.Close();
                }
                else
                {
                    MessageBox.Show(Lang.ErrorVerificationEmail, Lang.TitleError);
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(Lang.ErrorServerUnavailable, Lang.TitleConnectionError);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorGeneric, ex.Message), Lang.TitleError);
            }
        }

        private async void ExecuteVerifyCode(object parameter)
        {
            if (string.IsNullOrEmpty(EnteredVerificationCode))
            {
                MessageBox.Show(string.Format(Lang.ErrorVerificationCodeEmpty));
                return;
            }

            try
            {
                var client = new SignUpClient();
                bool isCodeValid = await client.VerifyCodeAsync(playerInProgress.email, EnteredVerificationCode);

                if (isCodeValid)
                {
                    var signUpDataWindow = new SignUpData();
                    signUpDataWindow.DataContext = this;
                    signUpDataWindow.Show();
                    (parameter as Window)?.Close();
                }
                else
                {
                    MessageBox.Show(Lang.ErrorVerificationCodeIncorrect, Lang.TitleError);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorGeneric, ex.Message), Lang.TitleError);
            }
        }

        private async void ExecuteRegisterPlayer(object parameter)
        {
            Name = Name.Trim();
            string nameError = SignUpValidator.ValidateName(Name);
            if (!string.IsNullOrEmpty(nameError))
            {
                MessageBox.Show(nameError, Lang.TitleValidation);
                return;
            }
            LastName = LastName.Trim();
            string lastNameError = SignUpValidator.ValidateLastName(LastName);
            if (!string.IsNullOrEmpty(lastNameError))
            {
                MessageBox.Show(lastNameError, Lang.TitleValidation);
                return;
            }
            Nickname = Nickname.Trim();
            string nicknameError = SignUpValidator.ValidateNickname(Nickname);
            if (!string.IsNullOrEmpty(nicknameError))
            {
                MessageBox.Show(nicknameError, Lang.TitleValidation);
                return;
            }

            playerInProgress.name = Name;
            playerInProgress.lastName = LastName;
            playerInProgress.nickname = Nickname;
            playerInProgress.pathPhoto = "Resources/imageProfile/default.JPG";

            try
            {
                var client = new SignUpClient();
                if (await client.RegisterPlayerAsync(playerInProgress))
                {
                    MessageBox.Show(Lang.SuccessAccountCreated, Lang.TitleRegistrationComplete);
                    ExecuteNavigateToLogin(parameter);
                }
                else
                {
                    MessageBox.Show(Lang.ErrorRegistrationFailed, Lang.TitleRegistrationError);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format(Lang.ErrorConnectingToServer, ex.Message), Lang.TitleConnectionError);
            }
        }

        private void ExecuteNavigateToLogin(object parameter)
        {
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
    }
}