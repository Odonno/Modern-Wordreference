using System.Collections.Generic;
using Wordreference.Model;

namespace Wordreference.Core.Factories.Abstract
{
    public interface ILanguageFactory
    {
        IEnumerable<Language> Languages { get; }

        Language GetLanguageByAbbreviation(string abbreviation);
    }
}
