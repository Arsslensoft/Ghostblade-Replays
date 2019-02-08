namespace Ghostblade
{
    partial class ReplayPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.maplb = new MetroFramework.Controls.MetroLabel();
            this.Replaylb = new MetroFramework.Controls.MetroLink();
            this.Champion = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gameidlb = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.versionlb = new MetroFramework.Controls.MetroLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.maplb);
            this.panel1.Controls.Add(this.Replaylb);
            this.panel1.Controls.Add(this.Champion);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(752, 49);
            this.panel1.TabIndex = 13;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ReplayCtrl_MouseClick);
            // 
            // maplb
            // 
            this.maplb.AutoSize = true;
            this.maplb.FontSize = MetroFramework.MetroLabelSize.Small;
            this.maplb.Location = new System.Drawing.Point(82, 26);
            this.maplb.Name = "maplb";
            this.maplb.Size = new System.Drawing.Size(86, 15);
            this.maplb.Style = MetroFramework.MetroColorStyle.Green;
            this.maplb.TabIndex = 17;
            this.maplb.Text = "Summoners Rift";
            this.maplb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.maplb.UseStyleColors = true;
            // 
            // Replaylb
            // 
            this.Replaylb.AutoSize = true;
            this.Replaylb.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.Replaylb.Location = new System.Drawing.Point(82, 1);
            this.Replaylb.Name = "Replaylb";
            this.Replaylb.Size = new System.Drawing.Size(400, 25);
            this.Replaylb.TabIndex = 16;
            this.Replaylb.Text = "Replay";
            this.Replaylb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Replaylb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Replaylb.UseSelectable = true;
            this.Replaylb.Click += new System.EventHandler(this.Link_MouseClick);
            // 
            // Champion
            // 
            this.Champion.BackgroundImage = global::Ghostblade.Properties.Resources.button_play_red;
            this.Champion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Champion.Location = new System.Drawing.Point(26, 3);
            this.Champion.Name = "Champion";
            this.Champion.Size = new System.Drawing.Size(50, 43);
            this.Champion.TabIndex = 15;
            this.Champion.TabStop = false;
            this.Champion.Click += new System.EventHandler(this.Champion_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gameidlb);
            this.panel3.Controls.Add(this.metroLabel1);
            this.panel3.Controls.Add(this.versionlb);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(583, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(110, 49);
            this.panel3.TabIndex = 14;
            // 
            // gameidlb
            // 
            this.gameidlb.AutoSize = true;
            this.gameidlb.Dock = System.Windows.Forms.DockStyle.Top;
            this.gameidlb.FontSize = MetroFramework.MetroLabelSize.Small;
            this.gameidlb.Location = new System.Drawing.Point(0, 15);
            this.gameidlb.Name = "gameidlb";
            this.gameidlb.Size = new System.Drawing.Size(67, 15);
            this.gameidlb.TabIndex = 8;
            this.gameidlb.Text = "0000000000";
            this.gameidlb.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.Location = new System.Drawing.Point(0, 0);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(108, 15);
            this.metroLabel1.TabIndex = 7;
            this.metroLabel1.Text = "00/00/0000 00:00:00";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // versionlb
            // 
            this.versionlb.AutoSize = true;
            this.versionlb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.versionlb.Location = new System.Drawing.Point(0, 30);
            this.versionlb.Name = "versionlb";
            this.versionlb.Size = new System.Drawing.Size(58, 19);
            this.versionlb.Style = MetroFramework.MetroColorStyle.Silver;
            this.versionlb.TabIndex = 7;
            this.versionlb.Text = "5.3.0.291";
            this.versionlb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.versionlb.UseStyleColors = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.metroButton2);
            this.panel2.Controls.Add(this.metroButton1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(693, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(59, 49);
            this.panel2.TabIndex = 13;
            // 
            // metroButton2
            // 
            this.metroButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroButton2.Location = new System.Drawing.Point(0, 24);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(59, 25);
            this.metroButton2.Style = MetroFramework.MetroColorStyle.Red;
            this.metroButton2.TabIndex = 9;
            this.metroButton2.Text = "Delete";
            this.metroButton2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton2.UseSelectable = true;
            this.metroButton2.UseStyleColors = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // metroButton1
            // 
            this.metroButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroButton1.Location = new System.Drawing.Point(0, 0);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(59, 24);
            this.metroButton1.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroButton1.TabIndex = 8;
            this.metroButton1.Text = "Play";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.UseStyleColors = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            this.metroButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.metroButton1_MouseClick);
            // 
            // ReplayPanel
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Controls.Add(this.panel1);
            this.Name = "ReplayPanel";
            this.Size = new System.Drawing.Size(759, 57);
            this.Resize += new System.EventHandler(this.ReplayPanel_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetroFramework.Controls.MetroLabel maplb;
        private MetroFramework.Controls.MetroLink Replaylb;
        private System.Windows.Forms.PictureBox Champion;
        private System.Windows.Forms.Panel panel3;
        private MetroFramework.Controls.MetroLabel gameidlb;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel versionlb;
        private System.Windows.Forms.Panel panel2;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroButton metroButton1;

    }
}
