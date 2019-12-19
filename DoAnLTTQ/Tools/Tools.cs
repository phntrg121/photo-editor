using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace DoAnLTTQ.Tools
{
    public enum Tool
    {
        Pen, Eraser, Picker, Select, Drag, Transform, Shape, Line, Bucket
    }

    public class Tools
    {
        private Tool tool;
        public PenTool Pen;
        public Picker Picker;
        public Eraser Eraser;
        public Select Select;
        public Drag Drag;
        public Transform Transform;
        public Shape Shape;
        public Bucket Bucket;
        public Line Line;
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
                    case Tool.Select:
                        Current = Select;
                        break;
                    case Tool.Transform:
                        Current = Transform;
                        break;
                    case Tool.Drag:
                        Current = Drag;
                        break;
                    case Tool.Shape:
                        Current = Shape;
                        break;
                    case Tool.Line:
                        Current = Line;
                        break;
                    case Tool.Bucket:
                        Current = Bucket;
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
            Select = new Select();
            Transform = new Transform();
            Drag = new Drag();
            Shape = new Shape();
            Line = new Line();
            Bucket = new Bucket();
            tool = Tool.Pen;
            Current = Pen;
        }

        public Color Color
        {
            set
            {
                Pen.Color = value;
                Shape.Color = value;
                Line.Color = value;
                Bucket.Color = value;
            }
        }

        public int Size
        {
            get
            {
                if (tool == Tool.Pen)
                    return Pen.ToolSize;
                if (tool == Tool.Eraser)
                    return Eraser.ToolSize;
                return 0;
            }
        }
    }
}
