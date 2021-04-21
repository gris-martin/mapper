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
                markerOptionsEnabled = value;
                OnPropertyChanged("MarkerOptionsEnabled");
            }
        }

        private string moveMarkerText = "Move marker";
        /// <summary>
        /// Text for Move marker menu item (should be either "Move marker" or "Place marker"
        /// </summary>
        public string MoveMarkerText
        {
            get => moveMarkerText;
            set
            {
                moveMarkerText = value;
                OnPropertyChanged("MoveMarkerText");
            }
        }

        private bool addMarkerEnabled = true;
        /// <summary>
        /// Should the "Add marker..." menu item be enabled.
        /// </summary>
        public bool AddMarkerEnabled
        {
            get => addMarkerEnabled;
            set
            {
                addMarkerEnabled = value;
                OnPropertyChanged("AddMarkerEnabled");
            }
        }

        private string measureText = "Measure";
        /// <summary>
        /// Text for the Measure menu item. Should be either "Measure" or "Stop measuring"
        /// </summary>
        public string MeasureText
        {
            get => measureText;
            set
            {
                measureText = value;
                OnPropertyChanged("MeasureText");
            }
        }
    }
}
