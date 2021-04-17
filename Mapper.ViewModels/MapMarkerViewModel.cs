using System.ComponentModel;
using System.Numerics;

namespace Mapper.ViewModels
{
    public class MapMarkerViewModel : INotifyPropertyChanged
    {
        private static int staticId = 0;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="pos">Position of the marker in world space.</param>
        /// <param name="name">Name of the marker.</param>
        /// <param name="type">Type of the marker (i.e. the type of icon it should show).</param>
        public MapMarkerViewModel(Vector2 pos, string name, string type)
        {
            this.worldPos = MapViewModel.Instance.ToWorldSpace(pos);
            this.name = name;
            this.Id = staticId;
            this.type = type;
            staticId += 1;
        }

        /// <summary>
        /// The type of marker. Corresponds to a key in the resource dictionary
        /// containing the marker images.
        /// </summary>
        private string type;
        public string Type
        {
            get => type;
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

        /// <summary>
        /// Position of the marker in world space.
        /// </summary>
        private Vector2 worldPos;
        public Vector2 WorldPos
        {
            get => worldPos;
            set
            {
                worldPos = value;
                NotifyPropertyChanged("WorldPos");
            }
        }

        /// <summary>
        /// The name of the marker, i.e. what is displayed on the map when hovering.
        /// </summary>
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Screen position. Should be updated each time <see cref="WorldPos"/> is updated.
        /// </summary>
        private Vector2 screenPos;
        public Vector2 ScreenPos
        {
            get => screenPos;
            set
            {
                screenPos = value;
                NotifyPropertyChanged("ScreenPos");
                NotifyPropertyChanged("ScreenPosX");
                NotifyPropertyChanged("ScreenPosY");
            }
        }

        /// <summary>
        /// X position in screen space. Has to make this property since WPF
        /// can't access field data bindings, and .X and .Y of Vector2 are fields...
        /// </summary>
        public float ScreenPosX => ScreenPos.X;

        /// <summary>
        /// Y position in screen space. Has to make this property since WPF
        /// can't access field data bindings, and .X and .Y of Vector2 are fields...
        /// </summary>
        public float ScreenPosY => ScreenPos.Y;

        /// <summary>
        /// Unique ID of this marker. Will be assigned automatically during creation.
        /// </summary>
        public int Id { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
