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
    enum tool
    {
        pen, eraser
    }

    public partial class Form1 : Form
    {
        Bitmap currentBmp;
        Bitmap finalBmp;
        string filename;
        tool currentTool;
        Tools.PenTools pen;
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            pen = new Tools.PenTools();
            currentTool = tool.pen;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentBmp = new Bitmap(ofd.FileName);
                    filename = ofd.FileName;
                    mPicBox.Visible = true;
                }
            }
                
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewFileForm nff = new NewFileForm())
            {
                if (nff.ShowDialog() == DialogResult.OK)
                {
                    currentBmp = new Bitmap(nff.ImageSize.Width, nff.ImageSize.Height);
                    using (Graphics g = Graphics.FromImage(currentBmp))
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                    {
                        g.FillRectangle(brush, 0, 0, currentBmp.Width, currentBmp.Height);
                    }
                    filename = nff.FileName;
                    mPicBox.Visible = true;
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = filename;
                sfd.Filter = "Bitmap Image (*.BMP)|*.bmp|JPEG Image (*.JPEG)|*.jpeg|PNG Image (*.PNG)|*.png";
                sfd.FilterIndex = 3;
                sfd.DefaultExt = "png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    finalBmp.Save(sfd.FileName);
                }
                
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            workPanel.Width = this.Width - rightPanel.Width - leftPanel.Width - 16;
            workPanel.Height = this.Height - topPanel.Height - statusStrip1.Height - menuStrip.Height - 39;
            layerPanel.Height = statusStrip1.Location.Y - layerPanel.Location.Y - 34;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            finalBmp = currentBmp;
            mPicBox.Image = finalBmp;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch(currentTool)
            {
                case tool.pen:
                    {
                        pen.GetLocation(ref e);
                    }
                    break;
                default:
                    break;
            }
        }


        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                switch (currentTool)
                {
                    case tool.pen:
                        {
                            pen.Draw(ref currentBmp, ref e);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Size = (float)numericUpDown1.Value;
        }
    }
}
