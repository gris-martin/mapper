using System.Windows;

namespace Mapper.Windows
{
    /// <summary>
    /// Interaction logic for NameDialogWindow.xaml
    /// </summary>
    public partial class NameDialogWindow : Window
    {
        public string CreatedName { get; private set; }


        public NameDialogWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MarkerName.Text))
            {
                CreatedName = MarkerName.Text;
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
