using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace DoAnLTTQ.Forms
{
    public partial class BrightnessContrast : Form
    {
        private Bitmap image;
        private Bitmap adjusted;
        public BrightnessContrast()
        {
            InitializeComponent();
        }

        public Bitmap Image
        {
            set
            {
                image = new Bitmap(value);
                pictureBox1.Image = image;
                adjusted = new Bitmap(image);
            }
            get
            {
                return new Bitmap(pictureBox1.Image);
            }
        }

        private float brightness = 0f;
        private float contrast = 1f;
        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            
            adjusted = new Bitmap(image);
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = contrast;
                matrix.Matrix33 = matrix.Matrix44 = 1f;
                matrix.Matrix40 = matrix.Matrix41 = matrix.Matrix42 = brightness;

                imageAttributes.SetColorMatrix(matrix);
                using (Graphics g = Graphics.FromImage(adjusted))
                {
                    g.DrawImage(adjusted, new Rectangle(0, 0, adjusted.Width, adjusted.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                }
                pictureBox1.Image = adjusted;
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = brightnessTrack.Value.ToString();
            brightness = (float)brightnessTrack.Value / 100;
            Adjust();
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = contrastTrack.Value.ToString();
            contrast = (float)contrastTrack.Value / 100 + 1f;
            Adjust();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            brightnessTrack.Value = 0;
            label3.Text = brightnessTrack.Value.ToString();
            contrastTrack.Value = 0;
            label4.Text = contrastTrack.Value.ToString();
            pictureBox1.Image = image;
        }
    }
}
