using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace etf.santorini.sn160078d
{
    public class Game
    {
        private GameState state;
        private GameTable table;
        private GamePlayer[] players; 
        private List<GameMove> moves;
        private GameMove currentMove;
        private int turn;
        private int winner;

        public Game(GamePlayer[] players)
        {
            this.players = players;
            this.turn = 0;
            this.winner = 0;
            this.moves = new List<GameMove>();
            this.currentMove = null;
            this.table = new GameTable();
            this.State = GameState.WaitingForPlayer1ToPlaceFigure1;
        }

        /*public Game(Game copy)
        {
            this.players = new GamePlayer[2];
            this.players[0] = GamePlayer.CreateCopy(copy.players[0]);
            this.players[1] = GamePlayer.CreateCopy(copy.players[1]);
            this.turn = copy.turn;
            this.winner = copy.winner;
            this.moves = new List<GameMove>();
            foreach (GameMove move in copy.moves)
            {
                this.moves.Add(new GameMove(move.Type, move.FromI, move.FromJ, move.ToI, move.ToJ, move.BuildI, move.BuildJ, move.Player));
            }
            this.table = GameTable.CreateCopy(copy.table);
            this.table.SetFigure(this.players[0].Figures[0], 0, 0);
            this.table.SetFigure(this.players[0].Figures[1], 0, 1);
            this.table.SetFigure(this.players[1].Figures[0], 1, 0);
            this.table.SetFigure(this.players[1].Figures[1], 1, 1);
            this.state = copy.state;
        }
        */

        public GameState State { get => state; set => state = value; }
        public GameTable Table { get => table; set => table = value; }
        public int Turn { get => turn; set => turn = value; }
        public int Winner { get => winner; set => winner = value; }

        private void nextTurn()
        {
            turn++;
            turn %= 2;
        }

        private void nextState()
        {
            if (this.state == GameState.Finished) return;

            if (this.state == GameState.WaitingForPlayer1Move)
            {
                this.state = GameState.WaitingForPlayer2Move;
            }
            else if (this.state == GameState.WaitingForPlayer2Move)
            {
                this.state = GameState.WaitingForPlayer1Move;
            }
            else
            {
                this.state = this.state + 1;
            }
        }

        public GamePlayer GetCurrentPlayer()
        {
            return players[turn];
        }

        public GamePlayer GetPlayer(int i)
        {
            return this.players[i];
        }
        
        public void PlaceFigure(int x, int y)
        {
            if (table.IsFreeSpot(x, y))
            {
                GameFigure f = new GameFigure(x, y);

                players[turn].PlaceFigure(table, f);
                this.moves.Add(new GameMove(GameMove.MoveType.FigurePlacement, -1, -1, x, y, -1, -1, this.turn, this.state));

                nextTurn();
                nextState();
            }       
        }

        public bool MoveFigure(GameFigure selected, int i, int j)
        {

            if (players[turn].PlayersFigure(selected) && table.ValidMove(selected, i, j))
            {

                this.currentMove = new GameMove(GameMove.MoveType.FigureMovingAndBuilding, selected.X, selected.Y, i, j, -1, -1, this.turn, this.state);
                selected.MoveTo(i, j);

                if ((this.winner = this.table.CheckFinished(turn)) != 0)
                {
                    this.state = GameState.Finished;
                }

                return true;
            }

            return false;
        }

        public bool Build(GameFigure selected, int i, int j)
        {
            if (players[turn].PlayersFigure(selected) && table.ValidBuild(selected, i, j))
            {
                this.currentMove.BuildI = i;
                this.currentMove.BuildJ = j;
                this.moves.Add(this.currentMove);

                this.table.Build(i, j);
                nextTurn();
                nextState();

                if ((this.winner = this.table.CheckFinished(turn)) != 0)
                {
                    this.state = GameState.Finished;
                }

                return true;
            }

            return false;
        }

        public void PlayMove(GameMove moveToPlay)
        {
            if (moveToPlay == null) return;

            if (moveToPlay.Type == GameMove.MoveType.FigurePlacement)
            {
                this.PlaceFigure(moveToPlay.ToI, moveToPlay.ToJ);
            }
            else
            {
                GameFigure figure = this.table.FigureAt(moveToPlay.FromI, moveToPlay.FromJ);
                this.MoveFigure(figure, moveToPlay.ToI, moveToPlay.ToJ);
                this.Build(figure, moveToPlay.BuildI, moveToPlay.BuildJ);
            }
        }

        public void UndoMove(GameMove newMove)
        {
            if (newMove.Type == GameMove.MoveType.FigurePlacement)
            {
                GamePlayer player = this.GetPlayer(newMove.Player);
                player.RemoveFigure(this.table, newMove.ToI, newMove.ToJ);
            }
            else
            {
                GameFigure figure = this.table.FigureAt(newMove.ToI, newMove.ToJ);
                figure.MoveTo(newMove.FromI, newMove.FromJ);
                this.table.Unbuild(newMove.BuildI, newMove.BuildJ);
            }

            this.moves.RemoveAt(this.moves.Count - 1);
            nextTurn();
            if (this.moves.Count != 0)
            {
                this.state = this.moves.Last().GameState;
            }
            else
            {
                this.state = GameState.WaitingForPlayer1ToPlaceFigure1;
            }
                
        }

        public void PlayNext()
        {
            this.players[turn].PlayNextMove(this);
        }

        public bool IsFinished()
        {
            return this.state == GameState.Finished;
        }

        public enum GameState
        {
            WaitingForPlayer1ToPlaceFigure1,

            WaitingForPlayer2ToPlaceFigure1,

            WaitingForPlayer1ToPlaceFigure2,

            WaitingForPlayer2ToPlaceFigure2,

            WaitingForPlayer1Move,

            WaitingForPlayer2Move,

            Finished
        }
    }
}
