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
        private List<LayerRow> layers;
        private Bitmap final;
        private Bitmap back;
        private Bitmap front;
        private int current;
        private Size layerSize;

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

        public LayerRow Current
        {
            get
            {
                return layers[current];
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
        public Bitmap Back
        {
            get
            {
                return back;
            }
        }
        public Bitmap Front
        {
            get
            {
                return front;
            }
        }

        public void ProcessUpdate(Bitmap processing)
        {
            if (processing != null)
            {
                using (Graphics g = Graphics.FromImage(layers[current].Layer.Image))
                    g.DrawImageUnscaled(processing, 0, 0, layerSize.Width, layerSize.Height);
                processing.Dispose();
            }
        }

        public void BackUpdate()
        {
            using (Graphics g = Graphics.FromImage(back))
            {
                g.Clear(Color.Transparent);
                for (int i = 0; i <= current; i++)
                {
                    if (layers[i].Layer.Visible)
                    {
                        g.DrawImageUnscaled(layers[i].Layer.ImageWithOpacity, 0, 0, layerSize.Width, layerSize.Height);
                    }
                }
            }
        }

        public void FrontUpdate()
        {
            using (Graphics g = Graphics.FromImage(front))
            {
                g.Clear(Color.Transparent);
                for (int i = current + 1; i < layers.Count; i++)
                {
                    if (layers[i].Layer.Visible)
                    {
                        g.DrawImageUnscaled(layers[i].Layer.ImageWithOpacity, 0, 0, layerSize.Width, layerSize.Height);
                    }
                }
            }
        }

        public void FinalUpdate()
        {
            using (Graphics g = Graphics.FromImage(final))
            {
                g.Clear(Color.Transparent);
                g.DrawImageUnscaled(back, 0, 0, layerSize.Width, layerSize.Height);
                g.DrawImageUnscaled(front, 0, 0, layerSize.Width, layerSize.Height);
            }
        }

        public void AddLayerRow(ref Layer layer)
        {
            if (layers.Count == 0)
            {
                layerSize = layer.Image.Size;
                final = new Bitmap(layerSize.Width, layerSize.Height);
                back = new Bitmap(layerSize.Width, layerSize.Height);
                front = new Bitmap(layerSize.Width, layerSize.Height);
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
