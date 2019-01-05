using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents game table for Santorini game
    /// </summary>
    public class GameTable
    {
        private int[][] table;
        private GameFigure[][] figures;

        /// <summary>
        /// Default ctor
        /// </summary>
        public GameTable()
        {
            table = new int[5][];
            for (int i = 0; i < 5; i++)
            {
                table[i] = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    table[i][j] = 0;
                }
            }

            figures = new GameFigure[2][];
            for (int i = 0; i < 2; i++)
            {
                figures[i] = new GameFigure[2];
            }
        }

        /// <summary>
        /// Check if figure moving is valid
        /// </summary>
        /// <param name="selected">Selected figure to move</param>
        /// <param name="i">Row where to move figure</param>
        /// <param name="j">Column where to move figure</param>
        /// <returns>True if move is valid</returns>
        public bool ValidMove(GameFigure selected, int i, int j)
        {
            if (i < 0 || i > 4 || j < 0 || j > 4) return false;

            if (Math.Abs(selected.I - i) < 2 && Math.Abs(selected.J - j) < 2
                && !(selected.I == i && selected.J == j)
                && this.IsFreeSpot(i, j)
                && table[i][j] < 4 
                && ((table[i][j] <= table[selected.I][selected.J]) || (table[i][j] - table[selected.I][selected.J] <= 1)))
                return true;
            else return false;
        }

        /// <summary>
        /// Method for removing figure from game table
        /// </summary>
        /// <param name="playerTurn">Players turn which figure is</param>
        /// <param name="figure">Id of player's figure (0 or 1)</param>
        public void RemoveFigure(int playerTurn, int figure)
        {
            figures[playerTurn][figure] = null;
        }

        /// <summary>
        /// Check if build move is valid
        /// </summary>
        /// <param name="selected">Selected figure that is trying to build</param>
        /// <param name="i">Row where figure is trying to build</param>
        /// <param name="j">Column where figure is trying to build</param>
        /// <returns></returns>
        public bool ValidBuild(GameFigure selected, int i, int j)
        {
            if (i < 0 || i > 4 || j < 0 || j > 4) return false;

            if (Math.Abs(selected.I - i) < 2 && Math.Abs(selected.J - j) < 2
                && !(selected.I == i && selected.J == j)
                && IsFreeSpot(i, j)
                && table[i][j] < 4)
                return true;
            else return false;
        }

        /// <summary>
        /// Method to buid on given spot
        /// </summary>
        /// <param name="i">Row of spot</param>
        /// <param name="j">Column of spot</param>
        public void Build(int i, int j)
        {
            this.table[i][j]++;
        }

        /// <summary>
        /// Method to check if given spot is not occupied
        /// </summary>
        /// <param name="x">Row of spot</param>
        /// <param name="y">Column of spot</param>
        /// <returns></returns>
        public bool IsFreeSpot(int x, int y)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (figures[i][j] != null && figures[i][j].OnSpot(x, y)) return false;

            return true;
        }

        /// <summary>
        /// Method to get figure on given spot
        /// </summary>
        /// <param name="x">Row of spot</param>
        /// <param name="y">Column of spot</param>
        /// <returns>Figure on given spot or null</returns>
        public GameFigure FigureAt(int x, int y)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (figures[i][j] != null && figures[i][j].OnSpot(x, y)) return figures[i][j];

            return null;
        }

        /// <summary>
        /// Method to check if game is finished
        /// </summary>
        /// <param name="playerTurn">Player currently on move</param>
        /// <returns>0 - no winner , 1 - first player winner, 2 - second player winner</returns>
        public int CheckFinished(int playerTurn)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (figures[i][j] == null) return 0;

            if (table[figures[0][0].I][figures[0][0].J] == 3 || table[figures[0][1].I][figures[0][1].J] == 3)
            {
                // Player 1 on lvl 3 field
                return 1;
            }

            if (table[figures[1][0].I][figures[1][0].J] == 3 || table[figures[1][1].I][figures[1][1].J] == 3)
            {
                // Player 2 on lvl 3 field
                return 2;
            }

            if (!this.CanFigureMove(figures[0][0]) && !this.CanFigureMove(figures[0][1]) && playerTurn == 0)
            {
                // Player 1 cant move any figures
                return 2;
            }

            if (!this.CanFigureMove(figures[1][0]) && !this.CanFigureMove(figures[1][1]) && playerTurn == 1)
            {
                // Player 2 cant move any figures
                return 1;
            }

            return 0;

        }

        /// <summary>
        /// Method to set figure for player
        /// </summary>
        /// <param name="f">Figure to be set</param>
        /// <param name="player">Players turn</param>
        /// <param name="figure">Players figure id</param>
        public void SetFigure(GameFigure f, int player, int figure)
        {
            figures[player][figure] = f;
        }

        /// <summary>
        /// Method to unbuild from spot
        /// </summary>
        /// <param name="buildI">Row from which to unbuild</param>
        /// <param name="buildJ">Column from which to unbild</param>
        public void Unbuild(int buildI, int buildJ)
        {
            this.table[buildI][buildJ]--;
        }

        /// <summary>
        /// Method to get status of table spot
        /// </summary>
        /// <param name="x">Row of spot</param>
        /// <param name="y">Column of spot</param>
        /// <returns>Array with information. Arr[0] - height of spot, Arr[1] - Id of player whos figure is on that spot</returns>
        public int[] GetTableValueAt(int x,int y)
        {
            int[] res = new int[2];
            res[0] = table[x][y];
            res[1] = -1;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (figures[i][j] != null && figures[i][j].OnSpot(x, y))
                    {
                        res[1] = i;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Check if figure has valid move on table
        /// </summary>
        /// <param name="f">Figure for which to check</param>
        /// <returns>True if figure has valid move</returns>
        private bool CanFigureMove(GameFigure f)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
            {
                    if (this.ValidMove(f, f.I + i, f.J + j)) return true;
            }

            return false;
        }

        public override string ToString()
        {
            string res = "";
            for(int i = 0; i<5; i++)
            {
                for (int j = 0; j<5; j++)
                {
                    res += this.table[i][j];
                    if (this.figures[0][0].OnSpot(i, j))
                    {
                        res += "X00X";
                    }
                    if (this.figures[0][1].OnSpot(i, j))
                    {
                        res += "X01X";
                    }
                    if (this.figures[1][0].OnSpot(i, j))
                    {
                        res += "X10X";
                    }
                    if (this.figures[1][1].OnSpot(i, j))
                    {
                        res += "X11X";
                    }
                }
            }

            return res;
        }

    }
}
