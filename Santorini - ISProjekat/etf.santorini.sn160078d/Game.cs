using System;
using System.Collections.Generic;
using System.IO;
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

        public GameState State { get => state; set => state = value; }
        public GameTable Table { get => table; set => table = value; }
        public int Turn { get => turn; set => turn = value; }
        public int Winner { get => winner; set => winner = value; }

        public void LoadGame(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                string player1Figure2 = null, player2Figure2 = null;
                GameMove move;

                while ((line = sr.ReadLine()) != null)
                {             

                    if (this.state == GameState.WaitingForPlayer1ToPlaceFigure2)
                    {
                        move = new GameMove(player1Figure2, this.turn, this.state);
                        this.PlayMove(move, false);
                    }
                    
                    if (this.state == GameState.WaitingForPlayer2ToPlaceFigure2)
                    {
                        move = new GameMove(player2Figure2, this.turn, this.state);
                        this.PlayMove(move, false);
                    }

                    if (line.Length == 5)
                    {
                        if (this.state == GameState.WaitingForPlayer1ToPlaceFigure1) player1Figure2 = line.Substring(3);
                        if (this.state == GameState.WaitingForPlayer2ToPlaceFigure1) player2Figure2 = line.Substring(3);

                        line = line.Substring(0, 2);
                    }

                    move = new GameMove(line, this.turn, this.state);
                    this.PlayMove(move, false);
                }
            }
        }

        public void SaveGame(string file = null)
        {
            DateTime time = DateTime.UtcNow;
            string folder = "GameSaves";
            string fileName = "SaveID-" + time.ToString().Replace(':', '-') + ".txt";
            string path = Path.Combine(folder, fileName);

            Directory.CreateDirectory(folder);

            if (file != null) path = file;

            using (FileStream fs = File.Create(path))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string firstLine = "";
                    string secondLine = "";

                    for (int i = 0; i <= 3; i++)
                    {
                        if (this.moves.Count > i)
                        {
                            if (i % 2 == 0)
                            {
                                if (i == 2) firstLine += " ";
                                firstLine += this.moves[i].ToString();
                            }
                            else
                            {
                                if (i == 3) secondLine += " ";
                                secondLine += this.moves[i].ToString();
                            }
                        }
                    }

                    sw.WriteLine(firstLine);
                    sw.WriteLine(secondLine);

                    for (int i = 4; i < this.moves.Count; i++)
                    {
                        sw.WriteLine(this.moves[i].ToString());
                    }

                }
            }
            
        }

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

        private void previousState()
        {
            if (this.moves.Count != 0)
            {
                this.state = this.moves.Last().GameState;
            }
            else
            {
                this.state = GameState.WaitingForPlayer1ToPlaceFigure1;
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
        
        public bool PlaceFigure(int x, int y)
        {
            if (table.IsFreeSpot(x, y) && x >= 0 && x <= 4 && y >= 0 && y <= 4)
            {
                GameFigure f = new GameFigure(x, y);

                players[turn].PlaceFigure(table, f);
                this.moves.Add(new GameMove(GameMove.MoveType.FigurePlacement, -1, -1, x, y, -1, -1, this.turn, this.state));

                nextTurn();
                nextState();

                return true;
            }

            return false;
        }

        public bool MoveFigure(GameFigure selected, int i, int j, bool minimax)
        {

            if (players[turn].PlayersFigure(selected) && table.ValidMove(selected, i, j))
            {

                this.currentMove = new GameMove(GameMove.MoveType.FigureMovingAndBuilding, selected.X, selected.Y, i, j, -1, -1, this.turn, this.state);
                selected.MoveTo(i, j);

                if ((this.winner = this.table.CheckFinished(turn)) != 0)
                {
                    if (!minimax)
                    {
                        this.moves.Add(this.currentMove);
                        this.SaveGame();
                    }
                    this.state = GameState.Finished;
                }

                return true;
            }

            return false;
        }
    
        public bool Build(GameFigure selected, int i, int j, bool minimax)
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
                    if (!minimax)
                    {
                        this.SaveGame();
                    }
                    this.state = GameState.Finished;
                }

                return true;
            }

            return false;
        }

        public void PlayMove(GameMove moveToPlay, bool minimax)
        {
            if (moveToPlay == null) return;

            if (moveToPlay.Type == GameMove.MoveType.FigurePlacement)
            {
                this.PlaceFigure(moveToPlay.ToI, moveToPlay.ToJ);
            }
            else
            {
                GameFigure figure = this.table.FigureAt(moveToPlay.FromI, moveToPlay.FromJ);
                this.MoveFigure(figure, moveToPlay.ToI, moveToPlay.ToJ, minimax);
                this.Build(figure, moveToPlay.BuildI, moveToPlay.BuildJ, minimax);
            }
        }

        public void UndoMove(GameMove newMove)
        {
            if (this.moves.Count == 0) return;

            GameMove lastMove = this.moves.ElementAt(this.moves.Count - 1);

            //System.Console.WriteLine("Played : " + newMove.ToString() + " | Undo: " + lastMove.ToString());

            if (lastMove.Type == GameMove.MoveType.FigurePlacement)
            {
                GamePlayer player = this.GetPlayer(lastMove.Player);
                player.RemoveFigure(this.table, lastMove.ToI, lastMove.ToJ);
            }
            else
            {
                GameFigure figure = this.table.FigureAt(lastMove.ToI, lastMove.ToJ);
                this.table.Unbuild(lastMove.BuildI, lastMove.BuildJ);
                figure.MoveTo(lastMove.FromI, lastMove.FromJ);
            }

            nextTurn();
            previousState();

            this.moves.RemoveAt(this.moves.Count - 1);
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
