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

namespace ConquiánCliente.View.Authentication.PasswordRecovery
{
    /// <summary>
    /// Lógica de interacción para ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : Window
    {
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void ClickAcceptNewPassword(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();
        }

        private void ClickBackRequestRecovery(object sender, RoutedEventArgs e)
        {
            RequestRecovery requestRecovery = new RequestRecovery();
            requestRecovery.Show();
            this.Close();
        }
    }
}
