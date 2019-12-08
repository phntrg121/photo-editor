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

namespace DoAnLTTQ
{
    public partial class LayerContainer : UserControl
    {
        private List<LayerRow> layers;
        private int current;
        private Size layerSize;
        private Stack<KeyValuePair<int, LayerRow>> deletedRows;
        
        public LayerContainer()
        {
            InitializeComponent();
            layers = new List<LayerRow>();
            deletedRows = new Stack<KeyValuePair<int, LayerRow>>();
        }

        public Matrix ScaleMatrix { get; set; }

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

        public Tools.Tools Tool { get; set; }

        public void ProcessUpdate(Bitmap processing, bool preview = false, bool filter = false)
        {
            if (processing != null)
            {
                if (!preview) layers[current].Layer.Stacking();
                using (Graphics g = Graphics.FromImage(layers[current].Layer.Image))
                {
                    if (preview || filter || Tool.Tool == DoAnLTTQ.Tool.Eraser)
                        g.CompositingMode = CompositingMode.SourceCopy;
                    else g.CompositingMode = CompositingMode.SourceOver;

                    if (!Tool.Select.Selected)
                    {
                        g.DrawImageUnscaled(processing, 0, 0);
                    }
                    else
                    {
                        g.DrawImageUnscaled(processing, Tool.Select.SelectRect.X, Tool.Select.SelectRect.Y,
                            Tool.Select.SelectRect.Width, Tool.Select.SelectRect.Height);
                    }
                }
            }
        }

        public void FinalUpdate(Graphics g, Bitmap source)
        {
            g.Clear(Color.Transparent);
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].Layer.Visible)
                {
                    if (i == 0 || layers[i].Blend == Blend.Normal)
                    {
                        g.DrawImageUnscaled(layers[i].Layer.ImageWithOpacity, 0, 0);
                    }
                    else if (layers[i].Blend == Blend.Multiply)
                    {
                        using (Bitmap bmp = BlendMode.Multiply(layers[i].Layer.ImageWithOpacity, source))
                            g.DrawImageUnscaled(bmp, 0, 0);
                    }
                    else if (layers[i].Blend == Blend.Screen)
                    {
                        using (Bitmap bmp = BlendMode.Screen(layers[i].Layer.ImageWithOpacity, source))
                            g.DrawImageUnscaled(bmp, 0, 0);
                    }
                    else if (layers[i].Blend == Blend.Darken)
                    {
                        using (Bitmap bmp = BlendMode.Darken(layers[i].Layer.ImageWithOpacity, source))
                            g.DrawImageUnscaled(bmp, 0, 0);
                    }
                    else if (layers[i].Blend == Blend.Lighten)
                    {
                        using (Bitmap bmp = BlendMode.Lighten(layers[i].Layer.ImageWithOpacity, source))
                            g.DrawImageUnscaled(bmp, 0, 0);
                    }
                    else if (layers[i].Blend == Blend.Overlay)
                    {
                        using (Bitmap bmp = BlendMode.Overlay(layers[i].Layer.ImageWithOpacity, source))
                            g.DrawImageUnscaled(bmp, 0, 0);
                    }
                }
            }
        }

        public void AddLayerRow(ref Layer layer)
        {
            if (layers.Count == 0)
            {
                current = -1;
                layerSize = layer.Image.Size;
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
             layers[current].Layer.Name = layers[current].Text;
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
                g.DrawImage(layers[current].Layer.ImageWithOpacity, 0, 0, layerSize.Width, layerSize.Height);
            }
            RemoveLayerRow();
        }

        public void Duplicate()
        {
            LayerRow newRow = new LayerRow();
            newRow.Text = layers[current].Text + "(Copy)";

            Bitmap bmp;
            if (!Tool.Select.Selected) bmp = (Bitmap)layers[current].Layer.Image.Clone();
            else
            {
                bmp = new Bitmap(layerSize.Width, layerSize.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                    g.DrawImage(layers[current].Layer.Image, Tool.Select.SelectRect.X, Tool.Select.SelectRect.Y,
                            Tool.Select.SelectRect, GraphicsUnit.Pixel);
            }
            newRow.Layer = new Layer(bmp, newRow.Text, true);
            bmp.Dispose();

            current++;
            layers.Insert(current, newRow);
            panel.Controls.Add(newRow);
            Allocation();
        }
    }
}
