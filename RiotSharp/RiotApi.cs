using Newtonsoft.Json;
using RiotSharp.ChampionEndpoint;
using RiotSharp.GameEndpoint;
using RiotSharp.LeagueEndpoint;
using RiotSharp.MatchEndpoint;
using RiotSharp.StatsEndpoint;
using RiotSharp.StatusEndpoint;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RiotSharp
{
    /// <summary>
    /// Entry point for the API.
    /// </summary>
    public class RiotApi
    {
        private const string SummonerRootUrl = "/api/lol/{0}/v1.4/summoner";
        private const string ByNameUrl = "/by-name/{0}";
        private const string NamesUrl = "/{0}/name";
        private const string MasteriesUrl = "/{0}/masteries";
        private const string RunesUrl = "/{0}/runes";

        private const string ChampionRootUrl = "/api/lol/{0}/v1.2/champion";

        private const string GameRootUrl = "/api/lol/{0}/v1.3/game";
        private const string RecentGamesUrl = "/by-summoner/{0}/recent";

        private const string LeagueRootUrl = "/api/lol/{0}/v2.5/league";
        private const string LeagueChallengerUrl = "/challenger";
        private const string LeagueByTeamUrl = "/by-team/{0}";
        private const string LeagueBySummonerUrl = "/by-summoner/{0}";
        private const string LeagueEntryUrl = "/entry";

        private const string TeamRootUrl = "/api/lol/{0}/v2.4/team";
        private const string TeamBySummonerURL = "/by-summoner/{0}";

        private const string StatsRootUrl = "/api/lol/{0}/v1.3/stats";
        private const string StatsSummaryUrl = "/by-summoner/{0}/summary";
        private const string StatsRankedUrl = "/by-summoner/{0}/ranked";

        private const string MatchRootUrl = "/api/lol/{0}/v2.2/match";
        private const string MatchHistoryRootUrl = "/api/lol/{0}/v2.2/matchhistory";
        private const string CurrentGameRootUrl = "/observer-mode/rest/consumer/getSpectatorGameInfo/{0}/{1}";
        private const string FeaturedGameRootUrl = "http://{0}/observer-mode/rest/featured";

        private const string StaticDataRealmRootUrl = "/api/lol/static-data/{0}/v1.2/versions";

        private const string IdUrl = "/{0}";

        private RateLimitedRequester requester;

        private static RiotApi instance;
        /// <summary>
        /// Get the instance of RiotApi.
        /// </summary>
        /// <param name="apiKey">The api key.</param>
        /// <param name="rateLimitPer10s">The 10 seconds rate limit for your production api key.</param>
        /// <param name="rateLimitPer10m">The 10 minutes rate limit for your production api key.</param>
        /// <returns>The instance of RiotApi.</returns>
        public static RiotApi GetInstance(string apiKey, int rateLimitPer10s = 10, int rateLimitPer10m = 500)
        {
            if (instance == null || apiKey != RateLimitedRequester.ApiKey ||
                rateLimitPer10s != RateLimitedRequester.RateLimitPer10S ||
                rateLimitPer10m != RateLimitedRequester.RateLimitPer10M)
            {
                instance = new RiotApi(apiKey, rateLimitPer10s, rateLimitPer10m);
            }
            return instance;
        }

        private RiotApi(string apiKey, int rateLimitPer10s, int rateLimitPer10m)
        {
            requester = RateLimitedRequester.Instance;
            RateLimitedRequester.ApiKey = apiKey;
            RateLimitedRequester.RateLimitPer10S = rateLimitPer10s;
            RateLimitedRequester.RateLimitPer10M = rateLimitPer10m;
        }

        /// <summary>
        /// Get a summoner by id synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for a summoner.</param>
        /// <param name="summonerId">Id of the summoner you're looking for.</param>
        /// <returns>A summoner.</returns>
        public Summoner GetSummoner(Region region, long summonerId, bool cached = true )
        {
            string json = null;
            if (ApiCache.CacheEnabled && cached && (json = ApiCache.GetSummoner(summonerId, region)) != null)
            {
                var obj = JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json).Values.FirstOrDefault();
                if (obj != null)
                {
                    obj.Region = region;
                }
                return obj;
            }
            else
            {

                json = requester.CreateRequest(
                   string.Format(SummonerRootUrl, region.ToString()) + string.Format(IdUrl, summonerId), region);
                var obj = JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json).Values.FirstOrDefault();
                if (obj != null)
                {
                    obj.Region = region;
                    if (ApiCache.CacheEnabled)
                        ApiCache.AddSummoner(obj, json, region);
                }
                return obj;
            }
        }

  
        /// <summary>
        /// Get summoners by ids synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for summoners.</param>
        /// <param name="summonerIds">List of ids of the summoners you're looking for.</param>
        /// <returns>A list of summoners.</returns>
        public List<Summoner> GetSummoners(Region region, List<long> summonerIds)
        {
            var json = requester.CreateRequest(
                string.Format(SummonerRootUrl, region.ToString()) + string.Format(IdUrl,
                    Util.BuildIdsString(summonerIds)),
                region);
            var list = JsonConvert.DeserializeObject<Dictionary<long, Summoner>>(json).Values.ToList();
            foreach (var summ in list)
            {
                summ.Region = region;
            }
            return list;
        }

      

        /// <summary>
        /// Get a summoner by name synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for a summoner.</param>
        /// <param name="summonerName">Name of the summoner you're looking for.</param>
        /// <returns>A summoner.</returns>
        public Summoner GetSummoner(Region region, string summonerName, bool cached =true)
        {
            string json = null;
            if (ApiCache.CacheEnabled && cached && (json = ApiCache.GetSummoner(summonerName, region)) != null)
            {
                var obj = JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json).Values.FirstOrDefault();
                if (obj != null)
                {
                    obj.Region = region;
                }
                return obj;
            }
            else
            {
                json = requester.CreateRequest(
                    string.Format(SummonerRootUrl, region.ToString()) +
                        string.Format(ByNameUrl, Uri.EscapeDataString(summonerName)),
                    region);
             

                var obj = JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json).Values.FirstOrDefault();
              
                if (obj != null)
                {
                    obj.Region = region;
                    if (ApiCache.CacheEnabled)
                        ApiCache.AddSummoner(obj, json, region);
                }
              
                return obj;
            }
        }

      

        /// <summary>
        /// Get summoners by names synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for summoners.</param>
        /// <param name="summonerNames">List of names of the summoners you're looking for.</param>
        /// <returns>A list of summoners.</returns>
        public List<Summoner> GetSummoners(Region region, List<string> summonerNames)
        {
            var json = requester.CreateRequest(
                string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(ByNameUrl, Util.BuildNamesString(summonerNames)),
                region);
            var list = JsonConvert.DeserializeObject<Dictionary<string, Summoner>>(json).Values.ToList();
            foreach (var summ in list)
            {
                summ.Region = region;
            }
            return list;
        }

    

        /// <summary>
        /// Get a  summoner's name and id synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for summoners.</param>
        /// <param name="summonerId">Id of the summoner you're looking for.</param>
        /// <returns>A summoner (id and name).</returns>
        public SummonerBase GetSummonerName(Region region, int summonerId)
        {
            var json = requester.CreateRequest(
                string.Format(SummonerRootUrl, region.ToString()) + string.Format(NamesUrl, summonerId), region);
            var child = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return new SummonerBase(child.Keys.FirstOrDefault(), child.Values.FirstOrDefault(), requester, region);
        }

   

        /// <summary>
        /// Get a list of summoner's names and ids synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for summoners.</param>
        /// <param name="summonerIds">List of ids of the summoners you're looking for.</param>
        /// <returns>A list of ids and names of summoners.</returns>
        public List<SummonerBase> GetSummonersNames(Region region, List<long> summonerIds)
        {
            var json = requester.CreateRequest(
                string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(NamesUrl, Util.BuildIdsString(summonerIds)),
                region);
            var summoners = new List<SummonerBase>();
            var children = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (var child in children)
            {
                summoners.Add(new SummonerBase(child.Key, child.Value, requester, region));
            }
            return summoners;
        }

     
        /// <summary>
        /// Get the list of champions by region synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for champions.</param>
        /// <returns>A list of champions.</returns>
        public List<Champion> GetChampions(Region region)
        {
            var json = requester.CreateRequest(string.Format(ChampionRootUrl, region.ToString()), region);
            return JsonConvert.DeserializeObject<ChampionList>(json).Champions;
        }

      
    
        /// <summary>
        /// Get a champion from its id synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for a champion.</param>
        /// <param name="championId">Id of the champion you're looking for.</param>
        /// <returns>A champion.</returns>
        public Champion GetChampion(Region region, int championId)
        {
            var json = requester.CreateRequest(
                string.Format(ChampionRootUrl, region.ToString()) + string.Format(IdUrl, championId), region);
            return JsonConvert.DeserializeObject<Champion>(json);
        }
        /// <summary>
        /// Get a champion from its id synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for a champion.</param>
        /// <param name="championId">Id of the champion you're looking for.</param>
        /// <returns>A champion.</returns>
        public RootObject GetCurrentGame(Region region, long id,string platform)
        {
            var json = requester.CreateRequest(
                string.Format(CurrentGameRootUrl, platform, id), region);
            return JsonConvert.DeserializeObject<RootObject>(json);
        }
        /// <summary>
        /// Get a champion from its id synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for a champion.</param>
        /// <param name="championId">Id of the champion you're looking for.</param>
        /// <returns>A champion.</returns>
        public RootObject GetCurrentGamePBE(Region region, long id, string platform)
        {
            var json = requester.CreateRequest(
                string.Format(CurrentGameRootUrl, "PBE1", id), Region.pbe);
            return JsonConvert.DeserializeObject<RootObject>(json);
        }

        public RiotSharp.Featured.FeaturedGames GetFeaturedGames(string server)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(FeaturedGameRootUrl, server));
            request.Timeout = 10000;
            request.ReadWriteTimeout = 10000;
            var wresp = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(wresp.GetResponseStream());
         string json =   sr.ReadToEnd();
         sr.Close();
            return JsonConvert.DeserializeObject<RiotSharp.Featured.FeaturedGames>(json);
        }

        /// <summary>
        /// Get mastery pages for a list summoners' ids synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for mastery pages for a list of summoners.</param>
        /// <param name="summonerIds">A list of summoners' ids for which you wish to retrieve the masteries.</param>
        /// <returns>A dictionary where the keys are the summoners' ids and the values are lists of mastery pages.
        /// </returns>
        public Dictionary<long, List<MasteryPage>> GetMasteryPages(Region region, List<long> summonerIds)
        {
            var json = requester.CreateRequest(
                string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(MasteriesUrl, Util.BuildIdsString(summonerIds)),
                region);
            return ConstructMasteryDict(JsonConvert.DeserializeObject<Dictionary<string, MasteryPages>>(json));
        }

      
        /// <summary>
        /// Get rune pages for a list summoners' ids synchronously.
        /// </summary>
        /// <param name="region">Region in which you wish to look for mastery pages for a list of summoners.</param>
        /// <param name="summonerIds">A list of summoners' ids for which you wish to retrieve the masteries.</param>
        /// <returns>A dictionary where the keys are the summoners' ids and the values are lists of rune pages.
        /// </returns>
        public Dictionary<long, List<RunePage>> GetRunePages(Region region, List<long> summonerIds)
        {
            var json = requester.CreateRequest(
                string.Format(SummonerRootUrl, region.ToString()) +
                    string.Format(RunesUrl, Util.BuildIdsString(summonerIds)),
                region);
            return ConstructRuneDict(JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json));
        }

     

        /// <summary>
        /// Retrieves the league entries for the specified summoners.
        /// </summary>
        /// <param name="region">Region in which you wish to look for the leagues of summoners.</param>
        /// <param name="summonerIds">The summoner ids.</param>
        /// <returns>A map of list of league entries indexed by the summoner id.</returns>
        public Dictionary<long, List<League>> GetLeagues(Region region, List<long> summonerIds)
        {
            string json = null;
            if (ApiCache.CacheEnabled && (json = ApiCache.GetCache(summonerIds[0], region, "LS")) != null)
                return JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json);


             json = requester.CreateRequest(
                string.Format(LeagueRootUrl, region.ToString()) +
                    string.Format(LeagueBySummonerUrl, Util.BuildIdsString(summonerIds)) + LeagueEntryUrl,
                region);

             if (ApiCache.CacheEnabled)
                 ApiCache.AddCache(summonerIds[0], region.ToString(), json, "LS");

            return JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json);
        }

       
        /// <summary>
        /// Retrieves the entire leagues for the specified summoners.
        /// </summary>
        /// <param name="region">Region in which you wish to look for the leagues of summoners.</param>
        /// <param name="summonerIds">The summoner ids.</param>
        /// <returns>A map of list of leagues indexed by the summoner id.</returns>
        public Dictionary<long, List<League>> GetEntireLeagues(Region region, List<long> summonerIds)
        {
        


           var  json = requester.CreateRequest(
                string.Format(LeagueRootUrl, region.ToString()) +
                    string.Format(LeagueBySummonerUrl, Util.BuildIdsString(summonerIds)),
                region);



            return JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json);
        }

      

        /// <summary>
        /// Retrieves the league entries for the specified teams.
        /// </summary>
        /// <param name="region">Region in which you wish to look for the leagues of teams.</param>
        /// <param name="teamIds">The team ids.</param>
        /// <returns>A map of list of leagues indexed by the team id.</returns>
        public Dictionary<string, List<League>> GetLeagues(Region region, List<string> teamIds)
        {
            var json = requester.CreateRequest(
                string.Format(LeagueRootUrl, region.ToString()) +
                    string.Format(LeagueByTeamUrl, Util.BuildNamesString(teamIds)) + LeagueEntryUrl,
                region);
            return JsonConvert.DeserializeObject<Dictionary<string, List<League>>>(json);
        }


        /// <summary>
        /// Retrieves the entire leagues for the specified teams.
        /// </summary>
        /// <param name="region">Region in which you wish to look for the leagues of teams.</param>
        /// <param name="teamIds">The team ids.</param>
        /// <returns>A map of list of entire leagues indexed by the team id.</returns>
        public Dictionary<string, List<League>> GetEntireLeagues(Region region, List<string> teamIds)
        {
            var json = requester.CreateRequest(
                string.Format(LeagueRootUrl, region.ToString()) +
                    string.Format(LeagueByTeamUrl, Util.BuildNamesString(teamIds)),
                region);
            return JsonConvert.DeserializeObject<Dictionary<string, List<League>>>(json);
        }

       

        /// <summary>
        /// Get the challenger league for a particular queue.
        /// </summary>
        /// <param name="region">Region in which you wish to look for a challenger league.</param>
        /// <param name="queue">Queue in which you wish to look for a challenger league.</param>
        /// <returns>A league which contains all the challengers for this specific region and queue.</returns>
        public League GetChallengerLeague(Region region, Queue queue)
        {
            var json = requester.CreateRequest(
                string.Format(LeagueRootUrl, region.ToString()) + LeagueChallengerUrl,
                region,
                new List<string> { string.Format("type={0}", queue.ToCustomString()) });
            return JsonConvert.DeserializeObject<League>(json);
        }

       

        /// <summary>
        /// Get the teams for the specified ids synchronously.
        /// </summary>
        /// <param name="region">Region in which the teams are located.</param>
        /// <param name="summonerIds">List of summoner ids</param>
        /// <returns>A map of teams indexed by the summoner's id.</returns>
        public Dictionary<long, List<TeamEndpoint.Team>> GetTeams(Region region, List<long> summonerIds)
        {
            var json = requester.CreateRequest(
                string.Format(TeamRootUrl, region.ToString()) +
                    string.Format(TeamBySummonerURL, Util.BuildIdsString(summonerIds)),
                region);
            return JsonConvert.DeserializeObject<Dictionary<long, List<TeamEndpoint.Team>>>(json);
        }

        

        /// <summary>
        /// Get the teams for the specified ids synchronously.
        /// </summary>
        /// <param name="region">Region in which the teams are located.</param>
        /// <param name="teamIds">List of string of the teams' ids.</param>
        /// <returns>A map of teams indexed by their id.</returns>
        public Dictionary<string, TeamEndpoint.Team> GetTeams(Region region, List<string> teamIds)
        {
            var json = requester.CreateRequest(
                string.Format(TeamRootUrl, region.ToString()) + string.Format(IdUrl, Util.BuildNamesString(teamIds)),
                region);
            return JsonConvert.DeserializeObject<Dictionary<string, TeamEndpoint.Team>>(json);
        }

       

        /// <summary>
        /// Get match information about a specific match synchronously.
        /// </summary>
        /// <param name="region">Region in which the match took place.</param>
        /// <param name="matchId">The match ID to be retrieved.</param>
        /// <param name="includeTimeline">Whether or not to include timeline information.</param>
        /// <returns>A match detail object containing information about the match.</returns>
        public MatchDetail GetMatch(Region region, long matchId, bool includeTimeline = false,string cachefile = null,bool cache = false)
        {
            if (cache && cachefile != null && File.Exists(cachefile))
                return JsonConvert.DeserializeObject<MatchDetail>(File.ReadAllText(cachefile));
            else
            {

                var json = requester.CreateRequest(
                    string.Format(MatchRootUrl, region.ToString()) + string.Format(IdUrl, matchId),
                    region,
                    new List<string> { string.Format("includeTimeline={0}", includeTimeline) });

                if (cache)
                    File.WriteAllText(cachefile, json);

                return JsonConvert.DeserializeObject<MatchDetail>(json);
            }
        }

      

        /// <summary>
        /// Get the mach history of a specific summoner synchronously.
        /// </summary>
        /// <param name="region">Region in which the summoner is.</param>
        /// <param name="summonerId">Summoner ID for which you want to retrieve the match history.</param>
        /// <param name="beginIndex">The begin index to use for fetching games.
        /// The range has to be less than or equal to 15.</param>
        /// <param name="endIndex">The end index to use for fetching games.
        /// The range has to be less than or equal to 15.</param>
        /// <param name="championIds">List of champion IDs to use for fetching games.</param>
        /// <param name="rankedQueues">List of ranked queue types to use for fetching games. Non-ranked queue types
        /// will be ignored.</param>
        /// <returns>A list of match summaries object.</returns>
        public List<MatchSummary> GetMatchHistory(Region region, long summonerId,
            int beginIndex = 0, int endIndex = 14,
            List<long> championIds = null, List<Queue> rankedQueues = null)
        {
            var addedArguments = new List<string> {
                    string.Format("beginIndex={0}", beginIndex),
                    string.Format("endIndex={0}", endIndex),
            };
            if (championIds != null)
            {
                addedArguments.Add(string.Format("championIds={0}", Util.BuildIdsString(championIds)));
            }
            if (rankedQueues != null)
            {
                addedArguments.Add(string.Format("rankedQueues={0}", Util.BuildQueuesString(rankedQueues)));
            }

            var json = requester.CreateRequest(
                string.Format(MatchHistoryRootUrl, region.ToString()) + string.Format(IdUrl, summonerId),
                region,
                addedArguments);
            return JsonConvert.DeserializeObject<PlayerHistory>(json).Matches;
        }

  

        /// <summary>
        /// Get player stats by summoner ID synchronously.
        /// </summary>
        /// <param name="region">Region where to retrieve the data.</param>
        /// <param name="summonerId">ID of the summoner for which to retrieve player stats.</param>
        /// <returns>A list of player stats summaries.</returns>
        public List<PlayerStatsSummary> GetStatsSummaries(Region region, long summonerId)
        {
            string json = null;
            if (ApiCache.CacheEnabled && (json = ApiCache.GetCache(summonerId, region, "RS")) != null)
                return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;

            json = requester.CreateRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsSummaryUrl, summonerId),
                region);

            if (ApiCache.CacheEnabled)
                ApiCache.AddCache(summonerId, region.ToString(), json, "RS");


            return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;
        }

       

        /// <summary>
        /// Get player stats by summoner ID synchronously.
        /// </summary>
        /// <param name="region">Region where to retrieve the data.</param>
        /// <param name="summonerId">ID of the summoner for which to retrieve player stats.</param>
        /// <param name="season">If specified, stats for the given season are returned.
        /// Otherwise, stats for the current season are returned.</param>
        /// <returns>A list of player stats summaries.</returns>
        public List<PlayerStatsSummary> GetStatsSummaries(Region region, long summonerId, StatsEndpoint.Season season)
        {
            var json = requester.CreateRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsSummaryUrl, summonerId),
                region,
                new List<string> { string.Format("season={0}", season.ToString().ToUpper()) });
            return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;
        }

     

        /// <summary>
        /// Get ranked stats by summoner ID synchronously.
        /// </summary>
        /// <param name="region">Region where to retrieve the data.</param>
        /// <param name="summonerId">ID of the summoner for which to retrieve ranked stats.</param>
        /// <returns>A list of champion stats.</returns>
        public List<ChampionStats> GetStatsRanked(Region region, long summonerId)
        {
            string json = null;
            if (ApiCache.CacheEnabled && (json = ApiCache.GetCache(summonerId,region,"CS")) != null)
              return JsonConvert.DeserializeObject<RankedStats>(json).ChampionStats;


            json = requester.CreateRequest(
                string.Format(StatsRootUrl, region) + string.Format(StatsRankedUrl, summonerId),
                region);

            if (ApiCache.CacheEnabled)
                ApiCache.AddCache(summonerId, region.ToString(), json, "CS");

            return JsonConvert.DeserializeObject<RankedStats>(json).ChampionStats;
        }

      



        


        /// <summary>
        /// Get the 10 most recent games by summoner ID synchronously.
        /// </summary>
        /// <param name="region">Region where to retrieve the data.</param>
        /// <param name="summonerId">ID of the summoner for which to retrieve recent games.</param>
        /// <returns>A list of the 10 most recent games.</returns>
        public List<Game> GetRecentGames(Region region, long summonerId)
        {
            var json = requester.CreateRequest(
                string.Format(GameRootUrl, region) + string.Format(RecentGamesUrl, summonerId),
                region);
            return JsonConvert.DeserializeObject<RecentGames>(json).Games;
        }

     
        private Dictionary<long, List<MasteryPage>> ConstructMasteryDict(Dictionary<string, MasteryPages> dict)
        {
            var returnDict = new Dictionary<long, List<MasteryPage>>();
            foreach (var masteryPage in dict.Values)
            {
                returnDict.Add(masteryPage.SummonerId, masteryPage.Pages);
            }
            return returnDict;
        }

        private Dictionary<long, List<RunePage>> ConstructRuneDict(Dictionary<string, RunePages> dict)
        {
            var returnDict = new Dictionary<long, List<RunePage>>();
            foreach (var runePage in dict.Values)
            {
                returnDict.Add(runePage.SummonerId, runePage.Pages);
            }
            return returnDict;
        }
    }
}