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
    public partial class LayerRow : UserControl
    {
        bool visible = true;
        public LayerRow()
        {
            InitializeComponent();
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

        public bool layerVisible
        {
            set
            {
                visible = value;
            }
            get
            {
                return visible;
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
