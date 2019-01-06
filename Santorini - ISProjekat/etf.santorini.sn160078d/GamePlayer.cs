using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    /// <summary>
    /// Abstract class that represents player in Santorini game
    /// </summary>
    public abstract class GamePlayer
    {
        protected PlayerType type;
        protected GameFigure[] figures;
        protected int playerTurn;

        /// <summary>
        /// Gets or sets player type
        /// </summary>
        public PlayerType Type { get => type; set => type = value; }

        /// <summary>
        /// Gets or sets player figures
        /// </summary>
        public GameFigure[] Figures { get => figures; set => figures = value; }

        /// <summary>
        /// Gets or sets player's turn in game
        /// </summary>
        public int PlayerTurn { get => playerTurn; set => playerTurn = value; }

        /// <summary>
        /// Ctor for game player
        /// </summary>
        /// <param name="pt">Player's turn in game</param>
        public GamePlayer(int pt)
        {
            this.figures = new GameFigure[2];
            this.playerTurn = pt;
        }

        /// <summary>
        /// Method for adding figure to player
        /// </summary>
        /// <param name="table">Game table on which to place figure</param>
        /// <param name="f">Figure to add and place</param>
        public void PlaceFigure(GameTable table, GameFigure f)
        {
            if (this.Figures[0] == null)
            {
                this.Figures[0] = f;
                table.SetFigure(f, this.playerTurn, 0);
            }
            else
            {
                this.Figures[1] = f;
                table.SetFigure(f, this.playerTurn, 1);
            }
        }

        /// <summary>
        /// Method for removing figure from player if it is his
        /// </summary>
        /// <param name="table">Game table from which to remove figure</param>
        /// <param name="i">Row of figure</param>
        /// <param name="j">Column of figure</param>
        public void RemoveFigure(GameTable table, int i, int j)
        {
            if (this.figures[0] != null && this.figures[0].OnSpot(i, j))
            {
                this.figures[0] = null;
                table.RemoveFigure(this.playerTurn, 0);
            }
            else if (this.figures[1] != null && this.figures[1].OnSpot(i,j))
            {
                this.figures[1] = null;
                table.RemoveFigure(this.playerTurn, 1);
            }
        }

        /// <summary>
        /// Abstract method that playes next move for this player
        /// </summary>
        /// <param name="game">Game on which to play next move</param>
        /// <returns>Evaluation of move</returns>
        public abstract int PlayNextMove(Game game);

        /// <summary>
        /// Method for checking if give figure is owned by this player
        /// </summary>
        /// <param name="f">Game figure that need's to be checked for ownership</param>
        /// <returns>True if this player owns given figure</returns>
        public bool PlayersFigure(GameFigure f)
        {
            for (int i = 0; i < 2; i++)
            {
                if (figures[i] == f) return true;
            }

            return false;
        }

        /// <summary>
        /// Enum that represents player types in Santorini game
        /// </summary>
        public enum PlayerType
        {
            /// <summary>
            /// Player type human
            /// </summary>
            Human,
            
            /// <summary>
            /// Player type CPU easy
            /// </summary>
            CPUEasy,

            /// <summary>
            /// Player type CPU medium
            /// </summary>
            CPUMedium,

            /// <summary>
            /// Player type CPU hard
            /// </summary>
            CPUHard 
        }
    }
}
