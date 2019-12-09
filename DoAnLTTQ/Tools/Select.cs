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
    public partial class Select : UserControl
    {
        public bool Selected { get; set; }
        public Rectangle Rect { get; set; }
        PointF startP;
        PointF endP;
        public Pen Pen { get; set; }
        public Rectangle FixedRect { get; set; }
        public bool Move { get; set; }
        public Select()
        {
            InitializeComponent();
            Pen = new Pen(Color.Blue, 1);
            Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }
        public void GetLocation(PointF p)
        {
            startP = p;
        }

        public void Selecting(PointF p, Rectangle limitRect)
        {
            endP = p;

            Point p1 = new Point((int)Math.Min(startP.X, endP.X), (int)Math.Min(startP.Y, endP.Y));
            Point p2 = new Point((int)Math.Max(startP.X, endP.X), (int)Math.Max(startP.Y, endP.Y));
            Size size = new Size(p2.X - p1.X, p2.Y - p1.Y);

            Rect = new Rectangle(p1.X, p1.Y, size.Width, size.Height);
            FixedRect = Rectangle.Intersect(Rect, limitRect);
        }

        public void Moving(PointF p, Rectangle limitRect)
        {
            if(Move)
            {
                endP = p;
                int x = Rect.X;
                int y = Rect.Y;
                x += (int)(endP.X - startP.X);
                y += (int)(endP.Y - startP.Y);

                Rect = new Rectangle(x, y, Rect.Width, Rect.Height);
                FixedRect = Rectangle.Intersect(Rect, limitRect);

                startP = endP;
            }
        }

        public bool CheckInRect(PointF p)
        {
            return p.X >= Rect.X && p.X <= Rect.X + Rect.Width &&
                p.Y >= Rect.Y && p.Y <= Rect.Y + Rect.Height;
        }

        public void DrawRect(Graphics g)
        {
            g.DrawRectangle(Pen, Rect);
        }

    }
}
