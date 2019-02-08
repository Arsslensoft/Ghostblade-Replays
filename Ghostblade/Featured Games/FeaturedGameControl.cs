using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RiotSharp.Featured;
using GhostLib;
using RiotSharp.MatchEndpoint;

namespace Ghostblade
{
    public partial class FeaturedGameControl : UserControl
    {
        public FeaturedGameControl()
        {
            InitializeComponent();
           
        }
        DateTime starttime = DateTime.Now;
        public GameList CurrentGame { get; set; }

        public bool IsSelected
        {
            get { return timer1.Enabled; }
            set { timer1.Enabled = value; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Program.MainFormInstance.Visible)
                {
                    timelb.BeginInvoke(new MethodInvoker(delegate
                    {
                        TimeSpan ts = DateTime.Now.Subtract(starttime);
                      

                        timelb.Text = string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds);

                    }));
                }
            }
            catch
            {

            }
        }
        public void AddPlayer(bool red, string name, int champ)
        {
            FeaturedPlayer fp = new FeaturedPlayer(red);
            fp.Load(name, champ);
            fp.Dock = DockStyle.Top;
            if (red)
                redteam.Controls.Add(fp);
            else blueteam.Controls.Add(fp);
        }
   
        public void LoadFeatured(GameList game)
        {
            try
            {
                CurrentGame = game;
                ginfo.Text = RiotTool.ToMapString((RiotSharp.MapType)game.mapId) + ", " + RiotTool.ToQueueString((RiotSharp.MatchEndpoint.QueueType)game.gameQueueConfigId) + " - " + RiotTool.PlatformToString(game.platformId);
                TimeSpan ts = new TimeSpan(game.gameLength * TimeSpan.TicksPerSecond);

                starttime = DateTime.Now.Subtract(ts);

                //              starttime = GetGameTime((long)game.gameStartTime, game.platformId.ToUpper());
               

                foreach (RiotSharp.Featured.Participant p in game.participants)
                    AddPlayer((p.teamId == 200),p.summonerName, p.championId);
                
                

            }
            catch
            {

            }
        }

        private void FeaturedGameControl_Resize(object sender, EventArgs e)
        {
            this.vspanel.Width = 40;
            vslb.Location = new Point(0, (vspanel.Height - 30) / 2);
            blueteam.Width = (this.Width-40) / 2;
           
        }

        private void ginfo_Click(object sender, EventArgs e)
        {
            try
            {

                    RiotSharp.Region lolregsharp = RiotTool.PlatformToRegion(CurrentGame.platformId.ToUpper());
                    long sid = Program.MainFormInstance.API.GetSummoner(lolregsharp, CurrentGame.participants[0].summonerName).Id;

                    RootObject rg = Program.MainFormInstance.API.GetCurrentGame(lolregsharp, sid, CurrentGame.platformId.ToUpper());
                    UpdateCurrentGameInvoker u = new UpdateCurrentGameInvoker(Program.MainFormInstance.currentGame1.LoadGame);
                    u.BeginInvoke(rg, Program.MainFormInstance.API, lolregsharp, 0, null, null);
                    Program.MainFormInstance.MainTabControl.SelectedTab = Program.MainFormInstance.GameInfoTab;
                
            }
            catch
            {

            }
        }


    }
}
