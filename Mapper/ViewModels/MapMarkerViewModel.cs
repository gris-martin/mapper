using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mapper.ViewModels
{
    public class MapMarkerViewModel : INotifyPropertyChanged
    {
        private static int staticId = 0;

        public MapMarkerViewModel(DrawingImage image, Point pos, string name)
        {
            this.image = image;
            this.worldPos = MapViewModel.Instance.ToWorldSpace(pos);
            this.name = name;
            this.Id = staticId;
            staticId += 1;
        }

        private DrawingImage image;
        public DrawingImage Image {
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

        private Point screenPos;
        public Point ScreenPos { 
            get => screenPos;
            set {
                screenPos = value;
                NotifyPropertyChanged("ScreenPos");
            }
        }

        public int Id { get; }
            
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
