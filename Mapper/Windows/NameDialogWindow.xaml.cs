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
using System.Windows.Shapes;

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
