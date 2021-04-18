using Mapper.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
    /// <summary>
    /// Convert a Vec2 to a Point
    /// </summary>
    public class Vec2ToPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Handle errors when value is not of correct type. How?
            var v = (Vec2)value;
            return v.ToPoint();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Handle errors when value is not of correct type. How?
            var p = (Point)value;
            return p.ToVec2();
        }
    }
}
