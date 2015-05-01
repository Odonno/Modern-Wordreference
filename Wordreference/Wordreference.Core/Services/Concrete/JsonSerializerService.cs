using System;
using Newtonsoft.Json;
using Wordreference.Core.Services.Abstract;

namespace Wordreference.Core.Services.Concrete
{
    internal sealed class JsonSerializerService<T> : ISerializerService<T>
    {
        #region Methods

        public string Serialize(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public T Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        #endregion
    }
}
