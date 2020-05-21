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

        [Required]
        [MaxLength(210)]
        public string CommentMessage { get; set; }

        public DateTime PostDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int VideoId { get; set; }

        [ForeignKey("VideoId")]
        public Video Video { get; set; }

        public Comment()
        {
        }
        public string GetShortDate()
        {
            return PostDate.ToShortDateString();
        }
    }
}
