using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class CpuPlayer : GamePlayer
    {

        private Strategy strat;
        private int depth;

        public CpuPlayer(PlayerType type, int pt, int depth): base(pt)
        {
            this.type = type;
            this.depth = depth;

            switch (this.type)
            {
                case PlayerType.CPUEasy:
                    this.strat = new StrategyEasy(depth, pt);
                    break;
                case PlayerType.CPUMedium:
                    this.strat = new StrategyMedium(depth, pt);
                    break;
                case PlayerType.CPUHard:
                    this.strat = new StrategyHard(depth, pt);
                    break;
            }
        }

        public int Depth { get => depth; set => depth = value; }

        public override void PlayNextMove(Game g)
        {
            Tuple<int, GameMove> ret = this.strat.PlayNextMove(g, null, 0);
            g.PlayMove(ret.Item2);
        }
    }
}
