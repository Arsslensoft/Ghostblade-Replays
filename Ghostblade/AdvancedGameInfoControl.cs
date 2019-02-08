using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GBReplay.Replays.Riot;
using GBReplay.Replays.Game;
using RiotSharp.MatchEndpoint;
using GhostLib;
using System.IO;

namespace Ghostblade
{
    public partial class AdvancedGameInfoControl : UserControl
    {
        List<double> kills = new List<double>();
        List<double> gold = new List<double>();
        List<double> totdmg = new List<double>();
        List<double> wards = new List<double>();
        List<string> summoners = new List<string>();
        int explodedidx = 0;
        DataRow CreateStatsRow(DataTable dt, string name)
        {
            DataRow rw = dt.NewRow();
            rw[0] = name;
       
            return rw;
        }
        void LoadStats()
        {
            DataColumn dc = new DataColumn();
           
            DataTable dt = new DataTable();
            dt.Columns.Add(" ", typeof(string));
            int idx = 1;
            List<DataRow> rows = new List<DataRow>();

            foreach (PlayerParticipantStatsSummary p in EOG.TeamPlayerParticipantStats)
              dt.Columns.Add(p.SummonerName, typeof(string));
           


            foreach (PlayerParticipantStatsSummary p in EOG.OtherTeamPlayerParticipantStats)
                dt.Columns.Add(p.SummonerName, typeof(string));

           
            
            // Create Rows
           rows.Add( CreateStatsRow(dt, "Champion"));
             rows.Add(  CreateStatsRow(dt, "KDA"));
           rows.Add(    CreateStatsRow(dt, "Largest Killing Spree"));
            rows.Add(   CreateStatsRow(dt, "Largest Multi Kill"));

             rows.Add(  CreateStatsRow(dt, "Total Damage Dealt to Champions"));
            rows.Add(   CreateStatsRow(dt, "Physical Damage Dealt to Champions"));
            rows.Add(   CreateStatsRow(dt, "Magic Damage Dealt to Champions"));
            rows.Add(   CreateStatsRow(dt, "True Damage Dealt to Champions"));
            rows.Add(   CreateStatsRow(dt, "Total Damage Dealt"));
           rows.Add(    CreateStatsRow(dt, "Largest Critical Strike"));



          rows.Add(     CreateStatsRow(dt, "Total Heals"));
           rows.Add(    CreateStatsRow(dt, "Physical Damage Taken"));
           rows.Add(    CreateStatsRow(dt, "Magic Damage Taken"));
           rows.Add(    CreateStatsRow(dt, "True Damage Taken"));
           rows.Add(    CreateStatsRow(dt, "Total Damage Taken"));

           rows.Add(    CreateStatsRow(dt, "Wards Placed"));
           rows.Add(    CreateStatsRow(dt, "Wards Killed"));
           rows.Add(    CreateStatsRow(dt, "Towers Killed"));

             rows.Add(  CreateStatsRow(dt, "Total Time Dead"));
            rows.Add(   CreateStatsRow(dt, "Total Crowd Control Dealt"));

            rows.Add(   CreateStatsRow(dt, "Gold Earned"));
           rows.Add(    CreateStatsRow(dt, "Minions Killed"));
             rows.Add(  CreateStatsRow(dt, "Neutral Minions Killed"));
           rows.Add(    CreateStatsRow(dt, "Neutral Minions Killed in Team Jungle"));
           rows.Add(    CreateStatsRow(dt, "Neutral Minions Killed in Enemy Jungle"));




           foreach (PlayerParticipantStatsSummary p in EOG.TeamPlayerParticipantStats)
           {
               FillGrid(rows, p, idx);
               idx++;
           }



           foreach (PlayerParticipantStatsSummary p in EOG.OtherTeamPlayerParticipantStats)
           {
               FillGrid(rows, p, idx);
               idx++;
           }

          // Add rows
            foreach (DataRow rw in rows)
               dt.Rows.Add(rw);

           rows.Clear();
           rows = null;
            metroGrid1.DataSource = dt;
            metroGrid1.Columns[0].Width = 200;
            foreach (DataGridViewColumn column in metroGrid1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        RawStatDTO GetRawStat(string name, PlayerParticipantStatsSummary p)
        {
            foreach (RawStatDTO rd in p.Statistics)
                if (name == rd.StatTypeName)
                    return rd;

            return null;
        }
        string GoldToString(double g)
        {
            if (g > 1000)
                return String.Format("${0:0.0} K", g / 1000);
            else return "$" + g.ToString();
        }
   
        string GetRawStatString(string name, PlayerParticipantStatsSummary p, string type = "normal")
        {
            RawStatDTO dt = GetRawStat(name, p);
            if (dt == null)
                return "-";
             
            else if (type == "gold")
              return  GoldToString(dt.Value);
            else if (type == "time")
            {
                TimeSpan ts =new TimeSpan(0,0,(int)dt.Value);
              return string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds);
            }
                   else   return dt.Value.ToString("N0");
           

        }
        void FillGrid(List<DataRow> rows,PlayerParticipantStatsSummary p,int idx)
        {
       
          kills.Add( GetRawStat("CHAMPIONS_KILLED", p).Value);
          gold.Add( GetRawStat("GOLD_EARNED", p).Value);
         totdmg.Add( GetRawStat("TOTAL_DAMAGE_DEALT", p).Value);
            wards.Add( GetRawStat("WARD_PLACED", p).Value);
            if (p.SummonerName == PI.SummonerName)
                explodedidx = summoners.Count ;
            summoners.Add(p.SummonerName + "(" + p.SkinName + ")");
       

                rows[0][idx] = p.SkinName;
                rows[1][idx] = GetRawStat("CHAMPIONS_KILLED", p).Value + "/" + GetRawStat("NUM_DEATHS", p).Value + "/" + GetRawStat("ASSISTS", p).Value;
                rows[2][idx] = GetRawStatString("LARGEST_KILLING_SPREE", p);
                rows[3][idx] = GetRawStatString("LARGEST_MULTI_KILL", p);


                rows[4][idx] = GetRawStatString("TOTAL_DAMAGE_DEALT_TO_CHAMPIONS", p);
                rows[5][idx] = GetRawStatString("PHYSICAL_DAMAGE_DEALT_TO_CHAMPIONS", p);
                rows[6][idx] = GetRawStatString("MAGIC_DAMAGE_DEALT_TO_CHAMPIONS", p);
                rows[7][idx] = GetRawStatString("TRUE_DAMAGE_DEALT_TO_CHAMPIONS", p);
                rows[8][idx] = GetRawStatString("TOTAL_DAMAGE_DEALT", p);
                rows[9][idx] = GetRawStatString("LARGEST_CRITICAL_STRIKE", p);


                rows[10][idx] = GetRawStatString("TOTAL_HEAL", p);
                rows[11][idx] = GetRawStatString("PHYSICAL_DAMAGE_TAKEN", p);
                rows[12][idx] = GetRawStatString("MAGIC_DAMAGE_TAKEN", p);
                rows[13][idx] = GetRawStatString("TRUE_DAMAGE_TAKEN", p);
                rows[14][idx] = GetRawStatString("TOTAL_DAMAGE_TAKEN", p);

                rows[15][idx] = GetRawStatString("WARD_PLACED", p);
                rows[16][idx] = GetRawStatString("WARD_KILLED", p);
                rows[17][idx] = GetRawStatString("TURRETS_KILLED", p);

                rows[18][idx] = GetRawStatString("TOTAL_TIME_SPENT_DEAD", p,"time");
                rows[19][idx] = GetRawStatString("TOTAL_TIME_CROWD_CONTROL_DEALT", p, "time");

                rows[20][idx] = GetRawStatString("GOLD_EARNED", p,"gold");
                rows[21][idx] = GetRawStatString("MINIONS_KILLED", p);
                rows[22][idx] = GetRawStatString("NEUTRAL_MINIONS_KILLED", p);
                rows[23][idx] = GetRawStatString("NEUTRAL_MINIONS_KILLED_YOUR_JUNGLE", p);
                rows[24][idx] = GetRawStatString("NEUTRAL_MINIONS_KILLED_ENEMY_JUNGLE", p);
        }
        void LoadCharts(string player)
        {
            // SET STYLES
             killchart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            killchart.Series[0]["PieLabelStyle"] = "Outside";
            killchart.Series[0]["DoughnutRadius"] = "70";
            killchart.Series[0]["PieDrawingStyle"] = "SoftEdge";
          


            goldchart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            goldchart.Series[0]["PieLabelStyle"] = "Outside";
            goldchart.Series[0]["DoughnutRadius"] = "70";
           goldchart.Series[0]["PieDrawingStyle"] = "SoftEdge";
     

            totdmgchart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            totdmgchart.Series[0]["PieLabelStyle"] = "Outside";
            totdmgchart.Series[0]["DoughnutRadius"] = "70";
            totdmgchart.Series[0]["PieDrawingStyle"] = "SoftEdge";

            wardschart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            wardschart.Series[0]["PieLabelStyle"] = "Outside";
            wardschart.Series[0]["DoughnutRadius"] = "70";
            wardschart.Series[0]["PieDrawingStyle"] = "SoftEdge";

            killchart.Series[0].Points.DataBindXY(summoners, kills);
            goldchart.Series[0].Points.DataBindXY(summoners, gold);
            totdmgchart.Series[0].Points.DataBindXY(summoners, totdmg);
            wardschart.Series[0].Points.DataBindXY(summoners, wards);

            totdmgchart.Series[0].Points[explodedidx]["Exploded"] = "true";
            goldchart.Series[0].Points[explodedidx]["Exploded"] = "true";
            killchart.Series[0].Points[explodedidx]["Exploded"] = "true";
            wardschart.Series[0].Points[explodedidx]["Exploded"] = "true";
        }

        Dictionary<int, string> parts = new Dictionary<int, string>();
        string GetParticipant(int id)
        {
            if (parts.ContainsKey(id))
                return parts[id];
            else return "Unknown";
        }
        string GetItem(RiotSharp.StaticDataEndpoint.ItemListStatic it, int id)
        {
            if (it.Items.ContainsKey(id))
                return it.Items[id].Name;
            else return "Item " + id;
        }
        string GetSkill(int id)
        {
            switch (id)
            {
                case 1:
                    return "Q";
                case 2:
                    return "W";
                case 3:
                    return "E";
                case 4:
                    return "R";
                default:return id.ToString();
            }
        }
        string FindParticipant(int cid, int tid)
        {
            string champion = CurrentGame.GetChampion(cid);
            if (tid == 100)
            {
                foreach (PlayerParticipantStatsSummary p in EOG.TeamPlayerParticipantStats)
                    if (p.SkinName == champion)
                        return p.SummonerName;

                return champion +  " in Blue Team";
            }
            else
            {
                foreach (PlayerParticipantStatsSummary p in EOG.OtherTeamPlayerParticipantStats)
                    if (p.SkinName == champion)
                        return p.SummonerName;

                return champion + " in Blue Team";
            }
        }
        void LoadEvents()
        {
            try
            {
                string cachefile = Application.StartupPath + @"\Temp\"+ META.gameKey.gameId+"-"+META.gameKey.platformId+".cached";
               

                StringBuilder sb = new StringBuilder();
                RiotSharp.StaticDataEndpoint.ItemListStatic items = RiotSharp.StaticRiotApi.GetInstance(SettingsManager.Settings.ApiKey).GetItems(RiotTool.PlatformToRegion(META.gameKey.platformId));
                RiotSharp.MatchEndpoint.MatchDetail m = Program.MainFormInstance.API.GetMatch(RiotTool.PlatformToRegion(META.gameKey.platformId),META.gameKey.gameId ,true,cachefile,true);
                foreach (Participant p in m.Participants)
                    parts.Add(p.ParticipantId, FindParticipant(p.ChampionId, p.TeamId));


                foreach (Frame f in m.Timeline.Frames)
                {
                    sb.AppendLine("***" + string.Format("{0:0}:{1:00}", f.Timestamp.Minutes, f.Timestamp.Seconds) + "***********************************************************************************");
                    if (f.Events != null)
                    {
                        foreach (Event ev in f.Events)
                            sb.AppendLine(EventToString(ev, items));
                    }
                }
            metroTabControl1 .Invoke(new MethodInvoker(delegate
                {
                    nsTextBox1.Text = sb.ToString();
                 
                    metroTabControl1.TabPages.Add(metroTabPage4);
                    metroTabControl1.SelectedIndex = 0;
                }));
            sb.Length = 0;
            sb = null;
            }
            catch
            {
              
            }
        }
        string EventToString(Event ev, RiotSharp.StaticDataEndpoint.ItemListStatic items)
        {
         
            switch (ev.EventType)
            {
                case EventType.ItemPurchased:

                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Item Event]     " + GetParticipant(ev.ParticipantId) + " purchased " + GetItem(items, ev.ItemId);
                case EventType.ItemSold:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Item Event]     " + GetParticipant(ev.ParticipantId) + " sold " + GetItem(items, ev.ItemId);
                case EventType.ItemDestroyed:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Item Event]     " + GetParticipant(ev.ParticipantId) + " destroyed " + GetItem(items, ev.ItemId);
                case EventType.ItemUndo:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Item Event]     " + GetParticipant(ev.ParticipantId) + " undid " + GetItem(items, ev.ItemId);

