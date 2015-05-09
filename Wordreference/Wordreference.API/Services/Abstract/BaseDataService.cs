using System.Threading.Tasks;
using Wordreference.Model;

namespace Wordreference.API.Services.Abstract
{
    public abstract class BaseDataService : IDataService
    {
        #region Properties

        private readonly Translations _translations = new Translations();
        public Translations Translations { get { return _translations; } }

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

        public abstract Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche);

        #endregion
    }
}
