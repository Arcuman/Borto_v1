
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
        }

        public void Create(Video item)
        {
            using (var db = new PlayerContext())
            {
                db.Videos.Add(item);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new PlayerContext())
            {
                Video video = db.Videos.Find(id);
                if (video != null)
                {
                    db.Videos.Remove(video);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<Video> Find(Func<Video, bool> predicate)
        {
            using (var db = new PlayerContext())
            {
                return db.Videos.Where(predicate).ToList();
            }
        }
        /// <summary>
        /// Checks if there is video in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            using (var db = new PlayerContext())
            {
                Video test = db.Videos.Find(id);
                if (test != null)
                    return true;
                return false;
            }

        }
        
        public IEnumerable<Video> FindByUserId(int UserId)
        {
            using (var db = new PlayerContext())
            {
                return  db.Videos.AsNoTracking().Include(c => c.User).Where(c => c.UserId == UserId).ToList();
                
            } 
        }
        public Video Get(int id)
        {
            using (var db = new PlayerContext())
            {
                return db.Videos.Find(id);
            }
        }

        public IEnumerable<Video> GetAll()
        {
            using (var db = new PlayerContext())
            {
                return db.Videos.AsNoTracking().Include(c => c.User).ToList();
            }
        }

        public IEnumerable<Video> GetBySearch(string DateSearch,string TitleSearch, string UserSearch)
        {
            using (var db = new PlayerContext())
            {
                if (!String.IsNullOrWhiteSpace(DateSearch))
                {
                    DateTime searchFrom = Convert.ToDateTime(DateSearch);
                    DateTime searchTo = searchFrom.AddDays(1);
                    return db.Videos.AsNoTracking().Include(x => x.User).Where(x => x.UploadDate >= searchFrom && x.UploadDate <= searchTo
                                                    && x.Name.Contains(TitleSearch)
                                                    && x.User.NickName.Contains(UserSearch)).ToList();
                }
                else
                {
                    DateTime searchFrom = Convert.ToDateTime(DateSearch);
                    DateTime searchTo = searchFrom.AddDays(1);
                    return db.Videos.AsNoTracking().Include(x => x.User).Where(x => x.Name.Contains(TitleSearch)
                                                    && x.User.NickName.Contains(UserSearch)).ToList();
                }
            }
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
            using (var db = new PlayerContext())
            {
                int skip = (pageCount - 1) * numberOfItems;
                IEnumerable<Video> videos = null;
                count = db.Videos.AsNoTracking().Where(x => x.Name.Contains(searchString)).Count();
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
        }


        public void Update(Video item)
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

    }
}
