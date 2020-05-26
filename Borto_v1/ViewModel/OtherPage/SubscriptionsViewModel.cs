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
    class SubscriptionsViewModel : ViewModelBase
    {
        #region Private Fields

        private IFrameNavigationService _navigationService;

        private User user;

        EFUnitOfWork context = new EFUnitOfWork();

        private Thread loadedThread;

        private ObservableCollection<User> users;

        private bool isVisibleProgressBar;

        private bool isListNull;
        #endregion

        #region Public Fields

        public ObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                if (users == value)
                {
                    return;
                }
                users = value;
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

                                Users = new ObservableCollection<User>(context.Subscription.GetUsersByUserId(user.IdUser));
                                if (Users.Count() == 0)
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
        private RelayCommandParametr accountPageCommand;
        public RelayCommandParametr AccountPageCommand
        {
            get
            {
                return accountPageCommand
                    ?? (accountPageCommand = new RelayCommandParametr(
                    (obj) =>
                    {
                        var user = obj as User;
                        _navigationService.NavigateTo("UserAccount", user);
                    }));
            }
        }
        #endregion

        #region ctor
        public SubscriptionsViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            user = SimpleIoc.Default.GetInstance<MainViewModel>().User;

        }
        #endregion
    }
}
