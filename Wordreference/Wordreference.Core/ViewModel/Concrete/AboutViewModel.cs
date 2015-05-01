using System;
using Windows.System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wordreference.Core.ViewModel.Abstract;

namespace Wordreference.Core.ViewModel.Concrete
{
    internal class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        #region Commands

        public RelayCommand GoToTwitterCommand { get; private set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the AboutViewModel class.
        /// </summary>
        public AboutViewModel()
        {
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
            await Launcher.LaunchUriAsync(new Uri(@"https://twitter.com/intent/tweet?text=%40dbottiau%20%23ModernWordreference"));
        }

        #endregion
    }
}
