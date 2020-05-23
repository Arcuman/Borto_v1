using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class CommentRepostiroty : IRepository<Comment>
    {
        private PlayerContext db;

        public CommentRepostiroty(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(Comment item)
        {
            db.Comments.Add(item);
        }

        public void Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment != null)
                db.Comments.Remove(comment);
        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return db.Comments.Where(predicate).ToList();
        }

        public Comment Get(int id)
        {
            return db.Comments.Find(id);
        }
        public IEnumerable<Comment> GetBySearch(string DateSearch, string VideoSearch, string UserSearch, string CommentSearch)
        {

            if (!String.IsNullOrWhiteSpace(DateSearch))
            {
                DateTime searchFrom = Convert.ToDateTime(DateSearch);
                DateTime searchTo = searchFrom.AddDays(1);
                return db.Comments.AsNoTracking().Include(x=>x.User).Where(x =>
                                              x.PostDate >= searchFrom && x.PostDate <= searchTo
                                              && x.CommentMessage.Contains(CommentSearch)
                                              && x.Video.Name.Contains(VideoSearch)
                                              && x.User.NickName.Contains(UserSearch)).ToList();
            }
            else
            {
                DateTime searchFrom = Convert.ToDateTime(DateSearch);
                DateTime searchTo = searchFrom.AddDays(1);
                return db.Comments.AsNoTracking().Include(x => x.User).Where(x => x.CommentMessage.Contains(CommentSearch)
                                              && x.Video.Name.Contains(VideoSearch)
                                              && x.User.NickName.Contains(UserSearch)).ToList();
            }
        }
        public IEnumerable<Comment> GetAll()
        {
            return db.Comments.AsNoTracking().Include(c => c.User).Include(x => x.Video).ToList();
        }
        public IEnumerable<Comment> GetAllByVideo(int VideoID)
        {
            return db.Comments.AsNoTracking().Include(c => c.User).Where(x => x.VideoId == VideoID).OrderByDescending(x => x.PostDate).ToList();
        }

        public void Update(Comment item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
