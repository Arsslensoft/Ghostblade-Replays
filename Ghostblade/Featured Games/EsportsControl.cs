using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RiotSharp.LolEsportsEndPoint;
using RiotSharp;
using System.Diagnostics;

namespace Ghostblade
{
   
    public partial class EsportsControl : UserControl
    {
        public EsportsControl()
        {
            InitializeComponent();
        }
        internal static bool IsAlreadyChecking = false;
        public void LoadEsportsAsync()
        {
            if (!IsAlreadyChecking)
            {
                MethodInvoker mtd = new MethodInvoker(LoadEsports);
                mtd.BeginInvoke(null, null);
            }
        }

        List<ScheduleControl> ctrls = new List<ScheduleControl>();
        public void LoadEsports()
        {
            try
            {
                // clear controls
                IsAlreadyChecking = true;
                this.BeginInvoke(new MethodInvoker(delegate
                {
              
                foreach (Control c in ctrls)
                           ScheduleHostPanel.Controls.Remove(c);
                    ctrls.Clear();

                }));
                // seek all tourneys
                Tourneys ts = EsportsRiotApi.GetInstance().GetTourneys();
                List<Match> ml = new List<Match>();
                foreach(Tourney t in ts.TourneysList)
                {
                  if(!t.isFinished)
                    {
                        Schedule s = EsportsRiotApi.GetInstance().GetSchedule(t.id);
                        foreach (Match m in s.Matches)
                            if (m.IsFinished == "0" && m.Contestants != null)
                                ml.Add(m);
                            
                        
                    }
                }
               
             foreach (Match m in ml.OrderBy(x => x.DateTime))
                {
                  
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            ScheduleControl ctrl = new ScheduleControl();
                            ctrl.Dock = DockStyle.Top;
                            if (ctrl.Load(m))
                            {
                                ScheduleHostPanel.Controls.Add(ctrl);
                                ctrls.Add(ctrl);
                            }
                        }));
                    
                }

            }
            catch
            {

            }
            IsAlreadyChecking = false;

        }
        private void EsportsControl_Resize(object sender, EventArgs e)
        {
            ScheduleHostPanel.Height = this.Height - 32;
            metroLink1.Location = new Point(this.Width - 105, 3);
            metroLink2.Location = new Point(this.Width - 213, 3);
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            Process.Start("http://watch.euw.lolesports.com/en_GB#/livestream");
            LoadEsportsAsync();
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\TwitchStream.exe", "c=riotgames");
        }
    }
}
