using ConquiánCliente.ViewModel.Authentication;
using System.Windows;

namespace ConquiánCliente.View
{
    public partial class SignUpData : Window
    {
        public SignUpData()
        {
            InitializeComponent();
            this.Closing += SignUpData_Closing;
        }

        private async void SignUpData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataContext is SignUpViewModel vm && !vm.IsRegistrationCompleted)
            {
                await vm.CancelRegistrationOnServerAsync();
            }
        }
    }
}