                case EventType.SkillLevelUp:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Skill Level Up Event]     " + GetParticipant(ev.ParticipantId) + " leveled up his " + GetSkill(ev.SkillSlot);
                case EventType.WardKill:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Ward Kill Event]     " + GetParticipant(ev.KillerId) + " killed a " + ev.WardType.Value.ToString();
                case EventType.WardPlaced:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Ward Placed Event]     " + GetParticipant(ev.CreatorId) + " placed a " + ev.WardType.Value.ToString();
                case EventType.EliteMonsterKill:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Elite Monster Kill Event]     " + GetParticipant(ev.KillerId) + " has slained " + ev.MonsterType.Value.ToString();
                case EventType.ChampionKill:
                    string assist = "";
                    if (ev.AssistingParticipantIds != null)
                    {
                        foreach (int aid in ev.AssistingParticipantIds)
                            assist += GetParticipant(aid) + ", ";
                        assist = (assist == "") ? "" : "Assisted by " + assist.Remove(assist.Length - 3, 2);
                    }
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Champion Kill Event]     " + GetParticipant(ev.KillerId) + " has slained " + GetParticipant(ev.VictimId) + " "+assist;

                case EventType.BuildingKill:
                    return "[ " + string.Format("{0:0}:{1:00}", ev.Timestamp.Minutes, ev.Timestamp.Seconds) + "  |Building Kill Event]     " + ((ev.TeamId == 100) ? "Red Team has destoryed " : "Blue Team has destoryed ") + ev.LaneType.Value.ToString() + " " + ev.BuildingType.ToString() + ((ev.TowerType.Value == TowerType.UndefinedTurret) ? "" : " (" + ev.TowerType.Value + ")") + " by " + GetParticipant(ev.KillerId);

                default :
                    return "";

            }
        }

        EndOfGameStats EOG;
        GameMetaData META;
        BasicInfo PI; 
        bool BWINS;
        public void LoadGame(EndOfGameStats eog, GameMetaData meta, BasicInfo pi, bool bluewin)
        {
            EOG = eog;
            META = meta;
            PI = pi;
            BWINS = bluewin;
            gameInfoControl1.LoadGame(eog, meta, pi, bluewin);
            LoadStats();
            LoadCharts(pi.SummonerName);
            MethodInvoker mi = new MethodInvoker(LoadEvents);
            mi.BeginInvoke(null, null);

        }
        public AdvancedGameInfoControl()
        {
            InitializeComponent();
        }
    }
 
}
