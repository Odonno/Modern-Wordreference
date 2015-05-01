using System.Threading.Tasks;
using Wordreference.API.Model;
using Wordreference.Core.Services.Concrete;

namespace Wordreference.Core.Services.Abstract
{
    public interface IStorageService
    {
        #region Properties

        SaveData Data { get; }
        bool IsRestoring { get; set; }

        IFileStorageService FileStorageService { get; }
        ISerializerService<SaveData> SaveDataSerializerService { get; }

        #endregion

        #region Methods

        void Save();
        Task RestoreAsync();
        void UpdateData(string abbreviationLanguageDepart, string abbreviationLanguageArrive, string word, Translations translations);

        #endregion
    }
}
