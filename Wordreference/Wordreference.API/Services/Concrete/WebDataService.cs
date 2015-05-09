using System.Threading.Tasks;
using Wordreference.API.Services.Abstract;
using Wordreference.Model;

namespace Wordreference.API.Services.Concrete
{
    /// <summary>
    /// Retrieve Translations through Web page (HTML data)
    /// </summary>
    public class WebDataService : BaseDataService
    {
        #region Methods

        public override bool CanTranslate(Language languageDepart, Language languageArrive, string motRecherche)
        {
            return base.CanTranslate(languageDepart, languageArrive, motRecherche) &&
                   CanTranslate(languageDepart, languageArrive);
        }
        private bool CanTranslate(Language languageDepart, Language languageArrive)
        {
            // Exception 1 : French / Spanish translation
            if (languageDepart.Abbreviation == "fr" && languageArrive.Abbreviation == "es")
                return true;
            if (languageDepart.Abbreviation == "es" && languageArrive.Abbreviation == "fr")
                return true;

            // Exception 2 : Portuguese / Spanish translation
            if (languageDepart.Abbreviation == "pt" && languageArrive.Abbreviation == "es")
                return true;
            if (languageDepart.Abbreviation == "es" && languageArrive.Abbreviation == "pt")
                return true;

            return (languageDepart.Abbreviation == "en" || languageArrive.Abbreviation == "en");
        }

        public override Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
