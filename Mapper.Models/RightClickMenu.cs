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
        /// Should the marker options be enabled?
        /// </summary>
        public bool MarkerOptionsEnabled
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
        /// Text for Move marker menu item (should be either "Move marker" or "Place marker"
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
        /// True if the "Start measuring from marker" option should be available.
        /// </summary>
        public bool AllowMeasureFromMarker => IsNotMeasuring && MarkerOptionsEnabled;

        /// <summary>
        /// True if the "Set depth from marker" option should be available.
        /// </summary>
        public bool AllowSetDepthFromMarker => IsMeasuring && MarkerOptionsEnabled;
    }
}
