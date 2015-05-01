using System.Threading.Tasks;
using Wordreference.API.Model;
using Wordreference.Core.Services.Abstract;

namespace Wordreference.Core.Services.Concrete
{
    internal class LocalStorageService : IStorageService
    {
        #region Properties

        public SaveData Data { get; private set; }
        public bool IsRestoring { get; set; }

        public IFileStorageService FileStorageService { get; private set; }
        public ISerializerService<SaveData> SaveDataSerializerService { get; private set; }

        #endregion


        #region Constructor

        public LocalStorageService(IFileStorageService fileStorageService, ISerializerService<SaveData> saveDataSerializerService)
        {
            Data = new SaveData();
            FileStorageService = fileStorageService;
            SaveDataSerializerService = saveDataSerializerService;
        }

        #endregion


        #region Methods

        public void Save()
        {
            FileStorageService.SaveAsync("saveData", SaveDataSerializerService.Serialize(Data));
        }

        public async Task RestoreAsync()
        {
            var data = SaveDataSerializerService.Deserialize(await FileStorageService.RestoreAsync("saveData"));

            if (data != null)
                Data = new SaveData
                {
                    AbbreviationLanguageDepart = data.AbbreviationLanguageDepart,
                    AbbreviationLanguageArrive = data.AbbreviationLanguageArrive,
                    Word = data.Word,
                    Translations = data.Translations
                };
        }

        public void UpdateData(string abbreviationLanguageDepart, string abbreviationLanguageArrive, string word, Translations translations)
        {
            Data = new SaveData
            {
                AbbreviationLanguageDepart = abbreviationLanguageDepart,
                AbbreviationLanguageArrive = abbreviationLanguageArrive,
                Word = word,
                Translations = translations
            };
        }

        #endregion
    }
}
