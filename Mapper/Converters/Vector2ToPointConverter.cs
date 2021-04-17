using System;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
    /// <summary>
    /// Convert a Vector2 to a Point
    /// </summary>
    public class Vector2ToPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Handle errors when value is not of correct type. How?
            var v = (Vector2)value;
            return v.ToPoint();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Handle errors when value is not of correct type. How?
            var p = (Point)value;
            return p.ToVector2();
        }
    }
}
