﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mapper.ViewModels
{
    public class RulerViewModel : INotifyPropertyChanged
    {
        //public static RulerViewModel Instance = new();

        #region World space properties
        private Vector2 startPoint = new(0);
        /// <summary>
        /// The starting point of the ruler in world space.
        /// </summary>
        public Vector2 StartPoint
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

        private Vector2 endPoint = new(0);
        /// <summary>
        /// The end point of the ruler in world space.
        /// </summary>
        public Vector2 EndPoint
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
        public float Length => (EndPoint - StartPoint).Length();
        #endregion

        #region View space properties
        /// <summary>
        /// The start point of the ruler in view space.
        /// </summary>
        public Vector2 ViewStartPoint
        {
            get => MapViewModel.Instance.ToViewSpace(StartPoint);
            set
            {
                StartPoint = MapViewModel.Instance.ToWorldSpace(value);
                OnPropertyChanged("ViewStartPoint");
                OnPropertyChanged("ViewLength");
                OnPropertyChanged("ViewStartX");
                OnPropertyChanged("ViewStartY");
                OnPropertyChanged("ArcStartPoint");
                OnPropertyChanged("ArcEndPoint");
            }
        }

        /// <summary>
        /// The end point of the ruler in view space
        /// </summary>
        public Vector2 ViewEndPoint
        {
            get => MapViewModel.Instance.ToViewSpace(EndPoint);
            set
            {
                EndPoint = MapViewModel.Instance.ToWorldSpace(value);
                OnPropertyChanged("ViewEndPoint");
                OnPropertyChanged("ViewLength");
                OnPropertyChanged("ViewEndX");
                OnPropertyChanged("ViewEndY");
                OnPropertyChanged("ArcEndPoint");
            }
        }

        /// <summary>
        /// The length of the ruler in view space.
        /// </summary>
        public float ViewLength => (ViewEndPoint - ViewStartPoint).Length();

        /// <summary>
        /// X component of the end point in view space (for data binding).
        /// </summary>
        public float ViewEndX => ViewEndPoint.X;

        /// <summary>
        /// Y component of the end point in view space (for data binding).
        /// </summary>
        public float ViewEndY => ViewEndPoint.Y;

        /// <summary>
        /// X component of the start point in view space (for data binding).
        /// </summary>
        public float ViewStartX => ViewStartPoint.X;

        /// <summary>
        /// Y component of the start point in view space (for data binding).
        /// </summary>
        public float ViewStartY => ViewStartPoint.Y;

        #endregion

        #region Arc-related properties
        private float arcRadius = 40;

        /// <summary>
        /// Radius of the drawn arc in view space.
        /// </summary>
        public float ArcRadius
        {
            get => arcRadius;
            set
            {
                arcRadius = value;
                OnPropertyChanged("ArcRadis");
                OnPropertyChanged("ArcEndPoint");
                OnPropertyChanged("ArcStartPoint");
                OnPropertyChanged("ArcEndPoint");
            }
        }

        /// <summary>
        /// True if the arc is larger than 180 degrees, false otherwise.
        /// </summary>
        public bool IsLargeArc => EndPoint.X < StartPoint.X;

        /// <summary>
        /// Start point of the arc in view space.
        /// </summary>
        public Vector2 ArcStartPoint => ViewStartPoint + MapViewModel.Instance.North * ArcRadius;

        /// <summary>
        /// End point of the arc in view space.
        /// </summary>
        public Vector2 ArcEndPoint => ViewStartPoint + (ViewEndPoint - ViewStartPoint).UnitVector() * ArcRadius;

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