using System.Threading.Tasks;
using PCLStorage;
using Wordreference.Core.Services.Abstract;

namespace Wordreference.Core.Services.Concrete
{
    public class FileStorageService : IFileStorageService
    {
        #region Properties

        private string _folder = "ModernWordreference";
        public string Folder { get { return _folder; } set { _folder = value; } }

        private string _formatFile = "json";
        public string FormatFile { get { return _formatFile; } set { _formatFile = value; } }


        #endregion


        #region Methods

        public async Task SaveAsync(string filename, string text)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
            IFile file = await folder.CreateFileAsync(string.Format("{0}.{1}", filename, FormatFile), CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(text);
        }

        public async Task<string> RestoreAsync(string filename)
        {
            if (await IsExistAsync(filename))
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.GetFolderAsync(Folder);
                IFile file = await folder.GetFileAsync(string.Format("{0}.{1}", filename, FormatFile));
                return await file.ReadAllTextAsync();
            }

            return null;
        }

        public async Task<bool> IsExistAsync(string filename)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            if (await rootFolder.CheckExistsAsync(Folder) != ExistenceCheckResult.FolderExists)
                return false;

            IFolder folder = await rootFolder.GetFolderAsync(Folder);
            return (await folder.CheckExistsAsync(string.Format("{0}.{1}", filename, FormatFile))) == ExistenceCheckResult.FileExists;
        }

        #endregion
    }
}
