using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication3.Properties;

namespace WindowsFormsApplication3
{
    public partial class MainForm : Form
    {
        public Game g;
        private ToolStripMenuItem submenu_beginner;
        private ToolStripMenuItem submenu_intermediate;
        private ToolStripMenuItem submenu_expert;
        private ToolStripMenuItem submenu_custom;
        public MainForm()
        {
            InitializeComponent();
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.RenderMode = ToolStripRenderMode.System;
            menuStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menuStrip.Location = new Point(0, 0);
            //
            ToolStripMenuItem menu_game = new ToolStripMenuItem();
            menu_game.Name = "Game";
            menu_game.Text = "Game";
            menuStrip.Items.Add(menu_game);
            //
            ToolStripMenuItem submenu_restart = new ToolStripMenuItem();
            menu_game.DropDownItems.Add(submenu_restart);
            submenu_restart.Name = "New";
            submenu_restart.Text = "New";
            submenu_restart.ShortcutKeys = Keys.F2;
            submenu_restart.Click += this.restart;
            //
            menu_game.DropDownItems.Add(new ToolStripSeparator());
            //
            submenu_beginner = new ToolStripMenuItem();
            menu_game.DropDownItems.Add(submenu_beginner);
            submenu_beginner.Name = "Beginner";
            submenu_beginner.Text = "Beginner";
            submenu_beginner.Click += this.start_new_beginner;
            //
            submenu_intermediate = new ToolStripMenuItem();
            menu_game.DropDownItems.Add(submenu_intermediate);
            submenu_intermediate.Name = "Intermediate";
            submenu_intermediate.Text = "Intermediate";
            submenu_intermediate.Click += this.start_new_intermediate;
            //
            submenu_expert = new ToolStripMenuItem();
            menu_game.DropDownItems.Add(submenu_expert);
            submenu_expert.Name = "Expert";
            submenu_expert.Text = "Expert";
            submenu_expert.Click += this.start_new_expert;
            //
            submenu_custom = new ToolStripMenuItem();
            menu_game.DropDownItems.Add(submenu_custom);
            submenu_custom.Name = "Custom...";
            submenu_custom.Text = "Custom...";
            submenu_custom.Click += this.open_custom_form;
            //
            ToolStripMenuItem menu_solver = new ToolStripMenuItem();
            menu_solver.Name = "Solver";
            menu_solver.Text = "Solver";
            menuStrip.Items.Add(menu_solver);
            //
            ToolStripMenuItem submenu_start_solver = new ToolStripMenuItem();
            menu_solver.DropDownItems.Add(submenu_start_solver);
            submenu_start_solver.Name = "Open Solver";
            submenu_start_solver.Text = "Open Solver";
            submenu_start_solver.ShortcutKeys = Keys.F4;
            submenu_start_solver.Click += this.open_solver_form;
            //
            this.Controls.Add(menuStrip);

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }

        public void calc_size(int f_w, int f_h, int f_h_off, int f_v_off, int b_size)
        {
            this.Size = new Size(4+(b_size) * (f_w+1), 15+f_v_off*2 + ((b_size) * f_h));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.g = new Game(this, 16, 16, 40);
            this.g.start_game();
            marker(false, false, false, true);
        }

        private void open_solver_form(object sender, EventArgs e)
        {
            var myForm = new SolverForm(this);
            myForm.Show();
        }

        private void restart(object sender, EventArgs e)
        {
            this.g.restart_game();
        }

        private void start_new_beginner(object sender, EventArgs e)
        {
            this.g.delete_game();
            this.g = new Game(this, 9, 9, 10);
            this.g.start_game();
            marker(true, false, false, false);
        }

        private void start_new_intermediate(object sender, EventArgs e)
        {
            this.g.delete_game();
            this.g = new Game(this, 16, 16, 40);
            this.g.start_game();
            marker(false, true, false, false);
        }

        private void start_new_expert(object sender, EventArgs e)
        {
            this.g.delete_game();
            this.g = new Game(this, 30, 16, 99);
            this.g.start_game();
            marker(false, false, true, false);
        }

        public void start_custom(int width, int height, int mines_count)
        {
            this.g.delete_game();
            this.g = new Game(this, width, height, mines_count);
            this.g.start_game();
            marker(false, false, false, true);
        }

