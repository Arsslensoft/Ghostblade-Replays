using GhostLib;
using GBReplay;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GBReplay.Replays;
using GBReplay.Replays.Riot;
using System.Net;
using GBNet;

namespace Ghostblade
{

    internal delegate void UpdateInvoker();

    internal delegate void UpdateCurrentGameInvoker(RootObject rg, RiotSharp.RiotApi api, RiotSharp.Region reg, long sid);
   public class ReplayTask : IDisposable
    {
        public static string ReplayDir = SettingsManager.Settings.RecordingDirectory;
        public long GameID { get; set; }
        public string Server { get; set; }
        public string Platform { get; set; }
        public string Key { get; set; }
        internal RecordingPanel RecordGui { get; set; }
        public GhostReplay ReplayRecording { get; set; }
        public string Player { get; set; }
        public ReplayTask(long gID, string region, string key, string server, string player)
        {

            Key = key;
            Platform = region;
            Server = server;
            GameID = gID;
            Player = player;
            // Game Version
            string v = SettingsManager.Settings.GameVersion;
            if ((SettingsManager.Settings.GameVersion = RiotTool.Instance.GetGameVersion()) != null)
                SettingsManager.Save();
            else SettingsManager.Settings.GameVersion = v;

            // PBE Version
            v = SettingsManager.Settings.PbeVersion;
            if (SettingsManager.Settings.HasPBE && (SettingsManager.Settings.PbeVersion = RiotTool.Instance.GetGameVersion(RiotTool.Instance.GetLatestGameReleaseDeploy(SettingsManager.Settings.PbeDirectory) + @"\League of Legends.exe")) != null)
                SettingsManager.Save();
            else SettingsManager.Settings.PbeVersion = v;

            ReplayRecording = new GhostReplay(ReplayTask.ReplayDir + @"\Replays\" + GameID.ToString() + "-" + Platform + ".lgr", SettingsManager.Settings.GameVersion, true, Application.StartupPath + @"\Temp", File.ReadAllBytes(Application.StartupPath + @"\CS.pfx"), "GBCSKPAS1");
            ReplayRecording.IsPBE = (region == "PBE1") ;
        
            ReplayRecording.GameStats = new EndOfGameStats();
            if (player != null)
                ReplayRecording.SummonerName = player;
            else ReplayRecording.SummonerName = " ";

        }


        void recorder_OnReplayRecorded()
        {
            // SAVE REPLAY
            try
            {

                Program.MainFormInstance.BeginInvoke(new UpdateReplayRecorded(Program.MainFormInstance.ReplayRecorded), this, RecordGui);
 
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to trigger", ex);
            }
        }

        void recorder_OnGotChunk(DownloadTask t)
        {
            RecordGui.UpdateStatus(t, "Recording...");
        }
        internal ReplayRecorder recorder;

        void recorder_OnFailedToSave(Exception ex)
        {
            try
            {

                MetroFramework.MetroMessageBox.Show(Program.MainFormInstance, "Error : " + ex.Message + "\nUnable to save replay. \nMatch Detail Error : Riot Servers returned 404", "Failed to record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                recorder.Recording = false;
                Program.MainFormInstance.BeginInvoke(new UpdateReplayRecorded(Program.MainFormInstance.CancelReplayRecorded), null, this.RecordGui);


            }
            catch
            {

            }
        }
        void recorder_OnAttemptToDownload(DownloadTask t, int attemp)
        {
            RecordGui.UpdateStatusA(t, attemp);
        }
        void recorder_OnFailedToRecord(Exception ex)
        {
            try
            {
                //if (!SettingsManager.Settings.IgnoreHttpError)
                //{
                MetroFramework.MetroMessageBox.Show(Program.MainFormInstance, "Error : " + ex.Message + "\nUnable to get chunk or key data from Riot server. \nMaybe it was too late to join the game ?\nTo force recording enable IgnoreHttpError", "Failed to record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                recorder.Recording = false;
                Program.MainFormInstance.BeginInvoke(new UpdateReplayRecorded(Program.MainFormInstance.CancelReplayRecorded), null, this.RecordGui);
                // }
             

            }
            catch
            {

            }
        }
        void recorder_OnDownloadProgress(DownloadTask task, int prog)
        {
           // RecordGui.UpdateProgress(task, prog);
        }
        void recorder_OnProblemOccured(object sender, EventArgs e)
        {
            try
            {
                RecordGui.UpdateStatusA();
            }
            catch
            {
            }
        }
        public void Record()
        {
            try
            {

                recorder = new ReplayRecorder(Server, GameID, Platform, Key, ReplayRecording);
             


                //DateTime st = recorder.GetGameStartTime();
                //RecordGui.SetGameStartTime(st);
                recorder.API = Program.MainFormInstance.API;
                recorder.OnReplayRecorded += recorder_OnReplayRecorded;
                recorder.OnGotData += recorder_OnGotChunk;
                recorder.OnAttemptToDownload += recorder_OnAttemptToDownload;
                recorder.OnFailedToRecord += recorder_OnFailedToRecord;
                recorder.OnFailedToSave += recorder_OnFailedToSave;
                recorder.OnDownloadProgress += recorder_OnDownloadProgress;
     recorder.OnProblemOccured +=recorder_OnProblemOccured;
                recorder.CancelRecordOnFail = true;
                if (!recorder.Recording)
                    recorder_OnFailedToRecord(new Exception("The game has ended a long time ago or never existed"));

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to init record", ex);
            }
        }

        // Flag: Has Dispose already been called?
        bool disposed = false;


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                recorder.Dispose();
                recorder = null;
                ReplayRecording.Dispose();
                ReplayRecording = null;
                RecordGui.Dispose();
                RecordGui = null;

                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
