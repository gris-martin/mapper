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


        private readonly List<MarkerSource> markers = Map.MarkerTypes.Select(type => new MarkerSource(type)).ToList();
        /// <summary>
        /// All the markers that can be chosen
        /// </summary>
        public IEnumerable<MarkerSource> Markers => markers;

        /// <summary>
        /// Position of the new marker to be created. Should probably be set by the entity creating this ViewModel's view.
        /// </summary>
        public Point Position { get; set; }

        private string type;
        /// <summary>
        /// The type of marker to be created.
        /// </summary>
        public string Type {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged("Type");
                OnPropertyChanged("OkCommandEnabled");
            }
        }

        private string name;
        /// <summary>
        /// The name of the marker to be created.
        /// </summary>
        public string Name {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("OkCommandEnabled");
            }
        }

        //private bool okCommandEnabled = false;
        public bool OkCommandEnabled => !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Name);
        //{
        //    get => okCommandEnabled;
        //    set
        //    {
        //        okCommandEnabled = value;
        //        OnPropertyChanged("OkCommandEnabled");
        //    }
        //}

        /// <summary>
        /// Function to be called when the OK button is clicked. Adds a new marker to the Map.
        /// </summary>
        /// <returns>False if the name or the type has not been set, true otherwise.</returns>
        public bool OkClickedCommand()
        {
            if (OkCommandEnabled)
            {
                Map.Instance.Markers.Add(new MapMarker(Position.ToVec2(), this.Name, this.Type));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Function to be called when a marker is clicked.
        /// </summary>
        /// <param name="type">The type of marker that was clicked.</param>
        public void MarkerClickedCommand(string type)
        {
            this.Type = type;

            // Maybe not the most elegant, but performance doesn't seem to be a
            // problem unless there are very many markers.
            foreach (var marker in Markers)
            {
                if (marker.Type == type)
                    marker.BorderBrush = new SolidColorBrush(Colors.Black);
                else
                    marker.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }

        /// <summary>
        /// Helper class containing a marker type and its corresponding source
        /// </summary>
        public class MarkerSource : ViewModelBase
        {
            private MarkerSource() { }
            public MarkerSource(string type)
            {
                this.Type = type;
            }
            public string Type { get; }
            public ImageSource Source
                => Application.Current.FindResource(Type) as ImageSource;

            private Brush borderBrush = new SolidColorBrush(Colors.Transparent);
            public Brush BorderBrush
            {
                get => borderBrush;
                set
                {
                    borderBrush = value;
                    OnPropertyChanged("BorderBrush");
                }
            }
        }
    }
}
