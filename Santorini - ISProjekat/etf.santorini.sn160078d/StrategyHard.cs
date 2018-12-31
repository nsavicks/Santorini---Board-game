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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to evaluate move on game
        /// </summary>
        /// <param name="g">Game on which move to be evaluated</param>
        /// <param name="move">Move to be evaluated</param>
        /// <returns>Evaluation of move</returns>
        protected override int EvaluateMove(Game g, GameMove move)
        {
            throw new NotImplementedException();
        }
    }
}
