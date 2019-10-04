using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    class LayerRowManager
    {
        List<LayerRow> layers;
        int current;
        Size layerSize;
        public LayerRowManager()
        {
            layers = new List<LayerRow>();
        }

        public int Count
        {
            get
            {
                return layers.Count;
            }
        }

        public Layer Current
        {
            get
            {
                return layers[current].Layer;
            }
        }
        public int CurrentIndex
        {
            get
            {
                return current;
            }
        }

        public Bitmap FinalImageUpdate()
        {
            Bitmap bmp = new Bitmap(layerSize.Width, layerSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach(LayerRow row in layers)
                {
                    if (row.Layer.Visible)
                        g.DrawImage(row.Layer.Image, 0, 0, layerSize.Width, layerSize.Height);
                }
            }
            return bmp;
        }

        public void AddLayerRow(ref Panel panel, ref Layer layer)
        {
            if (layers.Count == 0)
                layerSize = layer.Image.Size;

            LayerRow row = new LayerRow();
            row.Layer = layer;
            row.Text = layer.Name;
            layers.Add(row);
            current = layers.Count - 1;
            panel.Controls.Add(row);
            Allocation(ref panel);
        }

        public void RemoveLayerRow(ref Panel panel)
        {
            layers[current].Layer.Dispose();
            layers[current].Dispose();
            panel.Controls.Remove(layers[current]);
            layers.Remove(layers[current]);
            if (current != 0)
                current--;
            Allocation(ref panel);
        }

        void Allocation(ref Panel panel)
        {
            foreach (LayerRow row in layers)
            {
                int index = layers.IndexOf(row);
                int offset = (layers.Count - 1 - index) * (row.Size.Height + 3);
                row.Location = new Point(2, 2 + offset);
                if (index == current)
                    row.BackColor = Color.LightBlue;
                else
                    row.BackColor = Color.White;
            }
            panel.Refresh();
        }

        public void UpdateName()
        {
            layers[current].Text = layers[current].Layer.Name;
        }

        public void MoveUp(ref Panel panel)
        {
            SwapRow(layers, current, current + 1);
            current++;
            Allocation(ref panel);
        }

        public void MoveDown(ref Panel panel)
        {
            SwapRow(layers, current, current - 1);
            current--;
            Allocation(ref panel);
        }

        void SwapRow(List<LayerRow> list, int row1, int row2)
        {
            LayerRow tmp = list[row1];
            list[row1] = list[row2];
            list[row2] = tmp;
        }
    }
}
