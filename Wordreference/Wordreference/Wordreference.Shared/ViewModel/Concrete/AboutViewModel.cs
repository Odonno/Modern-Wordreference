using System;
using Windows.System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wordreference.Core.Services.Abstract;
using Wordreference.Core.ViewModel.Abstract;

namespace Wordreference.Core.ViewModel.Concrete
{
    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        #region Fields

        private readonly ITelemetryService _telemetryService;

        #endregion


        #region Commands

        public RelayCommand GoToTwitterCommand { get; private set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AboutViewModel class.
        /// </summary>
        public AboutViewModel(ITelemetryService telemetryService)
        {
            // Services
            _telemetryService = telemetryService;

            // Commands
            GoToTwitterCommand = new RelayCommand(GoToTwitter);

            //if (IsInDesignMode)
            //{
            //    // Code runs in Blend --> create design time data.
            //}
            //else
            //{
            //    // Code runs "for real"
            //}
        }

        #endregion


        #region Command methods

        private async void GoToTwitter()
        {
            _telemetryService.TelemetryClient.TrackEvent("GoToTwitter");

            await Launcher.LaunchUriAsync(new Uri(@"https://twitter.com/intent/tweet?text=%40dbottiau%20%23ModernWordreference"));
        }

        #endregion
    }
}
