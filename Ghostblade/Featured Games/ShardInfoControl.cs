using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostblade
{
    public partial class ShardInfoControl : UserControl
    {
        RiotSharp.StatusEndpoint.Service gamesvc;
        RiotSharp.StatusEndpoint.Service boardsvc;
        RiotSharp.StatusEndpoint.Service storesvc;
        RiotSharp.StatusEndpoint.Service wbsvc;
        public ShardInfoControl()
        {
            InitializeComponent();
            timer1.Enabled = true;
            //MethodInvoker mtd = new MethodInvoker(LoadShard);
            //mtd.BeginInvoke(null, null);
        }
       
        public void AddService(RiotSharp.StatusEndpoint.Service svc)
        {
            if(svc.Incidents.Count == 0)
                svc.Status = char.ToUpper(svc.Status[0]) + svc.Status.Remove(0, 1);
            else {svc.Status =char.ToUpper( svc.Status[0]) + svc.Status.Remove(0,1) + " with some incidents";

            }
            switch (svc.Slug)
            {

                case "game":
                    gamebx.SubTitle = svc.Status;
                    gamesvc = svc;
                    break;
                case "web":
                    webbx.SubTitle = svc.Status;
                    wbsvc = svc;
                    break;
                case "store":
                    storebx.SubTitle = svc.Status;
                    storesvc = svc;
                    break;
                case "forums":
                case "boards":
                    forumbx.SubTitle = svc.Status;
                    boardsvc = svc;
                    break;
            }
        }

        public void LoadShardStatus(RiotSharp.StatusEndpoint.ShardStatus ss)
        {
            foreach (RiotSharp.StatusEndpoint.Service svc in ss.Services)
                AddService(svc);
        }

        void LoadShard()
        {
            try
            {
                if (GhostLib.SettingsManager.Settings.Accounts[0].Region == RiotSharp.Region.kr)
                    return;

                RiotSharp.StatusRiotApi SAPI = RiotSharp.StatusRiotApi.GetInstance();
                RiotSharp.StatusEndpoint.ShardStatus ss = SAPI.GetShardStatus(      GhostLib.SettingsManager.Settings.Accounts[0].Region);
            
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    if(ss != null)
                    LoadShardStatus(ss);
                    //timer1.Enabled = false;
                }));

            }
            catch
            {

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Program.MainFormInstance.IsInGame)
            {
                timer1.Interval = 60000;
                MethodInvoker mtd = new MethodInvoker(LoadShard);
                mtd.BeginInvoke(null, null);
            }
            else timer1.Interval = 10000;
        }

        string GetStatus(RiotSharp.StatusEndpoint.Service svc)
        {
            if (svc == null)
                return "Service unavailable";
            StringBuilder sbstat = new StringBuilder();
            if (svc.Incidents.Count > 0)
            {
                sbstat.AppendLine("      Incidents :");
                foreach (RiotSharp.StatusEndpoint.Incident incd in svc.Incidents)
                {

                    if (incd.Active)
                        foreach (RiotSharp.StatusEndpoint.Message upd in incd.Updates)
                        {
                            sbstat.AppendLine("          " + upd.Author + " - " + upd.CreatedAt.ToString());
                            sbstat.AppendLine("              [" + upd.Severity.ToUpper() + "] " + upd.Content);
                        }


                }
            }
            else sbstat.AppendLine("No incident");

            return sbstat.ToString();
        }
        private void gamebx_MouseEnter(object sender, EventArgs e)
        {

            Helper.GetToolTip(gamebx, "Game", GetStatus(gamesvc));
        }

        private void forumbx_MouseEnter(object sender, EventArgs e)
        {
            Helper.GetToolTip(forumbx, "Forums", GetStatus(boardsvc));
        }

        private void storebx_MouseEnter(object sender, EventArgs e)
        {
            Helper.GetToolTip(storebx, "Store", GetStatus(storesvc));
        }

        private void webbx_MouseEnter(object sender, EventArgs e)
        {
            Helper.GetToolTip(webbx, "Website", GetStatus(wbsvc));
        }

    }
}
