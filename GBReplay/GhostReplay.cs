using GBReplay.Replays.Game;
using GBReplay.Replays.Riot;
using Newtonsoft.Json;
using RtmpSharp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay.Replays
{
    /// <summary>
    /// League of Legends Ghostblade Replay File
    /// </summary>
  public  class GhostReplay : IDisposable
    {
      public ReplaySignature Signer { get; set; }
      public ReplayHeader Header { get; set; }
    

      public string ObserverKey
      {
          get
          {
              if (Header != null)
                  return Convert.ToBase64String(Header.ObserverKey.Data);
              else return "";
          }
          set
          {
              if (Header != null)
                  Header.ObserverKey = new RawEntry8(Convert.FromBase64String(value));

          }
      }

      public Version64 LolVersion
      {
          get
          {
              if(Header != null)
               return Header.ClientVersion;
              else return Version64.Parse(0);
          }
          set
          {
         if(Header != null)
                       Header.ClientVersion= value;
          
          }

      }
      public ReplayInfo Flags
      {
          get
          {
              if(Reader != null)
               return Reader.ReplayFlags;
              else return ReplayInfo.StandardFormat;
          }

      }
      public string Version
      {
          get
          {
              if (Header != null)
                  return  Header.ServerVersion.ToString();
              else return "";
          }
          set
          {
              if (Header != null)
                  Header.ServerVersion = Version24.Parse(value);

          }
      }
     
      public string SummonerName {
          get
          {
              if (PlayerInfos != null)
                  return PlayerInfos.SummonerName;
              else return "";
          }
          set
          {
              if (PlayerInfos != null)
                  PlayerInfos.SummonerName = value;

          }
      }

      long _gid = 0; 
      public long GameId
      {
          get
          {
              if (MetaData != null)
                  _gid = MetaData.gameKey.gameId;
                
              return _gid;
              
          }
          set
          {
              _gid = value;
          
          }
      }
      string _reg = "";
      public string Platform
      {
          get
          {
              if (MetaData != null)
                  _reg = MetaData.gameKey.platformId;

              return _reg;

          }
          set
          {
              _reg = value;

          }
      }


      public DateTime RecordDate
      {
          get {
              if (MetaData != null)
                  return DateTime.Parse(MetaData.endTime);
              else return new FileInfo(FileName).CreationTime;
          }
      }
      public TimeSpan GameLength {
          get
          {
              if (MetaData != null)
                  return new TimeSpan(MetaData.gameLength * TimeSpan.TicksPerMillisecond);
              else
                  return new TimeSpan(0);
          }
       }
      public string GameVersion {
          get
          {
              return LolVersion.ToString();
          }
          set
          {
              LolVersion = Version64.Parse(value);
          }
      }


      // Key Section
      public List<GameData> Keys { get; set; }
      // Chunks Section
      public List<GameData> Chunks { get; set; }

    // Ressource Section
      public GBReplay.Replays.Game.GameMetaData MetaData { get; set; }
      public GBReplay.Replays.Riot.EndOfGameStats GameStats { get; set; }
      public GBReplay.Replays.Game.ChunkInfo LastChunkInfo { get; set; }
      public GBReplay.Replays.Riot.BasicInfo PlayerInfos { get; set; }

      public byte[] GameStatsRaw { get; set; }

      #region IO FILE

      public ReplayWriter Writer { get; set; }
      public Stream ReplayStream { get; set; }
      public ReplayReader Reader { get; set; }
      public string FileName { get; set; }

      #endregion



      #region Replay

      public byte[] GetChunk(short id)
      {
          foreach (GameData g in Chunks)
              if (g.Id == id)
                  return g.Data;

          return null;
      }
      public byte[] GetKey(short id)
      {
          foreach (GameData g in Keys)
              if (g.Id == id)
                  return g.Data;

          return null;
      }
      public byte[] GetMetaData()
      {
          return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(MetaData));
      }
      public byte[] GetChunkInfo(ChunkInfo ci)
      {
          return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ci));

      }


      #endregion


      public bool IsPBE { get; set; }
      public bool UseCache { get; set; }
      public string CacheDirectory { get; set; }

      public GhostReplay(string file, string gameversion, bool usecache, string tempdir)
        {
            IsPBE = File.Exists(tempdir + @"\ISPBE");
             
            PlayerInfos = new BasicInfo();
            Header = new ReplayHeader();
            UseCache = usecache;
            CacheDirectory = tempdir + @"\" + Path.GetFileNameWithoutExtension(file);
            GameVersion = gameversion;
         
            Keys = new List<GameData>();
            Chunks = new List<GameData>();
      
            FileName = file;
            if (UseCache)
            {
                if (!Directory.Exists(CacheDirectory))
                    Directory.CreateDirectory(CacheDirectory);
                RestoreFromCache();
            }
        }
      public GhostReplay(string file, string gameversion, bool usecache, string tempdir, byte[] cert,string pass)
      {
          PlayerInfos = new BasicInfo();
          Header = new ReplayHeader();
          UseCache = usecache;
          CacheDirectory = tempdir + @"\" + Path.GetFileNameWithoutExtension(file);
          GameVersion = gameversion;
          Signer = new ReplaySignature(cert, pass);
          Keys = new List<GameData>();
          Chunks = new List<GameData>();

          FileName = file;
          if (UseCache)
          {
              if (!Directory.Exists(CacheDirectory))
                  Directory.CreateDirectory(CacheDirectory);
              RestoreFromCache();
          }
      }
 
      public void DeleteDirectory(string target_dir)
      {
          string[] files = Directory.GetFiles(target_dir);
          string[] dirs = Directory.GetDirectories(target_dir);

          foreach (string file in files)
          {
              File.SetAttributes(file, FileAttributes.Normal);
              File.Delete(file);
          }

          foreach (string dir in dirs)
          {
              DeleteDirectory(dir);
          }

          Directory.Delete(target_dir, false);
      }
      public void RestoreFromCache()
      {
          UseCache = false;
          try
          {
              if (Directory.Exists(CacheDirectory))
              {
                  foreach (string file in Directory.GetFiles(CacheDirectory, "*.*", SearchOption.TopDirectoryOnly))
                  {
                      string name = Path.GetFileName(file);

                      if (name.StartsWith("key-"))
                      {
                          // key file
                          byte id = byte.Parse(name.Split('-')[1]);
                          AddKey(File.ReadAllBytes(file), id);

                      }
                      else if (name.StartsWith("chunk-"))
                      {
                          // chunk file
                          byte id = byte.Parse(name.Split('-')[1]);
                          AddChunk(File.ReadAllBytes(file), id);

                      }
                  }
              }
          }
          catch
          {


          }
          finally
          {
              UseCache = true;
          }

      }
      public byte[] Extract(byte[] data, int offset, int length)
      {
          if (data.Length > length && offset >= 0 && offset + length <= data.Length)
          {
              byte[] d = new byte[length];
              for (int i = offset; i < offset + length; i++)
                  d[i - offset] = data[i];

              return d;
          }
          else
              return null;
      }
      bool ContainsId(List<GameData> gd, short id)
      {
          foreach (GameData g in gd)
              if (g.Id == id)
                  return true;

          return false;
      }
      void SortData(GameData[] lg)
      {
          Array.Sort(lg, new GameDataComparer());
      }
      public void RecoverMissing(int keyframe, int chunkid, string path, out  List<int> missingKeys, out  List<int> missingChunks)
      {
          missingKeys = new List<int>();
          missingChunks = new List<int>();
          try
          {


              for (int i = 1; i <= keyframe; i++)
              {
                  if (!ContainsId(Keys, (short)i))
                      missingKeys.Add(i);
              }
              for (int j = 1; j <= chunkid; j++)
              {
                  if (!ContainsId(Chunks, (short)j))
                      missingChunks.Add(j);
              }
          }
          catch (Exception ex)
          {

          }
      }

      public bool AddKey(byte[] key, byte id)
      {
          if (!ContainsId(Keys, id))
          {
              Keys.Add(new GameData(key, id));
              if (UseCache)
                  File.WriteAllBytes(CacheDirectory + @"\key-" + id.ToString(), key);

              return true;
          }
          return false;
      }
      public bool AddChunk(byte[] chunk, byte id)
      {
          if (!ContainsId(Chunks, id))
          {
              Chunks.Add(new GameData(chunk, id));

              if (UseCache)
                  File.WriteAllBytes(CacheDirectory + @"\chunk-" + id.ToString(), chunk);

              return true;
          }
          return false;

      }


      public bool ParseStats(byte[] stat)
      {
          try
          {
              if (stat != null)
              {
                  GameStatsRaw = stat;
                  SerializationContext context = new SerializationContext();

                  //Convert replay end of game stats to parsable object
                  context.Register(typeof(EndOfGameStats));
                  context.Register(typeof(PlayerParticipantStatsSummary));
                  context.Register(typeof(RawStatDTO));


                  AmfReader statsReader = new AmfReader(new MemoryStream(stat), context);

                  GameStats = (EndOfGameStats)statsReader.ReadAmf3Item();
              }
              else GameStats = null;
              return true;
          }
          catch
          {
              return false;
          }
      }
      public bool ParseMetaData(string meta)
      {
          try
          {
              MetaData = JsonConvert.DeserializeObject<Game.GameMetaData>(meta);
              return false;
          }
          catch
          {

          }
          return false;
      }
      public bool ParseLastChunkInfo(string chunkinfo)
      {
          try
          {
              LastChunkInfo = JsonConvert.DeserializeObject<Game.ChunkInfo>(chunkinfo);
              return false;
          }
          catch
          {

          }
          return false;
      }



      void ParseSections(List<ReplaySection> Sections)
      {
          byte id = 0;
          foreach (ReplaySection sec in Sections)
          {
              if (sec.Type == SectionType.Keys)
              {
                  id = 0;
                  foreach (RawEntry24 re in sec.GetRawEntries())
                  {
                      id++;
                      byte[] dt = re.Data;
                      Keys.Add(new GameData(dt, id));
                  }

              }
              else if (sec.Type == SectionType.Chunks)
              {
                  id = 0;
                  foreach (RawEntry24 re in sec.GetRawEntries())
                  {
                      id++;
                      byte[] dt = re.Data;
                      Chunks.Add(new GameData(dt, id));
                  }

              }
              else if (sec.Type == SectionType.ChunkInfo)
              {
                  string json = Encoding.UTF8.GetString(sec.Data);
                  ParseLastChunkInfo(json);
              }
              else if (sec.Type == SectionType.MetaData)
              {
                  string json = Encoding.UTF8.GetString(sec.Data);
                  ParseMetaData(json);
              }
              else if (sec.Type == SectionType.EndOfGameStats)
                  ParseStats(sec.Data);

              else if (sec.Type == SectionType.Informations)
              {
                  PlayerInfos.Map = sec.Data[0];
                  PlayerInfos.Queue = sec.Data[1];
                  PlayerInfos.SummonerName = Encoding.UTF8.GetString(sec.Data, 2, sec.Data.Length - 2);
              }
          
        
          }
      }
      public bool ReadReplay(ReadMode mode)
      {
          ReplayStream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
          // byte flag = (byte)ReplayStream.ReadByte();
          Reader = new ReplayReader(ReplayStream);
          try
          {
            
              // Read File
              if (mode == ReadMode.HeaderOnly)
              {
                 Header = Reader.ReadHeader();
                  ReplayStream.Flush();
                  Reader.Close();
              
                  
                  
              }
              else if (mode == ReadMode.All)
              {
                  // Header
                  Header = Reader.ReadHeader();
       
                  // Sections
                  List<ReplaySection> secs = new List<ReplaySection>();
                  while (!Reader.EOF())
                  {
                      ReplaySection se =Reader.ReadSection();
                      secs.Add(se);

                  }
         
               
                  
           //  ReplayStream.Flush();
                  Reader.Close();
                  ParseSections(secs);

              }
              else if (mode == ReadMode.AllExceptData)
              {
                  // Header
                  Header = Reader.ReadHeader();

                  // Sections
                  List<ReplaySection> secs = new List<ReplaySection>();
                  while (!Reader.EOF())
                  {
                      ReplaySection se = Reader.ReadSection();
                      if(se.Type !=  SectionType.Keys && se.Type != SectionType.Chunks)
                      secs.Add(se);

                  }



                  //  ReplayStream.Flush();
                  Reader.Close();
                  ParseSections(secs);

              }
              else if (mode == ReadMode.DataOnly)
              {
                  // Header
                  Header = Reader.ReadHeader();

                  // Sections
                  List<ReplaySection> secs = new List<ReplaySection>();
                  while (!Reader.EOF())
                  {
                      ReplaySection se = Reader.ReadSection();
                      if (se.Type == SectionType.Keys || se.Type == SectionType.Chunks)
                          secs.Add(se);

                  }



                  //  ReplayStream.Flush();
                  Reader.Close();
                  ParseSections(secs);
              }

              return true;


          }
          catch
          {

          }
          finally
          {
           
          
  
           

          }
          return false;
      }
    
      public bool Save(bool sign, ReplayInfo flag)
      {
          if (File.Exists(FileName))
              File.Delete(FileName);
          ReplayStream = File.Create(FileName);
         
          if(sign && (flag & ReplayInfo.Signed) != ReplayInfo.Signed)
              flag |= ReplayInfo.Signed;


          Writer = new ReplayWriter(ReplayStream, flag);
          if (SaveReplay(sign))
          {


          //    Writer.Compress(ReplayStream, FileName);
              ReplayStream.Close();
              Writer.Dispose();
              //File.Delete(FileName);
              //File.Move(FileName + ".gz", FileName);
              if (Directory.Exists(CacheDirectory))
                  DeleteDirectory(CacheDirectory);
              return true;
          }

          ReplayStream.Close();
          Writer.Dispose();
          File.Delete(FileName);
          return false;
      }
      bool WriteSections()
      {
          
 

             // SORT
              GameData[] schunks = Chunks.ToArray();
              GameData[] skeys = Keys.ToArray();
              SortData(schunks);
              SortData(skeys);
              // Last Chunk
             string json = JsonConvert.SerializeObject(LastChunkInfo);
             Writer.Write(StringToSection(json, SectionType.ChunkInfo), false);

              // Meta Data
             json = JsonConvert.SerializeObject(MetaData);
             Writer.Write(StringToSection(json, SectionType.MetaData),false);


             // End of Game Stats
             if (GameStats != null)
                  Writer.Write(RawToSection(GameStatsRaw, SectionType.EndOfGameStats),false);


              // Informations
              byte[] sum = Encoding.UTF8.GetBytes(SummonerName);
              Writer.Write(RawToSection(BinaryUtil.Append(new byte[2] { PlayerInfos.Map, PlayerInfos.Queue }, sum, 2, sum.Length), SectionType.Informations), false);
           
             //// Chunks
             List<RawEntry24> ent = new List<RawEntry24>();
             foreach (GameData c in schunks)
                 ent.Add(new RawEntry24(c));

         Writer.Write(new ReplaySection(ent, SectionType.Chunks), false);
             
             // Keys
             ent.Clear();
             foreach (GameData k in skeys)
                 ent.Add(new RawEntry24(k));
           
              // FINAL SECTION FOR COMPUTE HASH
             Writer.Write(new ReplaySection(ent, SectionType.Keys),true);
           

           

          //   Writer.WriteByte(65);
              return true;
      

      }
 
      public bool SaveReplay(bool sign)
      {
          if (sign)
          {
            
                  //// Sign


                  // Write Header
              if (Signer != null)
              {
                  Header.Signature = new RawEntry16(new byte[256]);
                  Header.Certificate = new RawEntry16(Signer.MixedCert.GetRawCertData());

              }
              long hpos = ReplayStream.Position;
                  Writer.Write(Header);

                  // Write Sections
                  WriteSections();
              //Signature
                  if (Signer != null)
                  {
                      SignStream();
                      ReplayStream.Seek(hpos, SeekOrigin.Begin);
                   
                      Writer.Write(Header);
                  }
             
                  return true;
              
          }
          else
          {

              // Write Header
              Writer.Write(Header);
              // Write Sections
              WriteSections();

                  return true;
           
             
          }
        
      }

  
      ReplaySection StringToSection(string data, SectionType sec)
      {
          return new ReplaySection(Encoding.UTF8.GetBytes(data), sec);
      }
      ReplaySection RawToSection(byte[] data, SectionType sec)
      {
          return new ReplaySection(data, sec);
      }

      bool SignStream()
      {
          if (Signer != null)
          {
          Header.Signature = new RawEntry16(    Signer.Sign(Writer.Hasher.ComputeHash(), Signer.MixedCert));
             
            
              return true;
          }
          else return false;
      }
      public bool Verify()
      {
          if (Signer != null)
          {

              Signer = new ReplaySignature(Header.Certificate.Data);
              bool b = Signer.Verify(Reader.Hasher.ComputeHash(), Header.Signature.Data, Signer.MixedCert);
    
              return b;
          }
          else return false;
      }
  /// <summary>
    /// Destructor
    /// </summary>
    ~GhostReplay()
    {
        Dispose(false);
    }
 public void Dispose()
{
   // Dispose of unmanaged resources.
   Dispose(true);
   // Suppress finalization.
   GC.SuppressFinalize(this);
}
       /// <summary>
   
bool disposed;
    /// <summary>
    /// The virtual dispose method that allows
    /// classes inherithed from this one to dispose their resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Dispose managed resources here.
                Header = null;
         
                GameStats = null;
                Chunks.Clear();
                Keys.Clear();
                Chunks = null;
                Keys = null;
                    
            }

            // Dispose unmanaged resources here.
        }

        disposed = true;
    }

    }
}
