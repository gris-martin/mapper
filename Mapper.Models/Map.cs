﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Mapper.Models
{
    public class Map : INotifyPropertyChanged
    {
        private static readonly Map instance = new();
        public static Map Instance => instance;

        private Map()
        {
            this.Markers.CollectionChanged += Markers_CollectionChanged;
        }

        #region Callbacks
        /// <summary>
        /// Add a callback for all newly created markers to make sure their view position is updated
        /// when the world position is updated.
        /// </summary>
        private void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (MapMarker removedSymbol in e.OldItems)
                {
                    removedSymbol.PropertyChanged -= NewSymbol_PropertyChanged;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (MapMarker newSymbol in e.NewItems)
                {
                    newSymbol.ViewPos = ToViewSpace(newSymbol.WorldPos);
                    newSymbol.PropertyChanged += NewSymbol_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// Update the view position of markers when their world position is updated.
        /// </summary>
        private void NewSymbol_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WorldPos")
            {
                var symbol = sender as MapMarker;
                symbol.ViewPos = ToViewSpace(symbol.WorldPos);
            }
        }
        #endregion

        /// <summary>
        /// List of all the map symbols assigned to this view model
        /// </summary>
        public ObservableCollection<MapMarker> Markers { get; } = new ObservableCollection<MapMarker>();

        private Vec2 origin = new(0, 0);
        /// <summary>
        /// Current world coordinate of upper left corner
        /// </summary>
        public Vec2 Origin
        {
            get => origin;
            set
            {
                origin = value;
                foreach (var symbol in Markers)
                    symbol.ViewPos = ToViewSpace(symbol.WorldPos);
                OnPropertyChanged("Origin");
                OnPropertyChanged("North");
                Ruler.Instance.OnPropertyChanged("ViewStartPoint");
                Ruler.Instance.OnPropertyChanged("ViewEndPoint");
            }
        }

        private double scale = 1;
        /// <summary>
        /// Scale in meters per pixel
        /// </summary>
        public double Scale
        {
            get => scale;
            set
            {
                foreach (var symbol in Markers)
                    symbol.ViewPos = ToViewSpace(symbol.WorldPos);
                scale = value;
                OnPropertyChanged("Scale");
                OnPropertyChanged("North");
                Ruler.Instance.OnPropertyChanged("ViewStartPoint");
                Ruler.Instance.OnPropertyChanged("ViewEndPoint");
            }
        }

        /// <summary>
        /// A unit vector in view space which corresponds to north in world space
        /// </summary>
        public static Vec2 North
        {
            get
            {
                return new Vec2(0, -1);
            }
        }

        /// <summary>
        /// Reset the Origin and Scale. I.e. Scale == 1 and Origin == (0, 0).
        /// </summary>
        public void Reset()
        {
            Scale = 1;
            Origin = new Vec2(0, 0);
        }

        /// <summary>
        /// Update the Origin from a mouse movement.
        /// </summary>
        /// <param name="lastPosition">The previous position of the mouse.</param>
        /// <param name="currentPosition">The current position of the mouse.</param>
        public void UpdateOriginFromMouseMovement(Vec2 lastPosition, Vec2 currentPosition)
        {
            Vec2 scaledDelta = (lastPosition - currentPosition) * Scale;
            scaledDelta.Y = -scaledDelta.Y;
            Origin += scaledDelta;
        }

        /// <summary>
        /// Set the scale of the map given a view center.
        /// </summary>
        /// <param name="center">The point in view coordinates that the scaling should
        /// be done around. This point will have the same coordinates (view and world)
        /// before and after the scaling.</param>
        /// <param name="zoomIn">Set to true if the scaling is a zoom in operation
        /// (Scale will decrease), and false if the scaling is a zoom out operation
        /// (Scale will increase).</param>
        public void SetScaleAroundPoint(Vec2 center, bool zoomIn)
        {
            var zoomConstant = 1.3;
            var mousePos = ToWorldSpace(center);

            var scaleAmount = zoomConstant;
            if (zoomIn)
                scaleAmount = 1.0 / scaleAmount;

            Scale *= scaleAmount;

            // Zoom in
            if (zoomIn)
            {
                var newOrigin = mousePos + (Origin - mousePos) * scaleAmount;
                Origin = newOrigin;
            }
            // Zoom out
            else
            {
                var newOrigin = Origin - (mousePos - Origin) * (scaleAmount - 1);
                Origin = newOrigin;
            }
        }

        /// <summary>
        /// Convert a point in view space to world space
        /// </summary>
        /// <param name="viewPoint">A point in view space</param>
        /// <returns>The same point in world space</returns>
        public Vec2 ToWorldSpace(Vec2 viewPoint)
        {
            var scaledViewPoint = viewPoint * Scale;
            scaledViewPoint.Y = -scaledViewPoint.Y;
            return Origin + scaledViewPoint;
        }

        /// <summary>
        /// Convert a point in world space to view space
        /// </summary>
        /// <param name="worldPoint">A point in world space</param>
        /// <returns>The same point in view space</returns>
        public Vec2 ToViewSpace(Vec2 worldPoint)
        {
            var viewPoint = (worldPoint - Origin) / Scale;
            viewPoint.Y = -viewPoint.Y;
            return viewPoint;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private static readonly List<string> markerTypes = new()
        {
            "_001_helmetDrawingImage",
            "_003_coralDrawingImage",
            "_005_oxygen_tankDrawingImage",
            "_006_diverDrawingImage",
            "_004_flippersDrawingImage",
            "_007_cameraDrawingImage",
            "_008_submarineDrawingImage",
            "_009_flashlightDrawingImage",
            "_010_turtleDrawingImage",
            "_011_sharkDrawingImage",
            "_012_boatDrawingImage",
            "_013_reefDrawingImage",
            "_014_fishDrawingImage",
            "_016_puffer_fishDrawingImage",
            "_018_gaugeDrawingImage",
            "_021_harpoonDrawingImage",
            "_022_coralDrawingImage",
            "_023_coralDrawingImage",
            "_024_scuba_divingDrawingImage",
            "_026_jellyfishDrawingImage",
            "_027_clownfishDrawingImage",
            "_028_bagDrawingImage",
            "_029_caveDrawingImage",
            "_030_reefDrawingImage",
            "_031_manta_rayDrawingImage",
            "_032_snorkel_gearDrawingImage",
            "_033_shipwreckDrawingImage",
            "_034_fishDrawingImage",
            "_037_snorkelDrawingImage",
            "_038_face_maskDrawingImage",
            "_040_compassDrawingImage"
        };
        public static List<string> MarkerTypes => markerTypes;
    }
}
