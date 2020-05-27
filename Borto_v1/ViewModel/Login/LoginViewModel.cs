
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Linq;
using System.Resources;
using System.Threading;
using WPFLocalizeExtension.Engine;

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
        private RelayCommand forgotPasswordCommand;
        public RelayCommand ForgotPasswordCommand
        {
            get
            {
                return forgotPasswordCommand
                    ?? (forgotPasswordCommand = new RelayCommand(
                    () =>
                    {
                        IsOpenDialog = true;
                    }));
            }
        }
        private RelayCommand restoryCommand;
        public RelayCommand RestoryCommand
        {
            get
            {
                return restoryCommand
                    ?? (restoryCommand = new RelayCommand(
                    () =>
                    {
                        ThreadPool.QueueUserWorkItem(
                       o =>
                       {
                           IsVisibleProgressBar = true;
                           if (!context.Users.IsExist(Login))
                           {

                               IsOpenDialog = false;
                               SimpleIoc.Default.GetInstance<LoginWindowViewModel>().Message = Properties.Resources.This_login_is_not_exist;
                               SimpleIoc.Default.GetInstance<LoginWindowViewModel>().IsOpenDialog = true;
                           }
                           else if (context.Users.GetEmail(Login) == null)
                           {
                               IsOpenDialog = false;
                               SimpleIoc.Default.GetInstance<LoginWindowViewModel>().Message = Properties.Resources.Mail_is_not_attached;
                               SimpleIoc.Default.GetInstance<LoginWindowViewModel>().IsOpenDialog = true;
                           }
                           else
                           {
                               User user = context.Users.Find(x => x.Login.Equals(Login)).FirstOrDefault();
                               string Password = MailsService.GetPass(10);
                               user.Password = User.getHash(Password);
                               context.Users.Update(user);
                               MailsService.SendEmail(context.Users.GetEmail(Login), "Password recovery", $"<h2>Your temporary password {Password} </h2>");
                               IsOpenDialog = false;
                               SimpleIoc.Default.GetInstance<LoginWindowViewModel>().Message = Properties.Resources.A_new_password_has_been_sent_to_the_mail;
                               SimpleIoc.Default.GetInstance<LoginWindowViewModel>().IsOpenDialog = true;
                           }
                           IsVisibleProgressBar = false;
                       });
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
                                    SimpleIoc.Default.GetInstance<LoginWindowViewModel>().Message = Properties.Resources.IncorrectData;
                                    SimpleIoc.Default.GetInstance<LoginWindowViewModel>().IsOpenDialog = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<LoginWindowViewModel>().Message = Properties.Resources.ServerError + ex.Message;
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
