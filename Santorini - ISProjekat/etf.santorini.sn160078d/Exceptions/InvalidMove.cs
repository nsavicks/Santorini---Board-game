using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d.Exceptions
{
    /// <summary>
    /// Exception when user tries to make invalid move
    /// </summary>
    public class InvalidMove: Exception
    {
        /// <summary>
        /// String description of exception
        /// </summary>
        /// <returns>String description of exception</returns>
        public override string ToString()
        {
            return "Invalid move!";
        }
    }
}
