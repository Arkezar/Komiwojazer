using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Komiwojazer
{
    class MainLogic
    {
        public MainLogic instance {get;set;}
        public static Random r = new Random();
        public static List<Miasto> listaMiast;
        public static int[,] macierzOdl;
        public static Osobnik[] osobnicy;
        static int[] tras {get; set;}
        static int lm;
        public static int max;
        static int sumaTras;
        public KeyValuePair<int, int>[] cl;

        public MainLogic()
        {
            instance = this;
            listaMiast = new List<Miasto>(); 
        }

        // ladowanie z pliku

        public void miastaZPliku(String path)
        {
            
            StreamReader objReader = new StreamReader(path);
            string sLine = "";
            ArrayList list = new ArrayList();
            String[] arrText = null;
            for (int i = 1; i < 7; i++)
                objReader.ReadLine();
            while ((sLine = objReader.ReadLine()) != null)
            {
                    arrText=sLine.Split(' ');
                    foreach (string sOutput in arrText)
                        if (!sOutput.Equals("") && !sOutput.Contains(" ") && !sOutput.Contains("  "))
                            list.Add(sOutput);
            }
            objReader.Close();

            String[] dane = new String[list.Count-1];
            int x = 0;
            foreach (String s in list)
            {
                if(!s.Equals("EOF"))
                    dane[x] = s;
                x++;
            }

            for (int i = 0; i < dane.Length; )
            {
                listaMiast.Add(new Miasto(int.Parse(dane[i + 1]), int.Parse(dane[i + 2]), dane[i]));
                i = i + 3;
            }
        }

        
        // krzyzowanie
        public void krzyzowanie(int procent)
        {
            PMX.krzyzowaniePMX(procent, osobnicy, listaMiast);
        }

        // mutacja
        public void mutacja(int procent)
        {
            osobnicy = Mutacja.mutacja(procent, osobnicy, macierzOdl);
            Ocena.sumujOceny(osobnicy);
            Selekcja.selection(osobnicy, 0);
        }

        //ocena populacji

        public void ocenPop()
        {
            osobnicy = Ocena.ocen(osobnicy, macierzOdl);
        }

        //selekcja

        public void select(int przyrost)
        {
            sumaTras = Ocena.sumujOceny(osobnicy);
            osobnicy = Selekcja.selection(osobnicy, przyrost);
        }

        //laczenie

        public static void polacz(Osobnik[] os1, Osobnik[] os2)
        {
            Osobnik[] os3 = new Osobnik[os1.Length + os2.Length];

            for (int i = 0; i < os1.Length; i++)
            {
                os3[i] = os1[i];
            }
            for (int i = 0; i < os2.Length; i++)
            {
                os3[(i + os1.Length)] = os2[i];
            }
            osobnicy = os3;
            if (os3.Length > max)
            {
                sumaTras = Ocena.sumujOceny(osobnicy);
                osobnicy = Selekcja.selection(osobnicy, 0);
                os3 = new Osobnik[max];
                for (int i = 0; i < os3.Length; i++)
                {
                    os3[i] = osobnicy[i];
                }
            }
            osobnicy = os3;
        }

        // tworzenie osobnikow

        public void createOsob(int liczbaMiast, int liczbaOsobnikow)
        {
            createOsobnik(liczbaMiast, liczbaOsobnikow);
        }


        public static void createOsobnik(int liczbaMiast, int liczbaOsobnikow)
        {
            osobnicy = new Osobnik[liczbaOsobnikow];
            for (int i = 0; i < liczbaOsobnikow; i++)
            {
                int[] trasa = createTras(liczbaMiast);
                String[] binary = createBinaryTras(trasa);
                int ocena = Ocena.ocen(trasa, macierzOdl);
                osobnicy[i] = new Osobnik(trasa, binary, ocena);
            }
        }

        public static int[] createTras(int liczbaMiast)
        {
            createTrasa(liczbaMiast);
            int[] trasa = new int[liczbaMiast + 1];
            int wielkosc = liczbaMiast;

            trasa[0] = 1;
            for (int i = 1; i < trasa.Length - 1; i++)
            {
                int x = r.Next(wielkosc - 1) + 1;
                trasa[i] = tras[x];
                tras[x] = 0;
                bool dalej = true;
                int index = 0;
                while (dalej)
                {
                    if (tras[index] == 0)
                    {
                        tras[index] = tras[(wielkosc - 1)];
                        dalej = false;
                    }
                    index++;
                }
                wielkosc--;
            }
            trasa[liczbaMiast] = 1;
            return trasa;
        }
        public static void createTrasa(int liczba)
        {
            tras = new int[liczba];
            for (int i = 0; i < liczba; i++)
                tras[i] = (i + 1);
        }

        public static int mod(int liczba)
        {
            int a = liczba / 2;
            int b = 2 * a;
            if (b == liczba) return 0; return 1;
        }

        public static String binary(int i)
        {
            return Convert.ToString(i, 2);
        }
        public static String binary2(int l)
        {
            if (l == 1) return "1";
            if (l == 0) return "0";
            String wynik = "";
            while (l != 1)
            {
                wynik = wynik + mod(l);
                l /= 2;
                if (l != 1) continue; wynik = wynik + 1;
            }
            String wynik2 = "";
            for (int i = 0; i < wynik.Length; i++)
                wynik2 = wynik2 + wynik.ElementAt(i + wynik.Length - i * 2 - 1);
            return wynik2;
        }
        public static String[] createBinaryTras(int[] trasaa)
        {
            String[] binTras = new String[trasaa.Length];

            int dlugoscSlowa = binary(trasaa.Length).Length;
            for (int b = 0; b < trasaa.Length; b++)
            {
                String slowo = binary(trasaa[b]);
                if (slowo.Length < dlugoscSlowa)
                    while (slowo.Length != dlugoscSlowa)
                        slowo = "0" + slowo;
                binTras[b] = slowo;
            }
            return binTras;
        }

        public String[] printOsobnicy()
        {
            int l = osobnicy.Length;
            String[] osobStat = new String[l];
            for (int i = 0; i < l; i++) {
                int[] trasaka = osobnicy[i].trasa;
                int tl = trasaka.Length;
                String o = "";
                for (int b = 0; b < tl; b++)
                    o+=trasaka[b] + "->";
                o+="  Ocena: " + osobnicy[i].dlugosc;
                osobStat[i] = o;
            }
            return osobStat;
        }

        public String printOsobnik()
        {
            String osobStat;
                int[] trasaka = osobnicy[0].trasa;
                int tl = trasaka.Length;
                String o = "";
                for (int b = 0; b < tl; b++)
                    o += trasaka[b] + "->";
                o += "  Ocena: " + osobnicy[0].dlugosc;
                osobStat = o;
            return osobStat;
        }

        public Osobnik[] getPopulacja()
        {
            return osobnicy;
        }

        // liczenie odleglosci
        public void macierz()
        {
            int liczbaM = listaMiast.Count;
            macierzOdl = new int[liczbaM,liczbaM];
            for (int i = 0; i < liczbaM; i++)
                macierzOdl[i, i] = -1;
            for (int i = 0; i < liczbaM; i++)
            {
                for (int j = 0; j < liczbaM; j++)
                {
                    if (i != j)
                    {
                        macierzOdl[i, j] = odleglosc(listaMiast.ElementAt(i), listaMiast.ElementAt(j));
                    }
                }
            }
        }

        public int odleglosc(Miasto a, Miasto b)
        {
            int odl;

            odl = (int)Math.Sqrt((a.x-b.x)*(a.x-b.x) + (a.y-b.y)*(a.y-b.y));

            return odl;
        }

        public int[,] getMac()
        {
            return macierzOdl;
        }

        // losowe miasta
        public void miasta(int l)
        {
            lm = listaMiast.Count;
            for (int i = 0; i < l; i++)
            {
                listaMiast.Add(rndM(lm+1));
                lm++;
            }

        }
        public Miasto rndM(int i)
        {
            Miasto m = new Miasto(0,0,i.ToString());
            int x;
            int y;
            
            x = r.Next(100);
            y = r.Next(100);
            m.x = x;
            m.y = y;
            return m;
        }
        public List<Miasto> getLista()
        {
            return listaMiast;
        }

        public void clearList()
        {
            listaMiast.Clear();
        }

        public void setMax(int i)
        {
            max = i;
        }
    }
}
