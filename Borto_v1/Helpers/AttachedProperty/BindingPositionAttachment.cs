using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Borto_v1
{
    public  class BindingPositionAttachment
    {

        private static readonly TimeSpan DefaultValue = new TimeSpan(0);

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.RegisterAttached("Position",
            typeof(TimeSpan), typeof(BindingPositionAttachment),
            new FrameworkPropertyMetadata(DefaultValue, PositionPropertyChanged));

        private static void PositionPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var richEditControl = obj as MediaElement;

            if (richEditControl != null)
            {
                richEditControl.Position = (TimeSpan)e.NewValue;
            }
        }
        public static void SetPosition(UIElement element, TimeSpan value)
        {
            element.SetValue(PositionProperty, value);
        }

        public static TimeSpan GetPosition(UIElement element)
        {
            return (TimeSpan)element.GetValue(PositionProperty);
        }
    }
}
