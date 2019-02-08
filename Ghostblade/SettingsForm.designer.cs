namespace Ghostblade
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.settingsControl1 = new Ghostblade.SettingsControl();
            this.SuspendLayout();
            // 
            // settingsControl1
            // 
            this.settingsControl1.BackColor = System.Drawing.Color.Transparent;
            this.settingsControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.settingsControl1.Location = new System.Drawing.Point(8, 37);
            this.settingsControl1.Name = "settingsControl1";
            this.settingsControl1.Size = new System.Drawing.Size(941, 601);
            this.settingsControl1.TabIndex = 0;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 646);
            this.Controls.Add(this.settingsControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MenuIconInnerBorder = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Ghostblade Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private SettingsControl settingsControl1;

    }
}