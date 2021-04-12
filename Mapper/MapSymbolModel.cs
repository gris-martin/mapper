using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Mapper
{
    class MapSymbolModel : INotifyPropertyChanged
    {
        public MapSymbolModel(BitmapImage image, Point pos)
        {
            this.image = image;
            this.pos = new Point(pos.X - Math.Round(image.Width / 2.0), pos.Y - Math.Round(image.Height / 2.0));
        }

        private BitmapImage image;
        public BitmapImage Image {
            get => image;
            set
            {
                image = value;
                NotifyPropertyChanged("Y");
            }
        }

        private Point pos;
        public Point Pos
        {
            get => pos;
            set
            {
                pos = value;
                NotifyPropertyChanged("Pos");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
