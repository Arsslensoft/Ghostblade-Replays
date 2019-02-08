using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GhostLib
{
   public class LeagueProfile
    {
       public string GameDirectory { get; set; }
       public string Launcher { get; set; }
       public Version GameVersion { get; set; }
       public string LeagueExecutable { get; set; }
       public string ReleaseDeploy { get; set; }

       public string Region { get; set; }

       public bool IsPbe { get; set; }

       void ThrowFileIfNotExist(string dir)
       {
           if (!File.Exists(dir))
               throw new IOException(dir + " does not exist");
       }
       void ThrowDirIfNotExist(string dir)
       {
           if (!Directory.Exists(dir))
               throw new IOException(dir + " does not exist");
       }

       public LeagueProfile(string gdir)
       {
           GameDirectory = gdir;
           ThrowDirIfNotExist(GameDirectory);
        
           ReleaseDeploy = RiotTool.Instance.GetLatestGameReleaseDeploy(GameDirectory);
           ThrowDirIfNotExist(ReleaseDeploy);

           LeagueExecutable = Path.Combine(ReleaseDeploy, "League of Legends.exe");
           ThrowFileIfNotExist(LeagueExecutable);

           Launcher = Path.Combine(GameDirectory, "lol.launcher.admin.exe");
           ThrowFileIfNotExist(Launcher);

           GameVersion = new Version( RiotTool.Instance.GetGameVersion(LeagueExecutable));

           Region = RiotTool.Instance.GetRegion(LeagueExecutable);

           IsPbe = (Region == "PBE");
       }

    }
}
