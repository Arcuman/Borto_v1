
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Borto_v1
{
    public class VideoRepository : IRepository<Video>
    {
        private PlayerContext db;

        public VideoRepository(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(Video item)
        {
            db.Videos.Add(item);
        }

        public void Delete(int id)
        {
            Video video = db.Videos.Find(id);
            if (video != null)
                db.Videos.Remove(video);
        }

        public IEnumerable<Video> Find(Func<Video, bool> predicate)
        {
            return db.Videos.Where(predicate).ToList();
        }
        public IEnumerable<Video> FindByUserId(int UserId)
        {
            return db.Set<Video>().AsNoTracking().Where(c => c.UserId == UserId).ToList();
        }

        public Video Get(int id)
        {
            return db.Videos.Find(id);
        }

        public IEnumerable<Video> GetAll()
        {
            return db.Videos.AsNoTracking().Include(c => c.User).ToList();
        }

        public void Update(Video item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
       

    }
}
