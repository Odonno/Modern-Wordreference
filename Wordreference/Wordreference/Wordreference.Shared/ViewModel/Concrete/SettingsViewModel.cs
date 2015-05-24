using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using Wordreference.Core.Services.Abstract;
using Wordreference.Core.ViewModel.Abstract;

namespace Wordreference.Core.ViewModel.Concrete
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        #region Fields

        private readonly IFileStorageService _fileStorageService;
        private readonly ITelemetryService _telemetryService;

        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private const string _filename = "settings";
        private bool _isRestoring;

        #endregion


        #region Properties

        public IEnumerable<string> Themes { get; private set; }

        private string _selectedTheme;
        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set { RequestSaveChanges(value); }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel(IFileStorageService fileStorageService,
            ITelemetryService telemetryService)
        {
            // injection
            _fileStorageService = fileStorageService;
            _telemetryService = telemetryService;

            _fileStorageService.FormatFile = "txt";

            // properties
            _isRestoring = true;

            Themes = new[] { "light", "dark" };
            _selectedTheme = Themes.First();

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"

                // get selected theme in local storage
                Restore();
            }

            _isRestoring = false;
        }

        #endregion


        #region Methods

        private async void Restore()
        {
            bool exist = await _fileStorageService.IsExistAsync(_filename);

            if (exist)
            {
                string savedSelectedTheme = await _fileStorageService.RestoreAsync(_filename);
                string selectedTheme = Themes.FirstOrDefault(s => s == savedSelectedTheme);

                if (selectedTheme != null)
                    _selectedTheme = selectedTheme;
            }
        }

        private async Task Save(string newTheme)
        {
            if (!_isRestoring)
                await _fileStorageService.SaveAsync(_filename, newTheme);
        }

        private async void RequestSaveChanges(string newTheme)
        {
            var messageDialog = new MessageDialog(_resourceLoader.GetString("messageSavingSettings"), _resourceLoader.GetString("savingSettings"));

            messageDialog.Commands.Add(new UICommand(
                _resourceLoader.GetString("ok"), command => SaveChanges(newTheme)));

            messageDialog.Commands.Add(new UICommand(
                _resourceLoader.GetString("cancel"), null));
            
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();
        }

        private async void SaveChanges(string newTheme)
        {
            _telemetryService.TelemetryClient.TrackEvent("SwitchTheme",
               new Dictionary<string, string>
                {
                    {"To", newTheme}
                });

            // save and exit the app
            await Save(newTheme);
            Application.Current.Exit();
        }

        #endregion
    }
}
