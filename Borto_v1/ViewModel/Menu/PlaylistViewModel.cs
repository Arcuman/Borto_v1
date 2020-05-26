using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class PlaylistViewModel : ViewModelBase
    {
        #region  Private Fields
        private IFrameNavigationService _navigationService;

        private ObservableCollection<Playlist> playlist;

        private Playlist selectedPlaylist;

        private Thread loadedThread;

        private User user;

        private string name;

        EFUnitOfWork context = new EFUnitOfWork();

        private bool isVisibleProgressBar;

        private bool isVisibleAddFields;

        private byte[] image;
        #endregion

        #region Public Members

        public ObservableCollection<Playlist> Playlists
        {
            get
            {
                return playlist;
            }
            set
            {
                if (playlist == value)
                {
                    return;
                }
                playlist = value;
                RaisePropertyChanged();
            }
        }
        public Playlist SelectedPlaylist
        {
            get
            {
                return selectedPlaylist;
            }
            set
            {
                if (selectedPlaylist == value)
                {
                    return;
                }

                selectedPlaylist = value;
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
        public bool IsVisibleAddFields
        {
            get
            {
                return isVisibleAddFields;
            }
            set
            {
                if (isVisibleAddFields == value)
                {
                    return;
                }
                isVisibleAddFields = value;
                RaisePropertyChanged();
            }
        }

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
        #endregion

        #region Commands

        private RelayCommandParametr setPathtoImageCommand;
        public RelayCommandParametr SetPathtoImageCommand
        {
            get
            {
                return setPathtoImageCommand
                    ?? (setPathtoImageCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        Messenger.Default.Send<NotificationMessage>(new NotificationMessage(this, "ChooseImage"));
                    }));
            }
        }

        private RelayCommandParametr showAddFieldsCommand;
        public RelayCommandParametr ShowAddFieldsCommand
        {
            get
            {
                return showAddFieldsCommand
                    ?? (showAddFieldsCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        SetImage();
                        IsVisibleAddFields = true;
                    }));
            }
        }
        private RelayCommandParametr cancelAddCommand;
        public RelayCommandParametr CancelAddCommand
        {
            get
            {
                return cancelAddCommand
                    ?? (cancelAddCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleAddFields = false;
                        Name = null;
                        Image = null;
                    }));
            }
        }
        private RelayCommandParametr addPlaylistCommand;
        public RelayCommandParametr AddPlaylistCommand
        {
            get
            {
                return addPlaylistCommand
                    ?? (addPlaylistCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleAddFields = false;
                        Playlist playlist = new Playlist()
                        {
                            UserId = user.IdUser,
                            Name = Name,
                            Image = Image
                        };
                        Playlists.Add(playlist);
                        var addPlaylist = new Thread(() =>
                          {
                              context.Playlist.Create(playlist);
                              Name = null;
                          });
                        addPlaylist.IsBackground = true;
                        addPlaylist.Start();

                    },
                    x=> !String.IsNullOrEmpty(Name)));
            }
        }
        private RelayCommandParametr deletePlaylistCommand;
        public RelayCommandParametr DeletePlaylistCommand
        {
            get
            {
                return deletePlaylistCommand
                    ?? (deletePlaylistCommand = new RelayCommandParametr(
                    obj =>
                    {
                        SelectedPlaylist = obj as Playlist;
                        context.Playlist.Delete(SelectedPlaylist.IdPlaylist);
                        Playlists.Remove(SelectedPlaylist);
                    }));
            }
        }
        private RelayCommandParametr playlistVideosCommand;
        public RelayCommandParametr PlaylistVideosCommand
        {
            get
            {
                return playlistVideosCommand
                    ?? (playlistVideosCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        SelectedPlaylist = obj as Playlist;
                        _navigationService.NavigateTo("PlaylistVideos", SelectedPlaylist);
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
                        IsVisibleAddFields = false;
                        user = SimpleIoc.Default.GetInstance<MainViewModel>().User;
                        loadedThread = new Thread(() =>
                        {
                            try
                            {
                                Playlists = new ObservableCollection<Playlist>(context.Playlist.FindByUserId(user.IdUser));

                                IsVisibleProgressBar = false;
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;

                            }

                        });
                        loadedThread.IsBackground = true;
                        loadedThread.Start();

                    }));
            }
        }
        #endregion


        #region ctor

        public PlaylistViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region Helpers

        private void SetImage()
        {

            BitmapImage img = new BitmapImage(new Uri("pack://application:,,,/Assets/camera.jpg"));

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(img));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                Image = ms.ToArray();
            }
        }

        #endregion
    }
}
