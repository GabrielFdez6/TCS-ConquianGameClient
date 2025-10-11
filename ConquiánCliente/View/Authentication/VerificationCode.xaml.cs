using ConquiánCliente.ServiceSignUp;
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
    /// Lógica de interacción para VerificationCode.xaml
    /// </summary>
    public partial class VerificationCode : Window
    {
        private string originalCode;
        private Player newPlayer;
        public VerificationCode(string code, Player newPlayer)
        {
            InitializeComponent();
            originalCode = code;
            this.newPlayer = newPlayer;
        }

        private void ClickAcceptCode(object sender, RoutedEventArgs e)
        {
            string enteredCode = tbVerificationCode.Text;

            if (enteredCode == originalCode)
            {
                SignUpData signUpData = new SignUpData(newPlayer);
                signUpData.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("El código de verificación es incorrecto.", "Error de verificación");
            }
        }

        private void ClickBackSignUp(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Close();
        }
    }
}
