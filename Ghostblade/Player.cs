using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using RiotSharp.LeagueEndpoint;
using GhostLib;
using System.IO;

namespace Ghostblade
{
    public partial class Player : MetroUserControl
    {
        public void LoadSummonerData(LeaguePlayer pl)
        {
            this.summoner.Text = pl.SummonerName;
            if (pl.Level == 30)
            {
                if (pl.RankedLeague != null)
                    this.Ranklabel.Text = pl.Rank + " " + pl.RankedLeague.Division + " (" + pl.RankedLeague.LeaguePoints.ToString() + " LP)";
                else
                    Ranklabel.Text = pl.Rank;
            }
            else Ranklabel.Text = "Level "+ pl.Level.ToString();
            this.Rank.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Rank\" + pl.Rank.ToUpper() + ".png");
            if (File.Exists(Application.StartupPath + @"\Champions\" + pl.Champion + ".png"))
                this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + pl.Champion + ".png");
            else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
            championname.Text = pl.Champion;
            this.Spell1.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Spells\" + pl.Spell1.ToString() + ".png");
            this.Spell2.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Spells\" + pl.Spell2.ToString() + ".png");
            if (pl.ChampStat != null)
                this.KDA.Text = ((double)pl.ChampStat.Stats.TotalChampionKills / pl.ChampStat.Stats.TotalSessionsPlayed).ToString("0.0") + " / " + ((double)pl.ChampStat.Stats.TotalDeathsPerSession / pl.ChampStat.Stats.TotalSessionsPlayed).ToString("0.0") + " / " + ((double)pl.ChampStat.Stats.TotalAssists / pl.ChampStat.Stats.TotalSessionsPlayed).ToString("0.0");
            else this.KDA.Text = "0 / 0 / 0";
            
               
                wins.Text = pl.Wins;
                rankedwins.Text = pl.RankedWins + " / " + pl.RankedLosses;
           
            this.Refresh();
        }
        public Player(bool red)
        {
            InitializeComponent();
            if (red)
            {
                summoner.Style = MetroFramework.MetroColorStyle.Red;
                Ranklabel.Style = MetroFramework.MetroColorStyle.Red;
                wins.Style = MetroFramework.MetroColorStyle.Red;
                rankedwins.Style = MetroFramework.MetroColorStyle.Red;
                KDA.Style = MetroFramework.MetroColorStyle.Red;
                championname.Style = MetroFramework.MetroColorStyle.Red;
            }
        }

        private void Champion_Click(object sender, EventArgs e)
        {
            Program.MainFormInstance.ShowChampInfo(CurrentGame.GetChampionID(championname.Text));
        }
    }
}
