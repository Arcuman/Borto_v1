using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Borto_v1
{
    public class PlaylistVideoRepository : IRepository<PlaylistVideo>
    {
        public PlaylistVideoRepository(PlayerContext context)
        {
        }

        public void Create(PlaylistVideo item)
        {
            using (var db = new PlayerContext())
            {
                db.PlaylistVideos.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                PlaylistVideo favvideo = db.PlaylistVideos.Find(id);
                if (favvideo != null)
                {
                    db.PlaylistVideos.Remove(favvideo);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<PlaylistVideo> Find(Func<PlaylistVideo, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.PlaylistVideos.Where(predicate).ToList();
            }
        }
        public PlaylistVideo GetIfExist(int playlistId,int videoId)
        {
            using (var db = new PlayerContext())
            {
                return db.PlaylistVideos.AsNoTracking().Where(x => x.VideoId == videoId && x.PlaylistId == playlistId).FirstOrDefault();
            }
        }
        public PlaylistVideo Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.PlaylistVideos.Find(id);
            }
        }

        public IEnumerable<PlaylistVideo> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.PlaylistVideos.AsNoTracking().Include(i => i.Video).ToList();
            }
        }
         public IEnumerable<Video> GetVideoByPlayList(int pageCount, int numberOfItems, int PlaylistID,out int count)
        {
            using (var db = new PlayerContext())
            {
                int skip = (pageCount - 1) * numberOfItems;
                IEnumerable<Video> videos = null;
                count = db.PlaylistVideos.AsNoTracking().Include(x => x.Video).Where(x => x.PlaylistId == PlaylistID).Count();
                videos = db.PlaylistVideos.Where(x => x.PlaylistId == PlaylistID).OrderByDescending(x => x.Video.UploadDate)
                                                     .Skip(skip).Take(numberOfItems).Select(x => x.Video).ToList();
                foreach (var item in videos)
                {
                    db.Entry(item).Reference("User").Load();
                }
                return videos;
            }
        }

        public void Update(PlaylistVideo item)
        {
            using (var db = new PlayerContext())
            {
                db.Entry(item).State = EntityState.Modified;
            }
        }
    }
}
