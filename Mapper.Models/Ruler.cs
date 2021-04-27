﻿using System;
using System.ComponentModel;

namespace Mapper.Models
{
    public class Ruler : PropertyChangedBase
    {
        public Ruler(Vec2 startPoint)
        {
            Map.Instance.PropertyChanged += Map_PropertyChanged;
            ViewStartPoint = startPoint;
            ViewEndPoint = startPoint;
        }

        private void Map_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Origin" || e.PropertyName == "Scale")
            {
                OnPropertyChanged("ViewStartPoint");
                OnPropertyChanged("ViewEndPoint");
            }
        }

        #region World space properties
        private Vec2 startPoint = new Vec2(0);
        /// <summary>
        /// The starting point of the ruler in world space.
        /// </summary>
        public Vec2 StartPoint
        {
            get => startPoint;
            set
            {
                if (SetProperty(ref startPoint, value))
                {
                    OnPropertyChanged("StartPoint");
                    OnPropertyChanged("Length");
                    OnPropertyChanged("ViewStartPoint");
                    OnPropertyChanged("IsLargeArc");
                    OnPropertyChanged("Angle");
                    OnPropertyChanged("Direction");
                }
            }
        }

        private Vec2 endPoint = new Vec2(0);
        /// <summary>
        /// The end point of the ruler in world space.
        /// </summary>
        public Vec2 EndPoint
        {
            get => endPoint;
            set
            {
                if (SetProperty(ref endPoint, value))
                {
                    OnPropertyChanged("EndPoint");
                    OnPropertyChanged("Length");
                    OnPropertyChanged("ViewEndPoint");
                    OnPropertyChanged("IsLargeArc");
                    OnPropertyChanged("Angle");
                    OnPropertyChanged("Direction");
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
        public Vec2 Direction => (EndPoint - StartPoint).Unit();
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
                OnPropertyChanged("ViewMiddlePoint");
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
                OnPropertyChanged("ViewMiddlePoint");
                OnPropertyChanged("ArcRadius");
                OnPropertyChanged("ArcStartPoint");
                OnPropertyChanged("ArcEndPoint");
            }
        }

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
    }
}
