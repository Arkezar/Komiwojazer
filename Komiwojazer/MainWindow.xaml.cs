using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;

namespace Komiwojazer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker bgw = null;
        MainLogic ml = new MainLogic();
        int krok;
        int[,] kro;
        int index;
        public int mutacja;
        public int selekcja;
        public int krzyzowanie;

        public MainWindow()
        {
            InitializeComponent();
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            chart1.DataContext = ml.cl ;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            listBox1.Items.Clear();
            listBox1.Items.Add("Miasta:");
            ml.miasta(int.Parse(textBox2.Text));
            foreach(Miasto m in ml.getLista())
                listBox1.Items.Add(m.toString());
            ml.macierz();
            int l = ml.getLista().Count;
            listBox1.Items.Add("Macierz odległości między miastami:");
            for (int i = 0; i < l; i++)
            {
                String o = "";
                for (int j = 0; j < l; j++)
                {
                    o += " " + ml.getMac()[i, j].ToString();
                }
                listBox1.Items.Add(o);
            }
            ml.setMax(int.Parse(textBox2.Text));
            button3.IsEnabled = true;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "tsp files (*.tsp)|*.tsp|All files (*.*)|*.*";
            dialog.InitialDirectory = "C:\\" ;
            dialog.Title = "Select a text file";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Miasta:");
                ml.clearList();
                textBox4.Text = "Źródło zlokalizowano";
                ml.miastaZPliku(dialog.FileName);
                foreach (Miasto m in ml.getLista())
                    listBox1.Items.Add(m.toString());
                ml.macierz();
                int l = ml.getLista().Count;
                listBox1.Items.Add("Macierz odległości między miastami:");
                for (int i = 0; i < l; i++)
                {
                    String o = "";
                    for (int j = 0; j < l; j++)
                    {
                        o += " " + ml.getMac()[i, j].ToString();
                    }
                    listBox1.Items.Add(o);
                }
                ml.setMax(int.Parse(textBox2.Text));
                button3.IsEnabled = true;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            String[] osob;
            int Start = Environment.TickCount;

            ml.createOsob(ml.getLista().Count, int.Parse(textBox1.Text));
            osob=ml.printOsobnicy();
            listBox1.Items.Add("Osobnicy:");
            foreach (String s in osob)
                listBox1.Items.Add(s);
            int Elapsed = (Environment.TickCount) - Start;
            Console.WriteLine("Time Elapsed: {0} ms", Elapsed);
            button4.IsEnabled = true;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            

            if ((null != bgw) && bgw.IsBusy)
            {
                bgw.CancelAsync();
                chart1.DataContext = ml.cl;
                button4.Content = "START";
                bgw = null;
            } else {
                ml.cl = new KeyValuePair<int, int>[int.Parse(textBox6.Text)];
                kro = new int[int.Parse(textBox6.Text), 2];
                progressBar1.Maximum = int.Parse(textBox6.Text);
                krok = int.Parse(textBox6.Text);
                ml.createOsob(ml.getLista().Count, int.Parse(textBox1.Text));
                selekcja = int.Parse(textBox7.Text);
                krzyzowanie = int.Parse(textBox3.Text);
                mutacja = int.Parse(textBox5.Text);
                bgw = new BackgroundWorker();
                bgw.DoWork +=
                    new DoWorkEventHandler(bgw_DoWork);
                bgw.RunWorkerCompleted +=
                    new RunWorkerCompletedEventHandler(
                    bgw_RunWorkerCompleted);
                bgw.ProgressChanged +=
                    new ProgressChangedEventHandler(bgw_ProgressChanged);
                bgw.WorkerReportsProgress = true;
                bgw.WorkerSupportsCancellation = true;
                progressBar1.Value = 0;
                listBox1.Items.Add("Oceny najlepszych osobników w kolejnych generacjach:");
                button4.Content = "STOP";
                bgw.RunWorkerAsync();
            }        
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            for (int i = 0; i < krok; i++)
            {
                if (bgw.CancellationPending)
                {
                    int fill = ml.getPopulacja()[0].dlugosc;
                    e.Cancel = true;
                    for (; i < krok; i++)
                    {
                        ml.cl[i] = new KeyValuePair<int, int>(i, fill);  
                    }
                    break;
                }   
                ml.select(selekcja);
                ml.krzyzowanie(krzyzowanie);
                ml.mutacja(mutacja);
                if (i % 20 == 0)
                {
                    kro[index, 0] = (int)ml.getPopulacja()[0].dlugosc;
                    if (kro[0, 0] != 0)
                        kro[index, 1] = i;
                    index += 1;
                }            
                bgw.ReportProgress(i+1);
                ml.cl[i] = new KeyValuePair<int, int>(i , ml.getPopulacja()[0].dlugosc);               
            }        
        }

        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listBox1.Items.Add(ml.getPopulacja()[0].dlugosc);
            progressBar1.Value = e.ProgressPercentage;
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Przerwano.");
            }
            else
            {
                chart1.DataContext = ml.cl;
                listBox1.Items.Add("Najlepszy osobnik ostatniej generacji:");
                listBox1.Items.Add(ml.printOsobnik());
                button4.Content = "START";
                Console.WriteLine("Zakończono.");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Clear();
            ml = new MainLogic();
            bgw = null;
            chart1.DataContext = ml.cl;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            textBox4.Text = "Wskaż źródło";
        }

    }
}
