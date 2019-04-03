using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public class MyButton : Button
    {
        public int state;
        public Point pos;
        public Game father;
        public Point[] with_bombs;
        public MyButton(Game f, int xx, int yy) : base()
        {
            pos = new Point(xx, yy);
            this.father = f;
        }

        public void on_click(Object sender, MouseEventArgs e)
        {
            if (!Game.end)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Console.WriteLine(me);
                switch (me.Button)
                {
                    case MouseButtons.Left:
                        Console.WriteLine("LEFT coords {0}, {1}", this.pos.X, this.pos.Y);
                        if (this.Image == Game.img_flagged)
                            break;
                        if (this.father.check_exist_point(this.pos))
                        {
                            this.father.blow_up();
                            Console.WriteLine("end");
                        }
                        else
                        {
                            int count = this.father.bomb_neighbours(this);
                            Console.WriteLine("count {0}", count);
                            this.Image = Game.numbers[count];
                            this.Enabled = false;
                        }
                        break;
                    case MouseButtons.Right:
                        Console.WriteLine("RIGHT coords {0}, {1}", this.pos.X, this.pos.Y);
                        this.Image = this.Image == Game.img_flagged ? Game.img_untouched : Game.img_flagged;
                        break;
                }
            }
        }

        public void explode()
        {
            this.Image = Game.img_bomb;
        }
    }
}
