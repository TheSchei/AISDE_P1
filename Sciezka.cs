using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE
{
    class Sciezka
    {
        private int index;
        public int Index //numer pobierany z pliku
            {
            get { return index; }
            }
        private int start;
        public int Start
        {
            get { return start; }
        }
        private int koniec;
        public int Koniec
        {
            get { return koniec; }
        }
        private int val;
        public int Val //zaokrąglona długość
        {
            get { return val; }
        }
        public Sciezka(string id, string start, string koniec, List<Wezel> wezels)
        {
            this.index = Convert.ToInt32(id);
            this.start = Convert.ToInt32(start);
            this.koniec = Convert.ToInt32(koniec);
            setVal(wezels);
        }
        private void setVal(List<Wezel> wezels)
        {
            double temp1, temp2;
            temp1 = Convert.ToDouble(wezels[start - 1].X - wezels[koniec - 1].X);
            temp2 = Convert.ToDouble(wezels[start - 1].Y - wezels[koniec - 1].Y);
            val = Convert.ToInt32(Math.Sqrt((temp1 * temp1) + (temp2 * temp2)));
        }
    }
}
