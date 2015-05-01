using System.Threading.Tasks;

namespace Wordreference.Core.Services.Abstract
{
    public interface IFileStorageService
    {
        #region Properties

        string Folder { get; set; }
        string FormatFile { get; set; }

        #endregion


        #region Methods

        Task SaveAsync(string filename, string text);
        Task<string> RestoreAsync(string filename);
        Task<bool> IsExistAsync(string filename);

        #endregion
    }
}
