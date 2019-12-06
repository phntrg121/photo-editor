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
    public partial class GaussianBlur : Form
    {
        private Form1 f;
        private LayerContainer lc;
        private Bitmap origin;
        private Bitmap adjusted;
        private System.Drawing.Imaging.BitmapData bmpData;
        private byte[] imagePixels;
        private int[] alpha;
        private int[] red;
        private int[] green;
        private int[] blue;
        private int dataSize;
        private int width;
        private int height;

        public GaussianBlur(Form1 f, LayerContainer lc)
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
            width = origin.Width;
            height = origin.Height;
        }

        int radial = 0;
        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }

            if (radial == 0)
                adjusted = new Bitmap(origin);
            else
            {
                alpha = red = blue = green = null;

                alpha = new int[width * height];
                red = new int[width * height];
                green = new int[width * height];
                blue = new int[width * height];

                for (int i = 0, j = 0; i < dataSize; i += 4, j++)
                {
                    red[j] = imagePixels[i + 0];
                    green[j] = imagePixels[i + 1];
                    blue[j] = imagePixels[i + 2];
                    alpha[j] = imagePixels[i + 3];
                }

                adjusted = Process(radial);
            }

            lc.ProcessUpdate(adjusted, true);
            f.DSUpdate();
        }

        Bitmap Process(int r)
        {
            int[] newalpha = new int[width * height];
            int[] newred = new int[width * height];
            int[] newgreen = new int[width * height];
            int[] newblue = new int[width * height];
            byte[] pixels = new byte[dataSize];

            gaussBlur(alpha, newalpha, r);
            gaussBlur(red, newred, r);
            gaussBlur(green, newgreen, r);
            gaussBlur(blue, newblue, r);

            for (int i = 0, j = 0; i < dataSize/4; i ++, j+=4)
            {
                if (newalpha[i] > 255) newalpha[i] = 255;
                if (newred[i] > 255) newred[i] = 255;
                if (newgreen[i] > 255) newgreen[i] = 255;
                if (newblue[i] > 255) newblue[i] = 255;

                if (newalpha[i] < 0) newalpha[i] = 0;
                if (newred[i] < 0) newred[i] = 0;
                if (newgreen[i] < 0) newgreen[i] = 0;
                if (newblue[i] < 0) newblue[i] = 0;

                pixels[j + 0] = (byte)newred[i];
                pixels[j + 1] = (byte)newgreen[i];
                pixels[j + 2] = (byte)newblue[i];
                pixels[j + 3] = (byte)newalpha[i];
            }

            Bitmap img = new Bitmap(width, height);
            var datas = img.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, origin.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, datas.Scan0, dataSize);
            img.UnlockBits(datas);
            return img;
        }
        private void gaussBlur(int[] source, int[] dest, int r)
        {
            var bxs = boxesForGauss(r, 3);
            boxBlur(source, dest, width, height, (bxs[0] - 1) / 2);
            boxBlur(dest, source, width, height, (bxs[1] - 1) / 2);
            boxBlur(source, dest, width, height, (bxs[2] - 1) / 2);
        }

        private int[] boxesForGauss(int sigma, int n)
        {
            var wIdeal = Math.Sqrt((12 * sigma * sigma / n) + 1);
            var wl = (int)Math.Floor(wIdeal);
            if (wl % 2 == 0) wl--;
            var wu = (int)(wl + 2);

            var mIdeal = (double)(12 * sigma * sigma - n * wl * wl - 4 * n * wl - 3 * n) / (-4 * wl - 4);
            var m = Math.Round(mIdeal);

            var sizes = new List<int>();
            for (var i = 0; i < n; i++) sizes.Add(i < m ? wl : wu);
            return sizes.ToArray();
        }

        private void boxBlur(int[] source, int[] dest, int w, int h, int r)
        {
            for (var i = 0; i < source.Length; i++) dest[i] = source[i];
            boxBlurH(dest, source, w, h, r);
            boxBlurT(source, dest, w, h, r);
        }

        private void boxBlurH(int[] source, int[] dest, int w, int h, int r)
        {
            var iar = (double)1 / (r + r + 1);
            for (var i = 0; i < h; i++)
            {
                var ti = i * w;
                var li = ti;
                var ri = ti + r;
                var fv = source[ti];
                var lv = source[ti + w - 1];
                var val = (r + 1) * fv;
                for (var j = 0; j < r; j++) val += source[ti + j];
                for (var j = 0; j <= r; j++)
                {
                    val += source[ri++] - fv;
                    dest[ti++] = (int)Math.Round(val * iar);
                }
                for (var j = r + 1; j < w - r; j++)
                {
                    val += source[ri++] - dest[li++];
                    dest[ti++] = (int)Math.Round(val * iar);
                }
                for (var j = w - r; j < w; j++)
                {
                    val += lv - source[li++];
                    dest[ti++] = (int)Math.Round(val * iar);
                }
            }
        }

        private void boxBlurT(int[] source, int[] dest, int w, int h, int r)
        {
            var iar = (double)1 / (r + r + 1);
            for(var i =0;i <w; i++)
            { 
                var ti = i;
                var li = ti;
                var ri = ti + r * w;
                var fv = source[ti];
                var lv = source[ti + w * (h - 1)];
                var val = (r + 1) * fv;
                for (var j = 0; j < r; j++) val += source[ti + j * w];
                for (var j = 0; j <= r; j++)
                {
                    val += source[ri] - fv;
                    dest[ti] = (int)Math.Round(val * iar);
                    ri += w;
                    ti += w;
                }
                for (var j = r + 1; j < h - r; j++)
                {
                    val += source[ri] - source[li];
                    dest[ti] = (int)Math.Round(val * iar);
                    li += w;
                    ri += w;
                    ti += w;
                }
                for (var j = h - r; j < h; j++)
                {
                    val += lv - source[li];
                    dest[ti] = (int)Math.Round(val * iar);
                    li += w;
                    ti += w;
                }
            }
        }

        private void PixelTrack_Scroll(object sender, EventArgs e)
        {
            radial = pixelTrack.Value;
            label3.Text = pixelTrack.Value.ToString();
            Adjust();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            lc.ProcessUpdate(origin, true);
            f.DSUpdate();
        }
    }
}
