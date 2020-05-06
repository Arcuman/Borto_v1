using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class PlayerContext: DbContext
    {
        public PlayerContext() : base("Borto")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; } 
    }
}
