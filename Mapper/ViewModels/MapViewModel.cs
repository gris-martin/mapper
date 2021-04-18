using Mapper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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

#pragma warning disable CA1822 // Mark members as static
        public ObservableCollection<MarkerViewModel> Markers =>
            new(Model.Markers.Select(marker => new MarkerViewModel(marker)));
#pragma warning restore CA1822 // Mark members as static
    }
}
