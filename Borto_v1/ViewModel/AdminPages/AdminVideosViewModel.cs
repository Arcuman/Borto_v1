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
    class AdminVideosViewModel : ViewModelBase
    {
        #region Private Fields

        EFUnitOfWork context = new EFUnitOfWork();

        private ObservableCollection<Video> videos;

        private Video selectedVideo;

        private string titleSearch;

        private string dateSearch;

        private string userSearch;

        private Thread loadedThread;

        private Thread seacrhThread;

        private bool isVisibleProgressBar;
        #endregion

        #region Public Fields

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
        public string TitleSearch
        {
            get
            {
                return titleSearch;
            }

            set
            {
                if (titleSearch == value)
                {
                    return;
                }

                titleSearch = value;
                RaisePropertyChanged();
            }
        }

        public string DateSearch
        {
            get
            {
                return dateSearch;
            }

            set
            {
                if (dateSearch == value)
                {
                    return;
                }

                dateSearch = value;
                RaisePropertyChanged();
            }
        }

        public string UserSearch
        {
            get
            {
                return userSearch;
            }

            set
            {
                if (userSearch == value)
                {
                    return;
                }

                userSearch = value;
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
        private RelayCommandParametr searchCommand;
        public RelayCommandParametr SearchCommand
        {
            get
            {
                return searchCommand
                    ?? (searchCommand = new RelayCommandParametr(
                    (o) =>
                    {

                        IsVisibleProgressBar = true;
                        seacrhThread = new Thread(
                            () =>
                            {
                                try
                                {
                                    Search();
                                    IsVisibleProgressBar = false;
                                }
                                catch (Exception ex)
                                {
                                    SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                                    SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                                }
                            });
                        seacrhThread.IsBackground = true;
                        seacrhThread.Start();
                    }));
            }
        }

        private RelayCommandParametr deleteVideoCommand;
        public RelayCommandParametr DeleteVideoCommand
        {
            get
            {
                return deleteVideoCommand
                    ?? (deleteVideoCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        try
                        {
                            AzureHelper azureHelper = new AzureHelper();
                            azureHelper.delete_FromBlob(SelectedVideo.Path);
                            context.Videos.Delete(SelectedVideo.IdVideo);
                            context.Save();
                            Videos.Remove(SelectedVideo);
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.Video_Deleted;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                        catch (Exception ex)
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                    },
                    x => SelectedVideo != null));
            }
        }
        private RelayCommandParametr loadedCommand;
        public RelayCommandParametr LoadedCommand
        {
            get
            {
                return loadedCommand
                    ?? (loadedCommand = new RelayCommandParametr(
                    (o) =>
                    {

                        IsVisibleProgressBar = true;
                        loadedThread = new Thread(() =>
                        {
                            try
                            {
                                Videos = new ObservableCollection<Video>(context.Videos.GetAll());

                                IsVisibleProgressBar = false;
                            }
                            catch (Exception ex)
                            {
                                SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
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

        public AdminVideosViewModel()
        {

            DateSearch = TitleSearch = UserSearch = String.Empty;
        }

        #endregion

        #region Helpers


        public void Search()
        {
            if (String.IsNullOrWhiteSpace(TitleSearch)
                                   && String.IsNullOrWhiteSpace(DateSearch)
                                   && String.IsNullOrWhiteSpace(UserSearch))
            {
                Videos = new ObservableCollection<Video>(context.Videos.GetAll());
            }
            else 
            {
                Videos = new ObservableCollection<Video>(context.Videos.GetBySearch(DateSearch, TitleSearch, UserSearch));
            }
        }

        #endregion

    }
}
