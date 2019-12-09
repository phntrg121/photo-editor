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
        public Rectangle Rect { get; set; }
        public Pen Pen { get; set; }
        public Bitmap Image { get; set; }
        PointF startP;
        PointF P;
        Matrix translate;
        Matrix scale;
        public bool Move { get; set; }
        public bool Done { get; set; }
        public PointF StartPoint
        {
            set
            {
                startP = value;
                translate.Reset();
                translate.Translate(1, 1);
            }
        }

        public Transform()
        {
            InitializeComponent();
            Pen = new Pen(Color.Black, 1);;
            translate = new Matrix();
            translate.Translate(0, 0);
            scale = new Matrix();
            scale.Scale(1, 1);
        }
        public void GetLocation(PointF p)
        {
            P = p;
        }

        public void Moving(PointF p, Rectangle limitRect)
        {
            if (Move)
            {
                float x = p.X - P.X;
                float y = p.Y - P.Y;

                Rect = new Rectangle((int)(startP.X + x), (int)(startP.Y + y), Rect.Width, Rect.Height);

                translate.Reset();
                translate.Translate(x, y);
            }
        }

        public void DrawRect(Graphics g)
        {
            //g.MultiplyTransform(translate);
            g.DrawRectangle(Pen, Rect);
            g.ResetTransform();
        }

        public void DrawImg(Graphics g)
        {
            g.MultiplyTransform(translate);
            g.DrawImage(Image, startP.X - 1, startP.Y - 1);
            g.ResetTransform();
        }

        public bool CheckInRect(PointF p)
        {
            return p.X >= Rect.X && p.X <= Rect.X + Rect.Width &&
                p.Y >= Rect.Y && p.Y <= Rect.Y + Rect.Height;
        }
    }
}
