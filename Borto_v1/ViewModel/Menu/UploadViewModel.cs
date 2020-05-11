
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private BitmapImage pathImage;   

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
        #endregion

        #region Public members
        public BitmapImage PathImage
        {
            get
            {
                return pathImage;
            }
            set
            {
                if (pathImage == value)
                {
                    return;
                }

                pathImage = value;
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

        #endregion

        #region Commands
        private RelayCommand _setPathtoImageCommand;
        public RelayCommand SetPathtoImageCommand
        {
            get
            {
                

                return _setPathtoImageCommand
                    ?? (_setPathtoImageCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, "ChooseImage"));
                       
                    }));
            }
        }
        private RelayCommand _setPathtoVideoCommand;
        public RelayCommand SetPathtoVideoCommand
        {
            get
            {
                

                return _setPathtoVideoCommand
                    ?? (_setPathtoVideoCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, "ChooseVideo"));
                        
                    }));
            }
        }

        private RelayCommand uploadCommand;
        public RelayCommand UploadCommand
        {
            get
            {
                return uploadCommand
                    ?? (uploadCommand = new RelayCommand(
                    () =>
                    {
                        if (PathVideo != "Choose Video" && !String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(Description))
                        {

                            image = Video.BitMapToByteArray(PathImage);

                            maxDuration = Video.GetMaxDuration(PathVideo);

                            AzureHelper helper = new AzureHelper();

                            serverfilepath = helper.UploadBlobsInChunks(PathVideo, Name);

                            user = SimpleIoc.Default.GetInstance<MainViewModel>().User;

                            Video video = new Video(Name, Description, image, user, serverfilepath, maxDuration);

                            context.Videos.Create(video);

                            context.Save();
                        }
                    }));
            }
        }

        #endregion
        #region ctor
        public UploadViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            PathImage = new BitmapImage(new Uri("/Assets/camera.jpg",UriKind.RelativeOrAbsolute));
            PathVideo = "Choose Video";
        }
        #endregion
    }

}
