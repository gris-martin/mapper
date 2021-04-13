using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Mapper.Models
{
    class MapSymbolModel : INotifyPropertyChanged
    {
        private static int staticId = 0;

        public MapSymbolModel(BitmapImage image, Point pos, string name)
        {

            this.image = image;
            this.worldPos = new Point(pos.X - Math.Round(image.Width / 2.0),
                                      pos.Y - Math.Round(image.Height / 2.0));
            this.name = name;
            this.Id = staticId;
            this.name += staticId;
            staticId += 1;
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

        private Point worldPos;
        public Point WorldPos
        {
            get => worldPos;
            set
            {
                worldPos = value;
                NotifyPropertyChanged("WorldPos");
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }



        public int Id { get; }
            
        public Point ScreenPos { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
