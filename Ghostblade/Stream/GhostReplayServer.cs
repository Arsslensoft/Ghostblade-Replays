
using GBReplay.Replays;
using GBReplay.Replays.Game;
using GBReplay.Replays.Riot;
using GhostLib;
using GhostLib.Network;
using Newtonsoft.Json;
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

   public class GhostbladeStream
   {
       public string StreamId { get; set; }


     internal  int LatestChunk;
     internal int NextChunk;
     internal int LatestKeyframe;
     internal int LastChunk;
     internal   GhostReplay CurrentReplay;


   internal  DateTime FirstChunkInfo = DateTime.Now;
     public bool Streaming = true;
     internal bool IsHeroSpawned = false;

    internal Dictionary<string, HeroSpawn> Clients = new Dictionary<string, HeroSpawn>();
    internal ChunkInfo FChunk;

    public GhostbladeStream(GhostReplay rep)
    {
        StreamId = rep.GameId.ToString() + "-" + rep.Platform;
        CurrentReplay = rep;
        LatestChunk = 1;
        NextChunk = 2;
        LatestKeyframe = 1;
        FChunk = new ChunkInfo();
        FChunk.chunkId = rep.MetaData.startGameChunkId;
        FChunk.startGameChunkId = rep.MetaData.startGameChunkId;
        FChunk.endGameChunkId = 0;
        FChunk.endStartupChunkId = rep.MetaData.endStartupChunkId;
        FChunk.nextAvailableChunk = 500;
        FChunk.nextChunkId = rep.MetaData.startGameChunkId + 1;
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

        IsHeroSpawned = true;
    }

   }

   public  class GhostReplayServer : HttpServer
    {
       public string Version = null;
       public Dictionary<string, GhostbladeStream> Streams = new Dictionary<string, GhostbladeStream>();
          [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public GhostReplayServer(int port)
           : base(port)
       {
           HomePage = global::Ghostblade.Properties.Resources.HOME;
           GetIP();
       }

          public RiotSharp.Featured.FeaturedGames GetCurrentGames()
          {
              RiotSharp.Featured.FeaturedGames fg = new RiotSharp.Featured.FeaturedGames();
              fg.gameList = new List<RiotSharp.Featured.GameList>();
              fg.clientRefreshInterval = 300;
        
              foreach (KeyValuePair<string, GhostbladeStream> kp in Streams)
              {
                  RiotSharp.Featured.GameList gl = new RiotSharp.Featured.GameList();
                  gl.gameId = kp.Value.CurrentReplay.GameId;
                  gl.gameLength = kp.Value.CurrentReplay.MetaData.gameLength;
                  gl.gameMode = kp.Value.CurrentReplay.GameStats.GameMode;
                  gl.gameQueueConfigId =(int) kp.Value.CurrentReplay.PlayerInfos.Queue;
                  gl.gameStartTime = kp.Value.CurrentReplay.MetaData.startTime;
                  gl.gameType = kp.Value.CurrentReplay.GameStats.GameType;
                  gl.mapId = kp.Value.CurrentReplay.PlayerInfos.Map;
                  gl.observers = new RiotSharp.Featured.Observers();
                  gl.observers.encryptionKey = kp.Value.CurrentReplay.ObserverKey;
                  gl.platformId = kp.Value.CurrentReplay.Platform;
                  gl.bannedChampions = new List<RiotSharp.Featured.BannedChampion>();
                  gl.participants = new List<RiotSharp.Featured.Participant>();
                  foreach (PlayerParticipantStatsSummary p in kp.Value.CurrentReplay.GameStats.TeamPlayerParticipantStats)
                  {
                      RiotSharp.Featured.Participant part = new RiotSharp.Featured.Participant();
                      part.bot = p.BotPlayer;
                      part.championId = Ghostblade.CurrentGame.GetChampionID(p.SkinName); ;
                      part.profileIconId = 0;
                      part.spell1Id = (int)p.Spell1Id;
                      part.spell2Id = (int)p.Spell2Id;
                      part.summonerName = p.SummonerName;
                      part.teamId =(int) p.TeamId;

                      gl.participants.Add(part);
                  }

                  foreach (PlayerParticipantStatsSummary op in kp.Value.CurrentReplay.GameStats.OtherTeamPlayerParticipantStats)
                  {
                      RiotSharp.Featured.Participant part = new RiotSharp.Featured.Participant();
                      part.bot = op.BotPlayer;
                      part.championId = Ghostblade.CurrentGame.GetChampionID(op.SkinName); ;
                      part.profileIconId = 0;
                      part.spell1Id = (int)op.Spell1Id;
                      part.spell2Id = (int)op.Spell2Id;
                      part.summonerName = op.SummonerName;
                      part.teamId = (int)op.TeamId;

                      gl.participants.Add(part);
                  }

                  fg.gameList.Add(gl);
              }

              return fg;

          }
          public bool AddStream(GhostReplay rep)
          {
              rep.ReadReplay(ReadMode.All);
              Version = rep.Version;
              if (Streams.ContainsKey(rep.GameId.ToString() + "-" + rep.Platform))
                  return false;
              else Streams.Add(rep.GameId.ToString() + "-" + rep.Platform, new GhostbladeStream(rep));

              return true;
          }
          public bool IsStreaming(GhostReplay rep)
          {
              return Streams.ContainsKey(rep.GameId.ToString() + "-" + rep.Platform);
          }
          public bool IsStreaming(string id)
          {
              return Streams.ContainsKey(id);
          }
          public GhostbladeStream GetStream(string id)
          {
              if (IsStreaming(id))
                  return Streams[id];
              else return null;
          }
          public bool RemoveStream(string id)
          {
              if (!IsStreaming(id))
                  return false;

              else Streams.Remove(id);
              return true;
          }



      string HomePage = "";


       void ResetForClient(string cl)
       {
           foreach (GhostbladeStream str in Streams.Values)
           {
               if (str.Clients.ContainsKey(cl))
                  str.Clients[cl].IsHeroSpawned = false;
               
           }
       }
       string ExternalIp;
       public void GetIP()
       {
           try
           {
               using (SmartWebClient wbc = new SmartWebClient(4000))
                  ExternalIp = wbc.DownloadString("https://api.ipify.org/?format=text");
      
           }
           catch
           {

               ExternalIp = "127.0.0.1";
           }
       }

       public string GetCmd(GhostReplay rep)
       {
           try
           {
           
                  return "\"" + RiotTool.Instance.InstalledGameProfile.LeagueExecutable + "\" " + "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
                + ExternalIp + ":" + SettingsManager.Settings.GhostStreamPort.ToString() + " "
                + rep.ObserverKey + " "
                + rep.GameId.ToString() + " "
                + rep.Platform + "\"";

           }
           catch
           {
               return "NONE";
           }
       }
       public override void handleGETRequest(HttpProcessor p)
       {
           try
           {
               string content = "";
               // bool ShutdownAfterQuery = false;
               bool SERVED = true;
               if (Version == null)
               {
                   p.writeFailure();
                   p.outputStream.WriteLine("No Stream available");
                   return;
               }
              
               //string home = "Hello World";
               if (p.http_url == "/")
               {
                   p.writeSuccess();
                   StringBuilder sb = new StringBuilder();
                    foreach(KeyValuePair<string,GhostbladeStream> kp in Streams)
                    {
                          string dur = string.Format("{0:0}:{1:00}", kp.Value.CurrentReplay.GameLength.Minutes, kp.Value.CurrentReplay.GameLength.Seconds);
                          sb.AppendLine("<p>" + kp.Key + " [" + dur + "]   </p>"+"<br>Spectator Command : <pre>" + GetCmd(kp.Value.CurrentReplay)+ " </pre>");

                    }
                    sb.AppendLine("<hr />");
               string home = HomePage.Replace("{$.games.$}",sb.ToString());
                   p.outputStream.WriteLine(home);
                   //p.outputStream.WriteLine("<html><head><title>Ghostblade Replay Server</title></head><body><h1>ACCESS DENIED - Spectator Server</h1><p>This is the Ghostblade League of Legends Replay Server Version 1.0 .</p></body></html>");
               }
               else if (p.http_url.StartsWith("/observer-mode/rest/featured"))
               {
                   RiotSharp.Featured.FeaturedGames fg = GetCurrentGames();
                   byte[] buffer = new byte[0];
                   string dat = JsonConvert.SerializeObject(fg);
                   buffer =  Encoding.UTF8.GetBytes(dat);
                   p.outputStream.WriteLine("HTTP/1.1 200 OK");
                   p.outputStream.WriteLine("Content-Type: application/json");
                   p.outputStream.WriteLine("Accept-Ranges: bytes");
                   p.outputStream.WriteLine("Server: GhostReplay");
                   p.outputStream.WriteLine("Pragma: no-cache");
                   p.outputStream.WriteLine("Cash-Control: no-cache");
                   p.outputStream.WriteLine("Expires: Thu, 01 Jan 1970 08:00:00 CST");
                   p.outputStream.WriteLine("Content-Length: " + buffer.Length.ToString());
                   p.outputStream.WriteLine("Connection: close");
                   p.outputStream.WriteLine("");
                   p.outputStream.WriteContent(buffer);
              

               }
               else if (p.http_url.StartsWith("/observer-mode/rest/consumer/"))
               {
                   // Replay handler

                   string RequestedURL = p.http_url.Replace("/observer-mode/rest/consumer/", "");
                   //  [DISABLED] Console.Clear();
                   //  [DISABLED] Console.WriteLine("Requested  " + RequestedURL + "   "+DateTime.Now.ToLongTimeString());
                   string client = p.socket.Client.RemoteEndPoint.ToString().Split(':')[0];




                   //     if (hr.IsHeroSpawned)
                   //  [DISABLED] Console.Title = "Champions Spawned";

                   byte[] buffer = new byte[0];

                   if (RequestedURL == "version")
                   {
                       buffer = Encoding.UTF8.GetBytes(Version);
                       content = "text/plain";
                   }
                   else if (RequestedURL == "end")
                   {
                       buffer = Encoding.UTF8.GetBytes("done");
                       content = "text/plain";
                       //  ShutdownAfterQuery = true;
                   }
                   else if (RequestedURL == "reset")
                   {

                       ResetForClient(client);
                       buffer = Encoding.UTF8.GetBytes("done");
                       content = "text/plain";
                       //  ShutdownAfterQuery = true;
                   }
                   string[] Params = RequestedURL.Split('/');
                   if (Params.Length > 2)
                   {


                       string platform = Params[1];
                       string gid = Params[2];


                       if (IsStreaming(gid + "-" + platform))
                       {
                           GhostbladeStream stream = GetStream(gid + "-" + platform);
                           if (!stream.Clients.ContainsKey(client))
                               stream.Clients.Add(client, new HeroSpawn());

                           HeroSpawn hr = stream.Clients[client];
                           if (Params[0] == "getGameMetaData")
                           {
                               content = "application/json";

                               buffer = stream.CurrentReplay.GetMetaData();
                           }
                           else if (Params[0] == "getLastChunkInfo")
                           {
                               ChunkInfo ci = stream.CurrentReplay.LastChunkInfo;

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
                                   buffer = stream.CurrentReplay.GetChunkInfo(stream.FChunk);


                               else if (hr.IsHeroSpawned && (DateTime.Now - hr.FirstChunkInfo).TotalMilliseconds < 9000)
                                   buffer = stream.CurrentReplay.GetChunkInfo(stream.FChunk);

                               else
                                   buffer = stream.CurrentReplay.GetChunkInfo(stream.CurrentReplay.LastChunkInfo);


                           }
                           else if (Params[0] == "getGameDataChunk")
                           {
                               //if (Convert.ToInt32(Params[3]) == (LastChunk - 1))
                               //    Params[3] = LastChunk.ToString();

                               buffer = stream.CurrentReplay.GetChunk(short.Parse(Params[3]));

                               if (Convert.ToInt32(Params[3]) >= stream.LatestChunk)
                               {
                                   stream.LatestChunk += 1;
                                   stream.NextChunk += 1;

                                   if (stream.LatestChunk >= 7)
                                   {
                                       if (stream.LatestChunk % 2 == 0)
                                           stream.LatestKeyframe += 1;
                                   }
                               }
                               content = "application/octet-stream";

                           }
                           else if (Params[0] == "getKeyFrame")
                           {
                               content = "application/octet-stream";
                               buffer = stream.CurrentReplay.GetKey(short.Parse(Params[3]));
                           }






                       }
                       else
                       {
                           // no stream
                           SERVED = false;
                           buffer = Encoding.UTF8.GetBytes(Ghostblade.Properties.Resources.NOTFOUND.Replace("{$msg$}", "Stream unavailable : " + gid + "-" + platform));
                       }

                   }




                   CultureInfo provider = new CultureInfo("en-US");
                   if (SERVED && buffer != null)
                       p.outputStream.WriteLine("HTTP/1.1 200 OK");
                   else
                   {
                       content = "text/html";
                       p.outputStream.WriteLine("HTTP/1.0 404 File not found");

                       if (buffer == null)
                           buffer = Encoding.UTF8.GetBytes(Ghostblade.Properties.Resources.NOTFOUND.Replace("{$msg$}", "Not found"));
                   }
                   p.outputStream.WriteLine("Content-Type: " + content);

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
               //if (ShutdownAfterQuery)
               //    Environment.Exit(0);
           }
           catch(Exception ex)
           {
               byte[] buffer = Encoding.UTF8.GetBytes(Ghostblade.Properties.Resources.NOTFOUND.Replace("{$msg$}", "Internal Error " + ex.Message));
                             p.outputStream.WriteLine("HTTP/1.0 404 File not found");
                   
                   p.outputStream.WriteLine("Content-Type: " + "text/html");

                   p.outputStream.WriteLine("Accept-Ranges: bytes");
                   p.outputStream.WriteLine("Server: GhostReplay");
                   p.outputStream.WriteLine("Pragma: no-cache");
                   p.outputStream.WriteLine("Cash-Control: no-cache");
                   p.outputStream.WriteLine("Expires: Thu, 01 Jan 1970 08:00:00 CST");
                   p.outputStream.WriteLine("Content-Length: " + buffer.Length.ToString());
           //        p.outputStream.WriteLine("Date: " + DateTime.UtcNow.ToString("ddd, dd MM yyyy HH:mm:ss GMT", provider));
                   p.outputStream.WriteLine("Connection: close");
                   p.outputStream.WriteLine("");
                   p.outputStream.WriteContent(buffer);
           }
       }
       public override void handlePOSTRequest(HttpProcessor p, System.IO.MemoryStream inputData)
       {
           p.writeFailure();
           p.outputStream.WriteLine("ACCESS DENIED");
       }
    }
}
