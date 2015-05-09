using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Wordreference.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string))
                return Visibility.Collapsed;
            return (string.IsNullOrWhiteSpace(value as string)) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
