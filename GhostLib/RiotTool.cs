using RiotSharp;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace GhostLib
{
    /// <summary>
    /// Riot Games League of Legends Utility
    /// </summary>
    public class RiotTool
    {
        static RiotTool _instance = null;
      public  static ProcessInfo pi;
        public static RiotTool Instance { 
            
            get {
                if (_instance == null)
                    _instance = new RiotTool();

                return _instance;
            } 
        }

        public Logger Log { get; set; }

        public VersionResult CompareVersions(string gamever, string repver)
        {
            if (gamever != repver)
            {
              
                Version rv = new Version(repver);
                Version gv = new Version(gamever);
                if (gv.Major > rv.Major || (gv.Major == rv.Major && rv.Minor < gv.Minor))
                    return VersionResult.OUTDATED;
                else if (rv.Major > gv.Major || (gv.Major == rv.Major && gv.Minor < rv.Minor))
                    return VersionResult.PREDATED;
                else return VersionResult.PATCHED;
                 

            }
            else return VersionResult.OK;
            
        }

        public RiotTool()
        {
            try{
           // _api = api;
              
                ProcessId = 0;
            Log = Logger.Instance;
            pi = new ProcessInfo("League of Legends.exe");
            pi.Started +=
     new ProcessInfo.StartedEventHandler(LolStarted);
            GhostBase.GhostbladeInstance.CheckDir(SettingsManager.Settings.RecordingDirectory);
            GhostBase.GhostbladeInstance.CheckDir(SettingsManager.Settings.RecordingDirectory +@"\Replays");
            GhostBase.GhostbladeInstance.CheckDir(SettingsManager.Settings.RecordingDirectory + @"\OldReplays");
            LoadInstalledLolAsync();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.Log.Fatal("Failed to initialize RiotTool", ex);
            }
        }
        void LoadInstalledLolAsync()
        {
            Thread thdSyncRead = new Thread(new ThreadStart(LoadInstalledLol));
            thdSyncRead.SetApartmentState(ApartmentState.STA);
            thdSyncRead.Start();
        }
        bool FindLol()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "Lol Launcher|*.launcher.admin.exe";
                ofd.Title = "Find League of Legends";
                ofd.InitialDirectory = "C:\\";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        string dir = Path.GetDirectoryName(ofd.FileName);
                        SettingsManager.Settings.GameDirectory = dir;
                        SettingsManager.Save();
                        return true;
                    }

                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        void LoadInstalledLol()
        {
            try
            {
                InstalledGameProfile = new LeagueProfile(SettingsManager.Settings.GameDirectory);
                SettingsManager.Settings.GameVersion = InstalledGameProfile.GameVersion.ToString();
                SettingsManager.Save();
               SetDirectory(SettingsManager.Settings.GameDirectory);
            }
            catch (Exception ex)
            {
                if (Log != null)
                Log.Log.Warn(ex.Message);
                 // CALL LOL FINDER
                if (FindLol())
                    LoadInstalledLol();
                else
                {
                    System.Windows.Forms.MessageBox.Show("Failed to find league of legends install directory, the recorder will now exit to prevent possible failures.", "Lol Finder", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                }
            }

            // PBE PROFILE
            if (SettingsManager.Settings.HasPBE)
            {
                // PBE
                try
                {
                    InstalledPbeProfile = new LeagueProfile(SettingsManager.Settings.PbeDirectory);
                    if (!InstalledPbeProfile.IsPbe)
                    {
                        // NO PBE
                        SettingsManager.Settings.HasPBE = false;
                        SettingsManager.Settings.PbeVersion = "";
                        SettingsManager.Settings.PbeDirectory = "";
                        SettingsManager.Save();
                    }
                    else
                    {
                        SettingsManager.Settings.PbeVersion = InstalledPbeProfile.GameVersion.ToString();
                        SettingsManager.Save();
                    }
                }
                catch(Exception ex)
                {
                    if (Log != null)
                    Log.Log.Warn(ex.Message);
                }
            }
        }
        public static string GetCommandLine(Process process)
        {
            var commandLine = new StringBuilder(process.MainModule.FileName + " ");

            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"] + " ");
                }
            }

            return commandLine.ToString();
        }
        public bool MoveReplay(string file, string ver)
        {
            try
            {
                if (File.Exists(file))
                {
                    if (!Directory.Exists(SettingsManager.Settings.RecordingDirectory + @"\OldReplays\" + ver))
                        Directory.CreateDirectory(SettingsManager.Settings.RecordingDirectory + @"\OldReplays\" + ver);

                    if (File.Exists(SettingsManager.Settings.RecordingDirectory + @"\OldReplays\" + ver + @"\" + Path.GetFileName(file)))
                        File.Delete(SettingsManager.Settings.RecordingDirectory + @"\OldReplays\" + ver + @"\" + Path.GetFileName(file));


                    File.Move(file, SettingsManager.Settings.RecordingDirectory + @"\OldReplays\" + ver + @"\" + Path.GetFileName(file));
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool NeedsToCheckLol(Process p)
        {
            return (ProcessId != p.Id);
        }
       public int ProcessId { get; set; }
       public void LolStarted(object sender, EventArgs e)
        {
            try
            {
                Process proc = Process.GetProcessesByName("League of Legends")[0];

                if (ProcessId == proc.Id)
                    return;

           

             string args = GetCommandLine(proc);
             string svreg = GetRegion(proc.MainModule.FileName);
             string[] specs = CommandLineToArgs(args);

             // Set Game Directory
             if (svreg == "PBE")
             {
                 // PBE Game Directory
                 SettingsManager.Settings.HasPBE = true;
                 SettingsManager.Settings.PbeDirectory = GetGameDirectory(proc.MainModule.FileName);
                 SettingsManager.Settings.PbeVersion = GetGameVersion(proc.MainModule.FileName);
                 SettingsManager.Save();
             }
             else if (GameExecutable.ToLower() != proc.MainModule.FileName.ToLower())
             {
                 // Main Game directory
                 SettingsManager.Settings.GameDirectory = GetGameDirectory(proc.MainModule.FileName);
                 SettingsManager.Save();
                 SetDirectory(GetGameDirectory(proc.MainModule.FileName));

             }



             if (args.Contains("spectator"))
             {
               
                      string[] RemoveExcessInfo = args.Split(new string[1] { "spectator " }, StringSplitOptions.None);

                    if (RemoveExcessInfo.Length != 2)
                    {
                        Log.Log.Info("Corrupt spectator command [RemoveExcessInfo]");
                        return;
                    }

                    string[] Info = RemoveExcessInfo[1].Replace("\"", "").Split(' ');

                    if (Info.Length != 5)
                    {
                         Log.Log.Info("Corrupt spectator command [Info]");
                        return;
                    }



                    long GameId = Convert.ToInt64(Info[2]);
                    string platform = Info[3];
                 string key =  Info[1];
                 string server = Info[0];
                 LoLProcessEventArgs evargs = new LoLProcessEventArgs(GameId, proc, platform, key,server);
                 OnGameClientLaunch(this, evargs);

             
             }
             else
             {
                 string[] data = specs[specs.Length - 1].Split(' ');
                 long summoner = Convert.ToInt64(data[3]);
                 LoLProcessEventArgs evargs = new LoLProcessEventArgs(summoner, proc,svreg , (svreg.ToUpper() == "PBE"));
                 OnGameClientLaunch(this, evargs);
             }

             ProcessId = proc.Id;

            }
            catch(Exception ex)
            {
                Log.Log.Error("Failed to pass game command", ex);
            }
        }

      

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        public string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        public  string ExtractGameName(string args)
        {
            Regex regex = new Regex("spectator (?<ADDR>[A-Za-z0-9\\.]+:*[0-9]*) (?<KEY>.{32}) (?<GID>[0-9]+) (?<PF>[A-Z0-9]+)", RegexOptions.IgnoreCase);
            Match match = regex.Match(args);
            if (match.Success)
                return match.Groups["GID"].Value + "-" + match.Groups["PF"].Value;
            else return null;
        }
        public event LoLProcessEventHandler OnGameClientLaunch;

        string _loldir = null;
        public static Dictionary<string, string> ServerNames
        {
            get
            {
                return new System.Collections.Generic.Dictionary<string, string>
			{

                {"TW","Taiwan"},
{"NA1","North America"},
{"EUW1","Europe West"},
{"EUN1","Europe Nordic & East"},
{"EU","Europe"},
{"KR","Korea"},
{"BR1","Brazil"},
{"TR1","Turkey"},
{"SG","Singapore"},
{"RU","Russia"},
{"LA1","North Latin America"},
{"LA2","South Latin America"},
{"PBE1","Public Beta Environment"},
{"TRRU","Turkey, Russia"},
{"BRLA","Brazil, Latin America"},
{"OC1","Oceanic"},
{"VN","Vietnam"},
{"PH","Philippines"},
{"ID1","Indonesia"},
{"TH","Thailand"}

            };

            }
        }

     
        public bool IsValidPath(ref string path)
        {
                 if (path.EndsWith(@"\"))
                path = path.Substring(0, path.Length - 1);

                 return (Directory.Exists(path));
                 


        }
        public void SetDirectory(string loldir)
        {
            if (IsValidPath(ref loldir))
            {
                if (File.Exists(Path.Combine(loldir, "lol.launcher.exe")))
                    _loldir = loldir;
            }
            else _loldir = null;
        }

        public string GameDirectory
        {
            get
            {
                return _loldir;
            }
        }
        public string GameVersion { get { return GetGameVersion(); } }
        public string DefaultRegion { get { return GetRegion(); } }
        public string AirDirectory
        {
            get
            {
                return GetAirDirectory();
            }
        }
        public string AssetsDirectory
        {
            get
            {
                return GetRiotAssetsPath();
            }
        }
        public string GameExecutable
        {
            get
            {
                return GetGameExe();
            }
        }
       
        string _api = null;
        public string ApiKey
        {
            get { return _api; }
        }
        public LeagueProfile InstalledGameProfile
        {
            get;
            set;
        }
        public LeagueProfile InstalledPbeProfile
        {
            get;
            set;
        }
        public int VersionToInt(string versionStr)
        {
            int num = 0;
            try
            {
                char[] chrArray = new char[] { '.' };
                string[] strArrays = versionStr.Split(chrArray, 4);
                for (int i = 0; i < (int)strArrays.Length && i < 4; i++)
                {
                    num = num + (int.Parse(strArrays[i]) << (8 * (3 - i) & 31));
                }
            }
            catch(Exception ex)
            {
                Log.Log.Error("Failed to convert version", ex);
            }
            return num;
        }
        public string GetGameDirectory(string exe)
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(exe.Replace(@"\deploy\", ""))).Replace(@"\RADS\solutions\lol_game_client_sln", "");
        }
        public string GetLatestGameReleaseDeploy(string dir)
        {
            try
            {
                string Directory = Path.Combine(dir, @"RADS\solutions\lol_game_client_sln\releases");

                DirectoryInfo dInfo = new DirectoryInfo(Directory);
                DirectoryInfo[] subdirs = null;
                try
                {
                    subdirs = dInfo.GetDirectories();
                }
                catch
                {


                }
                string latestVersion = "0.0.1";
                foreach (DirectoryInfo info in subdirs)
                    latestVersion = info.Name;


                Directory = Path.Combine(Directory, latestVersion, "deploy");

                return Directory;

            }
            catch (Exception ex) { Log.Log.Error("Failed to get latest release deploy", ex); }
            return null;
        }
        public string GetLatestGameReleaseDeploy()
        {
            try
            {
                string Directory = Path.Combine(GameDirectory, @"RADS\solutions\lol_game_client_sln\releases");

                DirectoryInfo dInfo = new DirectoryInfo(Directory);
                DirectoryInfo[] subdirs = null;
                try
                {
                    subdirs = dInfo.GetDirectories();
                }
                catch
                {


                }
                string latestVersion = "0.0.1";
                foreach (DirectoryInfo info in subdirs)
                    latestVersion = info.Name;


                Directory = Path.Combine(Directory, latestVersion, "deploy");

                return Directory;

            }
            catch (Exception ex) { Log.Log.Error("Failed to get latest release deploy", ex); }
            return null;
        }
        public string GetGameExe()
        {
            try
            {
                string Directory = Path.Combine(GameDirectory, @"RADS\solutions\lol_game_client_sln\releases");

                DirectoryInfo dInfo = new DirectoryInfo(Directory);
                DirectoryInfo[] subdirs = null;
                try
                {
                    subdirs = dInfo.GetDirectories();
                }
                catch (Exception ex) { Log.Log.Error("Failed to get game executable [1]", ex); }
                string latestVersion = "0.0.1";
                foreach (DirectoryInfo info in subdirs)
                {
                    latestVersion = info.Name;
                }

                Directory = Path.Combine(Directory, latestVersion, "deploy");

                if (!File.Exists(Path.Combine(Directory, "League of Legends.exe")))
                    return null;
                else
                {
                    // Get the file version for the notepad.


                    return Path.Combine(Directory, "League of Legends.exe");
                }

            }
            catch (Exception ex) { Log.Log.Error("Failed to get game executable [2]", ex); }
            return null;
        }
        public string GetGameVersion(string gexe)
        {
            try
            {
           
                if (!File.Exists(gexe))
                    return null;
                else
                {
                    // Get the file version for the notepad.


                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(gexe);
                    return myFileVersionInfo.FileVersion;
                }

            }
            catch (Exception ex) { Log.Log.Error("Failed to get game version", ex); }
            return null;

        }
        public string GetGameVersion()
        {
            try
            {
                string gexe = GetGameExe();
                if (!File.Exists(gexe))
                    return null;
                else
                {
                    // Get the file version for the notepad.


                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(gexe);
                    return myFileVersionInfo.FileVersion;
                }

            }
            catch (Exception ex) { Log.Log.Error("Failed to get game version", ex); }
            return null;

        }
        public string GetRegion(string exepath)
        {
            string str;
            string value = null;
            try
            {
                string exeDir = GetGameDirectory(exepath);

                exeDir = Path.Combine(exeDir, "RADS\\system\\system.cfg");
                if (File.Exists(exeDir))
                {
                    string str1 = File.ReadAllText(exeDir).Replace(" ","");
                    Match match = (new Regex("Region=([A-Z]+)")).Match(str1);
                    if (match.Success)
                    {
                        value = match.Groups[1].Value;
                        return value;
                    }
                }

            }
            catch
            {

                str = Path.Combine(GetAirDirectory(), "lol.properties");
                if (File.Exists(str))
                {
                    return ParseRegion(File.ReadAllText(str));
                }

                return "";
            }
            return value;
        }
        public string GetRegion()
        {
            string str;
            string value = null;
            try
            {
                string exeDir = GameDirectory;

                exeDir = Path.Combine(exeDir, "RADS\\system\\system.cfg");
                if (File.Exists(exeDir))
                {
                    string str1 = File.ReadAllText(exeDir);
                    Match match = (new Regex("Region=([A-Z]+)")).Match(str1);
                    if (match.Success)
                    {
                        value = match.Groups[1].Value;
                        return value;
                    }
                }

            }
            catch
            {

                str = Path.Combine(GetAirDirectory(), "lol.properties");
                if (File.Exists(str))
                {
                    return ParseRegion(File.ReadAllText(str));
                }

                return "";
            }
            return value;
        }
        public string ParseRegion(string contents)
        {
            Match match = (new Regex("regionTag=([a-z0-9]+)")).Match(contents);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else return "";
        }
        public string GetAirDirectory()
        {
            string exeDir = GameDirectory;
            string str = "";
            if (exeDir == null)
            {
                return "";
            }
            if (exeDir.ToLower().Contains("\\rads\\"))
            {
                str = Path.Combine(Directory.GetParent(exeDir).FullName, "air");
            }
            else
            {

                exeDir = Path.Combine(exeDir, "RADS\\projects\\lol_air_client\\releases");
                string[] directories = Directory.GetDirectories(exeDir, "*", SearchOption.TopDirectoryOnly);
                Array.Sort(directories, new FileNameComparer());
                string str1 = directories[(int)directories.Length - 1];
                try
                {
                    DateTime minValue = DateTime.MinValue;
                    for (int i = (int)directories.Length - 1; i >= 0; i--)
                    {
                        FileInfo fileInfo = new FileInfo(Path.Combine(directories[i], "deploy\\preferences\\global\\global.properties"));
                        if (fileInfo.LastWriteTime > minValue)
                        {
                            str1 = directories[i];
                            minValue = fileInfo.LastWriteTime;
                        }
                    }
                }
                catch (Exception ex) { Log.Log.Error("Failed to get air directory", ex); }
                str = Path.Combine(str1, "deploy");
            }
            return str;
        }
        public string GetRiotAssetsPath()
        {
      
            return Path.Combine(AirDirectory , "assets\\");
        }


        public int LaunchGame(bool pbe,string gid, string server, string key, string platform)
        {
            try
            {

                LeagueProfile lg = InstalledGameProfile;
                if (pbe)
                    lg = InstalledPbeProfile;


                Process p = new System.Diagnostics.Process();
                p.StartInfo.WorkingDirectory = lg.ReleaseDeploy;
                p.StartInfo.FileName = lg.LeagueExecutable;
                p.StartInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
                    + server + " "
                    + key + " "
                    + gid + " "
                    + platform + "\"";

                p.Start();
                GhostOverlay.ShowOverlay(lg.ReleaseDeploy);
                return p.Id;
            }
            catch (Exception ex)
            {
                Log.Log.Error("Failed to launch the game", ex);
                return -1;
            }
        }

        public static string RegionToPlatformId(string region)
        {
            region = region.ToUpper();
            if (region == "EU")
                region = "EUW1";
            else if (region == "NA")
                region = "NA1";
            else if (region == "EUW")
                region = "EUW1";
            else if (region == "EUNE")
                region = "EUN1";
            else if (region == "BR")
                region = "BR1";
            else if (region == "OC" || region == "OCE")
                region = "OC1";
            else if (region == "LAN")
                region = "LA1";
            else if (region == "LAS")
                region = "LA2";
            else if (region == "PBE")
                region = "PBE1";
            else if (region == "TR")
                region = "TR1";
            else if (region == "LOL")
                region = "SG";
            return region;
        }
        public static string RegionToString(string region)
        {
            region = region.ToUpper();
            if (region == "EU")
                region = "Europe West";
            else if (region == "NA")
                region = "North America";
            else if (region == "EUW")
                region = "Europe West";
            else if (region == "EUNE")
                region = "Europe Nordic & East";
            else if (region == "BR")
                region = "Brazil";
            else if (region == "OC" || region == "OCE")
                region = "Oceanic";
            else if (region == "LAN")
                region = "North Latin America";
            else if (region == "LAS")
                region = "Sounth Latin America";
            else if (region == "PBE")
                region = "Public Beta Environment";
            else if (region == "TR")
                region = "Turkey";
            else if (region == "LOL")
                region = "SG";


            return region;
        }
        public static string ToQueueString(QueueType q)
        {

            switch (q)
            {
                case QueueType.Aram5x5:
                    return "ARAM 5 vs 5";
                case QueueType.Bot5x5:
                    return "BOT 5 vs 5";
                case QueueType.Bot5x5Beginner:
                    return "BOT 5 vs 5 BEGINNER";
                case QueueType.Bot5x5Intermediate:
                    return "BOT 5 vs 5 INTERMEDIATE";
                case QueueType.Bot5x5Intro:
                    return "BOT 5 vs 5 INTRO";
                case QueueType.BotOdin5x5:
                    return "BOT DOMINIONS 5 vs 5";
                case QueueType.BotTt3x3:
                    return "BOT_TT_3x3";
                case QueueType.BotUrf5x5:
                    return "BOT URF 5 vs 5";
                case QueueType.Custom:
                    return "CUSTOM";
                case QueueType.GroupFinder5x5:
                    return "TEAMBUILDER FINDER 5 vs 5";
                case QueueType.NightmareBot5x5Rank1:
                    return "NIGHTMARE_BOT 5 vs 5 RANK1";
                case QueueType.NightmareBot5x5Rank2:
                    return "NIGHTMARE_BOT 5 vs 5 RANK2";
                case QueueType.NightmareBot5x5Rank5:
                    return "NIGHTMARE_BOT 5 vs 5 RANK5";
                case QueueType.Normal3x3:
                    return "NORMAL 3 vs 3";
                case QueueType.Normal5x5Blind:
                    return "NORMAL 5 vs 5 BLIND";
                case QueueType.Normal5x5Draft:
                    return "NORMAL 5 vs 5 DRAFT";
                case QueueType.Odin5x5Blind:
                    return "DOMINIONS 5 vs 5 BLIND";
                case QueueType.Odin5x5Draft:
                    return "DOMINIONS 5 vs 5 DRAFT";
                case QueueType.OneForAll5x5:
                    return "ONEFORALL 5 vs 5";
                case QueueType.RankedPremade3x3:
                    return "RANKED PREMADE 3 vs 3";
                case QueueType.RankedPremade5x5:
                    return "RANKED PREMADE 5 vs 5";
                case QueueType.RankedSolo5x5:
                    return "RANKED SOLO 5 vs 5";
                case QueueType.RankedTeam3x3:
                    return "RANKED TEAM 3 vs 3";
                case QueueType.RankedTeam5x5:
                    return "RANKED TEAM 5 vs 5";
                case QueueType.Sr6x6:
                    return "SR_6x6";
                case QueueType.Urf5x5:
                    return "URF 5 vs 5";
                default:
                    return string.Empty;

            }

        }
        public static string ToMapString(MapType map)
        {
            switch (map)
            {

                case MapType.HowlingAbyss:
                    return "Howling Abyss";
                case MapType.SummonersRiftAutumnVariant:
                    return "Original Autumn Variant";
                case MapType.SummonersRiftBeta:
                    return "Summoner's Rift";
                case MapType.SummonersRiftSummerVariant:
                    return "Original Summer Variant";
                case MapType.TheCrystalScar:
                    return "The Crystal Scar";
                case MapType.TheProvingGrounds:
                    return "The Proving Grounds";
                case MapType.TwistedTreelineCurrent:
                    return "Twisted Treeline";
                case MapType.TwistedTreelineOriginal:
                    return "Twisted Treeline";
                case MapType.ButcherBridge:
                    return "Butcher's Bridge";
                default:
                    return "";
            }


        }
        #region Ping & Servers
        static Ping pingSender = new Ping();
        static PingOptions options = new PingOptions();
        public static long PingServer(IPAddress ip)
        {
            options.DontFragment = true;
            int timeout = 1000;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            PingReply reply = pingSender.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
                return reply.RoundtripTime;
            else return -1;
        }
        static Dictionary<Region, IPAddress> IPCache = new Dictionary<Region, IPAddress>();
        public static long PingServer(Region reg)
        {
            if (!IPCache.ContainsKey(reg))
                IPCache.Add(reg, Dns.Resolve(PingServers[reg.ToString().ToUpper()]).AddressList[0]);

            return PingServer(IPCache[reg]);

        }

     

        public static Dictionary<string, string> PingServers
        {
            get
            {
                return new System.Collections.Generic.Dictionary<string, string>
			{

			{"EUW","lq.eu.lol.riotgames.com"},
{"NA","lq.na1.lol.riotgames.com"},
{"EUNE","lq.eun1.lol.riotgames.com"},
{"KR","lq.kr.lol.riotgames.com"},
{"BR","lq.br.lol.riotgames.com"},
{"TR","lq.tr.lol.riotgames.com"},
{"RU","lq.ru.lol.riotgames.com"},
{"LAN","lq.la1.lol.riotgames.com"},
{"LAS","lq.la2.lol.riotgames.com"},
{"OCE","lq.oc1.lol.riotgames.com"}
			};
            }
        }
        #endregion
        public static string PlatformToString(string region)
        {
            region = region.ToUpper();
            if (ServerNames.ContainsKey(region))
                return ServerNames[region];
            return region;
        }

        public static Region PlatformToRegion(string plat)
        {
            switch (plat.ToUpper())
            {
                case "BRLA":
                case "BR1":
                    return Region.br;
                case "EUN1":
                    return Region.eune;
                case "EUW1":
                    return Region.euw;

                case "KR":
                    return Region.kr;
                case "LA1":
                    return Region.lan;
                case "LA2":
                    return Region.las;
                case "NA1":
                    return Region.na;
                case "OC1":
                    return Region.oce;
                case "RU":
                case "RU1":
                    return Region.ru;
                case "TRRU":
                case "TR1":
                    return Region.tr;
                case "PBE1":
                    return Region.pbe;
                case "ID1":
                    return Region.Indonesia;
       
             
            }
            return Region.euw;
        }
        public static Dictionary<string, string> Servers
        {
            get
            {
                return new System.Collections.Generic.Dictionary<string, string>
			{

                //{
                //    "TW",
                //    "112.121.84.194:8088"
                //},

                //{
                //    "NA1",
                //    "192.64.174.163:80"
                //},

                //{
                //    "EUW1",
                //    "185.40.64.163:80"
                //},

                //{
                //    "EUN1",
                //    "95.172.65.26:8088"
                //},

				

                //{
                //    "BR1",
                //    "66.151.33.19:80"
                //},

                //{
                //    "LA1",
                //    "66.151.33.19:80"
                //},

                //{
                //    "LA2",
                //    "66.151.33.19:80"
                //},


                //{
                //    "TR1",
                //    "95.172.65.242:80"
                //},

                //{
                //    "RU",
                //    "95.172.65.242:80"
                //},

                //{
                //    "TRRU",
                //    "spectator.tr.lol.riotgames.com:80"
                //},

                //{
                //    "PBE1",
                //    "spectator.pbe1.lol.riotgames.com:8088"
                //},

                //{
                //    "SG",
                //    "203.116.112.222:8088"
                //},

                //{
                //    "KR",
                //    "110.45.191.11:80"
                //},

                //{
                //    "OC1",
                //    "192.64.169.29:8088"
                //}

        	{ "TW", "112.121.84.194:8088" },
				{ "NA1", "spectator.na1.lol.riotgames.com:80" },
				{ "EUW1", "spectator.euw1.lol.riotgames.com:80" },
				{ "EUN1", "spectator.eu.lol.riotgames.com:8088" },
				{ "EU", "spectator.eu.lol.riotgames.com:8088" },
				{ "BR1", "spectator.br.lol.riotgames.com:80" },
				{ "LA1", "spectator.la1.lol.riotgames.com:80" },
				{ "LA2", "spectator.la2.lol.riotgames.com:80" },
				{ "BRLA", "spectator.br.lol.riotgames.com:80" },
				{ "TR1", "spectator.tr.lol.riotgames.com:80" },
				{ "RU", "spectator.ru.lol.riotgames.com:80" },
				{ "TRRU", "spectator.tr.lol.riotgames.com:80" },
				{ "PBE1", "spectator.pbe1.lol.riotgames.com:8088" },
				{ "SG", "203.116.112.222:8088" },
				{ "KR", "spectator.kr.lol.riotgames.com:80" },
				{ "OC1", "spectator.oc1.lol.riotgames.com:80" },
				{ "VN", "210.211.119.15:80" },
				{ "PH", "125.5.6.152:8088" },
				{ "ID1", "103.248.58.26:80" },
				{ "TH", "112.121.157.15:8088" },
				{ "TRSA", "66.151.33.244:8088" },
				{ "TRNA", "216.52.241.149:8088" },
				{ "TRTW", "112.121.84.193:8088" },
				{ "HN1_NEW", "14.17.32.160:8088" }
			};
            }
        }
        public static bool IsPlayerInGame(RiotApi api, out RootObject GameInfo, long player, string plat, Region reg)
        {

            try
            {

                if (reg == Region.pbe)
                    GameInfo = api.GetCurrentGamePBE(Region.pbe, player, "PBE1");
                else
                    GameInfo = api.GetCurrentGame(reg, player, plat);


                if (GameInfo != null)
                    return true;
            }
            catch
            {

            }
            GameInfo = null;
            return false;
        }
    }

    /// <summary>
    /// Simple Version Comparer
    /// </summary>
    public class VersionComparer : IComparer
    {
        public VersionComparer()
        {
        }

        int System.Collections.IComparer.Compare(object a, object b)
        {
            int num;
            try
            {
                FileInfo fileInfo = new FileInfo((string)a);
                FileInfo fileInfo1 = new FileInfo((string)b);
                num = RiotTool.Instance.VersionToInt(fileInfo.Name) - RiotTool.Instance.VersionToInt(fileInfo1.Name);
            }
            catch
            {
                return 0;
            }
            return num;
        }
    }
    /// <summary>
    /// File Data Comparer
    /// </summary>
    public class FileDateComparer : IComparer
    {
        private bool reverse;

        public FileDateComparer()
        {
        }

        public FileDateComparer(bool reverse)
        {
            this.reverse = reverse;
        }

        int System.Collections.IComparer.Compare(object a, object b)
        {
            FileInfo fileInfo = new FileInfo((string)a);
            FileInfo fileInfo1 = new FileInfo((string)b);
            DateTime lastWriteTime = fileInfo.LastWriteTime;
            DateTime dateTime = fileInfo1.LastWriteTime;
            if (this.reverse)
            {
                return DateTime.Compare(lastWriteTime, dateTime);
            }
            return DateTime.Compare(dateTime, lastWriteTime);
        }
    }
    /// <summary>
    /// FileName Comparer
    /// </summary>
    public class FileNameComparer : IComparer
    {
        public FileNameComparer()
        {
        }

        int System.Collections.IComparer.Compare(object a, object b)
        {
            FileInfo fileInfo = new FileInfo((string)a);
            FileInfo fileInfo1 = new FileInfo((string)b);
            return string.Compare(fileInfo.FullName, fileInfo1.FullName);
        }
    }


    public class LoLProcessEventArgs{
        public Process LolProc;
        public long Player = 0;
        public long GameID = 0;
        public string PlatformId = null;

        public string EncryptionKey;
        public string Server;

        public bool IsSpectator;
        public bool IsPBE;
        public LoLProcessEventArgs(long gid, Process proc, string plat, string enc, string server)
        {
            IsSpectator = true;
            GameID = gid;
            PlatformId = plat;
            EncryptionKey = enc;
            Server = server;
            LolProc = proc;
            IsPBE = false;
        }
        public LoLProcessEventArgs(long sid, Process proc,string platform)
        {
            IsSpectator = false;
            Player = sid;
            GameID = 0;
            PlatformId = platform;
            EncryptionKey = null;
            Server = null;
            LolProc = proc;
            IsPBE = false;
        }


        public LoLProcessEventArgs(long gid, Process proc, string plat, string enc, string server, bool pbe)
        {
            IsSpectator = true;
            GameID = gid;
            PlatformId = plat;
            EncryptionKey = enc;
            Server = server;
            LolProc = proc;
            IsPBE = pbe;
        }
        public LoLProcessEventArgs(long sid, Process proc, string platform,bool pbe)
        {
            IsSpectator = false;
            Player = sid;
            GameID = 0;
            PlatformId = platform;
            EncryptionKey = null;
            Server = null;
            LolProc = proc;
            IsPBE = pbe;
        }
    }
    public delegate void LoLProcessEventHandler(object sender, LoLProcessEventArgs e);

    /// <summary>
    /// ProcessInfo class.
    /// </summary>
    public class ProcessInfo
    {
        // defenition of the delegates
        public delegate void StartedEventHandler(object sender, EventArgs e);
        public delegate void TerminatedEventHandler(object sender, EventArgs e);

        // events to subscribe
        public StartedEventHandler Started = null;
        public TerminatedEventHandler Terminated = null;

        // WMI event watcher
        private ManagementEventWatcher watcher;

        // The constructor uses the application name like notepad.exe
        // And it starts the watcher
        public ProcessInfo(string appName)
        {
            // querry every 2 seconds
            string pol = "2";

            string queryString =
                "SELECT *" +
                "  FROM __InstanceOperationEvent " +
                "WITHIN  " + pol +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + appName + "'";

            // You could replace the dot by a machine name to watch to that machine
            string scope = @"\\.\root\CIMV2";

            // create the watcher and start to listen
            watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += new EventArrivedEventHandler(this.OnEventArrived);
            watcher.Start();
        }
        public void Dispose()
        {
            watcher.Stop();
            watcher.Dispose();
        }
        public static DataTable RunningProcesses()
        {
            /* One way of constructing a query
            string className = "Win32_Process";
            string condition = "";
            string[] selectedProperties = new string[] {"Name", "ProcessId", "Caption", "ExecutablePath"};
            SelectQuery query = new SelectQuery(className, condition, selectedProperties);
            */

            // The second way of constructing a query
            string queryString =
                "SELECT Name, ProcessId, Caption, ExecutablePath" +
                "  FROM Win32_Process";

            SelectQuery query = new SelectQuery(queryString);
            ManagementScope scope = new System.Management.ManagementScope(@"\\.\root\CIMV2");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection processes = searcher.Get();

            DataTable result = new DataTable();
            result.Columns.Add("Name", Type.GetType("System.String"));
            result.Columns.Add("ProcessId", Type.GetType("System.Int32"));
            result.Columns.Add("Caption", Type.GetType("System.String"));
            result.Columns.Add("Path", Type.GetType("System.String"));

            foreach (ManagementObject mo in processes)
            {
                DataRow row = result.NewRow();
                row["Name"] = mo["Name"].ToString();
                row["ProcessId"] = Convert.ToInt32(mo["ProcessId"]);
                if (mo["Caption"] != null)
                    row["Caption"] = mo["Caption"].ToString();
                if (mo["ExecutablePath"] != null)
                    row["Path"] = mo["ExecutablePath"].ToString();
                result.Rows.Add(row);
            }
            return result;
        }
        private void OnEventArrived(object sender, System.Management.EventArrivedEventArgs e)
        {
            try
            {
                string eventName = e.NewEvent.ClassPath.ClassName;

                if (eventName.CompareTo("__InstanceCreationEvent") == 0)
                {
                    // Started
                    if (Started != null)
                        Started(this, e);
                }
                else if (eventName.CompareTo("__InstanceDeletionEvent") == 0)
                {
                    // Terminated
                    if (Terminated != null)
                        Terminated(this, e);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

    }

    public enum VersionResult : byte
    {
        OK = 1,
        OUTDATED = 2,
        PREDATED = 3,
        PATCHED = 4
    }
}
