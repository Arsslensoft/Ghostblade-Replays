namespace Ghostblade
{
    partial class Player
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.championname = new MetroFramework.Controls.MetroLink();
            this.KDA = new MetroFramework.Controls.MetroLink();
            this.wins = new MetroFramework.Controls.MetroLink();
            this.rankedwins = new MetroFramework.Controls.MetroLink();
            this.Rank = new System.Windows.Forms.PictureBox();
            this.Spell2 = new System.Windows.Forms.PictureBox();
            this.Spell1 = new System.Windows.Forms.PictureBox();
            this.Champion = new System.Windows.Forms.PictureBox();
            this.Ranklabel = new MetroFramework.Controls.MetroLink();
            this.summoner = new MetroFramework.Controls.MetroLink();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Rank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Spell2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Spell1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.championname);
            this.panel3.Controls.Add(this.KDA);
            this.panel3.Controls.Add(this.wins);
            this.panel3.Controls.Add(this.rankedwins);
            this.panel3.Controls.Add(this.Rank);
            this.panel3.Controls.Add(this.Spell2);
            this.panel3.Controls.Add(this.Spell1);
            this.panel3.Controls.Add(this.Champion);
            this.panel3.Controls.Add(this.Ranklabel);
            this.panel3.Controls.Add(this.summoner);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(972, 39);
            this.panel3.TabIndex = 7;
            // 
            // championname
            // 
            this.championname.Location = new System.Drawing.Point(258, 9);
            this.championname.Name = "championname";
            this.championname.Size = new System.Drawing.Size(105, 23);
            this.championname.Style = MetroFramework.MetroColorStyle.Blue;
            this.championname.TabIndex = 11;
            this.championname.Text = "Champion";
            this.championname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.championname.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.championname.UseSelectable = true;
            this.championname.UseStyleColors = true;
            // 
            // KDA
            // 
            this.KDA.Location = new System.Drawing.Point(817, 9);
            this.KDA.Name = "KDA";
            this.KDA.Size = new System.Drawing.Size(152, 23);
            this.KDA.Style = MetroFramework.MetroColorStyle.Blue;
            this.KDA.TabIndex = 10;
            this.KDA.Text = "0";
            this.KDA.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.KDA.UseSelectable = true;
            this.KDA.UseStyleColors = true;
            // 
            // wins
            // 
            this.wins.Location = new System.Drawing.Point(674, 9);
            this.wins.Name = "wins";
            this.wins.Size = new System.Drawing.Size(152, 23);
            this.wins.Style = MetroFramework.MetroColorStyle.Blue;
            this.wins.TabIndex = 9;
            this.wins.Text = "0";
            this.wins.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.wins.UseSelectable = true;
            this.wins.UseStyleColors = true;
            // 
            // rankedwins
            // 
            this.rankedwins.Location = new System.Drawing.Point(530, 9);
            this.rankedwins.Name = "rankedwins";
            this.rankedwins.Size = new System.Drawing.Size(169, 23);
            this.rankedwins.Style = MetroFramework.MetroColorStyle.Blue;
            this.rankedwins.TabIndex = 8;
            this.rankedwins.Text = "0 / 0";
            this.rankedwins.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.rankedwins.UseSelectable = true;
            this.rankedwins.UseStyleColors = true;
            // 
            // Rank
            // 
            this.Rank.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Rank.Location = new System.Drawing.Point(369, 4);
            this.Rank.Name = "Rank";
            this.Rank.Size = new System.Drawing.Size(32, 32);
            this.Rank.TabIndex = 7;
            this.Rank.TabStop = false;
            // 
            // Spell2
            // 
            this.Spell2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Spell2.Location = new System.Drawing.Point(236, 20);
            this.Spell2.Name = "Spell2";
            this.Spell2.Size = new System.Drawing.Size(16, 16);
            this.Spell2.TabIndex = 6;
            this.Spell2.TabStop = false;
            // 
            // Spell1
            // 
            this.Spell1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Spell1.Location = new System.Drawing.Point(236, 4);
            this.Spell1.Name = "Spell1";
            this.Spell1.Size = new System.Drawing.Size(16, 16);
            this.Spell1.TabIndex = 5;
            this.Spell1.TabStop = false;
            // 
            // Champion
            // 
            this.Champion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Champion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Champion.Location = new System.Drawing.Point(194, 4);
            this.Champion.Name = "Champion";
            this.Champion.Size = new System.Drawing.Size(32, 32);
            this.Champion.TabIndex = 4;
            this.Champion.TabStop = false;
            this.Champion.Click += new System.EventHandler(this.Champion_Click);
            // 
            // Ranklabel
            // 
            this.Ranklabel.Location = new System.Drawing.Point(369, 9);
            this.Ranklabel.Name = "Ranklabel";
            this.Ranklabel.Size = new System.Drawing.Size(199, 23);
            this.Ranklabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.Ranklabel.TabIndex = 2;
            this.Ranklabel.Text = "Season Rank";
            this.Ranklabel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Ranklabel.UseSelectable = true;
            this.Ranklabel.UseStyleColors = true;
            // 
            // summoner
            // 
            this.summoner.Location = new System.Drawing.Point(3, 9);
            this.summoner.Name = "summoner";
            this.summoner.Size = new System.Drawing.Size(169, 23);
            this.summoner.Style = MetroFramework.MetroColorStyle.Blue;
            this.summoner.TabIndex = 0;
            this.summoner.Text = "Player 1";
            this.summoner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.summoner.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.summoner.UseSelectable = true;
            this.summoner.UseStyleColors = true;
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel3);
            this.Name = "Player";
            this.Size = new System.Drawing.Size(972, 39);
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Rank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Spell2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Spell1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Champion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox Rank;
        private System.Windows.Forms.PictureBox Spell2;
        private System.Windows.Forms.PictureBox Spell1;
        private System.Windows.Forms.PictureBox Champion;
        private MetroFramework.Controls.MetroLink Ranklabel;
        private MetroFramework.Controls.MetroLink summoner;
        private MetroFramework.Controls.MetroLink wins;
        private MetroFramework.Controls.MetroLink rankedwins;
        private MetroFramework.Controls.MetroLink KDA;
        private MetroFramework.Controls.MetroLink championname;
    }
}
