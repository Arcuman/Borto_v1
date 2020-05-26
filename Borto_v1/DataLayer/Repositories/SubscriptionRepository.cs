using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Forms;

namespace Borto_v1
{
    public class SubscriptionRepository : IRepository<Subscription>
    {
        public SubscriptionRepository(PlayerContext context)
        {
        }
        public void Create(Subscription item)
        {
            using (var db = new PlayerContext())
            {
                db.Subscriptions.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                Subscription favvideo = db.Subscriptions.Find(id);
                if (favvideo != null)
                {
                    db.Subscriptions.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public void Delete(int userId, int subUserId)
        {
            using (var db = new PlayerContext())
            {
                Subscription favvideo = db.Subscriptions.Where(x => x.UserId == userId && x.UserSubId == subUserId).FirstOrDefault();
                if (favvideo != null)
                {
                    db.Subscriptions.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<Subscription> Find(Func<Subscription, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.Subscriptions.Where(predicate).ToList();
            }
        }
        public List<User> GetUsersByUserId(int userId)
        {
            using (var db = new PlayerContext())
            {
                var temp = db.Subscriptions.Where(x => x.UserId == userId).Select(x => x.UserSubId).ToList();
                List<User> users = new List<User>();
                foreach (var item in temp)
                {
                    var user = db.Users.Where(x => x.IdUser == item).FirstOrDefault();
                    if (user != null)
                        users.Add(user);
                };
                return users;
            }
        }

        public Subscription Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.Subscriptions.Find(id);
            }
        }

        public bool IsExist(int userId, int subUserId)
        {
            using (var db = new PlayerContext())
            {
                if (db.Subscriptions.Where(x => x.UserId == userId && x.UserSubId == subUserId).FirstOrDefault() == null)
                {
                    return false;
                }
                return true;
            }
        }
        public int Count(int userId)
        {
            using (var db = new PlayerContext())
            {
                return db.Subscriptions.Where(x => x.UserSubId == userId).Count();
            }
        }

        public IEnumerable<Subscription> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.Subscriptions.AsNoTracking().Include(c => c.User).ToList();
            }
        }

        public void Update(Subscription item)
        {
            using (var db = new PlayerContext())
            {
                db.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
