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
        float angle;
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
            }
        }

        public Transform()
        {
            InitializeComponent();
            rotate = new Matrix();
            angle = 0;
        }

        public void Reset()
        {
            angle = 0;
        }

        public void GetLocation(PointF p)
        {
            pos = p;
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

                switch(smallRectIndex)
                {
                    case 0:
                        Rect = new Rectangle(baseR.X, (int)(baseR.Y + y), baseR.Width, (int)(baseR.Height - y));
                        break;
                    case 1:
                        Rect = new Rectangle((int)(baseR.X + x), baseR.Y, (int)(baseR.Width - x), baseR.Height);
                        break;
                    case 2:
                        Rect = new Rectangle(baseR.X, baseR.Y, (int)(baseR.Width + x), baseR.Height);
                        break;
                    case 3:
                        Rect = new Rectangle(baseR.X, baseR.Y, baseR.Width, (int)(baseR.Height + y));
                        break;
                    case 4:
                        Rect = new Rectangle((int)(baseR.X + x), (int)(baseR.Y + y), (int)(baseR.Width - x), (int)(baseR.Height - y));
                        break;
                    case 5:
                        Rect = new Rectangle(baseR.X, (int)(baseR.Y + y), (int)(baseR.Width + x), (int)(baseR.Height - y));
                        break;
                    case 6:
                        Rect = new Rectangle((int)(baseR.X + x), baseR.Y, (int)(baseR.Width - x), (int)(baseR.Height + y));
                        break;
                    case 7:
                        Rect = new Rectangle(baseR.X, baseR.Y, (int)(baseR.Width + x), (int)(baseR.Height + y));
                        break;
                }
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

                double a = (float)Math.Acos((v1.X * v2.X + v1.Y * v2.Y) / (d1 * d2));
                    angle += (float)a;
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

            g.DrawImage(Image, rect.X, rect.Y, rect.Width, rect.Height);
            g.ResetTransform();
        }

        public bool InRect(PointF p)
        {
            return p.X >= rect.X && p.X <= rect.X + rect.Width &&
                p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
        }
        public bool InSmallRect(PointF p)
        {
            for (int i = 0; i < 8; i++)
            {
                if (p.X >= smallrects[i].X && p.X <= smallrects[i].X + smallrects[i].Width &&
                    p.Y >= smallrects[i].Y && p.Y <= smallrects[i].Y + smallrects[i].Height)
                {
                    smallRectIndex = (byte)i;
                    return true;
                }
            }

            return false;
        }
        public bool InRotateRect(PointF p)
        {
            for (int i = 0; i < 4; i++)
            {
                if (p.X >= rotaterects[i].X && p.X <= rotaterects[i].X + rotaterects[i].Width &&
                    p.Y >= rotaterects[i].Y && p.Y <= rotaterects[i].Y + rotaterects[i].Height)
                    return true;
            }
            return false;
        }

        public PointF RotatedPoint(PointF p, bool re = false)
        {
            PointF pp = new PointF();
            double theta = ((re) ? angle : -angle) * Math.PI / 180;
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
