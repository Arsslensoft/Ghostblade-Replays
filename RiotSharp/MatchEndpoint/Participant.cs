using System;
using System.Collections.Generic;
using RiotSharp.LeagueEndpoint;
using Newtonsoft.Json;

namespace RiotSharp.MatchEndpoint
{


    public class Observers
    {
        [JsonProperty("encryptionKey")]
        public string encryptionKey { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("gameId")]
        public long gameId { get; set; }
        [JsonProperty("mapId")]
        [JsonConverter(typeof(MapTypeConverter))]
        public MapType MapType { get; set; }

        [JsonProperty("gameMode")]
        [JsonConverter(typeof(GameModeConverter))]
        public GameMode gameMode { get; set; }

        [JsonProperty("gameType")]
        [JsonConverter(typeof(GameTypeConverter))]
        public GameType gameType { get; set; }

        [JsonProperty("gameQueueConfigId")]
        public QueueType gameQueueConfigId { get; set; }

        [JsonProperty("participants")]
        public List<Participant> participants { get; set; }
        [JsonProperty("observers")]
        public Observers observers { get; set; }
        [JsonProperty("platformId")]
        public string platformId { get; set; }

        [JsonProperty("bannedChampions")]
        public List<BannedChampion> bannedChampions { get; set; }

        [JsonProperty("gameStartTime")]
        [JsonConverter(typeof(DateTimeConverterFromLong))]
        public DateTime gameStartTime { get; set; }

        [JsonProperty("gameLength")]
        public int gameLength { get; set; }
    }
    /// <summary>
    /// Class representing a participant in a match (Match API).
    /// </summary>
    [Serializable]
    public class Participant
    {
        internal Participant() { }

        /// <summary>
        /// Champion ID.
        /// </summary>
        [JsonProperty("championId")]
        public int ChampionId { get; set; }

        /// <summary>
        /// List of mastery information.
        /// </summary>
        [JsonProperty("masteries")]
        public List<Mastery> Masteries { get; set; }

        /// <summary>
        /// Participant ID.
        /// </summary>
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }


        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty("summonerName")]
        public string Name { get; set; }


        /// <summary>
        /// summonerId.
        /// </summary>
        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        /// <summary>
        /// List of rune information.
        /// </summary>
        [JsonProperty("runes")]
        public List<Rune> Runes { get; set; }

        /// <summary>
        /// First summoner spell ID.
        /// </summary>
        [JsonProperty("spell1Id")]
        public int Spell1Id { get; set; }

        /// <summary>
        /// Second summoner spell ID.
        /// </summary>
        [JsonProperty("spell2Id")]
        public int Spell2Id { get; set; }

        /// <summary>
        /// Participant statistics.
        /// </summary>
        [JsonProperty("stats")]
        public ParticipantStats Stats { get; set; }

        /// <summary>
        /// Team ID.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// Timeline data.
        /// </summary>
        [JsonProperty("timeline")]
        public ParticipantTimeline Timeline { get; set; }
        
        /// <summary>
        /// Highest achieved season tier.
        /// </summary>
        [JsonProperty("highestAchievedSeasonTier")]
        public Tier HighestAchievedSeasonTier { get; set; }
    }
}
