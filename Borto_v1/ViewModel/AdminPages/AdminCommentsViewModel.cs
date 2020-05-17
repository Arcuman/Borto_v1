using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                        if (String.IsNullOrWhiteSpace(CommentSearch)
                                && String.IsNullOrWhiteSpace(DateSearch)
                                && String.IsNullOrWhiteSpace(VideoSearch)
                                && String.IsNullOrWhiteSpace(UserSearch))
                        {
                            Comments = new ObservableCollection<Comment>(context.Comments.GetAll());
                        }
                        else if (!String.IsNullOrWhiteSpace(DateSearch))
                        {
                            Comments = new ObservableCollection<Comment>(Comments.Where(x => x.CommentMessage.Contains(CommentSearch)
                                                          && x.PostDate.ToShortDateString() == DateSearch
                                                          && x.Video.Name.Contains(VideoSearch)
                                                          && x.User.NickName.Contains(UserSearch)).ToList());
                        }
                        else
                        {
                            Comments = new ObservableCollection<Comment>(Comments.Where(x => x.CommentMessage.Contains(CommentSearch)
                                                          && x.Video.Name.Contains(VideoSearch)
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

                        context.Comments.Delete(SelectedComment.IdComment);
                        context.Save();
                        Comments.Remove(SelectedComment);
                    },
                    x => SelectedComment != null));
            }
        }

        #endregion

        #region ctor

        public AdminCommentsViewModel()
        {
            Comments = new ObservableCollection<Comment>(context.Comments.GetAll());

            DateSearch = CommentSearch = VideoSearch = UserSearch = String.Empty;
        }

        #endregion

    }
}
