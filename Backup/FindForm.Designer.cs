namespace SearchableControls
{
    /// <summary>
    /// A simple dialog to find a supplied text string
    /// </summary>
    partial class FindForm
    {
        /// <summary>
        /// Required designer variable
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
            this.searchButton = new System.Windows.Forms.Button();
            this.ignoreCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.searchTypeComboBox = new System.Windows.Forms.ComboBox();
            this.searchHistoryComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.replaceHistoryComboBox = new System.Windows.Forms.ComboBox();
            this.replaceButton = new System.Windows.Forms.Button();
            this.replaceAllButton = new System.Windows.Forms.Button();
            this.replaceModeCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(300, 24);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(79, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "&Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            this.searchButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // ignoreCaseCheckBox
            // 
            this.ignoreCaseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ignoreCaseCheckBox.AutoSize = true;
            this.ignoreCaseCheckBox.Checked = true;
            this.ignoreCaseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ignoreCaseCheckBox.Location = new System.Drawing.Point(11, 102);
            this.ignoreCaseCheckBox.Name = "ignoreCaseCheckBox";
            this.ignoreCaseCheckBox.Size = new System.Drawing.Size(82, 17);
            this.ignoreCaseCheckBox.TabIndex = 3;
            this.ignoreCaseCheckBox.Text = "Ignore &case";
            this.ignoreCaseCheckBox.UseVisualStyleBackColor = true;
            this.ignoreCaseCheckBox.CheckedChanged += new System.EventHandler(this.ignoreCaseCheckBox_CheckedChanged);
            this.ignoreCaseCheckBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // searchTypeComboBox
            // 
            this.searchTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchTypeComboBox.FormattingEnabled = true;
            this.searchTypeComboBox.Location = new System.Drawing.Point(90, 100);
            this.searchTypeComboBox.Name = "searchTypeComboBox";
            this.searchTypeComboBox.Size = new System.Drawing.Size(119, 21);
            this.searchTypeComboBox.TabIndex = 4;
            this.searchTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.searchTypeComboBox_SelectedIndexChanged);
            this.searchTypeComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // searchHistoryComboBox
            // 
            this.searchHistoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.searchHistoryComboBox.FormattingEnabled = true;
            this.searchHistoryComboBox.Location = new System.Drawing.Point(6, 26);
            this.searchHistoryComboBox.Name = "searchHistoryComboBox";
            this.searchHistoryComboBox.Size = new System.Drawing.Size(288, 21);
            this.searchHistoryComboBox.TabIndex = 5;
            this.searchHistoryComboBox.TextChanged += new System.EventHandler(this.searchHistoryComboBox_TextChanged);
            this.searchHistoryComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Find text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Replace with";
            // 
            // replaceHistoryComboBox
            // 
            this.replaceHistoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceHistoryComboBox.FormattingEnabled = true;
            this.replaceHistoryComboBox.Location = new System.Drawing.Point(6, 64);
            this.replaceHistoryComboBox.Name = "replaceHistoryComboBox";
            this.replaceHistoryComboBox.Size = new System.Drawing.Size(288, 21);
            this.replaceHistoryComboBox.TabIndex = 7;
            // 
            // replaceButton
            // 
            this.replaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceButton.Location = new System.Drawing.Point(300, 62);
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Size = new System.Drawing.Size(79, 23);
            this.replaceButton.TabIndex = 2;
            this.replaceButton.Text = "&Replace";
            this.replaceButton.UseVisualStyleBackColor = true;
            this.replaceButton.Click += new System.EventHandler(this.replaceButton_Click);
            this.replaceButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // replaceAllButton
            // 
            this.replaceAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceAllButton.Location = new System.Drawing.Point(300, 100);
            this.replaceAllButton.Name = "replaceAllButton";
            this.replaceAllButton.Size = new System.Drawing.Size(79, 23);
            this.replaceAllButton.TabIndex = 2;
            this.replaceAllButton.Text = "Replace &All";
            this.replaceAllButton.UseVisualStyleBackColor = true;
            this.replaceAllButton.Click += new System.EventHandler(this.replaceAllButton_Click);
            this.replaceAllButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // replaceModeCheckBox
            // 
            this.replaceModeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceModeCheckBox.AutoSize = true;
            this.replaceModeCheckBox.Checked = true;
            this.replaceModeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.replaceModeCheckBox.Location = new System.Drawing.Point(319, 5);
            this.replaceModeCheckBox.Name = "replaceModeCheckBox";
            this.replaceModeCheckBox.Size = new System.Drawing.Size(66, 17);
            this.replaceModeCheckBox.TabIndex = 9;
            this.replaceModeCheckBox.Text = "Replace";
            this.replaceModeCheckBox.UseVisualStyleBackColor = true;
            this.replaceModeCheckBox.CheckedChanged += new System.EventHandler(this.replaceModeCheckBox_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(215, 100);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(79, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            this.cancelButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            // 
            // FindForm
            // 
            this.AcceptButton = this.searchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 130);
            this.Controls.Add(this.replaceModeCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.replaceHistoryComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchHistoryComboBox);
            this.Controls.Add(this.searchTypeComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.replaceAllButton);
            this.Controls.Add(this.replaceButton);
            this.Controls.Add(this.ignoreCaseCheckBox);
            this.Controls.Add(this.searchButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 199);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(392, 26);
            this.Name = "FindForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Find Text";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindDialog_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.CheckBox ignoreCaseCheckBox;
        private System.Windows.Forms.ComboBox searchTypeComboBox;
        private System.Windows.Forms.ComboBox searchHistoryComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox replaceHistoryComboBox;
        private System.Windows.Forms.Button replaceButton;
        private System.Windows.Forms.Button replaceAllButton;
        private System.Windows.Forms.CheckBox replaceModeCheckBox;
        private System.Windows.Forms.Button cancelButton;

    }
}