namespace Ghostblade
{
    partial class RecordingPanel
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
            this.Champion = new System.Windows.Forms.PictureBox();
            this.progspin = new MetroFramework.Controls.MetroProgressSpinner();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cancelbtn = new MetroFramework.Controls.MetroButton();
            this.specbtn = new MetroFramework.Controls.MetroButton();
            this.playerlabel = new MetroFramework.Controls.MetroLabel();
            this.Replaylb = new MetroFramework.Controls.MetroLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Champion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(49, 42);
            this.panel1.TabIndex = 8;
            // 
            // Champion
            // 
            this.Champion.BackgroundImage = global::Ghostblade.Properties.Resources.player_record;
            this.Champion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Champion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Champion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Champion.Location = new System.Drawing.Point(0, 0);
            this.Champion.Name = "Champion";
            this.Champion.Size = new System.Drawing.Size(49, 42);
            this.Champion.TabIndex = 0;
            this.Champion.TabStop = false;
            this.Champion.Click += new System.EventHandler(this.Champion_Click);
            // 
            // progspin
            // 
            this.progspin.Dock = System.Windows.Forms.DockStyle.Right;
            this.progspin.Location = new System.Drawing.Point(537, 0);
            this.progspin.Maximum = 100;
            this.progspin.Name = "progspin";
            this.progspin.Size = new System.Drawing.Size(44, 42);
            this.progspin.Style = MetroFramework.MetroColorStyle.Green;
            this.progspin.TabIndex = 9;
            this.progspin.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.progspin.UseSelectable = true;
            this.progspin.UseStyleColors = true;
            this.progspin.MouseEnter += new System.EventHandler(this.progspin_MouseEnter);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cancelbtn);
            this.panel2.Controls.Add(this.specbtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(483, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(54, 42);
            this.panel2.TabIndex = 10;
            // 
            // cancelbtn
            // 
            this.cancelbtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelbtn.Location = new System.Drawing.Point(0, 24);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(54, 18);
            this.cancelbtn.TabIndex = 1;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cancelbtn.UseSelectable = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // specbtn
            // 
            this.specbtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.specbtn.Location = new System.Drawing.Point(0, 0);
            this.specbtn.Name = "specbtn";
            this.specbtn.Size = new System.Drawing.Size(54, 24);
            this.specbtn.TabIndex = 0;
            this.specbtn.Text = "Spectate";
            this.specbtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.specbtn.UseSelectable = true;
            this.specbtn.Visible = false;
            this.specbtn.Click += new System.EventHandler(this.specbtn_Click);
            // 
            // playerlabel
            // 
            this.playerlabel.AutoSize = true;
            this.playerlabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.playerlabel.FontSize = MetroFramework.MetroLabelSize.Small;
            this.playerlabel.Location = new System.Drawing.Point(49, 27);
            this.playerlabel.Name = "playerlabel";
            this.playerlabel.Size = new System.Drawing.Size(56, 15);
            this.playerlabel.Style = MetroFramework.MetroColorStyle.Teal;
            this.playerlabel.TabIndex = 12;
            this.playerlabel.Text = "Spectator";
            this.playerlabel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.playerlabel.UseStyleColors = true;
            // 
            // Replaylb
            // 
            this.Replaylb.AutoSize = true;
            this.Replaylb.Dock = System.Windows.Forms.DockStyle.Top;
            this.Replaylb.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.Replaylb.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.Replaylb.Location = new System.Drawing.Point(49, 0);
            this.Replaylb.Name = "Replaylb";
            this.Replaylb.Size = new System.Drawing.Size(251, 25);
            this.Replaylb.Style = MetroFramework.MetroColorStyle.Lime;
            this.Replaylb.TabIndex = 13;
            this.Replaylb.Text = "Waiting for spectator data...";
            this.Replaylb.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Replaylb.UseStyleColors = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Replaylb);
            this.panel3.Controls.Add(this.playerlabel);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.progspin);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(581, 42);
            this.panel3.TabIndex = 14;
            // 
            // RecordingPanel
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Controls.Add(this.panel3);
            this.Name = "RecordingPanel";
            this.Size = new System.Drawing.Size(587, 48);
            this.Resize += new System.EventHandler(this.RecordingPanel_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Champion;
        private MetroFramework.Controls.MetroProgressSpinner progspin;
        private System.Windows.Forms.Panel panel2;
        private MetroFramework.Controls.MetroButton cancelbtn;
        private MetroFramework.Controls.MetroButton specbtn;
        private MetroFramework.Controls.MetroLabel playerlabel;
        private MetroFramework.Controls.MetroLabel Replaylb;
        private System.Windows.Forms.Panel panel3;
    }
}
