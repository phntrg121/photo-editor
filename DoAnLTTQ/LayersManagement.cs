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
        List<Layer> layerList;
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

        public void Add(ref Bitmap image, string name, bool visible)
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

        public ref Bitmap Current
        {
            get
            {
                return ref layerList[current].Image;
            }
        }

        public void Remove()
        {
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
                        g.DrawImage(layer.Image, new Point(0, 0));
                }
            }
            return final;
        }


    }

    class Layer
    {
        Bitmap image;
        string name;
        bool visible;

        public Layer(Bitmap image, string name, bool visible)
        {
            this.image = image;
            this.name = name;
            this.visible = visible;
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
