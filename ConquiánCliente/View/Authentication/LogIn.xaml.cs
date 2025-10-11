using ConquiánCliente.View;
using ConquiánCliente.View.Authentication.PasswordRecovery;
using ConquiánCliente.View.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConquiánCliente
{
    /// <summary>
    /// Lógica de interacción para LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void CbxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxLanguageChanged.SelectedIndex == 1)
            {
                Properties.Settings.Default.languageCode = "es-MX";
            }
            else if (cbxLanguageChanged.SelectedIndex == 2)
            {
                Properties.Settings.Default.languageCode = "en-US";
            }
            Properties.Settings.Default.Save();
        }

        private void ClickSignUp (object sender, RoutedEventArgs e)
        {
            SignUp signUpWindow = new SignUp();
            signUpWindow.Show();
            this.Close();
        }

        private void ClickForgotPassword(object sender, RoutedEventArgs e)
        {
            RequestRecovery requestRecovery = new RequestRecovery();
            requestRecovery.Show();
            this.Close();
        }

        private void ClickLogIn(object sender, RoutedEventArgs e)
        {
            string email = txtBoxEmail.Text;
            string password = pbPassword.Password;

            try
            {
                ServiceLogin.LoginClient client = new ServiceLogin.LoginClient();
                if (client.SignIn(email, password))
                {
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Credenciales invalidas");
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Servidor no disponible");
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }
    }
}
