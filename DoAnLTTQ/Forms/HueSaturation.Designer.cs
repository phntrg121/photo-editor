namespace DoAnLTTQ.Forms
{
    partial class HueSaturation
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
            origin.Dispose();
            adjusted.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saturationTrack = new System.Windows.Forms.TrackBar();
            this.hueTrack = new System.Windows.Forms.TrackBar();
            this.luminosityTrack = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.saturationTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.luminosityTrack)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(377, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "0";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(315, 137);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(234, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Saturation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Hue";
            // 
            // saturationTrack
            // 
            this.saturationTrack.LargeChange = 10;
            this.saturationTrack.Location = new System.Drawing.Point(71, 56);
            this.saturationTrack.Maximum = 100;
            this.saturationTrack.Minimum = -100;
            this.saturationTrack.Name = "saturationTrack";
            this.saturationTrack.Size = new System.Drawing.Size(300, 45);
            this.saturationTrack.TabIndex = 6;
            this.saturationTrack.TickStyle = System.Windows.Forms.TickStyle.None;
            this.saturationTrack.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // hueTrack
            // 
            this.hueTrack.AutoSize = false;
            this.hueTrack.LargeChange = 1;
            this.hueTrack.Location = new System.Drawing.Point(71, 20);
            this.hueTrack.Maximum = 180;
            this.hueTrack.Minimum = -180;
            this.hueTrack.Name = "hueTrack";
            this.hueTrack.Size = new System.Drawing.Size(300, 45);
            this.hueTrack.TabIndex = 7;
            this.hueTrack.TickStyle = System.Windows.Forms.TickStyle.None;
            this.hueTrack.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
            // 
            // luminosityTrack
            // 
            this.luminosityTrack.LargeChange = 10;
            this.luminosityTrack.Location = new System.Drawing.Point(71, 92);
            this.luminosityTrack.Maximum = 100;
            this.luminosityTrack.Minimum = -100;
            this.luminosityTrack.Name = "luminosityTrack";
            this.luminosityTrack.Size = new System.Drawing.Size(300, 45);
            this.luminosityTrack.TabIndex = 6;
            this.luminosityTrack.TickStyle = System.Windows.Forms.TickStyle.None;
            this.luminosityTrack.Scroll += new System.EventHandler(this.TrackBar3_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Luminosity";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(377, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "0";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 137);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Reset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // HueSaturation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 173);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.luminosityTrack);
            this.Controls.Add(this.saturationTrack);
            this.Controls.Add(this.hueTrack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HueSaturation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hue and Saturation";
            ((System.ComponentModel.ISupportInitialize)(this.saturationTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.luminosityTrack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar saturationTrack;
        private System.Windows.Forms.TrackBar hueTrack;
        private System.Windows.Forms.TrackBar luminosityTrack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
    }
}