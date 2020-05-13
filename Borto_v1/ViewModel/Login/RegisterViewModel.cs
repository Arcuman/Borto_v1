using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;

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

        private byte[] image;

        private string message;
        /// <summary>
        /// Is request to DB is send ?
        /// </summary>
        private bool isVisibleProgressBar;
        /// <summary>
        /// Is User agree with term of using
        /// </summary>
        private bool isCheck;

        private bool isOpenDialog;
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
                                    Message = "This login is already exist";
                                    IsOpenDialog = true;
                                }
                                //ADD VALIDATION HERE
                                else if (Login != null && Password != null && Name != null)
                                {
                                    string hashPass = User.getHash(Password);
                                    User user = new User(Name,Login,Name, hashPass, image);
                                    context.Users.Create(user);
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
                                    Message = "Incorrect data!";
                                    IsOpenDialog = true;
                                }
                            }
                            );
                    },
                    (x) =>
                    IsCheck == true && Name?.Length > 0 && Login?.Length > 0 && Password?.Length > 0));
            }
        }
        #endregion

        #region ctor

        public RegisterViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            IsVisibleProgressBar = false;
            IsCheck = false;
            Image img = System.Drawing.Image.FromFile(new Uri("../../Assets/camera.jpg", UriKind.RelativeOrAbsolute).OriginalString);

            image = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
        }

        #endregion

    }
}
