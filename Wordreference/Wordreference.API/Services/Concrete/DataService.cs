using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wordreference.API.Helpers;
using Wordreference.API.Model;
using Wordreference.API.Services.Abstract;

namespace Wordreference.API.Services.Concrete
{
    public class DataService : IDataService
    {
        #region Properties

        private Translations _translations;
        public Translations Translations { get { return _translations ?? (_translations = new Translations()); } }

        private Language _languageDepart;
        public Language LanguageDepart { get { return _languageDepart; } private set { _languageDepart = value; } }

        private Language _languageArrive;
        public Language LanguageArrive { get { return _languageArrive; } private set { _languageArrive = value; } }

        private string _motRecherche;
        public string MotRecherche { get { return _motRecherche; } private set { _motRecherche = value; } }

        #endregion


        #region Methods

        public bool CanLoad(Language languageDepart, Language languageArrive, string motRecherche)
        {
            throw new NotImplementedException();
        }

        public void ClearData()
        {
            Translations.TraductionsPrincipales.Clear();
            Translations.TraductionsAdditionnelles.Clear();
            Translations.FormesComposees.Clear();
        }

        public async Task Load(Language languageDepart, Language languageArrive, string motRecherche)
        {
            ClearData();

            try
            {
                var client = new HttpClient();
                var apiLink = new Uri(string.Format("http://api.wordreference.com/0.8/8ef69/json/{0}{1}/{2}", languageDepart.Abbreviation, languageArrive.Abbreviation, motRecherche));
                var formattedJson = await client.GetStringAsync(apiLink);
                JObject value = JObject.Parse(formattedJson);

                if (value["term0"] != null)
                {
                    if (value["term0"]["PrincipalTranslations"] != null)
                        foreach (var translationToken in value["term0"]["PrincipalTranslations"])
                            Translations.TraductionsPrincipales.Add(translationToken.JsonToTranslation());

                    if (value["term0"]["AdditionalTranslations"] != null)
                        foreach (var translationToken in value["term0"]["AdditionalTranslations"])
                            Translations.TraductionsAdditionnelles.Add(translationToken.JsonToTranslation());
                }

                if (value["original"] != null && value["original"]["Compounds"] != null)
                    foreach (var translationToken in value["original"]["Compounds"])
                        Translations.FormesComposees.Add(translationToken.JsonToTranslation());

                LanguageDepart = languageDepart;
                LanguageArrive = languageArrive;
                MotRecherche = motRecherche;
            }
            catch (Exception ex)
            {
                throw new Exception("Accès impossible à l'API", ex);
            }
        }

        #endregion
    }
}
