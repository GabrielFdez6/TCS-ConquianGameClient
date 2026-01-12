using ConquiánCliente.ViewModel.Authentication;
using System.Windows;

namespace ConquiánCliente.View
{
    public partial class VerificationCode : Window
    {
        public VerificationCode()
        {
            InitializeComponent();
            this.Closing += VerificationCodeClosing;
        }

        private async void VerificationCodeClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataContext is SignUpViewModel vm && !vm.IsVerificationSuccessful)
            {
                await vm.CancelRegistrationOnServerAsync();
            }
        }
    }
}
