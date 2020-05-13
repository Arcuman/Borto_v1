using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class User
    {
        public int IdUser { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public byte[] Image { get; set; }

        public ObservableCollection<Video> Videos { get; set; }

        public User()
        { }

        public User(string name, string login, string nickName, string password,byte[] image)
        {
            Name = name;
            NickName = nickName;
            Login = login;
            Password = password;
            Image = image;
        }

        public static string getHash(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return "Error";
            }
            else
            {
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
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
    }
}
