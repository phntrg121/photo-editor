namespace DoAnLTTQ
{
    partial class DrawSpace
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (processing != null) processing.Dispose();
            if (g != null) g.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.finalBox = new System.Windows.Forms.PictureBox();
            this.processBox = new System.Windows.Forms.PictureBox();
            this.topBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.finalBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topBox)).BeginInit();
            this.SuspendLayout();
            // 
            // finalBox
            // 
            this.finalBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.finalBox.Location = new System.Drawing.Point(0, 0);
            this.finalBox.Name = "finalBox";
            this.finalBox.Size = new System.Drawing.Size(150, 150);
            this.finalBox.TabIndex = 0;
            this.finalBox.TabStop = false;
            this.finalBox.Controls.Add(this.processBox);
            // 
            // processBox
            // 
            this.processBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processBox.Location = new System.Drawing.Point(0, 0);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(150, 150);
            this.processBox.TabIndex = 1;
            this.processBox.TabStop = false;
            this.processBox.Controls.Add(this.topBox);
            // 
            // topBox
            // 
            this.topBox.BackColor = System.Drawing.Color.Transparent;
            this.topBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topBox.Location = new System.Drawing.Point(0, 0);
            this.topBox.Name = "topBox";
            this.topBox.Size = new System.Drawing.Size(150, 150);
            this.topBox.TabIndex = 3;
            this.topBox.TabStop = false;
            this.topBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.topBox.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.topBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mouse_Move);
            this.topBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // DrawSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.finalBox);
            this.Name = "DrawSpace";
            ((System.ComponentModel.ISupportInitialize)(this.finalBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox finalBox;
        private System.Windows.Forms.PictureBox processBox;
        private System.Windows.Forms.PictureBox topBox;
    }
}
