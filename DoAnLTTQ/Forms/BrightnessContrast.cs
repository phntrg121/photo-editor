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
                image = value;
                pictureBox1.Image = image;
            }
            get
            {
                return new Bitmap(pictureBox1.Image);
            }
        }

        private float brightness = 1f;
        private float contrast = 1f;
        private void Adjust()
        {
            float[][] colorMatrix =
            {
                new float[]{contrast,0,0,0,0},
                new float[]{0,contrast,0,0,0},
                new float[]{0,0,contrast,0,0},
                new float[]{0,0,0,1f,0},
                new float[]{brightness,brightness,brightness,0,1f}
            };
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            
            adjusted = new Bitmap(image);
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                imageAttributes.SetColorMatrix(new ColorMatrix(colorMatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
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
            brightness = (float)brightnessTrack.Value/100;
            Adjust();
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = contrastTrack.Value.ToString();
            contrast = (float)contrastTrack.Value/100 + 1f;
            Adjust();
        }

    }
}
