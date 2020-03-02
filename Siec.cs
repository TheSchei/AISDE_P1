using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace AISDE
{
    class Siec
    {
        private List<Wezel> Wezly=new List<Wezel>();
        private List<Sciezka> Sciezki=new List<Sciezka>();
        public bool Wczytaj(string filename)
        {
            //StreamReader sr = File.OpenText(@"..\..\data\" + string + ".txt");
            if (!File.Exists(@"..\..\data\" + filename + ".txt")) return false;
            StreamReader sr = File.OpenText(@"..\..\data\" + filename + ".txt");
            string linia;
            while ((linia = sr.ReadLine()) != null)//czytanie do końca pliku
            {
                if (linia[0] == '#') continue;//sprawdzenie komentarza
                string[] elementy = linia.Split(' ');
                if (elementy[0] == "WEZLY")
                {
                    int liczba_wezlow = Convert.ToInt32(elementy[2]);
                    for (int i = 0; i < liczba_wezlow; i++)
                    {
                        linia = sr.ReadLine();
                        if (linia[0] == '#')
                        {
                            i--;
                            continue;
                        }
                        elementy = linia.Split(' ');
                        Wezly.Add(new Wezel(elementy[0], elementy[1], elementy[2]));
                    }
                }
                if (elementy[0] == "LACZA")
                {
                    int liczba_lacz = Convert.ToInt32(elementy[2]);
                    for (int i = 0; i < liczba_lacz; i++)
                    {
                        linia = sr.ReadLine();
                        if (linia[0] == '#')
                        {
                            i--;
                            continue;
                        }
                        elementy = linia.Split(' ');
                        Sciezki.Add(new Sciezka(elementy[0], elementy[1], elementy[2], Wezly));
                    }
                    Sciezki.Sort((x,y)=>x.Val.CompareTo(y.Val));//sortowanie
                    for (int i=0; i<Sciezki.Count(); i++)//test
                    {
                        Console.WriteLine("{0}. {2}-{1}",i+1,Sciezki[i].Val, Sciezki[i].Index);
                    }
                }

            }
            sr.Close();
            DirectoryInfo di = new DirectoryInfo(@"..\..\resources");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            //Directory.Delete(@"..\..\resources");
            //Directory.CreateDirectory(@"..\..\resources");
            Rysuj();
            return true;
        }
        public void Mst()
        {
            List<Sciezka> MST = new List<Sciezka>();
            List<int> drzewo = new List<int>();
            int i = 0, k=0;
            drzewo.Add(1);
            while(drzewo.Count()!=Wezly.Count())
            {
                k = Test_mst(drzewo, Sciezki[i]);
                if (k != 0)
                {
                    MST.Add(Sciezki[i]);
                    drzewo.Add(k);
                    i = 0;
                }
                else i++;
            }
            Rysuj("MST", MST);
        }
        public void Dijkstra(int start, int koniec)
        {
            int[] odleglosci = new int[Wezly.Count()];
            int[] status = new int[Wezly.Count()];
            int inf = int.MaxValue;
            int[,] trasy = new int[Wezly.Count(), Wezly.Count()];
            for (int i = 0; i < Wezly.Count(); i++)
            {
                for (int j = 0; j < Wezly.Count(); j++)
                {
                    if (i == j) trasy[i, j] = 0;
                    else trasy[i, j] = inf;
                }
            }
            for (int i=0; i<Sciezki.Count(); i++)
            {
                trasy[Sciezki[i].Start-1, Sciezki[i].Koniec-1] = Sciezki[i].Val;
                trasy[Sciezki[i].Koniec-1, Sciezki[i].Start-1] = Sciezki[i].Val;
            }
            for(int i=0; i< Wezly.Count(); i++)
            {
                odleglosci[i] = inf;
                status[i] = 0;
            }
            int nastepny=start-1;
            odleglosci[start - 1] = 0;
            //status[koniec - 1] = -1;//test
            for(int i=0; i<Wezly.Count(); i++)//dijsktra
            {
                for(int j=0; j<Sciezki.Count(); j++)
                {
                    if (Sciezki[j].Start == Wezly[nastepny].Index)
                    {
                        if (odleglosci[Sciezki[j].Koniec-1] > (odleglosci[nastepny] + Sciezki[j].Val)) odleglosci[Sciezki[j].Koniec-1] = odleglosci[nastepny] + Sciezki[j].Val;
                        if (status[Sciezki[j].Koniec-1] == 0) status[Sciezki[j].Koniec-1] = 1;
                    }
                    if (Sciezki[j].Koniec == Wezly[nastepny].Index)
                    {
                        if (odleglosci[Sciezki[j].Start-1] > (odleglosci[nastepny] + Sciezki[j].Val)) odleglosci[Sciezki[j].Start-1] = odleglosci[nastepny] + Sciezki[j].Val;
                        if (status[Sciezki[j].Start-1] == 0) status[Sciezki[j].Start-1] = 1;
                    }
                }
                status[nastepny] = -1;
                nastepny = 0;
                for(int j=0; j<status.Length; j++)
                {
                    if (status[j] == 1)
                    {
                        nastepny = j;
                        break;
                    }
                }
                for (int j = 0; j < status.Length; j++)//test
                {
                    if ((status[j] == 1)&&(odleglosci[nastepny]>odleglosci[j]))
                    {
                        nastepny = j;
                    }
                }
            }
            Console.WriteLine("---------------------");//test
            for (int j = 0; j < odleglosci.Length; j++)//test
            {
                Console.WriteLine("{0}. {1}",j ,odleglosci[j]);
            }
            Console.WriteLine("---------------------");//test
            //algorytm wyznaczania trasy
            List<Sciezka> Trasa = new List<Sciezka>();//wyznaczanie trasy od końca, algorytmem "czy przebyta droga odejmuje się do etykiety"
            int aktualny=koniec-1;
            while(aktualny!=start-1)
            {
                for(int i=0; i<Sciezki.Count(); i++)
                {
                    if ((Sciezki[i].Start==aktualny+1)&&(odleglosci[aktualny]-Sciezki[i].Val==odleglosci[Sciezki[i].Koniec-1]))
                    {
                        aktualny = Sciezki[i].Koniec - 1;
                        Trasa.Add(Sciezki[i]);
                        break;
                    }
                    if ((Sciezki[i].Koniec == aktualny + 1) && (odleglosci[aktualny] - Sciezki[i].Val == odleglosci[Sciezki[i].Start - 1]))
                    {
                        aktualny = Sciezki[i].Start - 1;
                        Trasa.Add(Sciezki[i]);
                        break;
                    }
                }
            }
            Trasa.Reverse();
            Rysuj("NS_f" + (start) + "to" + (koniec), Trasa);
        }
        private void Rysuj(string algorytm="Graf",List<Sciezka> tempSciezki=null)
        {
            if (algorytm == "Graf")
            {
                Bitmap myBitmap = new Bitmap(1200, 1200);
                Graphics g = Graphics.FromImage(myBitmap);
                Pen pen = new Pen(Brushes.Aquamarine, 2);
                Pen pen2 = new Pen(Brushes.DeepSkyBlue, 20);
                Pen pen3 = new Pen(Brushes.Red, 2);
                for (int i = 0; i < Wezly.Count(); i++)
                {
                    g.DrawEllipse(pen2, (Wezly[i].X) * 10, (Wezly[i].Y) * 10, 20, 20);
                    g.DrawEllipse(pen3, (Wezly[i].X) * 10 - 10, (Wezly[i].Y) * 10 - 10, 40, 40);
                    //g.DrawString(Convert.ToString(Wezly[i].Index), new Font("Segoe UI Light", 22), Brushes.Black, Wezly[i].X * 10-30, Wezly[i].Y * 10-10);
                }
                for (int i = 0; i < Sciezki.Count(); i++)
                {
                    g.DrawLine(pen, (Wezly[Sciezki[i].Start - 1].X) * 10 + 10, (Wezly[Sciezki[i].Start - 1].Y) * 10 + 10, (Wezly[Sciezki[i].Koniec - 1].X) * 10 + 10, (Wezly[Sciezki[i].Koniec - 1].Y) * 10 + 10);
                }
                for (int i = 0; i < Wezly.Count(); i++) g.DrawString(Convert.ToString(Wezly[i].Index), new Font("Segoe UI Light", 22), Brushes.Black, Wezly[i].X * 10 - 30, Wezly[i].Y * 10 - 10);
                myBitmap.Save(@"..\..\resources\" + algorytm + ".bmp");
            }
            else
            {
                Bitmap myBitmap = new Bitmap(@"..\..\resources\Graf.bmp");
                Graphics g = Graphics.FromImage(myBitmap);
                Pen pen = new Pen(Brushes.Red, 2);
                for (int i = 0; i < tempSciezki.Count(); i++)
                {
                    g.DrawLine(pen, (Wezly[tempSciezki[i].Start - 1].X) * 10 + 10, (Wezly[tempSciezki[i].Start - 1].Y) * 10 + 10, (Wezly[tempSciezki[i].Koniec - 1].X) * 10 + 10, (Wezly[tempSciezki[i].Koniec - 1].Y) * 10 + 10);
                }
                myBitmap.Save(@"..\..\resources\" + algorytm + ".bmp");
            }
        }
        private int Test_mst(List<int> drzewo, Sciezka galaz)
        {
            for (int i=0; i<drzewo.Count(); i++)
            {
                if (galaz.Start==drzewo[i])
                {
                    for (int j = 0; j < drzewo.Count(); j++)
                    {
                        if (galaz.Koniec == drzewo[j]) return 0;
                    }
                    return galaz.Koniec;
                }
                if (galaz.Koniec == drzewo[i])
                {
                    for (int j = 0; j < drzewo.Count(); j++)
                    {
                        if (galaz.Start == drzewo[j]) return 0;
                    }
                    return galaz.Start;
                }
            }
            return 0;
        }
        public int Get_wezels()
        {
            return Wezly.Count();
        }
    }
}
