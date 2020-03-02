using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE
{
    class Program
    {
        static void Main(string[] args)
        {
            int from=1,to=1;
            Siec siec=new Siec();
            string filename;
            do
            {
                Console.WriteLine("Podaj nazwe pliku z katalogu data:");
                filename = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Plik nie istnieje");
            } while (!(siec.Wczytaj(filename)));
            siec.Mst();
            Console.Clear();
            do
            {
                Console.WriteLine("Podaj początek i koniec poszukiwanej ścieżki (max {0}), aby wyjść podaj 0", siec.Get_wezels());
                try
                {
                    Console.WriteLine("Podaj początek:");
                    from = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Podaj koniec:");
                    to = Convert.ToInt32(Console.ReadLine());
                }
                catch(FormatException e)
                {
                    Console.Clear();
                    Console.WriteLine("Niepoprawny format danych");
                    continue;
                }
                Console.Clear();
                if (from > 0 && from <= siec.Get_wezels() && to > 0 && to <= siec.Get_wezels()) siec.Dijkstra(from, to);
                else Console.WriteLine("Wyjście poza zakres");
            } while (from != 0 && to != 0);
            Console.Clear();
            Console.WriteLine("fin.");
            Console.ReadLine();
        }
    }
}
