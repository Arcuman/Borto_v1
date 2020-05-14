using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;

namespace Borto_v1
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DownloadViewModel>();
            SimpleIoc.Default.Register<UploadViewModel>();
            SimpleIoc.Default.Register<VideoWatchingPageViewModel>();
            SimpleIoc.Default.Register<LoginWindowViewModel>();
            SetupNavigation();
        }

        private static void SetupNavigation()
        {
            var navigationService = new FrameNavigationService();
            navigationService.Configure("Download", new Uri("../Pages/Menu/DownloadPage.xaml", UriKind.Relative));
            navigationService.Configure("Upload", new Uri("../Pages/Menu/UploadPage.xaml", UriKind.Relative));
            navigationService.Configure("Watching", new Uri("../Pages/Menu/WatchingPage.xaml", UriKind.Relative));
            navigationService.Configure("Account", new Uri("../Pages/Menu/AccountPage.xaml", UriKind.Relative));
            navigationService.Configure("Settings", new Uri("../Pages/Menu/SettingsPage.xaml", UriKind.Relative));
            navigationService.Configure("VideoWatching", new Uri("../Pages/VideoWatchingPage.xaml", UriKind.Relative));
            navigationService.Configure("Login", new Uri("../Pages/Login/Login.xaml", UriKind.Relative));
            navigationService.Configure("Register", new Uri("../Pages/Login/RegisterPage.xaml", UriKind.Relative));
            navigationService.Configure("Player", new Uri("../Pages/Menu/VideoPlayerPage.xaml", UriKind.Relative));
            SimpleIoc.Default.Register<IFrameNavigationService>(() => navigationService);
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public LoginWindowViewModel LoginWindowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginWindowViewModel>();
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
        public VideoWatchingPageViewModel VideoWatchingPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<VideoWatchingPageViewModel>();
            }
        }  

        public static void Cleanup()
        {
            
        }
    }
}
