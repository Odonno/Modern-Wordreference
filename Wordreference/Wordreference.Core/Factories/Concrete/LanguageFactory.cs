using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Wordreference.Core.Factories.Abstract;
using Wordreference.Model;

namespace Wordreference.Core.Factories.Concrete
{
    internal class LanguageFactory : ILanguageFactory
    {
        #region Properties

        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("LanguageResources");

        public IEnumerable<Language> Languages { get; private set; }

        #endregion


        #region Constructor

        public LanguageFactory()
        {
            Languages = new List<Language>
                {
                    new Language {Nom = _resourceLoader.GetString("arabic"),       Abbreviation = "ar",    Image = "arabic_language.png"},
                    new Language {Nom = _resourceLoader.GetString("chinese"),      Abbreviation = "zh",    Image = "china.png"},
                    new Language {Nom = _resourceLoader.GetString("czech"),        Abbreviation = "cz",    Image = "czech-republic.png"},
                    new Language {Nom = _resourceLoader.GetString("english"),      Abbreviation = "en",    Image = "United_Kingdom.png"},
                    new Language {Nom = _resourceLoader.GetString("french"),       Abbreviation = "fr",    Image = "French.png"},
                    new Language {Nom = _resourceLoader.GetString("german"),       Abbreviation = "de",    Image = "Germany.png"},
                    new Language {Nom = _resourceLoader.GetString("greek"),        Abbreviation = "gr",    Image = "Greek.png"},
                    new Language {Nom = _resourceLoader.GetString("italian"),      Abbreviation = "it",    Image = "Italy.png"},
                    new Language {Nom = _resourceLoader.GetString("japanese"),     Abbreviation = "ja",    Image = "japan.png"},
                    new Language {Nom = _resourceLoader.GetString("korean"),       Abbreviation = "ko",    Image = "south_korea.png"},
                    new Language {Nom = _resourceLoader.GetString("polish"),       Abbreviation = "pl",    Image = "Poland.png"},
                    new Language {Nom = _resourceLoader.GetString("portuguese"),   Abbreviation = "pt",    Image = "Portugal.png"},
                    new Language {Nom = _resourceLoader.GetString("romanian"),     Abbreviation = "ro",    Image = "Romania.png"},
                    new Language {Nom = _resourceLoader.GetString("russian"),      Abbreviation = "ru",    Image = "Russia.png"},
                    new Language {Nom = _resourceLoader.GetString("spanish"),      Abbreviation = "es",    Image = "Spain.png"},
                    new Language {Nom = _resourceLoader.GetString("swedish"),      Abbreviation = "sv",    Image = "Sweden.png"},
                    new Language {Nom = _resourceLoader.GetString("turkish"),      Abbreviation = "tr",    Image = "turkey.png"},
                };
        }

        #endregion


        #region Methods

        public Language GetLanguageByAbbreviation(string abbreviation)
        {
            return Languages.FirstOrDefault(l => l.Abbreviation == abbreviation);
        }

        #endregion
    }
}
