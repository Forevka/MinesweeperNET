using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public class GameTimer:Form1
    {
        static int clock = 0;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private PictureBox[] pic;

        public GameTimer(PictureBox[] pic)
        {
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 1000; // one second
            myTimer.Start();
            this.pic = pic;
        }

        public int get_time()
        {
            return clock;
        }

        private void TimerEventProcessor(Object myObject,
                                            EventArgs myEventArgs)
        {
            clock += 1;
            draw_time();
        }

        public void draw_time()
        {
            List<Image> num_list = Game.num_to_font(get_time());
            for (int i = 0; i < num_list.Count; i++)
            {
                Image num = num_list[i];
                Console.WriteLine(num);
                pic[i].Image = num;
            }
        }
    }
}
