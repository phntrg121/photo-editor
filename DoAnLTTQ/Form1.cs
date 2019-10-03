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
    enum Tool
    {
        pen, eraser, picker
    }

    public partial class Form1 : Form
    {
        Bitmap finalBmp;
        Image bgImage;
        Size bmpSize;
        LayerRowManager layers;
        string filename;
        Tool currentTool;
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
            layerContainer.Height = layerPanel.Height - panel5.Height - layerToolStrip.Height - 3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bgImage = Properties.Resources.TransparencyBG;
            pen = new Tools.PenTools();
            picker = new Tools.Picker();

            currentTool = Tool.pen;
        }

        #region MenuStip

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap newBmp = new Bitmap(ofd.FileName))
                    {
                        filename = ofd.FileName;
                        mPicBox.Visible = true;
                        bmpSize = newBmp.Size;
                        layers = new LayerRowManager();
                        Layer firstLayer = new Layer(newBmp, "Layer1", true);
                        newLStripButton.Enabled = true;
                        clearLStripButton.Enabled = true;
                        renameLStripButton.Enabled = true;
                        finalBmp = newBmp;
                        layers.AddLayerRow(ref layerContainer, ref firstLayer);
                        BackGroundGenerator();
                        MPicBoxUpdate();
                    }
                }
            }

        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewFileForm nff = new NewFileForm())
            {
                if (nff.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap newBmp = new Bitmap(nff.ImageSize.Width, nff.ImageSize.Height))
                    {
                        using (Graphics g = Graphics.FromImage(newBmp))
                        {
                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                            {
                                g.FillRectangle(brush, 0, 0, newBmp.Width, newBmp.Height);
                            }
                            filename = nff.FileName;
                            mPicBox.Visible = true;
                            bmpSize = newBmp.Size;
                            layers = new LayerRowManager();
                            Layer firstLayer = new Layer(newBmp, "Layer1", true);
                            newLStripButton.Enabled = true;
                            clearLStripButton.Enabled = true;
                            renameLStripButton.Enabled = true;
                            finalBmp = newBmp;
                            layers.AddLayerRow(ref layerContainer, ref firstLayer);
                            BackGroundGenerator();
                            MPicBoxUpdate();
                        }
                    }
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
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
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewLStripButton_Click(sender, e);
        }

        private void DeleteLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteLStripButton_Click(sender, e);
        }
        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameLStripButton_Click(sender, e);
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearLStripButton_Click(sender, e);
        }

        #endregion

        #region ToolStrip

        private void MToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            UncheckAll();
            if (e.ClickedItem.GetType() == typeof(ToolStripButton))
            {
                ToolStripButton button = (ToolStripButton)e.ClickedItem;
                button.Checked = true;
                button.CheckState = CheckState.Checked;
                if (button.Text == penStripButton.Text)
                {
                    currentTool = Tool.pen;
                }
                else if (button.Text == eraserStripButton.Text)
                {
                    currentTool = Tool.eraser;
                }
                else if (button.Text == pickerStripButton.Text)
                {
                    currentTool = Tool.picker;
                }
            }
        }

        void UncheckAll()
        {
            foreach (ToolStripButton button in mToolStrip.Items)
            {
                button.Checked = false;
                button.CheckState = CheckState.Unchecked;
            }
        }


        #endregion

        #region mPicBox

        void MPicBoxUpdate()
        {
            finalBmp.Dispose();
            finalBmp = layers.FinalImageUpdate();
            mPicBox.Image = finalBmp;
        }

        void BackGroundGenerator()
        {
            int m = (int)Math.Ceiling((double)bmpSize.Width / 512);
            int n = (int)Math.Ceiling((double)bmpSize.Height / 512);
            using (Bitmap bg = new Bitmap(512 * m, 512 * n))
            {
                using (Graphics g = Graphics.FromImage(bg))
                {
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            g.DrawImage(bgImage, i * 512, j * 512);
                        }
                    }

                    mPicBox.BackgroundImage = new Bitmap(bg);
                }
            }
        }

        private void MPicBox_MouseDown(object sender, MouseEventArgs e)
        {
            switch (currentTool)
            {
                case Tool.pen:
                    {
                        pen.GetLocation(ref e);
                    }
                    break;
                case Tool.picker:
                    {
                        mainColorPic.BackColor = picker.GetColor(ref finalBmp, ref e);
                    }
                    break;
                default:
                    break;
            }
        }

        private void MPicBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (currentTool)
                {
                    case Tool.pen:
                        {
                            pen.Draw(ref layers.Current.Image, ref e);
                            MPicBoxUpdate();
                        }
                        break;
                    default:
                        break;
                }
            }
            mouseLocation.Text = e.Location.ToString();
        }
        private void MPicBox_MouseLeave(object sender, EventArgs e)
        {
            mouseLocation.Text = "";
        }

        private void MPicBox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #region TopPanel
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Size = (float)numericUpDown1.Value;
        }

        #endregion

        #region ColorTabPage

        bool colorIsPicking = false;
        private void ColorGradian_MouseDown(object sender, MouseEventArgs e)
        {
            colorIsPicking = true;
        }

        private void ColorGradian_MouseMove(object sender, MouseEventArgs e)
        {
            if (colorIsPicking)
            {
                using (Bitmap bmp = new Bitmap(colorGradian.Image))
                {
                    if (e.X > 0 && e.Y > 0 && e.X < colorGradian.Width && e.Y < colorGradian.Height)
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

        int redVal = 0;
        int blueVal = 0;
        int greenVal = 0;

        private void MainColorPic_BackColorChanged(object sender, EventArgs e)
        {
            redVal = mainColorPic.BackColor.R;
            greenVal = mainColorPic.BackColor.G;
            blueVal = mainColorPic.BackColor.B;

            BarUpdate(ref redBar, Color.Red, redVal);
            BarUpdate(ref greenBar, Color.Green, greenVal);
            BarUpdate(ref blueBar, Color.Blue, blueVal);
            label7.Text = redVal.ToString();
            label8.Text = greenVal.ToString();
            label9.Text = blueVal.ToString();

            pen.Color = mainColorPic.BackColor;
        }
        void BarUpdate(ref PictureBox bar, Color c, int val)
        {
            using (SolidBrush b = new SolidBrush(c))
            {
                using (Graphics g = bar.CreateGraphics())
                {
                    g.Clear(bar.BackColor);
                    g.FillRectangle(b, new Rectangle(0, 0, val/2 , bar.Height));
                }
            }
        }
        
        void ValCheck(ref int n)
        {
            if (n > 255) n = 255;
            if (n < 0) n = 0;
        }

        private void RedBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                redVal = (int)(((double)e.Location.X / redBar.Width) * 255);
                ValCheck(ref redVal);
                mainColorPic.BackColor = Color.FromArgb(redVal, greenVal, blueVal);
            }
        }

        private void GreenBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                greenVal = (int)(((double)e.Location.X / greenBar.Width) * 255);
                ValCheck(ref greenVal);
                mainColorPic.BackColor = Color.FromArgb(redVal, greenVal, blueVal);
            }
        }

        private void BlueBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blueVal = (int)(((double)e.Location.X / blueBar.Width) * 255);
                ValCheck(ref blueVal);
                mainColorPic.BackColor = Color.FromArgb(redVal, greenVal, blueVal);
            }
        }

        #endregion

        #region Layer

        private void NewLStripButton_Click(object sender, EventArgs e)
        {
            using (Forms.NewLayer nlf = new Forms.NewLayer())
            {
                nlf.SetDefaultName(layers.Count);
                if (nlf.ShowDialog() == DialogResult.OK)
                {
                    string name = nlf.LayerName;
                    bool visible = nlf.IsVisible;
                    using (Bitmap newBmp = new Bitmap(bmpSize.Width, bmpSize.Height))
                    {
                        newBmp.MakeTransparent();
                        Layer layer = new Layer(newBmp, name, visible);
                        layers.AddLayerRow(ref layerContainer, ref layer);
                        deleteLStripButton.Enabled = true;
                    }
                }
            }
        }

        private void DeleteLStripButton_Click(object sender, EventArgs e)
        {
            layers.RemoveLayerRow(ref layerContainer);
            if (layers.Count == 1) deleteLStripButton.Enabled = false;
            MPicBoxUpdate();
        }
        private void RenameLStripButton_Click(object sender, EventArgs e)
        {
            using (Forms.LayerRename lr = new Forms.LayerRename())
            {
                lr.DefaultName = layers.Current.Name;
                if (lr.ShowDialog() == DialogResult.OK)
                {
                    layers.Current.Name = lr.NewName;
                    layers.UpdateName();
                }
            }
        }

        private void ClearLStripButton_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(layers.Current.Image))
            {
                g.Clear(Color.Transparent);
            }
            MPicBoxUpdate();
        }

        #endregion

        
    }
}
