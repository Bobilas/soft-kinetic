using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Configurator
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class ContainerToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)value[0];
            var items = value[1] as ObservableCollection<ContainerInfo>;

            return !string.IsNullOrEmpty(name)
                && !items.Select(e => e.Name).Contains(name);
        }
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
