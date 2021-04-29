using System;
using System.ComponentModel;

namespace Mapper.Models
{
    public class Ruler : PropertyChangedBase
    {
        /// <summary>
        /// Construct a ruler from a point in view space and a height.
        /// </summary>
        /// <param name="startPoint">Point in view space.</param>
        /// <param name="height">Height above water level.</param>
        public Ruler(Vec2 startPoint, double height)
        {
            Map.Instance.PropertyChanged += Map_PropertyChanged;
            SetViewStartPoint(startPoint, height);
            SetViewEndPoint(startPoint);
        }

        /// <summary>
        /// Construct a ruler from a point in world space.
        /// </summary>
        /// <param name="startPoint">Point in world space.</param>
        public Ruler(Vec3 startPoint)
        {
            Map.Instance.PropertyChanged += Map_PropertyChanged;
            StartPoint = startPoint;
            EndPoint = startPoint;
        }

        private void Map_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Origin" || e.PropertyName == "Scale")
            {
                UpdateAll();
            }
        }

        #region World space properties
        private Vec3 startPoint = new Vec3(0, 0, 0);
        /// <summary>
        /// The starting point of the ruler in world space.
        /// </summary>
        public Vec3 StartPoint
        {
            get => startPoint;
            set
            {
                if (SetProperty(ref startPoint, value))
                {
                    UpdateAll();
                }
            }
        }

        private Vec3 endPoint = new Vec3(0, 0, 0);
        /// <summary>
        /// The end point of the ruler in world space.
        /// </summary>
        public Vec3 EndPoint
        {
            get => endPoint;
            set
            {
                if (SetProperty(ref endPoint, value))
                {
                    UpdateAll();
                }
            }
        }

        /// <summary>
        /// The length of the ruler in world space.
        /// </summary>
        public double Length => (EndPoint - StartPoint).Length();

        /// <summary>
        /// The current "compass" angle between north and the current angle, in degrees.
        /// Goes between 0 and 360, where north => 0, east => 90, south => 180 and west => 270.
        /// </summary>
        public double Angle
        {
            get
            {
                var dir = (EndPoint - StartPoint).Unit();
                var rotatedDir = new Vec2(dir.Y, -dir.X);  // Rotate vector so that north == (1, 0) (instead of (0, 1))
                //var rotatedDir = dir;
                var angle = Math.Atan2(rotatedDir.Y, rotatedDir.X);

                if (rotatedDir.Y > 0)
                    angle = Math.PI * 2.0 - angle;
                else
                    angle = -angle;
                return angle * 180.0 / Math.PI;
            }
        }

        /// <summary>
        /// The direction the ruler is pointing towards, as a unit vector in world space.
        /// </summary>
        public Vec3 Direction => (EndPoint - StartPoint).Unit();
        #endregion

        #region View space properties
        /// <summary>
        /// The start point of the ruler in view space.
        /// </summary>
        public Vec2 ViewStartPoint => Map.Instance.ToViewSpace(StartPoint);

        /// <summary>
        /// The end point of the ruler in view space
        /// </summary>
        public Vec2 ViewEndPoint => Map.Instance.ToViewSpace(EndPoint);

        /// <summary>
        /// The length of the ruler in view space.
        /// </summary>
        public double ViewLength => (ViewEndPoint - ViewStartPoint).Length();

        /// <summary>
        /// Middle point between start and end position.
        /// </summary>
        public Vec2 ViewMiddlePoint
        {
            get
            {
                var v = (ViewEndPoint - ViewStartPoint);
                return ViewStartPoint + v / 2.0;
            }
        }

        /// <summary>
        /// Set the start point from a view point and a height.
        /// </summary>
        /// <param name="pos">The position in view space.</param>
        /// <param name="height">The height above see level.</param>
        public void SetViewStartPoint(Vec2 pos, double height)
        {
            StartPoint = Map.Instance.ToWorldSpace(pos, height);
        }

        /// <summary>
        /// Set the end point from a view point and a height.
        /// </summary>
        /// <param name="pos">The position in view space.</param>
        /// <param name="height">The height above see level.</param>
        public void SetViewEndPoint(Vec2 pos)
        {
            EndPoint = Map.Instance.ToWorldSpace(pos, EndPoint.Z);
        }

        /// <summary>
        /// Set the depth (i.e. the inverse height) of the ruler end point.
        /// </summary>
        /// <param name="depth"></param>
        public void SetEndPointDepth(double depth)
        {
            EndPoint = new Vec3(EndPoint.X, EndPoint.Y, -depth);
        }
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
        public double ArcRadius => Math.Min(MaxArcRadius, ViewLength);

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

        #region Private methods
        private void UpdateAll()
        {
            OnPropertyChanged("Length");
            OnPropertyChanged("ViewStartPoint");
            OnPropertyChanged("ViewEndPoint");
            OnPropertyChanged("ViewLength");
            OnPropertyChanged("ViewMiddlePoint");

            OnPropertyChanged("IsLargeArc");
            OnPropertyChanged("Angle");
            OnPropertyChanged("Direction");
            OnPropertyChanged("ArcRadius");
            OnPropertyChanged("ArcStartPoint");
            OnPropertyChanged("ArcEndPoint");
        }
        #endregion
    }
}
