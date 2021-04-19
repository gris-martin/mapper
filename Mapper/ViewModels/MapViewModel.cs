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
        }

        private void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Markers");
        }

        public static Map Model => Map.Instance;
        public static RulerViewModel Ruler => new();

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
                mousePosition = value;
                OnPropertyChanged("MousePosition");
                OnPropertyChanged("FormattedWorldPosition");
                OnPropertyChanged("WorldPositionPopupHorizontalOffset");
                OnPropertyChanged("WorldPositionPopupVerticalOffset");
            }
        }

        private bool worldPositionPopupEnabled = true;
        /// <summary>
        /// Set to true if the popup showing world position at the mouse should be enabled,
        /// and false if it should be hidden.
        /// </summary>
        public bool WorldPositionPopupEnabled
        {
            get => worldPositionPopupEnabled;
            set
            {
                worldPositionPopupEnabled = value;
                OnPropertyChanged("WorldPositionPopupEnabled");
            }
        }

        /// <summary>
        /// Display the world position on the form (xxx, yyy)
        /// </summary>
        public string FormattedWorldPosition
        {
            get
            {
                var worldPos = Map.Instance.ToWorldSpace(MousePosition.ToVec2());
                return $"{Math.Round(worldPos.X)}, {Math.Round(worldPos.Y)}";
            }
        }

        /// <summary>
        /// Horizontal offset of the world position popup (relative to mouse)
        /// </summary>
        public double WorldPositionPopupHorizontalOffset => MousePosition.X;

        /// <summary>
        /// Vertical offset of the world position popup (relative to mouse)
        /// </summary>
        public double WorldPositionPopupVerticalOffset => MousePosition.Y + 20;
        #endregion

#pragma warning disable CA1822 // Mark members as static
        public ObservableCollection<MarkerViewModel> Markers =>
            new(Model.Markers.Select(marker => new MarkerViewModel(marker)));
#pragma warning restore CA1822
    }
}
