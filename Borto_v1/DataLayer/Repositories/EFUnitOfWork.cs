using System;

namespace Borto_v1
{
    public class EFUnitOfWork: IUnitOfWork
    {
        private PlayerContext db = new PlayerContext();

        private UserRepository userRepository;

        private VideoRepository videoRepository;

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
