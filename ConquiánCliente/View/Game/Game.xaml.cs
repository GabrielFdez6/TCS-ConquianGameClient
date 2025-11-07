using System.Windows;


namespace ConquiánCliente.View.Game
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public Game()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmExitGame confirmDialog = new ConfirmExitGame();

            confirmDialog.Owner = this;

            bool? result = confirmDialog.ShowDialog();

            if (result == true)
            {

                var mainMenu = new ConquiánCliente.View.MainMenu.MainMenu();
                mainMenu.Show();
                this.Close();
            }
        }
    }
}
