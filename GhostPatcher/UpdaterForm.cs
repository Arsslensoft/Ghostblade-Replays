using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GhostBase
{
    public partial class UpdaterForm : MetroForm
    {
        public UpdaterForm()
        {
            InitializeComponent();
            GhostbladeInstance.DataDragonInstance.OnProgress += DataDragonInstance_OnProgress;
            GhostbladeInstance.DataDragonInstance.OnStatusMessage += DataDragonInstance_OnStatusMessage;
        }
        StringBuilder ddsb = new StringBuilder();
        void DataDragonInstance_OnProgress(int prog)
        {
            try
            {
                metroProgressBar1.Invoke(new MethodInvoker(delegate
                {
                    metroProgressBar1.Value = prog;
                    metroProgressBar1.Refresh();
                }));
            }
            catch
            {

            }
        }
        void DataDragonInstance_OnStatusMessage(string msg)
        {
            try
            {
                ddsb.AppendLine(msg);
                statlb.Invoke(new MethodInvoker(delegate
                {
                    notesbox.Text = ddsb.ToString();
                    statlb.Text = msg;
                    statlb.Refresh();
                    notesbox.Refresh();
                }));
            }
            catch
            {

            }
        }

        Thread thr;
        internal string version = null;
        void StartUpdate()
        {
            if (GhostbladeInstance.DataDragonInstance.UpdateAll(version))
                Thread.Sleep(3000);
          
                this.Invoke(new MethodInvoker(delegate
             {
                 this.Close();
             }));

            
        }
        private void UpdaterForm_Shown(object sender, EventArgs e)
        {
            thr = new Thread(new ThreadStart(StartUpdate));
            thr.Start();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Do you want to cancel the update ? \nthis process may harm Ghostblade files.", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                thr.Abort();
                this.Close();
            }
            
        }
    }
}
