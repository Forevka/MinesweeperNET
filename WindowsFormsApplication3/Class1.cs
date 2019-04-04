﻿using System;
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
        public bool hovered = false;
        public MyButton(Game f, int xx, int yy) : base()
        {
            this.pos = new Point(xx, yy);
            this.father = f;
        }
        //this.father.smiley_button.Image = Game.smiley[1];
        public void on_click_up(Object sender, MouseEventArgs e)
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
                        if (this.father.with_bomb(this.pos))
                        {
                            this.father.blow_up();
                            this.father.smiley_button.Image = Game.smiley[3];
                            Console.WriteLine("end");
                        }
                        else
                        {
                            int count = this.father.bomb_count(this);
                            //this.father.Opened += 1;
                            Console.WriteLine("count {0}", count);
                            this.Image = Game.numbers[count];
                            this.Enabled = false;
                            this.father.smiley_button.Image = Game.smiley[0];
                            if (!this.father.have_opened())
                            {
                                this.father.smiley_button.Image = Game.smiley[2];
                                Console.WriteLine("END ONLY BOMBS");
                                this.father.blow_up_win();
                            }
                        }
                        break;
                    case MouseButtons.Right:
                        Console.WriteLine("RIGHT coords {0}, {1}", this.pos.X, this.pos.Y);
                        this.Image = this.Image == Game.img_flagged ? Game.img_untouched : Game.img_flagged;
                        break;
                }
            }
            //this.father.smiley_button.Image = Game.smiley[3];
            this.father.smiley_button.Focus();
        }

        public void on_click_down(Object sender, MouseEventArgs e)
        {
            if (!Game.end)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Console.WriteLine(me);
                switch (me.Button)
                {
                    case MouseButtons.Left:
                        this.father.smiley_button.Image = Game.smiley[1];
                        break;
                }
                Console.WriteLine("click down");
            }
        }
        //img_bomb_win
        public void explode()
        {
            this.Image = Game.img_bombed;
        }

        public void explode_win()
        {
            this.Image = Game.img_bomb_win;
            Console.WriteLine("bomb win");
        }
    }
}