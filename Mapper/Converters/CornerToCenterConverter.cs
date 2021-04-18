using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
    /// <summary>
    /// By default the markers are placed so that the Canvas.Left and Canvas.Right positions refers
    /// to their upper left corner. This converter makes sure that the position refers to the
    /// center of the marker instead.
    /// </summary>
    public class CornerToCenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double pos = (double)value;
            double markerSize = (double)Application.Current.FindResource("MarkerSize");
            return pos - markerSize / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double pos = (double)value;
            double markerSize = (double)Application.Current.FindResource("MarkerSize");
            return pos + markerSize / 2.0;
        }
    }
}
