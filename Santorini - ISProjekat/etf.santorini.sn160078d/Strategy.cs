using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public abstract class Strategy
    {
        protected int depth;
        protected int playerTurn;

        protected Strategy(int depth, int playerTurn)
        {
            this.depth = depth;
            this.playerTurn = playerTurn;
        }

        public abstract Tuple<int, GameMove> PlayNextMove(Game g, GameMove move, int currentDepth, int alpha, int beta); 

        protected abstract int EvaluateMove(Game g, GameMove move);

        protected List<GameMove> GetValidMoves(Game g)
        {
            List<GameMove> res = new List<GameMove>();

            if (g.State == Game.GameState.Finished) return res;

            if (g.State == Game.GameState.WaitingForPlayer1Move || g.State == Game.GameState.WaitingForPlayer2Move)
            {
                GamePlayer player = g.GetCurrentPlayer();
                GameFigure[] figures = player.Figures;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int k = -1; k<=1; k++)
                        {
                            if (g.Table.ValidMove(figures[i], figures[i].X + j, figures[i].Y + k))
                            {
                                for (int m = -1; m <=1; m++)
                                {
                                    for (int z= -1; z<=1; z++)
                                    {
                                        int fromI = figures[i].X;
                                        int fromJ = figures[i].Y;
                                        int toI = figures[i].X + j;
                                        int toJ = figures[i].Y + k;
                                        int buildI = figures[i].X + j + m;
                                        int buildJ = figures[i].Y + k + z;
                                      
                                        if (toI == buildI && toJ == buildJ) continue;

                                        figures[i].MoveTo(toI, toJ);

                                        if (g.Table.ValidBuild(figures[i], buildI, buildJ))
                                        {
                                            res.Add(new GameMove(GameMove.MoveType.FigureMovingAndBuilding, fromI, fromJ, toI, toJ, buildI, buildJ, g.Turn, g.State));
                                        }

                                        figures[i].MoveTo(fromI, fromJ);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (g.Table.IsFreeSpot(i, j)) res.Add(new GameMove(GameMove.MoveType.FigurePlacement, -1, -1, i, j, -1, -1, g.Turn, g.State));
                    }
                }
            }

            return res;
        }
    }
}
