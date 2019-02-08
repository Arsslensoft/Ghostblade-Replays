namespace Ghostblade
{
    partial class ScheduleControl
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
            this.gamebx = new GhostLib.Gui.NSGroupBox();
            this.TEAM1PIC = new System.Windows.Forms.PictureBox();
            this.TEAM2PIC = new System.Windows.Forms.PictureBox();
            this.TEAM1LB = new MetroFramework.Controls.MetroLabel();
            this.TEAM2LB = new MetroFramework.Controls.MetroLabel();
            this.VSLB = new MetroFramework.Controls.MetroLabel();
            this.gamebx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TEAM1PIC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TEAM2PIC)).BeginInit();
            this.SuspendLayout();
            // 
            // gamebx
            // 
            this.gamebx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.gamebx.Controls.Add(this.VSLB);
            this.gamebx.Controls.Add(this.TEAM2LB);
            this.gamebx.Controls.Add(this.TEAM1LB);
            this.gamebx.Controls.Add(this.TEAM2PIC);
            this.gamebx.Controls.Add(this.TEAM1PIC);
            this.gamebx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamebx.DrawSeperator = true;
            this.gamebx.Location = new System.Drawing.Point(0, 0);
            this.gamebx.Name = "gamebx";
            this.gamebx.Size = new System.Drawing.Size(910, 114);
            this.gamebx.SubTitle = "LCK SPRING PROMOTION - DAY 2           22:00 GMT  ";
            this.gamebx.TabIndex = 10;
            this.gamebx.Title = "Best of 5 Games";
            // 
            // TEAM1PIC
            // 
            this.TEAM1PIC.BackgroundImage = global::Ghostblade.Properties.Resources.Aatrox;
            this.TEAM1PIC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.TEAM1PIC.Location = new System.Drawing.Point(100, 47);
            this.TEAM1PIC.Name = "TEAM1PIC";
            this.TEAM1PIC.Size = new System.Drawing.Size(65, 52);
            this.TEAM1PIC.TabIndex = 2;
            this.TEAM1PIC.TabStop = false;
            // 
            // TEAM2PIC
            // 
            this.TEAM2PIC.BackgroundImage = global::Ghostblade.Properties.Resources.Aatrox;
            this.TEAM2PIC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.TEAM2PIC.Location = new System.Drawing.Point(717, 47);
            this.TEAM2PIC.Name = "TEAM2PIC";
            this.TEAM2PIC.Size = new System.Drawing.Size(65, 52);
            this.TEAM2PIC.TabIndex = 3;
            this.TEAM2PIC.TabStop = false;
            // 
            // TEAM1LB
            // 
            this.TEAM1LB.AutoSize = true;
            this.TEAM1LB.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.TEAM1LB.Location = new System.Drawing.Point(180, 66);
            this.TEAM1LB.Name = "TEAM1LB";
            this.TEAM1LB.Size = new System.Drawing.Size(145, 19);
            this.TEAM1LB.Style = MetroFramework.MetroColorStyle.Blue;
            this.TEAM1LB.TabIndex = 4;
            this.TEAM1LB.Text = "SBENU SONICBOOM";
            this.TEAM1LB.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TEAM1LB.UseStyleColors = true;
            // 
            // TEAM2LB
            // 
            this.TEAM2LB.AutoSize = true;
            this.TEAM2LB.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.TEAM2LB.Location = new System.Drawing.Point(559, 66);
            this.TEAM2LB.Name = "TEAM2LB";
            this.TEAM2LB.Size = new System.Drawing.Size(17, 19);
            this.TEAM2LB.Style = MetroFramework.MetroColorStyle.Red;
            this.TEAM2LB.TabIndex = 5;
            this.TEAM2LB.Text = "S";
            this.TEAM2LB.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TEAM2LB.UseStyleColors = true;
            // 
            // VSLB
            // 
            this.VSLB.AutoSize = true;
            this.VSLB.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.VSLB.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.VSLB.Location = new System.Drawing.Point(437, 60);
            this.VSLB.Name = "VSLB";
            this.VSLB.Size = new System.Drawing.Size(34, 25);
            this.VSLB.Style = MetroFramework.MetroColorStyle.Yellow;
            this.VSLB.TabIndex = 6;
            this.VSLB.Text = "VS";
            this.VSLB.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.VSLB.UseStyleColors = true;
            // 
            // ScheduleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gamebx);
            this.Name = "ScheduleControl";
            this.Size = new System.Drawing.Size(910, 114);
            this.Resize += new System.EventHandler(this.ScheduleControl_Resize);
            this.gamebx.ResumeLayout(false);
            this.gamebx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TEAM1PIC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TEAM2PIC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private GhostLib.Gui.NSGroupBox gamebx;
        private MetroFramework.Controls.MetroLabel TEAM2LB;
        private MetroFramework.Controls.MetroLabel TEAM1LB;
        private System.Windows.Forms.PictureBox TEAM2PIC;
        private System.Windows.Forms.PictureBox TEAM1PIC;
        private MetroFramework.Controls.MetroLabel VSLB;
    }
}
