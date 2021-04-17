using Mapper.ViewModels;
using System.Windows;

namespace Mapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MapViewModel.Instance;
        }
    }
}
