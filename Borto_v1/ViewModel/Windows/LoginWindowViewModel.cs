using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Net;
using System.Threading;

namespace Borto_v1
{
    public class LoginWindowViewModel: ViewModelBase
    {
        #region Private members
        private bool isOpenDialog;

        private IFrameNavigationService _navigationService;

        private bool isNoInternetConnection;

        private string message;
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
                                    Message = "No internet connection";
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
