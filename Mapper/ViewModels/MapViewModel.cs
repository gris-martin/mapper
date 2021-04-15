using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Mapper.ViewModels
{
    class MapViewModel : INotifyPropertyChanged
    {
        public static MapViewModel Instance = new MapViewModel();

        public ObservableCollection<MapMarkerViewModel> MapSymbols { get; } = new ObservableCollection<MapMarkerViewModel>();


        public MapViewModel()
        {
            this.MapSymbols.CollectionChanged += MapSymbols_CollectionChanged;
        }


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
                    newSymbol.ScreenPos = ToViewSpace(newSymbol.WorldPos);
                    newSymbol.PropertyChanged += NewSymbol_PropertyChanged;
                }
            }
        }


        private void NewSymbol_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WorldPos")
            {
                var symbol = sender as MapMarkerViewModel;
                symbol.ScreenPos = ToViewSpace(symbol.WorldPos);
            }
        }


        private double lastZoom;
        public double LastZoom
        {
            get => lastZoom;
            set
            {
                lastZoom = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Current world coordinate of lower left corner
        /// </summary>
        public Point Origin {
            get => origin;
            set
            {
                origin = value;
                foreach (var symbol in MapSymbols)
                    symbol.ScreenPos = ToViewSpace(symbol.WorldPos);
                OnPropertyChanged();
            }
        }
        private Point origin = new Point(0, 0);

        /// <summary>
        /// Scale in meters per pixel
        /// </summary>
        public double Scale {
            get => scale;
            set
            {
                foreach (var symbol in MapSymbols)
                    symbol.ScreenPos = ToViewSpace(symbol.WorldPos);
                scale = value;
                OnPropertyChanged();
            }
        }
        private double scale = 1;


        /// <summary>
        /// Reset the Origin and Scale. I.e. Scale == 1 and Origin == (0, 0).
        /// </summary>
        public void Reset()
        {
            Scale = 1;
            Origin = new Point(0, 0);
        }


        /// <summary>
        /// Update the Origin from a mouse movement.
        /// </summary>
        /// <param name="lastPosition">The previous position of the mouse.</param>
        /// <param name="currentPosition">The current position of the mouse.</param>
        public void UpdateOriginFromMouseMovement(Point lastPosition, Point currentPosition)
        {
            double scaledDeltaX = (lastPosition.X - currentPosition.X) * Scale;
            double scaledDeltaY = (lastPosition.Y - currentPosition.Y) * Scale;
            double newX = Origin.X + scaledDeltaX;
            double newY = Origin.Y + scaledDeltaY;
            Origin = new Point(newX, newY);
        }


        /// <summary>
        /// Set the scale of the map given a view center.
        /// </summary>
        /// <param name="center">The point in view coordinates that the scaling should 
        /// be done around. This point will have the same coordinates (screen and world)
        /// before and after the scaling.</param>
        /// <param name="zoomIn">Set to true if the scaling is a zoom in operation 
        /// (Scale will decrease), and false if the scaling is a zoom out operation
        /// (Scale will increase).</param>
        public void SetScaleAroundPoint(Point center, bool zoomIn)
        {
            var zoomConstant = 1.3;
            var mousePos = ToWorldSpace(center);

            var scaleAmount = zoomConstant;
            if (zoomIn)
                scaleAmount = 1.0 / scaleAmount;

            Scale *= scaleAmount;

            var mouseX = mousePos.X;
            var mouseY = mousePos.Y;
            var oldX = origin.X;
            var oldY = origin.Y;

            // Zoom in
            if (zoomIn)
            {
                var newX = mouseX + (oldX - mouseX) * scaleAmount;
                var newY = mouseY + (oldY - mouseY) * scaleAmount;
                Origin = new Point(newX, newY);
            }
            // Zoom out
            else
            {
                var newX = oldX - (mouseX - oldX) * (scaleAmount - 1);
                var newY = oldY - (mouseY - oldY) * (scaleAmount - 1);
                Origin = new Point(newX, newY);
            }
            LastZoom = scaleAmount;
        }


        /// <summary>
        /// Convert a point in view space to world space
        /// </summary>
        /// <param name="viewPoint">A point in view space</param>
        /// <returns>The same point in world space</returns>
        public Point ToWorldSpace(Point viewPoint)
        {
            var viewX = viewPoint.X;
            var viewY = viewPoint.Y;
            var worldX = viewX * Scale + Origin.X;
            var worldY = viewY * Scale + Origin.Y;
            return new Point(worldX, worldY);
        }


        /// <summary>
        /// Convert a point in world space to view space
        /// </summary>
        /// <param name="worldPoint">A point in world space</param>
        /// <returns>The same point in view space</returns>
        public Point ToViewSpace(Point worldPoint)
        {
            var worldX = worldPoint.X;
            var worldY = worldPoint.Y;
            var viewX = (worldX - Origin.X) / Scale;
            var viewY = (worldY - Origin.Y) / Scale;
            return new Point(viewX, viewY);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
