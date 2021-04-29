using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace Mapper.Models
{
    public class Map : PropertyChangedBase
    {
        private static readonly Map instance = new Map();
        public static Map Instance => instance;

        #region Public properties
        private Vec3 origin = new Vec3(0, 0, 0);
        /// <summary>
        /// Current world coordinate of upper left corner
        /// </summary>
        public Vec3 Origin
        {
            get => origin;
            set
            {
                origin = value;
                foreach (var marker in Markers)
                    marker.OnPropertyChanged("ViewPos");
                OnPropertyChanged("Origin");
                OnPropertyChanged("North");
            }
        }

        private double scale = 1;
        /// <summary>
        /// Scale in meters per pixel
        /// </summary>
        public double Scale
        {
            get => scale;
            set
            {
                foreach (var marker in Markers)
                    marker.OnPropertyChanged("ViewPos");
                scale = value;
                OnPropertyChanged("Scale");
                OnPropertyChanged("North");
            }
        }

        private Vec2 north = new Vec2(0, -1);
        /// <summary>
        /// A unit vector in view space which corresponds to north in world space
        /// </summary>
        [JsonIgnore]
        public Vec2 North
        {
            get => north;
            set
            {
                north = value;
                OnPropertyChanged("North");
            }
        }

        private ObservableCollection<Ruler> rulers = new ObservableCollection<Ruler>();
        public ObservableCollection<Ruler> Rulers => rulers;

        #endregion

        #region Public methods
        /// <summary>
        /// Reset the Origin and Scale. I.e. Scale == 1 and Origin == (0, 0, 0).
        /// </summary>
        public void Reset()
        {
            Scale = 1;
            Origin = new Vec3(0, 0, 0);
        }

        /// <summary>
        /// Update the Origin from a mouse movement.
        /// </summary>
        /// <param name="lastPosition">The previous position of the mouse in view space.</param>
        /// <param name="currentPosition">The current position of the mouse in view space.</param>
        public void UpdateOriginFromMouseMovement(Vec2 lastPosition, Vec2 currentPosition)
        {
            Vec2 scaledDelta = (lastPosition - currentPosition) * Scale;
            scaledDelta.Y = -scaledDelta.Y;
            Origin += new Vec3(scaledDelta, 0);
        }

        /// <summary>
        /// Set the scale of the map given a view center.
        /// </summary>
        /// <param name="center">The point in view coordinates that the scaling should
        /// be done around. This point will have the same coordinates (view and world)
        /// before and after the scaling.</param>
        /// <param name="zoomIn">Set to true if the scaling is a zoom in operation
        /// (Scale will decrease), and false if the scaling is a zoom out operation
        /// (Scale will increase).</param>
        public void SetScaleAroundPoint(Vec2 center, bool zoomIn)
        {
            var zoomConstant = 1.3;
            var mousePos = ToWorldSpace(center);

            var scaleAmount = zoomConstant;
            if (zoomIn)
                scaleAmount = 1.0 / scaleAmount;

            Scale *= scaleAmount;

            // Zoom in
            if (zoomIn)
            {
                var newOrigin = mousePos + (Origin - mousePos) * scaleAmount;
                Origin = newOrigin;
            }
            // Zoom out
            else
            {
                var newOrigin = Origin - (mousePos - Origin) * (scaleAmount - 1);
                Origin = newOrigin;
            }
        }

        /// <summary>
        /// Convert a point in view space to world space
        /// </summary>
        /// <param name="viewPoint">A point in view space</param>
        /// <returns>The same point in world space</returns>
        public Vec3 ToWorldSpace(Vec2 viewPoint, double height = 0)
        {
            var scaledViewPoint = viewPoint * Scale;
            scaledViewPoint.Y = -scaledViewPoint.Y;
            return Origin + new Vec3(scaledViewPoint, height);
        }

        /// <summary>
        /// Convert a point in world space to view space
        /// </summary>
        /// <param name="worldPoint">A point in world space</param>
        /// <returns>The same point in view space</returns>
        public Vec2 ToViewSpace(Vec3 worldPoint)
        {
            var viewPoint = (worldPoint - Origin) / Scale;
            viewPoint.Y = -viewPoint.Y;
            return new Vec2(viewPoint.X, viewPoint.Y);
        }

        /// <summary>
        /// Add a new ruler at the specified position in view space.
        /// </summary>
        /// <param name="viewStartPoint">The start position of the ruler in view space.</param>
        /// <param name="height">The start height of the ruler.</param>
        public void AddRuler(Vec2 viewStartPoint, double height)
            => this.Rulers.Add(new Ruler(viewStartPoint, height));

        /// <summary>
        /// Add a new ruler at a position in world space.
        /// </summary>
        /// <param name="startPoint">The start position of the ruler in world space.</param>
        public void AddRuler(Vec3 viewStartPoint)
            => this.Rulers.Add(new Ruler(viewStartPoint));

        /// <summary>
        /// Clear all rulers from map
        /// </summary>
        public void ClearRulers()
            => this.Rulers.Clear();

        /// <summary>
        /// Serialize this Map object into a file.
        /// </summary>
        /// <param name="filepath">Path of the file to be serialized to.</param>
        public void SaveToFile(string filepath)
        {
            var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            var folder = Directory.GetParent(filepath);
            Directory.CreateDirectory(folder.FullName);
            File.WriteAllText(filepath, jsonString);
        }

        /// <summary>
        /// Deserialize a file into this Map object.
        /// </summary>
        /// <param name="filepath">Path of the file to be deserialized.</param>
        public void LoadFromFile(string filepath)
        {
            var jsonString = File.ReadAllText(filepath);
            var newMap = JsonConvert.DeserializeObject<Map>(jsonString);
            this.Markers.Clear();
            foreach (var newMarker in newMap.Markers)
            {
                if (!markerTypes.Contains(newMarker.Type))
                    newMarker.Type = markerTypes[0];
                this.Markers.Add(newMarker);
            }
            this.Origin = newMap.Origin;
            this.Scale = newMap.Scale;
        }
        #endregion

        #region Markers
        /// <summary>
        /// List of all the map symbols assigned to this view model
        /// </summary>
        public ObservableCollection<MapMarker> Markers { get; } = new ObservableCollection<MapMarker>();

        private static readonly List<string> markerTypes = new List<string>()
        {
            "_001_helmetDrawingImage",
            "_003_coralDrawingImage",
            "_005_oxygen_tankDrawingImage",
            "_006_diverDrawingImage",
            "_004_flippersDrawingImage",
            "_007_cameraDrawingImage",
            "_008_submarineDrawingImage",
            "_009_flashlightDrawingImage",
            "_010_turtleDrawingImage",
            "_011_sharkDrawingImage",
            "_012_boatDrawingImage",
            "_013_reefDrawingImage",
            "_014_fishDrawingImage",
            "_016_puffer_fishDrawingImage",
            "_018_gaugeDrawingImage",
            "_021_harpoonDrawingImage",
            "_022_coralDrawingImage",
            "_023_coralDrawingImage",
            "_024_scuba_divingDrawingImage",
            "_026_jellyfishDrawingImage",
            "_027_clownfishDrawingImage",
            "_028_bagDrawingImage",
            "_029_caveDrawingImage",
            "_030_reefDrawingImage",
            "_031_manta_rayDrawingImage",
            "_032_snorkel_gearDrawingImage",
            "_033_shipwreckDrawingImage",
            "_034_fishDrawingImage",
            "_037_snorkelDrawingImage",
            "_038_face_maskDrawingImage",
            "_040_compassDrawingImage"
        };
        public static List<string> MarkerTypes => markerTypes;
        #endregion
    }
}
