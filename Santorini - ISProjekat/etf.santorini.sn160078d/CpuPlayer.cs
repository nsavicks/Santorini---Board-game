using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Interface for CPU Player
    /// </summary>
    public class CpuPlayer : GamePlayer
    {

        private Strategy strat;
        private int depth;

        /// <summary>
        /// Ctor for CPUPlayer class
        /// </summary>
        /// <param name="type">Player type</param>
        /// <param name="pt">Player turn</param>
        /// <param name="depth">Depth for minimax tree</param>
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

        /// <summary>
        /// Method for getting and playing next move on game
        /// </summary>
        /// <param name="g">Game on which to play next move</param>
        public override int PlayNextMove(Game g)
        {
            Tuple<int, GameMove> ret = this.strat.PlayNextMove(g, null, 0, int.MinValue, int.MaxValue);
            //System.Console.WriteLine("Best value is " + ret.Item1);
            g.PlayMove(ret.Item2, false);
            return ret.Item1;
        }
    }
}
