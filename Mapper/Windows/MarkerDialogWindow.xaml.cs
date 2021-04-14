using Mapper.Models;
using Mapper.ViewModels;
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
    /// Interaction logic for MarkerDialogWindow.xaml
    /// </summary>
    public partial class MarkerDialogWindow : Window
    {
        public BitmapImage Image { get; private set; }

        public MarkerDialogWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = e.Source as Button;
            var i = b.Content as Image;
            Image = i.Source as BitmapImage;
            DialogResult = true;
            Close();
        }
    }
}
