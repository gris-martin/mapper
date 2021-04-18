using Mapper.Models;
using System.ComponentModel;
using System.Windows;

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
            if (e.PropertyName == "IsHidden")
                OnPropertyChanged("Visibility");
            if (e.PropertyName == "ArcStartPoint")
                OnPropertyChanged("ArcStartPoint");
            if (e.PropertyName == "ArcEndPoint")
                OnPropertyChanged("ArcEndPoint");
            if (e.PropertyName == "ArcRadius")
                OnPropertyChanged("ArcRadius");
        }

        public Visibility Visibility => Model.IsHidden ? Visibility.Hidden : Visibility.Visible;
        public Point ArcStartPoint => Model.ArcStartPoint.ToPoint();
        public Point ArcEndPoint => Model.ArcEndPoint.ToPoint();
        public Size ArcRadius => new Size(Model.ArcRadius, Model.ArcRadius);

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
