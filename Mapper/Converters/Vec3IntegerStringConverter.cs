using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
    /// <summary>
    /// Convert a Vec2 or Vec3 to a string of the form (x, y, z), where x, y and z are rounded to closest integer
    /// </summary>
    class Vec3IntegerStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;
            if (value is Models.Vec3 v3)
            {
                return $"({Math.Round(v3.X)}, {Math.Round(v3.Y)}, {Math.Round(v3.Z)}";
            }
            else if (value is Models.Vec2 v2)
            {
                return $"({Math.Round(v2.X)}, {Math.Round(v2.Y)}";
            }
            else return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
