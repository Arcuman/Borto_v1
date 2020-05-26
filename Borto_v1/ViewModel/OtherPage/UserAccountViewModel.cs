using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Borto_v1
{
   public class UserAccountViewModel : ViewModelBase
    {
        #region Private Fields

        private IFrameNavigationService _navigationService;

        EFUnitOfWork context = new EFUnitOfWork();

        private User user;

        private string name;

        private string nickName;

        private byte[] image;

        private ObservableCollection<Video> videos;

        private Video selectedVideo;

        private Thread loadedThread;

        private bool isVisibleProgressBar;

        private bool isSubscribe;

        private int countSubscribers;

        #endregion

        #region Public Fields

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
        public int CountSubscribers
        {
            get
            {
                return countSubscribers;
            }
            set
            {
                if (countSubscribers == value)
                {
                    return;
                }
                countSubscribers = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSubscribe
        {
            get
            {
                return isSubscribe;
            }
            set
            {
                if (isSubscribe == value)
                {
                    return;
                }
                isSubscribe = value;
                RaisePropertyChanged();
            }
        }

        public string NickName
        {
            get
            {
                return nickName;
            }
            set
            {
                if (nickName == value)
                {
                    return;
                }
                nickName = value;
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
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (user == value)
                {
                    return;
                }
                user = value;
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

        #region Command

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
                        _navigationService.NavigateTo("VideoWatching", SelectedVideo);
                    }));
            }
        }
        private RelayCommandParametr subscribeCommand;
        public RelayCommandParametr SubscribeCommand
        {
            get
            {
                return subscribeCommand
                    ?? (subscribeCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        Subscription subscription = new Subscription()
                        {
                            UserId = SimpleIoc.Default.GetInstance<MainViewModel>().User.IdUser,
                            UserSubId = User.IdUser
                        };
                        if (IsSubscribe)
                        {
                            context.Subscription.Delete(SimpleIoc.Default.GetInstance<MainViewModel>().User.IdUser, User.IdUser);
                            IsSubscribe = false;
                            CountSubscribers--;
                        }
                        else
                        {
                            context.Subscription.Create(subscription); 
                            IsSubscribe = true;
                            CountSubscribers++;
                        }
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

                        Name = User.Name;

                        NickName = User.NickName;

                        loadedThread = new Thread(() =>
                        {
                            try
                            {
                                if (context.Subscription.IsExist(SimpleIoc.Default.GetInstance<MainViewModel>().User.IdUser, User.IdUser))
                                {
                                    IsSubscribe = true;
                                }
                                else
                                {
                                    IsSubscribe = false;
                                }

                                CountSubscribers = context.Subscription.Count(User.IdUser);

                                Image = User.Image;

                                Videos = new ObservableCollection<Video>(context.Videos.FindByUserId(User.IdUser));

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

        public UserAccountViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            User = navigationService.Parameter as User;
        }

        #endregion
    }
}
