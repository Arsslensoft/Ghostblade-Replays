using GBNet;
using GBReplay.Replays;
using GBReplay.Replays.Game;
using GhostLib;
using GhostLib.Network;
using Newtonsoft.Json;
//using Replay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace GhostLib
{
    public delegate void FailedToRecordEventHandler(Exception ex);
    public delegate void DataAttemptToDownloadEventHandler(DownloadTask task, int attemp);
    public delegate void DownloadTaskCompletedEventHandler(DownloadTask task);
    public delegate void DownloadTaskProgressEventHandler(DownloadTask task, int progress);
    public delegate void RecordingStatusEventHandler(string status);
    

    public delegate void OnReplayRecordedEventHandler();


   public class ReplayRecorder : IDisposable
    {

  
       public long GameId { get; set; }
       public int LastChunkNumber { get; set; }
       public string Region { get; set; }
       public string Server { get; set; }
       public bool Recording
       {
           get;
           set;

       }

       public GhostReplay CurrentReplay { get; set; }

       
       public int LastKeyGot = 0;
       public bool CancelRecordOnFail = false;
       public RiotSharp.RiotApi API;
       public IPAddress ExternalIP = IPAddress.Any;
       byte RetryAttempt = 0;
       #region Download Tasks
       public List<DownloadTask> DownloadTasks { get; set; }

       public bool IsBusy(DownloadTask id)
       {
           foreach (DownloadTask dt in DownloadTasks)
               if (dt.Id == id.Id && dt.IsChunk == id.IsChunk)
                   return true;


           return false;
       }
       public bool EndTask(DownloadTask id)
       {

           for (int i = 0; i < DownloadTasks.Count; i++)
           {
               if (DownloadTasks[i].Id == id.Id && DownloadTasks[i].IsChunk == id.IsChunk)
               {
                   DownloadTasks.RemoveAt(i);
                   return true;
               }
           }
           return false;
       }

       public DownloadTask CreateChunkTask(int id)
       {
           return new DownloadTask((byte)id, true, GameId);
       }
       public DownloadTask CreateKeyTask(int id)
       {
           return new DownloadTask((byte)id, false, GameId);
       }

       #endregion

       #region Events
       public event OnReplayRecordedEventHandler OnReplayRecorded;


       public event DownloadTaskCompletedEventHandler OnGotData;
       public event DownloadTaskProgressEventHandler OnDownloadProgress;

       public event FailedToRecordEventHandler OnFailedToRecord;
       public event FailedToRecordEventHandler OnFailedToSave;

       public event DataAttemptToDownloadEventHandler OnAttemptToDownload;
       public event RecordingStatusEventHandler OnStatusChanged;
       public event EventHandler OnProblemOccured;
       #endregion






       public void DownloadChunk(SmartWebClient client, DownloadTask task, int attemp)
       {

           try
           {
               if (Recording)
               {
                   while (client.IsBusy)
                       Thread.Sleep(100);

                   if (OnStatusChanged != null)
                       OnStatusChanged("Downloading chunk " + task.Id.ToString());


                   if (OnAttemptToDownload != null && attemp != 0)
                       OnAttemptToDownload(task, attemp);



                   client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) =>
                   {
                       if (OnDownloadProgress != null)
                           OnDownloadProgress(task, e.ProgressPercentage);
                   });

                   if (!IsBusy(task))
                       DownloadTasks.Add(task);
                   else
                       Logger.Instance.Log.Warn("Multiple Download Tasks " + task.Id + "[" + task.IsChunk.ToString() + "]");

                   client.DownloadDataAsync(new Uri(
                                        String.Format("{0}/consumer/{1}/{2}/{3}/{4}/token", this.Server + "/observer-mode/rest",
                                        "getGameDataChunk",
                                        this.Region,
                                        this.GameId, task.Id)));
                   byte[] chunk = null;
                   bool failed = false;
                   client.DownloadDataCompleted += new DownloadDataCompletedEventHandler((sender, e) =>
                   {
                       EndTask(task);
                       if (!e.Cancelled && e.Error == null)
                           chunk = e.Result;
                       else if (e.Error != null && attemp < 5)
                           failed = true;

                   });
                   while (client.IsBusy)
                       Thread.Sleep(100);

                   if (failed)
                   {
                //       OnAttemptToDownload(task, attemp + 1);
                       DownloadChunk(client, task, attemp + 1);

                   }
                   if (OnDownloadProgress != null)
                       OnDownloadProgress(task, -1);
                   if (chunk == null)

                       throw new Exception("Failed to download replay data Maximum attempt exceeded");
                   CurrentReplay.AddChunk(chunk, (byte)task.Id);
                   chunk = null;
               }
           }
           catch (Exception ex)
           {

               if (ResolveException(ex, task))
                   DownloadChunk(client, task, attemp + 1);
               else if (attemp < 5)
               {
                   Thread.Sleep(1000);
                   DownloadChunk(client, task, attemp + 1);
               }
               else OnFailedToRecord(ex);

               //if ((ex is WebException) || ex.Message.Contains("404"))
               //{
               //    if (attemp == 3 && OnFailedToRecord != null)
               //        OnFailedToRecord(ex);
               //    else if (attemp < 3)
               //        DownloadChunk(client, task, attemp + 1);
               //}
               //if (OnFailedToRecord != null)
               //    OnFailedToRecord(ex);
           }
       }
       public void DownloadKey(SmartWebClient client, DownloadTask task, int attemp)
       {

           try
           {
               if (Recording)
               {
                   while (client.IsBusy)
                       Thread.Sleep(100);

                   if (OnStatusChanged != null)
                       OnStatusChanged("Downloading key " + task.Id.ToString());
                   if (OnAttemptToDownload != null && attemp != 0)
                       OnAttemptToDownload(task, attemp);



                   client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) =>
                   {
                       if (OnDownloadProgress != null)
                           OnDownloadProgress(task, e.ProgressPercentage);
                   });

                   if (attemp == 0 && !IsBusy(task))
                       DownloadTasks.Add(task);
                   else Logger.Instance.Log.Warn("Multiple Download Tasks " + task.Id + "[" + task.IsChunk.ToString() + "]");

                   client.DownloadDataAsync(new Uri(
                                          String.Format("{0}/consumer/{1}/{2}/{3}/{4}/token", Server + "/observer-mode/rest",
                               "getKeyFrame",
                               Region,
                               GameId,
                               task.Id)));
                   byte[] chunk = null;
                   bool failed = false;
                   client.DownloadDataCompleted += new DownloadDataCompletedEventHandler((sender, e) =>
                   {
                       EndTask(task);
                       if (!e.Cancelled && e.Error == null)
                           chunk = e.Result;
                       else if (e.Error != null && attemp < 5)
                           failed = true;

                   });
                   if (failed)
                       DownloadKey(client, task, attemp + 1);

                   while (client.IsBusy)
                       Thread.Sleep(100);



                   if (OnDownloadProgress != null)
                       OnDownloadProgress(task, -1);
                  
                   if (chunk == null)

                       throw new Exception("Failed to download replay data Maximum attempt exceeded");


                   CurrentReplay.AddKey(chunk, (byte)task.Id);
                   chunk = null;
               }
           }
           catch (Exception ex)
           {
               if(ResolveException(ex, task) )
                   DownloadKey(client, task, attemp + 1);
               else if (attemp < 5)
               {
                   Thread.Sleep(1000);
                   DownloadKey(client, task, attemp + 1);
               }
               else OnFailedToRecord(ex);

               //if ((ex is WebException) || ex.Message.Contains("404"))
               //{
               //    if (attemp == 3 && OnFailedToRecord != null)
               //        OnFailedToRecord(ex);
               //    else if (attemp < 3)
               //        DownloadKey(client, task, attemp + 1);
               //}
               //if (OnFailedToRecord != null)
               //    OnFailedToRecord(ex);
           }
       }


       public void DownloadChunk(SmartWebClient client, int id)
       {
           DownloadTask ct = CreateChunkTask(id);
           DownloadChunk(client, ct, 0);

           if (OnGotData != null)
               OnGotData(ct);
       }
       public void DownloadKey(SmartWebClient client, int id)
       {
           DownloadTask ct = CreateKeyTask(id);
           DownloadKey(client, ct, 0);
           if (OnGotData != null)
               OnGotData(ct);
       }

       public bool ResolveException(Exception ex, DownloadTask t)
       {
           if (ex is WebException || ex is ArgumentNullException)
               return true;
       

           return false;
       }


        public ReplayRecorder(string Server, long GameId, string Region, string Key, GhostReplay rep)
        {
       try
              {
            this.DownloadTasks = new List<DownloadTask>();
            this.GameId = GameId;
            this.Region = Region;
            CurrentReplay = rep;
            Recording = true;
       
            if (Directory.Exists(rep.CacheDirectory))
            {
                File.WriteAllText(rep.CacheDirectory + @"\cache.dat", Key );
                if (!string.IsNullOrEmpty(rep.SummonerName))
                    File.WriteAllText(rep.CacheDirectory + @"\summoner.dat", rep.SummonerName );
            }

            if (!Server.StartsWith("127.0.0.1:90"))
            {
                this.Server = "http://" + Server;
        
                rep.ObserverKey = Key;
          
                int ChunkTimeInterval;
                int LastChunk = 0;
                using (SmartWebClient client = new SmartWebClient(30000))
                {
                    // Proxy
                    client.Proxy = client.GetDefaulProxy();
                    // Version 
                   rep.Version =  client.DownloadString(String.Format("{0}/consumer/version", this.Server + "/observer-mode/rest")                  );
                    // Token
                 string token = client.DownloadString(                      String.Format("{0}/consumer/{1}/{2}/{3}/token", this.Server + "/observer-mode/rest",                        "getGameMetaData",                        Region,                        GameId));

                 GameMetaData meta = JsonConvert.DeserializeObject<GameMetaData>(token);
                 ChunkTimeInterval = meta.chunkTimeInterval;
                 LastChunk = meta.endStartupChunkId;

                }

                ThreadPool.QueueUserWorkItem(delegate
                {
                
                    while (Recording)
                    {
                        try
                        {
                            GetChunk();
                           RetryAttempt = 0;
                        }
                        catch (Exception ex)
                        {
                            RetryAttempt++;
                           if(OnProblemOccured!=null)
                            OnProblemOccured(this, EventArgs.Empty);
                         
                           if (RetryAttempt >= 10 && OnFailedToRecord != null)
                           {
                               Recording = false;
                               OnFailedToRecord(ex);
                           }
                           else Thread.Sleep(15000);
                          if(RetryAttempt == 1)
                            Logger.Instance.Log.Error("Failed to record [GET CHUNK]", ex);
                         
                        }
                    }
                });
            }
                        }
               catch (Exception ex)
               {
                   if(OnFailedToRecord != null)
                   OnFailedToRecord(ex);
                   Logger.Instance.Log.Error("Failed to record [GLOBAL]", ex);
                   Recording = false;
               }
        }

        #region TimeZone

        public const string PacificTimeZone = "Central Pacific Standard Time";
        public const string UtcTimeZone = "UTC";
        public const string ChinaTimeZone = "Singapore Standard Time";
        public static Dictionary<string, string> TimeZones
        {
            get
            {
                return new System.Collections.Generic.Dictionary<string, string>
			{

             

        	
				{ "NA1",PacificTimeZone},
				{ "EUW1", UtcTimeZone },
				{ "EUN1",PacificTimeZone },
				{ "EU", PacificTimeZone },
				{ "BR1", PacificTimeZone},
				{ "LA1",PacificTimeZone },
				{ "LA2", PacificTimeZone },
				{ "BRLA", PacificTimeZone },
				{ "TR1", PacificTimeZone },
				{ "RU", PacificTimeZone},
				{ "TRRU", PacificTimeZone},
				{ "PBE1", PacificTimeZone },
				{ "SG", ChinaTimeZone },
                	{ "TW", ChinaTimeZone },
				{ "KR", PacificTimeZone },
				{ "OC1", ChinaTimeZone },
				{ "VN", ChinaTimeZone},
				{ "PH", ChinaTimeZone},
				{ "ID1", ChinaTimeZone },
				{ "TH", ChinaTimeZone },
				{ "TRSA", PacificTimeZone},
				{ "TRNA", PacificTimeZone },
				{ "TRTW", ChinaTimeZone},
				{ "HN1_NEW", PacificTimeZone }
			};
            }
        }
        public DateTime GetGameStartTime()
        {
            try
            {
                using (SmartWebClient client = new SmartWebClient(30000))
                {

                    // Proxy
                    client.Proxy = client.GetDefaulProxy();


                    string token = client.DownloadString(
         String.Format("{0}/consumer/{1}/{2}/{3}/token", this.Server + "/observer-mode/rest",
         "getGameMetaData",
         Region,
         GameId));
                    GameMetaData gd = JsonConvert.DeserializeObject<GameMetaData>(token);
                 DateTime start = TimeZoneInfo.ConvertTime(DateTime.Parse(gd.startTime), TimeZoneInfo.FindSystemTimeZoneById(TimeZones[Region]), TimeZoneInfo.Local);
             //       TimeSpan ts = new TimeSpan(gd.gameLength * TimeSpan.TicksPerSecond);

                  //  DateTime starttime = DateTime.Now.Subtract(ts);

                 return start;
                }
            }


              
            catch
            {

            }
            return DateTime.Now;
        }

        #endregion



        void SaveReplay(SmartWebClient client, int ChunkId)
        {
            //Sometimes chunk 1 isn't retrieved so get it again... it's like 7 kb so np
            // Download Chunk 0
            DownloadChunk(client, 1);

            // Download Current Chunk id
            DownloadChunk(client, ChunkId);

            if (OnStatusChanged != null)
                OnStatusChanged("Saving replay");

            // End Stats
            string stat = client.DownloadString(
                         String.Format("{0}/consumer/{1}/{2}/{3}/token", Server + "/observer-mode/rest",
                         "endOfGameStats",
                         Region,
                         GameId));
          
            
            string sum = CurrentReplay.SummonerName;
            CurrentReplay.ParseStats(Convert.FromBase64String(stat));
            CurrentReplay.SummonerName = sum;

            // Last Chunk Info End
          string  token = client.DownloadString(
            String.Format("{0}/consumer/{1}/{2}/{3}/0/token", Server + "/observer-mode/rest",
            "getLastChunkInfo",
            Region,
            GameId));

            CurrentReplay.ParseLastChunkInfo(token);

            // Last game meta data
            token = client.DownloadString(
       String.Format("{0}/consumer/{1}/{2}/{3}/token", this.Server + "/observer-mode/rest",
       "getGameMetaData",
       Region,
       GameId));
            ReplayInfo flags = ReplayInfo.StandardFormat | ReplayInfo.Ghostblade;


            if (CurrentReplay.IsPBE)
                flags |= ReplayInfo.PBE;


            if (string.IsNullOrEmpty(CurrentReplay.SummonerName) || CurrentReplay.SummonerName == "Spectator")
            {
                flags |= ReplayInfo.Spectator;
                if (CurrentReplay.GameStats.TeamPlayerParticipantStats.Count > 0)
                    CurrentReplay.SummonerName = CurrentReplay.GameStats.TeamPlayerParticipantStats[0].SummonerName;

                else if (CurrentReplay.GameStats.OtherTeamPlayerParticipantStats.Count > 0)
                    CurrentReplay.SummonerName = CurrentReplay.GameStats.OtherTeamPlayerParticipantStats[0].SummonerName;
            }
            CurrentReplay.ParseMetaData(token);
            CurrentReplay.MetaData.clientBackFetchingEnabled = true;
            CurrentReplay.MetaData.clientBackFetchingFreq = 50;
            CurrentReplay.MetaData.pendingAvailableChunkInfo.Clear();
            CurrentReplay.MetaData.pendingAvailableKeyFrameInfo.Clear();
            if (!CurrentReplay.IsPBE)
            {
                try
                {
                    RiotSharp.MatchEndpoint.MatchDetail det = API.GetMatch(RiotTool.PlatformToRegion(Region), GameId); // TODO ADD COMPRESSION FLAG
                    CurrentReplay.PlayerInfos.Map = (byte)det.MapType;
                    CurrentReplay.PlayerInfos.Queue = (byte)det.QueueType;


                }
                catch
                {
                    CurrentReplay.PlayerInfos.Map = 11;
                    CurrentReplay.PlayerInfos.Queue = 4;
                }
            }
            try
            {
                CurrentReplay.Save(CurrentReplay.Signer != null, flags);
            }
            catch (Exception ex)
            {
                if (OnFailedToSave != null)
                    OnFailedToSave(ex);
            }
            if (OnReplayRecorded != null)
                OnReplayRecorded();

            Recording = false;
        }
        void RecoverMissing(SmartWebClient client, ChunkInfo ci)
        {
            List<int> missingKeys;
            List<int> missingChunks;
            CurrentReplay.RecoverMissing(ci.keyFrameId,ci.chunkId, Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Temp", GameId + "-" + Region), out missingKeys, out missingChunks);
            if (missingKeys.Count > 0)
            {
                // Missing Keys
                foreach (int keyid in missingKeys)
                {

                    // GET THEM
                    try
                    {
                        if (Recording)
                            DownloadKey(client,keyid);

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("404") && OnFailedToRecord != null && CancelRecordOnFail)
                        {
                            OnFailedToRecord(ex);

                            return;
                        }
                    }
                }
            }
            if (missingChunks.Count > 0)
            {
                // Missing Chunks
                foreach (int chunk in missingChunks)
                {
                    try
                    {

                        if (Recording)
                            DownloadChunk(client,chunk);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("404") && OnFailedToRecord != null)
                            OnFailedToRecord(ex);
                    }
                }
            }
        }
       
        void GetChunk()
        {
            using (SmartWebClient client = new SmartWebClient(30000))
            {
                // Proxy
                client.Proxy = client.GetDefaulProxy();
                // token
              
                //string token = client.DownloadString(
                //    String.Format("{0}/consumer/{1}/{2}/{3}/0/token", Server + "/observer-mode/rest",
                //    "getLastChunkInfo",
                //    Region,
                //    GameId));

                ChunkInfo ci = JsonConvert.DeserializeObject<ChunkInfo>(client.DownloadString(
                    String.Format("{0}/consumer/{1}/{2}/{3}/0/token", Server + "/observer-mode/rest",
                    "getLastChunkInfo",
                    Region,
                    GameId)));
               
                int ChunkId =ci.chunkId;
     
                if (ChunkId == 0)
                {
                    //Try get chunk once avaliable
                    return;
                }

                // Save Replay
                if (LastChunkNumber == ChunkId)
                {
                    SaveReplay(client, ChunkId);
                    return;
                }

                //Get keyframe
                if (ChunkId % 2 == 0)
                {
                    int KeyFrameId = ci.keyFrameId;
                    if (KeyFrameId != 0)
                        DownloadKey(client, KeyFrameId);
                }

                // Get Current Chunk
                if (Recording)
                    DownloadChunk(client, ChunkId);

                LastChunkNumber = ChunkId;


             // Recover MISSING
                RecoverMissing(client, ci);
              
                Thread.Sleep(ci.nextAvailableChunk);
           }
        }



        // Flag: Has Dispose already been called?
        bool disposed = false;


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
           
                DownloadTasks.Clear();
                API = null;

                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
