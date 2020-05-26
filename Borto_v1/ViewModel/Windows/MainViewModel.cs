
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using WPFLocalizeExtension.Engine;

namespace Borto_v1
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Members
        private User user;

        private IFrameNavigationService _navigationService;

        private bool isOpenDialog;

        private bool isNoInternetConnection;

        private string message;

        private bool isAdmin;

        private bool cultureInfoEn;
        #endregion

        #region Public members
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (user == value)
                {
                    return;
                }
                user = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Is Open Dialog 
        /// </summary>
        public bool IsOpenDialog
        {
            get
            {
                return isOpenDialog;
            }
            set
            {
                if (isOpenDialog == value)
                {
                    return;
                }
                isOpenDialog = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Is Open Dialog 
        /// </summary>
        public bool IsNoInternetConnection
        {
            get
            {
                return isNoInternetConnection;
            }
            set
            {
                if (isNoInternetConnection == value)
                {
                    return;
                }
                isNoInternetConnection = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Message for the dialog  
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message == value)
                {
                    return;
                }
                message = value;
                RaisePropertyChanged();
            }
        }
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                if (isAdmin == value)
                {
                    return;
                }
                isAdmin = value;
                RaisePropertyChanged();
            }
        }
        public bool CultureInfoEn
        {
            get
            {
                return cultureInfoEn;
            }
            set
            {
                if (cultureInfoEn == value)
                {
                    return;
                }
                cultureInfoEn = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private RelayCommand _downloadpageCommand;
        public RelayCommand DownloadPageCommand
        {
            get
            {
                return _downloadpageCommand
                    ?? (_downloadpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Download");
                    }));
            }
        }
        private RelayCommand _loginpageCommand;
        public RelayCommand LoginPageCommand
        {
            get
            {
                return _loginpageCommand
                    ?? (_loginpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Login");
                    }));
            }
        }
        private RelayCommand _upoadpageCommand;
        public RelayCommand UploadPageCommand
        {
            get
            {
                return _upoadpageCommand
                    ?? (_upoadpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Upload");
                    }));
            }
        }
        private RelayCommand _watchingpageCommand;
        public RelayCommand WatchingpageCommand
        {
            get
            {
                return _watchingpageCommand
                    ?? (_watchingpageCommand = new RelayCommand(
                    () =>
                    {
                        CultureInfoEn = Thread.CurrentThread.CurrentCulture.Name == "en-US";
                        var thread = new Thread(() =>
                        {
                            while (true)
                            {
                                Thread.Sleep(2000);
                                bool result = IsInternetConnection();
                                if (result == true)
                                {
                                    if (IsNoInternetConnection == true)
                                    {
                                        IsNoInternetConnection = false;
                                        IsOpenDialog = false;
                                    }
                                }
                                else
                                {
                                    IsOpenDialog = true;
                                    Message = Properties.Resources.No_internet_connection;
                                    IsNoInternetConnection = true;
                                }
                            }
                        });
                        thread.IsBackground = true;
                        thread.Start();
                        _navigationService.NavigateTo("Watching");
                    }));
            }
        }
        private RelayCommand _accountpageCommand;
        public RelayCommand AccountpageCommand
        {
            get
            {
                return _accountpageCommand
                    ?? (_accountpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Account");
                    }));
            }
        }
        private RelayCommand adminpageCommand;
        public RelayCommand AdminpageCommand
        {
            get
            {
                return adminpageCommand
                    ?? (adminpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Settings");
                    }));
            }
        }

        private RelayCommand _viewWatchingPageCommand;
        public RelayCommand ViewWatchingPageCommand
        {
            get
            {
                return _viewWatchingPageCommand
                    ?? (_viewWatchingPageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Player");
                    }));
            }
        }
        private RelayCommand favoriteVideosPageCommand;
        public RelayCommand FavoriteVideosPageCommand
        {
            get
            {
                return favoriteVideosPageCommand
                    ?? (favoriteVideosPageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("FavoriteVideos");
                    }));
            }
        }
        private RelayCommand closeDialodCommand;
        public RelayCommand CloseDialodCommand
        {
            get
            {
                return closeDialodCommand
                    ?? (closeDialodCommand = new RelayCommand(
                    () =>
                    {
                        IsOpenDialog = false;
                    }));
            }
        }
        private RelayCommand switchLanguageCommand;
        public RelayCommand SwitchLanguageCommand
        {
            get
            {
                return switchLanguageCommand
                    ?? (switchLanguageCommand = new RelayCommand(
                    () =>
                    {
                        if (LocalizeDictionary.CurrentCulture.Name == "")
                        {
                            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                            LocalizeDictionary.Instance.Culture = new CultureInfo("ru-RU");
                            var culture = new CultureInfo("ru-RU");
                            Thread.CurrentThread.CurrentCulture = culture;
                            Thread.CurrentThread.CurrentUICulture = culture;
                            CultureInfoEn = false;
                        }
                        else
                        {
                            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                            LocalizeDictionary.Instance.Culture = new CultureInfo("");
                            var culture = new CultureInfo("en-US");
                            Thread.CurrentThread.CurrentCulture = culture;
                            Thread.CurrentThread.CurrentUICulture = culture;
                            CultureInfoEn = true;
                        }
                    }));
            }
        }

        #endregion

        #region ctor

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            
        }

        #endregion

        #region Helpers

        public static bool IsInternetConnection()
        {
            WebRequest req = WebRequest.Create("http://google.com/generate_204");
            WebResponse resp;
            try
            {
                resp = req.GetResponse();
                resp.Close();
                req = null;
                return true;
            }
            catch (Exception ex)
            {
                req = null;
                return false;
            }
        }


        #endregion
    }
}
