using Mapper.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Mapper.ViewModels
{
    public class MarkerDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// Helper class containing a marker type and its corresponding source
        /// </summary>
        public class MarkerSource
        {
            private MarkerSource() { }
            public MarkerSource(string type)
            {
                this.Type = type;
            }
            public string Type { get; }
            public ImageSource Source
                => Application.Current.FindResource(Type) as ImageSource;
        }

        public static IEnumerable<MarkerSource> Markers
            => Map.MarkerTypes.Select(type => new MarkerSource(type));
    }
}
