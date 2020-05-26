using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class Video
    {
        public int IdVideo { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        /// <summary>
        /// Path to the Video on Azure (Name + uniq)
        /// </summary>
        public string Path { get; set; }

        public double MaxDuration { get; set; }

        public bool HasConvertation { get; set; }

        public DateTime UploadDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public virtual List<Mark> Marks { get; set; }

        public virtual List<WatchLater> WatchLaters { get; set; }

        public virtual List<PlaylistVideo> PlaylistVideos { get; set; }

        public virtual List<Comment> Comments { get; set; }
        [NotMapped]
        public string Duration {
            get
            {
                return TimeSpan.FromSeconds(MaxDuration).ToString(@"hh\:mm\:ss");
            } 
        }

        public Video()
        {
        }

        public Video(string name, string description, byte[] image, int userId, string path, double maxDuration = 0)
        {
            Name = name;
            Description = description;
            Image = image;
            UserId = userId;
            Path = path;
            MaxDuration = maxDuration;
            UploadDate = DateTime.Now;
        }

        #region Helper
        /// <summary>
        /// Подсчет длительности видео
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static double GetMaxDuration(string path)
        {
            var inputFile = new MediaFile { Filename = path };
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
            }
            return inputFile.Metadata.Duration.TotalSeconds;
        }
        #endregion
    }
}
