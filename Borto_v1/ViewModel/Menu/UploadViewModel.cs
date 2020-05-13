
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class UploadViewModel : ViewModelBase
    {
        #region Private members 
        private IFrameNavigationService _navigationService;

        EFUnitOfWork context = new EFUnitOfWork();

        private string name;

        private User user;

        private string description;

        private string pathVideo;

        /// <summary>
        /// Duration of the video
        /// </summary>
        private double maxDuration;
        /// <summary>
        /// Path to the file on server
        /// </summary>
        private string serverfilepath;
        /// <summary>
        /// image for the store in database
        /// </summary>
        private byte[] image;

        private bool isVisibleProgressBar;

        #endregion

        #region Public members
        public byte[] Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image == value)
                {
                    return;
                }

                image = value;
                RaisePropertyChanged();
            }
        }
        public string PathVideo
        {
            get
            {
                return pathVideo;
            }
            set
            {
                if (pathVideo == value)
                {
                    return;
                }

                pathVideo = value;
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
         public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description == value)
                {
                    return;
                }
                description = value;
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
        private RelayCommandParametr _setPathtoImageCommand;
        public RelayCommandParametr SetPathtoImageCommand
        {
            get
            {
                

                return _setPathtoImageCommand
                    ?? (_setPathtoImageCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, "ChooseImage"));

                    },
                    (x) => IsVisibleProgressBar == false));
            }
        }
        private RelayCommandParametr _setPathtoVideoCommand;
        public RelayCommandParametr SetPathtoVideoCommand
        {
            get
            {
                

                return _setPathtoVideoCommand
                    ?? (_setPathtoVideoCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, "ChooseVideo"));

                    },
                    (x) => IsVisibleProgressBar == false));
            }
        }

        private RelayCommandParametr uploadCommand;
        public RelayCommandParametr UploadCommand
        {
            get
            {
                return uploadCommand
                    ?? (uploadCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        if (PathVideo != "Choose Video" && !String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(Description))
                        {
                            IsVisibleProgressBar = true;
                            ThreadPool.QueueUserWorkItem(
                            (o) =>
                            {
                                maxDuration = Video.GetMaxDuration(PathVideo);

                                AzureHelper helper = new AzureHelper();

                                serverfilepath = helper.upload_ToBlob(PathVideo, Name);

                                user = SimpleIoc.Default.GetInstance<MainViewModel>().User;

                                Video video = new Video(Name, Description, image, user, serverfilepath, maxDuration);

                                context.Videos.Create(video);

                                context.Save();

                            });

                            IsVisibleProgressBar = false;
                        }
                    },
                    (x)=> !String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(PathVideo)));
            }
        }

        #endregion
        #region ctor
        public UploadViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            if (!IsVisibleProgressBar)
            {
                Image img = System.Drawing.Image.FromFile(new Uri("../../Assets/camera.jpg", UriKind.RelativeOrAbsolute).OriginalString);

                image = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

                PathVideo = "Choose Video";

                Name = String.Empty;

                Description = String.Empty;
            }
        }
        #endregion
    }

}