        private void open_custom_form(object sender, EventArgs e)
        {
            var myForm = new CustomFieldForm(this);
            myForm.Show();
        }

        private void marker(bool beg, bool med, bool expert, bool cust)
        {
            submenu_beginner.Checked = beg;
            submenu_intermediate.Checked = med;
            submenu_expert.Checked = expert;
            submenu_custom.Checked = cust;
        }
    }

    public class Game : Form
    {
        private int time_clock;
        private int strip_menu_offset = 15;
        public int field_width;
        public int field_height;
        public static bool end = false;
        public static bool start = false;
        public int opened = 0;
        public int bombs_count;
        public GameTimer my_timer;
        public Point[] with_bombs;
        public MyButton[,] field_stats;
        public MyButton smiley_button;
        public static Image img_untouched;
        public static Image img_flagged;
        public static Image img_bombed;
        public static Image img_bomb_win;
        public static Image img_no_bomb;
        public static Image[] smiley;
        public static Image[] numbers;
        public static Image[] my_font;
        public static List<MyButton> flagged_btn;
        public static MoveCounter move_counter;
        public PictureBox[] timer_pic;
        public PictureBox[] move_count;
        public MainForm father;
        

        public Game(MainForm father, int width, int height, int bomb_count)
        {
            field_width = width;
            field_height = height;
            this.bombs_count = bomb_count;
            this.father = father;

            with_bombs = new Point[bomb_count];
            field_stats = new MyButton[field_width, field_height];
            load_assets();
            
            
            timer_pic = new PictureBox[3];
            move_count = new PictureBox[3];
            
            /*foreach (Point p in this.with_bombs)
            {
                Console.WriteLine("x {0} y {1}", p.X, p.Y);
            }*/
        }

        public void start_game()
        {
            int button_size = 16;
            int field_horizontal_offset = 2;
            int field_vertical_offset = 50;

            Random rand = new Random();
            MainForm father_form = this.father;
            flagged_btn = new List<MyButton>();

            for (int i = 0; i < 3; i++)
            {
                timer_pic[i] = new PictureBox();
                timer_pic[i].Location = new Point(field_width * button_size - button_size / 2 - (3 - i) * 14, strip_menu_offset+13);
                timer_pic[i].Size = new Size(14, 23);
                timer_pic[i].Image = my_font[0];
                father_form.Controls.Add(timer_pic[i]);

                move_count[i] = new PictureBox();
                move_count[i].Location = new Point(button_size / 2 + i * 14, strip_menu_offset+13);
                move_count[i].Size = new Size(14, 23);
                move_count[i].Image = my_font[0];
                father_form.Controls.Add(move_count[i]);
            }
            move_counter = new MoveCounter(move_count);
            move_counter.MoveCount = this.bombs_count;
            for (int i = 0; i < this.bombs_count; i++)
            {
                Point t = new Point(rand.Next(field_width), rand.Next(field_height));
                while (with_bomb(t))
                {
                    t = new Point(rand.Next(field_width), rand.Next(field_height));
                }
                this.with_bombs[i] = t;
            }

            this.smiley_button = new MyButton(this, -1, -1);
            this.smiley_button.Image = Game.smiley[0];
            this.smiley_button.BackColor = Color.Transparent;
            this.smiley_button.Size = new Size(26, 26);
            this.smiley_button.Location = new Point(field_width * button_size / 2 - 13, strip_menu_offset + 13);
            this.smiley_button.MouseUp += new MouseEventHandler(this.smiley_button.on_click_up_smile);
            this.smiley_button.MouseDown += new MouseEventHandler(this.smiley_button.on_click_down_smile);
            father_form.Controls.Add(this.smiley_button);

            for (int i = 0; i < field_width; i++)
            {
                for (int j = 0; j < field_height; j++)
                {
                    MyButton newButton = new MyButton(this, i, j);
                    newButton.Image = img_untouched;
                    newButton.BackColor = Color.Transparent;
                    newButton.Size = new Size(button_size, button_size);
                    newButton.MouseUp += new MouseEventHandler(newButton.on_click_up);
                    newButton.MouseDown += new MouseEventHandler(newButton.on_click_down);
                    newButton.Location = new Point(field_horizontal_offset + i * button_size,
                                                    strip_menu_offset + field_vertical_offset + j * button_size);
                    father_form.Controls.Add(newButton);

                    field_stats[i, j] = newButton;
                }
            }
            father_form.calc_size(field_width, field_height, field_horizontal_offset, field_vertical_offset, button_size);
            my_timer = new GameTimer(this.timer_pic);//.start();
        }

