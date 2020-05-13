
using System.Data.Entity;

namespace Borto_v1
{
    public class PlayerContext: DbContext
    {
        public PlayerContext() : base("BortoLocal")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new VideoConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; } 
    }
}
