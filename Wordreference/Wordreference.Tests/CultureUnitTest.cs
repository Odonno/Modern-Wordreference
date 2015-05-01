using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordreference.DAL.Factories.Concrete;
using Wordreference.DAL.Model;

namespace Wordreference.Tests
{
    [TestClass]
    public class CultureUnitTest
    {
        [TestMethod]
        public void Can_Get_Current_Culture()
        {
            // arrange
            var currentCulture = CultureInfo.CurrentCulture;

            // act
            string abbreviation = currentCulture.Name.Substring(0, 2).ToLower();

            // assert
            Assert.AreEqual(currentCulture.Name, "fr-FR");
            Assert.AreEqual(abbreviation, "fr");
        }

        [TestMethod]
        public void Can_Get_Language_From_Current_Culture()
        {
            // arrange
            var currentCulture = CultureInfo.CurrentCulture;
            string abbreviation = currentCulture.Name.Substring(0, 2).ToLower();
            LanguageFactory languageFactory = new LanguageFactory();

            // act
            Language languageWithDefaultValue = languageFactory.GetLanguageByAbbreviation(abbreviation) ?? languageFactory.GetLanguageByAbbreviation("en");
            Language language = languageFactory.GetLanguageByAbbreviation(abbreviation);

            // assert
            Assert.IsNotNull(languageWithDefaultValue);
            Assert.IsNotNull(language);
            Assert.AreEqual(languageWithDefaultValue.Abbreviation, "fr");
            Assert.AreEqual(language.Abbreviation, "fr");
            Assert.AreNotEqual(languageWithDefaultValue.Abbreviation, "en");
        }
    }
}
