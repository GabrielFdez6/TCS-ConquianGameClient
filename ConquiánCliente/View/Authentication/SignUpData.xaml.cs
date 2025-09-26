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
    /// Lógica de interacción para SignUpData.xaml
    /// </summary>
    public partial class SignUpData : Window
    {
        public SignUpData()
        {
            InitializeComponent();
        }

        private void ClickAcceptNewAccount(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();
        }

        private void ClickBackSignUp(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Close();
        }
    }
}
