using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents figure in Santorini game
    /// </summary>
    public class GameFigure
    {
        private int i, j;

        /// <summary>
        /// Ctor for game figure
        /// </summary>
        /// <param name="x">Row where to place figure on game table</param>
        /// <param name="y">Column where to place figure on game table</param>
        public GameFigure(int i, int j)
        {
            this.i = i;
            this.j = j;
        }

        public int I { get => i; set => i = value; }
        public int J { get => j; set => j = value; }

        /// <summary>
        /// Method for moving game figure 
        /// </summary>
        /// <param name="ni">Row where to move figure</param>
        /// <param name="nj">Column where to move figure</param>
        public void MoveTo(int ni, int nj)
        {
            this.i = ni;
            this.j = nj;
        }

        /// <summary>
        /// Method for checking if figure is on this spot
        /// </summary>
        /// <param name="i">Row of spot</param>
        /// <param name="j">Column of spot</param>
        /// <returns>True if figure is on given spot</returns>
        public bool OnSpot(int i, int j)
        {
            if (this.i == i && this.j == j) return true;
            else return false;
        }
    }
}
