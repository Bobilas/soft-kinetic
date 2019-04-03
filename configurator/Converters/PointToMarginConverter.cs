using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Configurator
{
    [ValueConversion(typeof(Point), typeof(Thickness))]
    public class PointToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pos = (Point)value;
            return new Thickness(pos.X, pos.Y, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
