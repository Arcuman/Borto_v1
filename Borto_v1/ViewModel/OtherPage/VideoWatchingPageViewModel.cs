
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Borto_v1
{
    public class VideoWatchingPageViewModel : ViewModelBase
    {
        #region Private members
        private IFrameNavigationService _navigationService;

        private Video video { get; set; }

        private byte[] videoImage;

        private string videoName;

        private string videoDescription;

        private byte[] userImage;

        private string userName;

        private string userNickName;
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
        public byte[] UserImage
        {
            get
            {
                return userImage;
            }
            set
            {
                if (userImage == value)
                {
                    return;
                }
                userImage = value;
                RaisePropertyChanged();
            }
        }
        public byte[] VideoImage
        {
            get
            {
                return videoImage;
            }
            set
            {
                if (videoImage == value)
                {
                    return;
                }
                videoImage = value;
                RaisePropertyChanged();
            }
        }
        public string VideoName
        {
            get
            {
                return videoName;
            }
            set
            {
                if (videoName == value)
                {
                    return;
                }
                videoName = value;
                RaisePropertyChanged();
            }
        }
        public string VideoDescription
        {
            get
            {
                return videoDescription;
            }
            set
            {
                if (videoDescription == value)
                {
                    return;
                }
                videoDescription = value;
                RaisePropertyChanged();
            }
        }
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (userName == value)
                {
                    return;
                }
                userName = value;
                RaisePropertyChanged();
            }
        }
        public string UserNickName
        {
            get
            {
                return userNickName;
            }
            set
            {
                if (userNickName == value)
                {
                    return;
                }
                userNickName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands 

        #endregion

        #region ctor
        public VideoWatchingPageViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Video = navigationService.Parameter as Video;
            Initialize();
        }
        #endregion

        #region Helpers

        public void Initialize()
        {
            VideoImage = Video.Image;

            VideoName = Video.Name;

            VideoDescription = Video.Description;

            UserName = Video.User.Name;

            UserNickName = Video.User.NickName;

            UserImage = Video.User.Image;

        }

        #endregion
    }
}
