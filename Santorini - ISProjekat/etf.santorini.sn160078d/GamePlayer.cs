using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etf.santorini.sn160078d
{
    public abstract class GamePlayer
    {
        protected PlayerType type;
        protected GameFigure[] figures;
        protected int playerTurn;

        public PlayerType Type { get => type; set => type = value; }
        public GameFigure[] Figures { get => figures; set => figures = value; }
        public int PlayerTurn { get => playerTurn; set => playerTurn = value; }

        public GamePlayer(int pt)
        {
            this.figures = new GameFigure[2];
            this.playerTurn = pt;
        }

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

        public void RemoveFigure(GameTable table, int x, int y)
        {
            if (this.figures[0] != null && this.figures[0].OnSpot(x, y))
            {
                this.figures[0] = null;
                table.RemoveFigure(this.playerTurn, 0);
            }
            else if (this.figures[1] != null && this.figures[1].OnSpot(x,y))
            {
                this.figures[1] = null;
                table.RemoveFigure(this.playerTurn, 1);
            }
        }

        public abstract void PlayNextMove(Game game);

        public bool PlayersFigure(GameFigure f)
        {
            for (int i = 0; i < 2; i++)
            {
                if (figures[i] == f) return true;
            }

            return false;
        }

        public enum PlayerType
        {
            Human,
            
            CPUEasy,

            CPUMedium,

            CPUHard 
        }
    }
}
