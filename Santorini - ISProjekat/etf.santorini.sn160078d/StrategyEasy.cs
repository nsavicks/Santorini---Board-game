using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents easy strategy for CPU player
    /// </summary>
    public class StrategyEasy : Strategy
    {
        /// <summary>
        /// Default ctor for strategy easy
        /// </summary>
        /// <param name="depth">Depth of minimax tree</param>
        /// <param name="playerTurn">Players turn for which we are creating minimax tree</param>
        public StrategyEasy(int depth, int playerTurn) : base(depth, playerTurn)
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
            if (currentDepth == this.depth)
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
                    if (retVal >= bestValue)
                    {
                        bestValue = retVal;
                        moveToPlay = newMove;
                    }
                }
                else
                {
                    if (retVal <= bestValue)
                    {
                        bestValue = retVal;
                        moveToPlay = newMove;
                    }
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
            if (move.Type == GameMove.MoveType.FigurePlacement) return 0;

            int winner = g.Table.CheckFinished(g.Turn);
            if (winner != 0)
            {
                if (this.playerTurn + 1 == winner) return int.MaxValue; else return int.MinValue;
            }

            int m = (g.Table.GetTableValueAt(move.ToI, move.ToJ))[0];
            GamePlayer me = g.GetPlayer(this.playerTurn);
            GamePlayer op = g.GetPlayer((this.playerTurn + 1) % 2);

            int myDistance = Math.Max(Math.Abs(me.Figures[0].I - move.BuildI), Math.Abs(me.Figures[0].J - move.BuildJ)) + Math.Max(Math.Abs(me.Figures[1].I - move.BuildI), Math.Abs(me.Figures[1].J - move.BuildJ));
            int opDistance = Math.Max(Math.Abs(op.Figures[0].I - move.BuildI), Math.Abs(op.Figures[0].J - move.BuildJ)) + Math.Max(Math.Abs(op.Figures[1].J - move.BuildI), Math.Abs(op.Figures[1].J - move.BuildJ));

            int l = ((g.Table.GetTableValueAt(move.BuildI, move.BuildJ))[0] + 1) * (opDistance - myDistance);

            return m + l;
        }
    }
}
