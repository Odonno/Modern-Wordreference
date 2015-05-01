using System;
using System.Collections.Generic;
using System.Linq;
using Wordreference.Core.DataModel.Abstract;

namespace Wordreference.Core.DataModel.Concrete
{
    public class AlphaKeyGroup : List<object>
    {
        /// <summary>
        /// The delegate that is used to get the key information.
        /// </summary>
        /// <param name="item">An object</param>
        /// <returns>The key value to use for this object</returns>
        public delegate string GetKeyDelegate(ITranslationDataModel item);

        /// <summary>
        /// The Key of this group.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        public AlphaKeyGroup(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Create a list of AlphaGroup with keys
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<AlphaKeyGroup> CreateGroups(IEnumerable<ITranslationDataModel> items, GetKeyDelegate getKey)
        {
            // list of objects
            List<AlphaKeyGroup> list = (from i in items
                                        group i by i.GroupName into g
                                        select new AlphaKeyGroup(g.Key)).ToList();

            foreach (var item in items)
            {
                string index = getKey(item);
                if (!string.IsNullOrEmpty(index))
                    list.Find(a => a.Key == index).Add(item);
            }

            return list;
        }
    }
}
