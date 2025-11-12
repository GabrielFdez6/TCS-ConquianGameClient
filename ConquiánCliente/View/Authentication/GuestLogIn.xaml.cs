using ConquiánCliente.ViewModel.Authentication;
using System.Windows;


namespace ConquiánCliente.View.Authentication
{
    public partial class GuestLogIn : Window
    {
        public GuestLogIn()
        {
            InitializeComponent();
            this.DataContext = new GuestLogInViewModel(this);
        }
    }
}
