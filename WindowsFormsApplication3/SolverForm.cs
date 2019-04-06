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
    public class Group
    {
        private Point master;//owner
        private int master_bombs;
        private List<Point> neigh;
        private int[,] field;
        public bool to_delete = false;
        Random rnd = new Random();

        public Group(Point master, int[,] field)
        {
            this.master = master;
            
            neigh = new List<Point>();
            this.field = field;
            create_group();
            show_group();
        }

        public Group(int x, int y, int[,] field)
        {
            this.master = new Point(x, y);
            this.field = field;
        }

        private void create_group()
        {
            int start_x = this.master.X;
            int start_y = this.master.Y;

            int rowLength = this.field.GetLength(0);
            int colLength = this.field.GetLength(1);

            for (int i = -1 + start_x; i < 2 + start_x; i++)
            {
                for (int j = -1 + start_y; j < 2 + start_y; j++)
                {
                    if(i >= 0 && i < rowLength && j>=0 && j<colLength)
                    {
                        if (this.field[i, j] == 0)
                            add_to_group(new Point(i, j));
                    }
                }
            }
        }

        public void compare_group(Group other_group)
        {
            var other_list = other_group.get_group();
            var this_list = this.get_group();

            if (!(other_group.master.X == this.master.X && other_group.master.Y == this.master.Y))
            { 
                var contains = ContainsAllItems(this_list, other_list);
                if(is_full_copy(this_list, other_list))
                {
                    Console.WriteLine("{0} {1} Full copy of {2} {3}", this.master.X, this.master.Y, other_group.master.X, other_group.master.Y);
                    other_group.clear_group();//neigh.Clear();
                }
                if (contains == 1)
                {
                    Console.WriteLine("{0} {1} Contains other group with {2} {3}", this.master.X, this.master.Y, other_group.master.X, other_group.master.Y);
                    if (this_list.Count >= other_list.Count)
                    {
                        //var new_list = this_list.Concat(other_list);
                        foreach(Point p in other_list)
                        {
                            this_list.Remove(p);
                        }
                    }
                }else if(contains == other_list.Count)
                {
                    //other_group.neigh.Clear();
                    //this_list.Clear();
                }else
                {
                    
                }
            }
            this.neigh = this_list;
            //need returning group!!
        }

        public static int ContainsAllItems(List<Point> a, List<Point> b)
        {
            //return !b.Except(a).Any();
            int count = 0;
            /*foreach(Point p in a)
            {
                if()
            }*/
            for(int i = 0; i < a.Count; i++)
            {
                for(int j = 0; j< b.Count; j++)
                {
                    if (a[i] == b[j])
                        count++;
                }
            }
            return count;
        }

        public static bool is_full_copy(List<Point> a, List<Point> b)
        {
            return !b.Except(a).Any();
        }

        public List<Point> get_group()
        {
            return neigh;
        }

        public void clear_group()
        {
            this.neigh.Clear();
        }

        public void show_group()
        {
            foreach(Point p in neigh)
            {
                Console.WriteLine("Master {0} neigh: x {1} y {2}", this.master, p.X, p.Y);
            }
            Console.WriteLine("\n");
        }

        private void add_to_group(Point newbie)
        {
            neigh.Add(newbie);
        }
    }

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

    //public class 

    public partial class SolverForm : Form
    {
        private MainForm father;
        private Game game_instance;
        private List<Panel> board_info;
        private List<Point> history;
        private Random rnd = new Random();
        private List<Group> group_list;
        private int move_number = 0;

        public SolverForm(MainForm father)
        {
            InitializeComponent();
            this.father = father;
            game_instance = father.g;
            this.board_info = get_board_info();
            Console.WriteLine(this.board_info.Count);
            group_list = new List<Group>();
            history = new List<Point>();
            draw_array(this.board_info);
            button1.Text = "First Move";
        }

        private void clear_groups()
        {
            group_list.Clear();
        }

        private void append_group(Group n)
        {
            group_list.Add(n);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(is_game_end());
            clear_groups();
            Console.WriteLine(group_list.Count);
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
                }
                else
                {
                    RandomMove();
                }
                Endgame();
                //ObviousNumbers();
            }
            this.board_info = get_board_info();
            draw_array(this.board_info);
            write_history(String.Format("-- Ending move {0} --", move_number));
            move_number++;
            button1.Text = String.Format("Move {0}", move_number);
            
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

                if (flaggedNeighbors.Count() == numberPanel.AdjacentMines)
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

                //If the total number of hidden == the number of mines revealed by this panel...
                if (neighborPanels.Count(x => !x.IsRevealed) == panel.AdjacentMines)
                {
                    //All those hidden panels must be mines, so flag them.
                    foreach (var neighbor in neighborPanels.Where(x => !x.IsRevealed))
                    {
                        game_instance.field_stats[neighbor.pos.X, neighbor.pos.Y].set_flagged();
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

                //If the number of flagged neighbors equals the number in the current panel...
                if (flaggedNeighbors.Count() == numberPanel.AdjacentMines)
                {
                    //All hidden neighbors must *not* have mines in them, so reveal them.
                    foreach (var hiddenPanel in neighborPanels.Where(x => !x.IsRevealed && !x.IsFlagged))
                    {
                        MyButton btn = game_instance.field_stats[hiddenPanel.pos.X, hiddenPanel.pos.Y];
                        send_click(btn, 0);
                        write_history(String.Format("clicked on x - {0}, y - {1}", hiddenPanel.pos.X, hiddenPanel.pos.Y));
                    }
                }
            }

        }

        public void Endgame()
        {
            //Count all the flagged panels.  If the number of flagged panels == the number of mines on the board, reveal all non-flagged panels.
            var board = this.get_board_info();
            var flaggedPanels = board.Where(x => x.IsFlagged).Count();
            Console.WriteLine("ENDGAME FLAGGED COUNT {0}", flaggedPanels);
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
