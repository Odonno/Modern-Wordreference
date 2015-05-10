using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Wordreference.API.Services.Abstract;
using Wordreference.Model;

namespace Wordreference.API.Services.Concrete
{
    /// <summary>
    /// Retrieve Translations through Web page (HTML data)
    /// </summary>
    public class WebDataService : BaseDataService
    {
        #region Methods

        public override bool CanTranslate(Language languageDepart, Language languageArrive, string motRecherche)
        {
            return base.CanTranslate(languageDepart, languageArrive, motRecherche) &&
                   CanTranslate(languageDepart, languageArrive);
        }
        private bool CanTranslate(Language languageDepart, Language languageArrive)
        {
            // Exception 1 : French / Spanish translation
            if (languageDepart.Abbreviation == "fr" && languageArrive.Abbreviation == "es")
                return true;
            if (languageDepart.Abbreviation == "es" && languageArrive.Abbreviation == "fr")
                return true;

            // Exception 2 : Portuguese / Spanish translation
            if (languageDepart.Abbreviation == "pt" && languageArrive.Abbreviation == "es")
                return true;
            if (languageDepart.Abbreviation == "es" && languageArrive.Abbreviation == "pt")
                return true;

            return (languageDepart.Abbreviation == "en" || languageArrive.Abbreviation == "en");
        }

        public override async Task<bool?> LoadAsync(Language languageDepart, Language languageArrive, string motRecherche)
        {
            ClearData();

            string url = string.Format("http://www.wordreference.com/{0}{1}/{2}",
                languageDepart.Abbreviation,
                languageArrive.Abbreviation,
                motRecherche);

            try
            {
                using (var client = new HttpClient())
                {
                    string html = await client.GetStringAsync(url);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    var traductionsRootTable = htmlDocument.DocumentNode.Descendants().FirstOrDefault(d =>
                        d.Name == "table" &&
                        d.Attributes["class"] != null &&
                        d.Attributes["class"].Value == "WRD");

                    bool nextTopSection = false;

                    foreach (var traduction in traductionsRootTable.Descendants().Where(d => d.Name == "tr").Skip(1))
                    {
                        if (traduction.Attributes["class"] != null && traduction.Attributes["class"].Value == "wrtopsection")
                            nextTopSection = true;

                        if (traduction.Attributes["id"] == null)
                            continue;
                        
                        var translation = new Translation();

                        // Add the orginal term FROM
                        var fromWord = traduction.Descendants()
                            .FirstOrDefault(d => d.Name == "td" && d.Attributes["class"] != null && d.Attributes["class"].Value == "FrWrd");

                        translation.TermeOriginal = new Term
                        {
                            Nom = WebUtility.HtmlDecode(fromWord.Descendants().FirstOrDefault(d => d.Name == "strong").InnerText),
                            Type = WebUtility.HtmlDecode(fromWord.Descendants().FirstOrDefault(d => d.Name == "em").FirstChild.InnerText)
                        };

                        // Add the translation TO
                        var toWord = traduction.Descendants()
                            .FirstOrDefault(d => d.Name == "td" && d.Attributes["class"] != null && d.Attributes["class"].Value == "ToWrd");

                        translation.FirstTranslation = new Term
                        {
                            Nom = WebUtility.HtmlDecode(toWord.FirstChild.InnerText),
                            Type = WebUtility.HtmlDecode(toWord.Descendants().FirstOrDefault(d => d.Name == "em").FirstChild.InnerText)
                        };

                        // Add the "Sens" property
                        var middle = traduction.Descendants()
                            .FirstOrDefault(d => d.Name == "td" && d.Attributes["class"] == null);

                        translation.TermeOriginal.Sens = WebUtility.HtmlDecode(middle.FirstChild.InnerText);
                        if (middle.ChildNodes.Count > 1)
                            translation.FirstTranslation.Sens = WebUtility.HtmlDecode(middle.ChildNodes[1].InnerText);

                        if (nextTopSection)
                        {
                            // Retrieve TraductionsAdditionnelles
                            Translations.TraductionsAdditionnelles.Add(translation);
                        }
                        else
                        {
                            // Retrieve TraductionsPrincipales
                            Translations.TraductionsPrincipales.Add(translation);
                        }
                    }
                    
                    // Retrieve FormesComposees
                    var formesComposeesRootTable = htmlDocument.DocumentNode.Descendants().FirstOrDefault(d =>
                        d.Name == "table" &&
                        d.Attributes["class"] != null &&
                        d.Attributes["class"].Value == "WRD" &&
                        d.Attributes["id"] != null &&
                        d.Attributes["id"].Value == "compound_forms");

                    //Translations.FormesComposees.Add();

                    LanguageDepart = languageDepart;
                    LanguageArrive = languageArrive;
                    MotRecherche = motRecherche;

                    return true;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
