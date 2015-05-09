using System.Threading.Tasks;
using Wordreference.API.Model;
using Wordreference.API.Services.Abstract;

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
