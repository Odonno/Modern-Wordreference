/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Wordreference.W8"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Wordreference.API.Services.Abstract;
using Wordreference.API.Services.Concrete;
using Wordreference.Core.Factories.Abstract;
using Wordreference.Core.Factories.Concrete;
using Wordreference.Core.Services.Abstract;
using Wordreference.Core.Services.Concrete;
using Wordreference.Core.ViewModel.Abstract;
using Wordreference.Core.ViewModel.Concrete;

namespace Wordreference.Core.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    // Create design time view services and models
            //    //SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    // Create run time view services and models
            //    //SimpleIoc.Default.Register<IDataService, ApiDataService>();
            //}
            
            // Factories
            SimpleIoc.Default.Register<ILanguageFactory, LanguageFactory>();

            // Services
            SimpleIoc.Default.Register<IDataService, ApiDataService>();
            SimpleIoc.Default.Register<IStorageService, LocalStorageService>();
            SimpleIoc.Default.Register<IFileStorageService, FileStorageService>();
            SimpleIoc.Default.Register<ISerializerService<SaveData>, JsonSerializerService<SaveData>>();
            SimpleIoc.Default.Register<IRatingService, RatingService>();
            SimpleIoc.Default.Register<IDataService, ApiDataService>();
            SimpleIoc.Default.Register<ISecondaryTileService<ITranslationViewModel>, TranslationsSecondaryTileService>();
            SimpleIoc.Default.Register<ILocalNotificationService, LocalNotificationService>();
            SimpleIoc.Default.Register<ITelemetryService, TelemetryService>();

            // View Models
            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
            SimpleIoc.Default.Register<ITranslationViewModel, TranslationViewModel>();
            SimpleIoc.Default.Register<IAboutViewModel, AboutViewModel>();
            SimpleIoc.Default.Register<ISettingsViewModel, SettingsViewModel>();
        }


        #region Static Properties

        public static IMainViewModel MainVM { get { return ServiceLocator.Current.GetInstance<IMainViewModel>(); } }
        public static ITranslationViewModel TranslationVM { get { return ServiceLocator.Current.GetInstance<ITranslationViewModel>(); } }
        public static IAboutViewModel AboutVM { get { return ServiceLocator.Current.GetInstance<IAboutViewModel>(); } }
        public static ISettingsViewModel SettingsVM { get { return ServiceLocator.Current.GetInstance<ISettingsViewModel>(); } }

        #endregion


        #region Dynamic Properties

        public IMainViewModel Main { get { return ServiceLocator.Current.GetInstance<IMainViewModel>(); } }
        public ITranslationViewModel Translation { get { return ServiceLocator.Current.GetInstance<ITranslationViewModel>(); } }
        public IAboutViewModel About { get { return ServiceLocator.Current.GetInstance<IAboutViewModel>(); } }
        public ISettingsViewModel Settings { get { return ServiceLocator.Current.GetInstance<ISettingsViewModel>(); } }

        #endregion
    }
}