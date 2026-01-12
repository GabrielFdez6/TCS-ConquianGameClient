using ConquiánCliente.ViewModel;
using ConquiánCliente.ViewModel.Lobby;
using System.Collections.Specialized;
using System.Windows;

namespace ConquiánCliente.View.Lobby
{
    public partial class LobbyGame : Window
    {
        public LobbyGame(string roomCode)
        {
            InitializeComponent();
            DataContext = new LobbyGameViewModel(roomCode);
            this.Closing += LobbyGame_Closing;
            this.Loaded += LobbyGame_Loaded;
        }

        private void LobbyGame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (PlayerSession.IsNetworkDown)
            {
                return;
            }

            if (DataContext is LobbyGameViewModel vm)
            {
                if (vm.IsNavigatingAway) return;
                vm.ShutdownApplicationCommand.Execute(this);
                e.Cancel = true;
            }
        }

        private void LobbyGame_Loaded(object sender, RoutedEventArgs e)
        {
            if (ltBxChat.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += (s, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add && ltBxChat.Items.Count > 0)
                    {
                        var ultimoMensaje = ltBxChat.Items[ltBxChat.Items.Count - 1];
                        ltBxChat.ScrollIntoView(ultimoMensaje);
                    }
                };
            }
        }
    }
}