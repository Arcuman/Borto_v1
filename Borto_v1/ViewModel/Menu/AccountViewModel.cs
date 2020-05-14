using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class AccountViewModel : ViewModelBase
    {
        #region Private Fields

        private IFrameNavigationService _navigationService;

        EFUnitOfWork context = new EFUnitOfWork();

        private User user;

        private string name;

        private string nickName;

        private string oldPassword;

        private string newPassword;

        private byte[] image;

        private bool isVisibleEditNameIcon;

        private bool isVisibleEditPasswrodIcon;

        private ObservableCollection<Video> videos;

        private Video selectedVideo;

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
        public string OldPassword
        {
            get
            {
                return oldPassword;
            }
            set
            {
                if (oldPassword == value)
                {
                    return;
                }
                oldPassword = value;
                RaisePropertyChanged();
            }
        }
        public string NewPassword
        {
            get
            {
                return newPassword;
            }
            set
            {
                if (newPassword == value)
                {
                    return;
                }
                newPassword = value;
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
        public bool IsVisibleEditPasswrodIcon
        {
            get
            {
                return isVisibleEditPasswrodIcon;
            }
            set
            {
                if (isVisibleEditPasswrodIcon == value)
                {
                    return;
                }
                isVisibleEditPasswrodIcon = value;
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
        #endregion

        #region Command

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
        private RelayCommandParametr changePasswordCommand;
        public RelayCommandParametr ChangePasswordCommand
        {
            get
            {
                return changePasswordCommand
                    ?? (changePasswordCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        IsVisibleEditPasswrodIcon = false;

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
                        User.Name = Name;

                        User.Image = Image;
                        User.NickName = NickName;
                        context.Users.Update(User);
                        context.Save();
                        IsVisibleEditNameIcon = true;
                    }));
            }
        }
        private RelayCommandParametr saveChangePasswordCommand;
        public RelayCommandParametr SaveChangePasswordCommand
        {
            get
            {
                return saveChangePasswordCommand
                    ?? (saveChangePasswordCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        if (context.Users.IsUser(User.Login, User.getHash(OldPassword)))
                        {
                            user.Password = User.getHash(NewPassword);
                            context.Users.Update(User);
                            context.Save();
                            IsVisibleEditPasswrodIcon = true;
                        }
                        else {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = "Incorrect old password";
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                    }));
            }
        }
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
        private RelayCommandParametr cancelNameCommand;
        public RelayCommandParametr CancelNameCommand
        {
            get
            {
                return cancelNameCommand
                    ?? (cancelNameCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        Name = User.Name;
                        NickName = User.NickName;
                        Image = User.Image;
                        IsVisibleEditNameIcon = true;
                    }));
            }
        }
        private RelayCommandParametr cancelPasswordCommand;
        public RelayCommandParametr CancelPasswordCommand
        {
            get
            {
                return cancelPasswordCommand
                    ?? (cancelPasswordCommand = new RelayCommandParametr(
                    (x) =>
                    {
                        OldPassword = string.Empty;
                        NewPassword = string.Empty;
                        IsVisibleEditPasswrodIcon = true;
                    }));
            }
        }

        #endregion

        #region ctor

        public AccountViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            User = SimpleIoc.Default.GetInstance<MainViewModel>().User;
            Name = User.Name;
            
            Image = User.Image;
            
            NickName = User.NickName;
            
            Videos = new ObservableCollection<Video>(context.Videos.FindByUserId(User.IdUser));

            IsVisibleEditNameIcon = true;

            IsVisibleEditPasswrodIcon = true;
        }

        #endregion
    }
}
