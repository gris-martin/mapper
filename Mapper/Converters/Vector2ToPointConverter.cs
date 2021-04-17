using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
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
