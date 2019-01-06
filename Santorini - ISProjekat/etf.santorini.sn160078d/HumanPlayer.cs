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
        /// <summary>
        /// Ctor for HumanPlayer
        /// </summary>
        /// <param name="pt"></param>
        public HumanPlayer(int pt) : base(pt)
        {
            this.type = PlayerType.Human;
        }

        /// <summary>
        /// Method for generating and playing next move
        /// </summary>
        /// <param name="g">Game on which to play next move</param>
        /// <returns>Evaluation of played move</returns>
        public override int PlayNextMove(Game g)
        {
            return 0;
        }

    }
}
