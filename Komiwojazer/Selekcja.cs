using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komiwojazer
{
    class Selekcja
    {

        public static Osobnik[] selection(Osobnik[] osobnik, int p)
        {
            double procent = 1.0D - p / 100.0D;
            int liczba = (int)(osobnik.Length * procent);

            int[] tablica = new int[osobnik.Length];

            for (int i = 0; i < tablica.Length; i++) 
                tablica[i] = (int)osobnik[i].dlugosc;

            Array.Sort<int>(tablica);

            Osobnik[] nowaGrupa = new Osobnik[liczba];

            for (int i = 0; i < nowaGrupa.Length; i++)
            {
                bool znalazl = true;
                int index = 0;
                while (znalazl)
                {
                    if (tablica[i] == (int)osobnik[index].dlugosc)
                    {
                        nowaGrupa[i] = osobnik[index];
                        znalazl = false;
                    }
                    index++;
                }
               
            }
            return nowaGrupa;
        }
    }
}
