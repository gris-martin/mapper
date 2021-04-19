using Mapper.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Windows
{
    /// <summary>
    /// Interaction logic for MarkerDialogWindow.xaml
    /// </summary>
    public partial class MarkerDialogWindow : Window
    {
        public MarkerDialogWindow(Point position)
        {
            InitializeComponent();
            (this.DataContext as MarkerDialogViewModel).Position = position;
        }

        private void MarkerButton_Click(object sender, RoutedEventArgs e)
        {
            var b = e.Source as Button;
            var type = b.Tag as string;
            (this.DataContext as MarkerDialogViewModel).MarkerClickedCommand(type);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var success = (this.DataContext as MarkerDialogViewModel).OkClickedCommand();
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
            MarkerName.Focus();
        }
    }
}
