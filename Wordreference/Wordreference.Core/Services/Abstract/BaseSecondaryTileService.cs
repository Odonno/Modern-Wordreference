using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Wordreference.Core.Services.Abstract
{
    public abstract class BaseSecondaryTileService<T> : ISecondaryTileService<T> where T : class
    {
        public abstract string TileId(T entity);
        public bool IsSecondaryTileExist(T entity)
        {
            return SecondaryTile.Exists(TileId(entity));
        }
        public abstract Task<bool> CreateSecondaryTile(T entity);
        public async Task<bool> RemoveSecondaryTile(T entity)
        {
            try
            {
                var tileToBeDeleted = new SecondaryTile(TileId(entity));
                await tileToBeDeleted.RequestDeleteAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
