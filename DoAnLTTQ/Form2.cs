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
using System.IO;
using System.Runtime.InteropServices;

namespace DoAnLTTQ
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        Image file;
        Boolean opened = false; // Kiểm tra ảnh đã được mở hay chưa
     
        // Open Image 
        void openImage()
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {

                file = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = file;
                opened = true;

            }
            Bitmap bmp = new Bitmap((Bitmap)this.pictureBox1.Image);
            
            while(w > pictureBox1.Width || h < pictureBox1.Height)
            {
                
            }

        }
        // Save Image
        void saveImage()
        {
            if (opened)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Images|*.png;*bmp;*.jpg";
                ImageFormat format = ImageFormat.Png;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string text = Path.GetExtension(sfd.FileName);
                    switch (text)
                    {
                        case ".jng":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }
                    pictureBox1.Image.Save(sfd.FileName, format);
                }

            }
            else
            {
                MessageBox.Show("No image loaded");
            }
        }
        // Reload Image
        void Reload()
        {
            if (!opened)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = file;
                opened = true;
            }
            else
            {
                if (opened)
                {
                    file = Image.FromFile(openFileDialog1.FileName);
                    pictureBox1.Image = file;
                    opened = true;
                }
            }
        }
      
        public void SetGrayscale()
        {
            Bitmap bmp = new Bitmap((Bitmap)this.pictureBox1.Image);
            //Lock bitmap's bits to system memory
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //Scan for the first line
            IntPtr ptr = bmpData.Scan0;

            //Declare an array in which your RGB values will be stored
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            //Copy RGB values in that array
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                //Set RGB values in a Array where all RGB values are stored
                byte gray = (byte)(rgbValues[i] * .299 + rgbValues[i + 1] * .587  + rgbValues[i + 2] * .114 );
                rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = gray;
            }

            //Copy changed RGB values back to bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            //Unlock the bits
            bmp.UnlockBits(bmpData);
            this.pictureBox1.Image = bmp;
        }        
        public void Gray()
        {
            Bitmap bmp = new Bitmap((Bitmap)this.pictureBox1.Image);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color c = bmp.GetPixel(i, j);

                    //Apply conversion equation
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    //Set the color of this pixel
                    bmp.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            pictureBox1.Image = bmp;
        }
        public void Invert()
        {
            Bitmap bmp = new Bitmap((Bitmap)this.pictureBox1.Image);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                  //Set the color of this pixel
                    bmp.SetPixel(i, j, Color.FromArgb(255-c.R, 255-c.G, 255-c.B));
                }
            }
            pictureBox1.Image = bmp;
        }

        private byte[] CreateGammaArray(double color)
        {
            byte[] gammaArray = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                gammaArray[i] = (byte)Math.Min(255,
        (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
            }
            return gammaArray;
        }

        public void SetGamma(double red, double green, double blue)
        {
            Bitmap bmp = new Bitmap((Bitmap)this.pictureBox1.Image);
            Color c;
            byte[] redGamma = CreateGammaArray(red);
            byte[] greenGamma = CreateGammaArray(green);
            byte[] blueGamma = CreateGammaArray(blue);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    bmp.SetPixel(i, j, Color.FromArgb(redGamma[c.R],
                      greenGamma[c.G], blueGamma[c.B]));
                }
            }
            pictureBox1.Image = bmp;
        }
        public void RotateFlip(RotateFlipType rotateFlipType)
        {
            Bitmap temp = (Bitmap)pictureBox1.Image;
            Bitmap bmap = (Bitmap)temp.Clone();
            bmap.RotateFlip(rotateFlipType);
            pictureBox1.Image = (Bitmap)bmap.Clone();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            openImage();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            saveImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Reload();
            //SetGrayscale();
            Gray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Invert();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetGamma(3,3,3);
        }
        public void Resize(int newWidth, int newHeight)
        {
            if (newWidth != 0 && newHeight != 0)
            {
                Bitmap temp = (Bitmap)pictureBox1.Image;
                Bitmap bmap = new Bitmap(newWidth, newHeight, temp.PixelFormat);

                double nWidthFactor = (double)temp.Width / (double)newWidth;
                double nHeightFactor = (double)temp.Height / (double)newHeight;

                double fx, fy, nx, ny;
                int cx, cy, fr_x, fr_y;
                Color color1 = new Color();
                Color color2 = new Color();
                Color color3 = new Color();
                Color color4 = new Color();
                byte nRed, nGreen, nBlue;

                byte bp1, bp2;

                for (int x = 0; x < bmap.Width; ++x)
                {
                    for (int y = 0; y < bmap.Height; ++y)
                    {

                        fr_x = (int)Math.Floor(x * nWidthFactor);
                        fr_y = (int)Math.Floor(y * nHeightFactor);
                        cx = fr_x + 1;
                        if (cx >= temp.Width) cx = fr_x;
                        cy = fr_y + 1;
                        if (cy >= temp.Height) cy = fr_y;
                        fx = x * nWidthFactor - fr_x;
                        fy = y * nHeightFactor - fr_y;
                        nx = 1.0 - fx;
                        ny = 1.0 - fy;

                        color1 = temp.GetPixel(fr_x, fr_y);
                        color2 = temp.GetPixel(cx, fr_y);
                        color3 = temp.GetPixel(fr_x, cy);
                        color4 = temp.GetPixel(cx, cy);

                        // Blue
                        bp1 = (byte)(nx * color1.B + fx * color2.B);

                        bp2 = (byte)(nx * color3.B + fx * color4.B);

                        nBlue = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        // Green
                        bp1 = (byte)(nx * color1.G + fx * color2.G);

                        bp2 = (byte)(nx * color3.G + fx * color4.G);

                        nGreen = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        // Red
                        bp1 = (byte)(nx * color1.R + fx * color2.R);

                        bp2 = (byte)(nx * color3.R + fx * color4.R);

                        nRed = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        bmap.SetPixel(x, y, System.Drawing.Color.FromArgb
                (255, nRed, nGreen, nBlue));
                    }
                }
                pictureBox1.Image = (Bitmap)bmap.Clone();
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            SetGamma(trackBar1.Value / 20, trackBar2.Value / 20, trackBar3.Value / 20);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            SetGamma(trackBar1.Value / 20, trackBar2.Value / 20, trackBar3.Value / 20);
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            SetGamma(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Resize(100, 100);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }
}

   
