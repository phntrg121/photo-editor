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
        pen, eraser, picker
    }

    public partial class Form1 : Form
    {
        Bitmap finalBmp;
        Size bmpSize;
        LayersManagement layers;
        string filename;
        tool currentTool;
        Tools.PenTools pen;
        Tools.Picker picker;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            workPanel.Width = this.Width - rightPanel.Width - leftPanel.Width - 16;
            workPanel.Height = this.Height - topPanel.Height - statusStrip1.Height - menuStrip.Height - 39;
            layerPanel.Height = statusStrip1.Location.Y - layerPanel.Location.Y - 34;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pen = new Tools.PenTools();
            picker = new Tools.Picker();

            currentTool = tool.pen;
        }

        #region MenuStip

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap newBmp = new Bitmap(ofd.FileName);
                    filename = ofd.FileName;
                    mPicBox.Visible = true;
                    bmpSize = newBmp.Size;
                    layers = new LayersManagement(newBmp.Size);
                    layers.Add(ref newBmp, "Layer1", true);
                    newLStripButton.Enabled = true;
                    finalBmp = newBmp;
                    MPicBoxUpdate();
                }
            }
                
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewFileForm nff = new NewFileForm())
            {
                if (nff.ShowDialog() == DialogResult.OK)
                {
                    Bitmap newBmp = new Bitmap(nff.ImageSize.Width, nff.ImageSize.Height);
                    using (Graphics g = Graphics.FromImage(newBmp))
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                    {
                        g.FillRectangle(brush, 0, 0, newBmp.Width, newBmp.Height);
                    }
                    filename = nff.FileName;
                    mPicBox.Visible = true;
                    bmpSize = newBmp.Size;
                    layers = new LayersManagement(newBmp.Size);
                    layers.Add(ref newBmp, "Layer1", true);
                    newLStripButton.Enabled = true;
                    finalBmp = newBmp;
                    MPicBoxUpdate();
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

        #endregion

        #region ToolStrip

        private void MToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            UncheckAll();
            if(e.ClickedItem.GetType() == typeof(ToolStripButton))
            {
                ToolStripButton button = (ToolStripButton)e.ClickedItem;
                button.Checked = true;
                button.CheckState = CheckState.Checked;
                if(button.Text == penStripButton.Text)
                {
                    currentTool = tool.pen;
                }
                else if (button.Text == eraserStripButton.Text)
                {
                    currentTool = tool.eraser;
                }
                else if (button.Text == pickerStripButton.Text)
                {
                    currentTool = tool.picker;
                }
            } 
        }

        void UncheckAll()
        {
            foreach(ToolStripButton button in mToolStrip.Items)
            {
                button.Checked = false;
                button.CheckState = CheckState.Unchecked;
            }
        }


        #endregion

        #region mPicBox

        void MPicBoxUpdate()
        {
            finalBmp = layers.FinalImageUpdate();
            mPicBox.Image = finalBmp;
        }

        private void mPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            switch (currentTool)
            {
                case tool.pen:
                    {
                        pen.GetLocation(ref e);
                    }
                    break;
                case tool.picker:
                    {
                        mainColorPic.BackColor = picker.GetColor(ref finalBmp, ref e);
                    }
                    break;
                default:
                    break;
            }
        }


        private void mPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (currentTool)
                {
                    case tool.pen:
                        {
                            pen.Draw(ref layers.Current, ref e);
                            MPicBoxUpdate();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void mPicBox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        #endregion

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Size = (float)numericUpDown1.Value;
        }

        #region ColorTabPage

        bool colorIsPicking = false;
        private void ColorGradian_MouseDown(object sender, MouseEventArgs e)
        {
            colorIsPicking = true;
        }

        private void ColorGradian_MouseMove(object sender, MouseEventArgs e)
        {
            if(colorIsPicking)
            {
                using (Bitmap bmp = new Bitmap(colorGradian.Image))
                {
                    if(e.X >0 && e.Y > 0 && e.X < colorGradian.Width && e.Y < colorGradian.Height)
                    {
                        Color c = bmp.GetPixel(e.X, e.Y);
                        mainColorPic.BackColor = c;
                    }
                    
                }
            }
        }

        private void ColorGradian_MouseUp(object sender, MouseEventArgs e)
        {
            colorIsPicking = false;
        }

        private void ColorGradian_MouseClick(object sender, MouseEventArgs e)
        {
            using (Bitmap bmp = new Bitmap(colorGradian.Image))
            {
                Color c = bmp.GetPixel(e.X, e.Y);
                mainColorPic.BackColor = c;
            }
        }

        private void MainColorPic_BackColorChanged(object sender, EventArgs e)
        {
            redTrackBar.Value = mainColorPic.BackColor.R;
            greenTrackBar.Value = mainColorPic.BackColor.G;
            blueTrackBar.Value = mainColorPic.BackColor.B;

            pen.Color = mainColorPic.BackColor;
        }

        private void RedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            Color c = mainColorPic.BackColor;
            mainColorPic.BackColor = Color.FromArgb(redTrackBar.Value, c.G, c.B);
            label7.Text = Convert.ToString(redTrackBar.Value);
        }

        private void GreenTrackBar_ValueChanged(object sender, EventArgs e)
        {
            Color c = mainColorPic.BackColor;
            mainColorPic.BackColor = Color.FromArgb(c.R, greenTrackBar.Value, c.B);
            label8.Text = Convert.ToString(greenTrackBar.Value);
        }

        private void BlueTrackBar_ValueChanged(object sender, EventArgs e)
        {
            Color c = mainColorPic.BackColor;
            mainColorPic.BackColor = Color.FromArgb(c.R, c.G, blueTrackBar.Value);
            label9.Text = Convert.ToString(blueTrackBar.Value);
        }

        private void AlphaTrackBar_ValueChanged(object sender, EventArgs e)
        {
            mainColorPic.BackColor = Color.FromArgb(alphaTrackBar.Value, mainColorPic.BackColor);
            label10.Text = Convert.ToString(alphaTrackBar.Value);
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            if (redTrackBar.Value != 0)
                redTrackBar.Value = 0;
            else redTrackBar.Value = 255;
        }

        private void Label4_Click(object sender, EventArgs e)
        {
            if (greenTrackBar.Value != 0)
                greenTrackBar.Value = 0;
            else greenTrackBar.Value = 255;
        }

        private void Label5_Click(object sender, EventArgs e)
        {
            if (blueTrackBar.Value != 0)
                blueTrackBar.Value = 0;
            else blueTrackBar.Value = 255;
        }

        private void Label6_Click(object sender, EventArgs e)
        {
            if (alphaTrackBar.Value != 0)
                alphaTrackBar.Value = 0;
            else alphaTrackBar.Value = 100;
        }


        #endregion

        #region Layer

        private void NewLStripButton_Click(object sender, EventArgs e)
        {
            using(Forms.NewLayerForm nlf = new Forms.NewLayerForm())
            {
                nlf.SetDefaultName(layers.LayerCount);
                if(nlf.ShowDialog()==DialogResult.OK)
                {
                    string name = nlf.LayerName;
                    bool visible = nlf.IsVisible;
                    Bitmap newBmp = new Bitmap(bmpSize.Width, bmpSize.Height);
                    newBmp.MakeTransparent();
                    layers.Add(ref newBmp, name, visible);
                    deleteLStripButton.Enabled = true;
                }
            }
        }

        private void DeleteLStripButton_Click(object sender, EventArgs e)
        {
            layers.Remove();
            if (layers.LayerCount == 1) deleteLStripButton.Enabled = false;
            MPicBoxUpdate();
        }


        #endregion
    }
}
