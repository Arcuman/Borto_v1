using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    public class ByteToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[])
            {
                byte[] array = (byte[])value;
                using (var ms = new MemoryStream(array))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            else {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage)
            {
                BitmapImage image = (BitmapImage)value;
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
            else
            {
                return null;
            }
        }
    }
}
