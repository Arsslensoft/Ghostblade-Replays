namespace GhostBase
{
    partial class UpdaterForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statlb = new MetroFramework.Controls.MetroLabel();
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.cancelbtn = new MetroFramework.Controls.MetroButton();
            this.notesbox = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // statlb
            // 
            this.statlb.AutoSize = true;
            this.statlb.Location = new System.Drawing.Point(9, 300);
            this.statlb.Name = "statlb";
            this.statlb.Size = new System.Drawing.Size(52, 19);
            this.statlb.Style = MetroFramework.MetroColorStyle.Blue;
            this.statlb.TabIndex = 0;
            this.statlb.Text = "Status...";
            this.statlb.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroProgressBar1
            // 
            this.metroProgressBar1.Location = new System.Drawing.Point(9, 322);
            this.metroProgressBar1.Name = "metroProgressBar1";
            this.metroProgressBar1.Size = new System.Drawing.Size(739, 15);
            this.metroProgressBar1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroProgressBar1.TabIndex = 1;
            this.metroProgressBar1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // cancelbtn
            // 
            this.cancelbtn.Location = new System.Drawing.Point(625, 370);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.Style = MetroFramework.MetroColorStyle.Blue;
            this.cancelbtn.TabIndex = 2;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cancelbtn.UseSelectable = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // notesbox
            // 
            this.notesbox.Lines = new string[] {
        "No Patch Notes"};
            this.notesbox.Location = new System.Drawing.Point(23, 63);
            this.notesbox.MaxLength = 32767;
            this.notesbox.Multiline = true;
            this.notesbox.Name = "notesbox";
            this.notesbox.PasswordChar = '\0';
            this.notesbox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.notesbox.SelectedText = "";
            this.notesbox.Size = new System.Drawing.Size(716, 234);
            this.notesbox.TabIndex = 4;
            this.notesbox.Text = "No Patch Notes";
            this.notesbox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.notesbox.UseSelectable = true;
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 403);
            this.Controls.Add(this.notesbox);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.metroProgressBar1);
            this.Controls.Add(this.statlb);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdaterForm";
            this.Text = "Ghostblade Update";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Shown += new System.EventHandler(this.UpdaterForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel statlb;
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
        private MetroFramework.Controls.MetroButton cancelbtn;
        private MetroFramework.Controls.MetroTextBox notesbox;
    }
}