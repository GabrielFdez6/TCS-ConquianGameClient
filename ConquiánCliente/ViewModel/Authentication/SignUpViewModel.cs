using ConquiánCliente.ServiceSignUp;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication;
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
        private string verificationCodeFromServer;
        private readonly Player playerInProgress;

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
            playerInProgress = new Player();

            SendVerificationCodeCommand = new RelayCommand(ExecuteSendVerificationCode, CanExecuteSendVerificationCode);
            NavigateToLoginCommand = new RelayCommand(ExecuteNavigateToLogin);

            VerifyCodeCommand = new RelayCommand(ExecuteVerifyCode, CanExecuteVerifyCode);
            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp);

            RegisterPlayerCommand = new RelayCommand(ExecuteRegisterPlayer, CanExecuteRegisterPlayer);
        }

        private bool CanExecuteSendVerificationCode(object parameter) => !string.IsNullOrEmpty(Email);

        private void ExecuteSendVerificationCode(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;
            if (string.IsNullOrEmpty(password)) return;

            try
            {
                var client = new SignUpClient();
                verificationCodeFromServer = client.SendVerificationCode(Email);

                if (!string.IsNullOrEmpty(verificationCodeFromServer))
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
                    MessageBox.Show("No se pudo enviar el correo de verificación.", "Error");
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Servidor no disponible.", "Error de Conexión");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error");
            }
        }

        private bool CanExecuteVerifyCode(object parameter) => !string.IsNullOrEmpty(EnteredVerificationCode);

        private void ExecuteVerifyCode(object parameter)
        {
            if (EnteredVerificationCode == verificationCodeFromServer)
            {
                var signUpDataWindow = new SignUpData();
                signUpDataWindow.DataContext = this;
                signUpDataWindow.Show();
                (parameter as Window)?.Close();
            }
            else
            {
                MessageBox.Show("El código de verificación es incorrecto.", "Error");
            }
        }

        private bool CanExecuteRegisterPlayer(object parameter) => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Nickname);

        private void ExecuteRegisterPlayer(object parameter)
        {
            playerInProgress.name = Name;
            playerInProgress.lastName = LastName;
            playerInProgress.nickname = Nickname;
            playerInProgress.level = "1";
            playerInProgress.currentPoints = "0";

            try
            {
                var client = new SignUpClient();
                if (client.RegisterPlayer(playerInProgress))
                {
                    MessageBox.Show("¡Cuenta creada exitosamente! Por favor, inicie sesión.", "Registro Completo");
                    ExecuteNavigateToLogin(parameter);
                }
                else
                {
                    MessageBox.Show("No se pudo registrar. El correo o nickname ya podrían estar en uso.", "Error de Registro");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message, "Error de Conexión");
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
            var signUpWindow = new SignUp();
            signUpWindow.DataContext = this;
            signUpWindow.Show();
            (parameter as Window)?.Close();
        }
    }
}