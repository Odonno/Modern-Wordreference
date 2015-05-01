using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Wordreference.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return ((bool) value) ? Visibility.Visible : Visibility.Collapsed;

            if (value is Int32)
                return ((Int32)value == 0) ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
