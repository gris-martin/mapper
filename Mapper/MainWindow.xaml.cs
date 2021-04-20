using Mapper.Models;
using Mapper.Utils;
using Mapper.Windows;
using Microsoft.Win32;
using System;
using System.IO;
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
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to create a new map? Unsaved changes will be lost.",
                                         "New map",
                                         MessageBoxButton.OKCancel,
                                         MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
            {
                Map.Instance.Reset();
                Map.Instance.Markers.Clear();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var lastSavePath = Settings.FromFile().LastSavePath;
            if (string.IsNullOrEmpty(lastSavePath))
                SaveAs_Click(sender, e);
            else
                Map.Instance.SaveToFile(lastSavePath);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
                AddExtension = true
            };
            if (saveDialog.ShowDialog() == true)
            {
                Map.Instance.SaveToFile(saveDialog.FileName);
                var settings = Settings.FromFile();
                settings.LastSavePath = saveDialog.FileName;
                settings.Save();
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"
            };
            if (openDialog.ShowDialog() == true)
                Map.Instance.LoadFromFile(openDialog.FileName);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
