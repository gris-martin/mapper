using Mapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mapper.ViewModels
{
    /// <summary>
    /// This view model is for displaying a scale indicator as a line,
    /// with an accompanying text mentioning how long the line is (in world coordinates)
    /// </summary>
    public class ScaleViewModel : ViewModelBase
    {
        /// <summary>
        /// We need to watch for changes in scale, so we need a custom constructor to add the callback.
        /// </summary>
        public ScaleViewModel()
        {
            Map.Instance.PropertyChanged += Map_PropertyChanged;
        }

        /// <summary>
        /// Callback to watch out for changes in scale.
        /// </summary>
        private void Map_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Scale")
            {
                OnPropertyChanged("EndPosition");
                OnPropertyChanged("ScaleText");
            }
        }

        /// <summary>
        /// The start (right) position of the line
        /// </summary>
        private Point startPosition = new Point(0, 0);
        public Point StartPosition {
            get => startPosition;
            set
            {
                startPosition = value;
                OnPropertyChanged("StartPosition");
                OnPropertyChanged("EndPoint");
            }
        }

        /// <summary>
        /// Discretize a value between 1 and 9.99... to be either 1, 2 or 5.
        /// </summary>
        /// <param name="v">The value to be discretized.</param>
        /// <returns>The discretized value</returns>
        private static double Discretize(double v)
        {
            if (v < 2)
                return 1;
            else if (v < 5)
                return 2;
            else
                return 5;
        }

        /// <summary>
        /// The end (left) position of the line
        /// </summary>
        public Point EndPosition {
            get {
                var numZeros = Math.Floor(Math.Log10(Map.Instance.Scale));
                var powerOf10 = Math.Pow(10, numZeros);
                var normalizedScale = Map.Instance.Scale / powerOf10;  // This is a number between 1 and 10
                var normalizedDiscreteScale = Discretize(normalizedScale);
                var discreteScale = normalizedDiscreteScale * powerOf10;
                var worldStartPoint = Map.Instance.ToWorldSpace(StartPosition.ToVec2());
                var worldEndPoint = worldStartPoint - new Vec2(100 * discreteScale, 0);
                return Map.Instance.ToViewSpace(worldEndPoint).ToPoint();
            }
        }

        /// <summary>
        /// A text representation of the scale.
        /// </summary>
        public string ScaleText
        {
            get
            {
                var roundedDistance = Math.Abs(EndPosition.X - StartPosition.X) * Map.Instance.Scale;
                if (roundedDistance < 10)
                    return $"{roundedDistance.ToString("G1")} m";
                else
                    return $"{Math.Round(roundedDistance)} m";
            }
        }
    }
}