        public void delete_game()
        {
            foreach (MyButton btn_row in field_stats)
            {
                //Console.WriteLine(btn_row);
                father.Controls.Remove(btn_row);
            }
            for (int i = 0; i < 3; i++)
            {
                father.Controls.Remove(timer_pic[i]);
                father.Controls.Remove(move_count[i]);
            }
            father.Controls.Remove(this.smiley_button);
            my_timer.stop();// = false;
            Game.end = false;
            Game.start = false;
        }

        public void restart_game(object sender, EventArgs e)
        {
            this.Opened = 0;
            this.delete_game();
            this.start_game();
        }

        public void restart_game()
        {
            this.Opened = 0;
            this.delete_game();
            this.start_game();
        }


        public bool with_bomb(Point t)
        {
            //Console.WriteLine("x {0} y {1}", t.X, t.Y);
            foreach(Point p in this.with_bombs)
            {
                if (p == t)
                    return true;
            }
            return false;
        }

        public int bomb_count(MyButton btn)
        {
            int start_x = btn.pos.X;
            int start_y = btn.pos.Y;
            int count = 0;
            //if (btn.Enabled)
            //    this.Opened += 1;
            //btn.bomb_neighbours = -1;
            for (int i = -1 + start_x; i < 2 + start_x; i++)
            {
                for (int j = -1 + start_y; j < 2 + start_y; j++)
                {
                    Point t = new Point(i, j);
                    if (with_bomb(t))
                    {
                        count++;
                    }
                }
            }
            if (count == 0)
            {
                List<MyButton> b_list = new List<MyButton>();
                btn.bomb_neighbours = -1;
                b_list.Add(btn);
                for (int i = -1 + start_x; i < 2 + start_x; i++)
                {
                    for (int j = -1 + start_y; j < 2 + start_y; j++)
                    {
                        if (i >= 0 && i < this.field_width && j >= 0 && j < this.field_height)
                        {
                            MyButton neigh_btn = this.field_stats[i, j];
                            this.open_if_no_bomb(neigh_btn, b_list);
                        }
                    }
                }
            }
            else {
                if (btn.Enabled)
                {
                    btn.bomb_neighbours = count;
                    this.Opened += 1;
                }
            }
            return count;
        }

        public int open_if_no_bomb(MyButton btn, List<MyButton> except_button)
        {
            int start_x = btn.pos.X;
            int start_y = btn.pos.Y;
            int count = 0;
            if (btn.Enabled)
            {
                this.Opened += 1;
            }
            for (int i = -1 + start_x; i < 2 + start_x; i++)
            {
                for (int j = -1 + start_y; j < 2 + start_y; j++)
                {
                    Point t = new Point(i, j);
                    if (with_bomb(t))
                    {
                        count++;
                    }
                }
            }
            
            if (count == 0)
            {
                Game.flagged_btn.Remove(btn);
                btn.bomb_neighbours = -1;
                btn.Image = Game.numbers[count];
                btn.Enabled = false;
                for (int i = -1 + start_x; i < 2 + start_x; i++)
                {
                    for (int j = -1 + start_y; j < 2 + start_y; j++)
                    {
                        if (i >= 0 && i < this.field_width && j >= 0 && j < this.field_height)
                        {
                            MyButton neigh_btn = this.field_stats[i, j];
                            if (!except_button.Contains(neigh_btn))
                            {
                                
                                except_button.Add(neigh_btn);
                                this.open_if_no_bomb(neigh_btn, except_button);
                            }
                        }
                    }
                }
            }else
            {
                Game.flagged_btn.Remove(btn);
                btn.Image = Game.numbers[count];
                btn.bomb_neighbours = count;
                btn.Enabled = false;
            }
            
            if (!have_opened())
            {
                smiley_button.Image = Game.smiley[2];
                blow_up_win();
                Console.WriteLine("END ONLY BOMBS");
            }
            return count;
        }

