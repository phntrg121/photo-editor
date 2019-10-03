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

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    numericUpDown1.Value = numericUpDown2.Value = 512;
                    break;
                case 2:
                    numericUpDown1.Value = 320;
                    numericUpDown2.Value = 240;
                    break;
                case 3:
                    numericUpDown1.Value = 640;
                    numericUpDown2.Value = 480;
                    break;
                case 4:
                    numericUpDown1.Value = 800;
                    numericUpDown2.Value = 600;
                    break;
                case 5:
                    numericUpDown1.Value = 1024;
                    numericUpDown2.Value = 768;
                    break;
                case 6:
                    numericUpDown1.Value = 1280;
                    numericUpDown2.Value = 1024;
                    break;
                case 7:
                    numericUpDown1.Value = 1600;
                    numericUpDown2.Value = 1200;
                    break;
                default:
                    comboBox1.SelectedIndex = 0;
                    break;
            }
        }
    }
}
