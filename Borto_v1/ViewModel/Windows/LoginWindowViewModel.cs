using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class LoginWindowViewModel: ViewModelBase
    {
        #region Private members

        private IFrameNavigationService _navigationService;

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
                        _navigationService.NavigateTo("Login");
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


    }
}
