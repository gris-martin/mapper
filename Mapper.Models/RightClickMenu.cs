using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper.Models
{
    public class RightClickMenu : PropertyChangedBase
    {
        private static readonly RightClickMenu instance = new RightClickMenu();
        public static RightClickMenu Instance => instance;

        private bool markerOptionsEnabled = false;
        /// <summary>
        /// True if a marker was right clicked
        /// </summary>
        public bool MarkerClicked
        {
            get => markerOptionsEnabled;
            set
            {
                if (SetProperty(ref markerOptionsEnabled, value))
                {
                    OnPropertyChanged("AllowMeasureFromMarker");
                    OnPropertyChanged("AllowSetDepthFromMarker");
                }
            }
        }

        private string moveMarkerText = "Move marker";
        /// <summary>
        /// Text for Move marker menu item (should be either "Move marker" or "Place marker")
        /// </summary>
        public string MoveMarkerText
        {
            get => moveMarkerText;
            set => SetProperty(ref moveMarkerText, value);
        }

        private bool addMarkerEnabled = true;
        /// <summary>
        /// Should the "Add marker..." menu item be enabled.
        /// </summary>
        public bool AddMarkerEnabled
        {
            get => addMarkerEnabled;
            set => SetProperty(ref addMarkerEnabled, value);
        }

        private bool isMeasuring = false;
        /// <summary>
        /// True if a ruler is currently measuring.
        /// </summary>
        public bool IsMeasuring
        {
            get => isMeasuring;
            set
            {
                if (SetProperty(ref isMeasuring, value))
                {
                    OnPropertyChanged("IsNotMeasuring");
                    OnPropertyChanged("AllowMeasureFromMarker");
                    OnPropertyChanged("AllowSetDepthFromMarker");
                }
            }
        }

        /// <summary>
        /// Inverse of <see cref="IsMeasuring"/>.
        /// </summary>
        public bool IsNotMeasuring => !IsMeasuring;

        /// <summary>
        /// True if it should be possible to start measuring from a marker.
        /// </summary>
        public bool AllowMeasureFromMarker => IsNotMeasuring && MarkerClicked;

        /// <summary>
        /// True if it should be possible to set the depth of a ruler from a currently clicked marker.
        /// </summary>
        public bool AllowSetDepthFromMarker => IsMeasuring && MarkerClicked;
    }
}
