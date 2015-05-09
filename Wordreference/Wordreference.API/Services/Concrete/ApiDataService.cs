using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wordreference.API.Services.Abstract;
using Wordreference.Model;

namespace Wordreference.API.Services.Concrete
{
    /// <summary>
    /// Retrieve Translations through API Service (JSON lightweight data)
    /// </summary>
    public class ApiDataService : BaseDataService
    {
        #region Fields

        private const string ApiKey = "8ef69";
        private const string ApiVersion = "0.8";

        #endregion


        #region Methods

        public override bool CanTranslate(Language languageDepart, Language languageArrive, string motRecherche)
        {
            return base.CanTranslate(languageDepart, languageArrive, motRecherche) &&
                   (languageDepart.Abbreviation == "en" || languageArrive.Abbreviation == "en") &&
                   !RequireLanguage(languageDepart, "de", "sv", "ru") &&
                   !RequireLanguage(languageArrive, "de", "sv", "ru"); // Not supporting German / Swedish / Russian
        }
        private bool RequireLanguage(Language language, params string[] abbreviations)
        {
            return abbreviations.Any(abbr => language.Abbreviation == abbr);
        }

        public override async Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche)
        {
            ClearData();

            try
            {
                var client = new HttpClient();
                var apiLink = new Uri(string.Format("http://api.wordreference.com/{0}/{1}/json/{2}{3}/{4}", ApiVersion, ApiKey, languageDepart.Abbreviation, languageArrive.Abbreviation, motRecherche));
                var formattedJson = await client.GetStringAsync(apiLink);
                JObject value = JObject.Parse(formattedJson);

                var errorValue = value.GetValue("Error");

                if (errorValue != null && errorValue.ToString() == "NoTranslation")
                    return false;

                if (value["term0"] != null)
                {
                    if (value["term0"]["PrincipalTranslations"] != null)
                        foreach (var translationToken in value["term0"]["PrincipalTranslations"])
                            Translations.TraductionsPrincipales.Add(JsonToTranslation(translationToken));

                    if (value["term0"]["AdditionalTranslations"] != null)
                        foreach (var translationToken in value["term0"]["AdditionalTranslations"])
                            Translations.TraductionsAdditionnelles.Add(JsonToTranslation(translationToken));
                }

                if (value["original"] != null && value["original"]["Compounds"] != null)
                    foreach (var translationToken in value["original"]["Compounds"])
                        Translations.FormesComposees.Add(JsonToTranslation(translationToken));

                LanguageDepart = languageDepart;
                LanguageArrive = languageArrive;
                MotRecherche = motRecherche;

                return true;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion


        #region Helpers

        private static Translation JsonToTranslation(JToken translationToken)
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

        #endregion
    }
}
