using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public class MoveCounter:Form1
    {
        private int move_count = 0;
        private PictureBox[] pic;

        public MoveCounter(PictureBox[] pic)
        {
            this.pic = pic;
        }

        public int MoveCount
        {
            get { return move_count; }
            set { move_count = value; draw_count(); }
        }

        public void draw_count()
        {
            List<Image> num_list = Game.num_to_font(MoveCount);
            for (int i = 0; i < num_list.Count; i++)
            {
                Image num = num_list[i];
                Console.WriteLine(num);
                pic[i].Image = num;
            }
        }

    }
}
