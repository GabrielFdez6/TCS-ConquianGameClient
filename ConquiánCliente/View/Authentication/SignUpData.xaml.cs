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
    /// Lógica de interacción para SignUpData.xaml
    /// </summary>
    public partial class SignUpData : Window
    {
        private Player newPlayer;
        public SignUpData(Player newPlayer)
        {
            InitializeComponent();
            this.newPlayer = newPlayer;
        }

        private void ClickAcceptNewAccount(object sender, RoutedEventArgs e)
        {
            try
            {

                newPlayer.level = "1";
                newPlayer.currentPoints = "0";
                newPlayer.idPlayer = 56;

                var client = new ServiceSignUp.SignUpClient();
                bool registered = client.RegisterPlayer(newPlayer);

                if (registered)
                {
                    MessageBox.Show("¡Cuenta creada exitosamente!", "Registro completo");
                    LogIn logIn = new LogIn();
                    logIn.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la cuenta. Inténtalo de nuevo.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message, "Error de conexión");
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
