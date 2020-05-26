using System;
using System.Windows;

namespace Borto_v1
{
    public class EFUnitOfWork 
    {
        private PlayerContext db = new PlayerContext();

        private UserRepository userRepository;

        private VideoRepository videoRepository;

        private MarkRepository markRepository;

        private CommentRepostiroty commentRepository;

        private WatchLaterRepository WatchLaterRepostiroty;

        private PlaylistRepository playlistRepository;

        private PlaylistVideoRepository playlistVideoRepository;

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
                if (commentRepository == null)
                    commentRepository = new CommentRepostiroty(db);
                return commentRepository;
            }
        }
        public WatchLaterRepository WatchLaters
        {
            get
            {
                if (WatchLaterRepostiroty == null)
                    WatchLaterRepostiroty = new WatchLaterRepository(db);
                return WatchLaterRepostiroty;
            }
        }
         public PlaylistRepository Playlist
        {
            get
            {
                if (playlistRepository == null)
                    playlistRepository = new PlaylistRepository(db);
                return playlistRepository;
            }
        }
         public PlaylistVideoRepository PlaylistVideo
        {
            get
            {
                if (playlistVideoRepository == null)
                    playlistVideoRepository = new PlaylistVideoRepository(db);
                return playlistVideoRepository;
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
            Dispose(true);
            GC.WaitForPendingFinalizers();
        }

    }
}
