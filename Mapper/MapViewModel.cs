using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Mapper
{
    class MapViewModel : INotifyPropertyChanged
    {
        public static MapViewModel Instance = new MapViewModel();

        public ObservableCollection<MapSymbolModel> MapSymbols { get; } = new ObservableCollection<MapSymbolModel>();


        public MapViewModel()
        {
            this.MapSymbols.CollectionChanged += MapSymbols_CollectionChanged;
        }


        private void MapSymbols_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (MapSymbolModel removedSymbol in e.OldItems)
                {
                    removedSymbol.PropertyChanged -= NewSymbol_PropertyChanged;
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (MapSymbolModel newSymbol in e.NewItems)
                {
                    newSymbol.ScreenPos = ToScreenCoordinate(newSymbol.WorldPos);
                    newSymbol.PropertyChanged += NewSymbol_PropertyChanged;
                }
            }
        }


        private void NewSymbol_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WorldPos")
            {
                var symbol = sender as MapSymbolModel;
                symbol.ScreenPos = ToScreenCoordinate(symbol.WorldPos);
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
                    symbol.ScreenPos = ToScreenCoordinate(symbol.WorldPos);
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
                    symbol.ScreenPos = ToScreenCoordinate(symbol.WorldPos);
                scale = value;
                OnPropertyChanged();
            }
        }
        private double scale = 1;


        public Point ToWorldCoordinate(Point screenPoint)
        {
            var screenX = screenPoint.X;
            var screenY = screenPoint.Y;
            var worldX = screenX * Scale + Origin.X;
            var worldY = screenY * Scale + Origin.Y;
            return new Point(worldX, worldY);
        }


        public Point ToScreenCoordinate(Point worldPoint)
        {
            var worldX = worldPoint.X;
            var worldY = worldPoint.Y;
            var screenX = (worldX - Origin.X) / Scale;
            var screenY = (worldY - Origin.Y) / Scale;
            return new Point(screenX, screenY);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
