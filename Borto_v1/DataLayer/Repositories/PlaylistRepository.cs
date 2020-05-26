using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Borto_v1
{
    public class PlaylistRepository : IRepository<Playlist>
    {
        public PlaylistRepository(PlayerContext context)
        {
        }

        public void Create(Playlist item)
        {
            using (var db = new PlayerContext())
            {
                db.Playlists.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                Playlist favvideo = db.Playlists.Find(id);
                if (favvideo != null)
                {
                    db.Playlists.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<Playlist> Find(Func<Playlist, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.Playlists.Where(predicate).ToList();
            }
        }

        public Playlist Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.Playlists.Find(id);
            }
        }

        public IEnumerable<Playlist> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.Playlists.AsNoTracking().Include(c => c.User).ToList();
            }
        }
        public IEnumerable<Playlist> FindByUserId(int UserId)
        {
            using (var db = new PlayerContext())
            {
                return db.Playlists.AsNoTracking().Where(c => c.UserId == UserId).ToList();
            }
        }
        public void Update(Playlist item)
        {
            using (var db = new PlayerContext())
            {
                db.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
