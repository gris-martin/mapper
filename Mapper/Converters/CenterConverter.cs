using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mapper.Converters
{
    /// <summary>
    /// Used to convert a position from top or left to center.
    /// Inputs are 2 doubles corresponding to the position and the actual width or height of the object
    /// Example:
    ///
    /// <ItemsControl.ItemContainerStyle>
    ///     <Style>
    ///         <Setter Property = "Canvas.Left" >
    ///             <Setter.Value>
    ///                 <MultiBinding Converter="{StaticResource CenterConverter}">
    ///                     <Binding Path = "Model.ViewPos.X"/>
    ///                     <Binding RelativeSource="{RelativeSource Self}"  Path="ActualWidth"/>
    ///                 </MultiBinding>
    ///             </Setter.Value>
    ///         </Setter>
    ///     </Style>
    /// </ItemsControl.ItemContainerStyle>
    ///
    /// Inspired by https://stackoverflow.com/a/9779799/6685703
    /// </summary>
    public class CenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            double center = (double)values[0];  // Desired center position
            double dimension = (double)values[1];  // Width or height

            return center - dimension / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
