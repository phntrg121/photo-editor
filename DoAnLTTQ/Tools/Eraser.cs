﻿using System;
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
        Point oldPoint;
        Point currentPoint;
        Pen pen;
        Color color;
        int size;
        public Eraser()
        {
            InitializeComponent();
            color = Color.FromArgb(255, 253, 254, 255);
            pen = new Pen(color, 1);
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            Dock = DockStyle.Fill;
            size = 10;
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

        private void Bar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                size = (int)((float)e.Location.X / (sender as Control).Width * 100);
                if (size > 100) size = 100;
                if (size < 0) size = 1;
                label3.Text = size.ToString();
                pen.Width = size;
                BarUpdate(sender as Control, size);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            BarUpdate(sizeBar, size);
        }

        private void BarUpdate(Control sender, int val)
        {
            using (Graphics g = sender.CreateGraphics())
            {
                int w = (int)Math.Ceiling(((float)val / 100) * sender.Width);
                g.Clear(sender.BackColor);
                g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, w, sender.Height));
            }
        }
    }
}
