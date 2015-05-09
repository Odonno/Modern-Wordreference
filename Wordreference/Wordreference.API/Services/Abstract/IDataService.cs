using System.Threading.Tasks;
using Wordreference.Model;

namespace Wordreference.API.Services.Abstract
{
    public interface IDataService
    {
        #region Properties

        Language LanguageDepart { get; }
        Language LanguageArrive { get; }
        string MotRecherche { get; }
        Translations Translations { get; }

        #endregion


        #region Methods

        void ClearData();
        bool CanTranslate(Language languageDepart, Language languageArrive, string motRecherche);
        Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche);

        #endregion
    }
}
