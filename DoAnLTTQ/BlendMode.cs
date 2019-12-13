using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace DoAnLTTQ
{
    public enum Blend
    {
        Normal, Multiply, Screen, Darken, Lighten, Overlay
    }

    public class BlendMode
    {
        public static Bitmap Multiply(Bitmap mask, Bitmap img)
        {
            Bitmap equation = new Bitmap(mask);

            BitmapData data1 = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int size1 = Math.Abs(data1.Stride) * img.Height;
            byte[] pixels1 = new byte[size1];
            System.Runtime.InteropServices.Marshal.Copy(data1.Scan0, pixels1, 0, size1);

            BitmapData data2 = equation.LockBits(new Rectangle(0, 0, equation.Width, equation.Height), ImageLockMode.ReadOnly, equation.PixelFormat);
            int size2 = Math.Abs(data2.Stride) * equation.Height;
            byte[] pixels2 = new byte[size2];
            System.Runtime.InteropServices.Marshal.Copy(data2.Scan0, pixels2, 0, size2);

            for (int i = 0; i < size1; i += 4)
            {
                float r, g, b;

                if (pixels1[i + 3] != 0 && pixels2[i + 3] != 0)
                {
                    r = (float)(pixels1[i + 0] * pixels2[i + 0]) / 255;
                    g = (float)(pixels1[i + 1] * pixels2[i + 1]) / 255;
                    b = (float)(pixels1[i + 2] * pixels2[i + 2]) / 255;

                    pixels2[i + 0] = (byte)r;
                    pixels2[i + 1] = (byte)g;
                    pixels2[i + 2] = (byte)b;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels2, 0, data2.Scan0, size2);
            img.UnlockBits(data1);
            equation.UnlockBits(data2);

            return equation;
        }

        public static Bitmap Screen(Bitmap mask, Bitmap img)
        {
            Bitmap equation = new Bitmap(mask);

            BitmapData data1 = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int size1 = Math.Abs(data1.Stride) * img.Height;
            byte[] pixels1 = new byte[size1];
            System.Runtime.InteropServices.Marshal.Copy(data1.Scan0, pixels1, 0, size1);

            BitmapData data2 = equation.LockBits(new Rectangle(0, 0, equation.Width, equation.Height), ImageLockMode.ReadOnly, equation.PixelFormat);
            int size2 = Math.Abs(data2.Stride) * equation.Height;
            byte[] pixels2 = new byte[size2];
            System.Runtime.InteropServices.Marshal.Copy(data2.Scan0, pixels2, 0, size2);

            for (int i = 0; i < size1; i += 4)
            {
                float r, g, b;

                if (pixels1[i + 3] != 0 && pixels2[i + 3] != 0)
                {
                    r = (float)((255 - pixels1[i + 0]) * (255 - pixels2[i + 0])) / 255;
                    g = (float)((255 - pixels1[i + 1]) * (255 - pixels2[i + 1])) / 255;
                    b = (float)((255 - pixels1[i + 2]) * (255 - pixels2[i + 2])) / 255;

                    pixels2[i + 0] = (byte)(255 - r);
                    pixels2[i + 1] = (byte)(255 - g);
                    pixels2[i + 2] = (byte)(255 - b);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels2, 0, data2.Scan0, size2);
            img.UnlockBits(data1);
            equation.UnlockBits(data2);

            return equation;
        }

        public static Bitmap Darken(Bitmap mask, Bitmap img)
        {
            Bitmap equation = new Bitmap(mask);

            BitmapData data1 = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int size1 = Math.Abs(data1.Stride) * img.Height;
            byte[] pixels1 = new byte[size1];
            System.Runtime.InteropServices.Marshal.Copy(data1.Scan0, pixels1, 0, size1);

            BitmapData data2 = equation.LockBits(new Rectangle(0, 0, equation.Width, equation.Height), ImageLockMode.ReadOnly, equation.PixelFormat);
            int size2 = Math.Abs(data2.Stride) * equation.Height;
            byte[] pixels2 = new byte[size2];
            System.Runtime.InteropServices.Marshal.Copy(data2.Scan0, pixels2, 0, size2);

            for (int i = 0; i < size1; i += 4)
            {
                float r, g, b;

                if (pixels1[i + 3] != 0 && pixels2[i + 3] != 0)
                {
                    r = Math.Min(pixels1[i + 0], pixels2[i + 0]);
                    g = Math.Min(pixels1[i + 1], pixels2[i + 1]);
                    b = Math.Min(pixels1[i + 2], pixels2[i + 2]);

                    pixels2[i + 0] = (byte)r;
                    pixels2[i + 1] = (byte)g;
                    pixels2[i + 2] = (byte)b;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels2, 0, data2.Scan0, size2);
            img.UnlockBits(data1);
            equation.UnlockBits(data2);

            return equation;
        }

        public static Bitmap Lighten(Bitmap mask, Bitmap img)
        {
            Bitmap equation = new Bitmap(mask);

            BitmapData data1 = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int size1 = Math.Abs(data1.Stride) * img.Height;
            byte[] pixels1 = new byte[size1];
            System.Runtime.InteropServices.Marshal.Copy(data1.Scan0, pixels1, 0, size1);

            BitmapData data2 = equation.LockBits(new Rectangle(0, 0, equation.Width, equation.Height), ImageLockMode.ReadOnly, equation.PixelFormat);
            int size2 = Math.Abs(data2.Stride) * equation.Height;
            byte[] pixels2 = new byte[size2];
            System.Runtime.InteropServices.Marshal.Copy(data2.Scan0, pixels2, 0, size2);

            for (int i = 0; i < size1; i += 4)
            {
                float r, g, b;

                if (pixels1[i + 3] != 0 && pixels2[i + 3] != 0)
                {
                    r = Math.Max(pixels1[i + 0], pixels2[i + 0]);
                    g = Math.Max(pixels1[i + 1], pixels2[i + 1]);
                    b = Math.Max(pixels1[i + 2], pixels2[i + 2]);

                    pixels2[i + 0] = (byte)r;
                    pixels2[i + 1] = (byte)g;
                    pixels2[i + 2] = (byte)b;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels2, 0, data2.Scan0, size2);
            img.UnlockBits(data1);
            equation.UnlockBits(data2);

            return equation;
        }

        public static Bitmap Overlay(Bitmap mask, Bitmap img)
        {
            Bitmap equation = new Bitmap(mask);

            BitmapData data1 = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int size1 = Math.Abs(data1.Stride) * img.Height;
            byte[] pixels1 = new byte[size1];
            System.Runtime.InteropServices.Marshal.Copy(data1.Scan0, pixels1, 0, size1);

            BitmapData data2 = equation.LockBits(new Rectangle(0, 0, equation.Width, equation.Height), ImageLockMode.ReadOnly, equation.PixelFormat);
            int size2 = Math.Abs(data2.Stride) * equation.Height;
            byte[] pixels2 = new byte[size2];
            System.Runtime.InteropServices.Marshal.Copy(data2.Scan0, pixels2, 0, size2);

            for (int i = 0; i < size1; i += 4)
            {
                float r, g, b;

                if (pixels1[i + 3] != 0 && pixels2[i + 3] != 0)
                {
                    r = (float)pixels1[i + 0] / 255 * (pixels1[i + 0] + (float)(2 * pixels2[i + 0] * (255 - pixels1[i + 0])) / 255);
                    g = (float)pixels1[i + 1] / 255 * (pixels1[i + 1] + (float)(2 * pixels2[i + 1] * (255 - pixels1[i + 1])) / 255);
                    b = (float)pixels1[i + 2] / 255 * (pixels1[i + 2] + (float)(2 * pixels2[i + 2] * (255 - pixels1[i + 2])) / 255);

                    pixels2[i + 0] = (byte)r;
                    pixels2[i + 1] = (byte)g;
                    pixels2[i + 2] = (byte)b;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixels2, 0, data2.Scan0, size2);
            img.UnlockBits(data1);
            equation.UnlockBits(data2);

            return equation;
        }
    }
}
