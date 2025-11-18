using ConquiánCliente.ServiceGame;
using ConquiánCliente.ViewModel.Game.Behaviors;
using ConquiánCliente.ViewModel.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ConquiánCliente.Properties.Langs;

namespace ConquiánCliente.View.Game
{
    public partial class Game : Window
    {
        private Point? dragStartPoint = null;
        private Point? discardDragStartPoint = null;
        private DragAdorner dragAdorner;
        private AdornerLayer adornerLayer;
        private GameViewModel viewModel;
        private const string SELECTED_CARDS = "SelectedCards";
        private const string DISCARD_CARD = "DiscardCard";

        public Game(String roomCode)
        {
            InitializeComponent();
            this.Loaded += GameLoaded;
        }

        private void GameLoaded(object sender, RoutedEventArgs e)
        {
            viewModel = DataContext as GameViewModel;

            if (viewModel == null)
            {
                MessageBox.Show(Lang.ErrorViewModel);
                this.Close();
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
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

        private void PlayerCardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn)
            {
                return;
            }

            if (sender is Border cardBorder && cardBorder.DataContext is CardViewModel cardVM)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    cardVM.IsSelected = !cardVM.IsSelected;
                }
                else
                {
                    if (!cardVM.IsSelected)
                    {
                        ClearSelections();
                        cardVM.IsSelected = true;
                    }
                }

