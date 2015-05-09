using Wordreference.Model;

namespace Wordreference.Core.Services.Concrete
{
    public class SaveData
    {
        public string AbbreviationLanguageDepart { get; set; }
        public string AbbreviationLanguageArrive { get; set; }
        public string Word { get; set; }
        public Translations Translations { get; set; }
    }
}
