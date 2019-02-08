namespace Ghostblade
{
    partial class FeaturedPlayer
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.Champion = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(41, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(102, 19);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel1.TabIndex = 8;
            this.metroLabel1.Text = "Player 1            ";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel1.UseStyleColors = true;
            // 
            // Champion
            // 
            this.Champion.BackgroundImage = global::Ghostblade.Properties.Resources.Aatrox;
            this.Champion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Champion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Champion.Location = new System.Drawing.Point(3, 0);
            this.Champion.Name = "Champion";
            this.Champion.Size = new System.Drawing.Size(32, 29);
            this.Champion.TabIndex = 7;
            this.Champion.TabStop = false;
            this.Champion.Click += new System.EventHandler(this.Champion_Click);
            // 
            // FeaturedPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.Champion);
            this.Name = "FeaturedPlayer";
            this.Size = new System.Drawing.Size(189, 29);
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.PictureBox Champion;
    }
}
