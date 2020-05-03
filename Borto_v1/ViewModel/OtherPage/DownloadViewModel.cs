using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Borto_v1
{
    public class DownloadViewModel: ViewModelBase
    {

        #region Private members

        private IFrameNavigationService _navigationService;

        private Video video { get; set; }

        #endregion

        #region Public members

        public Video Video
        {
            get
            {
                return video;
            }
            set
            {
                if (video == value)
                {
                    return;
                }
                video = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private RelayCommand _uploadpageCommand;
        public RelayCommand UploadPageCommand
        {
            get
            {
                return _uploadpageCommand
                    ?? (_uploadpageCommand = new RelayCommand(
                    () =>
                    {
                        
                        MessageBox.Show(_navigationService.Parameter.ToString());
                    }));
            }
        }   
        private RelayCommand _loadedpageCommand;
        public RelayCommand LoadedPageCommand
        {
            get
            {
                return _loadedpageCommand
                    ?? (_loadedpageCommand = new RelayCommand(
                    () =>
                    {
                        Video = _navigationService.Parameter as Video;
                    }));
            }
        }
        #endregion

        #region ctor

        public DownloadViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Video = navigationService.Parameter as Video;
        }

        #endregion
    }
}
