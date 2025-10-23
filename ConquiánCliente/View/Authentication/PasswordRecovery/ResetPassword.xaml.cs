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
using ConquiánCliente.Properties.Langs;
using ConquiánCliente.ViewModel.Authentication.PasswordRecovery;

namespace ConquiánCliente.View.Authentication.PasswordRecovery
{
    /// <summary>
    /// Lógica de interacción para ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : Page
    {
        private PasswordRecoveryViewModel viewModel;

        public ResetPassword(PasswordRecoveryViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
        }

        private void ClickAcceptNewPassword(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.NewPassword = pbNewPassword.Password;
                viewModel.ConfirmPassword = pbConfirmPassword.Password;
            }
        }
    }
}
