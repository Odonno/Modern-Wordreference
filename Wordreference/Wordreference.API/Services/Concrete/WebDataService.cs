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

        public override Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
