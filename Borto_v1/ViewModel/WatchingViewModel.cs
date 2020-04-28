using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.ViewModel
{
   public class WatchingViewModel : ViewModelBase
    {
        private ObservableCollection<Video> videos = new ObservableCollection<Video>();
        private IFrameNavigationService _navigationService;

      
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
        } private RelayCommand _uploadpageCommand;
        public RelayCommand UploadPageCommand
        {
            get
            {
                return _uploadpageCommand
                    ?? (_uploadpageCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("Upload");
                    }));
            }
        }
        public WatchingViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Videos.Add(new Video("Video1","FunnyVideos", "/Assets/camera.jpg",new User("Anton")));
            Videos.Add(new Video("Video1","FunnyVideos", "/Assets/camera.jpg",new User("Anton")));
            Videos.Add(new Video("Video1","FunnyVideos", "/Assets/camera.jpg",new User("Anton")));
            Videos.Add(new Video("Video1","FunnyVideos", "/Assets/camera.jpg",new User("Anton")));
            Videos.Add(new Video("Video1","FunnyVideos", "/Assets/camera.jpg",new User("Anton")));
        }
    }

    
}
