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


        private readonly List<MarkerViewModel> markers = Map.MarkerTypes.Select(type => new MarkerViewModel(new Marker(type))).ToList();
        /// <summary>
        /// All the markers that can be chosen
        /// </summary>
        public IEnumerable<MarkerViewModel> Markers => markers;

        /// <summary>
        /// Position of the new marker to be created. Should probably be set by the entity creating this ViewModel's view.
        /// </summary>
        public Point Position {
            get => this._marker.ViewPos.ToPoint();
            set
            {
                this._marker.ViewPos = value.ToVec2();
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// The type of marker to be created.
        /// </summary>
        public string Type {
            get => this._marker.Type;
            set
            {
                this._marker.Type = value;
                OnPropertyChanged("Type");
                OnPropertyChanged("OkCommandEnabled");
            }
        }

        /// <summary>
        /// The name of the marker to be created.
        /// </summary>
        public string Name {
            get => this._marker.Name;
            set
            {
                this._marker.Name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("OkCommandEnabled");
            }
        }

        /// <summary>
        /// The depth of the marker, represented as a string
        /// </summary>
        public string Depth
        {
            get
            {
                var d = (-this._marker.WorldPos.Z).ToString();
                if (d == "-0") d = "0";
                return d;
            }
            set
            {
                var v = value.Replace(".", ",");
                var success = double.TryParse(v, out double d);
                if (success)
                {
                    this._marker.WorldPos = new Vec3(this._marker.WorldPos.X, this._marker.WorldPos.Y, -d);
                    OnPropertyChanged("Depth");
                    OnPropertyChanged("OkCommandEnabled");
                }
            }
        }

        public string Description
        {
            get => _marker.Description;
            set => _marker.Description = value;
        }

        /// <summary>
        /// Should the OK button be enabled?
        /// </summary>
        public bool OkCommandEnabled =>
            !string.IsNullOrEmpty(Type) &&
            !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Depth);

        /// <summary>
        /// Function to be called when the OK button is clicked. Adds a new marker to the Map.
        /// </summary>
        /// <returns>False if the name or the type has not been set, true otherwise.</returns>
        public bool OkClickedCommand()
        {
            if (OkCommandEnabled)
            {
                if (!_isEditing)
                    Map.Instance.Markers.Add(this._marker);
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
            UpdateBorders();
        }

        /// <summary>
        /// Update the borders of the markers. Should remove the borders from all markers except for
        /// the one with type specified by the Type property.
        /// </summary>
        private void UpdateBorders()
        {
            // Maybe not the most elegant, but performance doesn't seem to be a
            // problem unless there are very many markers.
            foreach (var marker in Markers)
            {
                if (marker.Model.Type == this.Type)
                    marker.BorderColor = new SolidColorBrush(Colors.Black);
                else
                    marker.BorderColor = new SolidColorBrush(Colors.Transparent);
            }
        }

        public void StartEdit(Marker marker)
        {
            this._isEditing = true;
            this._marker = marker;
            UpdateBorders();
            OnPropertyChanged("Name");
            OnPropertyChanged("Type");
            OnPropertyChanged("Description");
            OnPropertyChanged("OkCommandEnabled");
        }

        private bool _isEditing = false;
        private Marker _marker = new Marker(new Vec3(), "", "");
    }
}
