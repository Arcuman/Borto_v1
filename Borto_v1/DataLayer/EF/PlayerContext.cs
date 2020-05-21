
using System.Data.Entity;

namespace Borto_v1
{
    public class PlayerContext : DbContext
    {
        public PlayerContext() : base("BortoServer")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new VideoConfiguration());
            modelBuilder.Configurations.Add(new MarkConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new FavoriteVideoConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<FavoriteVideo> FavoriteVideos { get; set; }
    }
}
