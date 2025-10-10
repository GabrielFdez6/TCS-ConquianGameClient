using ConquiánCliente.ConquianService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConquiánCliente.View
{
    /// <summary>
    /// Lógica de interacción para SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private string verificationCode;
        public SignUp()
        {
            InitializeComponent();
        }

        private void ClickLogIn(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();
        }

        private void ClickSignUp(object sender, RoutedEventArgs e)
        {
            string email = tbEmail.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Por favor, ingresa un correo electrónico.", "Campo vacío");
                return;
            }

            try
            {
                ConquianService.SignUpClient client = new ConquianService.SignUpClient();
                verificationCode = client.SendVerificationCode(email);

                if (!string.IsNullOrEmpty(verificationCode))
                {
                    string password = pbConfirmPassowrd.Password;
                    ConquianService.Player newPlayer = new ConquianService.Player();
                    newPlayer.email = email;
                    newPlayer.password = password;
                    VerificationCode verificationCodeWindow = new VerificationCode(verificationCode, newPlayer);
                    verificationCodeWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo enviar el correo de verificación. Inténtalo de nuevo.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message, "Error de conexión");
            }
        }
    }
}

