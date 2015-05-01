using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

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

            PointerEntered += MainPage_PointerEntered;

            var pointerMoved = Observable.FromEventPattern<PointerRoutedEventArgs>(animationHiderButton, "PointerMoved")
                .Sample(TimeSpan.FromMilliseconds(30))
                .ObserveOnDispatcher(CoreDispatcherPriority.High);

            pointerMoved.Subscribe(OnPointerMoved);
        }




        private void Translation_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                Focus(FocusState.Programmatic);
        }

        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }


        #region Show / Hide parameter block animation

        private double _mouseVerticalPosition;

        private void MainPage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _mouseVerticalPosition = e.GetCurrentPoint(this).Position.Y;
        }

        private void OnPointerMoved(EventPattern<PointerRoutedEventArgs> args)
        {
            double pointerY = args.EventArgs.GetCurrentPoint(this).Position.Y;

            double deltaY = pointerY - _mouseVerticalPosition;
            double newHeigthValue = -parameterGrid.Margin.Bottom - deltaY;

            if (newHeigthValue < 0)
                newHeigthValue = 0;

            if (newHeigthValue > 180)
                newHeigthValue = 180;

            parameterGrid.Margin = new Thickness(0, 0, 0, -newHeigthValue);
            parameterGrid.Opacity = (180 - newHeigthValue) / 180 + 0.4;

            _mouseVerticalPosition = pointerY;
        }

        #endregion
    }
}
