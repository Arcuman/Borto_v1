using Borto_v1.Helpers;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DownloadViewModel>();
            SimpleIoc.Default.Register<UploadViewModel>();
            SimpleIoc.Default.Register<WatchingViewModel>();
            SetupNavigation();
        }

        private static void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("Download", new Uri("../Pages/DownloadPage.xaml", UriKind.Relative));
            navigationService.Configure("Upload", new Uri("../Pages/UploadPage.xaml", UriKind.Relative));
            navigationService.Configure("Watching", new Uri("../Pages/WatchingPage.xaml", UriKind.Relative));
            navigationService.Configure("Account", new Uri("../Pages/AccountPage.xaml", UriKind.Relative));
            navigationService.Configure("Settings", new Uri("../Pages/SettingsPage.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public DownloadViewModel DownloadViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DownloadViewModel>();
            }
        }
        public UploadViewModel UploadViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UploadViewModel>();
            }
        }
        public WatchingViewModel WatchingViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WatchingViewModel>();
            }
        }
        public AccountViewModel AccountViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AccountViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}
