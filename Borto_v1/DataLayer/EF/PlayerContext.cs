
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
            modelBuilder.Configurations.Add(new PlaylistConfiguration());
            modelBuilder.Configurations.Add(new WatchLaterConfiguration());
            modelBuilder.Configurations.Add(new PlaylistVideoConfiguration());
            modelBuilder.Configurations.Add(new SubscriptionConfiguration());
            modelBuilder.Configurations.Add(new NotificationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<WatchLater> WatchLaters { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<PlaylistVideo> PlaylistVideos { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
