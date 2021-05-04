using Mapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mapper.ViewModels
{
    public class AddMarkerRelativeViewModel : MarkerDialogViewModel
    {
        public AddMarkerRelativeViewModel()
        {
            _marker.PropertyChanged += _marker_PropertyChanged;
        }

        private void _marker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WorldPos")
            {
                OnPropertyChanged("Distance");
                OnPropertyChanged("Angle");
            }
        }

        private MarkerViewModel relativeMarker = new MarkerViewModel(new Marker(Map.MarkerTypes[0]));
        public MarkerViewModel RelativeMarker
        {
            get => relativeMarker;
            set
            {
                if (relativeMarker != null)
                    relativeMarker.PropertyChanged -= _marker_PropertyChanged;
                relativeMarker = value;
                relativeMarker.PropertyChanged += _marker_PropertyChanged;
                OnPropertyChanged("RelativeMarker");
                OnPropertyChanged("Distance");
                OnPropertyChanged("Angle");
            }
        }

        public double Distance
        {
            get => (_marker.WorldPos - RelativeMarker.Model.WorldPos).Length();
            set
            {
                var newMarkerPos = _marker.WorldPos;
                var relMarkerPos = RelativeMarker.Model.WorldPos;
                var relMarkerPlanePos = relMarkerPos.ToVec2(); // Position of the relative marker in the x-y plane
                var markerPosDiff = newMarkerPos - relMarkerPos; // The vector between the relative marker position and the new marker position
                var planeLength = Math.Sqrt(value * value - markerPosDiff.Depth * markerPosDiff.Depth); // New length in the x-y plane (Pythagoras)

                var planeUnit = markerPosDiff.ToVec2().Unit();
                var newPlanePos = relMarkerPlanePos + planeUnit * planeLength;

                _marker.WorldPos = new Vec3(newPlanePos, _marker.WorldPos.Z);
                OnPropertyChanged("Distance");
            }
        }

        public double Angle
        {
            get => Models.Utils.GetCompassAngle(_marker.WorldPos, RelativeMarker.Model.WorldPos);
            set
            {
                _marker.WorldPos = Models.Utils.SetCompassAngle(_marker.WorldPos, RelativeMarker.Model.WorldPos, value, moveFirst: true);
            }
        }
    }
}
