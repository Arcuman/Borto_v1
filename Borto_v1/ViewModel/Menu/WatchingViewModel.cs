
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Borto_v1
{

    public class WatchingViewModel : ViewModelBase, IDisposable
    {
        #region Consts

        private const int videoOnThePage = 6;

        #endregion

        #region Private members 

        EFUnitOfWork context = new EFUnitOfWork();

        private ObservableCollection<Video> videos;

        private IFrameNavigationService _navigationService;

        private Video selectedVideo;

        private string searchField;

        private SortState isSortedBy;

        private Thread loadedThread;

        private Thread searchedThread;

        private bool isVisibleProgressBar;

        private int pageCount;

        private int videosCount;
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
        public string SearchField
        {
            get
            {
                return searchField;
            }
            set
            {
                if (searchField == value)
                {
                    return;
                }

                searchField = value;
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
                        SearchField = string.Empty;
                        loadedThread = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, SortState.New, SearchField, out videosCount));
                                isSortedBy = SortState.New;
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

        private RelayCommandParametr searchCommand;
        public RelayCommandParametr SearchCommand
        {
            get
            {
                return searchCommand
                    ?? (searchCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount = 1;
                        IsVisibleProgressBar = true;
                        searchedThread = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, isSortedBy, SearchField, out videosCount));
                                IsVisibleProgressBar = false;
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        searchedThread.IsBackground = true;
                        searchedThread.Start();
                        this.Dispose();
                    }));
            }
        }
        private RelayCommandParametr sortByLongCommand;
        public RelayCommandParametr SortByLongCommand
        {
            get
            {
                return sortByLongCommand
                    ?? (sortByLongCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount = 1;
                        IsVisibleProgressBar = true;
                        Thread sort = new Thread(() =>
                          {
                              try
                              {
                                  Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, SortState.Long, SearchField, out videosCount));
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
                        this.Dispose();
                        isSortedBy = SortState.Long;
                    },
                    x => isSortedBy != SortState.Long));
            }
        }

        private RelayCommandParametr sortByShortCommand;
        public RelayCommandParametr SortByShortCommand
        {
            get
            {
                return sortByShortCommand
                    ?? (sortByShortCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount = 1;
                        IsVisibleProgressBar = true;
                        Thread sort = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, SortState.Short, SearchField, out videosCount));
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
                        this.Dispose();
                        isSortedBy = SortState.Short;
                    },
                    x => isSortedBy != SortState.Short));
            }
        }

        private RelayCommandParametr sortByNewCommand;
        public RelayCommandParametr SortByNewCommand
        {
            get
            {
                return sortByNewCommand
                    ?? (sortByNewCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount = 1;
                        IsVisibleProgressBar = true;
                        Thread sort = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, SortState.New, SearchField, out videosCount));
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
                        this.Dispose();
                        isSortedBy = SortState.New;
                    },
                    x => isSortedBy != SortState.New));
            }
        }
        private RelayCommandParametr sortByOldCommand;
        public RelayCommandParametr SortByOldCommand
        {
            get
            {
                return sortByOldCommand
                    ?? (sortByOldCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        PageCount = 1;
                        IsVisibleProgressBar = true;
                        Thread sort = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, SortState.Old, SearchField, out videosCount));
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
                        this.Dispose();
                        isSortedBy = SortState.Old;
                    },
                    x => isSortedBy != SortState.Old));
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
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, isSortedBy, SearchField, out videosCount));
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
                                Videos = new ObservableCollection<Video>(context.Videos.GetVideoByRange(PageCount, videoOnThePage, isSortedBy, SearchField, out videosCount));
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
        public WatchingViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
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
