using GalaSoft.MvvmLight.Command;
using Wordreference.Core.Services.Abstract;

namespace Wordreference.Core.ViewModel.Abstract
{
    public interface IMainViewModel
    {
        #region Properties

        IRatingService RatingService { get; }
        ITranslationViewModel TranslationViewModel { get; }
        ISettingsViewModel SettingsViewModel { get; }
        ISecondaryTileService<ITranslationViewModel> TranslationsSecondaryTileService { get; }

        bool CanPin { get; }
        bool CanUnpin { get; }

        string SecondaryTileArgument { get; set; }

        #endregion


        #region Commands

        RelayCommand PinCommand { get; }
        RelayCommand UnpinCommand { get; }

        #endregion


        #region Methods

        void TranslationDone();

        #endregion
    }
}
