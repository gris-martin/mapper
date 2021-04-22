﻿using Mapper.Models;
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
        private Point lastPanPosition = new();
        private MapViewModel ViewModel => this.DataContext as MapViewModel;
        private MarkerViewModel lastMarkerClicked;

        public MapView()
        {
            InitializeComponent();
        }

        #region Context menu callbacks
        private void AddMarkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MarkerDialogWindow dialog = new(lastRightClickPosition);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }


        private void MeasureMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var ruler = Ruler.Instance;
            if (ruler.IsMeasuring)
            {
                ruler.ViewEndPoint = lastRightClickPosition.ToVec2();
                ruler.IsMeasuring = false;
            }
            else
            {
                ruler.ViewStartPoint = ruler.ViewEndPoint = lastRightClickPosition.ToVec2();
                ruler.IsMeasuring = true;
            }
        }

        private void MoveMarkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.MarkerIsSelected)
            {
                ViewModel.MoveSelectedMarker(lastRightClickPosition);
                ViewModel.DeselectMarker();
            }
            else
            {
                ViewModel.SelectMarker(lastMarkerClicked);
                this.MarkerTip.IsOpen = false;
            }
        }

        private void RemoveMarkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete marker {lastMarkerClicked.Model.Name}?",
                                         "New map",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                MapViewModel.Model.Markers.Remove(lastMarkerClicked.Model);
            }
        }

        private void ChangeMarkerTypeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MarkerDialogWindow dialog = new(lastMarkerClicked.Model);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
        #endregion

        #region MapGrid callbacks

        private void MapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(MapGrid);
            this.ViewModel.MousePosition = currentPos;
            if (this.isPanning)
            {
                MapViewModel.Model.UpdateOriginFromMouseMovement(
                    this.lastPanPosition.ToVec2(),
                    currentPos.ToVec2());
                this.lastPanPosition = currentPos;
            }

            if (Ruler.Instance.IsMeasuring)
            {
                Ruler.Instance.ViewEndPoint = currentPos.ToVec2();
            }

            if (ViewModel.MarkerIsSelected)
                ViewModel.SelectedMarker.Model.ViewPos = currentPos.ToVec2();

            // Tooltip displaying world position
            if (!this.ViewModel.WorldPositionPopupEnabled)
                this.ViewModel.WorldPositionPopupEnabled = true;
        }

        private void MapGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ViewModel.WorldPositionPopupEnabled = false;
        }

        private void MapGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                this.isPanning = false;
        }

        private void MapGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dataContext = source?.DataContext;
            if (dataContext != null && dataContext is MarkerViewModel marker)
            {
                MapViewModel.RightClickMenu.MarkerOptionsEnabled = true;
                lastMarkerClicked = marker;
            }
            else
            {
                MapViewModel.RightClickMenu.MarkerOptionsEnabled = false;
            }

            RightClickMenu.Instance.MeasureText = Ruler.Instance.IsMeasuring ? "Stop measuring" : "Measure";

            var pos = Mouse.GetPosition(MapGrid);
            lastRightClickPosition = new Point(pos.X, pos.Y);
        }

        private void MapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(MapGrid);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (Ruler.Instance.IsMeasuring)
                {
                    Ruler.Instance.IsMeasuring = false;
                }
                else
                {
                    Ruler.Instance.ViewStartPoint = Ruler.Instance.ViewEndPoint = mousePos.ToVec2();
                    Ruler.Instance.IsMeasuring = true;
                }
            }
            else
            {
                isPanning = true;
                this.lastPanPosition = mousePos;
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
            this.MarkerTip.PlacementTarget = target;
            this.MarkerTip.IsOpen = true;

            this.MarkerTip.VerticalOffset = -25;

            var markerData = target.DataContext as MarkerViewModel;
            this.markerName.Text = markerData.Model.Name;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.isPanning)
                return;

            if (ViewModel.MarkerIsSelected)
                return;

            ShowMarkerTip(sender as FrameworkElement);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            this.MarkerTip.IsOpen = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.MarkerTip.IsOpen = false;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.MarkerIsSelected)
                return;

            ShowMarkerTip(sender as FrameworkElement);
        }
        #endregion
    }
}
