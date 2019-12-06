namespace DoAnLTTQ.Tools
{
    partial class PenTool
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sizeBar = new System.Windows.Forms.PictureBox();
            this.opacityBar = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Opacity";
            // 
            // sizeBar
            // 
            this.sizeBar.BackColor = System.Drawing.Color.Gainsboro;
            this.sizeBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sizeBar.Location = new System.Drawing.Point(80, 4);
            this.sizeBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sizeBar.Name = "sizeBar";
            this.sizeBar.Size = new System.Drawing.Size(106, 12);
            this.sizeBar.TabIndex = 5;
            this.sizeBar.TabStop = false;
            this.sizeBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseDown);
            this.sizeBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseDown);
            // 
            // opacityBar
            // 
            this.opacityBar.BackColor = System.Drawing.Color.Gainsboro;
            this.opacityBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opacityBar.Location = new System.Drawing.Point(80, 31);
            this.opacityBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.opacityBar.Name = "opacityBar";
            this.opacityBar.Size = new System.Drawing.Size(106, 12);
            this.opacityBar.TabIndex = 5;
            this.opacityBar.TabStop = false;
            this.opacityBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseDown);
            this.opacityBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Bar_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "100";
            // 
            // PenTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.opacityBar);
            this.Controls.Add(this.sizeBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PenTool";
            this.Size = new System.Drawing.Size(232, 185);
            ((System.ComponentModel.ISupportInitialize)(this.sizeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox sizeBar;
        private System.Windows.Forms.PictureBox opacityBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
