using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class GameTable
    {
        private int[][] table;
        private GameFigure[][] figures;

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

        public GameTable(string s)
        {
            figures = new GameFigure[2][];
            for (int i = 0; i < 2; i++)
            {
                figures[i] = new GameFigure[2];
            }

            int ind = 0;

            table = new int[5][];
            for (int i = 0; i < 5; i++)
            {
                table[i] = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    char c = s[ind];
                    if (c == 'X')
                    {
                        ind++;
                        c = s[ind];
                        int k = Int32.Parse(c.ToString());
                        ind++;
                        c = s[ind];
                        int m = Int32.Parse(c.ToString());
                        ind++;
                        c = s[ind];
                        table[i][j] = Int32.Parse(c.ToString());
                        figures[m][k] = new GameFigure(i, j);
                        ind++;
                    }
                    else
                    {
                        table[i][j] = Int32.Parse(c.ToString());
                        ind++;
                    }
                }
            }
        }

        public bool ValidMove(GameFigure selected, int i, int j)
        {
            if (i < 0 || i > 4 || j < 0 || j > 4) return false;

            if (Math.Abs(selected.X - i) < 2 && Math.Abs(selected.Y - j) < 2
                && !(selected.X == i && selected.Y == j)
                && this.IsFreeSpot(i, j)
                && table[i][j] < 4 
                && ((table[i][j] <= table[selected.X][selected.Y]) || (table[i][j] - table[selected.X][selected.Y] <= 1)))
                return true;
            else return false;
        }

        internal void RemoveFigure(int playerTurn, int figure)
        {
            figures[playerTurn][figure] = null;
        }

        public bool ValidBuild(GameFigure selected, int i, int j)
        {
            if (i < 0 || i > 4 || j < 0 || j > 4) return false;

            if (Math.Abs(selected.X - i) < 2 && Math.Abs(selected.Y - j) < 2
                && !(selected.X == i && selected.Y == j)
                && IsFreeSpot(i, j)
                && table[i][j] < 4)
                return true;
            else return false;
        }

        public void Build(int i, int j)
        {
            this.table[i][j]++;
        }

        public bool IsFreeSpot(int x, int y)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (figures[i][j] != null && figures[i][j].OnSpot(x, y)) return false;

            return true;
        }

        public GameFigure FigureAt(int x, int y)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (figures[i][j] != null && figures[i][j].OnSpot(x, y)) return figures[i][j];

            return null;
        }

        public int CheckFinished(int playerTurn)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    if (figures[i][j] == null) return 0;

            if (table[figures[0][0].X][figures[0][0].Y] == 3 || table[figures[0][1].X][figures[0][1].Y] == 3)
            {
                // Player 1 on lvl 3 field
                return 1;
            }

            if (table[figures[1][0].X][figures[1][0].Y] == 3 || table[figures[1][1].X][figures[1][1].Y] == 3)
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

        public void SetFigure(GameFigure f, int player, int figure)
        {
            figures[player][figure] = f;
        }

        public void Unbuild(int buildI, int buildJ)
        {
            this.table[buildI][buildJ]--;
        }

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

        public override string ToString()
        {
            string sol = "";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    bool figureOnSpot = false;
                    for (int k = 0; k < 2; k++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            if (figures[k][m] != null) figureOnSpot = figures[k][m].OnSpot(i, j);
                            if (figureOnSpot)
                            {
                                sol += 'X' + k + m + table[i][j];
                                break;
                            }
                        } // END FOR M
                        if (figureOnSpot) break;
                    } // END FOR K

                    if (!figureOnSpot) sol += table[i][j];
                } // END FOR J
            } // END FOR I

            return sol;
        }

        private bool CanFigureMove(GameFigure f)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
            {
                    if (this.ValidMove(f, f.X + i, f.Y + j)) return true;
            }

            return false;
        }

    }
}
