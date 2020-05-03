using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Members
        private User user;
        private IFrameNavigationService _navigationService;

        #endregion

        #region Public members
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
        #endregion

        #region Commands
        private RelayCommand _downloadpageCommand;
        public RelayCommand DownloadPageCommand
        {
            get
            {
                return _downloadpageCommand
                    ?? (_downloadpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Download");
                    }));
            }
        } 
        private RelayCommand _loginpageCommand;
        public RelayCommand LoginPageCommand
        {
            get
            {
                return _loginpageCommand
                    ?? (_loginpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Login");
                    }));
            }
        }
        private RelayCommand _upoadpageCommand;
        public RelayCommand UploadPageCommand
        {
            get
            {
                return _upoadpageCommand
                    ?? (_upoadpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Upload");
                    }));
            }
        }
        private RelayCommand _watchingpageCommand;
        public RelayCommand WatchingpageCommand
        {
            get
            {
                return _watchingpageCommand
                    ?? (_watchingpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Watching");
                    }));
            }
        }
        private RelayCommand _accountpageCommand;
        public RelayCommand AccountpageCommand
        {
            get
            {
                return _accountpageCommand
                    ?? (_accountpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Account");
                    }));
            }
        }
        private RelayCommand _settingspageCommand;
        public RelayCommand SettingspageCommand
        {
            get
            {
                return _settingspageCommand
                    ?? (_settingspageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Settings");
                    }));
            }
        }

        private RelayCommand _viewWatchingPageCommand;
        public RelayCommand ViewWatchingPageCommand
        {
            get
            {
                return _viewWatchingPageCommand
                    ?? (_viewWatchingPageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Player");
                    }));
            }
        }
        #endregion

        #region ctor

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion
    }
}
