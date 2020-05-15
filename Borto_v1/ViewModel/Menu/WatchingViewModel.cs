
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

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
                        this.Dispose();
                        videos = null;
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
                        Videos = new ObservableCollection<Video>(context.Videos.SearchByName(SearchField));
                    }));
            }
        } 
        #endregion

        #region ctor
        public WatchingViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            videos = new ObservableCollection<Video>(context.Videos.GetAll());
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            context.Dispose();
        }
        #endregion
    }


}
