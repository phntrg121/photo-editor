using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ.Tools
{
    class Eraser
    {
        Point oldPoint;
        Point currentPoint;
        Pen pen;
        Color color;
        public Eraser()
        {
            color = Color.FromArgb(255, 253, 254, 255);
            pen = new Pen(color, 1);
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }

        public void GetLocation(ref MouseEventArgs e)
        {
            oldPoint = e.Location;
        }

        public void Draw(Graphics g, MouseEventArgs e)
        {
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            currentPoint = e.Location;
            g.DrawLine(pen, oldPoint, currentPoint);
            oldPoint = currentPoint;
        }

        public float Size
        {
            set
            {
                pen.Width = value / 2;
            }
        }
    }
}
