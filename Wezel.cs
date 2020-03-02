using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AISDE
{
    class Wezel
    {
        private int x;
        public int X
        {
            get { return x; }
        }
        private int y;
        public int Y
        {
            get { return y; }
        }
        private int index;
        public int Index //numer pobierany z pliku
        {
            get { return index; }
        }
        public Wezel(string id, string x, string y)
        {
            this.index = Convert.ToInt32(id);
            this.x = Convert.ToInt32(x);
            this.y = Convert.ToInt32(y);
        }
    }
}
