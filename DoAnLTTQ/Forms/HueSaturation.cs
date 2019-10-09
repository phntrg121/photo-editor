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
        private Bitmap adjusted;
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
                return new Bitmap(pictureBox1.Image);
            }
        }

        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            adjusted = new Bitmap(image);
            pictureBox1.Image = adjusted;
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
            Adjust();
        }

        private void RGBtoHSL(float H, float S, float L, Color c)
        {
            float r, g, b;
            r = c.R / 255;
            g = c.G / 255;
            b = c.B / 255;
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            if (max == min) H = 0;
            else if (max == r) H = 60 * ((g - b) / (max - min));
            else if (max == g) H = 60 * (2 + (b - r) / (max - min));
            else if (max == b) H = 60 * (4 + (r - g) / (max - min));
            if (H < 0) H += 360f;
            L = (max + min) / 2;
            if (max == 0 || min == 1)
                S = 0;
            else S = (max - min) / (1 - Math.Abs(max + min - 1));
        }

        private void HSLtoRBG(float H, float S, float L , Color c)
        {
            float r, g, b;
            float C = (1 - Math.Abs(2 * L - 1)) * S;
            float h = H / 60;
            float X = C * (1 - Math.Abs(h % 2 - 1));
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString();
            Adjust();
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            label6.Text = trackBar3.Value.ToString();
            Adjust();
        }
    }
}
