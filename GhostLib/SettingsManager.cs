using GhostLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib
{
    public static class SettingsManager
    {
        public static GBConfig Settings { get; set; }


        public static void Init( string config)
        {
            try
            {
                string json = File.ReadAllText(config);
                Settings = Newtonsoft.Json.JsonConvert.DeserializeObject<GBConfig>(json);
                if (Settings.ProxyPort == 0)
                {
                    Settings.ProxyPort = 9060;
                    Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to load configuration file "+config,ex);
            }
        }
        public static void Save(string config)
        {
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(Settings);
                File.WriteAllText(config, json);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to save configuration file " + config, ex);
            }
        }
        public static void Save()
        {
            Save(AppDomain.CurrentDomain.BaseDirectory + @"\Config.txt");
        }

        public static void Init()

        {
            Init(AppDomain.CurrentDomain.BaseDirectory + @"\Config.txt");
        }

        public static void Default()
        {
            try
            {

                Settings = new GBConfig();

                Settings.Accounts = new List<GBAccount>();
                GBAccount acc = new GBAccount();
                acc.Region = RiotSharp.Region.euw;
                acc.PlayerID = 0;
                acc.SummonerName = "Summoner";
                acc.PbePlayerID = 0;
                acc.PbeSummonerName = "Summoner";
               


                Settings.Accounts.Add(acc);


                Settings.RecordIfLate = false;
                Settings.GameDirectory = "C:\\Riot Games\\League of Legends";
                Settings.ApiKey = "";           
                Settings.ClientId = "";
                Settings.DragonVersion = "5.0";
                Settings.MainServer = "ghostblade.tk";
                Settings.NetworkInterface = "default";
                Settings.PingEnabled = true;
                Settings.RecordingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GhostbladeReplays";
                Settings.SendLogs = true;
                Settings.Speech = false;
     
                Settings.GameVersion = "5.4.0.241";
  
                Settings.NetPort = 9070;
                Settings.ProxyOption = Network.ProxyType.None;

                Settings.ProxyPass = "";
                Settings.ProxyPort = 8080;
                Settings.ProxyUser = "";
                Settings.ProxyHost = "";


                Settings.ApiCacheEnabled = true;
                Settings.HelperEnabled = true;
                Settings.GhostOverlayEnabled = true;
                Settings.GhostStreamPort = 9069;


                Settings.HasPBE = false;
                Settings.PbeDirectory = "";
                Settings.PbeVersion = "5.4.0.241";

                Settings.Language = Lang.GhostLanguage.Default;
                Settings.AutoGameInfo = false;
                Settings.AutoMoveOld = true;
                Settings.TopBannerBg = "Default.png";
                Settings.AnimatorEnabled = false;
                Settings.AdvancedGameInfo = false;
                Settings.FollowedSummoners = null;
                Settings.PortForwarding = false;
                Settings.AutoRecordGame = true;

                GOverlay g = new GOverlay();
                g.OverlayText = "GHOSTBLADE REPLAYS";
                g.Left = System.Windows.Forms.SystemInformation.VirtualScreen.Width - 240;
                g.Bottom = System.Windows.Forms.SystemInformation.VirtualScreen.Height - 5;


                Settings.Overlays = new GOverlay[1] {g };

                Save();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to set default settings" , ex);
            }
        }
    }
}
