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
    public partial class LayerContainer : UserControl
    {
        List<LayerRow> layers;
        Bitmap final;
        int current;
        Size layerSize;

        public LayerContainer()
        {
            InitializeComponent();
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

        public LayerRow CurrentRow
        {
            set
            {
                current = layers.IndexOf(value);
                Allocation();
            }
        }

        public Bitmap Final
        {
            get
            {
                return final;
            }
        }

        public void FinalImageUpdate()
        {
            final.Dispose();
            final = new Bitmap(layerSize.Width, layerSize.Height);
            using (Graphics g = Graphics.FromImage(final))
            {
                foreach (LayerRow row in layers)
                {
                    if (row.Layer.Visible)
                        g.DrawImage(row.Layer.Image, 0, 0, layerSize.Width, layerSize.Height);
                }
            }
        }

        public void AddLayerRow(ref Layer layer)
        {
            if (layers.Count == 0)
            {
                layerSize = layer.Image.Size;
                final = new Bitmap(layerSize.Width, layerSize.Height);
            }

            LayerRow row = new LayerRow();
            row.Layer = layer;
            row.Text = layer.Name;
            layers.Add(row);
            current = layers.Count - 1;
            panel.Controls.Add(row);
            Allocation();
        }

        public void RemoveLayerRow()
        {
            layers[current].Layer.Dispose();
            layers[current].Dispose();
            panel.Controls.Remove(layers[current]);
            layers.Remove(layers[current]);
            if (current != 0)
                current--;
            Allocation();
        }

        private void Allocation()
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

        public void MoveUp()
        {
            SwapRow(layers, current, current + 1);
            current++;
            Allocation();
        }

        public void MoveDown()
        {
            SwapRow(layers, current, current - 1);
            current--;
            Allocation();
        }

        private void SwapRow(List<LayerRow> list, int row1, int row2)
        {
            LayerRow tmp = list[row1];
            list[row1] = list[row2];
            list[row2] = tmp;
        }

        public void Merge()
        {
            using(Graphics g = Graphics.FromImage(layers[current -1].Layer.Image))
            {
                g.DrawImage(layers[current].Layer.Image, 0, 0, layerSize.Width, layerSize.Height);
            }
            RemoveLayerRow();
        }

        public void Duplicate()
        {
            LayerRow newRow = new LayerRow();
            newRow.Text = layers[current].Text + "-Copy";
            newRow.Layer = new Layer(layers[current].Layer.Image, newRow.Text, true);
            current++;
            layers.Insert(current, newRow);
            panel.Controls.Add(newRow);
            Allocation();
        }

    }
}
