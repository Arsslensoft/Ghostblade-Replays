namespace Ghostblade
{
    partial class FeaturedGameControl
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
            this.ginfo = new MetroFramework.Controls.MetroLink();
            this.timelb = new MetroFramework.Controls.MetroLink();
            this.panel1 = new System.Windows.Forms.Panel();
            this.blueteam = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.vspanel = new System.Windows.Forms.Panel();
            this.vslb = new MetroFramework.Controls.MetroLabel();
            this.redteam = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.vspanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ginfo
            // 
            this.ginfo.AutoSize = true;
            this.ginfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ginfo.Location = new System.Drawing.Point(0, 0);
            this.ginfo.Name = "ginfo";
            this.ginfo.Size = new System.Drawing.Size(492, 19);
            this.ginfo.TabIndex = 4;
            this.ginfo.Text = "GAME_MAP, QUEUE TYPE - PLATFORM";
            this.ginfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ginfo.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ginfo.UseSelectable = true;
            this.ginfo.Click += new System.EventHandler(this.ginfo_Click);
            // 
            // timelb
            // 
            this.timelb.AutoSize = true;
            this.timelb.Dock = System.Windows.Forms.DockStyle.Right;
            this.timelb.Location = new System.Drawing.Point(492, 0);
            this.timelb.Name = "timelb";
            this.timelb.Size = new System.Drawing.Size(53, 19);
            this.timelb.TabIndex = 5;
            this.timelb.Text = "00:00";
            this.timelb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.timelb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.timelb.UseSelectable = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ginfo);
            this.panel1.Controls.Add(this.timelb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 219);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(545, 19);
            this.panel1.TabIndex = 6;
            // 
            // blueteam
            // 
            this.blueteam.Dock = System.Windows.Forms.DockStyle.Left;
            this.blueteam.Location = new System.Drawing.Point(0, 0);
            this.blueteam.Name = "blueteam";
            this.blueteam.Size = new System.Drawing.Size(225, 219);
            this.blueteam.TabIndex = 8;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // vspanel
            // 
            this.vspanel.Controls.Add(this.vslb);
            this.vspanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.vspanel.Location = new System.Drawing.Point(225, 0);
            this.vspanel.Name = "vspanel";
            this.vspanel.Size = new System.Drawing.Size(40, 219);
            this.vspanel.TabIndex = 9;
            // 
            // vslb
            // 
            this.vslb.AutoSize = true;
            this.vslb.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.vslb.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.vslb.Location = new System.Drawing.Point(3, 84);
            this.vslb.Name = "vslb";
            this.vslb.Size = new System.Drawing.Size(34, 25);
            this.vslb.Style = MetroFramework.MetroColorStyle.Yellow;
            this.vslb.TabIndex = 3;
            this.vslb.Text = "VS";
            this.vslb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.vslb.UseStyleColors = true;
            // 
            // redteam
            // 
            this.redteam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redteam.Location = new System.Drawing.Point(265, 0);
            this.redteam.Name = "redteam";
            this.redteam.Size = new System.Drawing.Size(280, 219);
            this.redteam.TabIndex = 10;
            // 
            // FeaturedGameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.redteam);
            this.Controls.Add(this.vspanel);
            this.Controls.Add(this.blueteam);
            this.Controls.Add(this.panel1);
            this.Name = "FeaturedGameControl";
            this.Size = new System.Drawing.Size(545, 238);
            this.Resize += new System.EventHandler(this.FeaturedGameControl_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.vspanel.ResumeLayout(false);
            this.vspanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLink ginfo;
        private MetroFramework.Controls.MetroLink timelb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel blueteam;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel vspanel;
        private MetroFramework.Controls.MetroLabel vslb;
        private System.Windows.Forms.Panel redteam;
    }
}
