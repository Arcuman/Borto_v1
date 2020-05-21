
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Drawing;
using System.IO;
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

        private Thread uploadThread;

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
                            uploadThread = new Thread(() =>
                            {
                                try
                                {
                                    Upload();
                                }
                                catch (Exception ex)
                                {
                                    SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                                    SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                                }
                            });
                            uploadThread.IsBackground = true;
                            uploadThread.Start();

                        }
                    },
                    (x)=> !String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(PathVideo)));
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
                        uploadThread.Abort();
                        IsVisibleProgressBar = false;
                    }));
            }
        }


        #endregion
        #region ctor
        public UploadViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            IsVisibleProgressBar = false;
            if (!IsVisibleProgressBar)
            {
                SetImage();

                PathVideo = "Choose Video";

                Name = String.Empty;

                Description = String.Empty;
            }
        }
        #endregion

        #region Helpers

        private void Upload()
        {
            maxDuration = Video.GetMaxDuration(PathVideo);

            AzureHelper helper = new AzureHelper();

            serverfilepath = helper.upload_ToBlob(PathVideo, Name);

            user = SimpleIoc.Default.GetInstance<MainViewModel>().User;

            Video video = new Video(Name, Description, image, user, serverfilepath, maxDuration);

            context.Videos.Create(video);

            context.Save();

            IsVisibleProgressBar = false;

            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Your video uploaded!";
            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;

            Name = Description = PathVideo = string.Empty;
        }

        private void SetImage()
        {

            BitmapImage img = new BitmapImage(new Uri("pack://application:,,,/Assets/camera.jpg"));

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(img));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                image = ms.ToArray();
            }
        }

        #endregion
    }

}
