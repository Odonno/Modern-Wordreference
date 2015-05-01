using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Wordreference
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;

            SizeChanged += Page_SizeChanged;
        }


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // When the size change, update the correct visual state
            VisualStateManager.GoToState(this, e.NewSize.Width < 900 ? "SnappedView" : "NormalView", true);
        }

        private void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var privacyPolicySetting =
                new SettingsCommand("PrivacyPolicy", "Privacy Policy", handler =>
                {
                    var privacyPolicyFlyout = new PrivacyPolicyFlyout();
                    privacyPolicyFlyout.Show();

                });

            args.Request.ApplicationCommands.Add(privacyPolicySetting);
        }
    }
}
