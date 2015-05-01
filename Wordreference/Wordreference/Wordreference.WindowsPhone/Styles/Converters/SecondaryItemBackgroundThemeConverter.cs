using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Wordreference.Styles.Converters
{
    public class SecondaryItemBackgroundThemeConverter : IValueConverter
    {
        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string theme = value as string;

            if (theme == _resourceLoader.GetString("dark"))
                return new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));

            return new SolidColorBrush(Color.FromArgb(255, 215, 215, 215));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
