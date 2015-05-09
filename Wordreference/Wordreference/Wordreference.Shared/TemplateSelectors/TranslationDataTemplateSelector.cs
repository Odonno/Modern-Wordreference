using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Wordreference.TemplateSelectors
{
    public class TranslationDataTemplateSelector : DataTemplateSelector
    {
        private int _alternativeNumber = 1;


        public DataTemplate TranslationTemplate { get; set; }
        public DataTemplate AlternativeTranslationTemplate { get; set; }


        protected override DataTemplate SelectTemplateCore(object item)
        {
            return (_alternativeNumber++ % 2 == 0) ? TranslationTemplate : AlternativeTranslationTemplate;
        }
    }
}
