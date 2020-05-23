using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using WPFLocalizeExtension.Engine;

namespace Borto_v1
{
    public class LoginWindowViewModel: ViewModelBase
    {
        #region Private members
        private bool isOpenDialog;

        private IFrameNavigationService _navigationService;

        private bool isNoInternetConnection;

        private string message;

        private bool cultureInfoEn;
        #endregion

        #region Public Fields

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

        private RelayCommand _loadedpageCommand;
        public RelayCommand LoadedPageCommand
        {
            get
            {
                return _loadedpageCommand
                    ?? (_loadedpageCommand = new RelayCommand(
                    () =>
                    {
                        CultureInfoEn = true;
                        var culture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
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
                        _navigationService.NavigateTo("Login");
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

        public LoginWindowViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion
        #region Helpers

        public static bool IsInternetConnection()
        {
            WebRequest req = WebRequest.Create("https://www.google.co.in/");
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
