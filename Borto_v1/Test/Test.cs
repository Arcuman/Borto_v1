using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class Test
    {
        public String Name { get; private set; }

        public PackIconKind Icon { get; private set; }

        public RelayCommand CommandMenu { get; private set; }

        public Test(string name, PackIconKind icon, RelayCommand relayCommand)
        {
            Name = name;
            Icon = icon;
            CommandMenu = relayCommand;
        }
    }
}
