using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mapper.Controls
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private Point lastRightClickPosition = new Point();
        private bool isPanning = false;
        private Point lastPanPosition = new Point();

        public MapView()
        {
            InitializeComponent();
            this.DataContext = MapViewModel.Instance;
        }


        #region AddMarker callbacks

        private void AddMarkerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void AddMarkerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Something clicked!");
            MarkerDialog markerDialog = new MarkerDialog(lastRightClickPosition);
            markerDialog.ShowDialog();
        }

        #endregion


        #region MapGrid callbacks

        private void MapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(MapGrid);
            if (this.isPanning)
            {
                double deltaX = this.lastPanPosition.X - currentPos.X;
                double deltaY = this.lastPanPosition.Y - currentPos.Y;
                Point origin = MapViewModel.Instance.Origin;
                double newX = origin.X + deltaX;
                double newY = origin.Y + deltaY;

                MapViewModel.Instance.Origin = new Point(newX, newY);

                this.lastPanPosition = currentPos;
            }

            // Tooltip displaying world position
            if (!this.worldPositionTip.IsOpen)
                this.worldPositionTip.IsOpen = true;

            this.worldPositionTip.HorizontalOffset = currentPos.X;
            this.worldPositionTip.VerticalOffset = currentPos.Y + 20;
            this.worldPosition.Text = $"{Math.Round(currentPos.X)}, {Math.Round(currentPos.Y)}";
        }

        private void MapGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.worldPositionTip.IsOpen = false;
        }

        private void MapGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            lastRightClickPosition = new Point(e.CursorLeft, e.CursorTop);
        }

        private void MapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isPanning = true;
            this.lastPanPosition = e.GetPosition(MapGrid);
        }

        private void MapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPanning = false;
        }
        #endregion


        #region Image callbacks

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            this.markerTip.PlacementTarget = image;
            this.markerTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            this.markerTip.IsOpen = true;

            this.markerTip.VerticalOffset = -25;

            var markerData = image.DataContext as MapSymbolModel;
            this.markerName.Text = markerData.Name;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            this.markerTip.IsOpen = false;
        }

        #endregion
    }

        public static class CustomCommands
    {
        public static readonly RoutedUICommand AddMarker = new RoutedUICommand(
            "Add marker...",
            "AddMarker",
            typeof(CustomCommands)
        );
    }

}
