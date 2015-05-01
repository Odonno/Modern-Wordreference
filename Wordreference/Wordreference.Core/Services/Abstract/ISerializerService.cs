namespace Wordreference.Core.Services.Abstract
{
    public interface ISerializerService<T>
    {
        #region Methods

        string Serialize(T item);
        T Deserialize(string json);

        #endregion
    }
}
