using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class NotificationViewModel : ViewModelBase
    {
        #region Private Fields

        private IFrameNavigationService _navigationService;

        private User user;

        EFUnitOfWork context = new EFUnitOfWork();

        private Thread loadedThread;

        private ObservableCollection<Notification> notifications;

        private bool isVisibleProgressBar;

        private bool isListNull;

        #endregion

        #region Public Fields

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return notifications;
            }
            set
            {
                if (notifications == value)
                {
                    return;
                }
                notifications = value;
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
        public bool IsListNull
        {
            get
            {
                return isListNull;
            }
            set
            {
                if (isListNull == value)
                {
                    return;
                }
                isListNull = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Command

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
                        IsListNull = false;
                        loadedThread = new Thread(() =>
                        {
                            try
                            {

                                Notifications = new ObservableCollection<Notification>(context.Notifications.GetAll(user.IdUser));
                                if (Notifications.Count() == 0)
                                    IsListNull = true;
                                else
                                    IsListNull = false;
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

        private RelayCommandParametr _viewWatchingPageCommand;
        public RelayCommandParametr ViewWatchingPageCommand
        {
            get
            {
                return _viewWatchingPageCommand
                    ?? (_viewWatchingPageCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        var temp = obj as Notification;
                        _navigationService.NavigateTo("VideoWatching", temp.Video);
                    }));
            }
        }
        private RelayCommandParametr _accountpageCommand;
        public RelayCommandParametr AccountpageCommand
        {
            get
            {
                return _accountpageCommand
                    ?? (_accountpageCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        var temp = obj as Notification;
                        _navigationService.NavigateTo("UserAccount", temp.Sender);

                    }));
            }
        }
         private RelayCommandParametr deleteCommand;
        public RelayCommandParametr DeleteCommand
        {
            get
            {
                return deleteCommand
                    ?? (deleteCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        var temp = obj as Notification;
                        context.Notifications.Delete(temp.NotificationId);
                        Notifications.Remove(temp);
                        if(Notifications.Count == 0)
                        {
                            IsListNull = true;
                        }
                    }));
            }
        }

        #endregion

        #region ctor 

        public NotificationViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            user = SimpleIoc.Default.GetInstance<MainViewModel>().User;
        }

        #endregion
    }
}
