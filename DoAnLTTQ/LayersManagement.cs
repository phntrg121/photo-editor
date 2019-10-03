using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    class LayersManagement
    {
        readonly List<Layer> layerList;
        Size size;
        int current;

        public LayersManagement(Size s)
        {
            layerList = new List<Layer>();
            size = s;
        }

        public Size LayerSize
        {
            set
            {
                if (size == new Size(0,0))
                    size = value;
            }
        }

        public void Add(Bitmap image, string name, bool visible)
        {
            layerList.Add(new Layer(image, name, visible));
            current = layerList.Count - 1;
        }

        public int LayerCount
        {
            get
            {
                return layerList.Count();
            }
        }

        public Layer Current
        {
            get
            {
                return layerList[current];
            }
        }

        public void Remove()
        {
            layerList[current].Dispose();
            layerList.Remove(layerList[current]);
            if (current == layerList.Count)
                current--;
        }

        public Bitmap FinalImageUpdate()
        {
            Bitmap final = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(final))
            {
                foreach (Layer layer in layerList)
                {
                    if (layer.Visible)
                        g.DrawImage(layer.Image, 0, 0);
                }
            }
            return final;
        }

        public void UpdatePanel(ref Panel panel )
        {
            foreach(Control c in panel.Controls)
            {
                c.Dispose();
            }
            panel.Controls.Clear();
            int count = layerList.Count - 1;
            int offset;
            foreach(Layer layer in layerList)
            {
                LayerRow row = new LayerRow()
                {
                    Text = layer.Name,
                    layerVisible = layer.Visible
                };
                offset = (count - layerList.IndexOf(layer)) * (row.Size.Height + 4);
                row.Location = new Point(2, 2 + offset);
                if (layerList.IndexOf(layer) == current)
                    row.BackColor = Color.LightBlue;
                panel.Controls.Add(row);
            }
        }
    }

    class Layer : IDisposable
    {
        Bitmap image;
        string name;
        bool visible;

        public Layer(Bitmap image, string name, bool visible)
        {
            this.image = new Bitmap(image);
            this.name = name;
            this.visible = visible;
        }
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                image.Dispose();
            }
        }

        public ref Bitmap Image
        {
            get
            {
                return ref image;
            }
        }

        public string Name
        {
            set
            {
                if (value != "")
                    name = value;
            }
            get
            {
                return name;
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
        }

        public void ChangeVisible()
        {
            visible = !visible;
        }
    }
}
