using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Borto_v1
{
    public class AdminViewModel : ViewModelBase
    {
        #region Private Fields

        private IFrameNavigationService _navigationService;

        #endregion


        #region Public Members


        #endregion

        #region Commands

        private RelayCommand adminCommentsCommand;
        public RelayCommand AdminCommentsCommand
        {
            get
            {
                return adminCommentsCommand
                    ?? (adminCommentsCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("AdminComments");
                    }));
            }
        }
         private RelayCommand adminVideosCommand;
        public RelayCommand AdminVideosCommand
        {
            get
            {
                return adminVideosCommand
                    ?? (adminVideosCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("AdminVideos");
                    }));
            }
        }

        #endregion


        #region ctor

        public AdminViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion
    }
}
