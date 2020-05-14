using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class MarkRepository : IRepository<Mark>
    {
        private PlayerContext db;

        public MarkRepository(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(Mark item)
        {
            db.Marks.Add(item);
        }

        public void Delete(int id)
        {
            Mark mark = db.Marks.Find(id);
            if (mark != null)
                db.Marks.Remove(mark);
        }
        public void DeleteByUserId(int userId,int videoId)
        {
            Mark mark = db.Marks.Where(x=>x.UserId == userId && x.VideoId==videoId).First();
            if (mark != null)
                db.Marks.Remove(mark);
        }

        public IEnumerable<Mark> Find(Func<Mark, bool> predicate)
        {
            return db.Marks.Where(predicate).ToList();
        }

        public int CountMarkByType(TypeMark type,int videoId)
        {
            return db.Marks.Where(x =>x. VideoId==videoId && x.TypeMark == type).Count();
        }

        public Mark FindMarkByUserId(int userId,int videoId)
        {
            return db.Marks.Where(x => x.UserId == userId && x.VideoId == videoId).FirstOrDefault();
        }

        public Mark Get(int id)
        {
            return db.Marks.Find(id);
        }
        public IEnumerable<Mark> GetAll()
        {
            return db.Marks.AsNoTracking().Include(c => c.User).ToList();
        }

        public void Update(Mark item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
