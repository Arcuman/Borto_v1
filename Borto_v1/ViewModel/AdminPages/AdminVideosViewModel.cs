using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                        if (String.IsNullOrWhiteSpace(TitleSearch)
                                && String.IsNullOrWhiteSpace(DateSearch)
                                && String.IsNullOrWhiteSpace(UserSearch))
                        {
                            Videos = new ObservableCollection<Video>(context.Videos.GetAll());
                        }
                        else if (!String.IsNullOrWhiteSpace(DateSearch))
                        {
                            Videos = new ObservableCollection<Video>(Videos.Where(x => x.Name.Contains(TitleSearch)
                                                          && x.UploadDate.ToShortDateString() == DateSearch
                                                          && x.User.NickName.Contains(UserSearch)).ToList());
                        }
                        else
                        {
                            Videos = new ObservableCollection<Video>(Videos.Where(x => x.Name.Contains(TitleSearch)
                                                         && x.User.NickName.Contains(UserSearch)).ToList());
                        }
                    }));
            }
        }

        private RelayCommandParametr deleteCommentCommand;
        public RelayCommandParametr DeleteCommentCommand
        {
            get
            {
                return deleteCommentCommand
                    ?? (deleteCommentCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        AzureHelper azureHelper = new AzureHelper();
                        azureHelper.delete_FromBlob(SelectedVideo.Path);
                        context.Videos.Delete(SelectedVideo.IdVideo);
                        context.Save();
                        Videos.Remove(SelectedVideo);
                    },
                    x => SelectedVideo != null));
            }
        }
        #endregion

        #region ctor

        public AdminVideosViewModel()
        {
            Videos = new ObservableCollection<Video>(context.Videos.GetAll());

            DateSearch = TitleSearch  = UserSearch = String.Empty;

        }

        #endregion

    }
}
