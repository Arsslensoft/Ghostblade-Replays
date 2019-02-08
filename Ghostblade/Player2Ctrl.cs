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
using System.IO;
using GBReplay.Replays.Riot;

namespace Ghostblade
{

    public partial class Player2Ctrl : UserControl
    {
        string champplayed;
        void SetItem(PictureBox img, int item)
        {
            if(File.Exists(Application.StartupPath + @"\Items\" + item.ToString() + ".png"))
            img.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Items\" + item.ToString() + ".png");
        }
        RawStatDTO GetRawStat(string name, PlayerParticipantStatsSummary p)
        {
            foreach (RawStatDTO rd in p.Statistics)
                if (name == rd.StatTypeName)
                    return rd;

            return null;
        }
       
        public void LoadPlayer(PlayerParticipantStatsSummary p, string name, string champion, ref TeamStats ts)
        {


            double k, d, a;
            RawStatDTO rd = GetRawStat("LEVEL", p);
            if(rd != null)
                this.summoner.Text = rd.Value.ToString() + ". "+ name;
            else
            this.summoner.Text = name;
            k =GetRawStat("CHAMPIONS_KILLED", p).Value;
            d = GetRawStat("NUM_DEATHS", p).Value;
            a = GetRawStat("ASSISTS", p).Value;
           
            ts.Kill += (int)k;
            ts.Deaths += (int)d;
            ts.Assists += (int)a;

            KDAlb.Text = k.ToString() + " / " + d.ToString() + " / " + a.ToString();
            champplayed = champion;
            if (File.Exists(Application.StartupPath + @"\Champions\" +champion + ".png"))
                this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champion + ".png");
            else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
          
            this.Spell1.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Spells\" + p.Spell1Id.ToString() + ".png");
            this.Spell2.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Spells\" + p.Spell2Id.ToString() + ".png");
            SetItem(item1, (int)GetRawStat("ITEM0", p).Value);
            SetItem(item2, (int)GetRawStat("ITEM1", p).Value);
            SetItem(item3, (int)GetRawStat("ITEM2", p).Value);
            SetItem(item4, (int)GetRawStat("ITEM3", p).Value);
            SetItem(item5, (int)GetRawStat("ITEM4", p).Value);
            SetItem(item6, (int)GetRawStat("ITEM5", p).Value);
            SetItem(item7, (int)GetRawStat("ITEM6", p).Value);

            // Gold
            rd = GetRawStat("GOLD_EARNED", p);
            if (rd != null)
                this.goldlb.Text =GoldToString(rd.Value);
            else
                this.goldlb.Text = "$ 0";
            // Creeps
            int totalfarm = 0;
            rd = GetRawStat("MINIONS_KILLED", p);
            if (rd != null)
            {
                totalfarm += (int)rd.Value;
                this.creepslb.Text = rd.Value.ToString();
                ts.CREEPS += (int)rd.Value;
            }
            else
                this.creepslb.Text = "0";


            rd = GetRawStat("NEUTRAL_MINIONS_KILLED", p);
            if (rd != null)
            {
            
                ts.CREEPS += (int)rd.Value;
                totalfarm += (int)rd.Value;
                this.creepslb.Text =    totalfarm.ToString();
            }
        
            // Ward placed
            rd = GetRawStat("WARD_PLACED", p);
            if (rd != null)
            {
                this.wardplacedlb.Text = rd.Value.ToString();
                ts.WP += (int)rd.Value;
            }
            else
                this.wardplacedlb.Text = "0";
            // Ward killed
            rd = GetRawStat("WARD_KILLED", p);
            if (rd != null)
            {
                this.wardkilledlb.Text = rd.Value.ToString();
                ts.WD += (int)rd.Value;
            }
            else
                this.wardkilledlb.Text = "0";
            // Turrets destoryed
            rd = GetRawStat("TURRETS_KILLED", p);
            if (rd != null)
            {
                this.Turretdestlb.Text = rd.Value.ToString();
                ts.TD += (int)rd.Value;
            }
            else
                this.Turretdestlb.Text = "0";
            // Dmg taken
            rd = GetRawStat("TOTAL_DAMAGE_TAKEN", p);
            if (rd != null)
                this.dmgtakenlb.Text = DmgToString(rd.Value);
            else
                this.dmgtakenlb.Text = "0";

            // Dmg dealt
            rd = GetRawStat("TOTAL_DAMAGE_DEALT", p);
            if (rd != null)
                this.dmgdealtlb.Text = DmgToString(rd.Value);
            else
                this.dmgdealtlb.Text = "0";

            // Largest Killing spree
            double           lks = 0;
            rd = GetRawStat("LARGEST_KILLING_SPREE", p);
            if (rd != null)
               lks = rd.Value;
            else
                lks = 0;
            // Largest Multi Kill
            double lmk = 0;
            rd = GetRawStat("LARGEST_MULTI_KILL", p);
            if (rd != null)
                lmk = rd.Value;
            else
                lmk = 0;

            lmkslb.Text = lmk.ToString() + " | " + lks.ToString();
            this.Refresh();

        }
        string GoldToString(double g)
        {
            if (g > 1000)
                return String.Format("${0:0.0} K", g / 1000);
            else return "$"+g.ToString();
        }
        string DmgToString(double g)
        {
            if (g > 1000)
                return String.Format("{0:0.00} K", g / 1000);
            else return g.ToString();
        }
        public void LoadPlayer(string player, string champion, int spell1, int spell2)
        {

            this.summoner.Text = player;
            champplayed = champion;
            if (File.Exists(Application.StartupPath + @"\Champions\" + champion + ".png"))
                this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champion + ".png");
            else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
            this.Spell1.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Spells\" + spell1.ToString() + ".png");
            this.Spell2.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Spells\" + spell2.ToString() + ".png");


        
            this.Refresh();
        
        }
        public Player2Ctrl(bool red)
        {
            InitializeComponent();

            if (red)
            {
                summoner.Style = MetroFramework.MetroColorStyle.Red;
                KDAlb.Style = MetroFramework.MetroColorStyle.Red;
                goldlb.Style = MetroFramework.MetroColorStyle.Red;
                creepslb.Style = MetroFramework.MetroColorStyle.Red;
                wardplacedlb.Style = MetroFramework.MetroColorStyle.Red;
                wardkilledlb.Style = MetroFramework.MetroColorStyle.Red;
                dmgdealtlb.Style = MetroFramework.MetroColorStyle.Red;
                dmgtakenlb.Style = MetroFramework.MetroColorStyle.Red;
                lmkslb.Style = MetroFramework.MetroColorStyle.Red;
                Turretdestlb.Style = MetroFramework.MetroColorStyle.Red;
              
            }
                  
          
            
        }
       
        private void Champion_Click(object sender, EventArgs e)
        {
            Program.MainFormInstance.ShowChampInfo(CurrentGame.GetChampionID( champplayed));
        }

        private void Champion_MouseEnter(object sender, EventArgs e)
        {
            Helper.GetToolTip(Champion, "Champion info", "Click here to see champion infos");
        }

        private void summoner_MouseEnter(object sender, EventArgs e)
        {
            Helper.GetToolTip(summoner, "Summoner", "Summoners name + in-game level");
        }
    }
}
