using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents Strategy how cpu players are playing
    /// </summary>
    public abstract class Strategy
    {
        protected int depth;
        protected int playerTurn;

        /// <summary>
        /// Ctor for strategy
        /// </summary>
        /// <param name="depth">Depth for minimax tree</param>
        /// <param name="playerTurn">Players turn for which we are creating minimax tree</param>
        protected Strategy(int depth, int playerTurn)
        {
            this.depth = depth;
            this.playerTurn = playerTurn;
        }

        /// <summary>
        /// Abstract method that plays next move
        /// </summary>
        /// <param name="g">Game on which to play move</param>
        /// <param name="move">Move to be played</param>
        /// <param name="currentDepth">Current depth of minimax tree</param>
        /// <param name="alpha">Alpha value for alpha-beta prunnig</param>
        /// <param name="beta">Beta value for alpha-beta prunnig</param>
        /// <returns>First value - godness of move to be played, second value - move to play</returns>
        public abstract Tuple<int, GameMove> PlayNextMove(Game g, GameMove move, int currentDepth, int alpha, int beta); 

        /// <summary>
        /// Abstract method to evaluate move on game
        /// </summary>
        /// <param name="g">Game on which move to be evaluated</param>
        /// <param name="move">Move to be evaluated</param>
        /// <returns>Evaluation of move</returns>
        protected abstract int EvaluateMove(Game g, GameMove move);

        /// <summary>
        /// Method to get all valid moves on game
        /// </summary>
        /// <param name="g">Game for which to get all valid moves</param>
        /// <returns>List of all valid moves</returns>
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
                            if (g.Table.ValidMove(figures[i], figures[i].I + j, figures[i].J + k))
                            {
                                for (int m = -1; m <=1; m++)
                                {
                                    for (int z= -1; z<=1; z++)
                                    {
                                        int fromI = figures[i].I;
                                        int fromJ = figures[i].J;
                                        int toI = figures[i].I + j;
                                        int toJ = figures[i].J + k;
                                        int buildI = figures[i].I + j + m;
                                        int buildJ = figures[i].J + k + z;
                                      
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
