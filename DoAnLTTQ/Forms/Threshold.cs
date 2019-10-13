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
    public partial class Threshold : Form
    {
        private Bitmap image;
        private Bitmap adjusted;
        private System.Drawing.Imaging.BitmapData bmpData;
        private byte[] imagePixels;
        private IntPtr ptr;
        private int dataSize;

        public Threshold()
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
            Adjust();
        }

        int level = 128;
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
                int n = (int)((float)imagePixels[i] * 0.4f + (float)imagePixels[i + 1] * 0.4f + (float)imagePixels[i + 2] * 0.2f);

                if (n < level)
                {
                    pixels[i] = 0;
                    pixels[i + 1] = 0;
                    pixels[i + 2] = 0;
                }
                else
                {
                    pixels[i] = 255;
                    pixels[i + 1] = 255;
                    pixels[i + 2] = 255;
                }
                pixels[i + 3] = imagePixels[i + 3];

            }
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, dataSize);
            adjusted.UnlockBits(bmpData);
            pictureBox1.Image = adjusted;
        }

        private void LevelTrack_Scroll(object sender, EventArgs e)
        {
            label3.Text = levelTrack.Value.ToString();
            level = levelTrack.Value;
            Adjust();
        }
    }
}
