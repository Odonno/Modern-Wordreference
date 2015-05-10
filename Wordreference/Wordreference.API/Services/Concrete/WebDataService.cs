using System;
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

                    var translationsRootTable = htmlDocument.DocumentNode.Descendants().FirstOrDefault(d =>
                        d.Name == "table" &&
                        d.Attributes["class"] != null &&
                        d.Attributes["class"].Value == "WRD");

                    if (translationsRootTable != null)
                    {
                        // Basic Grid search of data
                        RetrieveGridTanslations(htmlDocument, translationsRootTable);
                    }
                    else
                    {
                        // Retrieve Roman search of data
                        RetrieveRomanTranslations(htmlDocument, motRecherche);
                    }

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

        private void RetrieveGridTanslations(HtmlDocument htmlDocument, HtmlNode translationsRootTable)
        {
            bool nextTopSection = false;

            foreach (var translationNode in translationsRootTable.Descendants().Where(d => d.Name == "tr").Skip(1))
            {
                if (translationNode.Attributes["class"] != null && translationNode.Attributes["class"].Value == "wrtopsection")
                    nextTopSection = true;

                if (translationNode.Attributes["id"] == null)
                    continue;

                var translation = RetrieveGridTranslation(translationNode);

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

            if (formesComposeesRootTable != null)
                foreach (var translationNode in formesComposeesRootTable.Descendants().Where(d => d.Name == "tr").Skip(1))
                {
                    if (translationNode.Attributes["id"] == null)
                        continue;

                    Translations.FormesComposees.Add(RetrieveGridTranslation(translationNode));
                }
        }

        private void RetrieveRomanTranslations(HtmlDocument htmlDocument, string motRecherche)
        {
            foreach (var romanNode in htmlDocument.DocumentNode.Descendants()
                .Where(d => d.Name == "li" && d.Attributes["class"] != null && d.Attributes["class"].Value == "romanlist"))
            {
                foreach (var arabNode in romanNode.Descendants()
                    .Where(d => d.Name == "li" && d.Attributes["class"] != null && d.Attributes["class"].Value == "arablist"))
                {
                    string toTranslation = string.Empty;

                    foreach (var arabNodeChild in arabNode.Descendants()
                        .Where(d => d.Name == "span" && d.Attributes["class"] != null && !d.Attributes["class"].Value.Contains("headnumber")))
                    {
                        toTranslation += WebUtility.HtmlDecode(arabNodeChild.InnerText);
                    }

                    var translation = new Translation
                    {
                        TermeOriginal = new Term
                        {
                            Nom = motRecherche
                        },
                        FirstTranslation = new Term
                        {
                            Nom = toTranslation
                        }
                    };

                    Translations.TraductionsPrincipales.Add(translation);
                }
            }
        }


        private Translation RetrieveGridTranslation(HtmlNode translationNode)
        {
            var translation = new Translation();

            // Add the orginal term FROM
            var fromWord = translationNode.Descendants()
                .FirstOrDefault(d => d.Name == "td" && d.Attributes["class"] != null && d.Attributes["class"].Value == "FrWrd");

            if (fromWord != null)
            {
                translation.TermeOriginal = new Term();

                translation.TermeOriginal.Nom =
                    WebUtility.HtmlDecode(
                        fromWord.Descendants().FirstOrDefault(d => d.Name == "strong").InnerText);

                var typeNode = fromWord.Descendants().FirstOrDefault(d => d.Name == "em");
                if (typeNode != null && typeNode.FirstChild != null)
                    translation.TermeOriginal.Type = WebUtility.HtmlDecode(typeNode.FirstChild.InnerText);
            }

            // Add the translation TO
            var toWord = translationNode.Descendants()
                .FirstOrDefault(d => d.Name == "td" && d.Attributes["class"] != null && d.Attributes["class"].Value == "ToWrd");

            if (toWord != null)
            {
                translation.FirstTranslation = new Term();

                translation.FirstTranslation.Nom = WebUtility.HtmlDecode(toWord.FirstChild.InnerText);

                var typeNode = toWord.Descendants().FirstOrDefault(d => d.Name == "em");
                if (typeNode != null && typeNode.FirstChild != null)
                    translation.FirstTranslation.Type = WebUtility.HtmlDecode(typeNode.FirstChild.InnerText);
            }


            // Add the "Sens" property
            var middle = translationNode.Descendants()
                .FirstOrDefault(d => d.Name == "td" && d.Attributes["class"] == null);

            if (middle != null && middle.FirstChild != null)
            {
                translation.TermeOriginal.Sens = WebUtility.HtmlDecode(middle.FirstChild.InnerText);
                if (middle.ChildNodes.Count > 1)
                    translation.FirstTranslation.Sens = WebUtility.HtmlDecode(middle.ChildNodes[1].InnerText);
            }

            return translation;
        }

        #endregion
    }
}
