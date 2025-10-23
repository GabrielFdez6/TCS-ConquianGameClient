using ConquiánCliente.ViewModel.Authentication.PasswordRecovery;
using System.Windows;
using System.Windows.Controls;


namespace ConquiánCliente.View.Authentication.PasswordRecovery
{
    /// <summary>
    /// Lógica de interacción para RequestRecovery.xaml
    /// </summary>
    public partial class RequestRecovery : Page
    {
        public RequestRecovery()
        {
            InitializeComponent();
            this.DataContext = new PasswordRecoveryViewModel();
        }
    }
}
