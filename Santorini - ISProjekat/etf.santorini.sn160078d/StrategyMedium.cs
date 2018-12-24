using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class StrategyMedium : Strategy
    {
        public StrategyMedium(int depth, int playerTurn) : base(depth, playerTurn)
        {
        }

        public override Tuple<int, GameMove> PlayNextMove(Game g, GameMove move, int currentDepth)
        {
            throw new NotImplementedException();
        }

        protected override int EvaluateMove(Game g, GameMove move)
        {
            throw new NotImplementedException();
        }
    }
}
