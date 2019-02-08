using Newtonsoft.Json;
using RiotSharp.LolEsportsEndPoint;
using RiotSharp.StatusEndpoint;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RiotSharp
{
    /// <summary>
    /// Entry point for the status API.
    /// </summary>
    public class EsportsRiotApi
    {

        private const string RootDomain = "na.lolesports.com";

        private Requester requester;
        private WebClient wbc;
        private static EsportsRiotApi instance;
        /// <summary>
        /// Get the instance of StatusRiotApi.
        /// </summary>
        /// <returns>The instance of StatusRiotApi.</returns>
        public static EsportsRiotApi GetInstance()
        {
            return instance ?? (instance = new EsportsRiotApi());
        }

        private EsportsRiotApi()
        {
            requester = Requester.Instance;
            wbc = new WebClient();
        }

        public Tourneys GetTourneys()
        {
            var json = requester.CreateRequest("/api/tournament.json?published=1", RootDomain);
            return JsonConvert.DeserializeObject<Tourneys>(json, new TourneyConverter());

        }
        public Schedule GetSchedule(string id,bool live = true, bool future = true, bool finished = false)
        {
            var json = requester.CreateRequest(string.Format("/api/schedule.json?tournamentId={0}&includeFinished={1}&includeFuture{2}&includeLive={3}",id, finished, future,live), RootDomain);
            return JsonConvert.DeserializeObject<Schedule>(json, new ScheduleConverter());

        }

        public string DownloadIcon(string url,string file)
        {
            if(!File.Exists(file))
               wbc.DownloadFile(url, file);
            return file;

        }
    }
}
