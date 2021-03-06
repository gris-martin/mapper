using Mapper.Models;
using Mapper.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var settings = Settings.FromFile();
            if (File.Exists(settings.LastSavePath)) {
                Map.Instance.LoadFromFile(settings.LastSavePath);
            }
        }
    }
}
