using System.Linq;
using Newtonsoft.Json.Linq;
using Wordreference.API.Model;

namespace Wordreference.API.Helpers
{
    internal static class TranslationHelpers
    {
        internal static Translation JsonToTranslation(this JToken translationToken)
        {
            var translation = new Translation();

            var originalTermToken = translationToken.Children()["OriginalTerm"].FirstOrDefault();
            var firstTranslationToken = translationToken.Children()["FirstTranslation"].FirstOrDefault();
            var secondTranslationToken = translationToken.Children()["SecondTranslation"].FirstOrDefault();

            if (originalTermToken != null)
            {
                translation.TermeOriginal = new Term
                {
                    Nom = originalTermToken["term"].Value<string>(),
                    Type = originalTermToken["POS"].Value<string>(),
                    Sens = originalTermToken["sense"].Value<string>()
                };
            }
            if (firstTranslationToken != null)
            {
                translation.FirstTranslation = new Term
                {
                    Nom = firstTranslationToken["term"].Value<string>(),
                    Type = firstTranslationToken["POS"].Value<string>(),
                    Sens = firstTranslationToken["sense"].Value<string>()
                };
            }
            if (secondTranslationToken != null)
            {
                translation.SecondTranslation = new Term
                {
                    Nom = secondTranslationToken["term"].Value<string>(),
                    Type = secondTranslationToken["POS"].Value<string>(),
                    Sens = secondTranslationToken["sense"].Value<string>()
                };
            }
            return translation;
        }
    }
}
