using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DoAnLTTQ.Tools
{
    public partial class Transform : UserControl
    {
        Rectangle rect;
        Rectangle baseR;
        PointF startP;
        PointF pos;
        Matrix rotate;
        RectangleF[] smallrects;
        RectangleF[] rotaterects;
        PointF[] smallrectpos;
        PointF center;
        byte smallRectIndex;
        byte rotateRectIndex;
        float angle;
        float old_angle;
        DrawSpace drawspace;
        public Bitmap Image { get; set; }
        public bool Moving { get; set; }
        public bool Resizing { get; set; }
        public bool Rotating { get; set; }
        public bool Done { get; set; }
        public Rectangle Rect
        {
            get => rect;
            set
            {
                rect = value;
                MakeSmallRects();
            }
        }
        public PointF StartPoint
        {
            set
            {
                startP = value;
                baseR = rect;
                PointF tmp = new PointF(rect.X + ((float)rect.Width / 2), rect.Y + ((float)rect.Height / 2));
                if (Resizing || Moving)
                {
                    tmp = RotatedPoint(tmp, true);
                    Rect = new Rectangle((int)(tmp.X - (float)rect.Width / 2), (int)(tmp.Y - (float)rect.Height / 2), rect.Width, rect.Height);
                    startP = rect.Location;
                    baseR = rect;
                }
                center = tmp;

                Moving = Rotating = Resizing = false;
                old_angle = angle;
            }
        }

        public Transform()
        {
            InitializeComponent();
            rotate = new Matrix();
            angle = 0;
            radioButton1.Checked = true;
        }

        public void Reset()
        {
            old_angle = angle = 0;
        }

        public void GetLocation(PointF p , DrawSpace ds)
        {
            pos = p;
            drawspace = ds;
        }

        public void Translate(PointF p)
        {
            if (Moving)
            {
                float x = p.X - pos.X;
                float y = p.Y - pos.Y;

                Rect = new Rectangle((int)(startP.X + x), (int)(startP.Y + y), rect.Width, rect.Height);
            }
        }

        public void Resize(PointF p)
        {
            if(Resizing)
            {
                float x = p.X - pos.X;
                float y = p.Y - pos.Y;

                int X = baseR.X;
                int Y = baseR.Y;
                int W = baseR.Width;
                int H = baseR.Height;

                switch (smallRectIndex)
                {
                    case 0: // top
                        Y = Math.Min((int)(baseR.Y + y), baseR.Y + baseR.Height);
                        H = (int)(baseR.Height - y);
                        break;
                    case 1: // left
                        X = Math.Min((int)(baseR.X + x), baseR.X + baseR.Width);
                        W = (int)(baseR.Width - x);
                        break;
                    case 2: // right
                        W = (int)(baseR.Width + x);
                        X = Math.Min(X, baseR.X + W);
                        break;
                    case 3: // bottom
                        H = (int)(baseR.Height + y);
                        Y = Math.Min(Y, baseR.Y + H);
                        break;
                    case 4: // top left
                        X = Math.Min((int)(baseR.X + x), baseR.X + baseR.Width);
                        Y = Math.Min((int)(baseR.Y + y), baseR.Y + baseR.Height);
                        W = (int)(baseR.Width - x);
                        H = (int)(baseR.Height - y);
                        break;
                    case 5: // top right
                        Y = Math.Min((int)(baseR.Y + y), baseR.Y + baseR.Height);
                        W = (int)(baseR.Width + x);
                        X = Math.Min(X, baseR.X + W);
                        H = (int)(baseR.Height - y);
                        break;
                    case 6: // bottom left
                        X = Math.Min((int)(baseR.X + x), baseR.X + baseR.Width);
                        W = (int)(baseR.Width - x);
                        H = (int)(baseR.Height + y);
                        Y = Math.Min(Y, baseR.Y + H);
                        break;
                    case 7: // bottom right
                        W = (int)(baseR.Width + x);
                        X = Math.Min(X, baseR.X + W);
                        H = (int)(baseR.Height + y);
                        Y = Math.Min(Y, baseR.Y + H);
                        break;
                }

                Rect = new Rectangle(X, Y, Math.Abs(W), Math.Abs(H));
            }
        }

        public void Rotate(PointF p)
        {
            if(Rotating)
            {
                PointF v1 = new PointF(pos.X - center.X, pos.Y - center.Y);
                PointF v2 = new PointF(p.X - center.X, p.Y - center.Y);

                double d1 = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
                double d2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);

                double cos_a = (v1.X * v2.X + v1.Y * v2.Y) / (d1 * d2);
                double a = Math.Acos(cos_a) * 180 / Math.PI;

                //goc lech
                PointF t = new PointF(v1.X, 0);
                double dt = Math.Sqrt(t.X * t.X + t.Y * t.Y);
                double cos_t = (v1.X * t.X + v1.Y * t.Y) / (d1 * dt);
                double tmp = Math.Acos(cos_t) * 180 / Math.PI;

                if (rotateRectIndex == 0)
                {
                    PointF m = RotatedPoint(p, fix: (float)tmp);
                    if (m.Y > center.Y) a = 360 - a;
                }
                else if(rotateRectIndex == 1)
                {
                    PointF m = RotatedPoint(p, true, fix: (float)tmp);
                    if (m.Y < center.Y) a = 360 - a;
                }
                 else if (rotateRectIndex == 2)
                {
                    PointF m = RotatedPoint(p, true, fix: (float)tmp);
                    if (m.Y > center.Y) a = 360 - a;
                }
                else
                {
                    PointF m = RotatedPoint(p, fix: (float)tmp);
                    if (m.Y < center.Y) a = 360 - a;
                }

                angle = old_angle + (float)a;
                angle = angle % 360;
            }
        }

        public void DrawRect(Graphics g)
        {
            rotate.RotateAt(angle, center);
            g.MultiplyTransform(rotate);
            rotate.Reset();

            g.DrawRectangle(Pens.Black, rect);
            g.FillRectangles(Brushes.White, smallrects);
            g.DrawRectangles(Pens.Black, smallrects);
            g.ResetTransform();
        }

        public void DrawImg(Graphics g)
        {
            rotate.RotateAt(angle, center);
            g.MultiplyTransform(rotate);
            rotate.Reset();

            using (Bitmap bmp = (Bitmap)Image.Clone())
            {
                if (radioButton2.Checked) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                else if (radioButton3.Checked) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                else if (radioButton4.Checked) bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);

                g.DrawImage(bmp, rect.X, rect.Y, rect.Width, rect.Height);
            }
            g.ResetTransform();
        }

        public bool InRect(PointF p)
        {
            return p.X >= rect.X && p.X <= rect.X + rect.Width &&
                p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
        }
        public bool InSmallRect(PointF p)
        {
            for (byte i = 0; i < 8; i++)
            {
                if (p.X >= smallrects[i].X && p.X <= smallrects[i].X + smallrects[i].Width &&
                    p.Y >= smallrects[i].Y && p.Y <= smallrects[i].Y + smallrects[i].Height)
                {
                    smallRectIndex = i;
                    return true;
                }
            }

            return false;
        }
        public bool InRotateRect(PointF p)
        {
            for (byte i = 0; i < 4; i++)
            {
                if (p.X >= rotaterects[i].X && p.X <= rotaterects[i].X + rotaterects[i].Width &&
                    p.Y >= rotaterects[i].Y && p.Y <= rotaterects[i].Y + rotaterects[i].Height)
                {
                    rotateRectIndex = i;
                    return true;
                }
            }
            return false;
        }

        public PointF RotatedPoint(PointF p, bool re = false, float fix = 0)
        {
            PointF pp = new PointF();
            if (fix == 0) fix = old_angle;
            double theta = ((re) ? fix : -fix) * Math.PI / 180;
            pp.X = (float)(Math.Cos(theta) * (p.X - center.X) - Math.Sin(theta) * (p.Y - center.Y) + center.X);
            pp.Y = (float)(Math.Sin(theta) * (p.X - center.X) + Math.Cos(theta) * (p.Y - center.Y) + center.Y);
            return pp;
        }

        void MakeSmallRects()
        {
            smallrectpos = null;
            smallrects = null;
            rotaterects = null;

            smallrectpos = new PointF[]
            {
                new PointF(rect.X + rect.Width / 2, rect.Y), // top
                new PointF(rect.X, rect.Y + rect.Height / 2), // left
                new PointF(rect.X + rect.Width, rect.Y + rect.Height / 2), // right
                new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height), // bottom
                new PointF(rect.X, rect.Y), // top-left
                new PointF(rect.X + rect.Width, rect.Y), // top-right
                new PointF(rect.X, rect.Y + rect.Height), // bottom-left
                new PointF(rect.X + rect.Width, rect.Y + rect.Height) // bottom-right
            };

            smallrects = new RectangleF[]
            {
                new RectangleF(smallrectpos[0].X - 4, smallrectpos[0].Y - 4, 8, 8),
                new RectangleF(smallrectpos[1].X - 4, smallrectpos[1].Y - 4, 8, 8),
                new RectangleF(smallrectpos[2].X - 4, smallrectpos[2].Y - 4, 8, 8),
                new RectangleF(smallrectpos[3].X - 4, smallrectpos[3].Y - 4, 8, 8),
                new RectangleF(smallrectpos[4].X - 4, smallrectpos[4].Y - 4, 8, 8),
                new RectangleF(smallrectpos[5].X - 4, smallrectpos[5].Y - 4, 8, 8),
                new RectangleF(smallrectpos[6].X - 4, smallrectpos[6].Y - 4, 8, 8),
                new RectangleF(smallrectpos[7].X - 4, smallrectpos[7].Y - 4, 8, 8)
            };

            rotaterects = new RectangleF[]
            {
                new RectangleF(smallrectpos[4].X - 20, smallrectpos[4].Y - 20, 20, 20),
                new RectangleF(smallrectpos[5].X, smallrectpos[5].Y - 20, 20, 20),
                new RectangleF(smallrectpos[6].X - 20, smallrectpos[6].Y, 20, 20),
                new RectangleF(smallrectpos[7].X, smallrectpos[7].Y, 20, 20)
            };
        }
    }
}
