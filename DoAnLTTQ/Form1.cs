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
        private Bitmap finalBmp;
        private Bitmap processing;
        private Image bgImage;
        private Size bmpSize;
        private DoubleBufferPictureBox mPicBox;
        private DoubleBufferPictureBox subPB;
        private LayerContainer layerContainer;
        private string filename;
        private Tool currentTool;
        private Tools.PenTools pen;
        private Tools.Picker picker;

        #region Form

        public Form1()
        {
            InitializeComponent();
            pen = new Tools.PenTools();
            picker = new Tools.Picker();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            workPanel.Width = this.Width - rightPanel.Width - leftPanel.Width - 16;
            workPanel.Height = this.Height - topPanel.Height - statusStrip1.Height - menuStrip.Height - 39;
            layerPanel.Height = statusStrip1.Location.Y - layerPanel.Location.Y - 34;
            if (layerContainer != null)
                layerContainer.Height = layerPanel.Height - panel5.Height - layerToolStrip.Height - 3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bgImage = Properties.Resources.TransparencyBG;
            LayerMenuStripEnable(false);
            FilterMenuStripEnable(false);
            layerPanel.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            currentTool = Tool.pen;
        }

        #endregion

        #region Key shortcut

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.N):
                    NewToolStripMenuItem_Click(newToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.O):
                    OpenToolStripMenuItem_Click(openToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.S):
                    SaveToolStripMenuItem_Click(saveToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.W):
                    CloseToolStripMenuItem_Click(closeToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.Shift | Keys.N):
                    NewLayerToolStripMenuItem_Click(newLayerToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.Shift | Keys.D):
                    DeleteLayerToolStripMenuItem_Click(deleteLayerToolStripMenuItem, null);
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion

        #region MenuStip

        #region File menu

        private bool working = false;
        private bool saved = true;
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseToolStripMenuItem_Click(sender, e);

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    finalBmp = new Bitmap(ofd.FileName);
                    filename = ofd.FileName;
                    bmpSize = finalBmp.Size;
                    MPicBoxInit();
                    LayerContainerInit();
                    LayerMenuStripEnable(true);
                    FilterMenuStripEnable(true);
                    Layer firstLayer = new Layer(finalBmp, "Layer1", true);
                    layerContainer.AddLayerRow(ref firstLayer);
                    BackGroundGenerator();
                    MPicBoxUpdate();
                    working = true;
                    closeToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseToolStripMenuItem_Click(sender, e);

            using (NewFileForm nff = new NewFileForm())
            {
                if (nff.ShowDialog() == DialogResult.OK)
                {
                    finalBmp = new Bitmap(nff.ImageSize.Width, nff.ImageSize.Height);
                    filename = nff.FileName;
                    bmpSize = finalBmp.Size;
                    MPicBoxInit();
                    LayerContainerInit();
                    LayerMenuStripEnable(true);
                    FilterMenuStripEnable(true);
                    Layer firstLayer = new Layer(finalBmp, "Layer1", true);
                    layerContainer.AddLayerRow(ref firstLayer);
                    if (nff.BGColor == Color.Transparent)
                        BackGroundGenerator();
                    else
                        mPicBox.BackColor = nff.BGColor;
                    MPicBoxUpdate();
                    working = true;
                    closeToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(working)
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
                saved = true;
                saveToolStripMenuItem.Enabled = false;
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (working)
            {
                if(!saved)
                {
                    DialogResult dialog = MessageBox.Show(filename + " haven't saved yet.\nDo you want to save it", "Photo Editor",
                                                           MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                        SaveToolStripMenuItem_Click(sender, e);
                }

                finalBmp.Dispose();
                mPicBox.Dispose();
                mPicBox = null;
                workPanel.Controls.Remove(mPicBox);
                layerContainer.Dispose();
                layerContainer = null;
                layerPanel.Controls.Remove(layerContainer);
                LayerMenuStripEnable(false);
                FilterMenuStripEnable(false);
                layerPanel.Enabled = false;
                working = false;
                closeToolStripMenuItem.Enabled = false;
                saved = true;
                saveToolStripMenuItem.Enabled = false;
            }
        }       

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Layer menu

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
        private void DuplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuplicateLStripButton_Click(sender, e);
        }

        private void FillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(layerContainer.Current.Layer.Image))
            using (SolidBrush brush = new SolidBrush(mainColorPic.BackColor))
            {
                g.FillRectangle(brush, 0, 0, layerContainer.Current.Layer.Image.Width, layerContainer.Current.Layer.Image.Height);
            }
            MPicBoxUpdate();
        }

        #endregion

        #region Filter menu

        private void FilterMenuStripEnable(bool enable)
        {
            foreach(ToolStripMenuItem item in filterToolStripMenuItem.DropDownItems)
            {
                item.Enabled = enable;
            }
        }
        private void BrightnessAndContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.BrightnessContrast bc = new Forms.BrightnessContrast())
            {
                bc.Image = layerContainer.Current.Layer.Image;
                if (bc.ShowDialog() == DialogResult.OK)
                {
                    layerContainer.Current.Layer.Image.Dispose();
                    layerContainer.Current.Layer.Image = bc.Image;
                }
                MPicBoxUpdate();
            }
        }

        private void HueAndSaturationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.HueSaturation hs = new Forms.HueSaturation())
            {
                hs.Image = layerContainer.Current.Layer.Image;
                hs.Initialize();

                if (hs.ShowDialog() == DialogResult.OK)
                {
                    layerContainer.Current.Layer.Image.Dispose();
                    layerContainer.Current.Layer.Image = hs.Image;
                }
                MPicBoxUpdate();
            }
        }

        private void InvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(Bitmap bmp = new Bitmap(layerContainer.Current.Layer.Image))
            using(Graphics g = Graphics.FromImage(bmp))
            {
                System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();
                matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = -1f;
                matrix.Matrix33 = matrix.Matrix40 = matrix.Matrix41 = matrix.Matrix42 = matrix.Matrix44 = 1f;
                using (System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes())
                {
                    attributes.SetColorMatrix(matrix);
                    g.DrawImage(layerContainer.Current.Layer.Image, new Rectangle(0, 0, bmpSize.Width, bmpSize.Height),
                        0, 0, bmpSize.Width, bmpSize.Height, GraphicsUnit.Pixel, attributes);

                    layerContainer.Current.Layer.Image.Dispose();
                    layerContainer.Current.Layer.Image = new Bitmap(bmp);
                }
            }
            MPicBoxUpdate();
        }

        private void ThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.Threshold th = new Forms.Threshold())
            {
                th.Image = layerContainer.Current.Layer.Image;
                th.Initialize();

                if (th.ShowDialog() == DialogResult.OK)
                {
                    layerContainer.Current.Layer.Image.Dispose();
                    layerContainer.Current.Layer.Image = th.Image;
                }
                MPicBoxUpdate();
            }
        }

        #endregion

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

        private void UncheckAll()
        {
            foreach (ToolStripButton button in mToolStrip.Items)
            {
                button.Checked = false;
                button.CheckState = CheckState.Unchecked;
            }
        }


        #endregion

        #region mPicBox

        private void MPicBoxInit()
        {
            if (mPicBox == null)
            {
                mPicBox = new DoubleBufferPictureBox();
                mPicBox.Location = new System.Drawing.Point(0, 0);
                mPicBox.Name = "mPicBox";
                mPicBox.Size = bmpSize;
                mPicBox.Image = finalBmp;
                mPicBox.MouseDown += MPicBox_MouseDown;
                mPicBox.MouseLeave += MPicBox_MouseLeave;
                mPicBox.MouseMove += MPicBox_MouseMove;
                mPicBox.MouseUp += MPicBox_MouseUp;
                workPanel.Controls.Add(mPicBox);
            }
        }

        public void MPicBoxUpdate()
        {
            saved = false;
            saveToolStripMenuItem.Enabled = true;
            layerContainer.FinalImageUpdate(processing);
            if(processing !=null)
            {
                processing.Dispose();
                processing = null;
            }
            finalBmp = layerContainer.Final;
            mPicBox.Image = finalBmp;
        }

        private void BackGroundGenerator()
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
            processing = new Bitmap(bmpSize.Width, bmpSize.Height);
            subPB = new DoubleBufferPictureBox();
            subPB.Location = new Point(0, 0);
            subPB.Size = mPicBox.Size;
            subPB.BackColor = Color.Transparent;
            mPicBox.Controls.Add(subPB);

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
                            if (processing != null)
                            {
                                pen.Draw(ref processing, ref e);
                                subPB.Image = processing;
                                subPB.Invalidate();
                            }
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
            mPicBox.Controls.Remove(subPB);
            MPicBoxUpdate();
            if(subPB !=null)
            {
                subPB.Dispose();
                subPB = null;
            }
            
        }

        #endregion

        #region TopPanel
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Size = (float)numericUpDown1.Value;
        }

        #endregion

        #region ColorTabPage

        private bool colorIsPicking = false;
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

        private void ColorSwitch_Click(object sender, EventArgs e)
        {
            Color tmp = mainColorPic.BackColor;
            mainColorPic.BackColor = subColorPic.BackColor;
            subColorPic.BackColor = tmp;
        }

        private int redVal = 0;
        private int blueVal = 0;
        private int greenVal = 0;

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
        private void BarUpdate(ref PictureBox bar, Color c, int val)
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
        
        private void ValCheck(ref int n)
        {
            if (n > 255) n = 255;
            if (n < 0) n = 0;
        }

        private void BarVal(ref int val, ref PictureBox bar ,ref  MouseEventArgs e)
        {
            val = (int)(((double)e.Location.X / bar.Width) * 255);
            ValCheck(ref val);
            mainColorPic.BackColor = Color.FromArgb(redVal, greenVal, blueVal);
        }

        private void RedBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BarVal(ref redVal, ref redBar, ref e);
            }
        }

        private void GreenBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BarVal(ref greenVal, ref greenBar, ref e);
            }
        }

        private void BlueBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BarVal(ref blueVal, ref blueBar, ref e);
            }
        }

        #endregion

        #region Layer

        private void LayerContainerInit()
        {
            if (layerContainer == null)
            {
                layerContainer = new LayerContainer();
                layerContainer.AutoScroll = true;
                layerContainer.Location = new System.Drawing.Point(4, 55);
                layerContainer.Name = "layerContainer";
                layerContainer.Size = new System.Drawing.Size(201, 230);
                layerPanel.Controls.Add(layerContainer);
                layerPanel.Enabled = true;
                opacityVal = 100f;
                OpacityBarUpdate();
            }
        }

        private void LayerMenuStripEnable(bool enable)
        {
            foreach(ToolStripMenuItem item in layerToolStripMenuItem.DropDownItems)
            {
                item.Enabled = enable;
            }
        }

        private void NewLStripButton_Click(object sender, EventArgs e)
        {
            using (Forms.NewLayer nlf = new Forms.NewLayer())
            {
                nlf.SetDefaultName(layerContainer.Count);
                if (nlf.ShowDialog() == DialogResult.OK)
                {
                    string name = nlf.LayerName;
                    bool visible = nlf.IsVisible;
                    using (Bitmap newBmp = new Bitmap(bmpSize.Width, bmpSize.Height))
                    {
                        newBmp.MakeTransparent();
                        Layer layer = new Layer(newBmp, name, visible);
                        layerContainer.AddLayerRow(ref layer);
                        LayerButtonCheck();
                    }
                }
            }
        }

        private void DeleteLStripButton_Click(object sender, EventArgs e)
        {
            layerContainer.RemoveLayerRow();
            LayerButtonCheck();
            MPicBoxUpdate();
        }
        private void RenameLStripButton_Click(object sender, EventArgs e)
        {
            using (Forms.LayerRename lr = new Forms.LayerRename())
            {
                lr.DefaultName = layerContainer.Current.Name;
                if (lr.ShowDialog() == DialogResult.OK)
                {
                    layerContainer.Current.Name = lr.NewName;
                    layerContainer.UpdateName();
                }
            }
        }

        private void ClearLStripButton_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(layerContainer.Current.Layer.Image))
            {
                g.Clear(Color.Transparent);
            }
            MPicBoxUpdate();
        }

        private void DownLStripButton_Click(object sender, EventArgs e)
        {
            layerContainer.MoveDown();
            LayerButtonCheck();
            MPicBoxUpdate();
        }

        private void UpLStripButton_Click(object sender, EventArgs e)
        {
            layerContainer.MoveUp();
            LayerButtonCheck();
            MPicBoxUpdate();
        }

        public void LayerButtonCheck()
        {
            if (layerContainer.CurrentIndex == layerContainer.Count - 1)
            {

                mergeLStripButton.Enabled = true;
                downLStripButton.Enabled = true;
                upLStripButton.Enabled = false;
            }
            else if (layerContainer.CurrentIndex == 0)
            {
                downLStripButton.Enabled = false;
                mergeLStripButton.Enabled = false;
                upLStripButton.Enabled = true;
            }
            else
            {
                mergeLStripButton.Enabled = true;
                downLStripButton.Enabled = true;
                upLStripButton.Enabled = true;
            }

            if (layerContainer.Count > 1)
            {
                deleteLStripButton.Enabled = true;
                duplicateLStripButton.Enabled = true;
            }
            else
            {
                deleteLStripButton.Enabled = false;
                upLStripButton.Enabled = false;
                downLStripButton.Enabled = false;
                mergeLStripButton.Enabled = false;
            }

        }

        private void MergeLStripButton_Click(object sender, EventArgs e)
        {
            layerContainer.Merge();
            LayerButtonCheck();
            MPicBoxUpdate();
        }

        private void DuplicateLStripButton_Click(object sender, EventArgs e)
        {
            layerContainer.Duplicate();
            LayerButtonCheck();
            MPicBoxUpdate();
        }

        public float opacityVal;
        private void OpacityBar_MouseMoveOrDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                opacityVal = (float)e.Location.X / opacityBar.Width * 100;
                if (opacityVal > 100) opacityVal = 100;
                if (opacityVal < 0) opacityVal = 0;
                OpacityBarUpdate();
            }
        }

        public void OpacityBarUpdate()
        {
            using (SolidBrush b = new SolidBrush(Color.Gray))
            {
                using (Graphics g = opacityBar.CreateGraphics())
                {

                    label10.Text = ((int)opacityVal).ToString();
                    int w = (int)Math.Ceiling(((float)opacityVal / 100) * opacityBar.Width);
                    g.Clear(opacityBar.BackColor);
                    g.FillRectangle(b, new Rectangle(0, 0, w, opacityBar.Height));
                }
            }
        }

        private void OpacityBar_MouseUp(object sender, MouseEventArgs e)
        {
            layerContainer.Current.Opacity = opacityVal;
            MPicBoxUpdate();
        }

        #endregion

    }
}