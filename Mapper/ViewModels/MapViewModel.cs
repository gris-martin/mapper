using Mapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Mapper.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public MapViewModel()
        {
            Model.Markers.CollectionChanged += Markers_CollectionChanged;
            Model.Rulers.CollectionChanged += Rulers_CollectionChanged;
        }

        public static Map Model => Map.Instance;
        public static RightClickMenu RightClickMenu => RightClickMenu.Instance;

        #region Map manipulation
        private bool isPanning = false;
        public bool IsPanning
        {
            get => isPanning;
            set => SetProperty(ref isPanning, value);
        }

        private Point lastMousePosition;
        public Point LastMousePosition
        {
            get => lastMousePosition;
            set => SetProperty(ref lastMousePosition, value);
        }

        private Point lastRightClickPosition;
        public Point LastRightClickPosition
        {
            get => lastRightClickPosition;
            set => SetProperty(ref lastRightClickPosition, value);
        }
        #endregion

        #region Marker manipulation
        private MarkerViewModel lastMarkerClicked;
        public MarkerViewModel LastMarkerClicked
        {
            get => lastMarkerClicked;
            set => SetProperty(ref lastMarkerClicked, value);
        }

        /// <summary>
        /// The last selected marker (or an arbitrary marker otherwise)
        /// </summary>
        public MarkerViewModel SelectedMarker { get; private set; }

        public bool markerIsSelected;
        /// <summary>
        /// Set to true if the selected marker should be visible.
        /// E.g. if you are moving a marker.
        /// </summary>
        public bool MarkerIsSelected
        {
            get => markerIsSelected;
            private set
            {
                markerIsSelected = value;
                OnPropertyChanged("SelectedMarkerVisible");
            }
        }

        /// <summary>
        /// Call this method to select a marker for moving
        /// </summary>
        /// <param name="marker"></param>
        public void SelectMarker(MarkerViewModel marker)
        {
            SelectedMarker = marker;
            MarkerIsSelected = true;
            RightClickMenu.MoveMarkerText = "Place marker";
        }

        /// <summary>
        /// Call this method to stop moving a marker and deselect it
        /// </summary>
        public void DeselectMarker()
        {
            MarkerIsSelected = false;
            SelectedMarker = null;
            RightClickMenu.MoveMarkerText = "Move marker";
        }

        /// <summary>
        /// Move the selected marker
        /// </summary>
        /// <param name="viewPos">The new position of the marker, in view space</param>
        public void MoveSelectedMarker(Point viewPos)
        {
            SelectedMarker.Model.SetViewSpacePosition(viewPos.ToVec2());
        }
        #endregion

        #region World position popup properties
        private Point mousePosition;
        /// <summary>
        /// Mouse position in view space.
        /// </summary>
        public Point MousePosition
        {
            get => mousePosition;
            set
            {
                if (SetProperty(ref mousePosition, value))
                {
                    OnPropertyChanged("FormattedWorldPosition");
                    OnPropertyChanged("WorldPositionPopupHorizontalOffset");
                    OnPropertyChanged("WorldPositionPopupVerticalOffset");
                }
            }
        }
        #endregion

        #region Collections and callbacks (rulers and markers)
        /// <summary>
        /// Update state of Markers when underlying model is updated
        /// </summary>
        private void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Markers");
        }

        /// <summary>
        /// Model.Marker collection -> MarkerViewModel collection
        /// </summary>
        public ObservableCollection<MarkerViewModel> Markers =>
            new(Model.Markers.Select(marker => new MarkerViewModel(marker)));

        /// <summary>
        /// Update the state of Rulers when underlying model is updated
        /// </summary>
        private void Rulers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Rulers");
        }

        /// <summary>
        /// Model.Ruler collection -> RulerViewModel collection
        /// </summary>
        public ObservableCollection<RulerViewModel> Rulers
            => new ObservableCollection<RulerViewModel>(Model.Rulers.Select(ruler => new RulerViewModel(ruler)));
        #endregion
    }
}
