using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komiwojazer
{
    class Miasto
    {
        public Miasto() { }
        public Miasto(int x, int y, String name)
        {
            this.x = x;
            this.y = y;
            this.name = name;
        }

        public int x { get; set; }
        public int y { get; set; }
        public String name { get; set; }
        public String toString()
        {
            return name+" "+x + " " + y;
        }
    }
}
