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
        Layer layer;
        public LayerRow()
        {
            InitializeComponent();
        }

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

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            layer.ChangeVisible();
            pictureBox1.Image.Dispose();
            if (layer.Visible)
                pictureBox1.Image = Properties.Resources.visible;
            else
                pictureBox1.Image = Properties.Resources.not_visible;
        }
    }
}
