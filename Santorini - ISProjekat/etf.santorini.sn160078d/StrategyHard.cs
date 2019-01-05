using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents hard strategy for CPU player
    /// </summary>
    public class StrategyHard : Strategy
    {
        /// <summary>
        /// Default ctor for strategy hard
        /// </summary>
        /// <param name="depth">Depth of minimax tree</param>
        /// <param name="playerTurn">Players turn for which we are creating minimax tree</param>
        public StrategyHard(int depth, int playerTurn) : base(depth, playerTurn)
        {
        }

        /// <summary>
        /// Method that plays next move
        /// </summary>
        /// <param name="g">Game on which to play move</param>
        /// <param name="move">Move to be played</param>
        /// <param name="currentDepth">Current depth of minimax tree</param>
        /// <param name="alpha">Alpha value for alpha-beta prunnig</param>
        /// <param name="beta">Beta value for alpha-beta prunnig</param>
        /// <returns>First value - godness of move to be played, second value - move to play</returns>
        public override Tuple<int, GameMove> PlayNextMove(Game g, GameMove move, int currentDepth, int alpha, int beta)
        {
            if (currentDepth == this.depth || g.IsFinished())
            {
                int ret = EvaluateMove(g, move);
                return Tuple.Create<int, GameMove>(ret, null);
            }

            List<GameMove> moves = this.GetValidMoves(g);

            int bestValue = (currentDepth % 2 == 0) ? int.MinValue : int.MaxValue;
            GameMove moveToPlay = null;

            foreach (GameMove newMove in moves)
            {
                g.PlayMove(newMove, true);
                Tuple<int, GameMove> ret = PlayNextMove(g, newMove, currentDepth + 1, alpha, beta);
                int retVal = ret.Item1;
                g.UndoMove();
                if (currentDepth % 2 == 0)
                {
                    if (retVal > bestValue)
                    {
                        bestValue = retVal;
                        moveToPlay = newMove;
                    }

                    alpha = Math.Max(alpha, bestValue);

                    if (beta <= alpha) break;
                }
                else
                {
                    if (retVal < bestValue)
                    {
                        bestValue = retVal;
                        moveToPlay = newMove;
                    }

                    beta = Math.Min(beta, bestValue);

                    if (beta <= alpha) break;
                }
            }

            return Tuple.Create<int, GameMove>(bestValue, moveToPlay);
        }

        /// <summary>
        /// Method to evaluate move on game
        /// </summary>
        /// <param name="g">Game on which move to be evaluated</param>
        /// <param name="move">Move to be evaluated</param>
        /// <returns>Evaluation of move</returns>
        protected override int EvaluateMove(Game g, GameMove move)
        {
            if (move == null || g == null) return 0;

            int winner = g.Table.CheckFinished(g.Turn);
            if (winner != 0)
            {
                if (this.playerTurn + 1 == winner) return 1000; else return -1000;
            }

            if (move.Type == GameMove.MoveType.FigurePlacement)
            {
                return 0;
            }
            else
            {

                GamePlayer me = g.GetPlayer(this.playerTurn);
                GamePlayer op = g.GetPlayer((this.playerTurn + 1) % 2);

                int MyFigurePositionValue = FigurePositionValue(me.Figures[0], g) + FigurePositionValue(me.Figures[1], g);
                int OpFigurePositionValue = FigurePositionValue(op.Figures[0], g) + FigurePositionValue(op.Figures[1], g);

                return MyFigurePositionValue - OpFigurePositionValue;

            }

        }

        private int FigurePositionValue(GameFigure figure, Game g)
        {
            int breadth = 0;
            int res = 0;

            while (breadth < 5)
            {
                if (breadth == 0)
                {
                    res += 50 * (g.Table.GetTableValueAt(figure.I, figure.J)[0]);
                }
                else
                {
                    if (figure.I - breadth >= 0 && figure.J - breadth >= 0) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I - breadth, figure.J - breadth)[0]);
                    if (figure.I - breadth >= 0) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I - breadth, figure.J)[0]);
                    if (figure.I - breadth >= 0 && figure.J + breadth <= 4) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I - breadth, figure.J + breadth)[0]);
                    if (figure.J - breadth >= 0) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I, figure.J - breadth)[0]);
                    if (figure.J + breadth <= 4) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I, figure.J + breadth)[0]);
                    if (figure.I + breadth <= 4 && figure.J - breadth >= 0) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I + breadth, figure.J - breadth)[0]);
                    if (figure.I + breadth <= 4) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I + breadth, figure.J)[0]);
                    if (figure.I + breadth <= 4 && figure.J + breadth <= 4) res += (5 - breadth) * (g.Table.GetTableValueAt(figure.I + breadth, figure.J + breadth)[0]);
                }
                breadth++;
            }

            return res;
        }
    }
}
