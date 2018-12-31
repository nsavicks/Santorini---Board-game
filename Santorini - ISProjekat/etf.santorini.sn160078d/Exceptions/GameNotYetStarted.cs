using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d.Exceptions
{
    /// <summary>
    /// Exception when game is not yet started
    /// </summary>
    public class GameNotYetStarted: Exception
    {
        /// <summary>
        /// String description of exception
        /// </summary>
        /// <returns>String description of exception</returns>
        public override string ToString()
        {
            return "Game not yet started!";
        }
    }
}
