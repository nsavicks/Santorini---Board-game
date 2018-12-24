using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class GameFigure
    {
        private int x, y;

        public GameFigure(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public void MoveTo(int nx, int ny)
        {
            this.x = nx;
            this.y = ny;
        }

        public bool OnSpot(int x, int y)
        {
            if (this.x == x && this.y == y) return true;
            else return false;
        }
    }
}
