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
    public partial class Eraser : UserControl
    {
        PointF oldPoint;
        PointF currentPoint;
        Pen pen;
        Color color;
        int size;
        Graphics gSize;
        public Eraser()
        {
            InitializeComponent();
            color = Color.Transparent;
            pen = new Pen(color, 10);
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            Dock = DockStyle.Fill;
            size = 10;
            sizeBar.Image = new Bitmap(sizeBar.Width, sizeBar.Height);
            gSize = Graphics.FromImage(sizeBar.Image);
        }

        public void GetLocation(PointF p)
        {
            oldPoint = p;
        }

        public void Draw(Graphics g, PointF p)
        {
            currentPoint = p;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.DrawLine(pen, oldPoint, currentPoint);
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            oldPoint = currentPoint;
        }

        public int ToolSize { get => size; }

        private void Bar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                size = (int)((float)e.Location.X / (sender as Control).Width * 100);
                if (size > 100) size = 100;
                if (size < 0) size = 1;
                label3.Text = size.ToString();
                pen.Width = size;
                if (sender == sizeBar)
                    BarUpdate(sizeBar, gSize, size);

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            BarUpdate(sizeBar, gSize, size);
        }

        private void BarUpdate(Control sender, Graphics g, int val)
        {
            int w = (int)Math.Ceiling(((float)val / 100) * sender.Width);
            g.Clear(sender.BackColor);
            g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, w, sender.Height));
            sender.Invalidate();
        }

        public void MakeTransparent(Bitmap bmp, Rectangle rect)
        {
            using (Bitmap b = new Bitmap(rect.Width, rect.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.DrawImageUnscaled(b, rect.X, rect.Y);
            }
        }
    }
}
