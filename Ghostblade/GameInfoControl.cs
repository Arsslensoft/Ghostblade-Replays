using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RiotSharp.MatchEndpoint;
using GhostLib;
using GBReplay.Replays.Riot;
using GBReplay.Replays.Game;


namespace Ghostblade
{
    public partial class GameInfoControl : UserControl
    {
        public GameInfoControl()
        {

        
            InitializeComponent();
        
        }
        RawStatDTO GetRawStat(string name, PlayerParticipantStatsSummary p)
        {
            foreach (RawStatDTO rd in p.Statistics)
                if (name == rd.StatTypeName)
                    return rd;

            return null;
        }
        public void LoadGame(EndOfGameStats eog,GameMetaData meta, BasicInfo pi, bool bluewin)
        {
            try
            {
             //   RiotSharp.MatchEndpoint.MatchDetail m = Program.MainFormInstance.API.GetMatch(RiotTool.PlatformToRegion(meta.gameKey.platformId), (long)eog.GameId);
       
                TeamStats blueteam = new TeamStats();
                TeamStats redteam = new TeamStats();
                BluePanel.Controls.Clear();
                RedPanel.Controls.Clear();
                if (!bluewin)
                {
                    metroLabel1.Text = "Red Team Wins";
                    metroLabel1.Style = MetroFramework.MetroColorStyle.Red;
                }
            if(meta.gameKey.platformId != "PBE1")
                ginfo.Text = RiotTool.ToMapString((RiotSharp.MapType)pi.Map) + ", " + RiotTool.ToQueueString((QueueType)pi.Queue) + " - " + RiotTool.PlatformToRegion(meta.gameKey.platformId).ToString().ToUpper();
            else ginfo.Text = eog.GameMode+ ", " + eog.GameType.Replace("_"," ") + " - " + RiotTool.PlatformToRegion(meta.gameKey.platformId).ToString().ToUpper();
                if (eog != null)
                {
                   
                    foreach (PlayerParticipantStatsSummary p in eog.TeamPlayerParticipantStats)
                    {
                        Player2Ctrl p2 = new Player2Ctrl(p.TeamId == 200);
                    
                        p2.LoadPlayer(p, p.SummonerName, p.SkinName, ref blueteam);
                        p2.Dock = DockStyle.Top;
                        if (p.TeamId == 100)
                            BluePanel.Controls.Add(p2);
                        else RedPanel.Controls.Add(p2);
                    }
                    foreach (PlayerParticipantStatsSummary p in eog.OtherTeamPlayerParticipantStats)
                    {
                        Player2Ctrl p2 = new Player2Ctrl(p.TeamId == 200);
                     
                        p2.LoadPlayer(p, p.SummonerName, p.SkinName, ref redteam);
                        p2.Dock = DockStyle.Top;
                        if (p.TeamId == 100)
                            BluePanel.Controls.Add(p2);
                        else RedPanel.Controls.Add(p2);
                    }
                }

                bluewk.Text = blueteam.WD.ToString();
                bluewp.Text = blueteam.WP.ToString();
                bluecreeps.Text = blueteam.CREEPS.ToString();
                bluekda.Text = blueteam.Kill + " / " + blueteam.Deaths + " / " + blueteam.Assists;
                bluetd.Text = blueteam.TD.ToString();

                redwk.Text = redteam.WD.ToString();
                redwp.Text = redteam.WP.ToString();
                redcreeps.Text = redteam.CREEPS.ToString();
                redkda.Text = redteam.Kill + " / " + redteam.Deaths + " / " + redteam.Assists;
                redtd.Text = redteam.TD.ToString();

                BluePanel.Controls.Add(panel5);
                RedPanel.Controls.Add(panel4);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to load game", ex);
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }
    }

  public  class TeamStats
    {
        public int Kill =0;
        public int Deaths=0;
        public int Assists=0;

        public int WP=0;
        public int WD=0;
        public int TD=0;

        public int CREEPS=0;
    }
}
