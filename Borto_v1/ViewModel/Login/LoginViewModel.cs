
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Threading;

namespace Borto_v1
{
    public class LoginViewModel : ViewModelBase
    {
        #region Private members

        EFUnitOfWork context = new EFUnitOfWork();

        private IFrameNavigationService _navigationService;

        private string login;

        private string password;

        private bool isVisibleProgressBar;

        private bool isOpenDialog;

        private string message;
        #endregion


        #region Public Members
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                if (login == value)
                {
                    return;
                }
                login = value;
                RaisePropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password == value)
                {
                    return;
                }
                password = value;
                RaisePropertyChanged();
            }
        }

        public bool IsVisibleProgressBar
        {
            get
            {
                return isVisibleProgressBar;
            }
            set
            {
                if (isVisibleProgressBar == value)
                {
                    return;
                }
                isVisibleProgressBar = value;
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
        private RelayCommandParametr _registerCommand;
        public RelayCommandParametr RegisterCommand
        {
            get
            {
                return _registerCommand
                    ?? (_registerCommand = new RelayCommandParametr(
                    (x) =>
                    {

                        _navigationService.NavigateTo("Register");

                    },
                    (x) => IsVisibleProgressBar == false));
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


        private RelayCommandParametr _loginCommand;
        public RelayCommandParametr LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                        o =>
                        {
                            try
                            {
                                if (context.Users.IsUser(Login, User.getHash(Password)))
                                {

                                    User user = context.Users.GetUsersByLogin(Login);
                                    DispatcherHelper.CheckBeginInvokeOnUI(
                                        () =>
                                        {
                                            Messenger.Default.Send<OpenWindowMessage>(
                                            new OpenWindowMessage() { Type = WindowType.kMain, Argument = user });
                                        }
                                    );
                                }
                                else
                                {
                                    IsVisibleProgressBar = false;
                                    Message = "Incorrect data!";
                                    IsOpenDialog = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<LoginWindowViewModel>().Message = "Server error: " + ex.Message;
                                SimpleIoc.Default.GetInstance<LoginWindowViewModel>().IsOpenDialog = true;
                            }

                        }
                    );
                    },
                    (x) =>
                    Login?.Length > 0 && Password?.Length > 0));
            }
        }

        #endregion

        #region ctor

        public LoginViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

    }
}
