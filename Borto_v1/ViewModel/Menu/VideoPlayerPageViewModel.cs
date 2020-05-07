
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class VideoPlayerPageViewModel : ViewModelBase
    {
        #region Private members 

        private string name;
        
        private bool mediaPlayerIsPlaying = false;
        /// <summary>
        /// is Full screen
        /// </summary>
        private bool isFullScreen = false;
        /// <summary>
        /// URI of selected video
        /// </summary>
        private Uri selectedVideo;
       
        private bool isVisible = true;
        /// <summary>
        /// is buttons play and pause are visible
        /// </summary>
        private bool isClickable = false;

        /// <summary>
        /// Duration of the Video
        /// </summary>
        private double maxDuration;
         /// <summary>
        /// Position of the video
        /// </summary>
        private TimeSpan position;


        #endregion

        #region Public Members
        /// <summary>
        /// Name of Video
        /// </summary>
        public string Name
        {
            get { return name; }

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
        /// <summary>
        /// URI of selected video
        /// </summary>
        public Uri SelectedVideo
        {
            get { return selectedVideo; }

            set
            {
                if (selectedVideo == value)
                {
                    return;
                }
                selectedVideo = value;
                string temp = selectedVideo.OriginalString.Split('\\').Last();
                isClickable = true;
                Name = selectedVideo.OriginalString.Substring(selectedVideo.OriginalString.LastIndexOf('\\') + 1);
                var inputFile = new MediaFile {Filename = selectedVideo.OriginalString };
                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);            
                }
                MaxDuration = inputFile.Metadata.Duration.TotalSeconds;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// is interface visible
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }

            set
            {
                if (isVisible == value)
                {
                    return;
                }
                isVisible = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        ///Position of the video
        /// </summary>
        public TimeSpan Position
        {
            get { return position; }

            set
            {
                if (position == value)
                {
                    return;
                }
                position = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Is Media player playing now
        /// </summary>
        public bool MediaPlayerIsPlaying
        {
            get { return mediaPlayerIsPlaying; }

            set
            {
                if (mediaPlayerIsPlaying == value)
                {
                    return;
                }
                mediaPlayerIsPlaying = value;
                RaisePropertyChanged();
            }
        }
         /// <summary>
        /// Duration of the Video
        /// </summary>
        public double MaxDuration
        {
            get { return maxDuration; }

            set
            {
                if (maxDuration == value)
                {
                    return;
                }
                maxDuration = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private RelayCommand _chooseVideoCommand;
        public RelayCommand ChooseVideoCommand
        {
            get
            {
                return _chooseVideoCommand
                    ?? (_chooseVideoCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this,"Choose"));
                    }));
            }
        }
        /// <summary>
        ///playing/pause video
        /// </summary>
        private RelayCommandParametr _playpauseVideoCommand;
        public RelayCommandParametr PlayPauseVideoCommand
        {
            get
            {
                return _playpauseVideoCommand
                    ?? (_playpauseVideoCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        if (MediaPlayerIsPlaying == false)
                        {
                            MediaPlayerIsPlaying = true;
                            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Play"));
                        }
                        else
                        {
                            MediaPlayerIsPlaying = false;
                            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Pause"));
                        }
                    },
                    (x)=> this.isClickable));
            }
        }
        /// <summary>
        ///playing/pause video
        /// </summary>
        private RelayCommandParametr _showhideInterfaceCommand;
        public RelayCommandParametr ShowhideInterfaceCommand
        {
            get
            {
                return _showhideInterfaceCommand
                    ?? (_showhideInterfaceCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        IsVisible = !IsVisible;
                    }));
            }
        }
          /// <summary>
        ///+5 sec -5 sec position video
        /// </summary>
        private RelayCommandParametr _editPositonCommand;
        public RelayCommandParametr EditPositonCommand
        {
            get
            {
                return _editPositonCommand
                    ?? (_editPositonCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        if (obj.ToString() == "Right")
                        {
                            Position += TimeSpan.FromSeconds(5);
                        }
                        else if (obj.ToString() == "Left")
                        {
                            Position -= TimeSpan.FromSeconds(5);
                        }
                    }));
            }
        }

         /// <summary>
        ///Fullscreen/Fullscreen video
        /// </summary>
        private RelayCommandParametr _fullscreenVideoCommand;
        public RelayCommandParametr FullscreenVideoCommand
        {
            get
            {
                return _fullscreenVideoCommand
                    ?? (_fullscreenVideoCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        //If command was invoked by key Esc and it isn't fullscreen
                        if (!isFullScreen && obj != null)
                        {
                            return;
                        }
                        if (isFullScreen)
                        {
                            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NotFullScreen"));
                        }
                        else if(!isFullScreen)
                        {
                            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("FullScreen"));
                        }
                        isFullScreen = !isFullScreen;
                    }));
            }
        }

        #endregion

        #region ctor
        public VideoPlayerPageViewModel()
        {
            Name = "Test Name";
        }

        #endregion
    }
}
