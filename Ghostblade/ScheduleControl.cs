using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RiotSharp;

namespace Ghostblade
{
    public partial class ScheduleControl : UserControl
    {
        public ScheduleControl()
        {
            InitializeComponent();
        }
        public bool Load(RiotSharp.LolEsportsEndPoint.Match m)
        {
            try {
                gamebx.SubTitle = m.Tournament.Name + " [Round " + m.Tournament.Round + "]       " +((m.DateTime.Year == 1970)?"Soon": m.DateTime.ToString()) +"        "+ ((m.IsLive) ? "Live" : "");
                gamebx.Title = "Best of " + m.MaxGames + " games";
                TEAM1PIC.BackgroundImage = new Bitmap(EsportsRiotApi.GetInstance().DownloadIcon(m.Contestants.Blue.LogoURL, Application.StartupPath + @"\Icons\"+ m.Contestants.Blue.Acronym + ".png"));
                TEAM2PIC.BackgroundImage = new Bitmap(EsportsRiotApi.GetInstance().DownloadIcon(m.Contestants.Red.LogoURL, Application.StartupPath + @"\Icons\" + m.Contestants.Red.Acronym + ".png"));
                TEAM1LB.Text = m.Contestants.Blue.Name;
                TEAM2LB.Text = m.Contestants.Red.Name;
                int ml = this.Width / 2;
                TEAM1PIC.Location = new Point(100, 47);
                TEAM1LB.Location = new Point(180, 66);
                VSLB.Location = new Point(ml - 17, 63);
                TEAM2PIC.Location = new Point(this.Width - 165, 47);
                TEAM2LB.Location = new Point(this.Width - 187 - (TEAM2LB.Text.Length * 10), 66);
                return true;
            }
            catch
            {
                return false;

            }
        }
        private void ScheduleControl_Resize(object sender, EventArgs e)
        {
            try {
                int m = this.Width / 2;
                TEAM1PIC.Location = new Point(100, 47);
                TEAM1LB.Location = new Point(180, 66);
                VSLB.Location = new Point(m - 17, 63);
                TEAM2PIC.Location = new Point(this.Width - 165, 47);
                TEAM2LB.Location = new Point(this.Width - 187 - (TEAM2LB.Text.Length * 10), 66);
            }
            catch
            {

            }
               

        }
    }
}