        public void blow_up()
        {
            Game.end = true;
            foreach (Point p in this.with_bombs)
            {
                MyButton mb = this.field_stats[p.X, p.Y];
                if (!Game.flagged_btn.Contains(mb))
                    mb.explode();
            }
            foreach (MyButton btn in Game.flagged_btn)
            {
                if (!with_bomb(btn.pos))
                {
                    btn.false_bomb();//.Image = Game.img_no_bomb;
                }
            }
        }

        public void blow_up_win()
        {
            Game.end = true;
            foreach (Point p in this.with_bombs)
            {
                MyButton mb = this.field_stats[p.X, p.Y];
                mb.explode_win();
            }
        }

        public bool have_opened()
        {
            //int o = (this.field_width * this.field_height - this.Opened);
            //Console.WriteLine("HAVE OPENED {0} bombs count {1}", this.Opened, this.bombs_count);
            if (this.Opened + this.bombs_count == this.field_width * this.field_height)
                return false;
            return true;
        }

        public int Opened
        {
            get { return opened; }
            set { opened = value; }
        }

        public static List<Image> num_to_font(int number)
        {
            string s_n = number.ToString();
            List<Image> font_list = new List<Image>();
            foreach (char i in s_n)
            {
                //Console.WriteLine(i);
                font_list.Add(Game.my_font[i - '0']);
            }
            for(int i = font_list.Count; i < 3;i++)
                font_list.Insert(0, Game.my_font[0]);
            return font_list;
        }

        private void load_assets()
        {
            img_untouched = new Bitmap(WindowsFormsApplication3.Properties.Resources.untouched);//"F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\untouched.png");
            img_flagged = new Bitmap(WindowsFormsApplication3.Properties.Resources.flag);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\flag.png");
            img_bombed = new Bitmap(WindowsFormsApplication3.Properties.Resources.bomb); //Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\bomb.png");
            img_bomb_win = new Bitmap(WindowsFormsApplication3.Properties.Resources.bomb_win); //Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\bomb_win.png");
            img_no_bomb = new Bitmap(WindowsFormsApplication3.Properties.Resources.no_bomb);
            Game.numbers = new Image[9];
            Game.numbers[0] = new Bitmap(WindowsFormsApplication3.Properties.Resources.zero);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\zero.png");
            Game.numbers[1] = new Bitmap(WindowsFormsApplication3.Properties.Resources.one);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\one.png");
            Game.numbers[2] = new Bitmap(WindowsFormsApplication3.Properties.Resources.two);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\two.png");
            Game.numbers[3] = new Bitmap(WindowsFormsApplication3.Properties.Resources.three);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\three.png");
            Game.numbers[4] = new Bitmap(WindowsFormsApplication3.Properties.Resources.four);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\four.png");
            Game.numbers[5] = new Bitmap(WindowsFormsApplication3.Properties.Resources.five);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\five.png");
            Game.numbers[6] = new Bitmap(WindowsFormsApplication3.Properties.Resources.six); //Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\six.png");
            Game.numbers[7] = new Bitmap(WindowsFormsApplication3.Properties.Resources.seven);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\seven.png");
            Game.numbers[8] = new Bitmap(WindowsFormsApplication3.Properties.Resources.eight);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\eight.png");
            Game.smiley = new Image[4];
            Game.smiley[0] = new Bitmap(WindowsFormsApplication3.Properties.Resources.smiley);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\smiley.png");
            Game.smiley[1] = new Bitmap(WindowsFormsApplication3.Properties.Resources.smiley_ou);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\smiley_ou.png");
            Game.smiley[2] = new Bitmap(WindowsFormsApplication3.Properties.Resources.smiley_win);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\smiley_win.png");
            Game.smiley[3] = new Bitmap(WindowsFormsApplication3.Properties.Resources.smiley_defeat);//Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\smiley_defeat.png");
            Game.my_font = new Image[10];
            ResourceManager rm = new ResourceManager("Root", typeof(Image).Assembly);//string.ResourceManager;
            for (int i=0;i<10;i++)
            {
                string load_path = String.Format("_{0}", i);
                object o = Resources.ResourceManager.GetObject(load_path);
                Game.my_font[i] = (Image)o;
            }
        }
    }
}
