using Mapper.Models;
using System.ComponentModel;

namespace Mapper.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public static Map Map => Map.Instance;
        public static Ruler Ruler => Ruler.Instance;

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
