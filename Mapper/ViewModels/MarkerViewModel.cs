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
        public MarkerViewModel(MapMarker marker)
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

        private readonly MapMarker model;
        /// <summary>
        /// The underlying model for this view model.
        /// </summary>
        public MapMarker Model => model;

        /// <summary>
        /// Returns the view position of the upper left corner of this marker. Since
        /// Models.MapMarker only stores the center position we have to do a conversion.
        /// </summary>
        public Point CornerPosition
        {
            get
            {
                var centerPos = this.model.ViewPos;
                var markerSize = (double)Application.Current.FindResource("MarkerSize");
                return (centerPos - markerSize / 2).ToPoint();
            }
        }

        /// <summary>
        /// Get the source for the Image element. Models.MapMarker only stores strings, so
        /// we get the Application resource with that string as key.
        /// </summary>
        public ImageSource Source => Application.Current.FindResource(model.Type) as ImageSource;
    }
}
