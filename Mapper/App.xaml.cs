using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        }


        //private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        //{
        //    MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    e.Handled = true;
        //}


        // From https://stackoverflow.com/questions/25210122/displaying-a-grid-of-thumbnails-with-c-wpf
        private static BitmapImage CreateBitmap(string path)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path);
            bi.EndInit();
            return bi;
        }

        private static BitmapImage ScaleImage(BitmapImage original, double scale)
        {
            var scaledBitmapSource = new TransformedBitmap();
            scaledBitmapSource.BeginInit();
            scaledBitmapSource.Source = original;
            scaledBitmapSource.Transform = new ScaleTransform(scale, scale);
            scaledBitmapSource.EndInit();
            return BitmapSourceToBitmap(scaledBitmapSource);
        }

        private static BitmapImage BitmapSourceToBitmap(BitmapSource source)
        {
            var encoder = new PngBitmapEncoder();
            var memoryStream = new MemoryStream();
            var image = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memoryStream);

            image.BeginInit();
            image.StreamSource = new MemoryStream(memoryStream.ToArray());
            image.EndInit();
            memoryStream.Close();
            return image;
        }
    }
}
