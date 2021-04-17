using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
    /// <summary>
    /// Convert a float value to a System.Windows.Size value with both its elements
    /// set to the value. This is used for setting the ArcSegment radius using a float.
    /// </summary>
    public class RadiusToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Error handling if value is of wrong type
            var radius = (float)value;
            return new Size(radius, radius);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Error handling if value is of wrong type or if size[0] != size[1]
            var size = (Size)value;
            return (float)size.Width;
        }
    }
}
