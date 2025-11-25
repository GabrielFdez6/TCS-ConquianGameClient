using ConquiánCliente.ViewModel.Game;
using System.Windows;


namespace ConquiánCliente.View.Game
{
    /// <summary>
    /// Lógica de interacción para GameResults.xaml
    /// </summary>
    public partial class GameResults : Window
    {
        public GameResults()
        {
            InitializeComponent();
            DataContext = new GameResultsViewModel();
        }
    }
}
