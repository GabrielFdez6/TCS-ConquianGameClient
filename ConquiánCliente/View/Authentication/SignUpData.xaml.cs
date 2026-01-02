using ConquiánCliente.ServiceSignUp;
using ConquiánCliente.ViewModel.Authentication;
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
using System.Xml.Linq;

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
            this.Closing += SignUpData_Closing;
        }

        private async void SignUpData_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataContext is SignUpViewModel vm)
            {
                if (!vm.IsRegistrationCompleted)
                {
                    await vm.CancelRegistrationOnServerAsync();
                }
            }
        }
    }
}
