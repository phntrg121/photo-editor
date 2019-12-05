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
    public partial class PenTool : UserControl
    {
        PointF oldPoint;
        PointF currentPoint;
        Pen pen;
        Color color;
        int size;
        int opacity;
        Graphics gSize;
        Graphics gOpacity;

        public PenTool()
        {
            InitializeComponent();
            color = Color.Black;
            pen = new Pen(color, 10);
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            this.Dock = DockStyle.Fill;
            size = 10;
            opacity = 100;
            sizeBar.Image = new Bitmap(sizeBar.Width, sizeBar.Height);
            opacityBar.Image = new Bitmap(sizeBar.Width, sizeBar.Height);
            gSize = Graphics.FromImage(sizeBar.Image);
            gOpacity = Graphics.FromImage(opacityBar.Image);
        }

        public void GetLocation(PointF p)
        {
            oldPoint = p;
        }

        public void Draw(Graphics g, PointF p)
        {
            currentPoint = p;
            g.DrawLine(pen, oldPoint, currentPoint);
            oldPoint = currentPoint;
        }

        public int ToolSize { get => size; }

        public Color Color
        {
            set
            {
                pen.Color = Color.FromArgb((int)(255 * (float)opacity / 100), value);
            }
        }

        private void Bar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control c = sender as Control;

                int val = (int)((float)e.Location.X / c.Width * 100);
                if (val > 100) val = 100;
                if (val < 0) val = 1;

                if (c == sizeBar)
                {
                    size = val;
                    label3.Text = size.ToString();
                    pen.Width = size;
                    BarUpdate(sizeBar, gSize, size);
                }
                else if(c == opacityBar)
                {
                    opacity = val;
                    label4.Text = opacity.ToString();
                    pen.Color = Color.FromArgb((int)(255 * (float)opacity / 100), pen.Color);
                    BarUpdate(opacityBar, gOpacity, opacity);
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            BarUpdate(sizeBar, gSize, size);
            BarUpdate(opacityBar, gOpacity, opacity);
        }

        private void BarUpdate(Control sender, Graphics g, int val)
        {
            int w = (int)Math.Ceiling(((float)val / 100) * sender.Width);
            g.Clear(sender.BackColor);
            g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, w, sender.Height));
            sender.Invalidate();
        }
    }
}
