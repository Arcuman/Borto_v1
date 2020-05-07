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
    public class RegisterViewModel : ViewModelBase
    {
        #region Private members

        private IFrameNavigationService _navigationService;

        EFUnitOfWork context = new EFUnitOfWork();

        private string name;

        private string login;

        private string password;
        /// <summary>
        /// Is request to DB is send ?
        /// </summary>
        private bool isVisibleProgressBar;
        /// <summary>
        /// Is User agree with term of using
        /// </summary>
        private bool isCheck;
        #endregion

        #region Public Members


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                RaisePropertyChanged();
            }
        }
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
        /// <summary>
        /// Is User agree with term of using
        /// </summary>
        public bool IsCheck
        {
            get
            {
                return isCheck;
            }
            set
            {
                if (isCheck == value)
                {
                    return;
                }
                isCheck = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Is request to DB is send ?
        /// </summary>
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
        private RelayCommandParametr _loginCommand;
        public RelayCommandParametr LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new RelayCommandParametr(
                    (x) =>
                    {

                        _navigationService.NavigateTo("Login");

                    },
                    (x) => !IsVisibleProgressBar));
            }
        }

        private RelayCommandParametr _registerCommand;
        public RelayCommandParametr RegisterCommand
        {
            get
            {
                return _registerCommand
                    ?? (_registerCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleProgressBar = true;
                        ThreadPool.QueueUserWorkItem(
                            o =>
                            {
                                if (context.Users.IsExist(Login))
                                {
                                    IsVisibleProgressBar = false;
                                    MessageBox.Show("This Login is already exist");
                                }
                                //ADD VALIDATION HERE
                                else if (Login != null && Password != null && Name != null)
                                {
                                    string hashPass = User.getHash(Password);
                                    User user = new User(Name, Login, hashPass);
                                    context.Users.Create(user);
                                    MessageBox.Show("Successfully registered");
                                    context.Save();
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
                                    MessageBox.Show("Introduction data!");
                                }
                            }
                            );
                    },
                    (x) => 
                    IsCheck==true && Name?.Length>0 && Login?.Length>0 && Password?.Length>0));
            }
        }
        #endregion

        #region ctor

        public RegisterViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            IsVisibleProgressBar = false;
            IsCheck = false;
        }

        #endregion

    }
}
