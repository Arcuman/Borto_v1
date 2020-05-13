using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Borto_v1
{
    public class UserRepository : IRepository<User>
    {
        private PlayerContext db;

        public UserRepository(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Delete(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }

        public void Update(User item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
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
            User tmp = db.Users.FirstOrDefault(x => x.Login == login);
            return tmp != null ? true : false;
        }
        /// <summary>
        /// Check there is a user with this login and password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsUser(string login, string password)
        {
            User tmp = db.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            return tmp != null ? true : false;
        }
        /// <summary>
        /// FInd User by login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public User GetUsersByLogin(string login)
        {
            return db.Users.FirstOrDefault(x => x.Login == login);
        }
    }
}
