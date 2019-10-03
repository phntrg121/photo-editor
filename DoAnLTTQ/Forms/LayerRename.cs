using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ.Forms
{
    public partial class LayerRename : Form
    {
        public LayerRename()
        {
            InitializeComponent();
        }

        public string DefaultName
        {
            set
            {
                textBox1.Text = value;
            }
        }
        
        public string NewName
        {
            get
            {
                return textBox1.Text;
            }
        }

    }
}
