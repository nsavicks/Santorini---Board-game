using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents Human player in Santorini game
    /// </summary>
    public class HumanPlayer : GamePlayer
    {
        public HumanPlayer(int pt) : base(pt)
        {
            this.type = PlayerType.Human;
        }

        public override int PlayNextMove(Game table)
        {
            return 0;
        }

    }
}
