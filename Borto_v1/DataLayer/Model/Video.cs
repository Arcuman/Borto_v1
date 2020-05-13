using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class Video
    {
        public int IdVideo { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        /// <summary>
        /// Path to the Video on Azure (Name + uniq)
        /// </summary>
        public string Path { get; set; }

        public double MaxDuration { get; set; }

        public DateTime UploadDate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public Video()
        {
            Name = null;
            Description = null;
            Image = null;
            User = null;    
            Path = null;
            MaxDuration = 0;
            UploadDate = DateTime.Now;
        }

        public Video(string name, string description, byte[] image, User user, string path, double maxDuration = 0)
        {
            Name = name;
            Description = description;
            Image = image;
            UserId = user.IdUser;
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
        /// <summary>
        /// Convert BitmapImage to byte Array via MemoryStream
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] BitMapToByteArray(BitmapImage image)
        {
            byte[] data;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(image));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        #endregion

    }
}
