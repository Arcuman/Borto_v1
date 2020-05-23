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
    class AdminCommentsViewModel : ViewModelBase
    {
        #region Private Fields

        EFUnitOfWork context = new EFUnitOfWork();

        private ObservableCollection<Comment> comments;

        private Comment selectedComment;

        private string commentSearch;

        private string dateSearch;

        private string videoSearch;

        private string userSearch;

        private Thread loadedThread;

        private Thread seacrhThread;

        private bool isVisibleProgressBar;

        #endregion

        #region Public Fields

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
        public Comment SelectedComment
        {
            get
            {
                return selectedComment;
            }

            set
            {
                if (selectedComment == value)
                {
                    return;
                }

                selectedComment = value;
                RaisePropertyChanged();
            }
        }
        public string CommentSearch
        {
            get
            {
                return commentSearch;
            }

            set
            {
                if (commentSearch == value)
                {
                    return;
                }

                commentSearch = value;
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
        public string VideoSearch
        {
            get
            {
                return videoSearch;
            }

            set
            {
                if (videoSearch == value)
                {
                    return;
                }

                videoSearch = value;
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
        private RelayCommandParametr deleteCommentCommand;
        public RelayCommandParametr DeleteCommentCommand
        {
            get
            {
                return deleteCommentCommand
                    ?? (deleteCommentCommand = new RelayCommandParametr(
                    (o) =>
                    {
                        try
                        {
                            context.Comments.Delete(SelectedComment.IdComment);
                            context.Save();
                            Comments.Remove(SelectedComment);
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.Comment_Deleted;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                        catch (Exception ex)
                        {
                            SimpleIoc.Default.GetInstance<MainViewModel>().Message = Properties.Resources.ServerError + ex.Message;
                            SimpleIoc.Default.GetInstance<MainViewModel>().IsOpenDialog = true;
                        }
                    },
                    x => SelectedComment != null));
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
                                Comments = new ObservableCollection<Comment>(context.Comments.GetAll());

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

        public AdminCommentsViewModel()
        {

            DateSearch = CommentSearch = VideoSearch = UserSearch = String.Empty;
        }

        #endregion


        #region Helpers

        public void Search()
        {
            if (String.IsNullOrWhiteSpace(CommentSearch)
                                   && String.IsNullOrWhiteSpace(DateSearch)
                                   && String.IsNullOrWhiteSpace(VideoSearch)
                                   && String.IsNullOrWhiteSpace(UserSearch))
            {
                Comments = new ObservableCollection<Comment>(context.Comments.GetAll());
            }
            else if (!String.IsNullOrWhiteSpace(DateSearch))
            {
                Comments = new ObservableCollection<Comment>(context.Comments.GetBySearch(DateSearch,VideoSearch,UserSearch,CommentSearch));
            }

        }

        #endregion
    }
}
