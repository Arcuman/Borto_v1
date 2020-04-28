using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class BMenuItem
    {
        public String Name { get; private set; }

        public PackIconKind Icon { get; private set; }


        public BMenuItem(string name, PackIconKind icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}
