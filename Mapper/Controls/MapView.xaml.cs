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
        private bool isMeasuring = false;
        private Point lastPanPosition = new();

        public MapView()
        {
            InitializeComponent();
        }


        #region AddMarker callbacks

        private void AddMarkerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddMarkerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MarkerDialogWindow markerDialog = new()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var status = markerDialog.ShowDialog();
            if (status != true)
                return;

            var nameDialog = new NameDialogWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            status = nameDialog.ShowDialog();
            if (status != true)
                return;

            var pos = lastRightClickPosition;
            var name = nameDialog.CreatedName;
            var type = markerDialog.MarkerType;
            MapViewModel.Map.Markers.Add(new MapMarker(pos.ToVec2(), name, type));
        }

        #endregion


        #region MapGrid callbacks

        private void MapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(MapGrid);
            if (this.isPanning)
            {
                MapViewModel.Map.UpdateOriginFromMouseMovement(
                    this.lastPanPosition.ToVec2(),
                    currentPos.ToVec2());
                this.lastPanPosition = currentPos;
            }

            if (this.isMeasuring)
            {
                MapViewModel.Ruler.ViewEndPoint = currentPos.ToVec2();
            }

            // Tooltip displaying world position
            if (!this.worldPositionTip.IsOpen)
                this.worldPositionTip.IsOpen = true;

            this.worldPositionTip.HorizontalOffset = currentPos.X;
            this.worldPositionTip.VerticalOffset = currentPos.Y + 20;
            var worldPos = MapViewModel.Map.ToWorldSpace(currentPos.ToVec2());
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
                MapViewModel.Ruler.IsHidden = true;
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var ruler = MapViewModel.Ruler;
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
            MapViewModel.Map.SetScaleAroundPoint(e.GetPosition(MapGrid).ToVec2(), e.Delta > 0);
        }

        private void MapGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                MapViewModel.Map.Reset();
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

            var markerData = target.DataContext as MapMarker;
            this.markerName.Text = markerData.Name;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isPanning)
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

    public static class CustomCommands
    {
        public static readonly RoutedUICommand AddMarker = new(
            "Add marker...",
            "AddMarker",
            typeof(CustomCommands)
        );
    }
}
