using System.Threading.Tasks;
using Wordreference.Model;

namespace Wordreference.API.Services.Abstract
{
    public abstract class BaseDataService : IDataService
    {
        #region Properties

        public Translations Translations { get; } = new Translations();
        public Language LanguageDepart { get; protected set; }
        public Language LanguageArrive { get; protected set; }
        public string MotRecherche { get; protected set; }

        #endregion


        #region Methods

        public void ClearData()
        {
            Translations.TraductionsPrincipales.Clear();
            Translations.TraductionsAdditionnelles.Clear();
            Translations.FormesComposees.Clear();
        }

        public virtual bool CanTranslate(Language languageDepart, Language languageArrive, string motRecherche)
        {
            return !string.IsNullOrWhiteSpace(motRecherche) &&
                   languageDepart != null &&
                   languageArrive != null &&
                   languageDepart != languageArrive;
        }

        public abstract Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche);

        #endregion
    }
}
