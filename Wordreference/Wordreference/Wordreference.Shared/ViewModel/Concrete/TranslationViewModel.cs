﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel.Resources;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wordreference.API.Services.Abstract;
using Wordreference.Core.DataModel.Concrete;
using Wordreference.Core.Factories.Abstract;
using Wordreference.Core.Services.Abstract;
using Wordreference.Core.ViewModel.Abstract;
using Wordreference.Model;

namespace Wordreference.Core.ViewModel.Concrete
{
    public class TranslationViewModel : ViewModelBase, ITranslationViewModel
    {
        #region Fields

        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private readonly ILocalNotificationService _localNotificationService;
        private readonly ITelemetryService _telemetryService;

        #endregion


        #region Services

        public IStorageService StorageService { get; private set; }
        public IDataService DataService { get; private set; }
        public ILanguageFactory LanguageFactory { get; private set; }

        #endregion


        #region Properties

        private Language _languageDepart;
        public Language LanguageDepart
        {
            get
            {
                return _languageDepart;
            }
            set
            {
                _languageDepart = value;
                RaisePropertyChanged();
                TranslateCommand.RaiseCanExecuteChanged();
                SwitchCommand.RaiseCanExecuteChanged();
            }
        }

        private Language _languageArrive;
        public Language LanguageArrive
        {
            get
            {
                return _languageArrive;
            }
            set
            {
                _languageArrive = value;
                RaisePropertyChanged();
                TranslateCommand.RaiseCanExecuteChanged();
                SwitchCommand.RaiseCanExecuteChanged();
            }
        }

