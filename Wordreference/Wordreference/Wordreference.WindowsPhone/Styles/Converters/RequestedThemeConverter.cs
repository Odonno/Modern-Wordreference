using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace Wordreference.Styles.Converters
{
    public class RequestedThemeConverter : IValueConverter
    {
        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string theme = value as string;
            bool isInverse = (parameter as string) == "inverse";
            bool isDark = theme == _resourceLoader.GetString("dark");

            if (isInverse)
                isDark = !isDark;

            if (isDark)
                return "Dark";

            return "Light";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string theme = value as string;
            bool isInverse = (parameter as string) == "inverse";
            bool isDark = theme == "Dark";

            if (isInverse)
                isDark = !isDark;

            if (isDark)
                return _resourceLoader.GetString("dark");

            return _resourceLoader.GetString("light");
        }
    }
}
