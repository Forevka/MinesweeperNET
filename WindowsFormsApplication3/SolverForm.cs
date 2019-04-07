using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{

    public class Panel
    {
        public int AdjacentMines { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public Point pos;

        public Panel(int x, int y, bool is_rev, bool is_flag, int mines)
        {
            this.pos = new Point(x, y);
            this.IsRevealed = is_rev;
            this.IsFlagged = is_flag;
            this.AdjacentMines = mines;
        }
    }

    public partial class SolverForm : Form
    {
        private MainForm father;
        private Game game_instance;
        private List<Panel> board_info;
        private List<Point> history;
        private Random rnd = new Random();
        private int move_number = 0;

        public SolverForm(MainForm father)
        {
            InitializeComponent();
            this.father = father;
            game_instance = father.g;
            this.board_info = get_board_info();
            Console.WriteLine(this.board_info.Count);
            history = new List<Point>();
            draw_array(this.board_info);
            button1.Text = "First Move";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(is_game_end());
            //clear_groups();
            //Console.WriteLine(group_list.Count);
            List<Point> possible_moves = new List<Point>();
            write_history(String.Format("-- Starting move {0} --", move_number));
            if (move_number == 0)//first move
            {
                possible_moves.Add(new Point(0, 0));
                possible_moves.Add(new Point(0, game_instance.field_height-1));
                possible_moves.Add(new Point(game_instance.field_width-1, 0));
                possible_moves.Add(new Point(game_instance.field_width-1, game_instance.field_height-1));
                int r = rnd.Next(possible_moves.Count);
                Point rnd_point = possible_moves[r];
                MyButton btn = game_instance.field_stats[rnd_point.X, rnd_point.Y];
                send_click(btn, 0);
                write_history(String.Format("clicked on x - {0}, y - {1}", rnd_point.X, rnd_point.Y));
                
            }
            else 
            {
                FlagObviousMines();
                if (HasAvailableMoves())
                {
                    ObviousNumbers();
                    write_history(String.Format("Has available move"));
                }
                else
                {
                    RandomMove();
                    write_history(String.Format("Dont have moves. Random we trust"));
                }
                Endgame();
                //ObviousNumbers();
            }
            this.board_info = get_board_info();
            draw_array(this.board_info);
            write_history(String.Format("-- Ending move {0} --", move_number));
            if (!Game.end)
            {
                move_number++;
                button1.Text = String.Format("Move {0}", move_number);
            }else 
            {
                if (Game.win)
                    button1.Text = "Win";
                else
                    button1.Text = "Defeat";
                button1.Enabled = false;
            }
            
        }
        public List<Panel> GetNeighbors(Panel p)
        {
            return GetNeighbors(p.pos.X, p.pos.Y, 1);
        }

        public List<Panel> GetNeighbors(int x, int y, int depth)
        {
            var board = this.get_board_info();
            var nearbyPanels = board.Where(panel => panel.pos.X >= (x - depth) && panel.pos.X <= (x + depth)
                                                    && panel.pos.Y >= (y - depth) && panel.pos.Y <= (y + depth));
            var currentPanel = board.Where(panel => panel.pos.X == x && panel.pos.Y == y);
            return nearbyPanels.Except(currentPanel).ToList();
        }

        

        public bool HasAvailableMoves()
        {
            List<Panel> board = this.get_board_info();
            var numberedPanels = board.Where(x => x.IsRevealed && x.AdjacentMines > 0);
            foreach (var numberPanel in numberedPanels)
            {
                var neighborPanels = GetNeighbors(numberPanel);
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);
                //Console.WriteLine("Mster neigh is {0} {1}", numberPanel.pos.X, numberPanel.pos.Y);
                /*foreach (Panel p in flaggedNeighbors)
                    Console.WriteLine("Flagged neigh is {0} {1}", p.pos.X, p.pos.Y);*/
                var not_opened = neighborPanels.Where(x => !x.IsRevealed).Count();
                //Console.WriteLine("Master not opened {0}", not_opened);
                if (flaggedNeighbors.Count() == numberPanel.AdjacentMines && not_opened != flaggedNeighbors.Count())
                {
                    return true;
                }
            }
            return false;
        }

        public void FlagObviousMines()
        {
            List<Panel> board = this.get_board_info();
            var number_panels = board.Where(x => x.IsRevealed && x.AdjacentMines > 0);

            foreach (var panel in number_panels)
            {
                //For each revealed number panel on the board, get its neighbors.
                var neighborPanels = GetNeighbors(panel);

                //If the total number of hidden == number of mines revealed by this panel...
                if (neighborPanels.Count(x => !x.IsRevealed) == panel.AdjacentMines)
                {
                    //All those hidden panels must be mines, so flag them.
                    foreach (var neighbor in neighborPanels.Where(x => !x.IsRevealed))
                    {
                        if(!Game.flagged_btn.Contains(game_instance.field_stats[neighbor.pos.X, neighbor.pos.Y]))
                        {
                            MyButton btn = game_instance.field_stats[neighbor.pos.X, neighbor.pos.Y];
                            send_click(btn, 1);
                            write_history(String.Format("flagged on x - {0}, y - {1}", neighbor.pos.X, neighbor.pos.Y));
                        }
                    }
                }
            }
        }

        

        public void ObviousNumbers()
        {
            var board = this.get_board_info();
            var number_panels = board.Where(x => x.IsRevealed && x.AdjacentMines > 0);

            foreach (var numberPanel in number_panels)
            {
                //Foreach number panel
                var neighborPanels = GetNeighbors(numberPanel);

                //Get all of that panel's flagged neighbors
                var flaggedNeighbors = neighborPanels.Where(x => x.IsFlagged);

                //If the number of flagged neighbors == the number in the current panel...
                if (flaggedNeighbors.Count() == numberPanel.AdjacentMines)
                {
                    //All hidden neighbors must NOT have mines in them, so reveal them.
                    foreach (var hiddenPanel in neighborPanels.Where(x => !x.IsRevealed && !x.IsFlagged))
                    {
                        MyButton btn = game_instance.field_stats[hiddenPanel.pos.X, hiddenPanel.pos.Y];
                        send_click(btn, 0);
                        write_history(String.Format("clicked on x - {0}, y - {1}", hiddenPanel.pos.X, hiddenPanel.pos.Y));
                    }
                }
            }

        }

        public void RandomMove()
        {

            List<Panel> board = this.get_board_info();
            var randomID = rnd.Next(1, board.Count);
            var panel = board[randomID];//board.First(x => x.ID == randomID);
            while (panel.IsRevealed || panel.IsFlagged)
            {
                //We can only reveal an hidden, unflagged panel
                randomID = rnd.Next(1, board.Count);
                panel = board[randomID];//board.Panels.First(x => x.ID == randomID);
            }
            MyButton btn = game_instance.field_stats[panel.pos.X, panel.pos.Y];
            send_click(btn, 0);
            write_history(String.Format("clicked on x - {0}, y - {1}", btn.pos.X, btn.pos.Y));
        }

        public void Endgame()
        {
            //Count all the flagged panels.  If the number of flagged panels == the number of mines on the board, reveal all non-flagged panels.
            var board = this.get_board_info();
            var flaggedPanels = board.Where(x => x.IsFlagged).Count();
            Console.WriteLine("ENDGAME FLAGGED COUNT {0} {1}", flaggedPanels, flaggedPanels == game_instance.bombs_count);

            if (flaggedPanels == game_instance.bombs_count)
            {
                //Reveal all unrevealed, unflagged panels
                
                var unrevealedPanels = board.Where(x => !x.IsFlagged && !x.IsRevealed);
                foreach (var panel in unrevealedPanels)
                {
                    //Board.RevealPanel(panel.X, panel.Y);
                    MyButton btn = game_instance.field_stats[panel.pos.X, panel.pos.Y];
                    send_click(btn, 0);
                    write_history(String.Format("clicked on x - {0}, y - {1}", panel.pos.X, panel.pos.Y));
                }
            }
        }

        private void send_click(MyButton btn, int m_b)
        {
            //0 left 1 right
            var mb = MouseButtons.Left;
            if (m_b == 0)
            {
                mb = MouseButtons.Left;
            }else if (m_b == 1){
                mb = MouseButtons.Right;
            }
            var evt = new MouseEventArgs(mb, 0, 0, 0, 0);
            btn.on_click_up(btn, evt);
        }

        private void write_history(string s)
        {
            MoveHistoryList.Items.Add(s);
        }

        private List<Panel> get_board_info()
        {
            //int[,] board = new int[game_instance.field_width,game_instance.field_height];

            List<Panel> board = new List<Panel>();

            for (int i = 0; i < game_instance.field_width; i++)
            {
                for (int j = 0; j < game_instance.field_height; j++)
                {
                    //Console.WriteLine(btn.pos.X);
                    //Console.WriteLine(btn.pos.Y);
                    MyButton btn = game_instance.field_stats[i, j];

                    //board.Add(new Panel(i,j,));//[i, j] = btn.bomb_neighbours;
                    //Console.WriteLine(btn.bomb_neighbours);
                    if (btn.bomb_neighbours == -1)
                        board.Add(new Panel(i, j, true, false, 0));
                    else if (btn.bomb_neighbours == 0)
                    {
                        if (Game.flagged_btn.Contains(btn))
                            board.Add(new Panel(i, j, false, true, 0));
                        else
                            board.Add(new Panel(i, j, false, false, 0));
                    }
                    else
                    {
                        board.Add(new Panel(i, j, true, false, btn.bomb_neighbours));
                    }
                    
                }
            }
            //Console.WriteLine("BOARD TILE COUNT {0}", board.Count);
            return board;
        }

        private void draw_array(List<Panel> arr)
        {
            SolverEyeList.Items.Clear();
            int rowLength = game_instance.field_width;//arr.GetLength(0);
            int colLength = game_instance.field_height;//arr.GetLength(1);
            
            int[,] matrix = new int[game_instance.field_width, game_instance.field_height];
            foreach (Panel p in arr)
            {
                if (p.IsFlagged)
                    matrix[p.pos.X, p.pos.Y] = -2;//p.AdjacentMines;
                else if (p.IsRevealed && p.AdjacentMines == 0)
                    matrix[p.pos.X, p.pos.Y] = -1;
                else
                    matrix[p.pos.X, p.pos.Y] = p.AdjacentMines;
            }
            string to_write = "";
            //matrix = rotate_by_270(matrix);
            for (int i = 0; i < colLength ;i++)
            {
                for (int j = 0;j < rowLength; j++)
                {
                    to_write += String.Format("{0,3}", matrix[j, i]);
                }
                SolverEyeList.Items.Add(to_write);
                to_write = "\n";
            }
        }

        private bool is_game_end()
        {
            return Game.end;
        }

        private bool is_game_start()
        {
            return Game.start;
        }
    }
}
