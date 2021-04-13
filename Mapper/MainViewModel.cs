using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Mapper
{
    class MainViewModel
    {
        public static MainViewModel Instance = new MainViewModel();

        public ObservableCollection<MapSymbolModel> MapSymbols { get; } = new ObservableCollection<MapSymbolModel>();

        /// <summary>
        /// Current world coordinate of lower left corner
        /// </summary>
        public Point Origin { get; set; }

        /// <summary>
        /// Scale in meters per pixel
        /// </summary>
        public double Scale { get; set; }


        public Point ToWorldCoordinate(Point screenPoint, double screenHeight)
        {
            var screenX = screenPoint.X;
            var screenY = screenPoint.Y;
            var worldX = screenX * Scale + Origin.X;
            var worldY = (screenHeight - screenY) * Scale + Origin.Y;
            return new Point(worldX, worldY);
        }


        public Point ToScreenCoordinate(Point worldPoint, double screenHeight)
        {
            var worldX = worldPoint.X;
            var worldY = worldPoint.Y;
            var screenX = (worldX - Origin.X) / Scale;
            var screenY = (worldY - Origin.Y) / Scale - screenHeight;
            return new Point(screenX, screenY);
        }
    }
}
