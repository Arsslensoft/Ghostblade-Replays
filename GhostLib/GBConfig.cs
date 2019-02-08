using GhostLib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib
{
    public enum AccountTier
    {
        UnrankedOrBronze = 0,
        Silver = 1,
        Gold = 2,
        Plat = 3,
        Diamond = 4,
        Master = 5,
        Challenger = 6
    }
    public class GBAccount
    {
        [JsonProperty("sum")]
        public string SummonerName { get; set; }

        [JsonProperty("sid")]
        public long PlayerID { get; set; }

        [JsonProperty("region")]
        public RiotSharp.Region Region { get; set; }

        [JsonProperty("pbesum")]
        public string PbeSummonerName { get; set; }

        [JsonProperty("pbesid")]
        public long PbePlayerID { get; set; }

        [JsonProperty("sicon")]
        public int SummonerIconId { get; set; }


        [JsonProperty("stier")]
        public AccountTier SummonerTier { get; set; }

        [JsonProperty("sumlvl")]
        public long SummonerLevel { get; set; }

    }
  public class GBConfig
    {
      // [JsonProperty("sum")]
      //public string SummonerName { get; set; }

        //[JsonProperty("sid")]
        //public long PlayerID { get; set; }

        //   [JsonProperty("region")]
        //public RiotSharp.Region Region { get; set; }

      [JsonProperty("account")]
      public List<GBAccount> Accounts { get; set; }

       [JsonProperty("pport")]
       public int NetPort { get; set; }


      [JsonProperty("api")]
      public string ApiKey { get; set; }

       [JsonProperty("directory")]
      public string GameDirectory { get; set; }

       [JsonProperty("dragonver")]
       public string DragonVersion { get; set; }

       [JsonProperty("gamever")]
       public string GameVersion { get; set; }

       [JsonProperty("tts")]
       public bool Speech { get; set; }

       [JsonProperty("slog")]
       public bool SendLogs { get; set; }

       [JsonProperty("ril")]
       public bool RecordIfLate { get; set; }

       [JsonProperty("recdir")]
       public string RecordingDirectory { get; set; }

       [JsonProperty("pingE")]
       public bool PingEnabled { get; set; }

       [JsonProperty("ghserv")]
       public string MainServer { get; set; }
     
      [JsonProperty("cid")]
       public string ClientId { get; set; }

      [JsonProperty("netinterface" )]
      public string NetworkInterface { get; set; }


      [JsonProperty("proxytype")]
      public ProxyType ProxyOption { get; set; }


      [JsonProperty("proxyhost")]
      public string ProxyHost { get; set; }
      [JsonProperty("proxyport")]
      public int ProxyPort { get; set; }
      [JsonProperty("proxyuser")]
      public string ProxyUser { get; set; }
      [JsonProperty("proxypass")]
      public string ProxyPass { get; set; }


      [JsonProperty("apicache")]
      public bool ApiCacheEnabled { get; set; }

      [JsonProperty("helper")]
      public bool HelperEnabled { get; set; }

      [JsonProperty("goverlay")]
      public bool GhostOverlayEnabled { get; set; }

      [JsonProperty("gstreamport")]
      public int GhostStreamPort { get; set; }

      [JsonProperty("haspbe")]
      public bool HasPBE { get; set; }

      [JsonProperty("pbedir")]
      public string PbeDirectory { get; set; }

      [JsonProperty("pbever")]
      public string PbeVersion { get; set; }




      [JsonProperty("glocale")]
      public GhostLib.Lang.GhostLanguage Language { get; set; }


      [JsonProperty("autogi")]
      public bool AutoGameInfo { get; set; }

      [JsonProperty("automove")]
      public bool AutoMoveOld { get; set; }


        [JsonProperty("bgimg")]
        public string TopBannerBg { get; set; }

        [JsonProperty("animator")]
        public bool AnimatorEnabled { get; set; }

        [JsonProperty("advginfo")]
        public bool AdvancedGameInfo { get; set; }

        [JsonProperty("autorecord")]
        public bool AutoRecordGame { get; set; }
      
      [JsonProperty("portforwarding")]
        public bool PortForwarding { get; set; }



        [JsonProperty("featuredrecord")]
        public string[] FollowedSummoners { get; set; }

        [JsonProperty("goverlaylist")]
        public GOverlay[] Overlays { get; set; }
    }
}
