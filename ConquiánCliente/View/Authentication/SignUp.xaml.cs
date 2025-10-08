using ConquiánCliente.ConquianService;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Service1Client client = new Service1Client();

            ConquianService.Player testPlayer = new ConquianService.Player
            {
                name = "Jugador",
                lastName = "DePrueba",
                nickname = "JugadorDePrueba",
                email = "test@email.com",
                password = "password123",
                level = "1",
                currentPoints = "0",
                idPlayer = 8
            };

            try
            {
                bool result = client.RegisterPlayer(testPlayer);

                if (result)
                {
                    MessageBox.Show("¡Jugador de prueba registrado con éxito!");
                }
                else
                {
                    MessageBox.Show("Error: No se pudo registrar al jugador.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error al conectar con el servidor: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
