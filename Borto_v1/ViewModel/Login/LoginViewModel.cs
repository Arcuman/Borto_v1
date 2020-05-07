
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        #endregion

        #region Commands
        private RelayCommandParametr _registerCommand;
        public RelayCommandParametr RedisterCommand
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
                            if (context.Users.IsUser(Login, User.getHash(Password)))
                            {

                                User user = context.Users.GetUsersByLogin(Login);
                                context.Save();
                                MessageBox.Show("Successfully login");
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
                                MessageBox.Show("Incorrect data");
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
