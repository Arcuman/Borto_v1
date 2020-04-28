using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.Model
{
    public class Video
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public User User { get; set; }

        public Video(string name, string description, string image, User user)
        {
            Name = name;
            Description = description;
            Image = image;
            User = user;
        }
    }
}
