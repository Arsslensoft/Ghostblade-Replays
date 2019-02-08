using System;
using System.Windows.Forms;

namespace GhostLib.Gui
{
    /// <summary>
    /// Form for relaying a simple message to the user
    /// </summary>
    internal partial class InfoForm : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Make a form for relaying a simple message to the usre
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="owner">The owning form</param>
        public InfoForm(string text, Form owner)
        {
            Owner = owner;
            InitializeComponent();
            label1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}