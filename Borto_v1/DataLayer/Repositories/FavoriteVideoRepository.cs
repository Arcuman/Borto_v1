using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class FavoriteVideoRepository : IRepository<FavoriteVideo>
    {
        private PlayerContext db;

        public FavoriteVideoRepository(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(FavoriteVideo item)
        {
            db.FavoriteVideos.Add(item);
        }

        public void Delete(int id)
        {
            FavoriteVideo favvideo = db.FavoriteVideos.Find(id);
            if (favvideo != null)
                db.FavoriteVideos.Remove(favvideo);
        }

        public void DeleteByUserId(int userId, int videoId)
        {
            FavoriteVideo favvideo = db.FavoriteVideos.Where(x => x.UserId == userId && x.VideoId == videoId).First();
            if (favvideo != null)
                db.FavoriteVideos.Remove(favvideo);
        }

        public IEnumerable<FavoriteVideo> Find(Func<FavoriteVideo, bool> predicate)
        {
            return db.FavoriteVideos.Where(predicate).ToList();
        }
        public FavoriteVideo FindMarkByUserId(int userId, int videoId)
        {
            return db.FavoriteVideos.AsNoTracking().Where(x => x.UserId == userId && x.VideoId == videoId).FirstOrDefault();
        }
        public FavoriteVideo Get(int id)
        {
            return db.FavoriteVideos.Find(id);
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
                count = db.FavoriteVideos.Include(x=>x.Video).Where(x => x.Video.Name.Contains(searchString) && x.UserId==UserId).Count();
                switch (state)
                {
                    case SortState.New:
                        {
                            var temp = db.FavoriteVideos
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
                            var temp = db.FavoriteVideos
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

                            var temp = db.FavoriteVideos
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
                            var temp = db.FavoriteVideos
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

        public IEnumerable<FavoriteVideo> GetAll()
        {
            return db.FavoriteVideos.AsNoTracking().Include(c => c.User).ToList();
        }

        public void Update(FavoriteVideo item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
