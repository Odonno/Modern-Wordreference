using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Wordreference.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is String))
                return Visibility.Collapsed;
            return (String.IsNullOrWhiteSpace(value as String)) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
