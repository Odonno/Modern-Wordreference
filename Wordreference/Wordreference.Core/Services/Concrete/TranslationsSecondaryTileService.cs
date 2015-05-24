using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using NotificationsExtensions.TileContent;
using Wordreference.Core.Services.Abstract;
using Wordreference.Core.ViewModel.Abstract;

namespace Wordreference.Core.Services.Concrete
{
    public class TranslationsSecondaryTileService : BaseSecondaryTileService<ITranslationViewModel>
    {
        #region Properties

        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private readonly IStorageService _storageService;

        #endregion


        #region Constructor

        public TranslationsSecondaryTileService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        #endregion


        #region Methods

        public override string TileId(ITranslationViewModel entity)
        {
            return string.Format("{0}_{1}_{2}", entity.DataService.MotRecherche.Trim(), entity.DataService.LanguageDepart.Abbreviation, entity.DataService.LanguageArrive.Abbreviation);
        }

        public override async Task<bool> CreateSecondaryTile(ITranslationViewModel entity)
        {
            var tileId = TileId(entity);

            // save data of the future tile
            _storageService.FileStorageService.Folder = tileId;

            var abbreviationLanguageDepart = (entity.LanguageDepart == null) ? null : entity.LanguageDepart.Abbreviation;
            var abbreviationLanguageArrive = (entity.LanguageArrive == null) ? null : entity.LanguageArrive.Abbreviation;
            var word = entity.MotRecherche;
            var translations = entity.CurrentTranslations;

            _storageService.UpdateData(abbreviationLanguageDepart, abbreviationLanguageArrive, word, translations);

            _storageService.Save();

            // create secondary tile
            var secondaryTile = new SecondaryTile
            {
                TileId = tileId,
                Arguments = tileId,
                BackgroundColor = Color.FromArgb(255, 94, 88, 199),
                ForegroundText = ForegroundText.Light,
                ShortName = entity.DataService.MotRecherche.Trim(),
                DisplayName = entity.DataService.MotRecherche.Trim(),
                TileOptions = TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo
            };

#if WINDOWS_APP
            secondaryTile.SmallLogo = new Uri("ms-appx:///Assets/SmallLogo.scale-100.png");
            secondaryTile.Logo = new Uri("ms-appx:///Assets/Logo.scale-100.png");
            secondaryTile.WideLogo = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-100.png");
#elif WINDOWS_PHONE_APP
            secondaryTile.SmallLogo = new Uri("ms-appx:///Assets/SmallLogo.wp.png");
            secondaryTile.Logo = new Uri("ms-appx:///Assets/Logo.wp.png");
            secondaryTile.WideLogo = new Uri("ms-appx:///Assets/Wide310x150Logo.wp.png");
#endif

            await secondaryTile.RequestCreateAsync();

            try
            {
                // creating the Tile Updater
                TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(secondaryTile.TileId);
                tileUpdater.EnableNotificationQueue(true);

                {
                    // creating the basic info tile (word, languages)
                    var tileSquare = TileContentFactory.CreateTileSquare150x150Text01();
                    var tileWide = TileContentFactory.CreateTileWide310x150Text03();

                    tileSquare.TextHeading.Text = entity.DataService.MotRecherche.Trim();

                    tileSquare.TextBody1.Text = string.Format("{0} {1}",
                        _resourceLoader.GetString("from"),
                        entity.DataService.LanguageDepart.Nom);
                    tileSquare.TextBody2.Text = string.Format("{0} {1}",
                        _resourceLoader.GetString("to"),
                        entity.DataService.LanguageArrive.Nom);

                    tileWide.TextHeadingWrap.Text = string.Format("{0} {1} {2} {3}",
                        _resourceLoader.GetString("from"),
                        entity.DataService.LanguageDepart.Nom,
                        _resourceLoader.GetString("to"),
                        entity.DataService.LanguageArrive.Nom);

                    tileWide.Square150x150Content = tileSquare;

                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tileWide.GetXml().ToString());

                    TileNotification tileNotification = new TileNotification(xmlDocument);
                    tileUpdater.Update(tileNotification);
                }

                {
                    // creating the 1st stat tile (number of principal translations)
                    var tileSquare = TileContentFactory.CreateTileSquare150x150Text01();
                    var tileWide = TileContentFactory.CreateTileWide310x150BlockAndText01();

                    tileSquare.TextHeading.Text = entity.DataService.MotRecherche.Trim();

                    tileSquare.TextBody1.Text = tileWide.TextBlock.Text = entity.DataService.Translations.TraductionsPrincipales.Count.ToString();
                    tileSquare.TextBody2.Text = _resourceLoader.GetString("principal");
                    tileWide.TextSubBlock.Text = _resourceLoader.GetString("principalTranslations");

                    tileWide.Square150x150Content = tileSquare;

                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tileWide.GetXml().ToString());

                    TileNotification tileNotification = new TileNotification(xmlDocument);
                    tileUpdater.Update(tileNotification);
                }

                {
                    // creating the 2nd stat tile (number of addtionnal translations)
                    var tileSquare = TileContentFactory.CreateTileSquare150x150Text01();
                    var tileWide = TileContentFactory.CreateTileWide310x150BlockAndText01();

                    tileSquare.TextHeading.Text = entity.DataService.MotRecherche.Trim();

                    tileSquare.TextBody1.Text = tileWide.TextBlock.Text = entity.DataService.Translations.TraductionsAdditionnelles.Count.ToString();
                    tileSquare.TextBody2.Text = _resourceLoader.GetString("additionnal");
                    tileWide.TextSubBlock.Text = _resourceLoader.GetString("additionnalTranslations");

                    tileWide.Square150x150Content = tileSquare;

                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tileWide.GetXml().ToString());

                    TileNotification tileNotification = new TileNotification(xmlDocument);
                    tileUpdater.Update(tileNotification);
                }

                {
                    // creating the 3rd stat tile (number of compound forms)
                    var tileSquare = TileContentFactory.CreateTileSquare150x150Text01();
                    var tileWide = TileContentFactory.CreateTileWide310x150BlockAndText01();

                    tileSquare.TextHeading.Text = entity.DataService.MotRecherche.Trim();

                    tileSquare.TextBody1.Text = tileWide.TextBlock.Text = entity.DataService.Translations.FormesComposees.Count.ToString();
                    tileSquare.TextBody2.Text = _resourceLoader.GetString("compound");
                    tileWide.TextSubBlock.Text = _resourceLoader.GetString("compoundForms");

                    tileWide.Square150x150Content = tileSquare;

                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(tileWide.GetXml().ToString());

                    TileNotification tileNotification = new TileNotification(xmlDocument);
                    tileUpdater.Update(tileNotification);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