        private string _motRecherche;
        public string MotRecherche
        {
            get
            {
                return _motRecherche;
            }
            set
            {
                _motRecherche = value;
                RaisePropertyChanged();
                TranslateCommand.RaiseCanExecuteChanged();
                SwitchCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isTranslating;
        public bool IsTranslating
        {
            get
            {
                return _isTranslating;
            }
            private set
            {
                _isTranslating = value;
                RaisePropertyChanged();
                TranslateCommand.RaiseCanExecuteChanged();
            }
        }

        private readonly Translations _currentTranslations = new Translations();
        public Translations CurrentTranslations { get { return _currentTranslations; } }

        public List<AlphaKeyGroup> TranslatedKeyGroup
        {
            get
            {
                var traductionsPrincipales = (from t in (CurrentTranslations.TraductionsPrincipales)
                                              select new TranslationDataModel(t) { GroupName = _resourceLoader.GetString("principalTranslations") }).ToList();
                var traductionsAdditionnelles = (from t in (CurrentTranslations.TraductionsAdditionnelles)
                                                 select new TranslationDataModel(t) { GroupName = _resourceLoader.GetString("additionnalTranslations") }).ToList();
                var formesComposees = (from t in (CurrentTranslations.FormesComposees)
                                       select new TranslationDataModel(t) { GroupName = _resourceLoader.GetString("compoundForms") }).ToList();

                var translations = traductionsPrincipales.Concat(traductionsAdditionnelles).Concat(formesComposees).ToList();

                return AlphaKeyGroup.CreateGroups(translations, t => t.GroupName);
            }
        }

        #endregion


        #region Commands

        public RelayCommand TranslateCommand { get; private set; }
        public RelayCommand<bool> TranslateWithParamCommand { get; private set; }
        public RelayCommand<string> WordUpdatedCommand { get; private set; }
        public RelayCommand SwitchCommand { get; private set; }
        public RelayCommand GoToTwitterCommand { get; private set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TranslationViewModel class.
        /// </summary>
        public TranslationViewModel(IDataService dataService,
            ILanguageFactory languageFactory,
            IStorageService storageService,
            ILocalNotificationService localNotificationService,
            ITelemetryService telemetryService)
        {
            // Injection of services
            DataService = dataService;
            LanguageFactory = languageFactory;
            StorageService = storageService;
            _localNotificationService = localNotificationService;
            _telemetryService = telemetryService;

            // Commands
            TranslateCommand = new RelayCommand(Translate, CanTranslate);
            TranslateWithParamCommand = new RelayCommand<bool>(TranslateWithParam);
            WordUpdatedCommand = new RelayCommand<string>(WordUpdated);
            SwitchCommand = new RelayCommand(Switch, CanSwitch);


            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.

                // by default english to french
                LanguageDepart = LanguageFactory.GetLanguageByAbbreviation("en");
                LanguageArrive = LanguageFactory.GetLanguageByAbbreviation("fr");
                MotRecherche = "yellow";

                Translate();
            }
            else
            {
                // Code runs "for real"

            }
        }

        #endregion


        #region Methods

        public void Save()
        {
            if (StorageService != null && !StorageService.IsRestoring)
            {
                // save languages
                var abbreviationLanguageDepart = (LanguageDepart == null) ? null : LanguageDepart.Abbreviation;
                var abbreviationLanguageArrive = (LanguageArrive == null) ? null : LanguageArrive.Abbreviation;
                var word = MotRecherche;
                var translations = CurrentTranslations;

                StorageService.UpdateData(abbreviationLanguageDepart, abbreviationLanguageArrive, word, translations);

                StorageService.Save();
            }
        }

        public async void Restore(string secondaryTile)
        {
            // restore from Secondary Tile
            if (!string.IsNullOrWhiteSpace(secondaryTile))
                StorageService.FileStorageService.Folder = secondaryTile;

            // or restore from current storage (last instance)
            StorageService.IsRestoring = true;
            await StorageService.RestoreAsync();

            if (StorageService.Data.AbbreviationLanguageDepart != null && StorageService.Data.AbbreviationLanguageArrive != null)
            {
                LanguageDepart = LanguageFactory.GetLanguageByAbbreviation(StorageService.Data.AbbreviationLanguageDepart);
                LanguageArrive = LanguageFactory.GetLanguageByAbbreviation(StorageService.Data.AbbreviationLanguageArrive);
            }
            else
            {
                // by default from english
                LanguageDepart = LanguageFactory.GetLanguageByAbbreviation("en");

                // to (auto-detect) device language
                var currentCulture = CultureInfo.CurrentCulture;
                string abbreviation = currentCulture.Name.Substring(0, 2).ToLower();
                LanguageArrive = LanguageFactory.GetLanguageByAbbreviation(abbreviation) ?? LanguageFactory.GetLanguageByAbbreviation("en");
            }

            // restore word
            if (StorageService.Data.Word != null)
                MotRecherche = StorageService.Data.Word;

            // restore current translations
            if (StorageService.Data.Translations != null)
            {
                CurrentTranslations.Populate(StorageService.Data.Translations);
                RaisePropertyChanged("TranslatedKeyGroup");
            }

            StorageService.IsRestoring = false;

            // remove Secondary Tile key from Local Storage
            if (!string.IsNullOrWhiteSpace(secondaryTile))
                StorageService.FileStorageService.Folder = "ModernWordreference";
        }

        #endregion


        #region Command methods

        private bool CanTranslate()
        {
            return !IsTranslating && DataService.CanTranslate(LanguageDepart, LanguageArrive, MotRecherche);
        }
        private async void Translate()
        {
            // first step : clear data (logic & view)
            DataService.ClearData();
            CurrentTranslations.Clear();
            RaisePropertyChanged("TranslatedKeyGroup");

            // second step : start translating
            IsTranslating = true;
            bool? loaded = await DataService.LoadAsync(LanguageDepart, LanguageArrive, MotRecherche);

            if (loaded.HasValue)
            {
                if (loaded.Value)
                {
                    // third step : update TranslatedKeyGroup (that will notify view)
                    CurrentTranslations.Populate(DataService.Translations);
                    RaisePropertyChanged("TranslatedKeyGroup");

                    // last step : save translated data
                    Save();

                    // and notify main commands
                    ViewModelLocator.MainVM.TranslationDone();

                    _telemetryService.TelemetryClient.TrackEvent("TranslationDone", new Dictionary<string, string>
                    {
                        {"Word", MotRecherche},
                        {"From", LanguageDepart.Abbreviation},
                        {"To", LanguageArrive.Abbreviation}
                    }, new Dictionary<string, double>
                    {
                        {"PrincipalTranslations", CurrentTranslations.TraductionsPrincipales.Count},
                        {"AdditionalTranslations", CurrentTranslations.TraductionsAdditionnelles.Count},
                        {"CompoundForms", CurrentTranslations.FormesComposees.Count}
                    });
                }
                else
                {
                    _localNotificationService.SendNotification(null, _resourceLoader.GetString("ApiErrorDescription"));

                    _telemetryService.TelemetryClient.TrackEvent("NoAvailableTranslation",
                        new Dictionary<string, string>
                        {
                            {"Word", MotRecherche},
                            {"From", LanguageDepart.Abbreviation},
                            {"To", LanguageArrive.Abbreviation}
                        });
                }
            }
            else
            {
                _localNotificationService.SendNotification(null, _resourceLoader.GetString("RequiredInternetConnection"));

                _telemetryService.TelemetryClient.TrackEvent("NoInternetConnection");
            }

            IsTranslating = false;
        }


        private void TranslateWithParam(bool canTranslate)
        {
            if (CanTranslate() && canTranslate)
                Translate();
        }


        private void WordUpdated(string word)
        {
            MotRecherche = word;
            TranslateCommand.RaiseCanExecuteChanged();
        }


        private bool CanSwitch()
        {
            return LanguageDepart != LanguageArrive;
        }
        private void Switch()
        {
            _telemetryService.TelemetryClient.TrackEvent("SwitchLanguage",
                new Dictionary<string, string>
                {
                    {"From", LanguageDepart.Abbreviation},
                    {"To", LanguageArrive.Abbreviation}
                });

            var languageSwitch = LanguageDepart;
            LanguageDepart = LanguageArrive;
            LanguageArrive = languageSwitch;
        }

        #endregion
    }
}
