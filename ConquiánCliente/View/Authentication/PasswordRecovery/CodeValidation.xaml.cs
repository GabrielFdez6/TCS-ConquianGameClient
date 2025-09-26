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
    /// Lógica de interacción para CodeValidation.xaml
    /// </summary>
    public partial class CodeValidation : Window
    {
        public CodeValidation()
        {
            InitializeComponent();
        }

        private void ClickAcceptCode(object sender, RoutedEventArgs e)
        {
            ResetPassword resetPassword = new ResetPassword();
            resetPassword.Show();
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
