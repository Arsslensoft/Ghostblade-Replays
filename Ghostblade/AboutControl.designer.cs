namespace Ghostblade
{
    partial class AboutControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutControl));
            this.labelX5 = new System.Windows.Forms.Label();
            this.labelX4 = new System.Windows.Forms.Label();
            this.labelX3 = new System.Windows.Forms.Label();
            this.labelX1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX5
            // 
            this.labelX5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelX5.Location = new System.Drawing.Point(212, 307);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(340, 23);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "Copyright (c) 2010-2015 Arsslensoft. All rights reserved.";
            // 
            // labelX4
            // 
            this.labelX4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelX4.Location = new System.Drawing.Point(171, 138);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(393, 169);
            this.labelX4.TabIndex = 10;
            this.labelX4.Text = resources.GetString("labelX4.Text");
            // 
            // labelX3
            // 
            this.labelX3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelX3.Location = new System.Drawing.Point(172, 125);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(480, 56);
            this.labelX3.TabIndex = 9;
            this.labelX3.Text = "Ghostblade Replays is protected by the copyright law and international treaties a" +
    "ny unauthorized use is prohibited.";
            // 
            // labelX1
            // 
            this.labelX1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelX1.Location = new System.Drawing.Point(171, 87);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(480, 51);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "Ghostblade Replays is a 3rd party program for recording League of Legends games, " +
    "allowing you to watch and analyze them at a later date, share them.\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(722, 317);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(171, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "GHOSTBLADE REPLAYS";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ghostblade.Properties.Resources.ghost_white;
            this.pictureBox1.Location = new System.Drawing.Point(130, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 40);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // AboutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX1);
            this.Name = "AboutControl";
            this.Size = new System.Drawing.Size(789, 339);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelX5;
        private System.Windows.Forms.Label labelX4;
        private System.Windows.Forms.Label labelX3;
        private System.Windows.Forms.Label labelX1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
