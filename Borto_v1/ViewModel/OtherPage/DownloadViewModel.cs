
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
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

        private string pathFolder;
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
        
        public string PathFolder
        {
            get
            {
                return pathFolder;
            }
            set
            {
                if (pathFolder == value)
                {
                    return;
                }
                pathFolder = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private RelayCommand chooseFolderToSaveVideo;
        public RelayCommand ChooseFolderToSaveVideo
        {
            get
            {
                return chooseFolderToSaveVideo
                    ?? (chooseFolderToSaveVideo = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, "ChooseFolder"));
                    }));
            }
        }   
        private RelayCommand downloadVideoCommand;
        public RelayCommand DownloadVideoCommand
        {
            get
            {
                return downloadVideoCommand
                    ?? (downloadVideoCommand = new RelayCommand(
                    () =>
                    {
                        AzureHelper helper = new AzureHelper();
                        string NameToSave = Video.Name;
                        if (!string.IsNullOrWhiteSpace(PathFolder))
                        {
                            string[] files = Directory.GetFiles(PathFolder);
                            foreach (var file in files)
                            {
                                string filename = file.Substring(file.LastIndexOf('\\') + 1);
                                if (filename == (Video.Name + ".mp4"))
                                    NameToSave += Guid.NewGuid().ToString("N");
                            }
                        }
                        helper.download_FromBlob(Video.Path, NameToSave, PathFolder);
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
