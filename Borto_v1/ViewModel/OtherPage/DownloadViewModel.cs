
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.IO;
using System.Threading;

namespace Borto_v1
{
    public class DownloadViewModel : ViewModelBase
    {

        #region Private members

        private IFrameNavigationService _navigationService;

        private Video video { get; set; }

        private string pathFolder;

        private bool isVisibleProgressBar;

        private Thread downloadThread;
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
        private RelayCommandParametr downloadVideoCommand;
        public RelayCommandParametr DownloadVideoCommand
        {
            get
            {
                return downloadVideoCommand
                    ?? (downloadVideoCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        IsVisibleProgressBar = true;
                        downloadThread = new Thread(() =>
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
                            IsVisibleProgressBar = false;

                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Your video downloaded!";
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        });
                        downloadThread.IsBackground = true;
                        downloadThread.Start();
                    },
                    (x)=> !String.IsNullOrWhiteSpace(PathFolder)));
            }
        }
        private RelayCommandParametr cancelUploadCommand;
        public RelayCommandParametr CancelUploadCommand
        {
            get
            {
                return cancelUploadCommand
                    ?? (cancelUploadCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        downloadThread.Abort();
                        IsVisibleProgressBar = false;
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
                        if (isVisibleProgressBar == false)
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
            if (!isVisibleProgressBar)
            {
                PathFolder = string.Empty;
            }
        }

        #endregion
    }
}
