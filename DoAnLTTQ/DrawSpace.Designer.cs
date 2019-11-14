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
            this.backBox = new System.Windows.Forms.PictureBox();
            this.processBox = new System.Windows.Forms.PictureBox();
            this.frontBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.backBox)).BeginInit();
            this.backBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processBox)).BeginInit();
            this.processBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frontBox)).BeginInit();
            this.SuspendLayout();
            // 
            // backBox
            // 
            this.backBox.Controls.Add(this.processBox);
            this.backBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backBox.Location = new System.Drawing.Point(0, 0);
            this.backBox.Name = "backBox";
            this.backBox.Size = new System.Drawing.Size(150, 150);
            this.backBox.TabIndex = 0;
            this.backBox.TabStop = false;
            // 
            // processBox
            // 
            this.processBox.Controls.Add(this.frontBox);
            this.processBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processBox.Location = new System.Drawing.Point(0, 0);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(150, 150);
            this.processBox.TabIndex = 1;
            this.processBox.TabStop = false;
            // 
            // frontBox
            // 
            this.frontBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frontBox.Location = new System.Drawing.Point(0, 0);
            this.frontBox.Name = "frontBox";
            this.frontBox.Size = new System.Drawing.Size(150, 150);
            this.frontBox.TabIndex = 2;
            this.frontBox.TabStop = false;
            // 
            // DrawSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.backBox);
            this.Name = "DrawSpace";
            ((System.ComponentModel.ISupportInitialize)(this.backBox)).EndInit();
            this.backBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.processBox)).EndInit();
            this.processBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frontBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox backBox;
        private System.Windows.Forms.PictureBox processBox;
        private System.Windows.Forms.PictureBox frontBox;
    }
}
