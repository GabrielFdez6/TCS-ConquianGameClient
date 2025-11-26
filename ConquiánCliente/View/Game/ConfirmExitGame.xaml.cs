using System.Windows;

namespace ConquiánCliente.View.Game
{
    public partial class ConfirmExitGame : Window
    {
        public ConfirmExitGame()
        {
            InitializeComponent();
        }
        private void SalirButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void VolverAlJuegoButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
