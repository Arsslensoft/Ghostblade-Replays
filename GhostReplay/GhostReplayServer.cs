
using GBReplay.Replays;
using GBReplay.Replays.Game;
using GBReplay.Replays.Riot;
using GhostLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace GhostReplays
{
   internal class HeroSpawn
    {
       internal bool IsHeroSpawned= false;
       internal DateTime FirstChunkInfo= DateTime.Now;
    }
   public  class GhostReplayServer : HttpServer
    {

   
        int LatestChunk;
        int NextChunk;
        int LatestKeyframe;
        int LastChunk;
        GhostReplay rep;
        DateTime FirstChunkInfo = DateTime.Now;
        public bool Streaming = true;
        string GameLog = "";
        bool IsHeroSpawned = false;
     
        Dictionary<string, HeroSpawn> Clients = new Dictionary<string, HeroSpawn>();
          [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public GhostReplayServer(int port, string GameId, string Region,string gameepath,string LolDir,string replayfile)
           : base(port)
       {
           if (Process.GetProcessesByName("League of Legends").Length != 0)
         
               return;
           if (!File.Exists(replayfile))
           {
               //  [DISABLED] Console.WriteLine("Cannot find replay");
               return;
           }
    
           rep = new GhostReplay(replayfile, "0.0.0.0", false, AppDomain.CurrentDomain.BaseDirectory + @"\Temp");
          
              rep.ReadReplay(ReadMode.All);
        
           LatestChunk = 1;
           NextChunk = 2;
           LatestKeyframe = 1;
           FChunk = new ChunkInfo();
           FChunk.chunkId = rep.MetaData.startGameChunkId;
           FChunk.startGameChunkId = rep.MetaData.startGameChunkId;
           FChunk.endGameChunkId = 0;
           FChunk.endStartupChunkId = rep.MetaData.endStartupChunkId;
           FChunk.nextAvailableChunk = 500;
           FChunk.nextChunkId = rep.MetaData.startGameChunkId+1;
           FChunk.keyFrameId = 1;
           FChunk.duration = 30000;
           FChunk.availableSince = 30000;
           rep.MetaData.clientBackFetchingFreq = 50;
           foreach (GameData f in rep.Chunks)
           {
               int ChunkId = f.Id;
               if (ChunkId > LastChunk)
                   LastChunk = ChunkId;

            
           }



           foreach (GameData f in rep.Keys)
           {
               int KeyId = f.Id;
               if (KeyId < LatestKeyframe)
                   LatestKeyframe = KeyId;
           }

           Process p = new System.Diagnostics.Process();
           p.StartInfo.WorkingDirectory = LolDir;
           p.StartInfo.UseShellExecute = false;
           p.EnableRaisingEvents = true;
           p.Exited += p_Exited;
           p.StartInfo.FileName = Path.Combine(LolDir, "League of Legends.exe");
           p.StartInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
               + "127.0.0.1:" + port.ToString() + " "
               + rep.ObserverKey + " "
               + GameId + " "
               + Region + "\"";

           p.Start();

           GhostOverlay.ShowOverlay(LolDir);

           
         SetHome(rep, GameId, Region);
       }
        ChunkInfo FChunk;

      string HomePage = "";
      RawStatDTO GetRawStat(string name, PlayerParticipantStatsSummary p)
      {
          foreach (RawStatDTO rd in p.Statistics)
              if (name == rd.StatTypeName)
                  return rd;

          return null;
      }
      bool IsWon(int teamid, GhostReplay rep)
      {
          if (teamid == 100)
              return (GetRawStat("WIN",rep.GameStats.TeamPlayerParticipantStats[0]).Value == 1);
          else return (GetRawStat("WIN", rep.GameStats.OtherTeamPlayerParticipantStats[0]).Value == 1);
      }
      public void SetHome(GhostReplay rep, string gid, string reg)
      {
          try
          {
              HomePage = global::GhostReplays.Properties.Resources.HOME;
              HomePage = HomePage.Replace("{$.gid.$}", gid).Replace("{$.reg.$}", reg);
              if (rep.GameStats != null)
              {

                  HomePage = HomePage.Replace("{$.map.$}", RiotTool .ToMapString((RiotSharp.MapType) rep.PlayerInfos.Map));
                  HomePage = HomePage.Replace("{$.queue.$}", rep.GameStats.QueueType);
                  string dur = string.Format("{0:0}:{1:00}", rep.GameLength.Minutes, rep.GameLength.Seconds);
                  HomePage = HomePage.Replace("{$.glength.$}", dur);
                  HomePage = HomePage.Replace("{$.mplayer.$}", rep.SummonerName);
                  if (IsWon(100, rep))
                        HomePage = HomePage.Replace("{$.winner.$}", "Blue Team");
               
                  else
                      HomePage = HomePage.Replace("{$.winner.$}", "Red Team");
              }
              else
              {
                  HomePage = HomePage.Replace("{$.map.$}", "Unknown");
                  HomePage = HomePage.Replace("{$.queue.$}", "Unknown");
                  string dur = string.Format("{0:0}:{1:00}", rep.GameLength.Minutes, rep.GameLength.Seconds);
                  HomePage = HomePage.Replace("{$.glength.$}", dur);
                  HomePage = HomePage.Replace("{$.mplayer.$}", rep.SummonerName);

                  HomePage = HomePage.Replace("{$.winner.$}", "Unknown");

              }
          }
          catch
          {

          }

      }
       public override void handleGETRequest(HttpProcessor p)
       {
           string content = "";
           bool ShutdownAfterQuery = false;
           string home = HomePage.Replace("{$.spec.$}", Clients.Count.ToString());
       
           if (p.http_url == "/")
           {
               p.writeSuccess();
               p.outputStream.WriteLine(home);
               //p.outputStream.WriteLine("<html><head><title>Ghostblade Replay Server</title></head><body><h1>ACCESS DENIED - Spectator Server</h1><p>This is the Ghostblade League of Legends Replay Server Version 1.0 .</p></body></html>");
           }
           else if (p.http_url.StartsWith("/observer-mode/rest/consumer/"))
           {
               // Replay handler
             
               string RequestedURL = p.http_url.Replace("/observer-mode/rest/consumer/", "");
               //  [DISABLED] Console.Clear();
               //  [DISABLED] Console.WriteLine("Requested  " + RequestedURL + "   "+DateTime.Now.ToLongTimeString());
               string client = p.socket.Client.RemoteEndPoint.ToString().Split(':')[0];
               if (!Clients.ContainsKey(client))
                   Clients.Add(client, new HeroSpawn());

               HeroSpawn hr = Clients[client];
          //     if (hr.IsHeroSpawned)
                   //  [DISABLED] Console.Title = "Champions Spawned";
           
               byte[] buffer = new byte[0];

               if (RequestedURL == "version")
               {
                   buffer = Encoding.UTF8.GetBytes(rep.Version);
                   content = "text/plain";
               }
               else if (RequestedURL == "end")
               {
                   buffer = Encoding.UTF8.GetBytes("done");
                   content = "text/plain";
                 //  ShutdownAfterQuery = true;
               }
               string[] Params = RequestedURL.Split('/');
               if (Params.Length > 0)
               {
                   if (Params[0] == "getGameMetaData")
                   {
                       content = "application/json";
                       buffer = rep.GetMetaData();
                   }
                   else if (Params[0] == "getLastChunkInfo")
                   {
                       ChunkInfo ci = rep.LastChunkInfo;

                       if (Params[3] != "30000" && !hr.IsHeroSpawned)
                       {
                           hr.IsHeroSpawned = true;
                           hr.FirstChunkInfo = DateTime.Now;
                       }
                       else if (!hr.IsHeroSpawned)
                           hr.IsHeroSpawned = false;
                       content = "application/json";
                    
                           //if (!IsHeroSpawned)
                           //    CheckHero();
                           //    buffer = Encoding.UTF8.GetBytes(ChunkInfo);
                       if (!hr.IsHeroSpawned)
                                 buffer = rep.GetChunkInfo(FChunk);


                       else if (hr.IsHeroSpawned && (DateTime.Now - hr.FirstChunkInfo).TotalMilliseconds < 9000)
                               buffer = rep.GetChunkInfo(FChunk);

                           else
                               buffer = rep.GetChunkInfo(rep.LastChunkInfo);
                   

                   }
                   else if (Params[0] == "getGameDataChunk")
                   {
                       //if (Convert.ToInt32(Params[3]) == (LastChunk - 1))
                       //    Params[3] = LastChunk.ToString();

                       buffer = rep.GetChunk(short.Parse(Params[3]));

                       if (Convert.ToInt32(Params[3]) >= LatestChunk)
                       {
                           LatestChunk += 1;
                           NextChunk += 1;

                           if (LatestChunk >= 7)
                           {
                               if (LatestChunk % 2 == 0)
                                   LatestKeyframe += 1;
                           }
                       }
                               content = "application/octet-stream";
                
                   }
                   else if (Params[0] == "getKeyFrame")
                   {
                           content = "application/octet-stream";
                           buffer = rep.GetKey(short.Parse(Params[3]));
                   }
               }
           
                         CultureInfo provider = new CultureInfo("en-US");
               p.outputStream.WriteLine("HTTP/1.1 200 OK");
               p.outputStream.WriteLine("Content-Type: "+content);

               p.outputStream.WriteLine("Accept-Ranges: bytes");
               p.outputStream.WriteLine("Server: GhostReplay");
              p.outputStream.WriteLine("Pragma: no-cache");
           p.outputStream.WriteLine("Cash-Control: no-cache");
             p.outputStream.WriteLine("Expires: Thu, 01 Jan 1970 08:00:00 CST");
               p.outputStream.WriteLine("Content-Length: " + buffer.Length.ToString());
               p.outputStream.WriteLine("Date: " + DateTime.UtcNow.ToString("ddd, dd MM yyyy HH:mm:ss GMT", provider));
                p.outputStream.WriteLine("Connection: close");
               p.outputStream.WriteLine("");
               p.outputStream.WriteContent(buffer);
            

      

           }
           if (ShutdownAfterQuery)  
                   Environment.Exit(0);
        
       }

       static void p_Exited(object sender, EventArgs e){
           Process.GetCurrentProcess().Kill();
       }
       public override void handlePOSTRequest(HttpProcessor p, System.IO.MemoryStream inputData)
       {
           p.writeFailure();
           p.outputStream.WriteLine("ACCESS DENIED");
       }
    }
}
