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
