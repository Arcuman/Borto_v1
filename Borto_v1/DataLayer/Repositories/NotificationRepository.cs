using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Borto_v1
{
    public class NotificationRepository : IRepository<Notification>
    {
        public NotificationRepository(PlayerContext context)
        {
        }

        public void Create(Notification item)
        {
            using (var db = new PlayerContext())
            {
                db.Notifications.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                Notification favvideo = db.Notifications.Find(id);
                if (favvideo != null)
                {
                    db.Notifications.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<Notification> Find(Func<Notification, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.Notifications.Where(predicate).ToList();
            }
        }

        public Notification Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.Notifications.Find(id);
            }
        }

        public IEnumerable<Notification> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.Notifications.AsNoTracking().Include(c => c.User).ToList();
            }
        } 
         public IEnumerable<Notification> GetAll(int userId)
        {
            using (var db = new PlayerContext())
            {
                return db.Notifications.AsNoTracking().Include(x => x.Video).Include(x=>x.Sender).Include(c => c.User).Where(x=>x.UserId==userId).ToList();
            }
        } 

        public void Update(Notification item)
        {
            using (var db = new PlayerContext())
            {
                db.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
