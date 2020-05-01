using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {
        #region Private members
        private IFrameNavigationService _navigationService;

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

                        _navigationService.NavigateTo("Main");

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
