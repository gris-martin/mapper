using System;
using System.ComponentModel;

namespace Mapper.Models
{
    public class Ruler : INotifyPropertyChanged
    {
        public static Ruler Instance = new();

        private Ruler() { }

        #region World space properties
        private Vec2 startPoint = new(0);
        /// <summary>
        /// The starting point of the ruler in world space.
        /// </summary>
        public Vec2 StartPoint
        {
            get => startPoint;
            set
            {
                startPoint = value;
                OnPropertyChanged("StartPoint");
                OnPropertyChanged("ViewStartPoint");
                OnPropertyChanged("IsLargeArc");
            }
        }

        private Vec2 endPoint = new(0);
        /// <summary>
        /// The end point of the ruler in world space.
        /// </summary>
        public Vec2 EndPoint
        {
            get => endPoint;
            set
            {
                endPoint = value;
                OnPropertyChanged("EndPoint");
                OnPropertyChanged("ViewEndPoint");
                OnPropertyChanged("IsLargeArc");
            }
        }

        /// <summary>
        /// The length of the ruler in world space.
        /// </summary>
        public double Length => (EndPoint - StartPoint).Length();
        #endregion

        #region View space properties
        /// <summary>
        /// The start point of the ruler in view space.
        /// </summary>
        public Vec2 ViewStartPoint
        {
            get => Map.Instance.ToViewSpace(StartPoint);
            set
            {
                StartPoint = Map.Instance.ToWorldSpace(value);
                OnPropertyChanged("ViewStartPoint");
                OnPropertyChanged("ViewLength");
                OnPropertyChanged("ArcRadius");
                OnPropertyChanged("ArcStartPoint");
                OnPropertyChanged("ArcEndPoint");
            }
        }

        /// <summary>
        /// The end point of the ruler in view space
        /// </summary>
        public Vec2 ViewEndPoint
        {
            get => Map.Instance.ToViewSpace(EndPoint);
            set
            {
                EndPoint = Map.Instance.ToWorldSpace(value);
                OnPropertyChanged("ViewEndPoint");
                OnPropertyChanged("ViewLength");
                OnPropertyChanged("ArcRadius");
                OnPropertyChanged("ArcStartPoint");
                OnPropertyChanged("ArcEndPoint");
            }
        }

        /// <summary>
        /// The length of the ruler in view space.
        /// </summary>
        public double ViewLength => (ViewEndPoint - ViewStartPoint).Length();

        #endregion

        #region Arc-related properties
        private double maxArcRadius = 40;
        /// <summary>
        /// The maximum radius of the arc.
        /// </summary>
        public double MaxArcRadius
        {
            get => maxArcRadius;
            set
            {
                maxArcRadius = value;
                OnPropertyChanged("MaxArcRadius");
                OnPropertyChanged("ArcRadius");
                OnPropertyChanged("ArcStartPoint");
                OnPropertyChanged("ArcEndPoint");
            }
        }

        /// <summary>
        /// Radius of the drawn arc in view space. This will be the smallest value
        /// of the MaxArcRadius and the length of the ruler.
        /// </summary>
        public double ArcRadius
        {
            get => Math.Min(MaxArcRadius, ViewLength);
        }

        /// <summary>
        /// True if the arc is larger than 180 degrees, false otherwise.
        /// </summary>
        public bool IsLargeArc => EndPoint.X < StartPoint.X;

        /// <summary>
        /// Start point of the arc in view space.
        /// </summary>
        public Vec2 ArcEndPoint => ViewStartPoint + Map.Instance.North * ArcRadius;

        /// <summary>
        /// End point of the arc in view space.
        /// </summary>
        public Vec2 ArcStartPoint => ViewStartPoint + (ViewEndPoint - ViewStartPoint).Unit() * ArcRadius;

        #endregion

        #region Other properties
        private bool isHidden = true;
        /// <summary>
        /// True if the ruler should be hidden, false otherwise.
        /// </summary>
        public bool IsHidden
        {
            get => isHidden;
            set
            {
                isHidden = value;
                OnPropertyChanged("IsHidden");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
