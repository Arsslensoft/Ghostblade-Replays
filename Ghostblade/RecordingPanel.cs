using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GhostLib.Gui;
using RiotSharp.MatchEndpoint;
using GhostLib;
using GBNet;
using MetroFramework;
using System.Diagnostics;
using System.IO;
using GhostLib.Network;

namespace Ghostblade
{
    partial class RecordingPanel : UserControl
    {
         public string CurrentChampion = "unknown";
        public bool CanSpectate { get; set; }
        internal ReplayTask RepTask;

        public RecordingPanel()
        {
            InitializeComponent();
            CanSpectate = false;
            rep_chunkid = 0;
            rep_keyid = 1;
        }
      
        
        public void Record(long gameid, string platform, string key, string server, string player, RootObject game, long sid)
        {

            try
            {
                string champ = FindChamp(sid, game, out player);
                CurrentChampion = champ;
                if (champ != null)
                {
                    if (File.Exists(Application.StartupPath + @"\Champions\" + champ + ".png"))
                        this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champ + ".png");
                    else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
                }
                playerlabel.Text = player;
                RepTask = new ReplayTask(gameid, platform.ToUpper(), key, server, player);
                RepTask.RecordGui = this;
                RepTask.Record();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to record", ex);
            }

        }
        public void Record(long gameid, string platform, string key, string server, string player, string champ)
        {

            try
            {
                if (champ != null)
                {
                    if (File.Exists(Application.StartupPath + @"\Champions\" + champ + ".png"))
                        this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champ + ".png");
                    else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
                }
                CurrentChampion = champ;
                playerlabel.Text = player;
                RepTask = new ReplayTask(gameid, platform.ToUpper(), key, server, player);
                RepTask.RecordGui = this;
                RepTask.Record();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to record", ex);
            }
        }
        public void Record(long gameid, string platform, string key, string server, string player)
        {

            try
            {
                CurrentChampion = "unknown";
                RepTask = new ReplayTask(gameid, platform.ToUpper(), key, server, player);
                RepTask.RecordGui = this;
                RepTask.Record();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to record", ex);
            }

        }
        string FindChamp(long gi, RootObject game, out string name)
        {
            name = "";
            foreach (Participant p in game.participants)
                if (p.SummonerId == gi)
                {
                    name = p.Name;
                    return GhostRessources.Instance.GetChampion(p.ChampionId);

                }
            return null;
        }
        public void Record(long summonerid, RootObject game, string platform)
        {
            try
            {
                string player = null;
                string champ = FindChamp(summonerid, game, out player);
                CurrentChampion = champ;
                if (champ != null)
                {
                    if (File.Exists(Application.StartupPath + @"\Champions\" + champ + ".png"))
                        this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + champ + ".png");
                    else this.Champion.BackgroundImage = Ghostblade.Properties.Resources.Unknown;
                
                    playerlabel.Text = player;
                }
                RepTask = new ReplayTask(game.gameId, game.platformId.ToUpper(), game.observers.encryptionKey, RiotTool.Servers[platform.ToUpper()], player);
                RepTask.RecordGui = this;
                RepTask.Record();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to record", ex);
            }
        }



        public void UpdateStatus(DownloadTask t, string status)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    if (t.IsChunk)
                        rep_chunkid = t.Id;
                    else
                        rep_keyid = t.Id;

                    if (progspin.Style == MetroColorStyle.Red)
                        progspin.Style = MetroColorStyle.Default;
                 
                    if (rep_chunkid >= progspin.Maximum)
                        progspin.Maximum += 50;

                    progspin.Value = rep_chunkid;
                    Replaylb.Text = status;

                  
                    specbtn.Visible = !Program.MainFormInstance.IsInGame;
                    this.Refresh();
                }));
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Warn("FAILED to update status", ex);
            }
        }
        int rep_chunkid, rep_keyid;
        public void UpdateStatusA()
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    Replaylb.Text = "Problem Occured. Looking for solutions";
                    progspin.Style = MetroColorStyle.Red;
                    this.Refresh();
                }));
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Warn("FAILED to update status attempt", ex);
            }
        }
        public void UpdateStatusA(DownloadTask task, int attemp)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {

                    progspin.Style = MetroColorStyle.Red;
                    Replaylb.Text = "Chunk : " + task.Id.ToString() + " [Attempt " + attemp.ToString() + "]";
                    specbtn.Visible = !Program.MainFormInstance.IsInGame;
                    this.Refresh();
                }));
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Warn("FAILED to update status attempt", ex);
            }
        }


        private void specbtn_Click(object sender, EventArgs e)
        {
            try
            {

                if (Process.GetProcessesByName("League of Legends").Length != 0)
                {
                    MetroMessageBox.Show(Program.MainFormInstance, "Could not open League of Legends.\nAnother instance is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
            //    string[] details = new string[2] { RepTask.GameID.ToString(), RepTask.Platform };


                RiotTool.Instance.LaunchGame(RepTask.Platform == "PBE1", RepTask.GameID.ToString(), RepTask.Server, RepTask.Key, RepTask.Platform);
                //Process p = new System.Diagnostics.Process();
                //if(RepTask.Platform != "PBE1")
                //p.StartInfo.WorkingDirectory = RiotTool.Instance.GetLatestGameReleaseDeploy();
                //else p.StartInfo.WorkingDirectory = RiotTool.Instance.GetLatestGameReleaseDeploy(SettingsManager.Settings.PbeDirectory);

                //if (RepTask.Platform != "PBE1")
                //p.StartInfo.FileName = RiotTool.Instance.GameExecutable;
                //else p.StartInfo.FileName = Path.Combine(RiotTool.Instance.GetLatestGameReleaseDeploy(SettingsManager.Settings.PbeDirectory), "League of Legends.exe");

                //p.StartInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
                //    + RepTask.Server + " "
                //    + RepTask.Key + " "
                //    + details[0] + " "
                //    + details[1] + "\"";

                //p.Start();
                //GhostOverlay.ShowOverlay(p.StartInfo.WorkingDirectory);

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Warn("Failed to spectate record", ex);
            }

        }
        private void cancelbtn_Click(object sender, EventArgs e)
        {
            try
            {


                RepTask.recorder.Recording = false;
                Program.MainFormInstance.BeginInvoke(new UpdateReplayRecorded(Program.MainFormInstance.CancelReplayRecorded), RepTask, this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Warn("Failed to cancel recording", ex);
            }
        }


        private void RecordingPanel_Resize(object sender, EventArgs e)
        {
            panel3.Location = new Point(3, 3);
            panel3.Width = this.Size.Width - 6;
            panel3.Height = this.Height  -6;
        }

        private void progspin_MouseEnter(object sender, EventArgs e)
        {
            string Thirdmsg = "";
            if (NetworkManager.DefaultProxy is NetworkInterfaceProxy )
                Thirdmsg = "Interface IP: " + (NetworkManager.DefaultProxy as GhostLib.Network.NetworkInterfaceProxy).ExternalIp.ToString() + " via " + (NetworkManager.DefaultProxy as GhostLib.Network.NetworkInterfaceProxy).Interface.Name;
            
            Helper.GetToolTip(progspin, "Status", string.Format("{0} Chunks, {1} Keys downloaded so far. \n{2}", rep_chunkid, rep_keyid, Thirdmsg));
        }

        private void Champion_Click(object sender, EventArgs e)
        {
            Program.MainFormInstance.ShowChampInfo(CurrentGame.GetChampionID(CurrentChampion));
        }

    }
    internal delegate void UpdateReplayRecorded(ReplayTask rep, RecordingPanel rec);
    internal delegate void UpdateReplayRecording(long GameId, string server, string enc, string platform);
}
