using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class Comment
    {
        public int IdComment { get; set; }

        public string TypeMark { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int VideoId { get; set; }

        [Required]
        [ForeignKey("VideoId")]
        public Video Video { get; set; }

        public Comment()
        {
        }

    }
}
