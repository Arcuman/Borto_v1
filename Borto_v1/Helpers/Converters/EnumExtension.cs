using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Borto_v1
{
    public class EnumExtension : MarkupExtension
    {
        public IValueConverter Converter { get; set; }
        public Type EnumType { get; set; }
        public EnumExtension() { }
        public EnumExtension(Type enumType) => EnumType = enumType;
        public override object ProvideValue(IServiceProvider serviceProvider)
            => Enum.GetValues(EnumType).Cast<ValueType>()
                   .Select(t => Converter?.Convert(t, EnumType, null, Thread.CurrentThread.CurrentUICulture) ?? t);
    }
}
