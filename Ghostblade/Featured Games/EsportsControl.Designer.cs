namespace Ghostblade
{
    partial class EsportsControl
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
            this.nsGroupBox1 = new GhostLib.Gui.NSGroupBox();
            this.metroLink2 = new MetroFramework.Controls.MetroLink();
            this.metroLink1 = new MetroFramework.Controls.MetroLink();
            this.ScheduleHostPanel = new System.Windows.Forms.Panel();
            this.nsGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nsGroupBox1
            // 
            this.nsGroupBox1.AutoScroll = true;
            this.nsGroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.nsGroupBox1.Controls.Add(this.metroLink2);
            this.nsGroupBox1.Controls.Add(this.metroLink1);
            this.nsGroupBox1.Controls.Add(this.ScheduleHostPanel);
            this.nsGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsGroupBox1.DrawSeperator = true;
            this.nsGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.nsGroupBox1.Name = "nsGroupBox1";
            this.nsGroupBox1.Size = new System.Drawing.Size(935, 292);
            this.nsGroupBox1.SubTitle = "";
            this.nsGroupBox1.TabIndex = 0;
            this.nsGroupBox1.Text = "nsGroupBox1";
            this.nsGroupBox1.Title = "Esports Schedule";
            // 
            // metroLink2
            // 
            this.metroLink2.Location = new System.Drawing.Point(706, 3);
            this.metroLink2.Name = "metroLink2";
            this.metroLink2.Size = new System.Drawing.Size(109, 23);
            this.metroLink2.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroLink2.TabIndex = 2;
            this.metroLink2.Text = "Esports Twitch.tv";
            this.metroLink2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink2.UseSelectable = true;
            this.metroLink2.UseStyleColors = true;
            this.metroLink2.Click += new System.EventHandler(this.metroLink2_Click);
            // 
            // metroLink1
            // 
            this.metroLink1.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.metroLink1.Location = new System.Drawing.Point(821, 3);
            this.metroLink1.Name = "metroLink1";
            this.metroLink1.Size = new System.Drawing.Size(93, 23);
            this.metroLink1.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroLink1.TabIndex = 1;
            this.metroLink1.Text = "Esports Home";
            this.metroLink1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLink1.UseSelectable = true;
            this.metroLink1.UseStyleColors = true;
            this.metroLink1.Click += new System.EventHandler(this.metroLink1_Click);
            // 
            // ScheduleHostPanel
            // 
            this.ScheduleHostPanel.AutoScroll = true;
            this.ScheduleHostPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ScheduleHostPanel.Location = new System.Drawing.Point(0, 32);
            this.ScheduleHostPanel.Name = "ScheduleHostPanel";
            this.ScheduleHostPanel.Size = new System.Drawing.Size(935, 260);
            this.ScheduleHostPanel.TabIndex = 0;
            // 
            // EsportsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.nsGroupBox1);
            this.Name = "EsportsControl";
            this.Size = new System.Drawing.Size(935, 292);
            this.Resize += new System.EventHandler(this.EsportsControl_Resize);
            this.nsGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GhostLib.Gui.NSGroupBox nsGroupBox1;
        private System.Windows.Forms.Panel ScheduleHostPanel;
        private MetroFramework.Controls.MetroLink metroLink1;
        private MetroFramework.Controls.MetroLink metroLink2;
    }
}
