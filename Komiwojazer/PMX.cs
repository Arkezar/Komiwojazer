using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komiwojazer
{
    class PMX : MainLogic
    {
        public static void krzyzowaniePMX(int p, Osobnik[] osobnic, List<Miasto> m) {
            double procent = p / 100.0D;
            double nowych = osobnic.Length * procent;
            nowych /= 2.0D;
            int liczbaMiast = m.Count;

            Osobnik[] nowa = new Osobnik[(int)nowych * 2];
            int indNowa = 0;
            for (int i = 0; i < (int)nowych; i++)
            {
                int y;
                if (liczbaMiast == 1) 
                { 
                    y = 1;
                }
                else
                {
                if (liczbaMiast == 2) 
                { 
                    y = 2;
                }
                else
                {
          
                if (liczbaMiast == 3) 
                { 
                    y = 2;
                } 
                else 
                {
                    int z = r.Next(liczbaMiast - 1) + 1;
                    int l = r.Next(liczbaMiast - 1) + 1;
                    int x;
                    if (z > l) 
                    { 
                        y = z; 
                        x = l; 
                    } else 
                    { 
                        y = l; x = z;
                    }

                    int ro1 = r.Next(osobnic.Length);
                    int ro2 = r.Next(osobnic.Length);
                    int[] r1 = osobnic[ro1].trasa;
                    int[] r2 = osobnic[ro2].trasa;
                    int[] d1 = new int[liczbaMiast + 1];
                    int[] d2 = new int[liczbaMiast + 1];
                    d1[0] = 1; d1[liczbaMiast] = 1;
                    d2[0] = 1; d2[liczbaMiast] = 1;

                    for (int b = x; b < y + 1; b++) {
                        d1[b] = r2[b];
                        d2[b] = r1[b];
                    }

                    for (int b = 1; b < x; b++) {
                        bool znalazl = false;
                        bool znalazl2 = false;
                        for (int c = 1; c < d1.Length - 1; c++) {
                            if (r1[b] == d1[c]) znalazl = true;
                            if (r2[b] != d2[c]) continue; 
                            znalazl2 = true;
                        }
                        if (!znalazl) {
                            d1[b] = r1[b];
                        }
                        if (!znalazl2) {
                            d2[b] = r2[b];
                        }
                    }

                    for (int b = y + 1; b < d1.Length - 1; b++) {
                        bool znalazl = false;
                        bool znalazl2 = false;
                        for (int c = 1; c < d1.Length - 1; c++) {
                            if (r1[b] == d1[c]) znalazl = true;
                            if (r2[b] != d2[c]) continue; znalazl2 = true;
                        }
                        if (!znalazl) {
                            d1[b] = r1[b];
                        }
                        if (!znalazl2) {
                            d2[b] = r2[b];
                        }
                    }

                    int index = x;
                    for (int d = 1; d < d1.Length - 1; d++) {
                        if (((d >= x) && (d <= y)) || (d1[d] != 0)) continue;
                        bool dalej = true;
                        while (dalej) {
                            bool dobry = true;
                            for (int c = 1; c < d1.Length - 1; c++) {
                                if (d1[c] != r1[index]) continue; 
                                dobry = false;
                                }
                            if (dobry) 
                            {
                                d1[d] = r1[index];
                                dalej = false; 
                            } 
                            else 
                            {
                                index++;
                            }
                        }

                    }

                    index = x;
                    for (int d = 1; d < d2.Length - 1; d++) {
                        if (((d >= x) && (d <= y)) || (d2[d] != 0)) continue;
                        bool dalej = true;
                        while (dalej) {
                            bool dobry = true;
                            for (int c = 1; c < d2.Length - 1; c++) {
                                if (d2[c] != r2[index]) continue; dobry = false;
                            }
                            if (dobry) {
                                d2[d] = r2[index];
                                dalej = false; 
                            } 
                            else 
                            {
                            index++;
                            }
                        }

                    }
            
                    nowa[indNowa] = new Osobnik(d1, createBinaryTras(d1), Ocena.ocen(d1, macierzOdl));
                    indNowa++;
                    nowa[indNowa] = new Osobnik(d2, createBinaryTras(d2), Ocena.ocen(d2, macierzOdl));
                    indNowa++;
                    }
                }
            }
        }
        polacz(osobnic, nowa);
        }
    }
}
