using Mapper.Models;
using Mapper.ViewModels;
using Mapper.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mapper.Controls
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private Point lastRightClickPosition = new();
        private bool isPanning = false;
        private bool isMeasuring = false;
        private Point lastPanPosition = new();

        public MapView()
        {
            InitializeComponent();
        }

        #region Context menu callbacks
        private void AddMarkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MarkerDialogWindow dialog = new(lastRightClickPosition);
            dialog.ShowDialog();
        }
        #endregion

        #region MapGrid callbacks

        private void MapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(MapGrid);
            if (this.isPanning)
            {
                MapViewModel.Model.UpdateOriginFromMouseMovement(
                    this.lastPanPosition.ToVec2(),
                    currentPos.ToVec2());
                this.lastPanPosition = currentPos;
            }

            if (this.isMeasuring)
            {
                Ruler.Instance.ViewEndPoint = currentPos.ToVec2();
            }

            // Tooltip displaying world position
            if (!this.worldPositionTip.IsOpen)
                this.worldPositionTip.IsOpen = true;

            this.worldPositionTip.HorizontalOffset = currentPos.X;
            this.worldPositionTip.VerticalOffset = currentPos.Y + 20;
            var worldPos = MapViewModel.Model.ToWorldSpace(currentPos.ToVec2());
            this.worldPosition.Text = $"{Math.Round(worldPos.X)}, {Math.Round(worldPos.Y)}";
        }

        private void MapGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            //this.worldPositionTip.IsOpen = false;
        }

        private void MapGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                this.isPanning = false;
        }

        private void MapGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            lastRightClickPosition = new Point(e.CursorLeft, e.CursorTop);
        }

        private void MapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(MapGrid);
            Keyboard.Focus(MapGrid);
            if (isMeasuring)
            {
                isMeasuring = false;
                Models.Ruler.Instance.IsHidden = true;
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var ruler = Models.Ruler.Instance;
                ruler.ViewStartPoint = ruler.ViewEndPoint = mousePos.ToVec2();
                ruler.IsHidden = false;
                isMeasuring = true;
            }
            else
            {
                isPanning = true;
                this.lastPanPosition = e.GetPosition(MapGrid);
            }
        }

        private void MapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPanning = false;
        }

        private void MapGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MapViewModel.Model.SetScaleAroundPoint(e.GetPosition(MapGrid).ToVec2(), e.Delta > 0);
        }

        private void MapGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                MapViewModel.Model.Reset();
            }
        }
        #endregion

        #region Image callbacks

        private void ShowMarkerTip(FrameworkElement target)
        {
            this.markerTip.PlacementTarget = target;
            this.markerTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            this.markerTip.IsOpen = true;

            this.markerTip.VerticalOffset = -25;

            var markerData = target.DataContext as MarkerViewModel;
            this.markerName.Text = markerData.Model.Name;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.isPanning)
                return;

            ShowMarkerTip(sender as FrameworkElement);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            this.markerTip.IsOpen = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.markerTip.IsOpen = false;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowMarkerTip(sender as FrameworkElement);
        }
        #endregion
    }
}