                dragStartPoint = e.GetPosition(RootGrid);
                e.Handled = true;
            }
        }

        private void PlayerCardMouseMove(object sender, MouseEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn || e.LeftButton != MouseButtonState.Pressed || !dragStartPoint.HasValue)
            {
                return;
            }

            Point currentPosition = e.GetPosition(RootGrid);
            Vector diff = dragStartPoint.Value - currentPosition;

            if (Math.Abs(diff.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(diff.Y) < SystemParameters.MinimumVerticalDragDistance)
            {
                return;
            }

            var selectedCards = viewModel.PlayerHand.Where(c => c.IsSelected).ToList();
            if (selectedCards.Count == 0 || selectedCards.Count == 2)
            {
                dragStartPoint = null;
                return;
            }

            adornerLayer = AdornerLayer.GetAdornerLayer(RootGrid);
            if (adornerLayer == null)
            {
                return;
            }

            DataObject dragData;
            FrameworkElement dragVisual;
            if (selectedCards.Count == 1)
            {
                var cardVM = selectedCards[0];

                dragData = new DataObject(typeof(CardViewModel), cardVM);

                DataTemplate template = this.FindResource("DragCardVisualTemplate") as DataTemplate;
                dragVisual = (FrameworkElement)template.LoadContent();
                dragVisual.DataContext = cardVM;
                dragVisual.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                dragVisual.Arrange(new Rect(dragVisual.DesiredSize));

                cardVM.IsBeingDragged = true;
            }
            else
            {
                dragData = new DataObject(SELECTED_CARDS, selectedCards);

                DataTemplate template = this.FindResource("DragMeldVisualTemplate") as DataTemplate;
                dragVisual = (FrameworkElement)template.LoadContent();
                dragVisual.DataContext = selectedCards.Take(4).ToList();
                dragVisual.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                dragVisual.Arrange(new Rect(dragVisual.DesiredSize));
                selectedCards.ForEach(c => c.IsBeingDragged = true);
            }

            dragAdorner = new DragAdorner(RootGrid, dragVisual, currentPosition);
            adornerLayer.Add(dragAdorner);

            try
            {
                DragDrop.DoDragDrop((DependencyObject)sender, dragData, DragDropEffects.Move);
            }
            finally
            {
                if (dragAdorner != null)
                {
                    adornerLayer.Remove(dragAdorner);
                    dragAdorner = null;
                }
                selectedCards.ForEach(c => c.IsBeingDragged = false);
                dragStartPoint = null;
            }
        }

        private void PlayerCardMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragStartPoint = null;
            discardDragStartPoint = null;
        }

        private void StockPileMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn)
            {
                return;
            }

            _ = viewModel.DrawFromDeckAsync();
            e.Handled = true;
        }

        private void StockPileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CardViewModel)))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private async void StockPileDrop(object sender, DragEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn)
            {
                return;
            }

            if (e.Data.GetDataPresent(typeof(CardViewModel)))
            {
                var cardVM = e.Data.GetData(typeof(CardViewModel)) as CardViewModel;
                if (cardVM != null)
                {
                    await viewModel.DiscardCardAsync(cardVM);
                }
            }
            ClearSelections();
            e.Handled = true;
        }

        private void DiscardCardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn || viewModel.TopDiscardCard == null)
            {
                return;
            }

            discardDragStartPoint = e.GetPosition(RootGrid);
            e.Handled = true;
        }

        private void DiscardCardMouseMove(object sender, MouseEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn || e.LeftButton != MouseButtonState.Pressed || !discardDragStartPoint.HasValue)
            {
                return;
            }

            Point currentPosition = e.GetPosition(RootGrid);
            Vector diff = discardDragStartPoint.Value - currentPosition;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var discardCard = viewModel.TopDiscardCard;
                if (discardCard == null)
                {
                    return;
                }

                DataTemplate template = this.FindResource("DragCardVisualTemplate") as DataTemplate;
                FrameworkElement image = (FrameworkElement)template.LoadContent();
                image.DataContext = discardCard;
                image.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                image.Arrange(new Rect(image.DesiredSize));

                adornerLayer = AdornerLayer.GetAdornerLayer(RootGrid);
                if (adornerLayer != null)
                {
                    dragAdorner = new DragAdorner(RootGrid, image, currentPosition);
                    adornerLayer.Add(dragAdorner);
                }

                DataObject dragData = new DataObject(DISCARD_CARD, discardCard);

                try
                {
                    DragDrop.DoDragDrop((DependencyObject)sender, dragData, DragDropEffects.Move);
                }
                finally
                {
                    if (dragAdorner != null)
                    {
                        adornerLayer.Remove(dragAdorner);
                        dragAdorner = null;
                    }
                }
                discardDragStartPoint = null;
            }
        }

        private void DropZoneDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(SELECTED_CARDS))
            {
                e.Effects = DragDropEffects.Move;
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(0x80, 0x90, 0xEE, 0x90));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void DropZoneDragLeave(object sender, DragEventArgs e)
        {
            (sender as Border).Background = new SolidColorBrush(Color.FromArgb(0x40, 0xFF, 0xFF, 0xFF));
            e.Handled = true;
        }

        private async void DropZoneDrop(object sender, DragEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn)
            {
                return;
            }

            if (e.Data.GetDataPresent(SELECTED_CARDS))
            {
                var selectedCards = e.Data.GetData(SELECTED_CARDS) as List<CardViewModel>;
                if (selectedCards != null && selectedCards.Any())
                {
                    var cardIds = selectedCards.Select(c => c.Id).ToList();
                    await viewModel.PlayCardsAsync(cardIds);
                }
            }

            (sender as Border).Background = new SolidColorBrush(Color.FromArgb(0x40, 0xFF, 0xFF, 0xFF));
            ClearSelections();
            e.Handled = true;
        }

        private void PlayerHandDragEnter(object sender, DragEventArgs e)
        {
            if (viewModel != null && viewModel.IsMyTurn && e.Data.GetDataPresent(DISCARD_CARD))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private async void PlayerHandDrop(object sender, DragEventArgs e)
        {
            if (viewModel == null || !viewModel.IsMyTurn)
            {
                e.Handled = true;
                return;
            }

            if (e.Data.GetDataPresent(DISCARD_CARD))
            {
                var discardCard = e.Data.GetData(DISCARD_CARD) as CardDto;
                if (discardCard == null) return;

                await viewModel.DrawFromDiscardAsync(discardCard);
            }
            e.Handled = true;
        }

        private void ClearSelections()
        {
            if (viewModel != null)
            {
                foreach (var card in viewModel.PlayerHand.Where(c => c.IsSelected))
                {
                    card.IsSelected = false;
                }
            }
        }

        private void RootGridDragOver(object sender, DragEventArgs e)
        {
            if (dragAdorner != null)
            {
                dragAdorner.UpdatePosition(e.GetPosition(RootGrid));
            }
        }

        private void MainGridGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(Cursors.None);
            e.Handled = true;
        }
    }
}