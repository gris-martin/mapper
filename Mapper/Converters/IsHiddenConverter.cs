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
    /// Convert from a bool which is true when the object should be hidden and false otherwise, to
    /// a Visibility object.
    /// </summary>
    public class IsHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // TODO: Error handling when value is of wrong type.
            var isHidden = (bool)value;
            return isHidden ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO: Error handling when value is of wrong type, or unsupported typ (Collapsed)
            var visibility = (Visibility)value;
            return visibility != Visibility.Visible;
        }
    }
}
