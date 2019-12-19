using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ.Tools
{
    public partial class Bucket : UserControl
    {
        Bitmap fillpart;
        public Color Color { get; set; }
        Color basecolor;
        public Bucket()
        {
            InitializeComponent();
            Color = Color.Black;
        }

        public void Fill(PointF p, Bitmap bmp, int x = 0, int y = 0)
        {
            basecolor = bmp.GetPixel((int)p.X - x, (int)p.Y - y);
            fillpart = new Bitmap(bmp.Width, bmp.Height);

            FloodFill(fillpart, bmp , new Point((int)p.X - x, (int)p.Y - y));

        }

        public void DrawFill(Graphics g , int x = 0, int y =0)
        {
            g.DrawImageUnscaled(fillpart, x, y);
            fillpart.Dispose();
        }

        bool Match(Color c1, Color c2)
        {
            return (c1.R == c2.R) && (c1.G == c2.G) && (c1.B == c2.B) && (c1.A == c2.A);
        }

        void FloodFill(Bitmap des, Bitmap src, Point p)
        {
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(p);

            int W = src.Width;
            int H = src.Height;

            while (queue.Count > 0)
            {
                Point p2 = queue.Dequeue();

                if (!Match(src.GetPixel(p2.X, p2.Y), basecolor)) continue;
                if (Match(des.GetPixel(p2.X, p2.Y), Color)) continue;

                Point p3 = p2, p4 = new Point(p2.X + 1, p2.Y);

                while ((p3.X > 0) && Match(src.GetPixel(p3.X, p3.Y), basecolor))
                {
                    des.SetPixel(p3.X, p3.Y, Color);

                    if ((p3.Y > 0) && Match(src.GetPixel(p3.X, p3.Y - 1), basecolor))
                        queue.Enqueue(new Point(p3.X, p3.Y - 1));

                    if ((p3.Y < H - 1) && Match(src.GetPixel(p3.X, p3.Y + 1), basecolor))
                        queue.Enqueue(new Point(p3.X, p3.Y + 1));

                    p3.X--;
                }

                while ((p4.X < W - 1) && Match(src.GetPixel(p4.X, p4.Y), basecolor))
                {
                    des.SetPixel(p4.X, p4.Y, Color);

                    if ((p4.Y > 0) && Match(src.GetPixel(p4.X, p4.Y - 1), basecolor))
                        queue.Enqueue(new Point(p4.X, p4.Y - 1));

                    if ((p4.Y < H - 1) && Match(src.GetPixel(p4.X, p4.Y + 1), basecolor))
                        queue.Enqueue(new Point(p4.X, p4.Y + 1));

                    p4.X++;
                }
            }
        }

    }
}
