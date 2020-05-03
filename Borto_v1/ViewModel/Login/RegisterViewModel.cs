using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class RegisterViewModel : ViewModelBase
    {
        #region Private members

        private IFrameNavigationService _navigationService;

        private User user;

        private string name;

        private string login;

        #endregion

        #region Public Members

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

        #endregion

        #region Commands
        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new RelayCommand(
                    () =>
                    {

                        _navigationService.NavigateTo("Login");

                    }));
            }
        }

        private RelayCommand _registerCommand;
        public RelayCommand RegisterCommand
        {
            get
            {
                return _registerCommand
                    ?? (_registerCommand = new RelayCommand(
                    () =>
                    {
                        user = new User(name,login,"password");
                        Messenger.Default.Send<OpenWindowMessage>(
                            new OpenWindowMessage() { Type = WindowType.kMain, Argument = user });

                    }));
            }
        }
        #endregion

        #region ctor

        public RegisterViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

    }
}
