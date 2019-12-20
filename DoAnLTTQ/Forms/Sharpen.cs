using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ.Forms
{
    public partial class Sharpen : Form
    {
        private Form1 f;
        private LayerContainer lc;
        private Bitmap origin;
        private Bitmap adjusted;

        public Sharpen(Form1 f, LayerContainer lc)
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

        float sharpenforce;
        private void Adjust()
        {
            if (adjusted != null)
            {
                adjusted.Dispose();
                adjusted = null;
            }
            adjusted = new Bitmap(origin);

            adjusted = UnSharpMask(adjusted);

            lc.ProcessUpdate(adjusted, true);
            f.DSUpdate();
        }

        Bitmap UnSharpMask(Bitmap bmp)
        {
            float[,] kenel = new float[,]
            {
                {0f,-1f * sharpenforce,0f},
                {-1f * sharpenforce,-4f * sharpenforce +1,-1f * sharpenforce},
                {0f,-1f * sharpenforce,0f}
            };

            int W = bmp.Width;
            int H = bmp.Height;

            byte r, g, b;
            byte nr, ng, nb;

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int size = W * H * 4;
            byte[] pixel = new byte[size];
            byte[] new_pixel = new byte[size];

            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixel, 0, size);

            for (int y = 1; y < H - 1; y++)
            {
                for (int x = 1; x < W - 1; x++)
                {
                    nr = ng = nb = 0;

                    for (int yk = -1; yk < 2; yk++)
                    {
                        for (int xk = -1; xk < 2; xk++)
                        {
                            r = pixel[(y + yk) * W * 4 + (x + xk) * 4 + 0];
                            g = pixel[(y + yk) * W * 4 + (x + xk) * 4 + 1];
                            b = pixel[(y + yk) * W * 4 + (x + xk) * 4 + 2];

                            nr += (byte)(kenel[yk + 1, xk + 1] * r);
                            ng += (byte)(kenel[yk + 1, xk + 1] * g);
                            nb += (byte)(kenel[yk + 1, xk + 1] * b);
                        }
                    }
                    new_pixel[y * W * 4 + x * 4 + 0] = nr;
                    new_pixel[y * W * 4 + x * 4 + 1] = ng;
                    new_pixel[y * W * 4 + x * 4 + 2] = nb;
                    new_pixel[y * W * 4 + x * 4 + 3] = pixel[y * W * 4 + x * 4 + 3];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(new_pixel, 0, data.Scan0, size);
            bmp.UnlockBits(data);
            return bmp;
        }

        private void SharpenTrack_Scroll(object sender, EventArgs e)
        {
            sharpenforce = 1f + (float)pixelTrack.Value / 100;
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
