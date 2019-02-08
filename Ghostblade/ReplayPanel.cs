using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GhostLib.Gui;
using GBReplay.Replays.Riot;
using System.Diagnostics;
using GhostLib;
using MetroFramework;
using GBReplay.Replays;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using MetroFramework.Forms;

namespace Ghostblade
{
    partial class ReplayPanel : ReplayBorderPanel
    {

        public bool GameWon { get; set; }
        string champion= "";

        Process sv;
       internal GhostReplay rep;
        EndOfGameStats stats;
        bool IsPlaying { get; set; }
        public void PlayReplay(GhostReplay rep)
        {
            foreach (Process proc in Process.GetProcessesByName("GhostReplay"))
                proc.Kill();
            foreach (Process proc in Process.GetProcessesByName("League of Legends"))
                proc.Kill();

        
            if (SettingsManager.Settings.HasPBE &&   (rep.Platform == "PBE1"))
            {
      
                    VersionResult vr = RiotTool.Instance.CompareVersions(SettingsManager.Settings.PbeVersion, rep.GameVersion);
                
                    if (vr == VersionResult.PREDATED && vr == VersionResult.OUTDATED)
                    {
                        if (MetroMessageBox.Show(Program.MainFormInstance, "The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.PbeVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                            return;
                    }
                    else if(vr == VersionResult.PATCHED)
                    {
                        if (MetroMessageBox.Show(Program.MainFormInstance, "The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.PbeVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return;
                    }
                
            }
            else 
            {

             
                    VersionResult vr = RiotTool.Instance.CompareVersions(SettingsManager.Settings.GameVersion, rep.GameVersion);

                    if (vr == VersionResult.PREDATED && vr == VersionResult.OUTDATED)
                    {
                        if (MetroMessageBox.Show(Program.MainFormInstance, "The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.GameVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                            return;
                    }
                    else if (vr == VersionResult.PATCHED)
                    {
                        if (MetroMessageBox.Show(Program.MainFormInstance, "The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.GameVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return;
                    }
                
            }

            ProcessStartInfo Replay = new ProcessStartInfo();
            Replay.FileName = Application.StartupPath + @"\GhostReplay.exe";


            //      gr.Combine();

            if (SettingsManager.Settings.HasPBE && (rep.Platform == "PBE1"))
                Replay.Arguments = rep.MetaData.gameKey.gameId.ToString() + " " + rep.MetaData.gameKey.platformId + " \"" + SettingsManager.Settings.PbeDirectory + "\"" + " \"" + RiotTool.Instance.GetLatestGameReleaseDeploy(SettingsManager.Settings.PbeDirectory) + "\" \"" + rep.FileName + "\"";
            else 
                Replay.Arguments = rep.MetaData.gameKey.gameId.ToString() + " " + rep.MetaData.gameKey.platformId + " \"" + RiotTool.Instance.GameDirectory + "\"" + " \"" + RiotTool.Instance.GetLatestGameReleaseDeploy() + "\" \"" + rep.FileName + "\"";
            sv = Process.Start(Replay);
        }

        string FindSkinName(string player)
        {
            if (stats != null)
            {
                foreach (PlayerParticipantStatsSummary p in stats.TeamPlayerParticipantStats)
                    if (p.SummonerName == player)
                        return p.SkinName;

                foreach (PlayerParticipantStatsSummary p in stats.OtherTeamPlayerParticipantStats)
                    if (p.SummonerName == player)
                        return p.SkinName;
            }

            return null;
        }
        bool bluewins = true;
        RawStatDTO GetRawStat(string name, PlayerParticipantStatsSummary p)
        {
            foreach (RawStatDTO rd in p.Statistics)
                if (name == rd.StatTypeName)
                    return rd;

            return null;
        }
        bool IsWon(string player)
        {

            foreach (PlayerParticipantStatsSummary p in stats.TeamPlayerParticipantStats)
                if (p.SummonerName == player)
                {
                    RawStatDTO raw = GetRawStat("WIN", p);
                    if (raw != null)
                        return (raw.Value == 1);

                }


            foreach (PlayerParticipantStatsSummary p in stats.OtherTeamPlayerParticipantStats)
                if (p.SummonerName == player)
                {
                    RawStatDTO raw = GetRawStat("WIN", p);
                    if (raw != null)
                        return (raw.Value == 1);

                }
            return false;
        }
        bool IsWon(int teamid)
        {
            if (teamid == 100)
            {
                RawStatDTO raw = null;
                if( stats.TeamPlayerParticipantStats.Count > 0)
                   raw = GetRawStat("WIN", stats.TeamPlayerParticipantStats[0]);
                if (raw == null)
                {
                    if (stats.OtherTeamPlayerParticipantStats.Count > 0)
                       raw = GetRawStat("WIN", stats.OtherTeamPlayerParticipantStats[0]);
                    if (raw != null)
                        return (raw.Value != 1);
                    else return false; // default
                }
                else return (raw.Value == 1);
            }
            else
            {
                   RawStatDTO raw = null;
                   if (stats.OtherTeamPlayerParticipantStats.Count > 0)
                raw = GetRawStat("WIN", stats.OtherTeamPlayerParticipantStats[0]);
                if (raw != null)
                    return (raw.Value == 1);
                else return false; // default
            }
        }
        PlayerParticipantStatsSummary GetPlayer(string player)
        {

            foreach (PlayerParticipantStatsSummary p in stats.TeamPlayerParticipantStats)
                if (p.SummonerName == player)
                    return p;


            foreach (PlayerParticipantStatsSummary p in stats.OtherTeamPlayerParticipantStats)
                if (p.SummonerName == player)
                    return p;
            return null;
        }
        int cid = -1;
        public ReplayPanel(GhostReplay replay)
         : base(replay.Flags)
        {
            try
            {


                rep = replay;
                IsPBE = (rep.Platform == "PBE1");
                InitializeComponent();
                metroLabel1.Text = replay.RecordDate.ToShortDateString() + " " + replay.RecordDate.ToShortTimeString();
                stats = replay.GameStats;

                if (stats != null)
                {
                    bluewins = IsWon(100);
                    string dur = string.Format("{0:0}:{1:00}", rep.GameLength.Minutes, rep.GameLength.Seconds);
                    if (rep.SummonerName != "")
                    {
                        if (IsWon(rep.SummonerName))
                        {

                            GameWon = true;
                            maplb.Text = stats.GameMode + "  - " + dur + " - Win";
                        }
                        else
                        {
                            GameWon = false;
                            maplb.Text = stats.GameMode + "  - " + dur + " - Loss";
                            maplb.Style = MetroColorStyle.Red;

                        }
                    }
                    else
                    {
                        GameWon = false;
                        if (IsWon(100))
                            maplb.Text = stats.GameMode + "  - " + dur + " - Blue Team Wins";
                        else
                            maplb.Text = stats.GameMode + "  - " + dur + " - Red Team Wins";

                    }
                    if (replay.SummonerName != null)
                    {
                        champion = FindSkinName(replay.SummonerName);
                        if (champion != null)
                        {
                            Replaylb.Text = replay.SummonerName + " - " + champion;
                            cid = CurrentGame.GetChampionID(champion);
                          
                                if (File.Exists(Application.StartupPath + @"\Champions\" + champion + ".png"))
                                    this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champion + ".png");
                                else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
                            
                        }
                        else
                        {
                            Replaylb.Text = replay.SummonerName;

                        }
                    }
                    else
                    {
                        champion = FindSkinName(Program.MainFormInstance.SelectedAccount.SummonerName);
                        if (champion != null)
                        {

                            Replaylb.Text = Program.MainFormInstance.SelectedAccount.SummonerName + " - " + champion;
                            cid = CurrentGame.GetChampionID(champion);
                            if (File.Exists(Application.StartupPath + @"\Champions\" + champion + ".png"))
                                this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champion + ".png");
                            else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
                        }
                        else
                            Replaylb.Text = "Unknown";
                    }
                }
                gameidlb.Text = replay.MetaData.gameKey.gameId.ToString();
                if (replay.Platform == "PBE1" && SettingsManager.Settings.HasPBE)
                {

                    versionlb.Style = MetroColorStyle.Orange;
                    VersionResult vr = RiotTool.Instance.CompareVersions(SettingsManager.Settings.PbeVersion, rep.GameVersion);

                    if (vr == VersionResult.OUTDATED)
                        versionlb.Style = MetroColorStyle.Red;
                    else if (vr == VersionResult.PREDATED)
                        versionlb.Style = MetroColorStyle.Blue;
                    else if (vr == VersionResult.OK)
                        versionlb.Style = MetroColorStyle.Green;
                }
                else
                {
                    versionlb.Style = MetroColorStyle.Orange;
                    VersionResult vr = RiotTool.Instance.CompareVersions(SettingsManager.Settings.GameVersion, rep.GameVersion);

                    if (vr == VersionResult.OUTDATED)
                        versionlb.Style = MetroColorStyle.Red;
                    else if (vr == VersionResult.PREDATED)
                        versionlb.Style = MetroColorStyle.Blue;
                    else if (vr == VersionResult.OK)
                        versionlb.Style = MetroColorStyle.Green;
                }

                versionlb.Text = replay.GameVersion;

                if ((rep.Flags & ReplayInfo.BaronReplays) == ReplayInfo.BaronReplays)
                    Replaylb.Text += " - [BR]";
                else if ((rep.Flags & ReplayInfo.LolReplay) == ReplayInfo.LolReplay)
                    Replaylb.Text += " - [LR]";
                else if ((rep.Flags & ReplayInfo.UnknownRecorder) == ReplayInfo.UnknownRecorder)
                    Replaylb.Text += " - [UR]";


            }
            catch (Exception ex)
            {

            }
        }




        UserControl gm;
        private void ReplayCtrl_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
          
            
                    if (SettingsManager.Settings.AdvancedGameInfo)
                    {
                        gm = new AdvancedGameInfoControl();
                        ((AdvancedGameInfoControl)gm).LoadGame(stats, rep.MetaData, rep.PlayerInfos, bluewins);
                    }
                    else
                    {
                        gm = new GameInfoControl();
                       ((GameInfoControl) gm).LoadGame(stats, rep.MetaData, rep.PlayerInfos, bluewins);
                    }
               
               
                MetroTaskWindow.ShowTaskWindow(Program.MainFormInstance, rep.MetaData.gameKey.gameId.ToString(), gm, 60);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to load game info Window", ex);
            }
        }

        private void Link_MouseClick(object sender, EventArgs e)
        {
            try
            {
                if ((rep.Flags & ReplayInfo.Signed) == ReplayInfo.Signed)
                {
                    rep.Signer = new ReplaySignature(rep.Header.Certificate.Data);
                    if (rep.Verify())
                    {
                        X509Certificate2 cert = new X509Certificate2(rep.Header.Certificate.Data);


                        System.Security.Cryptography.X509Certificates.X509Certificate2UI.DisplayCertificate(cert, this.Handle);
                    }
                    else MessageBox.Show("The replay was modified [SIGNATURE MISMATCH]", "Digital Signature", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed toShow certificate", ex);
            }
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {

                File.Delete(rep.FileName);

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to delete replay file " + rep.FileName, ex);
            }
        }

        private void Champion_Click(object sender, EventArgs e)
        {
            Program.MainFormInstance.ShowChampInfo( CurrentGame
            .GetChampionID(champion));
        }


        private void ReplayPanel_Resize(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(3, 3);
                panel1.Width = this.Size.Width - 6;
                panel1.Height = this.Height - 6;
            }
            catch
            {

            }
        }


        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                PlayReplay(rep);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to play Replay", ex);
            }
        }

        private void metroButton1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && MessageBox.Show("Do you want to stream this replay ?", "Stream", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    Program.MainFormInstance.AddStream(rep, Replaylb.Text, Champion.BackgroundImage,cid);
            }
            catch
            {

            }
        }
    }
    
    class ReplayBorderPanel : GPanel
    {
 
        internal bool IsPBE = false;
        public ReplayBorderPanel(ReplayInfo flags)
        {
             if ((flags & ReplayInfo.PBE) == ReplayInfo.PBE)
            {
                P1 = new Pen(Color.FromArgb(0, 0, 255));
                P2 = new Pen(Color.FromArgb(20, 20, 255));
            }
            else if ((flags & ReplayInfo.Signed) == ReplayInfo.Signed)
            {
                P1 = new Pen(Color.FromArgb(0, 255, 0));
                P2 = new Pen(Color.FromArgb(20, 255, 20));
            }
            
            else
            {
                P1 = new Pen(Color.FromArgb(255, 0, 0));
                P2 = new Pen(Color.FromArgb(255, 20, 20));
            }
        }


    }
}
