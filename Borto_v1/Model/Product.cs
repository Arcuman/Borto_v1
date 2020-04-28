using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.Model
{
   public class Product
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Image { get; set; }

        public Product(string name, string value, string image)
        {
            Name = name;
            Value = value;
            Image = image;
        }
    }
}
