using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Borto_v1
{
    class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            culture = CultureInfo.CurrentCulture;
            if (value == null) return "";
            if (value is Quality)
            {
                switch ((Quality)value)
                {
                    case Quality.High:
                        {
                            if (culture.Name == "en-US")
                                return "High";
                            else
                                return "Высокое";
                        }
                    case Quality.Medium:
                        {
                            if (culture.Name == "en-US")
                                return "Medium";
                            else
                                return "Среднее";
                        }
                    case Quality.Low:
                        {
                            if (culture.Name == "en-US")
                                return "Low";
                            else
                                return "Низкое";
                        }
                }
            }
            if (value is NotificationType)
            {
                switch ((NotificationType)value)
                {
                    case NotificationType.NewVideo:
                        {
                            if (culture.Name == "en-US")
                                return " uploaded a new video:";
                            else
                                return " выложил новое видео: ";
                        }
                    case NotificationType.NewComment:
                        {
                            if (culture.Name == "en-US")
                                return " commented a video:";
                            else
                                return " оставил комментарий к видео:";
                        }
                }
                }
            return "";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            if (value == null) return "";
            switch (value.ToString())
            {
                case "Высокое":
                case "High":
                    {
                        return Quality.High;
                    }
                case "Среднее":
                case "Medium":
                    {
                        return Quality.Medium;
                    }
                case "Низкое":
                case "Low":
                    {
                        return Quality.Low;
                    }
            }
            return null;
        }
    }
}

