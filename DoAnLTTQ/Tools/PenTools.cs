using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ.Tools
{
    class PenTools
    {
        Point oldPoint;
        Point currentPoint;
        Pen pen;
        Color color;
        public PenTools()
        {
            color = Color.Black;
            pen = new Pen(color, 1);
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }

        public void GetLocation(ref MouseEventArgs e)
        {
            oldPoint = e.Location;
        }

        public void Draw(ref Bitmap bmp, ref MouseEventArgs e)
        {
            using(Graphics g = Graphics.FromImage(bmp))
            {
                currentPoint = e.Location;
                g.DrawLine(pen, oldPoint, currentPoint);
                oldPoint = currentPoint;
            }
        }

        public float Size
        {
            set
            {
                pen.Width = value / 2;
            }
        }

        public Color Color
        {
            set
            {
                pen.Color = value;
            }
        }

    }
}
