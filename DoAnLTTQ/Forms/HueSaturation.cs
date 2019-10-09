using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ.Forms
{
    public partial class HueSaturation : Form
    {
        private Bitmap image;
        public HueSaturation()
        {
            InitializeComponent();
        }

        public Bitmap Image
        {
            set
            {
                image = value;
                pictureBox1.Image = image;
            }
            get
            {
                return image;
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString();
        }

    }
}
