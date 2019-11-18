using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ
{

    public enum Tool
    {
        Pen, Eraser, Picker, Select
    }
    public partial class DrawSpace : UserControl
    {
        private Bitmap processing;
        private Bitmap final;
        private Color color;
        private float lineSize;
        private Tools.PenTools pen;
        private Tools.Picker picker;
        private Tools.Eraser eraser;
        private Tools.Select select;
        Graphics g;

        public DrawSpace()
        {
            InitializeComponent();
            pen = new Tools.PenTools();
            picker = new Tools.Picker();
            eraser = new Tools.Eraser();
            select = new Tools.Select();
        }

        public void Init()
        {
            processing = new Bitmap(this.Size.Width, this.Size.Height);
            g = Graphics.FromImage(processing);
        }

        public PictureBox Event
        {
            get
            {
                return frontBox;
            }
        }

        public bool CurrentVisible { get; set; }

        public Image BackBoxImage
        {
            set
            {
                backBox.Image = value;
            }
        }
        public Image FrontBoxImage
        {
            set
            {
                frontBox.Image = value;
            }
        }
        public Image ProcessBoxImage
        {
            set
            {
                processing = (Bitmap)value;
                g = Graphics.FromImage(processing);
            }
            get
            {
                return processing;
            }
        }

        public void ClearProcess()
        {
            if (processing == null) return;
            g.Clear(Color.Transparent);
        }

        public Image Final
        {
            set
            {
                final = (Bitmap)value;
            }
            get
            {
                return final;
            }
        }

        public void ColorUpdate(Color c)
        {
            color = c;
            pen.Color = color;
        }

        public void LineSizeUpdate(float n)
        {
            lineSize = n;
            pen.Size = lineSize;
            eraser.Size = lineSize;
        }

        public Color GetColor()
        {
            return color;
        }

        public void BGGenerator(Color c)
        {
            if(c==Color.Transparent)
            {
                int m = (int)Math.Ceiling((double)Size.Width / 512);
                int n = (int)Math.Ceiling((double)Size.Height / 512);
                Bitmap bg = new Bitmap(512 * m, 512 * n);
                using (Graphics g = Graphics.FromImage(bg))
                {
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            g.DrawImage(Properties.Resources.TransparencyBG, i * 512, j * 512);
                        }
                    }
                    BackgroundImage = bg;
                }
            }
            else
            {
                Bitmap bg = new Bitmap(Size.Width, Size.Height);
                using (Graphics g = Graphics.FromImage(bg))
                {
                    using (SolidBrush brush = new SolidBrush(c))
                        g.FillRectangle(brush, 0, 0, Size.Width, Size.Height);
                    BackgroundImage = bg;
                }
            }
        }

        public void Event_Mouse_Down(MouseEventArgs e, Tool current)
        {
            if (processing == null)
            {
                processing = new Bitmap(this.Size.Width, this.Size.Height);
                g = Graphics.FromImage(processing);
            }

            switch (current)
            {
                case Tool.Pen:
                    {
                        pen.GetLocation(ref e);
                    }
                    break;
                case Tool.Picker:
                    {
                        color = picker.GetColor(ref final, ref e);
                    }
                    break;
                case Tool.Eraser:
                    {
                        eraser.GetLocation(ref e);
                    }
                    break;
                case Tool.Select:
                    {
                        select.GetLocation(ref e);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Event_Mouse_Move(MouseEventArgs e, Tool current)
        {
            if (processing == null)
                return;

            if (e.Button == MouseButtons.Left)
            {
                switch (current)
                {
                    case Tool.Pen:
                        {
                            pen.Draw(g, e);
                            if (CurrentVisible)
                                processBox.Image = processing;
                        }
                        break;
                    case Tool.Eraser:
                        {
                            eraser.Draw(g, e);
                            if (CurrentVisible)
                                processBox.Image = processing;
                            
                        }
                        break;
                    case Tool.Select:
                        {
                            select.SelectField(this, e);
                            if (CurrentVisible)
                                processBox.Image = processing;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void Event_Mouse_Up(MouseEventArgs e, Tool current)
        {
        }
    }
}
