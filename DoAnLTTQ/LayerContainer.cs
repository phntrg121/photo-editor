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
        private Graphics gFinal;
        private Graphics gBack;
        private Graphics gFront;
        private int current;
        private Size layerSize;
        private Stack<KeyValuePair<int, LayerRow>> deletedRows;

        public LayerContainer()
        {
            InitializeComponent();
            layers = new List<LayerRow>();
            deletedRows = new Stack<KeyValuePair<int, LayerRow>>();
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

        public void ProcessUpdate(Bitmap processing, bool preview = false)
        {
            if (processing != null)
            {
                if (!preview) layers[current].Layer.Stacking();
                using (Graphics g = Graphics.FromImage(layers[current].Layer.Image))
                {
                    g.DrawImageUnscaled(processing, 0, 0, layerSize.Width, layerSize.Height);
                }

                layers[current].Layer.Image.MakeTransparent(Color.FromArgb(255, 253, 254, 255));
            }
        }

        public void BackUpdate()
        {
            gBack.Clear(Color.Transparent);
            for (int i = 0; i <= current; i++)
            {
                if (layers[i].Layer.Visible)
                {
                    gBack.DrawImageUnscaled(layers[i].Layer.ImageWithOpacity, 0, 0, layerSize.Width, layerSize.Height);
                }
            }
        }

        public void FrontUpdate()
        {
            gFront.Clear(Color.Transparent);
            for (int i = current + 1; i < layers.Count; i++)
            {
                if (layers[i].Layer.Visible)
                {
                    gFront.DrawImageUnscaled(layers[i].Layer.ImageWithOpacity, 0, 0, layerSize.Width, layerSize.Height);
                }
            }
        }

        public void FinalUpdate()
        {
            gFinal.Clear(Color.Transparent);
            gFinal.DrawImageUnscaled(back, 0, 0, layerSize.Width, layerSize.Height);
            gFinal.DrawImageUnscaled(front, 0, 0, layerSize.Width, layerSize.Height);
        }

        public void AddLayerRow(ref Layer layer)
        {
            if (layers.Count == 0)
            {
                current = -1;
                layerSize = layer.Image.Size;
                final = new Bitmap(layerSize.Width, layerSize.Height);
                gFinal = Graphics.FromImage(final);
                back = new Bitmap(layerSize.Width, layerSize.Height);
                gBack = Graphics.FromImage(back);
                front = new Bitmap(layerSize.Width, layerSize.Height);
                gFront = Graphics.FromImage(front);

            }

            LayerRow row = new LayerRow(layer.Visible);
            row.Layer = layer;
            row.Text = layer.Name;
            current++;
            layers.Insert(current, row);
            panel.Controls.Add(row);
            Allocation();
        }

        public void RestoreRow()
        {
            LayerRow l = layers[current];
            layers.Insert(deletedRows.Peek().Key, deletedRows.Peek().Value);
            panel.Controls.Add(deletedRows.Peek().Value);
            current = layers.IndexOf(l);
            deletedRows.Pop();
            Allocation();
        }

        public void RemoveLayerRow()
        {
            deletedRows.Push(new KeyValuePair<int, LayerRow>(current, layers[current]));
            panel.Controls.Remove(layers[current]);
            layers.Remove(layers[current]);
            if (current != 0)
                current--;
            Allocation();
        }

        public void DeleteRowAt(LayerRow lr)
        {
            int index = layers.IndexOf(lr);
            layers[index].Dispose();
            panel.Controls.Remove(layers[index]);
            layers.Remove(layers[index]);
            if (index <= current && current > 0)
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

        public void MoveBack(LayerRow lr,bool up)
        {
            int index = layers.IndexOf(lr);
            if (up) SwapRow(layers, index, index + 1);
            else SwapRow(layers, index, index - 1);
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
            layers[current - 1].Layer.Stacking();
            using (Graphics g = Graphics.FromImage(layers[current -1].Layer.Image))
            {
                g.DrawImage(layers[current].Layer.Image, 0, 0, layerSize.Width, layerSize.Height);
            }
            RemoveLayerRow();
        }

        public void Duplicate()
        {
            LayerRow newRow = new LayerRow();
            newRow.Text = layers[current].Text + "(Copy)";
            newRow.Layer = new Layer(layers[current].Layer.Image, newRow.Text, true);
            current++;
            layers.Insert(current, newRow);
            panel.Controls.Add(newRow);
            Allocation();
        }

    }
}
