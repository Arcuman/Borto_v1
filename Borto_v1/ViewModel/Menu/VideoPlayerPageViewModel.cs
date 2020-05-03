using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
        #endregion

        #region Public Members
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
        private RelayCommand _playVideoCommand;
        public RelayCommand PlayVideoCommand
        {
            get
            {
                return _playVideoCommand
                    ?? (_playVideoCommand = new RelayCommand(
                    () =>
                    {
                        mediaPlayerIsPlaying = true;
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Play"));
                    }, !mediaPlayerIsPlaying));
            }
        }
        private RelayCommand _pauseVideoCommand;
        public RelayCommand PauseVideoCommand
        {
            get
            {
                return _pauseVideoCommand
                    ?? (_pauseVideoCommand = new RelayCommand(
                    () =>
                    {
                        mediaPlayerIsPlaying = true;
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Pause"));
                    }, mediaPlayerIsPlaying));
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
