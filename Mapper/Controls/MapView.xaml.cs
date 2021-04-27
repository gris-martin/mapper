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
        private MapViewModel ViewModel => this.DataContext as MapViewModel;

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
            var rulers = Map.Instance.Rulers;
            if (Map.Instance.Rulers.Count > 0)
            {
                var ruler = rulers[0];
                ruler.ViewEndPoint = lastRightClickPosition.ToVec2();
                Map.Instance.ClearRulers();
            }
            else
            {
                Map.Instance.AddRuler(lastRightClickPosition.ToVec2());
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
                ViewModel.SelectMarker(ViewModel.LastMarkerClicked);
                ViewModel.LastMarkerClicked.HideMarkerName();
            }
        }

        private void RemoveMarkerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete marker {ViewModel.LastMarkerClicked.Model.Name}?",
                                         "New map",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                MapViewModel.Model.Markers.Remove(ViewModel.LastMarkerClicked.Model);
            }
        }

        private void ChangeMarkerTypeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MarkerDialogWindow dialog = new(ViewModel.LastMarkerClicked.Model);
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
        #endregion

        #region MapGrid callbacks

        private void MapGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(MapGrid);
            this.ViewModel.MousePosition = currentPos;
            if (this.ViewModel.IsPanning)
            {
                MapViewModel.Model.UpdateOriginFromMouseMovement(
                    ViewModel.LastMousePosition.ToVec2(),
                    currentPos.ToVec2());
                ViewModel.LastMousePosition = currentPos;
            }

            var rulers = Map.Instance.Rulers;
            if (rulers.Count > 0)
            {
                rulers[0].ViewEndPoint = currentPos.ToVec2();
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
                ViewModel.IsPanning = false;
        }

        private void MapGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dataContext = source?.DataContext;
            if (dataContext != null && dataContext is MarkerViewModel marker)
            {
                MapViewModel.RightClickMenu.MarkerOptionsEnabled = true;
                ViewModel.LastMarkerClicked = marker;
            }
            else
            {
                MapViewModel.RightClickMenu.MarkerOptionsEnabled = false;
            }

            RightClickMenu.Instance.MeasureText = Map.Instance.Rulers.Count > 0 ? "Stop measuring" : "Measure";

            var pos = Mouse.GetPosition(MapGrid);
            lastRightClickPosition = new Point(pos.X, pos.Y);
        }

        private void MapGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(MapGrid);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (Map.Instance.Rulers.Count > 0)
                {
                    Map.Instance.ClearRulers();
                }
                else
                {
                    Map.Instance.AddRuler(mousePos.ToVec2());
                }
            }
            else
            {
                ViewModel.IsPanning = true;
                ViewModel.LastMousePosition = mousePos;
            }
        }

        private void MapGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.IsPanning = false;
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
        private static void ShowMarkerTip(FrameworkElement target)
        {
            (target.DataContext as MarkerViewModel).ShowMarkerName();
        }

        private static void HideMarkerTip(FrameworkElement target)
        {
            (target.DataContext as MarkerViewModel).HideMarkerName();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ViewModel.IsPanning)
                return;

            if (ViewModel.MarkerIsSelected)
                return;

            ShowMarkerTip(sender as FrameworkElement);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            HideMarkerTip(sender as FrameworkElement);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideMarkerTip(sender as FrameworkElement);
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
