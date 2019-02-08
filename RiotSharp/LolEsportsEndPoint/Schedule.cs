using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiotSharp.LolEsportsEndPoint
{
    public class Tournament
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("round")]
        public string Round { get; set; }
    }

    public class Blue
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logoURL")]
        public string LogoURL { get; set; }

        [JsonProperty("acronym")]
        public string Acronym { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }
    }

    public class Red
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logoURL")]
        public string LogoURL { get; set; }

        [JsonProperty("acronym")]
        public string Acronym { get; set; }

        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }
    }

    public class CurrentContestants
    {

        [JsonProperty("blue")]
        public Blue Blue { get; set; }

        [JsonProperty("red")]
        public Red Red { get; set; }
    }



    public class LiveStreams
    {
        public string type { get; set; }
        public string URL { get; set; }
        public string embedCode { get; set; }
    }

    public class Game
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("winnerId")]
        public string WinnerId { get; set; }

        [JsonProperty("noVods")]
        public int NoVods { get; set; }

        [JsonProperty("hasVod")]
        public int HasVod { get; set; }
    }

    public class Games
    {
        public List<Game> GameList { get; set; }
    }

    public class Match
    {
        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty("winnerId")]
        public string WinnerId { get; set; }

        [JsonProperty("matchId")]
        public string MatchId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("maxGames")]
        public string MaxGames { get; set; }

        [JsonProperty("isLive")]
        public bool IsLive { get; set; }

        [JsonProperty("isFinished")]
        public string IsFinished { get; set; }

        [JsonProperty("tournament")]
        public Tournament Tournament { get; set; }

        [JsonProperty("contestants")]
        public CurrentContestants Contestants { get; set; }

        [JsonProperty("liveStreams")]
        public object LiveStreams { get; set; }

        [JsonProperty("polldaddyId")]
        public string PolldaddyId { get; set; }

        [JsonProperty("games")]
        public Games Games { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Schedule
    {
        public List<Match> Matches { get; set; }
    }


    public class ScheduleConverter : JsonCreationConverter<Schedule>
    {
        protected override Schedule Create(Type objectType, JObject jObject)
        {
            JsonSerializer s = new JsonSerializer();
            s.Converters.Add(new GamesConverter());
            s.Converters.Add(new LiveStreamsConverter());
            Schedule t = new Schedule();
            List<JToken> L = jObject.Children<JToken>().ToList<JToken>();
            t.Matches = new List<Match>();
            foreach (JProperty l in L)
            {
              
                Match m = l.Value.ToObject<Match>(s);
              m.DateTime =  TimeZoneInfo.ConvertTimeFromUtc(m.DateTime, TimeZoneInfo.Local);
                t.Matches.Add(m);
            }
            return t;
        }


    }
    public class GamesConverter : JsonCreationConverter<Games>
    {
        protected override Games Create(Type objectType, JObject jObject)
        {
            Games t = new Games();
            List<JToken> L = jObject.Children<JToken>().ToList<JToken>();
            t.GameList = new List<Game>();
            foreach (JProperty l in L)
                t.GameList.Add(l.Value.ToObject<Game>());

            return t;
        }


    }
    public class LiveStreamsConverter : JsonCreationConverter<LiveStreams>
    {
        protected override LiveStreams Create(Type objectType, JObject jObject)
        {
            //LiveStreams t = new LiveStreams();
            //if(jObject)
            //List<JToken> L = jObject.Children<JToken>().ToList<JToken>();
            //t.GameList = new List<Game>();
            //foreach (JProperty l in L)
            //    t.GameList.Add(l.Value.ToObject<Game>());

            return null;
        }


    }
}
