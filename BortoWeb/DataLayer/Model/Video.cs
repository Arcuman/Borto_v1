using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class Video
    {
        public int IdVideo { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Path { get; set; }

        // Это свойство будет использоваться как внешний ключ
       
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Video()
        {
            Name = null;
            Description = null;
            Image = null;
            User = null;
            Path = null;
        }

        public Video(string name, string description, string image, User user,string  path)
        {
            Name = name;
            Description = description;
            Image = image;
            User = user;
            Path = path;
        }

        public void SetVideoPath(string pathToVideo)
        {
            this.Path = pathToVideo;
        }
    }
}
