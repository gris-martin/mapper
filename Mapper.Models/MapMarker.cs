using Newtonsoft.Json;

namespace Mapper.Models
{
    public class MapMarker : PropertyChangedBase
    {
        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="worldPos">Position of the marker in world space.</param>
        /// <param name="name">Name of the marker.</param>
        /// <param name="type">Type of the marker (i.e. the type of icon it should show).</param>
        public MapMarker(Vec2 worldPos, string name, string type)
        {
            //this.worldPos = Map.Instance.ToWorldSpace(worldPos);
            this.worldPos = worldPos;
            this.name = name;
            this.Type = type;
        }

        /// <summary>
        /// Function for creating a marker from a position in view space.
        /// </summary>
        /// <param name="viewPos">Position of the marker in view space.</param>
        /// <param name="name">Name of the marker.</param>
        /// <param name="type">Type of the marker (i.e. the type of icon it should show).</param>
        /// <returns>A new MapMarker.</returns>
        public static MapMarker CreateFromViewPos(Vec2 viewPos, string name, string type)
            => new MapMarker(Map.Instance.ToWorldSpace(viewPos), name, type);

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
                OnPropertyChanged("Type");
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
                OnPropertyChanged("WorldPos");
                OnPropertyChanged("ViewPos");
            }
        }

        /// <summary>
        /// The name of the marker, i.e. what is displayed on the map when hovering.
        /// </summary>
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        }

        /// <summary>
        /// Screen position. Should be updated manually each time <see cref="WorldPos"/> is updated.
        /// </summary>
        [JsonIgnore]
        public Vec2 ViewPos
        {
            get => Map.Instance.ToViewSpace(this.WorldPos);
            set
            {
                this.WorldPos = Map.Instance.ToWorldSpace(value);
                OnPropertyChanged("ViewPos");
            }
        }
    }
}
