using Mapper.Models;
using Mapper.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Windows
{
    /// <summary>
    /// Interaction logic for AddMarkerRelativeWindow.xaml
    /// </summary>
    public partial class AddMarkerRelativeWindow : Window
    {
        /// <summary>
        /// Constructor when creating a new marker
        /// </summary>
        /// <param name="position"></param>
        public AddMarkerRelativeWindow(Point position, MarkerViewModel relativeMarker)
        {
            InitializeComponent();
            var viewModel = (this.DataContext as AddMarkerRelativeViewModel);
            viewModel.Position = position;
            viewModel.RelativeMarker = relativeMarker;
        }

        /// <summary>
        /// Constructor when editing a marker
        /// </summary>
        /// <param name="marker"></param>
        public AddMarkerRelativeWindow(Marker marker)
        {
            InitializeComponent();
            (this.DataContext as AddMarkerRelativeViewModel).StartEdit(marker);
        }

        private void MarkerButton_Click(object sender, RoutedEventArgs e)
        {
            var b = e.Source as Button;
            var type = b.Tag as string;
            (this.DataContext as AddMarkerRelativeViewModel).MarkerClickedCommand(type);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var success = (this.DataContext as AddMarkerRelativeViewModel).OkClickedCommand();
            if (success)
            {
                this.DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MarkerName_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (!(e.NewFocus is TextBox))
                e.OldFocus.Focus();
        }
    }
}
