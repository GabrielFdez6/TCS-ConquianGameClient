using System.Windows;


namespace ConquiánCliente.View.Game
{
    public partial class ConfirmExitGame : Window
    {
        public ConfirmExitGame()
        {
            InitializeComponent();
        }
        private void SalirButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void VolverAlJuegoButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
