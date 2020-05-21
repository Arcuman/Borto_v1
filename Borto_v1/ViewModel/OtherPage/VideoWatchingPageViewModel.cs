
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace Borto_v1
{
    public class VideoWatchingPageViewModel : ViewModelBase
    {
        #region Private members

        EFUnitOfWork context = new EFUnitOfWork();

        private IFrameNavigationService _navigationService;

        private Video video { get; set; }

        private User user;

        private byte[] videoImage;

        private string videoName;

        private string videoDescription;

        private byte[] userImage;

        private string userName;

        private string userNickName;

        private int countPositiveMark;

        private int countNegativeMark;

        private string maxDuration;

        private string uploadDate;

        private LikeState likeState;

        private ObservableCollection<Comment> comments;

        /// <summary>
        /// Is Owner Video 
        /// </summary>
        private bool isUserOwner;

        private string comment;

        private Thread loadedThread;

        private bool isVisibleEditNameIcon;

        private bool isFavorite;
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
        public int CountPositiveMark
        {
            get
            {
                return countPositiveMark;
            }
            set
            {
                if (countPositiveMark == value)
                {
                    return;
                }
                countPositiveMark = value;
                RaisePropertyChanged();
            }
        }
        public int CountNegativeMark
        {
            get
            {
                return countNegativeMark;
            }
            set
            {
                if (countNegativeMark == value)
                {
                    return;
                }
                countNegativeMark = value;
                RaisePropertyChanged();
            }
        }
        public string MaxDuration
        {
            get
            {
                return maxDuration;
            }
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
        public string UploadDate
        {
            get
            {
                return uploadDate;
            }
            set
            {
                if (uploadDate == value)
                {
                    return;
                }
                uploadDate = value;
                RaisePropertyChanged();
            }
        }
        public bool IsFavorite
        {
            get
            {
                return isFavorite;
            }
            set
            {
                if (isFavorite == value)
                {
                    return;
                }
                isFavorite = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Comment> Comments
        {
            get
            {
                return comments;
            }

            set
            {
                if (comments == value)
                {
                    return;
                }

                comments = value;
                RaisePropertyChanged();
            }
        }
        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                if (comment == value)
                {
                    return;
                }

                comment = value;
                RaisePropertyChanged();
            }
        }
        public bool IsVisibleEditNameIcon
        {
            get
            {
                return isVisibleEditNameIcon;
            }
            set
            {
                if (isVisibleEditNameIcon == value)
                {
                    return;
                }
                isVisibleEditNameIcon = value;
                RaisePropertyChanged();
            }
        }
        public bool IsUserOwner
        {
            get
            {
                return isUserOwner;
            }
            set
            {
                if (isUserOwner == value)
                {
                    return;
                }
                isUserOwner = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands 

        private RelayCommandParametr likeVideoCommand;
        public RelayCommandParametr LikeVideoCommand
        {
            get
            {
                return likeVideoCommand
                    ?? (likeVideoCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        try
                        {
                            if (likeState == LikeState.None)
                            {

                                Mark mark = new Mark()
                                {
                                    VideoId = Video.IdVideo,
                                    UserId = SimpleIoc.Default.GetInstance<MainViewModel>().User.IdUser,
                                    TypeMark = TypeMark.Positive
                                };
                                context.Marks.Create(mark);
                                context.Save();
                                CountPositiveMark++;
                                likeState = LikeState.Like;
                            }
                            else
                            {
                                context.Marks.DeleteByUserId(SimpleIoc.Default.GetInstance<MainViewModel>().User.IdUser, Video.IdVideo);
                                context.Save();
                                CountPositiveMark--;
                                likeState = LikeState.None;
                            }
                        }
                        catch (Exception ex)
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                    },
                    (x) =>
                    likeState == LikeState.Like || likeState == LikeState.None));
            }
        }
        private RelayCommandParametr dislikeVideoCommand;
        public RelayCommandParametr DislikeVideoCommand
        {
            get
            {
                return dislikeVideoCommand
                    ?? (dislikeVideoCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        try
                        {
                            if (likeState == LikeState.None)
                            {
                                Mark mark = new Mark()
                                {
                                    VideoId = Video.IdVideo,
                                    UserId = user.IdUser,
                                    TypeMark = TypeMark.Negative
                                };
                                context.Marks.Create(mark);
                                context.Save();
                                CountNegativeMark++;
                                likeState = LikeState.Dislike;
                            }
                            else
                            {
                                context.Marks.DeleteByUserId(user.IdUser, Video.IdVideo);
                                context.Save();
                                CountNegativeMark--;
                                likeState = LikeState.None;
                            }
                        }
                        catch (Exception ex)
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                    },
                    (x) =>
                    likeState == LikeState.Dislike || likeState == LikeState.None));
            }
        }

        /// <summary>
        /// Leave comment
        /// </summary>
        private RelayCommandParametr sendCommentCommand;
        public RelayCommandParametr SendCommentCommand
        {
            get
            {
                return sendCommentCommand
                    ?? (sendCommentCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        try
                        {
                            Comment comment = new Comment()
                            {
                                CommentMessage = Comment,
                                VideoId = Video.IdVideo,
                                PostDate = DateTime.Now,
                                UserId = user.IdUser
                            };
                            context.Comments.Create(comment);

                            context.Save();

                            Comments = new ObservableCollection<Comment>(context.Comments.GetAllByVideo(Video.IdVideo));

                            Comment = string.Empty;
                        }
                        catch (Exception ex)
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                    },
                    (x) =>
                    !String.IsNullOrWhiteSpace(Comment) && IsVisibleEditNameIcon));
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

                        Initialize();

                        loadedThread = new Thread(() =>
                        {
                            try
                            {
                                Loaded();
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                                SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                            }
                        });
                        loadedThread.IsBackground = true;
                        loadedThread.Start();

                    }));
            }
        }

        private RelayCommandParametr changeNameCommand;
        public RelayCommandParametr ChangeNameCommand
        {
            get
            {
                return changeNameCommand
                    ?? (changeNameCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleEditNameIcon = false;

                    }));
            }
        }
        private RelayCommandParametr saveChangeNameCommand;
        public RelayCommandParametr SaveChangeNameCommand
        {
            get
            {
                return saveChangeNameCommand
                    ?? (saveChangeNameCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        try
                        {
                            Video.Name = VideoName;

                            Video.Description = VideoDescription;

                            context.Videos.Update(Video);
                            context.Save();
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Data saved successfully ";
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                        catch (Exception ex)
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                        IsVisibleEditNameIcon = true;
                    }));
            }
        }
        private RelayCommandParametr cancelNameCommand;
        public RelayCommandParametr CancelNameCommand
        {
            get
            {
                return cancelNameCommand
                    ?? (cancelNameCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        VideoName = Video.Name;
                        VideoDescription = Video.Description;
                        IsVisibleEditNameIcon = true;
                    }));
            }
        }
        private RelayCommandParametr addToFavoriteCommand;
        public RelayCommandParametr AddToFavoriteCommand
        {
            get
            {
                return addToFavoriteCommand
                    ?? (addToFavoriteCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        Thread temp;
                        if (!IsFavorite)
                        {
                            FavoriteVideo favorite = new FavoriteVideo()
                            {
                                UserId = user.IdUser,
                                VideoId = Video.IdVideo
                            };
                            temp = new Thread(() =>
                             {
                                 try
                                 {
                                     context.FavoriteVideos.Create(favorite);
                                     context.Save();
                                     SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Added to favorites!";
                                     SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                                     IsFavorite = true;
                                 }
                                 catch (Exception ex)
                                 {
                                     SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                                     SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                                 }
                             });

                        }
                        else
                        {
                            temp = new Thread(() =>
                            {
                                try
                                {
                                    context.FavoriteVideos.DeleteByUserId(user.IdUser, Video.IdVideo);
                                    context.Save();
                                    SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Removed from favorite!";
                                    SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                                    IsFavorite = false;
                                }
                                catch (Exception ex)
                                {
                                    SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Server error: " + ex.Message;
                                    SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                                }
                            });
                        }
                        temp.IsBackground = true;
                        temp.Start();
                    }));
            }
        }

        #endregion

        #region ctor
        public VideoWatchingPageViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            Video = navigationService.Parameter as Video;
            VideoImage = Video.Image;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Not async load info 
        /// </summary>
        public void Initialize()
        {
            user = SimpleIoc.Default.GetInstance<MainViewModel>().User;

            IsVisibleEditNameIcon = true;

            VideoName = Video.Name;

            IsUserOwner = Video.UserId == user.IdUser;

            VideoDescription = Video.Description;

            UserName = Video.User.Name;

            UserNickName = Video.User.NickName;

            UserImage = Video.User.Image;

            MaxDuration = Video.Duration;

            UploadDate = Video.UploadDate.ToShortDateString();

        }
        /// <summary>
        /// Async load info about comment and other...
        /// </summary>
        public void Loaded()
        {
            FavoriteVideo favvideo = context.FavoriteVideos.FindMarkByUserId(user.IdUser, Video.IdVideo);

            IsFavorite = favvideo != null ? true : false;

            CountNegativeMark = context.Marks.CountMarkByType(TypeMark.Negative, Video.IdVideo);

            CountPositiveMark = context.Marks.CountMarkByType(TypeMark.Positive, Video.IdVideo);

            Mark mark = context.Marks.FindMarkByUserId(user.IdUser, Video.IdVideo);
            if (mark == null)
            {
                likeState = LikeState.None;
            }
            else
            {
                if (mark.TypeMark == TypeMark.Positive)
                    likeState = LikeState.Like;
                else
                    likeState = LikeState.Dislike;
            }

            Comments = new ObservableCollection<Comment>(context.Comments.GetAllByVideo(Video.IdVideo));
        }

        #endregion
    }
}
