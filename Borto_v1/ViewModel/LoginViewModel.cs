using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Private members
        private IFrameNavigationService _navigationService;

        #endregion

        

        #region Commands
        private RelayCommand _registerCommand;
        public RelayCommand RedisterCommand
        {
            get
            {
                return _registerCommand
                    ?? (_registerCommand = new RelayCommand(
                    () =>
                    {

                        _navigationService.NavigateTo("Register");

                    }));
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new RelayCommand(
                    () =>
                    {
                    Messenger.Default.Send<OpenWindowMessage>(
                             new OpenWindowMessage() { Type = WindowType.kMain, Argument = new User("Nikita") });

            }));
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
