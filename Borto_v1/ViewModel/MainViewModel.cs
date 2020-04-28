using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private List<BMenuItem> menuList = new List<BMenuItem>();

        public List<BMenuItem> MenuList
        {
            get
            {
                return menuList;
            }
            set
            {
                if (menuList == value)
                {
                    return;
                }
                menuList = value;
                RaisePropertyChanged();
            }
        }

        private IFrameNavigationService _navigationService;

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
        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            menuList.Add(new BMenuItem("Download", PackIconKind.Download));
            menuList.Add(new BMenuItem("Upload", PackIconKind.Upload)); ;
            menuList.Add(new BMenuItem("Watching", PackIconKind.Watch));
            menuList.Add(new BMenuItem("Account", PackIconKind.Account));
            menuList.Add(new BMenuItem("Settings", PackIconKind.Settings));
        }

    }
}
