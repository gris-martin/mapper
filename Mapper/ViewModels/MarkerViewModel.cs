using Mapper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Mapper.ViewModels
{
    public class MarkerViewModel : ViewModelBase
    {
        public MarkerViewModel(Marker marker)
        {
            this.model = marker;
            this.model.PropertyChanged += Marker_PropertyChanged;
        }

        private void Marker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ViewPos")
                OnPropertyChanged("CornerPosition");

            if (e.PropertyName == "Type")
                OnPropertyChanged("Source");
        }

        private readonly Marker model;
        /// <summary>
        /// The underlying model for this view model.
        /// </summary>
        public Marker Model => model;

        private Visibility markerNameEnabled = Visibility.Hidden;
        /// <summary>
        /// Should the name of the marker be visible on screen?
        /// </summary>
        public Visibility MarkerNameEnabled
        {
            get => markerNameEnabled;
            set => SetProperty(ref markerNameEnabled, value);
        }

        private Visibility markerDescriptionEnabled = Visibility.Hidden;
        /// <summary>
        /// Should the description of the marker be visible on screen?
        /// </summary>
        public Visibility MarkerDescriptionEnabled
        {
            get => markerDescriptionEnabled;
            set => SetProperty(ref markerDescriptionEnabled, value);
        }

        /// <summary>
        /// Show the marker name.
        /// </summary>
        public void ShowMarkerName()
            => MarkerNameEnabled = Visibility.Visible;

        /// <summary>
        /// Hide the marker name
        /// </summary>
        public void HideMarkerName()
            => MarkerNameEnabled = Visibility.Hidden;

        private Brush borderColor = new SolidColorBrush(Colors.Transparent);
        public Brush BorderColor
        {
            get => borderColor;
            set => SetProperty(ref borderColor, value);
        }

        /// <summary>
        /// Get the source for the Image element. Models.Marker only stores strings, so
        /// we get the Application resource with that string as key.
        /// </summary>
        public ImageSource Source => Application.Current.FindResource(model.Type) as ImageSource;
    }
}
