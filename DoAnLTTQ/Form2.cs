using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            // design form
            // panel1
            Panel panel1 = new Panel();
            panel1.Size = new Size(100, 100);

            

            Button button1 = new Button();
            button1.Text = "ass";
            button1.Location = new Point(100, 100);

        



            InitializeComponent();
            //to add the scroll bar to panel
            panel1.AutoScroll = false;
            panel1.HorizontalScroll.Enabled = false;
            panel1.HorizontalScroll.Visible = false;
            panel1.HorizontalScroll.Maximum = 0;
            panel1.AutoScroll = true;


        }
    }
}
