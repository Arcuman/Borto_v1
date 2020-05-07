
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class OpenWindowMessage
    {
        public WindowType Type { get; set; }
        public User Argument { get; set; }
    }
}
