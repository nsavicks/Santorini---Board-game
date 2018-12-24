using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public class GameMove
    {
        private readonly MoveType type;
        private int fromI, fromJ, toI, toJ, buildI, buildJ;
        private readonly int player;
        private readonly Game.GameState gameState;

        public int FromI { get => fromI; set => fromI = value; }
        public int FromJ { get => fromJ; set => fromJ = value; }
        public int ToI { get => toI; set => toI = value; }
        public int ToJ { get => toJ; set => toJ = value; }
        public int BuildI { get => buildI; set => buildI = value; }
        public int BuildJ { get => buildJ; set => buildJ = value; }
        public int Player => player;
        public MoveType Type => type;
        public Game.GameState GameState => gameState;

        public GameMove(MoveType type, int fromI, int fromJ, int toI, int toJ, int buildI, int buildJ, int player, Game.GameState gameState)
        {
            this.type = type;
            this.fromI = fromI;
            this.fromJ = fromJ;
            this.toI = toI;
            this.toJ = toJ;
            this.buildI = buildI;
            this.buildJ = buildJ;
            this.player = player;
            this.gameState = gameState;
        }

        public override string ToString()
        {
            string sol = "";

            if (this.type == MoveType.FigurePlacement)
            {
                sol += Char.ConvertFromUtf32(toI + 65);
                sol += (toJ + 1);
            }
            else if (this.type == MoveType.FigureMovingAndBuilding)
            {
                sol += Char.ConvertFromUtf32(fromI + 65);
                sol += fromJ + 1;
                sol += " ";
                sol += Char.ConvertFromUtf32(toI + 65);
                sol += toJ + 1;
                sol += " ";
                sol += Char.ConvertFromUtf32(buildI + 65);
                sol += buildJ + 1;
            }

            return sol;
        }

        public enum MoveType
        {
            FigurePlacement,

            FigureMovingAndBuilding,
        }
        
    }
}
