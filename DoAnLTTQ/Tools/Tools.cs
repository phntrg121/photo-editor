using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace DoAnLTTQ.Tools
{
    public class Tools
    {
        private Tool tool;
        public PenTool Pen;
        public Picker Picker;
        public Eraser Eraser;

        public Tool Tool
        {
            get => tool;
            set
            {
                tool = value;
                switch (value)
                {
                    case Tool.Pen:
                        Current = Pen;
                        break;
                    case Tool.Eraser:
                        Current = Eraser;
                        break;
                    case Tool.Picker:
                        Current = Picker;
                        break;
                    default:
                        Current = null;
                        break;
                }
            }
        }
        public UserControl Current { get; set; }

        public Tools()
        {
            Pen = new PenTool();
            Picker = new Picker();
            Eraser = new Eraser();

            tool = Tool.Pen;
            Current = Pen;
        }

        public Color Color
        {
            set
            {
                Pen.Color = value;
            }
        }


    }
}
