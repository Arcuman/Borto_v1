using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class WatchLaterRepository : IRepository<WatchLater>
    {

        public WatchLaterRepository(PlayerContext context)
        {
        }

        public void Create(WatchLater item)
        {
            using (var db = new PlayerContext())
            {
                db.WatchLaters.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                WatchLater favvideo = db.WatchLaters.Find(id);
                if (favvideo != null)
                {
                    db.WatchLaters.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public void DeleteByUserId(int userId, int videoId)
        {
            using (var db = new PlayerContext())
            {
                WatchLater favvideo = db.WatchLaters.Where(x => x.UserId == userId && x.VideoId == videoId).First();
                if (favvideo != null)
                {
                    db.WatchLaters.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<WatchLater> Find(Func<WatchLater, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.WatchLaters.Where(predicate).ToList();
            }
        }
        public WatchLater FindMarkByUserId(int userId, int videoId)
        {
            using (var db = new PlayerContext())
            {
                return db.WatchLaters.AsNoTracking().Where(x => x.UserId == userId && x.VideoId == videoId).FirstOrDefault();
            }
        }
        public WatchLater Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.WatchLaters.Find(id);
            }
        }

        /// <summary>
        ///  Gets range of video start from (pageCount-1)*numberOfItems order by state, count Videos
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="numberOfItems"></param>
        /// <param name="state"></param>
        /// <param name="searchString"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<Video> GetVideoByRange(int pageCount, int numberOfItems, SortState state, string searchString,int UserId, out int count)
        {
            using (PlayerContext db = new PlayerContext())
            {
                    int skip = (pageCount - 1) * numberOfItems;
                    IEnumerable<Video> videos = null;
                    count = db.WatchLaters.AsNoTracking().Include(x => x.Video).Where(x => x.Video.Name.Contains(searchString) && x.UserId == UserId).Count();
                    switch (state)
                    {
                        case SortState.New:
                            {
                                var temp = db.WatchLaters
                                                     .Where(x => x.Video.Name.Contains(searchString) && x.UserId == UserId)
                                                     .OrderByDescending(x => x.Video.UploadDate)
                                                     .Skip(skip).Take(numberOfItems).Select(x => x.Video).ToList();
                                foreach (var item in temp)
                                {
                                    db.Entry(item).Reference("User").Load();
                                }
                                videos = temp;
                                break;
                            }
                        case SortState.Old:
                            {
                                var temp = db.WatchLaters
                                  .Where(x => x.Video.Name.Contains(searchString) && x.UserId == UserId)
                                   .OrderBy(x => x.Video.UploadDate)
                                   .Skip(skip).Take(numberOfItems).Select(x => x.Video).ToList();
                                foreach (var item in temp)
                                {
                                    db.Entry(item).Reference("User").Load();
                                }
                                videos = temp;
                                break;
                            }
                        case SortState.Long:
                            {

                                var temp = db.WatchLaters
                                     .Where(x => x.Video.Name.Contains(searchString) && x.UserId == UserId)
                                     .OrderByDescending(x => x.Video.MaxDuration)
                                     .Skip(skip).Take(numberOfItems).Select(x => x.Video).ToList();
                                foreach (var item in temp)
                                {
                                    db.Entry(item).Reference("User").Load();
                                }
                                videos = temp;
                                break;
                            }
                        case SortState.Short:
                            {
                                var temp = db.WatchLaters
                                  .Where(x => x.Video.Name.Contains(searchString) && x.UserId == UserId)
                                  .OrderBy(x => x.Video.MaxDuration)
                                  .Skip(skip).Take(numberOfItems).Select(x => x.Video).ToList();
                                foreach (var item in temp)
                                {
                                    db.Entry(item).Reference("User").Load();
                                }
                                videos = temp;
                                break;
                            }
                    }
                    return videos;
            }
        }

        public IEnumerable<WatchLater> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.WatchLaters.AsNoTracking().Include(c => c.User).ToList();
            }
        }

        public void Update(WatchLater item)
        {
            using (var db = new PlayerContext())
            {
                db.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
