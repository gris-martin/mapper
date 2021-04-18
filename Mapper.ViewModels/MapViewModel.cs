﻿using System.Collections.ObjectModel;
using System.ComponentModel;

using Mapper.Models;

namespace Mapper.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public static MapViewModel Instance = new();

        public MapViewModel()
        {
            this.MapSymbols.CollectionChanged += MapSymbols_CollectionChanged;
        }

        #region Callbacks
        /// <summary>
        /// Add a callback for all newly created markers to make sure their view position is updated
        /// when the world position is updated.
        /// </summary>
        private void MapSymbols_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (MapMarkerViewModel removedSymbol in e.OldItems)
                {
                    removedSymbol.PropertyChanged -= NewSymbol_PropertyChanged;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (MapMarkerViewModel newSymbol in e.NewItems)
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
                var symbol = sender as MapMarkerViewModel;
                symbol.ViewPos = ToViewSpace(symbol.WorldPos);
            }
        }
        #endregion

        /// <summary>
        /// List of all the map symbols assigned to this view model
        /// </summary>
        public ObservableCollection<MapMarkerViewModel> MapSymbols { get; } = new ObservableCollection<MapMarkerViewModel>();

        /// <summary>
        /// The ruler view model related to this map view model
        /// </summary>
        public RulerViewModel Ruler { get; } = new();

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
                foreach (var symbol in MapSymbols)
                    symbol.ViewPos = ToViewSpace(symbol.WorldPos);
                OnPropertyChanged("Origin");
                OnPropertyChanged("OriginX");
                OnPropertyChanged("OriginY");
                OnPropertyChanged("North");
                Ruler.OnPropertyChanged("ViewStartPoint");
                Ruler.OnPropertyChanged("ViewEndPoint");
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
                foreach (var symbol in MapSymbols)
                    symbol.ViewPos = ToViewSpace(symbol.WorldPos);
                scale = value;
                OnPropertyChanged("Scale");
                OnPropertyChanged("North");
                Ruler.OnPropertyChanged("ViewStartPoint");
                Ruler.OnPropertyChanged("ViewEndPoint");
            }
        }

        /// <summary>
        /// A unit vector in view space which corresponds to north in world space
        /// </summary>
        public Vec2 North
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
    }
}
