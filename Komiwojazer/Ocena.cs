using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komiwojazer
{
    static class Ocena
    {
        public static Osobnik[] ocen(Osobnik[] o, int[,] m)
        {
            for (int g = 0; g < o.Length; g++)
            {
                o[g].dlugosc = ocen(o[g].trasa, m);
            }
            return o;
        }

        public static int ocen(int[] t, int[,] m)
        {
            int o=0;
            int tl = t.Length;
            for (int i = 0; i < tl - 1; i++)
            {
                o += m[t[i]-1, t[i + 1]-1];
            }
            return o;
        }

        public static int sumujOceny(Osobnik[] osobnik)
        {
            int suma = 0;
            for (int i = 0; i < osobnik.Length; i++)
            {
                suma += osobnik[i].dlugosc;
            }
            return suma;
        }
    }   
}
