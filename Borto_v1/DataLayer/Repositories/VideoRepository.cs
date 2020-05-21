
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Borto_v1
{
    public class VideoRepository : IRepository<Video>
    {
        private PlayerContext db;

        private PlayerContext temp_db_context;

        public VideoRepository(PlayerContext context)
        {
            this.db = context;
        }

        public void Create(Video item)
        {
            db.Videos.Add(item);
        }

        public void Delete(int id)
        {
            Video video = db.Videos.Find(id);
            if (video != null)
                db.Videos.Remove(video);
        }

        public IEnumerable<Video> Find(Func<Video, bool> predicate)
        {
            return db.Videos.Where(predicate).ToList();
        }
        /// <summary>
        /// Checks if there is video in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            temp_db_context = new PlayerContext();
            Video test = temp_db_context.Videos.Find(id);
            temp_db_context.Dispose();
            if (test != null)
                return true;
            return false;

        }

        public IEnumerable<Video> FindByUserId(int UserId)
        {
                var Videos = db.Set<Video>().AsNoTracking().Where(c => c.UserId == UserId).ToList();
                return Videos;
            
        }
        public Video Get(int id)
        {
            return db.Videos.Find(id);
        }

        public IEnumerable<Video> GetAll()
        {
            return db.Videos.AsNoTracking().Include(c => c.User).ToList();
        }
        public int CountVideos()
        {
            return db.Videos.AsNoTracking().Count();
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
        public IEnumerable<Video> GetVideoByRange(int pageCount, int numberOfItems, SortState state, string searchString, out int count)
        {
            int skip = (pageCount - 1) * numberOfItems;
            IEnumerable<Video> videos = null;
            count = db.Videos.Where(x => x.Name.Contains(searchString)).Count();
            switch (state)
            {
                case SortState.New:
                    {
                        videos = db.Videos.AsNoTracking().Include(c => c.User)
                           .Where(x => x.Name.Contains(searchString))
                           .OrderByDescending(x => x.UploadDate)
                           .Skip(skip).Take(numberOfItems).ToList();
                        break;
                    }
                case SortState.Old:
                    {
                        videos = db.Videos.AsNoTracking().Include(c => c.User)
                            .Where(x => x.Name.Contains(searchString))
                            .OrderBy(x => x.UploadDate)
                            .Skip(skip).Take(numberOfItems).ToList();
                        break;
                    }
                case SortState.Long:
                    {
                        videos = db.Videos.AsNoTracking().Include(c => c.User)
                            .Where(x => x.Name.Contains(searchString))
                            .OrderByDescending(x => x.MaxDuration)
                            .Skip(skip).Take(numberOfItems).ToList();
                        break;
                    }
                case SortState.Short:
                    {
                        videos = db.Videos.AsNoTracking().Include(c => c.User)
                            .Where(x => x.Name.Contains(searchString))
                            .OrderBy(x => x.MaxDuration).Skip(skip).Take(numberOfItems).ToList();
                        break;
                    }
            }
            return videos;
        }


        public void Update(Video item)
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

    }
}
