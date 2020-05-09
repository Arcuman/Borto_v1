
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
   public class WatchingViewModel : ViewModelBase
    {
        #region Private members 
        EFUnitOfWork context = new EFUnitOfWork();
        private ObservableCollection<Video> videos;
        private IFrameNavigationService _navigationService;

        private Video selectedVideo;

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
         private RelayCommandParametr loadedCommand;
        public RelayCommandParametr LoadedCommand
        {
            get
            {
                return loadedCommand
                    ?? (loadedCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        videos = new ObservableCollection<Video>(context.Videos.GetAll());
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
        #endregion
    }


}
