using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Borto_v1
{
    public class UserRepository : IRepository<User>
    {

        public UserRepository(PlayerContext context)
        {
        }

        public void Create(User item)
        {
            using (var db = new PlayerContext())
            {
                db.Users.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                User user = db.Users.Find(id);
                if (user != null)
                    db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.Users.AsNoTracking().Where(predicate).ToList();
            }
        }

        public User Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.Users.Find(id);
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.Users.AsNoTracking();
            }
        }

        public void Update(User item)
        {
            try
            {
                using (var db = new PlayerContext())
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Check there is a user with this login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool IsExist(string login)
        {
            using (var db = new PlayerContext())
            {
                User tmp = db.Users.AsNoTracking().FirstOrDefault(x => x.Login.Equals(login));
                return tmp != null ? true : false;
            }
        }
        /// <summary>
        /// Check there is a user with this login and password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsUser(string login, string password)
        {
            using (var db = new PlayerContext())
            {
                User tmp = db.Users.AsNoTracking().FirstOrDefault(x => x.Login.Equals(login) && x.Password == password);
                return tmp != null ? true : false;
            }
        }
        /// <summary>
        /// FInd User by login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public User GetUsersByLogin(string login)
        {
            using (var db = new PlayerContext())
            {
                return db.Users.AsNoTracking().FirstOrDefault(x => x.Login == login);
            }
        }
    }
}
