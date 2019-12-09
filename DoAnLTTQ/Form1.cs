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
    public partial class Form1 : Form
    {
        private WorkSpace Current;
        private Tools.Tools tools;

        #region Form

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LayerMenuStripEnable(false);
            ColorMenuStripEnable(false);
            FilterMenuStripEnable(false);
            ViewMenuStripEnable(false);
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            tools = new Tools.Tools();
            propertiesPanel.Controls.Add(tools.Current);
            hexCode.Text = ColorTranslator.ToHtml(mainColorPic.BackColor);
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            workSpaceTabControl.Width = this.Width - rightPanel.Width - leftPanel.Width - 16;
            workSpaceTabControl.Height = this.Height - bottomPanel.Height - statusStrip1.Height - menuStrip.Height - 39;
            layerPanel.Height = statusStrip1.Location.Y - layerPanel.Location.Y - 26;
            if (Current != null)
            {
                Current.LayerContainer.Height = layerPanel.Height - panel5.Height - layerToolStrip.Height - 7;
            }
            bottomPanel.Location = new Point(190, workSpaceTabControl.Location.Y + workSpaceTabControl.Height);
            bottomPanel.Width = this.Width - rightPanel.Width - leftPanel.Width - 16;
            toolPanel.Height = statusStrip1.Location.Y - toolPanel.Location.Y - 27;
            propertiesPanel.Height = statusStrip1.Location.Y - propertiesPanel.Location.Y - 226;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (Current!= null && !Current.Saved)
            {
                DialogResult dialog = MessageBox.Show("Your work haven't saved yet.\nDo you want to save it", "Photo Editor",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes)
                    SaveToolStripMenuItem_Click(this, e);
            }
        }
        private void NoKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
                case (Keys.Control | Keys.Z):
                    UndoToolStripMenuItem_Click(undoToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.Shift | Keys.N):
                    NewLayerToolStripMenuItem_Click(newLayerToolStripMenuItem, null);
                    return true;
                case (Keys.Control | Keys.Shift | Keys.D):
                    DeleteLayerToolStripMenuItem_Click(deleteLayerToolStripMenuItem, null);
                    return true;
                case (Keys.Up):
                    if (upLStripButton.Enabled)
                        UpLStripButton_Click(upLStripButton, null);
                    return true;
                case (Keys.Down):
                    if (downLStripButton.Enabled)
                        DownLStripButton_Click(downLStripButton, null);
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion

        #region MenuStip

        #region File menu

        private bool working = false;
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
                ofd.FilterIndex = 2;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(ofd.FileName);
                    AddWorkTab(bmp, Color.Transparent);
                    Current.FilePath = ofd.FileName;
                    Current.Parent.Text = Current.FileName;
                    DSUpdate();
                    Current.Saved = true;
                    Current.Stored = true;
                    working = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = true;
                    bmp.Dispose();
                }
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.NewFileForm nff = new Forms.NewFileForm())
            {
                nff.ColorFore = mainColorPic.BackColor; 
                nff.ColorBack = subColorPic.BackColor;

                if (nff.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(nff.ImageSize.Width, nff.ImageSize.Height);
                    AddWorkTab(bmp, nff.BGColor);
                    Current.FileName = nff.FileName;
                    Current.Parent.Text = Current.FileName;
                    DSUpdate();
                    Current.Saved = true;
                    working = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = true;
                    bmp.Dispose();
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(working)
            {
                if (Current.Stored)
                {
                    Current.DrawSpace.Final.Save(Current.FilePath);
                    Current.Saved = true;
                    saveToolStripMenuItem.Enabled = false;
                }
                else
                {
                    SaveAsToolStripMenuItem_Click(this, e);
                }
                Current.Parent.Text = Current.FileName;
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (working)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.FileName = Current.FileName;
                    sfd.Filter = "Bitmap Image (*.BMP)|*.bmp|JPEG Image (*.JPEG)|*.jpeg|PNG Image (*.PNG)|*.png";
                    sfd.FilterIndex = 3;
                    sfd.DefaultExt = "png";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Current.DrawSpace.Final.Save(sfd.FileName);
                        Current.FilePath = sfd.FileName;
                        Current.Saved = true;
                        saveToolStripMenuItem.Enabled = false;
                        Current.Stored = true;
                    }
                }
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (working)
            {
                if(!Current.Saved)
                {
                    DialogResult dialog = MessageBox.Show("Your work haven't saved yet.\nDo you want to save it", "Photo Editor",
                                                           MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                        SaveToolStripMenuItem_Click(sender, e);
                    else if (dialog == DialogResult.Cancel)
                        return;
                }

                tools.Select.Selected = false;

                workSpaceTabControl.SelectedTab.Controls.Remove(Current);
                Current.LayerContainer.Dispose();
                Current.History.Dispose();
                Current.Dispose();
                workSpaceTabControl.SelectedTab.Dispose();

                if (workSpaceTabControl.TabCount == 0)
                {
                    LayerMenuStripEnable(false);
                    ColorMenuStripEnable(false);
                    FilterMenuStripEnable(false);
                    ViewMenuStripEnable(false);
                    working = false;
                    closeToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    Current.FileName = Current.FilePath = "";
                    Current = null;
                }
                else Current = (WorkSpace)workSpaceTabControl.SelectedTab.Controls[0];
            }
        }       

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Edit menu
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Current.History.Remove())
                DSUpdate();
        }

        #endregion

        #region Tool menu

        #endregion

        #region View menu
        private void ZoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomInBtn_Click(zoomInBtn, null);
        }

        private void ZoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomOutBtn_Click(zoomInBtn, null);
        }

        private void CenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterBtn_Click(centerBtn, null);
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
            Bitmap bmp = new Bitmap(Current.BmpSize.Width, Current.BmpSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush brush = new SolidBrush(mainColorPic.BackColor))
            {
                g.FillRectangle(brush, 0, 0, Current.BmpSize.Width, Current.BmpSize.Height);
            }
            Current.DrawSpace.ProcessBoxImage = bmp;
            DSProcessUpdate(HistoryEvent.Draw);
            DSUpdate();
        }

        #endregion

        #region Color menu

        private void ColorMenuStripEnable(bool enable)
        {
            foreach(ToolStripMenuItem item in colorToolStripMenuItem.DropDownItems)
            {
                item.Enabled = enable;
            }
        }
        private void BrightnessAndContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.BrightnessContrast bc = new Forms.BrightnessContrast(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    bc.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    bc.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                if (bc.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = bc.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        private void HueAndSaturationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.HueSaturation hs = new Forms.HueSaturation(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    hs.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    hs.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                hs.Initialize();

                if (hs.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = hs.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        private void InvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp;
            float x, y;
            if (!tools.Select.Selected)
            {
                bmp = (Bitmap)Current.DrawSpace.ProcessBoxImage.Clone();
                x = y = 0;
            }
            else
            {
                bmp = (Current.DrawSpace.ProcessBoxImage as Bitmap).Clone(tools.Select.FixedRect, Current.BmpPixelFormat);
                x = tools.Select.FixedRect.X;
                y = tools.Select.FixedRect.Y;
            }

            using(Graphics g = Graphics.FromImage(bmp))
            {
                System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();
                matrix.Matrix00 = matrix.Matrix11 = matrix.Matrix22 = -1f;
                matrix.Matrix33 = matrix.Matrix40 = matrix.Matrix41 = matrix.Matrix42 = matrix.Matrix44 = 1f;

                using (System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes())
                {
                    attributes.SetColorMatrix(matrix);
                    g.DrawImage(Current.LayerContainer.Current.Layer.Image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                        x, y, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }

                Current.DrawSpace.ProcessBoxImage = new Bitmap(bmp);
            }

            bmp.Dispose();

            DSProcessUpdate(HistoryEvent.DrawFilter);
            DSUpdate();
        }

        private void ThresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.Threshold th = new Forms.Threshold(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    th.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    th.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                th.Initialize();

                if (th.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = th.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        private void ColorBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.ColorBalance cb = new Forms.ColorBalance(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    cb.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    cb.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                if (cb.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = cb.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        #endregion

        #region Filter menu

        private void FilterMenuStripEnable(bool enable)
        {
            foreach (ToolStripMenuItem item in filterToolStripMenuItem.DropDownItems)
            {
                item.Enabled = enable;
            }
        }

        private void NoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.Noise ns = new Forms.Noise(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    ns.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    ns.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                ns.Initialize();

                if (ns.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = ns.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        private void PixelateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.Pixelate px = new Forms.Pixelate(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    px.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    px.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                if (px.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = px.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        private void BlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.GaussianBlur gb = new Forms.GaussianBlur(this, Current.LayerContainer))
            {
                if (!tools.Select.Selected)
                    gb.Image = Current.LayerContainer.Current.Layer.Image;
                else
                    gb.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, Current.BmpPixelFormat);

                gb.Initialize();

                if (gb.ShowDialog() == DialogResult.OK)
                {
                    Current.DrawSpace.ProcessBoxImage = gb.Image;
                    DSProcessUpdate(HistoryEvent.DrawFilter);
                    DSUpdate();
                }
            }
        }

        private void SharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region Tool menu
        private void AboutPhotoEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #endregion

        #region WorkSpace

        private void AddWorkTab(Bitmap bmp,Color color)
        {
            LayerMenuStripEnable(true);
            ColorMenuStripEnable(true);
            FilterMenuStripEnable(true);
            ViewMenuStripEnable(true);

            if (Current != null)
            {
                layerPanel.Controls.Remove(Current.LayerContainer);
                historyTabPage.Controls.Remove(Current.History);
            }

            DrawSpace drawSpace = new DrawSpace();

            LayerContainer layerContainer = new LayerContainer();
            Layer firstLayer = new Layer(bmp, "Layer1", true);
            layerContainer.AddLayerRow(ref firstLayer);
            layerContainer.ScaleMatrix = drawSpace.ScaleMatrix;

            if (historyTabPage.Controls.Count != 0)
                historyTabPage.Controls.Clear();
            History history = new History();
            historyTabPage.Controls.Add(history);

            WorkSpace newWS = new WorkSpace(drawSpace, layerContainer, history);
            TabPage tab = new TabPage();
            tab.Controls.Add(newWS);
            workSpaceTabControl.TabPages.Add(tab);
            Current = newWS;
            Current.BmpSize = bmp.Size;
            Current.Rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            Current.BmpPixelFormat = bmp.PixelFormat;
            DrawSpaceInit();
            LayerContainerInit();
            Current.DrawSpace.BGGenerator(color);
            workSpaceTabControl.SelectedIndex = workSpaceTabControl.TabPages.IndexOf(tab);
        }

        private void WorkSpaceTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (workSpaceTabControl.TabCount == 0)
                return;

            layerPanel.Controls.Remove(Current.LayerContainer);
            historyTabPage.Controls.Remove(Current.History);
            Current = (WorkSpace)workSpaceTabControl.SelectedTab.Controls[0];
            layerPanel.Controls.Add(Current.LayerContainer);
            LayerButtonCheck();
            opacityVal = Current.LayerContainer.Current.Layer.Opacity;
            OpacityBarUpdate();
            historyTabPage.Controls.Add(Current.History);
            saveToolStripMenuItem.Enabled = !Current.Saved;
            BlendModeBoxUpdate(Current.LayerContainer.Current.Blend);
        }

        #endregion

        #region DrawSpace

        private void DrawSpaceInit()
        {
            Current.DrawSpace.Location = new System.Drawing.Point(0, 0);
            Current.DrawSpace.Name = "workspace";
            Current.DrawSpace.Size = Current.BmpSize;
            Current.DrawSpace.Tools = tools;
            Current.DrawSpace.Init();
            Current.DrawSpace.Event.MouseDown += DS_MouseDown;
            Current.DrawSpace.Event.MouseLeave += DS_MouseLeave;
            Current.DrawSpace.Event.MouseMove += DS_MouseMove;
            Current.DrawSpace.Event.MouseUp += DS_MouseUp;

            CenterBtn_Click(centerBtn, null);
        }

        public void DSProcessUpdate(HistoryEvent e)
        {
            Current.Saved = false;
            Current.Parent.Text = Current.FileName + "*";
            saveToolStripMenuItem.Enabled = true;

            Bitmap bmp;
            if (!tools.Select.Selected) bmp = (Bitmap)Current.DrawSpace.ProcessBoxImage.Clone();
            else bmp = (Current.DrawSpace.ProcessBoxImage as Bitmap).Clone(tools.Select.FixedRect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (e == HistoryEvent.Draw || e == HistoryEvent.Erase)
            {
                Current.LayerContainer.ProcessUpdate(bmp);
            }
            else if (e == HistoryEvent.DrawFilter )
            {
                Current.LayerContainer.ProcessUpdate(bmp, filter: true);
            }

            Current.DrawSpace.ClearProcess();
            bmp.Dispose();

            Current.History.Add(e, Current.LayerContainer.Current);
        }

        public void DSUpdate()
        {
            Current.LayerContainer.FinalUpdate(Current.DrawSpace.Final_Graphics, Current.DrawSpace.Final);
            Current.DrawSpace.FinalDisplay();
            Current.DrawSpace.CurrentVisible = Current.LayerContainer.Current.Layer.Visible;
            Current.DrawSpace.Invalidate();
        }

        private void DS_MouseDown(object sender, MouseEventArgs e)
        {
            if (tools.Tool == Tool.Picker)
            {
                mainColorPic.BackColor = tools.Picker.Color;
            }
        }

        private void DS_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation.Text = e.Location.ToString();
        }
        private void DS_MouseLeave(object sender, EventArgs e)
        {
            mouseLocation.Text = "";
        }

        private void DS_MouseUp(object sender, MouseEventArgs e)
        {
            switch(tools.Tool)
            {
                case Tool.Pen:
                    DSProcessUpdate(HistoryEvent.Draw);
                    break;
                case Tool.Eraser:
                    DSProcessUpdate(HistoryEvent.Erase);
                    break;
                case Tool.Transform:
                    if(!tools.Select.Selected && tools.Transform.Done)
                    {
                        DSProcessUpdate(HistoryEvent.Draw);
                        tools.Transform.Done = false;
                    }
                    break;
            }

            DSUpdate();
        }

        #endregion

        #region LeftPanel

        #region ColorPanel

        private bool colorIsPicking = false;
        private void ColorWheel_MouseDown(object sender, MouseEventArgs e)
        {
            colorIsPicking = true;
        }

        private void ColorWheel_MouseMove(object sender, MouseEventArgs e)
        {
            if (colorIsPicking)
            {
                using (Bitmap bmp = new Bitmap(colorWheel.Image))
                {
                    if (e.X > 0 && e.Y > 0 && e.X < colorWheel.Width && e.Y < colorWheel.Height)
                    {
                        Color c = bmp.GetPixel(e.X, e.Y);
                        if (c.A == 255)
                            mainColorPic.BackColor = c;
                    }
                }
            }
        }

        private void ColorWheel_MouseUp(object sender, MouseEventArgs e)
        {
            colorIsPicking = false;
        }

        private void ColorWheel_MouseClick(object sender, MouseEventArgs e)
        {
            using (Bitmap bmp = new Bitmap(colorWheel.Image))
            {
                Color c = bmp.GetPixel(e.X, e.Y);
                if (c.A == 255)
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

            hexCode.Text = ColorTranslator.ToHtml(mainColorPic.BackColor);
            tools.Color = mainColorPic.BackColor;
        }

        private void BarUpdate(ref PictureBox bar, Color c, int val)
        {
            using (SolidBrush b = new SolidBrush(c))
            {
                using (Graphics g = bar.CreateGraphics())
                {
                    g.Clear(bar.BackColor);
                    g.FillRectangle(b, new Rectangle(0, 0, val / 2, bar.Height));
                }
            }
        }

        private void ValCheck(ref int n)
        {
            if (n > 255) n = 255;
            if (n < 0) n = 0;
        }

        private void BarVal(ref int val, ref PictureBox bar, ref MouseEventArgs e)
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

        #region ToolPanel

        private void MToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            UncheckAll();
            if (e.ClickedItem.GetType() == typeof(ToolStripButton))
            {
                ToolStripButton button = (ToolStripButton)e.ClickedItem;
                button.Checked = true;
                button.CheckState = CheckState.Checked;
                if (button.Text == transformStripButton.Text)
                {
                    if(tools.Select.Selected)
                    {
                        tools.Transform.Done = false;
                        tools.Transform.Rect = tools.Select.Rect;
                        tools.Transform.StartPoint = tools.Select.Rect.Location;
                        tools.Transform.Image = Current.LayerContainer.Current.Layer.Image.Clone(tools.Select.FixedRect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Current.LayerContainer.Current.Layer.Stacking();
                        tools.Eraser.MakeTransparent(Current.LayerContainer.Current.Layer.Image, tools.Select.FixedRect);
                        DSUpdate();
                        Current.DrawSpace.TransformRectDisplay();
                    }
                    tools.Tool = Tool.Transform;
                }
                else if (button.Text == toolStripButton.Text)
                {
                    tools.Tool = Tool.Select;
                }
                else if (button.Text == dragStripButton.Text)
                {
                    tools.Tool = Tool.Drag;
                }
            }
            ChangeTool();
        }
        private void PToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            UncheckAll();
            if (e.ClickedItem.GetType() == typeof(ToolStripButton))
            {
                ToolStripButton button = (ToolStripButton)e.ClickedItem;
                button.Checked = true;
                button.CheckState = CheckState.Checked;
                if (button.Text == penStripButton.Text)
                {
                    tools.Tool = Tool.Pen;
                }
                else if (button.Text == eraserStripButton.Text)
                {
                    tools.Tool = Tool.Eraser;
                }
                else if (button.Text == pickerStripButton.Text)
                {
                    tools.Tool = Tool.Picker;
                }
            }
            ChangeTool();
        }

        private void SToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            UncheckAll();
        }

        private void UncheckAll()
        {
            foreach (ToolStripButton button in mToolStrip.Items)
            {
                button.Checked = false;
                button.CheckState = CheckState.Unchecked;
            }
            foreach (ToolStripButton button in pToolStrip.Items)
            {
                button.Checked = false;
                button.CheckState = CheckState.Unchecked;
            }
            foreach (ToolStripButton button in sToolStrip.Items)
            {
                button.Checked = false;
                button.CheckState = CheckState.Unchecked;
            }

            if(tools.Tool == Tool.Transform)
            {
                if (tools.Select.Selected)
                {
                    tools.Select.Selected = false;
                    tools.Transform.Image.Dispose();
                    Current.DrawSpace.ClearTop();
                }
            }
        }

        private void ChangeTool()
        {
            if(propertiesPanel.Controls.Count !=0)
            {
                propertiesPanel.Controls.Remove(propertiesPanel.Controls[0]);
                propertiesPanel.Controls.Add(tools.Current);
            }
            else propertiesPanel.Controls.Add(tools.Current);
        }

        #endregion

        #endregion

        #region Layer

        private void LayerContainerInit()
        {
            Current.LayerContainer.AutoScroll = true;
            Current.LayerContainer.Location = new System.Drawing.Point(3, 85);
            Current.LayerContainer.Name = "Current.LayerContainer";
            Current.LayerContainer.Size = new System.Drawing.Size(layerPanel.Width - 6, layerPanel.Height - 87);
            Current.LayerContainer.Tool = tools;
            layerPanel.Controls.Add(Current.LayerContainer);
            blendModeBox.SelectedIndex = 0;
            BlendModeBox_Select();
            opacityVal = 100f;
            OpacityBarUpdate();
        }

        private void LayerMenuStripEnable(bool enable)
        {
            foreach(ToolStripMenuItem item in layerToolStripMenuItem.DropDownItems)
            {
                item.Enabled = enable;
            }
            layerPanel.Enabled = enable;
        }

        private void NewLStripButton_Click(object sender, EventArgs e)
        {
            using (Forms.NewLayer nlf = new Forms.NewLayer())
            {
                nlf.SetDefaultName(Current.LayerContainer.Count);
                if (nlf.ShowDialog() == DialogResult.OK)
                {
                    string name = nlf.LayerName;
                    bool visible = nlf.IsVisible;
                    using (Bitmap newBmp = new Bitmap(Current.BmpSize.Width, Current.BmpSize.Height))
                    {
                        newBmp.MakeTransparent();
                        Layer layer = new Layer(newBmp, name, visible);
                        Current.LayerContainer.AddLayerRow(ref layer);
                        LayerButtonCheck();
                        blendModeBox.SelectedIndex = 0;
                        BlendModeBox_Select();
                        opacityVal = Current.LayerContainer.Current.Layer.Opacity;
                        OpacityBarUpdate();
                        DSProcessUpdate(HistoryEvent.NewL);
                        DSUpdate();
                    }
                }
            }
        }

        private void DeleteLStripButton_Click(object sender, EventArgs e)
        {
            Current.LayerContainer.RemoveLayerRow();
            LayerButtonCheck();
            DSProcessUpdate(HistoryEvent.DeleteL);
            DSUpdate();
        }
        private void RenameLStripButton_Click(object sender, EventArgs e)
        {
            using (Forms.LayerRename lr = new Forms.LayerRename())
            {
                lr.DefaultName = Current.LayerContainer.Current.Text;
                if (lr.ShowDialog() == DialogResult.OK)
                {
                    if (lr.NewName != "")
                    {
                        Current.LayerContainer.Current.Text = lr.NewName;
                        Current.LayerContainer.UpdateName();
                    }
                }
            }
        }

        private void ClearLStripButton_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(Current.LayerContainer.Current.Layer.Image))
            {
                g.Clear(Color.Transparent);
            }
            DSProcessUpdate(HistoryEvent.Draw);
            DSUpdate();
        }

        private void DownLStripButton_Click(object sender, EventArgs e)
        {
            Current.LayerContainer.MoveDown();
            LayerButtonCheck();
            DSProcessUpdate(HistoryEvent.Ldown);
            DSUpdate();
        }

        private void UpLStripButton_Click(object sender, EventArgs e)
        {
            Current.LayerContainer.MoveUp();
            LayerButtonCheck();
            DSProcessUpdate(HistoryEvent.Lup);
            DSUpdate();
        }

        public void LayerButtonCheck()
        {
            if (Current.LayerContainer.CurrentIndex == Current.LayerContainer.Count - 1)
            {

                mergeLStripButton.Enabled = true;
                downLStripButton.Enabled = true;
                upLStripButton.Enabled = false;
            }
            else if (Current.LayerContainer.CurrentIndex == 0)
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

            if (Current.LayerContainer.Count > 1)
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
            Current.LayerContainer.Merge();
            LayerButtonCheck();
            DSProcessUpdate(HistoryEvent.MergeL);
            DSUpdate();
        }

        private void DuplicateLStripButton_Click(object sender, EventArgs e)
        {
            Current.LayerContainer.Duplicate();
            LayerButtonCheck();
            DSProcessUpdate(HistoryEvent.DuplicateL);
            DSUpdate();
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
            using (Graphics g = opacityBar.CreateGraphics())
            {
                label10.Text = ((int)opacityVal).ToString();
                int w = (int)Math.Ceiling(((float)opacityVal / 100) * opacityBar.Width);
                g.Clear(opacityBar.BackColor);
                g.FillRectangle(Brushes.Gray, new Rectangle(0, 0, w, opacityBar.Height));
            }
        }

        private void OpacityBar_MouseUp(object sender, MouseEventArgs e)
        {
            Current.LayerContainer.Current.Opacity = opacityVal;
            Current.LayerContainer.Current.Layer.Opacity = opacityVal;
            DSProcessUpdate(HistoryEvent.Opacity);
            DSUpdate();
        }

        private void BlendModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlendModeBox_Select();
            DSUpdate();
        }

        private void BlendModeBox_Select()
        {
            switch(blendModeBox.SelectedIndex)
            {
                case 0:
                    Current.LayerContainer.Current.Blend = Blend.Normal;
                    break;
                case 1:
                    Current.LayerContainer.Current.Blend = Blend.Multiply;
                    break;
                case 2:
                    Current.LayerContainer.Current.Blend = Blend.Screen;
                    break;
                case 3:
                    Current.LayerContainer.Current.Blend = Blend.Darken;
                    break;
                case 4:
                    Current.LayerContainer.Current.Blend = Blend.Lighten;
                    break;
                case 5:
                    Current.LayerContainer.Current.Blend = Blend.Overlay;
                    break;
            }
        }

        public void BlendModeBoxUpdate(Blend mode)
        {
            switch (mode)
            {
                case Blend.Normal:
                    blendModeBox.SelectedIndex = 0;
                    break;
                case Blend.Multiply:
                    blendModeBox.SelectedIndex = 1;
                    break;
                case Blend.Screen:
                    blendModeBox.SelectedIndex = 2;
                    break;
                case Blend.Darken:
                    blendModeBox.SelectedIndex = 3;
                    break;
                case Blend.Lighten:
                    blendModeBox.SelectedIndex = 4;
                    break;
                case Blend.Overlay:
                    blendModeBox.SelectedIndex = 5;
                    break;
            }
        }

        #endregion

        #region BottomPanel

        private void ViewMenuStripEnable(bool enable)
        {
            foreach (ToolStripMenuItem item in viewToolStripMenuItem.DropDownItems)
            {
                item.Enabled = enable;
            }
            bottomPanel.Enabled = enable;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Current == null) return;

            float zoom = 0;
            switch (comboBox1.SelectedIndex)
            {
                //50%
                case 0:
                    zoom = 50;
                    break;
                //75%
                case 1:
                    zoom = 75;
                    break;
                //100%
                case 2:
                    zoom = 100;
                    break;
                //150%
                case 3:
                    zoom = 150;
                    break;
                //200%
                case 4:
                    zoom = 200;
                    break; 
                //200%
                case 5:
                    zoom = 300;
                    break; 
                //200%
                case 6:
                    zoom = 400;
                    break;
            }

            Current.DrawSpace.Scaling(zoom / 100);
            comboBox1.Text = ((int)(Current.DrawSpace.Zoom * 100)).ToString() + '%';
            DSUpdate();
        }

        private void ZoomOutBtn_Click(object sender, EventArgs e)
        {
            float zoom = float.Parse(comboBox1.Text.Substring(0, comboBox1.Text.Length - 1));
            if (zoom <= 50) return;

            float n;
            float m = zoom;
            foreach (string text in comboBox1.Items)
            {
                n = float.Parse(text.Substring(0, text.Length - 1));
                if (n < zoom)
                {
                    m = n;
                }
                else
                {
                    zoom = m;
                    break;
                }
            }

            Current.DrawSpace.Scaling(zoom / 100);
            comboBox1.Text = ((int)(Current.DrawSpace.Zoom * 100)).ToString() + '%';
            DSUpdate();
        }

        private void ZoomInBtn_Click(object sender, EventArgs e)
        {
            float zoom = float.Parse(comboBox1.Text.Substring(0, comboBox1.Text.Length - 1));
            if (zoom >= 400) return;

            float n;
            foreach (string text in comboBox1.Items)
            {
                n = float.Parse(text.Substring(0, text.Length - 1));
                if(zoom <n)
                {
                    zoom = n;
                    break;
                }
            }

            Current.DrawSpace.Scaling(zoom / 100);
            comboBox1.Text = ((int)(Current.DrawSpace.Zoom * 100)).ToString() + '%';
            DSUpdate();
        }

        private void CenterBtn_Click(object sender, EventArgs e)
        {
            Current.DrawSpace.SetCenter();
            comboBox1.Text = ((int)(Current.DrawSpace.Zoom * 100)).ToString() + '%';
            DSUpdate();
        }

        #endregion

    }
}