using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komiwojazer
{
    class Mutacja : MainLogic
    {
        static Random ran = new Random();
        public static Osobnik[] mutacja(int p, Osobnik[] osobnik, int[,] mac)
        {
            double procent = p / 100.0D;
            int liczba = (int)(osobnik.Length * procent);

            int max = osobnik[0].trasa.Length - 1;

            for (int i = 0; i < liczba; i++)
            {
                int x = ran.Next(max - 1) + 1;
                int y = ran.Next(max - 1) + 1;
                int os = ran.Next(osobnik.Length - 1) + 1;
                int[] tr = osobnik[os].trasa;
                int pom = tr[x];
                tr[x] = tr[y];
                tr[y] = pom;

                osobnik[os].trasa = tr;
                osobnik[os].dlugosc = Ocena.ocen(tr, mac);
            }
            return osobnik;
            
        }
    }
}
