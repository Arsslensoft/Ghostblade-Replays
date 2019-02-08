using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay.Replays.Game
{

  

    public class ChunkInfo
    {
        public int chunkId { get; set; }
        public int availableSince { get; set; }
        public int nextAvailableChunk { get; set; }
        public int keyFrameId { get; set; }
        public int nextChunkId { get; set; }
        public int endStartupChunkId { get; set; }
        public int startGameChunkId { get; set; }
        public int endGameChunkId { get; set; }
        public int duration { get; set; }
    }

    public class GameKey
    {
        public long gameId { get; set; }
        public string platformId { get; set; }
    }

    public class PendingAvailableChunkInfo
    {
        public int id { get; set; }
        public int duration { get; set; }
        public string receivedTime { get; set; }
    }

    public class PendingAvailableKeyFrameInfo
    {
        public int id { get; set; }
        public string receivedTime { get; set; }
        public int nextChunkId { get; set; }
    }

    public class GameMetaData
    {
        public GameKey gameKey { get; set; }
        public string gameServerAddress { get; set; }
        public int port { get; set; }
        public string encryptionKey { get; set; }
        public int chunkTimeInterval { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public bool gameEnded { get; set; }
        public int lastChunkId { get; set; }
        public int lastKeyFrameId { get; set; }
        public int endStartupChunkId { get; set; }
        public int delayTime { get; set; }
        public List<PendingAvailableChunkInfo> pendingAvailableChunkInfo { get; set; }
        public List<PendingAvailableKeyFrameInfo> pendingAvailableKeyFrameInfo { get; set; }
        public int keyFrameTimeInterval { get; set; }
        public string decodedEncryptionKey { get; set; }
        public int startGameChunkId { get; set; }
        public int gameLength { get; set; }
        public int clientAddedLag { get; set; }
        public bool clientBackFetchingEnabled { get; set; }
        public int clientBackFetchingFreq { get; set; }
        public int interestScore { get; set; }
        public bool featuredGame { get; set; }
        public string createTime { get; set; }
        public int endGameChunkId { get; set; }
        public int endGameKeyFrameId { get; set; }
    }
   //public static class RecordFinisher
   // {
   //    static void Prepare(string metapath, string lcpath)
   //    {
   //        try
   //        {

   //            GameMetaData gmd = JsonConvert.DeserializeObject<GameMetaData>(File.ReadAllText(metapath));
   //            ChunkInfo ci = JsonConvert.DeserializeObject<ChunkInfo>(File.ReadAllText(lcpath));
   //           // ci.availableSince = 61646;
   //            ci.nextAvailableChunk = 1;
   //            ci.endGameChunkId = gmd.endGameChunkId;
   //            ci.startGameChunkId = gmd.startGameChunkId;
   //            ci.endStartupChunkId = gmd.endStartupChunkId;
               

   //            gmd.clientBackFetchingEnabled = true;
   //            gmd.clientBackFetchingFreq = 50;
   //            gmd.pendingAvailableChunkInfo.Clear();
   //            gmd.pendingAvailableKeyFrameInfo.Clear();

   //            string json = JsonConvert.SerializeObject(gmd);
   //            File.WriteAllText(metapath, json);
   //            json = JsonConvert.SerializeObject(ci);
   //            File.WriteAllText(lcpath, json);
   //        }
   //        catch (Exception ex)
   //        {
   //            Log.LogEx(ex);
   //        }
   //    }

   //    public static void PrepareRecord(string path)
   //    {
   //        Prepare(path+@"\token", path + @"\CI");
   //    }
   // }
}
