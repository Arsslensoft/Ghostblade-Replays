namespace Ghostblade
{
    partial class TopBanner
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
            this.components = new System.ComponentModel.Container();
            this.panel3 = new System.Windows.Forms.Panel();
            this.summoner2 = new MetroFramework.Controls.MetroLink();
            this.metroLink1 = new MetroFramework.Controls.MetroLink();
            this.metroLink5 = new MetroFramework.Controls.MetroLink();
            this.lvl2 = new System.Windows.Forms.Label();
            this.profilevox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.summoner1 = new MetroFramework.Controls.MetroLink();
            this.panel2 = new System.Windows.Forms.Panel();
            this.metroLink2 = new MetroFramework.Controls.MetroLink();
            this.pinglb = new MetroFramework.Controls.MetroLink();
            this.lvl1 = new System.Windows.Forms.Label();
            this.profileicon = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.profilevox)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.profileicon)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.summoner2);
            this.panel3.Controls.Add(this.lvl2);
            this.panel3.Controls.Add(this.profilevox);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 86);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1064, 60);
            this.panel3.TabIndex = 2;
            this.panel3.Visible = false;
            // 
            // summoner2
            // 
            this.summoner2.AutoSize = true;
            this.summoner2.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.summoner2.Location = new System.Drawing.Point(106, 3);
            this.summoner2.Name = "summoner2";
            this.summoner2.Size = new System.Drawing.Size(427, 23);
            this.summoner2.Style = MetroFramework.MetroColorStyle.White;
            this.summoner2.TabIndex = 11;
            this.summoner2.Text = "Master Killer G6";
            this.summoner2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.summoner2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.summoner2.UseCustomBackColor = true;
            this.summoner2.UseSelectable = true;
            this.summoner2.UseStyleColors = true;
            // 
            // metroLink1
            // 
            this.metroLink1.AutoSize = true;
            this.metroLink1.Location = new System.Drawing.Point(45, 29);
            this.metroLink1.Name = "metroLink1";
            this.metroLink1.Size = new System.Drawing.Size(94, 23);
            this.metroLink1.Style = MetroFramework.MetroColorStyle.White;
            this.metroLink1.TabIndex = 9;
            this.metroLink1.Text = "Facebook";
            this.metroLink1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink1.UseCustomBackColor = true;
            this.metroLink1.UseSelectable = true;
            this.metroLink1.UseStyleColors = true;
            this.metroLink1.Click += new System.EventHandler(this.metroLink2_Click);
            // 
            // metroLink5
            // 
            this.metroLink5.AutoSize = true;
            this.metroLink5.Location = new System.Drawing.Point(42, 5);
            this.metroLink5.Name = "metroLink5";
            this.metroLink5.Size = new System.Drawing.Size(96, 23);
            this.metroLink5.Style = MetroFramework.MetroColorStyle.Red;
            this.metroLink5.TabIndex = 6;
            this.metroLink5.Text = "80 ms";
            this.metroLink5.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink5.UseCustomBackColor = true;
            this.metroLink5.UseSelectable = true;
            this.metroLink5.UseStyleColors = true;
            this.metroLink5.Click += new System.EventHandler(this.metroLink5_Click);
            this.metroLink5.MouseEnter += new System.EventHandler(this.pinglb_MouseEnter);
            // 
            // lvl2
            // 
            this.lvl2.AutoSize = true;
            this.lvl2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lvl2.Location = new System.Drawing.Point(106, 29);
            this.lvl2.Name = "lvl2";
            this.lvl2.Size = new System.Drawing.Size(48, 13);
            this.lvl2.TabIndex = 2;
            this.lvl2.Text = "Level 30";
            // 
            // profilevox
            // 
            this.profilevox.BackgroundImage = global::Ghostblade.Properties.Resources._720;
            this.profilevox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.profilevox.Location = new System.Drawing.Point(32, 3);
            this.profilevox.Name = "profilevox";
            this.profilevox.Size = new System.Drawing.Size(57, 51);
            this.profilevox.TabIndex = 0;
            this.profilevox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Ghostblade.Properties.Resources.challenger;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.summoner1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lvl1);
            this.panel1.Controls.Add(this.profileicon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1064, 86);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            // 
            // summoner1
            // 
            this.summoner1.AutoSize = true;
            this.summoner1.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.summoner1.Location = new System.Drawing.Point(107, 21);
            this.summoner1.Name = "summoner1";
            this.summoner1.Size = new System.Drawing.Size(382, 23);
            this.summoner1.Style = MetroFramework.MetroColorStyle.White;
            this.summoner1.TabIndex = 10;
            this.summoner1.Text = "Master Killer G6";
            this.summoner1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.summoner1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.summoner1.UseCustomBackColor = true;
            this.summoner1.UseSelectable = true;
            this.summoner1.UseStyleColors = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.metroLink2);
            this.panel2.Controls.Add(this.pinglb);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(922, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(142, 86);
            this.panel2.TabIndex = 6;
            // 
            // metroLink2
            // 
            this.metroLink2.AutoSize = true;
            this.metroLink2.Location = new System.Drawing.Point(45, 50);
            this.metroLink2.Name = "metroLink2";
            this.metroLink2.Size = new System.Drawing.Size(65, 23);
            this.metroLink2.Style = MetroFramework.MetroColorStyle.White;
            this.metroLink2.TabIndex = 5;
            this.metroLink2.Text = "Facebook";
            this.metroLink2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink2.UseCustomBackColor = true;
            this.metroLink2.UseSelectable = true;
            this.metroLink2.UseStyleColors = true;
            this.metroLink2.Click += new System.EventHandler(this.metroLink2_Click);
            // 
            // pinglb
            // 
            this.pinglb.AutoSize = true;
            this.pinglb.Location = new System.Drawing.Point(45, 21);
            this.pinglb.Name = "pinglb";
            this.pinglb.Size = new System.Drawing.Size(64, 23);
            this.pinglb.Style = MetroFramework.MetroColorStyle.Red;
            this.pinglb.TabIndex = 3;
            this.pinglb.Text = "80 ms";
            this.pinglb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pinglb.UseCustomBackColor = true;
            this.pinglb.UseSelectable = true;
            this.pinglb.UseStyleColors = true;
            this.pinglb.Click += new System.EventHandler(this.metroLink5_Click);
            this.pinglb.MouseEnter += new System.EventHandler(this.pinglb_MouseEnter);
            // 
            // lvl1
            // 
            this.lvl1.AutoSize = true;
            this.lvl1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lvl1.Location = new System.Drawing.Point(107, 47);
            this.lvl1.Name = "lvl1";
            this.lvl1.Size = new System.Drawing.Size(48, 13);
            this.lvl1.TabIndex = 2;
            this.lvl1.Text = "Level 30";
            // 
            // profileicon
            // 
            this.profileicon.BackgroundImage = global::Ghostblade.Properties.Resources._720;
            this.profileicon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.profileicon.Location = new System.Drawing.Point(33, 17);
            this.profileicon.Name = "profileicon";
            this.profileicon.Size = new System.Drawing.Size(58, 55);
            this.profileicon.TabIndex = 0;
            this.profileicon.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.metroLink5);
            this.panel4.Controls.Add(this.metroLink1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(921, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(141, 58);
            this.panel4.TabIndex = 12;
            // 
            // TopBanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "TopBanner";
            this.Size = new System.Drawing.Size(1064, 148);
            this.Resize += new System.EventHandler(this.TopBanner_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.profilevox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.profileicon)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetroFramework.Controls.MetroLink metroLink2;
        private MetroFramework.Controls.MetroLink pinglb;
        private System.Windows.Forms.Label lvl1;
        private System.Windows.Forms.PictureBox profileicon;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private MetroFramework.Controls.MetroLink metroLink5;
        private System.Windows.Forms.Label lvl2;
        private System.Windows.Forms.PictureBox profilevox;
        private System.Windows.Forms.Timer timer1;
        private MetroFramework.Controls.MetroLink metroLink1;
        private MetroFramework.Controls.MetroLink summoner2;
        private MetroFramework.Controls.MetroLink summoner1;
        private System.Windows.Forms.Panel panel4;
    }
}
