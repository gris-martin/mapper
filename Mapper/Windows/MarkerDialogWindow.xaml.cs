using Mapper.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Mapper.Windows
{
    /// <summary>
    /// Interaction logic for MarkerDialogWindow.xaml
    /// </summary>
    public partial class MarkerDialogWindow : Window
    {
        public string MarkerType { get; private set; }

        public MarkerDialogWindow()
        {
            InitializeComponent();
            this.DataContext = new MarkerDialogViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = e.Source as Button;
            MarkerType = b.Tag as string;
            DialogResult = true;
            Close();
        }
    }
}
