using RtmpSharp.IO;
using System;
using System.Collections.Generic;

namespace GBReplay.Replays.Riot
{
    [Serializable]
    [SerializedName("com.riotgames.platform.observer.domain.EndOfGameStats")]
    public class EndOfGameStats
    {

 

        [SerializedName("ranked")]
        public Boolean Ranked { get; set; }

        [SerializedName("gameType")]
        public String GameType { get; set; }

  


        [SerializedName("teamPlayerParticipantStats")]
        public List<PlayerParticipantStatsSummary> TeamPlayerParticipantStats { get; set; }

        [SerializedName("difficulty")]
        public String Difficulty { get; set; }

        [SerializedName("gameLength")]
        public Double GameLength { get; set; }

        [SerializedName("invalid")]
        public Boolean Invalid { get; set; }

        [SerializedName("otherTeamInfo")]
        public TeamInfo OtherTeamInfo { get; set; }

    

        [SerializedName("otherTeamPlayerParticipantStats")]
        public List<PlayerParticipantStatsSummary> OtherTeamPlayerParticipantStats { get; set; }


        [SerializedName("gameId")]
        public Double GameId { get; set; }

 

        [SerializedName("gameMode")]
        public String GameMode { get; set; }

        [SerializedName("myTeamInfo")]
        public TeamInfo MyTeamInfo { get; set; }

        [SerializedName("queueType")]
        public String QueueType { get; set; }



        [SerializedName("myTeamStatus")]
        public String MyTeamStatus { get; set; }

    }


    public class BasicInfo
    {
        public byte Map { get; set; }
        public byte Queue { get; set; } 
        public string SummonerName { get; set; }
        public BasicInfo()
        {
            Map = 0;
            Queue = 0;
            SummonerName = "";
        }
    }
}