using Mapper.Models;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;
using System;

namespace Mapper.ViewModels
{
    public class RulerViewModel : ViewModelBase
    {
        public RulerViewModel()
        {
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsMeasuring")
                OnPropertyChanged("Visibility");
            else if (e.PropertyName == "ArcStartPoint")
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

#pragma warning disable CA1822 // Mark members as static
        public Visibility Visibility => Model.IsMeasuring ? Visibility.Visible : Visibility.Hidden;
        public Point ArcStartPoint => Model.ArcStartPoint.ToPoint();
        public Point ArcEndPoint => Model.ArcEndPoint.ToPoint();
        public Size ArcRadius => new(Model.ArcRadius, Model.ArcRadius);
        public Point Direction => Model.Direction.ToPoint();
        public double AnglePopupHorizontalOffset => Model.ViewStartPoint.X - 5;
        public double AnglePopupVerticalOffset => Model.ViewStartPoint.Y + 5;
        public string AngleText
        {
            get
            {
                var angle = Math.Round(Model.Angle);
                return $"{angle}\n{GetDirectionString()}";
            }
        }
        public string LengthText
        {
            get
            {
                return $"{Math.Round(Model.Length)} m";
            }
        }
#pragma warning restore CA1822 // Mark members as static

        private string GetDirectionString()
        {
            var angle = Model.Angle;
            var i = 22.5;
            if (angle < 11.25)
                return "N";
            else if (angle < (12.25 + i))
                return "NNE";
            else if (angle < (12.25 + i * 2))
                return "NE";
            else if (angle < (12.25 + i * 3))
                return "ENE";
            else if (angle < (12.25 + i * 4))
                return "E";
            else if (angle < (12.25 + i * 5))
                return "ESE";
            else if (angle < (12.25 + i * 6))
                return "SE";
            else if (angle < (12.25 + i * 7))
                return "SSE";
            else if (angle < (12.25 + i * 8))
                return "S";
            else if (angle < (12.25 + i * 9))
                return "SSW";
            else if (angle < (12.25 + i * 10))
                return "SW";
            else if (angle < (12.25 + i * 11))
                return "WSW";
            else if (angle < (12.25 + i * 12))
                return "W";
            else if (angle < (12.25 + i * 13))
                return "WNW";
            else if (angle < (12.25 + i * 14))
                return "NW";
            else if (angle < (12.25 + i * 15))
                return "NNW";
            return "N";
        }


        /*
         *                     <PathFigure StartPoint="{Binding Model.ArcStartPoint, Converter={StaticResource Vec2ToPointConverter}}">
                        <ArcSegment Size="{Binding Model.ArcRadius, Converter={StaticResource RadiusToSizeConverter}}"
                                    Point="{Binding Model.ArcEndPoint, Converter={StaticResource Vec2ToPointConverter}}"

         */

        /// <summary>
        /// Underlying Ruler model
        /// </summary>
        public static Ruler Model => Ruler.Instance;
    }
}
