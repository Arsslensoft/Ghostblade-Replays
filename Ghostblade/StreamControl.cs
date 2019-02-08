using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GhostReplays;
using GhostLib;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using GhostLib.Gui;
using System.IO;
using GBReplay.Replays;
using System.Diagnostics;
using GhostLib.Network;

namespace Ghostblade
{
    partial class StreamControl : GPanel
    {
        GhostReplay replay;
        string cmd = null;
  
        int cid;
         public void SetUI(string player, Image champ, int id)
        {
             cid = id;
            playerlabel.Text = player;
            Champion.BackgroundImage = champ;
        }
        public void LoadAndStart(string gamepath, string deploy, GhostReplay rep)
        {
            replay = rep;
            deploy = deploy.Replace("/", "\\");
            StreamName = rep.GameId.ToString() + "-" + rep.Platform;
            try
            {
                using (SmartWebClient wbc = new SmartWebClient(4000))
                {

                    string ip = wbc.DownloadString("https://api.ipify.org/?format=text");
                    cmd = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
                 + ip + ":" + SettingsManager.Settings.GhostStreamPort.ToString() + " "
                 + rep.ObserverKey + " "
                 + rep.GameId.ToString() + " "
                 + rep.Platform + "\"";

                }
            }
            catch
            {
                cmd = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
                 + "127.0.0.1" + ":" + SettingsManager.Settings.GhostStreamPort.ToString() + " "
                 + rep.ObserverKey + " "
                 + rep.GameId.ToString() + " "
                 + rep.Platform + "\"";
            }
            if (!StreamManager.IsRuning)
                StreamManager.InitStream();


            StreamManager.AddStream(rep);
            //WatchProc = new System.Diagnostics.Process();
            //WatchProc.StartInfo.WorkingDirectory = deploy;
            //WatchProc.StartInfo.UseShellExecute = false;
            //WatchProc.StartInfo.FileName = Path.Combine(deploy, "League of Legends.exe");
        
            //WatchProc.StartInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
            //    + "127.0.0.1:"+SettingsManager.Settings.GhostStreamPort.ToString() + " "
            //    + rep.ObserverKey + " "
            //    + rep.GameId.ToString() + " "
            //    + rep.Platform + "\"";
        

        }
        public string StreamName { get; set; }

        public StreamControl()
        {
            InitializeComponent();


        
        }


        private void specbtn_Click(object sender, EventArgs e)
        {
            try
            {
                RiotTool.Instance.LaunchGame(replay.Platform == "PBE1", replay.GameId.ToString(), "127.0.0.1:" + SettingsManager.Settings.GhostStreamPort.ToString(), replay.ObserverKey, replay.Platform);
                //WatchProc.Start();
                //GhostOverlay.ShowOverlay(WatchProc.StartInfo.WorkingDirectory);
            }
            catch (Exception ex)
            {
                RiotTool.Instance.Log.Log.Error("Failed to watch", ex);
            }
        }

        public void CancelStream()
        {
            try
            {
                StreamManager.StopStream(StreamName);

                // request cancel
                Program.MainFormInstance.CancelStream(this);
            }
            catch (Exception ex)
            {
                RiotTool.Instance.Log.Log.Error("Failed to stop streaming", ex);
            }
        }
        private void cancelbtn_Click(object sender, EventArgs e)
        {
            CancelStream();
        }

        private void Replaylb_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText("\"" + RiotTool.Instance.InstalledGameProfile.LeagueExecutable + "\" " + cmd);
                MessageBox.Show("Stream command copied to clipboard", "Stream", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {

            }
        }

        private void Champion_Click(object sender, EventArgs e)
        {
           if(cid != -1)
            Program.MainFormInstance.ShowChampInfo(cid);
        }
    }

    public class StreamManager
    {
        static GhostReplays.GhostReplayServer sv;
        public static int StreamCount
        {
            get {
            if(sv != null)    
                return sv.Streams.Count; 
                else return 0;
            }
        }
        public static bool Forwarded { get; set; }
        public static Thread StreamingThread { get; set; }
        public static bool IsRuning
        {
            get
            {
                if (StreamingThread != null)
                    return StreamingThread.IsAlive;
                else return false;
            }
        }
        static void Listen()
        {
            try
            {
                if(SettingsManager.Settings.PortForwarding)
                   HttpServerManager.ForwardPort();
                Forwarded = true;
                sv.listen();
            }
            catch
            {

            }
        }
        public static void InitStream()
        {
            if (!IsRuning)
            {
               HttpServerManager.ListenPort =(ushort) SettingsManager.Settings.GhostStreamPort;
             sv = new GhostReplays.GhostReplayServer(SettingsManager.Settings.GhostStreamPort);
             Forwarded = false;
             StreamingThread = new Thread(new ThreadStart(Listen));
             Start();
            }
        }

        public static void Start()
        {
            if (!IsRuning && StreamingThread != null)
              StreamingThread.Start();
        }
        public static void Stop()
        {
            try
            {
                if (IsRuning)
                    StreamingThread.Abort();
            }
            catch
            {

            }
        }

        public static bool IsStreaming(long gid, string platform)
        {
            if (IsRuning)
                return sv.IsStreaming(gid.ToString() + "-" + platform);
            else return false;
        }
        public static void AddStream(GhostReplay rep)
        {
            sv.AddStream(rep);
        }
        public static void StopStream(string sid)
        {
            sv.RemoveStream(sid);
        }
    }
}
