using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using Wordreference.API.Model;
using Wordreference.API.Services.Abstract;
using Wordreference.Core.DataModel.Concrete;
using Wordreference.Core.Factories.Abstract;
using Wordreference.Core.Services.Abstract;

namespace Wordreference.Core.ViewModel.Abstract
{
    public interface ITranslationViewModel
    {
        #region Properties

        IStorageService StorageService { get; }
        IDataService DataService { get; }
        ILanguageFactory LanguageFactory { get; }

        Language LanguageDepart { get; set; }
        Language LanguageArrive { get; set; }
        string MotRecherche { get; set; }
        bool IsTranslating { get; }
        Translations CurrentTranslations { get; }
        List<AlphaKeyGroup> TranslatedKeyGroup { get; }

        #endregion


        #region Commands

        RelayCommand TranslateCommand { get; }
        RelayCommand<bool> TranslateWithParamCommand { get; }
        RelayCommand<string> WordUpdatedCommand { get; }
        RelayCommand SwitchCommand { get; }

        #endregion


        #region Methods

        void Save();
        void Restore(string secondaryTile);

        #endregion
    }
}
