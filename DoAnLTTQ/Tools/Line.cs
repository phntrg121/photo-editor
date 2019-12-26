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
    public partial class Line : UserControl
    {
        PointF startP;
        PointF endP;
        int size;
        Graphics gSize;
        public Pen Pen { get; set; }
        public Color Color
        {
            set
            {
                Pen.Color = value;
            }
        }
        public bool Drawed;
        public Line()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            Pen = new Pen(Color.Black, 5);
            size = 5;
            sizeBar.Image = new Bitmap(sizeBar.Width, sizeBar.Height);
            gSize = Graphics.FromImage(sizeBar.Image);
        }

        public void GetLocation(PointF p)
        {
            startP = p;
            Drawed = false;
        }

        public void DrawLine(Graphics g, PointF p)
        {
            Drawed = true;
            endP = p;
            Draw(g);
        }

        public void Draw(Graphics g)
        {
            if (Drawed)
                g.DrawLine(Pen, startP, endP);
        }

        private void Bar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control c = sender as Control;

                int val = (int)((float)e.Location.X / c.Width * 100);
                if (val > 100) val = 100;
                if (val < 0) val = 1;
                size = val;
                label3.Text = size.ToString();
                Pen.Width = size;
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
            g.FillRectangle(Brushes.Gainsboro, new Rectangle(0, 0, w, sender.Height));
            sender.Invalidate();
        }
    }
}
