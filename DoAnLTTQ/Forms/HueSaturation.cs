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
        private System.Drawing.Imaging.BitmapData bmpData;
        private byte[] imagePixels;
        private IntPtr ptr;
        private int dataSize;

        public HueSaturation()
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

        public void Initialize()
        {
            bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, image.PixelFormat);
            ptr = bmpData.Scan0;
            dataSize = Math.Abs(bmpData.Stride) * image.Height;
            imagePixels = new byte[dataSize];
            System.Runtime.InteropServices.Marshal.Copy(ptr, imagePixels, 0, dataSize);
            image.UnlockBits(bmpData);
        }

        float H, S, V;
        float hueVal, saturationVal, brightnessVal;
        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            adjusted = new Bitmap(image);
            bmpData = adjusted.LockBits(new Rectangle(0, 0, adjusted.Width, adjusted.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, adjusted.PixelFormat);
            ptr = bmpData.Scan0;
            byte[] pixels = new byte[dataSize];
            for (int i = 0; i < dataSize; i += 4)
            {
                Color c = Color.FromArgb(imagePixels[i + 3], imagePixels[i + 0], imagePixels[i + 1], imagePixels[i + 2]);
                SetColor(ref c);
                pixels[i + 3] = imagePixels[i + 3];
                pixels[i + 0] = c.R;
                pixels[i + 1] = c.G;
                pixels[i + 2] = c.B;
            }
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, dataSize);
            adjusted.UnlockBits(bmpData);
            pictureBox1.Image = adjusted;
        }

        private void SetColor(ref Color c)
        {
            RGBtoHSV(ref H, ref S, ref V, c);
            H += hueVal;
            if (H < 0) H += 360;
            if (H >= 360) H -= 360;
            S *= 1 + saturationVal / 100;
            V *= 1 + brightnessVal / 100;
            HSVtoRBG(H, S, V, ref c);
        }


        private void RGBtoHSV(ref float H, ref float S, ref float V, Color c)
        {
            float r, g, b;
            r = (float)c.R / 255;
            g = (float)c.G / 255;
            b = (float)c.B / 255;
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            if (max == min) H = 0;
            else if (max == r) H = 60 * ((g - b) / (max - min));
            else if (max == g) H = 60 * (2 + (b - r) / (max - min));
            else if (max == b) H = 60 * (4 + (r - g) / (max - min));
            if (H < 0) H += 360;
            if (H >= 360) H -= 360;
            if (max == 0 || min == 1)
                S = 0;
            else S = (max - min) / max;
            V = max;
        }

        private void HSVtoRBG(float H, float S, float V, ref Color c)
        {
            float r, g, b;
            r = g = b = 0;
            float C = V * S;
            float X = C * (1 - Math.Abs((H / 60) % 2 - 1));
            float m = V - C;
            if (0 <= H && H < 60)
            {
                r = C;
                g = X;
                b = 0;
            }
            else if (60 <= H && H < 120)
            {
                r = X;
                g = C;
                b = 0;
            }
            else if (120 <= H && H < 180)
            {
                r = 0;
                g = C;
                b = X;
            }
            else if (180 <= H && H < 240)
            {
                r = 0;
                g = X;
                b = C;
            }
            else if (240 <= H && H < 300)
            {
                r = X;
                g = 0;
                b = C;
            }
            else if (300 <= H && H < 360)
            {
                r = C;
                g = 0;
                b = X;
            }
            int R = (int)((r + m) * 255);
            if (R < 0) R = 0;
            if (R > 255) R = 255;
            int G = (int)((g + m) * 255);
            if (G < 0) G = 0;
            if (G > 255) G = 255;
            int B = (int)((b + m) * 255);
            if (B < 0) B = 0;
            if (B > 255) B = 255;
            c = Color.FromArgb(R, G, B);
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = hueTrack.Value.ToString();
            hueVal = hueTrack.Value;
            Adjust();
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = saturationTrack.Value.ToString();
            saturationVal = saturationTrack.Value;
            Adjust();
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            label6.Text = luminosityTrack.Value.ToString();
            brightnessVal = luminosityTrack.Value;
            Adjust();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            hueTrack.Value = 0;
            label3.Text = hueTrack.Value.ToString();
            saturationTrack.Value = 0;
            label4.Text = saturationTrack.Value.ToString();
            luminosityTrack.Value = 0;
            label6.Text = luminosityTrack.Value.ToString();
            pictureBox1.Image = image;
        }
    }
}
