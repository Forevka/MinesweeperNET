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
    public partial class CustomFieldForm : Form
    {
        private MainForm father;
        public CustomFieldForm(MainForm father)
        {
            InitializeComponent();
            WidthNumeric.Value = father.g.field_width;
            HeightNumeric.Value = father.g.field_height;
            MinesNumeric.Value = father.g.bombs_count;
            this.father = father;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            father.start_custom((int)WidthNumeric.Value, (int)HeightNumeric.Value, (int)MinesNumeric.Value);
            this.Close();
        }
    }
}
