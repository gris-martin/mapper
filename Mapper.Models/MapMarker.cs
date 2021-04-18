using System.ComponentModel;

namespace Mapper.Models
{
    public class MapMarker : INotifyPropertyChanged
    {
        private static int staticId = 0;

        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="pos">Position of the marker in world space.</param>
        /// <param name="name">Name of the marker.</param>
        /// <param name="type">Type of the marker (i.e. the type of icon it should show).</param>
        public MapMarker(Vec2 pos, string name, string type)
        {
            this.worldPos = Map.Instance.ToWorldSpace(pos);
            this.name = name;
            this.Id = staticId;
            this.Type = type;
            staticId += 1;
        }

        private string type;
        /// <summary>
        /// The type of marker. Corresponds to a key in the resource dictionary
        /// containing the marker images.
        /// </summary>
        public string Type
        {
            get => type;
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

        private Vec2 worldPos;
        /// <summary>
        /// Position of the marker in world space.
        /// </summary>
        public Vec2 WorldPos
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
        /// Screen position. Should be updated manually each time <see cref="WorldPos"/> is updated.
        /// </summary>
        private Vec2 viewPos;
        public Vec2 ViewPos
        {
            get => viewPos;
            set
            {
                viewPos = value;
                NotifyPropertyChanged("ViewPos");
            }
        }

        /// <summary>
        /// Unique ID of this marker. Will be assigned automatically on creation.
        /// </summary>
        public int Id { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
