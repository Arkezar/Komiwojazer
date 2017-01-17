using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komiwojazer
{
    class Osobnik
    {
        public int dlugosc { get; set; }
        public String[] binary { get; set; }
        public int[] trasa { get; set; }
        
        public Osobnik() { }

        public Osobnik(int[] t,String[] b, int d)
        {
            trasa = t;
            binary = b;
            dlugosc = d;
        }

    }
}
