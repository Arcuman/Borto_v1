using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.Model
{
    public class User
    {
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
