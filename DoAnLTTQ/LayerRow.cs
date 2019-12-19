using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public partial class LayerRow : UserControl
    {
        private Layer layer;
        Blend blend;
        Stack<Blend> blendlist;
        public LayerRow(bool visible = true)
        {
            InitializeComponent();

            blend = Blend.Normal;
            blendlist = new Stack<Blend>();

            if (visible)
                pictureBox1.Image = Properties.Resources.visible;
            else
                pictureBox1.Image = Properties.Resources.not_visible;
        }

        public Blend Blend
        {
            get
            {
                return blend;
            }
            set
            {
                blend = value;
                blendlist.Push(blend);
                label3.Text = value.ToString("G");
            }
        }

        public void RestoreBlend()
        {
            blendlist.Pop();

            if (blendlist.Count != 0)
                blend = blendlist.Peek();
            else blend = Blend.Normal;

            label3.Text = Blend.ToString("G");
        }
        public int BlendCount => blendlist.Count();

        public Layer Layer
        {
            set
            {
                layer = value;
            }
            get
            {
                return layer;
            }
        }

        public override string Text
        {
            set
            {
                label1.Text = value;
            }
            get
            {
                return label1.Text;
            }
        }

        public float Opacity
        {
            set
            {
                label2.Text = ((int)value).ToString() + '%';
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            layer.ChangeVisible();
            pictureBox1.Image.Dispose();
            if (layer.Visible)
                pictureBox1.Image = Properties.Resources.visible;
            else
                pictureBox1.Image = Properties.Resources.not_visible;

            Form1 form = (Form1)Parent.Parent.Parent.Parent.Parent;
            form.DSUpdate();
        }

        private void LayerRow_Click(object sender, EventArgs e)
        {
            LayerContainer layCon = (LayerContainer)Parent.Parent;
            layCon.CurrentRow = this;

            Form1 form = (Form1)Parent.Parent.Parent.Parent.Parent;
            form.LayerButtonCheck();
            form.opacityVal = layer.Opacity;
            form.OpacityBarUpdate();
            form.BlendModeBoxUpdate(Blend);
        }
    }
}
