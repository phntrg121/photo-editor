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
        public Rectangle SelectRect { get; set; }
        PointF startP;
        PointF endP;
        public Pen Pen { get; set; }
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

        public void Selecting(PointF p, int w_limit, int h_limit)
        {
            endP = p;

            Point p1 = new Point((int)Math.Min(startP.X, endP.X), (int)Math.Min(startP.Y, endP.Y));
            if (p1.X < 0) p1.X = 0;
            if (p1.Y < 0) p1.Y = 0;
            Point p2 = new Point((int)Math.Max(startP.X, endP.X), (int)Math.Max(startP.Y, endP.Y));
            if (p2.X > w_limit) p2.X = w_limit;
            if (p2.Y > h_limit) p2.Y = h_limit;
            Size size = new Size(p2.X - p1.X, p2.Y - p1.Y);

            SelectRect = new Rectangle(p1.X, p1.Y, size.Width, size.Height);
        }

        public bool CheckInRect(PointF p)
        {
            return p.X >= SelectRect.X && p.X <= SelectRect.X + SelectRect.Width &&
                p.Y >= SelectRect.Y && p.Y <= SelectRect.Y + SelectRect.Height;
        }

    }
}
