
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using LibraryAzure;
using System;
using System.IO;
using System.Threading;

namespace Borto_v1
{
    public class DownloadViewModel : ViewModelBase
    {

        #region Private members
        private EFUnitOfWork context = new EFUnitOfWork();

        private IFrameNavigationService _navigationService;

        private Video video { get; set; }

        private string pathFolder;

        private bool isVisibleProgressBar;

        private Thread downloadThread;

        private Quality qualityType;
        /// <summary>
        /// Checks if video has been deleted
        /// </summary>
        private Thread checkingVideoThread;
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
        
        public Quality QualityType
        {
            get
            {
                return qualityType;
            }
            set
            {
                if (qualityType == value)
                {
                    return;
                }
                qualityType = value;
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
                            try
                            {
                                if (Video.HasConvertation)
                                {
                                    DownloadConvert();
                                }
                                else
                                {
                                    Download();
                                }
                            }
                            catch (ThreadAbortException ex) { }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        checkingVideoThread = new Thread(() =>
                        {
                            try
                            {
                                CheckIsVideoExist();
                            }
                            catch (ThreadAbortException ex) { }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        downloadThread.IsBackground = true;
                        downloadThread.Start();
                        checkingVideoThread.IsBackground = true;
                        checkingVideoThread.Start();
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

        #region Helpers
        /// <summary>
        /// Start download video from Azure with file name like in database,
        /// if this name is already taken , than add Guid.NewGuid().ToString("N")
        /// </summary>
        private void Download()
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
            
            SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.Your_video_downloaded;
            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
        }
           private async void DownloadConvert()
        {
            ConverterVideoAzure helper = new ConverterVideoAzure();
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
            await helper.DownloadConvert(Video.Path, NameToSave, PathFolder, (int)QualityType);
            IsVisibleProgressBar = false;
            
            SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.Your_video_downloaded;
            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
        }

        private void CheckIsVideoExist()
        {
            while (IsVisibleProgressBar)
            {
                AzureHelper helper = new AzureHelper();
                if (context.Videos.IsExist(Video.IdVideo))
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    downloadThread.Abort();
                    IsVisibleProgressBar = false;
                    SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.Video_was_deleted_from_server;
                    SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                }
            }
        }
        #endregion
    }
}
