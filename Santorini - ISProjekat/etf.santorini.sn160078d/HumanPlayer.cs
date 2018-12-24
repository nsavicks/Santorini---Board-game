using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class HumanPlayer : GamePlayer
    {
        public HumanPlayer(int pt) : base(pt)
        {
            this.type = PlayerType.Human;
        }

        public override void PlayNextMove(Game table)
        {
            throw new NotImplementedException();
        }

    }
}
