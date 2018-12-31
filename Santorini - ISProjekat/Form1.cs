using etf.santorini.sn160078d;
using etf.santorini.sn160078d.Exceptions;
using etf.santorini.sn160078d.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace etf.santorini.sn160078d
{
    public partial class Form1 : Form
    {

        private PictureBox[][] tableView;
        private Game g;
        private GameFigure selected;
        private bool moved;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.selected = null;
            this.moved = false;
            this.timer = new Timer();
            timer.Tick += new EventHandler(TimerElapsed);
            timer.Interval = 1000;

            this.tableView = new PictureBox[5][];
            for (int i = 0; i < 5; i++)
            {
                this.tableView[i] = new PictureBox[5];
                for (int j = 0; j < 5; j++)
                {
                    Control c = this.tableLayoutPanel1.GetControlFromPosition(j, i);
                    this.tableView[i][j] = (PictureBox)c;
                }
            }
        }

        private void RefreshTableView()
        {
            for(int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    GameTable table = g.Table;
                    int[] pos = table.GetTableValueAt(i, j);

                    if (pos[1] == -1)
                    {
                        switch (pos[0])
                        {
                            case 0:
                                this.tableView[i][j].Image = Image.FromFile("IMG/1polje.png");
                                break;
                            case 1:
                                this.tableView[i][j].Image = Image.FromFile("IMG/2polje.png");
                                break;
                            case 2:
                                this.tableView[i][j].Image = Image.FromFile("IMG/3polje.png");
                                break;
                            case 3:
                                this.tableView[i][j].Image = Image.FromFile("IMG/4polje.png");
                                break;
                            case 4:
                                this.tableView[i][j].Image = Image.FromFile("IMG/5polje.png");
                                break;
                        }
                    }
                    else if (pos[1] == 0)
                    {
                        if (selected != null && selected.OnSpot(i, j))
                        {
                            switch (pos[0])
                            {
                                case 0:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa1PlociS.png");
                                    break;
                                case 1:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa2PloceS.png");
                                    break;
                                case 2:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa3PloceS.png");
                                    break;
                                case 3:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa4PloceS.png");
                                    break;
                            }
                        }
                        else
                        {
                            switch (pos[0])
                            {
                                case 0:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa1Ploci.png");
                                    break;
                                case 1:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa2Ploce.png");
                                    break;
                                case 2:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa3Ploce.png");
                                    break;
                                case 3:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/crveniPiratNa4Ploce.png");
                                    break;
                            }
                        }
                        
                    }
                    else
                    {
                        if (selected != null && selected.OnSpot(i, j))
                        {
                            switch (pos[0])
                            {
                                case 0:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa1PlociS.png");
                                    break;
                                case 1:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa2PloceS.png");
                                    break;
                                case 2:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa3PloceS.png");
                                    break;
                                case 3:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa4PloceS.png");
                                    break;
                            }
                        }
                        else
                        {
                            switch (pos[0])
                            {
                                case 0:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa1Ploci.png");
                                    break;
                                case 1:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa2Ploce.png");
                                    break;
                                case 2:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa3Ploce.png");
                                    break;
                                case 3:
                                    this.tableView[i][j].Image = Image.FromFile("IMG/plaviPiratNa4Ploce.png");
                                    break;
                            }
                        }
                        
                    }
                }
            int turn = g.Turn + 1;
            lbPlayersTurn.Text = "Players turn: " + turn.ToString();
            lbGameState.Text = "Game state: " + g.State.ToString();

            if (g.State == Game.GameState.Finished)
            {
                lbGameState.Text += " Winner is Player " + g.Winner.ToString();
            }
        }

        private GamePlayer[] getPlayers()
        {
            GamePlayer[] players = new GamePlayer[2];

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    players[0] = new HumanPlayer(0);
                    break;
                case 1:
                    players[0] = new CpuPlayer(GamePlayer.PlayerType.CPUEasy, 0, (int)depthP1.Value);
                    break;
                case 2:
                    players[0] = new CpuPlayer(GamePlayer.PlayerType.CPUMedium, 0, (int)depthP1.Value);
                    break;
                case 3:
                    players[0] = new CpuPlayer(GamePlayer.PlayerType.CPUHard, 0, (int)depthP1.Value);
                    break;
            }

            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    players[1] = new HumanPlayer(1);
                    break;
                case 1:
                    players[1] = new CpuPlayer(GamePlayer.PlayerType.CPUEasy, 1, (int)depthP2.Value);
                    break;
                case 2:
                    players[1] = new CpuPlayer(GamePlayer.PlayerType.CPUMedium, 1, (int)depthP2.Value);
                    break;
                case 3:
                    players[1] = new CpuPlayer(GamePlayer.PlayerType.CPUHard, 1, (int)depthP2.Value);
                    break;
            }

            return players;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                btnP1Next.Enabled = false;
                depthP1.Enabled = false;
                stepP1.Enabled = false;
            }
            else
            {
                btnP1Next.Enabled = true;
                depthP1.Enabled = true;
                stepP1.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                btnP2Next.Enabled = false;
                depthP2.Enabled = false;
                stepP2.Enabled = false;
            }
            else
            {
                btnP2Next.Enabled = true;
                depthP2.Enabled = true;
                stepP2.Enabled = true;
            }
        }

        private void PictureBoxClickHandler(int i, int j, object sender, EventArgs e)
        {
            try
            {
                if (g == null) throw new GameNotYetStarted();

                if (g.GetPlayer(g.Turn).Type == GamePlayer.PlayerType.Human)
                {
                    if (g.State == Game.GameState.WaitingForPlayer1ToPlaceFigure1
                        || g.State == Game.GameState.WaitingForPlayer1ToPlaceFigure2
                         || g.State == Game.GameState.WaitingForPlayer2ToPlaceFigure1
                         || g.State == Game.GameState.WaitingForPlayer2ToPlaceFigure2)
                    {
                        if (!g.PlaceFigure(i, j)) throw new InvalidMove();
                    }
                    else if (g.State == Game.GameState.WaitingForPlayer1Move || g.State == Game.GameState.WaitingForPlayer2Move)
                    {
                        if (selected == null)
                        {
                            selected = g.Table.FigureAt(i, j);
                            if (selected != null && !g.GetCurrentPlayer().PlayersFigure(selected)) selected = null;
                        }
                        else
                        {
                            if (selected.OnSpot(i, j) && !moved)
                            {
                                selected = null;
                            }
                            else if (!moved)
                            {
                                moved = g.MoveFigure(selected, i, j, false);
                                if (!moved) throw new InvalidMove();
                            }
                            else
                            {
                                if (g.Build(selected, i, j, false))
                                {
                                    moved = false;
                                    selected = null;
                                }
                                else throw new InvalidMove();
                            }
                        }
                    }

                    if (g.IsFinished())
                    {
                        selected = null;
                        moved = false;
                    }
                    RefreshTableView();

                    timer.Start();
                }
            
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            try
            {
                if (g == null) throw new GameNotYetStarted();

                if ((g.Turn == 0 && this.stepP1.Checked) || (g.Turn == 1 && this.stepP2.Checked))
                {
                    this.timer.Stop();
                    return;
                }

                if (!g.IsFinished() && g.GetCurrentPlayer().Type != GamePlayer.PlayerType.Human)
                {
                    if (g.Turn == 0)
                    {
                        btnP1Next_Click(sender, e);
                    }
                    else
                    {
                        btnP2Next_Click(sender, e);
                    }

                    this.timer.Stop();

                    if (g.GetCurrentPlayer().Type != GamePlayer.PlayerType.Human) this.timer.Start();
                }
                else
                {
                    this.timer.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(0,0, sender, e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(0, 1, sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(0, 2, sender, e);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(0, 3, sender, e);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(0, 4, sender, e);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(1, 0, sender, e);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(1, 1, sender, e);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(1, 2, sender, e);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(1, 3, sender, e);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(1, 4, sender, e);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(2, 0, sender, e);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(2, 1, sender, e);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(2, 2, sender, e);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(2, 3, sender, e);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(2, 4, sender, e);
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(3, 0, sender, e);
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(3, 1, sender, e);
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(3, 2, sender, e);
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(3, 3, sender, e);
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(3, 4, sender, e);
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(4, 0, sender, e);
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(4, 1, sender, e);
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(4, 2, sender, e);
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(4, 3, sender, e);
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            PictureBoxClickHandler(4, 4, sender, e);
        }

        private void btnP1Next_Click(object sender, EventArgs e)
        {
            try
            {
                if (g == null) throw new GameNotYetStarted();

                if (g.GetPlayer(0).Type != GamePlayer.PlayerType.Human && g.Turn == 0)
                {
                    g.PlayNext();
                    RefreshTableView();
                    this.timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnP2Next_Click(object sender, EventArgs e)
        {
            try
            {
                if (g == null) throw new GameNotYetStarted();

                if (g.GetPlayer(1).Type != GamePlayer.PlayerType.Human && g.Turn == 1)
                {
                    g.PlayNext();
                    RefreshTableView();
                    this.timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g = new Game(getPlayers());
            RefreshTableView();
            timer.Start();
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                g = new Game(getPlayers());
                g.LoadGame(openFileDialog1.FileName);

                RefreshTableView();
                timer.Start();
            }         
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.g == null) throw new GameNotYetStarted();

                string file = null;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    file = saveFileDialog1.FileName;
                }
                this.g.SaveGame(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
