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
    public partial class Noise : Form
    {
        private Form1 f;
        private LayerContainer lc;
        private Bitmap origin;
        private Bitmap adjusted;
        private System.Drawing.Imaging.BitmapData bmpData;
        private byte[] imagePixels;
        private int dataSize;

        public Noise(Form1 f, LayerContainer lc)
        {
            InitializeComponent();
            this.f = f;
            this.lc = lc;
        }
        public Bitmap Image
        {
            set
            {
                origin = new Bitmap(value);
                adjusted = new Bitmap(origin);
            }
            get
            {
                return adjusted;
            }
        }

        public void Initialize()
        {
            bmpData = origin.LockBits(new Rectangle(0, 0, origin.Width, origin.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, origin.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            dataSize = Math.Abs(bmpData.Stride) * origin.Height;
            imagePixels = new byte[dataSize];
            System.Runtime.InteropServices.Marshal.Copy(ptr, imagePixels, 0, dataSize);
            origin.UnlockBits(bmpData);
        }

        int amount = 0;
        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            adjusted = new Bitmap(origin);
            bmpData = adjusted.LockBits(new Rectangle(0, 0, adjusted.Width, adjusted.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, adjusted.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[dataSize];

            Random rand = new Random();

            for (int i = 0; i < dataSize; i += 4)
            {
                int R = imagePixels[i + 0] + rand.Next(-amount, amount + 1);
                int G = imagePixels[i + 1] + rand.Next(-amount, amount + 1);
                int B = imagePixels[i + 2] + rand.Next(-amount, amount + 1);

                if (R > 255) R = 255;
                if (G > 255) G = 255;
                if (B > 255) B = 255;

                if (R < 0) R = 0;
                if (G < 0) G = 0;
                if (B < 0) B = 0;

                pixels[i + 0] = (byte)R;
                pixels[i + 1] = (byte)G;
                pixels[i + 2] = (byte)B;
                pixels[i + 3] = imagePixels[i + 3];
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, dataSize);
            adjusted.UnlockBits(bmpData);
            lc.ProcessUpdate(adjusted, true);
            f.DSUpdate();
        }

        private void NoiseTrack_Scroll(object sender, EventArgs e)
        {
            amount = noiseTrack.Value;
            label3.Text = noiseTrack.Value.ToString();
            Adjust();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            lc.ProcessUpdate(origin, true);
            f.DSUpdate();
        }
    }
}
