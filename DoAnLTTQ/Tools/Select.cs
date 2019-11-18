using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ.Tools
{
    class Select
    {
        Point oldPoint;
        Point currentPoint;
        Pen pen;
        Color color;
        public Select()
        {
        
        }

        public void GetLocation(ref MouseEventArgs e)
        {
            oldPoint = e.Location;
        }

        public void SelectField(System.Windows.Forms.Control control, MouseEventArgs e)
        {
            int minX = Math.Min(e.Location.X, oldPoint.X);
            int minY = Math.Min(e.Location.Y, oldPoint.Y);
            int maxX = Math.Max(e.Location.X, oldPoint.X);
            int maxY = Math.Max(e.Location.Y, oldPoint.Y);
            control.SetBounds(minX, minY, maxX - minX, maxY - minY);
            control.Invalidate();
        }

    }
}
