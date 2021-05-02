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

        private static void Save()
        {
            var lastSavePath = Settings.FromFile().LastSavePath;
            if (string.IsNullOrEmpty(lastSavePath))
                SaveAs();
            else
                Map.Instance.SaveToFile(lastSavePath);
        }

        private static void SaveAs()
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
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
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

        private void Controls_Click(object sender, RoutedEventArgs e)
        {
            var controls = new ControlsWindow();
            controls.ShowDialog();
        }

        private void MapView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var mousePos = e.GetPosition(MapView);
            var worldPos = Map.Instance.ToWorldSpace(mousePos.ToVec2());
            var text = $"x: {Math.Round(worldPos.X)}; y: {Math.Round(worldPos.Y)}";

            if (Map.Instance.Rulers.Count > 0)
            {
                var ruler = Map.Instance.Rulers[0];
                var depthText = Math.Round(ruler.EndPoint.Depth).ToString();
                if (depthText == "-0")
                    depthText = "0";
                text += $"; depth: {depthText}";
            }

            PositionBlock.Text = text;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Map.Instance.IsDirty)
            {
                var result = MessageBox.Show("There are unsaved changes. Do you want to save before exiting?",
                                             "Save before exiting?",
                                             MessageBoxButton.YesNoCancel,
                                             MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Cancel)
                    e.Cancel = true;
                else if (result == MessageBoxResult.Yes)
                    Save();
            }
        }
    }
}
