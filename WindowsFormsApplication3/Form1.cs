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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void calc_size(int f_w, int f_h, int f_h_off, int f_v_off, int b_size)
        {
            this.Size = new Size(f_h_off + (b_size+2) * f_w, f_v_off + (b_size+4) * f_h);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Game g = new Game(this, 10, 10, 20);
        }
    }

    public class Game
    {
        private int time_clock;
        public int field_width;
        public int field_height;
        public static bool end = false;
        public Point[] with_bombs;
        public MyButton[,] field_stats;
        public static Image img_untouched;
        public static Image img_flagged;
        public static Image img_bomb;
        public static Image[] numbers;

        public Game(Form1 father, int width, int height, int bomb_count)
        {
            field_width = width;
            field_height = height;
            int field_horizontal_offset = 2;
            int field_vertical_offset = 50;
            int button_size = 32;

            with_bombs = new Point[bomb_count];
            field_stats = new MyButton[field_width, field_height];
            load_assets();
            Random rand = new Random();
            Form1 father_form = father;

            father_form.calc_size(field_width, field_height, field_horizontal_offset, field_vertical_offset, button_size);

            for (int i = 0; i < bomb_count; i++)
            {
                Point t = new Point(rand.Next(field_width), rand.Next(field_height));
                while (check_exist_point(t))
                {
                    t = new Point(rand.Next(field_width), rand.Next(field_height));
                }
                this.with_bombs[i] = t;
            }

            for (int i = 0; i < field_width; i++)
            {
                for (int j = 0; j < field_height; j++)
                {
                    MyButton newButton = new MyButton(this, i, j);
                    newButton.Image = img_untouched;
                    newButton.FlatStyle = FlatStyle.Flat;
                    newButton.BackColor = Color.Transparent;
                    newButton.FlatAppearance.MouseDownBackColor = Color.Transparent;

                    newButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    newButton.Size = new Size(button_size, button_size);
                    newButton.MouseUp += new MouseEventHandler(newButton.on_click);
                    newButton.Location = new Point(field_horizontal_offset + i * button_size,
                                                    field_vertical_offset + j * button_size);
                    father_form.Controls.Add(newButton);

                    field_stats[i, j] = newButton;
                }
            }
            foreach (Point p in this.with_bombs)
            {
                Console.WriteLine("x {0} y {1}", p.X, p.Y);
            }

        }

        public bool check_exist_point(Point t)
        {
            Console.WriteLine("x {0} y {1}", t.X, t.Y);
            foreach(Point p in this.with_bombs)
            {
                if (p == t)
                    return true;
            }
            return false;
        }

        public int bomb_neighbours(MyButton btn)
        {
            int start_x = btn.pos.X;
            int start_y = btn.pos.Y;
            int count = 0;

            for (int i = -1 + start_x; i < 2 + start_x; i++)
            {
                for (int j = -1 + start_y; j < 2 + start_y; j++)
                {
                    Point t = new Point(i, j);
                    if (check_exist_point(t))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void blow_up()
        {
            //Game.end = true;
            foreach (Point p in this.with_bombs)
            {
                MyButton mb = this.field_stats[p.X, p.Y];
                mb.explode();
            }
        }

        private void load_assets()
        {
            img_untouched = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\untouched.png");
            img_flagged = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\flag.png");
            img_bomb = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\bomb.png");
            Game.numbers = new Image[9];
            Game.numbers[0] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\zero.png");
            Game.numbers[1] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\one.png");
            Game.numbers[2] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\two.png");
            Game.numbers[3] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\three.png");
            Game.numbers[4] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\four.png");
            Game.numbers[5] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\five.png");
            Game.numbers[6] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\six.png");
            Game.numbers[7] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\seven.png");
            Game.numbers[8] = Image.FromFile("F:\\csharp\\WindowsFormsApplication3\\WindowsFormsApplication3\\assets\\eight.png");
        }
    }
}
