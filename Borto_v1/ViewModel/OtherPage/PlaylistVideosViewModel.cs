using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class PlaylistVideosViewModel : ViewModelBase
    {
        #region Consts

        private const int videoOnThePage = 6;

        #endregion

        #region Private fields
        EFUnitOfWork context = new EFUnitOfWork();

        private ObservableCollection<Video> videos;

        private IFrameNavigationService _navigationService;

        private Video selectedVideo;

        private Thread loadedThread;

        private Playlist playlist;

        private string name;

        private bool isVisibleProgressBar;

        private int pageCount;

        private int videosCount;

        private bool isListNull;
        #endregion

        #region Public members
        public ObservableCollection<Video> Videos
        {
            get
            {
                return videos;
            }

            set
            {
                if (videos == value)
                {
                    return;
                }

                videos = value;
                RaisePropertyChanged();
            }
        }
        public Video SelectedVideo
        {
            get
            {
                return selectedVideo;
            }
            set
            {
                if (selectedVideo == value)
                {
                    return;
                }

                selectedVideo = value;
                RaisePropertyChanged();
            }
        }
        public bool IsListNull
        {
            get
            {
                return isListNull;
            }
            set
            {
                if (isListNull == value)
                {
                    return;
                }
                isListNull = value;
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
        public int PageCount
        {
            get
            {
                return pageCount;
            }
            set
            {
                if (pageCount == value)
                {
                    return;
                }
                pageCount = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private RelayCommandParametr _downloadpageCommand;
        public RelayCommandParametr DownloadPageCommand
        {
            get
            {
                return _downloadpageCommand
                    ?? (_downloadpageCommand = new RelayCommandParametr(
                    obj =>
                    {
                        SelectedVideo = obj as Video;
                        _navigationService.NavigateTo("Download", obj);
                    }));
            }
        }
        private RelayCommandParametr deletePlaylistVideoCommand;
        public RelayCommandParametr DeletePlaylistVideoCommand
        {
            get
            {
                return deletePlaylistVideoCommand
                    ?? (deletePlaylistVideoCommand = new RelayCommandParametr(
                    obj =>
                    {
                        SelectedVideo = obj as Video;
                        int id = context.PlaylistVideo.GetIfExist(playlist.IdPlaylist, SelectedVideo.IdVideo).IdPlaylistVideo;
                        context.PlaylistVideo.Delete(id);
                        Videos.Remove(SelectedVideo);
                        if (Videos.Count == 0)
                        {
                            IsListNull = true;
                        }
                    }));
            }
        }
        private RelayCommandParametr loadedCommand;
        public RelayCommandParametr LoadedCommand
        {
            get
            {
                return loadedCommand
                    ?? (loadedCommand = new RelayCommandParametr(
                    obj =>
                    {
                        IsVisibleProgressBar = true;
                        PageCount = 1;
                        IsListNull = false;
                        loadedThread = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.PlaylistVideo.GetVideoByPlayList(PageCount,videoOnThePage, playlist.IdPlaylist, out videosCount));
                                if (Videos.Count() == 0)
                                    IsListNull = true;
                                else
                                    IsListNull = false;
                                IsVisibleProgressBar = false;
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        loadedThread.IsBackground = true;
                        loadedThread.Start();

                    }));
            }
        }
        private RelayCommandParametr _viewWatchingPageCommand;
        public RelayCommandParametr ViewWatchingPageCommand
        {
            get
            {
                return _viewWatchingPageCommand
                    ?? (_viewWatchingPageCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        SelectedVideo = obj as Video;
                        _navigationService.NavigateTo("VideoWatching", SelectedVideo);
                    }));
            }
        }
        private RelayCommandParametr nextPageCommand;
        public RelayCommandParametr NextPageCommand
        {
            get
            {
                return nextPageCommand
                    ?? (nextPageCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount++;
                        IsVisibleProgressBar = true;
                        Thread sort = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.PlaylistVideo.GetVideoByPlayList(PageCount, videoOnThePage, playlist.IdPlaylist, out videosCount));
                                IsVisibleProgressBar = false;
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        sort.IsBackground = true;
                        sort.Start();
                    },
                    x => PageCount * 6 < videosCount));
            }
        }
        private RelayCommandParametr backPageCommand;
        public RelayCommandParametr BackPageCommand
        {
            get
            {
                return backPageCommand
                    ?? (backPageCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount--;
                        IsVisibleProgressBar = true;
                        Thread sort = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.PlaylistVideo.GetVideoByPlayList(PageCount, videoOnThePage, playlist.IdPlaylist, out videosCount));
                                IsVisibleProgressBar = false;
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        sort.IsBackground = true;
                        sort.Start();
                    },
                    x => PageCount > 1));
            }
        }

        #endregion


        #region ctor
        public PlaylistVideosViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            playlist = navigationService.Parameter as Playlist;
            Name = playlist.Name;
            IsVisibleProgressBar = true;
        }
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion
    }
}
