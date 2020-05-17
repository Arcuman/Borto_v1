
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Borto_v1
{

    public class WatchingViewModel : ViewModelBase,IDisposable
    {
        #region Private members 
        EFUnitOfWork context = new EFUnitOfWork();
        private ObservableCollection<Video> videos;
        private IFrameNavigationService _navigationService;

        private Video selectedVideo;

        private string searchField;

        private SortState isSortedBy;

        private Thread loadedThread;

        private bool isVisibleProgressBar;
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
                        _navigationService.NavigateTo("Download",obj);
                    }));
            }
        }
        private RelayCommandParametr unloadedCommand;
        public RelayCommandParametr UnloadedCommand
        {
            get
            {
                return unloadedCommand
                    ?? (unloadedCommand = new RelayCommandParametr(
                    obj =>
                    {
                        if (loadedThread.IsAlive)
                            loadedThread.Abort();
                        this.Dispose();
                        videos = null;
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
                        loadedThread = new Thread(() =>
                        {
                            Videos = new ObservableCollection<Video>(context.Videos.GetAll());
                            isSortedBy = SortState.None;
                            IsVisibleProgressBar = false;
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
                        _navigationService.NavigateTo("VideoWatching",SelectedVideo);
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
                        if (!String.IsNullOrWhiteSpace(SearchField))
                        {
                            Videos = new ObservableCollection<Video>(Videos.Where(x => x.Name.Contains(SearchField)));
                        }
                        else {
                            Videos = new ObservableCollection<Video>(context.Videos.GetAll());
                        }
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
                        
                        Videos = new ObservableCollection<Video>(Videos.OrderByDescending(x => x.MaxDuration));
                        this.Dispose();
                        isSortedBy = SortState.Long;
                    },
                    x=> isSortedBy != SortState.Long));
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
                        Videos = new ObservableCollection<Video>(Videos.OrderBy(x => x.MaxDuration));
                        this.Dispose();
                        isSortedBy = SortState.Short;
                    },
                    x=> isSortedBy != SortState.Short));
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
                        Videos = new ObservableCollection<Video>(Videos.OrderByDescending(x => x.UploadDate));
                        this.Dispose();
                        isSortedBy = SortState.New;
                    },
                    x=> isSortedBy != SortState.New));
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
                        Videos = new ObservableCollection<Video>(Videos.OrderBy(x => x.UploadDate));
                        this.Dispose();
                        isSortedBy = SortState.Old;
                    },
                    x=> isSortedBy != SortState.Old));
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
