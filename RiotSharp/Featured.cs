using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotSharp.Featured
{

    public class Participant
    {
        public int teamId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public int championId { get; set; }
        public int profileIconId { get; set; }
        public string summonerName { get; set; }
        public bool bot { get; set; }
    }

    public class Observers
    {
        public string encryptionKey { get; set; }
    }

    public class BannedChampion
    {
        public int championId { get; set; }
        public int teamId { get; set; }
        public int pickTurn { get; set; }
    }

    public class GameList
    {
        public long gameId { get; set; }
        public int mapId { get; set; }
        public string gameMode { get; set; }
        public string gameType { get; set; }
        public int gameQueueConfigId { get; set; }
        public List<Participant> participants { get; set; }
        public Observers observers { get; set; }
        public string platformId { get; set; }
        public List<BannedChampion> bannedChampions { get; set; }
        public object gameStartTime { get; set; }
        public int gameLength { get; set; }
    }

    public class FeaturedGames
    {
        public List<GameList> gameList { get; set; }
        public int clientRefreshInterval { get; set; }
    }
}
