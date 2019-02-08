using RiotSharp;
using RiotSharp.LeagueEndpoint;
using RiotSharp.MatchEndpoint;
using RiotSharp.StatsEndpoint;
using RiotSharp.SummonerEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib
{
   public class LeaguePlayer : IDisposable
    {
    
       public int Spell1 { get; set; }
       public int Spell2 { get; set; }
       public string Champion { get; set; }
       public string Wins { get; set; }
       public string Rank { get; set; }
       public double WinRate { get; set; }
 
       public string RankedWins { get; set; }
       public string RankedLosses { get; set; }

       public string SummonerName { get; set; }
       public long Level { get; set; }
       public long Id { get; set; }
       public LeagueEntry RankedLeague { get; set; }
       public ChampionStats ChampStat { get; set; }
       bool Success = true;
       public void LoadPlayer(Region reg, RiotSharp.RiotApi api, Participant p, string champion)
       {
           try
           {
               SummonerName = p.Name;
               Rank = "Unranked";
               WinRate = 0;
               RankedWins = "0";
               RankedLosses = "0";
               Id = 0;
               Spell1 = p.Spell1Id;
               Spell2 = p.Spell2Id;
               Champion = champion;

               Summoner sum = api.GetSummoner(reg, SummonerName);
               Id = sum.Id;
               Level = sum.Level;
             
List<PlayerStatsSummary>   sp =  api.GetStatsSummaries(reg, Id);
foreach (PlayerStatsSummary ps in sp)

{
    
    if (ps.PlayerStatSummaryType == PlayerStatsSummaryType.RankedSolo5x5)
    {
        RankedWins = ps.Wins.ToString();
        RankedLosses = ps.Losses.ToString();
    
    }
    else if (ps.PlayerStatSummaryType == PlayerStatsSummaryType.Unranked)
   
        Wins = ps.Wins.ToString();
       
   
}

try
{
   
    foreach (ChampionStats ci in api.GetStatsRanked(reg, Id))
    {

        if (ci.ChampionId == p.ChampionId)
        {
            ChampStat = ci;
            WinRate = (double)ci.Stats.TotalSessionsWon / ci.Stats.TotalSessionsPlayed;
        }
    }

}
catch (Exception ex)
{
    Success = false;
    ChampStat = null;
}
 try
 {
     List<long> asum = new List<long>(); asum.Add(p.SummonerId);
     Dictionary<long, List<RiotSharp.LeagueEndpoint.League>> league = api.GetLeagues(reg, asum);
   
     if (league.Count > 0)
     {
         
         foreach (League l in league[p.SummonerId])
         {
             if (l.Queue == RiotSharp.Queue.RankedSolo5x5)
             {
                 // tier
                 Rank = l.Tier.ToString();
                 RankedLeague = l.Entries[0];
                
                 break;
             }
         }

     }
 }
 catch (Exception ex)
 {
     Rank = "Unranked";
     RankedLeague = null;
     Success = false;
 }


           }
           catch (Exception ex)
           {
               Success = false;
               SummonerName = "Error occured";
           }
           if (Success && ApiCache.ForceDodge.Contains(Id))
               ApiCache.ForceDodge.Remove(Id);

           else if(!ApiCache.ForceDodge.Contains(Id) && !Success)
               ApiCache.ForceDodge.Add(Id);

       }


       // Flag: Has Dispose already been called?
       bool disposed = false;


       // Public implementation of Dispose pattern callable by consumers.
       public void Dispose()
       {
           Dispose(true);
           GC.SuppressFinalize(this);
       }

       // Protected implementation of Dispose pattern.
       protected virtual void Dispose(bool disposing)
       {
           if (disposed)
               return;

           if (disposing)
           {
               RankedLeague = null;
               ChampStat = null;
               // Free any other managed objects here.
               //
           }

           // Free any unmanaged objects here.
           //
           disposed = true;
       }
       
    }
}
