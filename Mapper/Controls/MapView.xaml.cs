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
        private Point lastRightClick = new Point();


        public MapView()
        {
            InitializeComponent();
            this.DataContext = MapViewModel.Instance;
        }


        private void AddMarkerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void AddMarkerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Something clicked!");
            MarkerDialog markerDialog = new MarkerDialog(lastRightClick);
            markerDialog.ShowDialog();
        }


        private void pnlMainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.mousePositionTip.IsOpen)
                this.mousePositionTip.IsOpen = true;

            Point currentPos = e.GetPosition(pnlMainGrid);

            this.mousePositionTip.HorizontalOffset = currentPos.X;
            this.mousePositionTip.VerticalOffset = currentPos.Y + 20;
            this.mousePosition.Text = $"{Math.Round(currentPos.X)}, {Math.Round(currentPos.Y)}";
        }

        private void pnlMainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.mousePositionTip.IsOpen = false;
        }


        private void pnlMainGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void pnlMainGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            lastRightClick = new Point(e.CursorLeft, e.CursorTop);
        }
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
