using Mapper.Models;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;
using System;

namespace Mapper.ViewModels
{
    public class RulerViewModel : ViewModelBase
    {
        public RulerViewModel(Ruler model)
        {
            this.model = model;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ArcStartPoint")
                OnPropertyChanged("ArcStartPoint");
            else if (e.PropertyName == "ArcEndPoint")
                OnPropertyChanged("ArcEndPoint");
            else if (e.PropertyName == "ArcRadius")
                OnPropertyChanged("ArcRadius");
            else if (e.PropertyName == "Direction")
                OnPropertyChanged("Direction");
            else if (e.PropertyName == "ViewStartPoint")
            {
                OnPropertyChanged("AnglePopupHorizontalOffset");
                OnPropertyChanged("AnglePopupVerticalOffset");
            }
            else if (e.PropertyName == "Angle")
                OnPropertyChanged("AngleText");
            else if (e.PropertyName == "Length")
                OnPropertyChanged("LengthText");
        }

        public Point ArcStartPoint => Model.ArcStartPoint.ToPoint();
        public Point ArcEndPoint => Model.ArcEndPoint.ToPoint();
        public Size ArcRadius => new(Model.ArcRadius, Model.ArcRadius);
        public Point Direction => Model.Direction.ToVec2().ToPoint();
        public double AnglePopupHorizontalOffset => Model.ViewStartPoint.X - 5;
        public double AnglePopupVerticalOffset => Model.ViewStartPoint.Y + 5;
        public string AngleText
        {
            get
            {
                var angle = Math.Round(Model.Angle);
                return $"{angle}°\n{Models.Utils.GetCompassString(Model.Angle)}";
            }
        }
        public string LengthText
        {
            get
            {
                return $"{Math.Round(Model.Length)} m";
            }
        }

        private Ruler model;
        /// <summary>
        /// Underlying Ruler model
        /// </summary>
        public Ruler Model => model;
    }
}
