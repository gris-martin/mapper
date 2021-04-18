using Mapper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mapper.ViewModels
{
    public class MarkerDialogViewModel : INotifyPropertyChanged
    {
        public static List<string> Markers => Map.MarkerTypes;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
