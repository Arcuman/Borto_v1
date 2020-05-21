using System;
using System.Windows;

namespace Borto_v1
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private PlayerContext db = new PlayerContext();

        private UserRepository userRepository;

        private VideoRepository videoRepository;

        private MarkRepository markRepository;

        private CommentRepostiroty commentRepostiroty;

        private FavoriteVideoRepository favoriteVideoRepostiroty;

        public UserRepository Users
        {
            get
            {

                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public VideoRepository Videos
        {
            get
            {
                if (videoRepository == null)
                    videoRepository = new VideoRepository(db);

                return videoRepository;
            }
        }
        public MarkRepository Marks
        {
            get
            {
                if (markRepository == null)
                    markRepository = new MarkRepository(db);
                return markRepository;
            }
        }
        public CommentRepostiroty Comments
        {
            get
            {
                if (commentRepostiroty == null)
                    commentRepostiroty = new CommentRepostiroty(db);
                return commentRepostiroty;
            }
        }
        public FavoriteVideoRepository FavoriteVideos
        {
            get
            {
                if (favoriteVideoRepostiroty == null)
                    favoriteVideoRepostiroty = new FavoriteVideoRepository(db);
                return favoriteVideoRepostiroty;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Dispose(true);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}
