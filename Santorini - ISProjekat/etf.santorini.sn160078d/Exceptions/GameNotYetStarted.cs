using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d.Exceptions
{
    public class GameNotYetStarted: Exception
    {
        public override string ToString()
        {
            return "Game not yet started!";
        }
    }
}
