using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Wordreference.Styles.Converters
{
    public class ItemBackgroundThemeConverter : IValueConverter
    {
        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string theme = value as string;

            if (theme == _resourceLoader.GetString("dark"))
                return new SolidColorBrush(Colors.Black);

            return new SolidColorBrush(Color.FromArgb(255, 235, 235, 242));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
