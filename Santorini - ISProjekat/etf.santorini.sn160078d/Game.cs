using etf.santorini.sn160078d.Exceptions;
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
    /// <summary>
    /// Class that represents main gameplay of Santorini game
    /// </summary>
    public class Game
    {
        private GameState state;
        private GameTable table;
        private GamePlayer[] players; 
        private List<GameMove> moves;
        private GameMove currentMove;
        private int turn;
        private int winner;

        /// <summary>
        /// Gets or sets current state of game
        /// </summary>
        public GameState State { get => state; set => state = value; }

        /// <summary>
        /// Gets or sets game table for game
        /// </summary>
        public GameTable Table { get => table; set => table = value; }

        /// <summary>
        /// Gets or sets current player turn for game
        /// </summary>
        public int Turn { get => turn; set => turn = value; }

        /// <summary>
        /// Gets or sets winner of game
        /// </summary>
        public int Winner { get => winner; set => winner = value; }

        /// <summary>
        /// Ctor for Game class 
        /// </summary>
        /// <param name="players">Players that will play game</param>
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

        /// <summary>
        /// Method for loading game from file
        /// </summary>
        /// <param name="file">Filepath from which to load game</param>
        public void LoadGame(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                string first, second;
                GameMove move;
                int ind = 0;

                while ((line = sr.ReadLine()) != null)
                {             
                    if (ind <= 1)
                    {
                        first = line.Substring(0, 2);
                        second = line.Substring(3, 2);

                        move = new GameMove(first, this.turn, this.state);
                        this.PlayMove(move, false);

                        move = new GameMove(second, this.turn, this.state);
                        this.PlayMove(move, false);
                    }
                    else
                    {
                        move = new GameMove(line, this.turn, this.state);
                        this.PlayMove(move, false);
                    }

                    ind++;
                }
            }

        }

        /// <summary>
        /// Method for saving game
        /// </summary>
        /// <param name="file">Filepath to file in which to save game</param>
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

                    for (int i = 0; i <= 3; i++)
                    {
                        if (this.moves.Count > i)
                        {
                            if (i == 0 || i == 2)
                            {
                                sw.Write(this.moves[i].ToString());
                            }
                            else
                            {
                                sw.WriteLine(" " + this.moves[i].ToString());
                            }
                        }
                    }

                    for (int i = 4; i < this.moves.Count; i++)
                    {
                        sw.WriteLine(this.moves[i].ToString());
                    }

                }
            }
            
        }

        /// <summary>
        /// Change to next player turn
        /// </summary>
        private void nextTurn()
        {
            if (this.state != GameState.WaitingForPlayer1ToPlaceFigure1 && this.state != GameState.WaitingForPlayer2ToPlaceFigure1)
            {
                turn++;
                turn %= 2;
            }
        }

        /// <summary>
        /// Change to next game state
        /// </summary>
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

        /// <summary>
        /// Change to previous game state
        /// </summary>
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

        /// <summary>
        /// Get player that is currently on move
        /// </summary>
        /// <returns>Player currently on move</returns>
        public GamePlayer GetCurrentPlayer()
        {
            return players[turn];
        }

        /// <summary>
        /// Get player for given player turn
        /// </summary>
        /// <param name="pt">Player turn</param>
        /// <returns></returns>
        public GamePlayer GetPlayer(int pt)
        {
            return this.players[pt];
        }
        
        /// <summary>
        /// Method for placing figure in game
        /// </summary>
        /// <param name="i">Row of game table</param>
        /// <param name="j">Column of game table</param>
        /// <returns></returns>
        public bool PlaceFigure(int i, int j)
        {
            if (table.IsFreeSpot(i, j) && i >= 0 && i <= 4 && j >= 0 && j <= 4)
            {
                GameFigure f = new GameFigure(i, j);

                players[turn].PlaceFigure(table, f);
                this.moves.Add(new GameMove(GameMove.MoveType.FigurePlacement, -1, -1, i, j, -1, -1, this.turn, this.state));

                nextTurn();
                nextState();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for moving figure in game
        /// </summary>
        /// <param name="selected">Figure to move</param>
        /// <param name="i">Row of game table where to move figure</param>
        /// <param name="j">Column of game table where to move figure</param>
        /// <param name="minimax">If this method is called from minimax algorithm or real move</param>
        /// <returns></returns>
        public bool MoveFigure(GameFigure selected, int i, int j, bool minimax)
        {

            if (players[turn].PlayersFigure(selected) && table.ValidMove(selected, i, j))
            {

                this.currentMove = new GameMove(GameMove.MoveType.FigureMovingAndBuilding, selected.I, selected.J, i, j, -1, -1, this.turn, this.state);
                this.moves.Add(this.currentMove);

                selected.MoveTo(i, j);

                if ((this.winner = this.table.CheckFinished(turn)) != 0)
                {
                    if (this.GetCurrentPlayer().Type == GamePlayer.PlayerType.Human && !minimax)
                    {
                        this.SaveGame();
                    }
                    this.state = GameState.Finished;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method for building in game
        /// </summary>
        /// <param name="selected">Figure which builds</param>
        /// <param name="i">Row of game table where to build</param>
        /// <param name="j">Column of game table where to build</param>
        /// <param name="minimax">If this method is called from minimax algorithm or real move</param>
        /// <returns></returns>
        public bool Build(GameFigure selected, int i, int j, bool minimax)
        {
            if (players[turn].PlayersFigure(selected) && table.ValidBuild(selected, i, j))
            {
                this.currentMove.BuildI = i;
                this.currentMove.BuildJ = j;
                
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

        /// <summary>
        /// Method for playing given move on this game
        /// </summary>
        /// <param name="moveToPlay">Move to be played</param>
        /// <param name="minimax">If this method is called from minimax algorithm or real move</param>
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
                if (figure == null) throw new InvalidMove();
                this.MoveFigure(figure, moveToPlay.ToI, moveToPlay.ToJ, minimax);
                if (moveToPlay.BuildI != -1)
                {
                    this.Build(figure, moveToPlay.BuildI, moveToPlay.BuildJ, minimax);
                }  
            }
        }

        /// <summary>
        /// Method to undo last move
        /// </summary>
        public void UndoMove()
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
                if (lastMove.BuildI != -1)
                {
                    this.table.Unbuild(lastMove.BuildI, lastMove.BuildJ);
                }
                figure.MoveTo(lastMove.FromI, lastMove.FromJ);
            }

            if (this.state != GameState.WaitingForPlayer1ToPlaceFigure2 && this.state != GameState.WaitingForPlayer2ToPlaceFigure2)
            {
                this.turn++;
                this.turn %= 2;
            }
            previousState();

            this.moves.RemoveAt(this.moves.Count - 1);
        }

        /// <summary>
        /// Method to call player who's turn is it to play next move
        /// </summary>
        /// <returns>Evaluation of move</returns>
        public int PlayNext()
        {
            return this.players[turn].PlayNextMove(this);
        }

        /// <summary>
        /// Method to check if game is finished
        /// </summary>
        /// <returns>True if game is finished</returns>
        public bool IsFinished()
        {
            return this.state == GameState.Finished;
        }

        /// <summary>
        /// Method to get string representation of game
        /// </summary>
        /// <returns>String representation of game</returns>
        public override string ToString()
        {
            string res = "";
            res += "state:" + this.state;
            res += "table:" + this.table.ToString();
            res += "turn:" + this.turn;
            res += "Moves:";
            foreach(GameMove move in this.moves)
            {
                res += move.ToString();
            }

            return res;
        }

        /// <summary>
        /// Enum that represents game states
        /// </summary>
        public enum GameState
        {
            /// <summary>
            /// Game state waiting for first player to place first figure
            /// </summary>
            WaitingForPlayer1ToPlaceFigure1,

            /// <summary>
            /// Game state waiting for first player to place second figure
            /// </summary>
            WaitingForPlayer1ToPlaceFigure2,

            /// <summary>
            /// Game state waiting for second player to place first figure
            /// </summary>
            WaitingForPlayer2ToPlaceFigure1,

            /// <summary>
            /// Game state waiting for second player to place second figure
            /// </summary>
            WaitingForPlayer2ToPlaceFigure2,

            /// <summary>
            /// Game state waiting for first player to make move
            /// </summary>
            WaitingForPlayer1Move,

            /// <summary>
            /// Game state waiting for second player to make move
            /// </summary>
            WaitingForPlayer2Move,

            /// <summary>
            /// Game state for finished game
            /// </summary>
            Finished
        }
    }
}
