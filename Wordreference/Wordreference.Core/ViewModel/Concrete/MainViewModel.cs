using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wordreference.Core.Services.Abstract;
using Wordreference.Core.ViewModel.Abstract;

namespace Wordreference.Core.ViewModel.Concrete
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    internal class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region Properties

        public IRatingService RatingService { get; private set; }
        public ITranslationViewModel TranslationViewModel { get; private set; }
        public ISettingsViewModel SettingsViewModel { get; private set; }
        public ISecondaryTileService<ITranslationViewModel> TranslationsSecondaryTileService { get; private set; }

        public bool CanPin
        {
            get
            {
                return (TranslationViewModel.DataService != null && !string.IsNullOrWhiteSpace(TranslationViewModel.DataService.MotRecherche) &&
                    !TranslationsSecondaryTileService.IsSecondaryTileExist(TranslationViewModel));
            }
        }

        public bool CanUnpin
        {
            get
            {
                return (TranslationViewModel.DataService != null && !string.IsNullOrWhiteSpace(TranslationViewModel.DataService.MotRecherche) &&
                    TranslationsSecondaryTileService.IsSecondaryTileExist(TranslationViewModel));
            }
        }


        private string _secondaryTileArgument;
        public string SecondaryTileArgument { get { return _secondaryTileArgument; } set { _secondaryTileArgument = value; TranslationViewModel.Restore(value); } }

        #endregion


        #region Commands

        public RelayCommand PinCommand { get; private set; }
        public RelayCommand UnpinCommand { get; private set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IRatingService ratingService, ISecondaryTileService<ITranslationViewModel> translationsSecondaryTileService)
        {
            // Injection
            RatingService = ratingService;
            TranslationViewModel = ViewModelLocator.TranslationVM;
            SettingsViewModel = ViewModelLocator.SettingsVM;
            TranslationsSecondaryTileService = translationsSecondaryTileService;

            // Commands
            PinCommand = new RelayCommand(Pin);
            UnpinCommand = new RelayCommand(Unpin);

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"

                // ask for rating
                RatingService.AskForRating();
            }
        }

        #endregion


        #region Command methods

        private async void Pin()
        {
            await TranslationsSecondaryTileService.CreateSecondaryTile(TranslationViewModel);
            TranslationDone();
        }

        private async void Unpin()
        {
            await TranslationsSecondaryTileService.RemoveSecondaryTile(TranslationViewModel);
            TranslationDone();
        }

        #endregion


        #region Methods

        public void TranslationDone()
        {
            RaisePropertyChanged("CanPin");
            RaisePropertyChanged("CanUnpin");
        }

        #endregion
    }
}