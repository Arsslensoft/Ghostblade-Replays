namespace Ghostblade
{
    partial class ShardInfoControl
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.webbx = new GhostLib.Gui.NSGroupBox();
            this.storebx = new GhostLib.Gui.NSGroupBox();
            this.forumbx = new GhostLib.Gui.NSGroupBox();
            this.gamebx = new GhostLib.Gui.NSGroupBox();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // webbx
            // 
            this.webbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.webbx.Dock = System.Windows.Forms.DockStyle.Top;
            this.webbx.DrawSeperator = false;
            this.webbx.Location = new System.Drawing.Point(0, 147);
            this.webbx.Name = "webbx";
            this.webbx.Size = new System.Drawing.Size(491, 49);
            this.webbx.SubTitle = "Offline";
            this.webbx.TabIndex = 11;
            this.webbx.Title = "Website";
            this.webbx.MouseEnter += new System.EventHandler(this.webbx_MouseEnter);
            // 
            // storebx
            // 
            this.storebx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.storebx.Dock = System.Windows.Forms.DockStyle.Top;
            this.storebx.DrawSeperator = false;
            this.storebx.Location = new System.Drawing.Point(0, 98);
            this.storebx.Name = "storebx";
            this.storebx.Size = new System.Drawing.Size(491, 49);
            this.storebx.SubTitle = "Offline";
            this.storebx.TabIndex = 10;
            this.storebx.Title = "Store";
            this.storebx.MouseEnter += new System.EventHandler(this.storebx_MouseEnter);
            // 
            // forumbx
            // 
            this.forumbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.forumbx.Dock = System.Windows.Forms.DockStyle.Top;
            this.forumbx.DrawSeperator = false;
            this.forumbx.Location = new System.Drawing.Point(0, 49);
            this.forumbx.Name = "forumbx";
            this.forumbx.Size = new System.Drawing.Size(491, 49);
            this.forumbx.SubTitle = "Offline";
            this.forumbx.TabIndex = 9;
            this.forumbx.Title = "Community";
            this.forumbx.MouseEnter += new System.EventHandler(this.forumbx_MouseEnter);
            // 
            // gamebx
            // 
            this.gamebx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.gamebx.Dock = System.Windows.Forms.DockStyle.Top;
            this.gamebx.DrawSeperator = false;
            this.gamebx.Location = new System.Drawing.Point(0, 0);
            this.gamebx.Name = "gamebx";
            this.gamebx.Size = new System.Drawing.Size(491, 49);
            this.gamebx.SubTitle = "Offline";
            this.gamebx.TabIndex = 8;
            this.gamebx.Title = "Game";
            this.gamebx.MouseEnter += new System.EventHandler(this.gamebx_MouseEnter);
            // 
            // ShardInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.webbx);
            this.Controls.Add(this.storebx);
            this.Controls.Add(this.forumbx);
            this.Controls.Add(this.gamebx);
            this.Name = "ShardInfoControl";
            this.Size = new System.Drawing.Size(491, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private GhostLib.Gui.NSGroupBox gamebx;
        private GhostLib.Gui.NSGroupBox forumbx;
        private GhostLib.Gui.NSGroupBox storebx;
        private GhostLib.Gui.NSGroupBox webbx;
        private System.Windows.Forms.Timer timer1;




    }
}
