using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class StrategyEasy : Strategy
    {
        public StrategyEasy(int depth, int playerTurn) : base(depth, playerTurn)
        {
        }

        public override Tuple<int, GameMove> PlayNextMove(Game g, GameMove move, int currentDepth)
        {
            if (currentDepth == this.depth)
            {
                int ret = EvaluateMove(g, move);
                return Tuple.Create<int, GameMove>(ret, null);
            }
            
            List<GameMove> moves = this.GetValidMoves(g);

            int bestValue = (currentDepth % 2 == 0) ? int.MinValue : int.MaxValue;
            GameMove moveToPlay = null;

            /*foreach (GameMove newMove in moves)
            {
                System.Console.Write(newMove + "|");
            }
            System.Console.WriteLine(); */

            foreach (GameMove newMove in moves)
            {
                Game.GameState prev = g.State;
                g.PlayMove(newMove);
                Tuple<int, GameMove> ret = PlayNextMove(g, newMove, currentDepth + 1);
                int retVal = ret.Item1;
                g.UndoMove(newMove);
                if (prev != g.State) System.Console.WriteLine("NIJE ISTO");
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

            //g.PlayMove(moveToPlay);

            return Tuple.Create<int, GameMove>(bestValue, moveToPlay);
        }

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

            int myDistance = Math.Max(Math.Abs(me.Figures[0].X - move.BuildI), Math.Abs(me.Figures[0].Y - move.BuildJ)) + Math.Max(Math.Abs(me.Figures[1].X - move.BuildI), Math.Abs(me.Figures[1].Y - move.BuildJ));
            int opDistance = Math.Max(Math.Abs(op.Figures[0].X - move.BuildI), Math.Abs(op.Figures[0].Y - move.BuildJ)) + Math.Max(Math.Abs(op.Figures[1].X - move.BuildI), Math.Abs(op.Figures[1].Y - move.BuildJ));

            int l = ((g.Table.GetTableValueAt(move.BuildI, move.BuildJ))[0] + 1) * (opDistance - myDistance);

            return m + l;
        }
    }
}
