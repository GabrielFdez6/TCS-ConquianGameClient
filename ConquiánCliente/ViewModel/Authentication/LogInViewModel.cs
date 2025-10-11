using ConquiánCliente.ServiceLogin;
using ConquiánCliente.View;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.View.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            NavigateToSignUpCommand = new RelayCommand(ExecuteNavigateToSignUp);
            NavigateToForgotPasswordCommand = new RelayCommand(ExecuteNavigateToForgotPassword);
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Email);
        }

        private void ExecuteLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            string password = passwordBox.Password;

            try
            {
                var client = new LoginClient();
                if (client.SignIn(Email, password))
                {
                    var mainMenu = new MainMenu();
                    mainMenu.Show();
                    Window.GetWindow(passwordBox)?.Close();
                }
                else
                {
                    MessageBox.Show("Credenciales inválidas.", "Error de Autenticación");
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Servidor no disponible.", "Error de Conexión");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error");
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
