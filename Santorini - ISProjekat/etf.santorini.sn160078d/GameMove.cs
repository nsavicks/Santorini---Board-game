using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Class that represents one move in game Santorini
    /// </summary>
    public class GameMove
    {
        private readonly MoveType type;
        private int fromI, fromJ, toI, toJ, buildI, buildJ;
        private readonly int player;
        private readonly Game.GameState gameState;

        /// <summary>
        /// Gets or sets row from which we moved figure
        /// </summary>
        public int FromI { get => fromI; set => fromI = value; }

        /// <summary>
        /// Gets or sets column from which we moved figure
        /// </summary>
        public int FromJ { get => fromJ; set => fromJ = value; }

        /// <summary>
        /// Gets or sets row to which we moved figure
        /// </summary>
        public int ToI { get => toI; set => toI = value; }

        /// <summary>
        /// Gets or sets column to which we moved figure
        /// </summary>
        public int ToJ { get => toJ; set => toJ = value; }

        /// <summary>
        /// Gets or sets row where we built
        /// </summary>
        public int BuildI { get => buildI; set => buildI = value; }

        /// <summary>
        /// Gets or sets column where we built
        /// </summary>
        public int BuildJ { get => buildJ; set => buildJ = value; }

        /// <summary>
        /// Gets player that played this move
        /// </summary>
        public int Player => player;

        /// <summary>
        /// Gets type of move
        /// </summary>
        public MoveType Type => type;

        /// <summary>
        /// Gets state in which was game when move was played
        /// </summary>
        public Game.GameState GameState => gameState;

        /// <summary>
        /// Ctor for game move
        /// </summary>
        /// <param name="type">Enum type of game move</param>
        /// <param name="fromI">Row from which we moved figure</param>
        /// <param name="fromJ">Column from which we move figure</param>
        /// <param name="toI">Row to which we moved/placed figure</param>
        /// <param name="toJ">Column to which we moved/placed figure</param>
        /// <param name="buildI">Row to which we built</param>
        /// <param name="buildJ">Column to which we built</param>
        /// <param name="player">Player's turn who made move</param>
        /// <param name="gameState">In which state was game before move is played</param>
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

        /// <summary>
        /// Ctor for getting game move from string
        /// </summary>
        /// <param name="move">String that represents game move</param>
        /// <param name="player">Player's turn who made move</param>
        /// <param name="gameState">In which state was game before move is played</param>
        public GameMove(string move, int player, Game.GameState gameState)
        {
            if (move.Length == 2)
            {
                this.type = MoveType.FigurePlacement;
                this.toI = move[0] - 'A';
                this.toJ = move[1] - '1';
            }
            else
            {
                this.type = MoveType.FigureMovingAndBuilding;
                this.fromI = move[0] - 'A';
                this.fromJ = move[1] - '1';
                this.toI = move[3] - 'A';
                this.toJ = move[4] - '1';
                this.buildI = move[6] - 'A';
                this.buildJ = move[7] - '1';
            }

            this.player = player;
            this.gameState = gameState;
        }

        /// <summary>
        /// String representation of  move
        /// </summary>
        /// <returns>String representation of move</returns>
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

        /// <summary>
        /// Enum that represents type of moves in Santorini game
        /// </summary>
        public enum MoveType
        {
            /// <summary>
            /// Type of figure placement move
            /// </summary>
            FigurePlacement,

            /// <summary>
            /// Type of figure moving and building move
            /// </summary>
            FigureMovingAndBuilding,
        }
        
    }
}
