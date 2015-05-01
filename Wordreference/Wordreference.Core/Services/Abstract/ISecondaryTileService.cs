using System.Threading.Tasks;

namespace Wordreference.Core.Services.Abstract
{
    public interface ISecondaryTileService<in T> where T : class
    {
        string TileId(T entity);
        bool IsSecondaryTileExist(T entity);
        Task<bool> CreateSecondaryTile(T entity);
        Task<bool> RemoveSecondaryTile(T entity);
    }
}
