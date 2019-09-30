using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnLTTQ
{
    public partial class NewFileForm : Form
    {
        public NewFileForm()
        {
            InitializeComponent();
        }
        public Size ImageSize
        {
            get
            {
                return new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            }
        }

        public string FileName
        {
            get
            {
                return textBox1.Text;
            }
        }

    }
}
