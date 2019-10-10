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

        float H, S, L;
        float hueVal, saturationVal, lightnessVal;
        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            adjusted = new Bitmap(image);
            bmpData = adjusted.LockBits(new Rectangle(0, 0, adjusted.Width, adjusted.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, adjusted.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            int size = Math.Abs(bmpData.Stride) * adjusted.Height;
            byte[] pixels1 = new byte[size];
            byte[] pixels2 = new byte[size];

            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels1, 0, size);

            for (int i = 0; i < pixels1.Length; i += 4)
            {
                Color c = Color.FromArgb(pixels1[i + 3], pixels1[i + 0], pixels1[i + 1], pixels1[i + 2]);
                SetColor(ref c);
                pixels2[i + 3] = c.A;
                pixels2[i + 0] = c.R;
                pixels2[i + 1] = c.G;
                pixels2[i + 2] = c.B;
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels2, 0, ptr, size);

            adjusted.UnlockBits(bmpData);

            pictureBox1.Image = adjusted;
        }

        private void SetColor(ref Color c)
        {
            RGBtoHSL(ref H, ref S, ref L, c);
            H += hueVal;
            if (H < 0) H += 360;
            if (H >= 360) H -= 360;
            S *= 1 + saturationVal / 100;
            L *= 1 + lightnessVal / 100;
            HSLtoRBG(H, S, L, ref c);
        }


        private void RGBtoHSL(ref float H, ref float S, ref float L, Color c)
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
            L = (max + min) / 2;
            if (max == 0 || min == 1)
                S = 0;
            else S = (max - min) / (1 - Math.Abs(max + min - 1));
        }

        private void HSLtoRBG(float H, float S, float L, ref Color c)
        {
            float r, g, b;
            r = g = b = 0;
            float C = (1 - Math.Abs(2 * L - 1)) * S;
            float X = C * (1 - Math.Abs((H / 60) % 2 - 1));
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
            float m = L - C / 2;
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
            label6.Text = lightnessTrack.Value.ToString();
            lightnessVal = lightnessTrack.Value;
            Adjust();
        }
    }
}
