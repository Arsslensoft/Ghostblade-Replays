using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ghostblade.Properties;
using RiotSharp.SummonerEndpoint;
using System.IO;
using GhostLib;
using System.Diagnostics;
using MetroFramework;
using System.Net.NetworkInformation;
using GhostLib.Network;
using GhostBase;
using RiotSharp.LeagueEndpoint;

namespace Ghostblade
{
    public partial class SettingsControl : UserControl
    {
        RiotSharp.RiotApi _api;

        RiotSharp.RiotApi API
        {
            get
            {
                if (frm != null)
                    return GhostbladeInstance.Api;
                else return _api;
            }
            set
            {
                _api = value;
            }
        }
      
        public MainForm frm;
        public SettingsControl()
        {
            InitializeComponent();
        }
        GBAccount FindAccount(int id)
        {
            if (SettingsManager.Settings.Accounts == null || SettingsManager.Settings.Accounts.Count <=id)
                return null;

           
                    return  SettingsManager.Settings.Accounts[id];

       
        }
        public void LoadInterfaces()
        {
            try
            {
                netint.Items.Clear();
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if(nic.OperationalStatus == OperationalStatus.Up)
                       netint.Items.Add(nic.Name);
                }
                netint.Items.Add("default");
            }
            catch
            {

            }
        }
        AccountTier GetTier(Summoner sum, RiotSharp.Region reg)
        {
            try
            {
                List<long> asum = new List<long>(); asum.Add(sum.Id);
                Dictionary<long, List<RiotSharp.LeagueEndpoint.League>> league = API.GetLeagues(reg, asum);

                if (league.Count > 0)
                {

                    foreach (League l in league[sum.Id])
                    {
                        if (l.Queue == RiotSharp.Queue.RankedSolo5x5)
                        {
                            // tier
                            switch (l.Tier.ToString().ToUpper())
                            {
                                case "CHALLENGER":
                                    return AccountTier.Challenger;
                                case "MASTER":
                                    return AccountTier.Master;
                                case "DIAMOND":
                                    return AccountTier.Diamond;
                                case "PLATINUM":
                                    return AccountTier.Plat;
                                case "GOLD":
                                    return AccountTier.Gold;
                                case "SILVER":
                                    return AccountTier.Silver;
                                default:
                                    return AccountTier.UnrankedOrBronze;
                            }
                        
                            break;
                        }
                    }

                }
                return AccountTier.UnrankedOrBronze;
            }
            catch (Exception ex)
            {
                return AccountTier.UnrankedOrBronze;
            }
        }
        public void LoadSettings()
        {
            try
            {
                LoadInterfaces();
                bannerbgbox.Items.Clear();
                accidbox.Items.Clear();
               SetStat();
               accidbox.Items.Add("Add new summoner");
                // Accounts
                if (SettingsManager.Settings.Accounts != null)
                {
                    int i = 0;
                    foreach (GBAccount acc in SettingsManager.Settings.Accounts)
                    {
                        accidbox.Items.Add(i.ToString());
                        i++;
                    }

                    if(SettingsManager.Settings.Accounts.Count > 0)
                    {

                        accidbox.Text = "0";

                        pslb.Visible = (accidbox.Text.Length > 0);
                        rlb.Visible = (accidbox.Text.Length > 0);
                        summoners.Visible = (accidbox.Text.Length > 0);

                        snlb.Visible = (accidbox.Text.Length > 0);
                        serverbox.Visible = (accidbox.Text.Length > 0);
                        pbesumbox.Visible = (accidbox.Text.Length > 0); 
                        summoners.Text = SettingsManager.Settings.Accounts[0].SummonerName;
                        serverbox.Text = SettingsManager.Settings.Accounts[0].Region.ToString().ToUpper();
                

                        if (SettingsManager.Settings.HasPBE && !string.IsNullOrEmpty( SettingsManager.Settings.Accounts[0].PbeSummonerName))
                            pbesumbox.Text = SettingsManager.Settings.Accounts[0].PbeSummonerName;
                        else if(SettingsManager.Settings.HasPBE)
                        pbesumbox.Text = "Not configured";
                    else pbesumbox.Text = "Not detected";
                    }
                }



             
                api.Text = SettingsManager.Settings.ApiKey;
              
                loldir.Text = SettingsManager.Settings.GameDirectory;
                ice.Checked = SettingsManager.Settings.RecordIfLate;
                recdir.Text = SettingsManager.Settings.RecordingDirectory;
      
                netint.Text = SettingsManager.Settings.NetworkInterface;

                proxyopt.Text = SettingsManager.Settings.ProxyOption.ToString();
                proxuser.Text = SettingsManager.Settings.ProxyUser;
                proxpass.Text = SettingsManager.Settings.ProxyPass;
                proxport.Text = SettingsManager.Settings.ProxyPort.ToString();
                proxhost.Text = SettingsManager.Settings.ProxyHost;


                apicache.Checked = SettingsManager.Settings.ApiCacheEnabled;
                govcheck.Checked = SettingsManager.Settings.GhostOverlayEnabled;

                helpercheck.Checked = SettingsManager.Settings.HelperEnabled;
                spcheck.Checked = SettingsManager.Settings.Speech;
                gstreamport.Text = SettingsManager.Settings.GhostStreamPort.ToString();
                autogicheck.Checked = SettingsManager.Settings.AutoGameInfo;
                automovecheck.Checked = SettingsManager.Settings.AutoMoveOld;
                animatorcheck.Checked = SettingsManager.Settings.AnimatorEnabled;
                advginfocheck.Checked = SettingsManager.Settings.AdvancedGameInfo;
                followedsumtxt.Lines = SettingsManager.Settings.FollowedSummoners;
                portforwardcheck.Checked = SettingsManager.Settings.PortForwarding;
                autorecordcheck.Checked = SettingsManager.Settings.AutoRecordGame;


                foreach (string file in Directory.GetFiles(Application.StartupPath + @"\Data\", "*.png", SearchOption.TopDirectoryOnly))
                    bannerbgbox.Items.Add(Path.GetFileName(file));

                bannerbgbox.Text = SettingsManager.Settings.TopBannerBg;
            }
            catch
            {

            }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            try{
                int gport = 9069;


                SettingsManager.Settings.ApiKey = api.Text;
          
                SettingsManager.Settings.GameDirectory = loldir.Text;
                SettingsManager.Settings.RecordIfLate = ice.Checked;
                SettingsManager.Settings.NetworkInterface = netint.Text;


                SettingsManager.Settings.ProxyUser = proxuser.Text;
                SettingsManager.Settings.ProxyPass = proxpass.Text;
                SettingsManager.Settings.ProxyPort = int.Parse(proxport.Text);
                SettingsManager.Settings.ProxyHost = proxhost.Text;

                SettingsManager.Settings.ApiCacheEnabled = apicache.Checked;
                SettingsManager.Settings.GhostOverlayEnabled = govcheck.Checked;
                SettingsManager.Settings.HelperEnabled = helpercheck.Checked;
                SettingsManager.Settings.AutoGameInfo = autogicheck.Checked;
                SettingsManager.Settings.AutoMoveOld =  automovecheck.Checked;
                SettingsManager.Settings.TopBannerBg= bannerbgbox.Text ;
                SettingsManager.Settings.AnimatorEnabled = animatorcheck.Checked;
               SettingsManager.Settings.AdvancedGameInfo=  advginfocheck.Checked ;
               SettingsManager.Settings.FollowedSummoners = followedsumtxt.Lines;
               SettingsManager.Settings.AutoRecordGame = autorecordcheck.Checked;
               SettingsManager.Settings.PortForwarding = portforwardcheck.Checked;


                if (int.TryParse(gstreamport.Text, out gport))
                    SettingsManager.Settings.GhostStreamPort = gport;
                else
                    SettingsManager.Settings.GhostStreamPort = 9069;


                if (proxyopt.Text == "Network") SettingsManager.Settings.ProxyOption = GhostLib.Network.ProxyType.Network;
                else if (proxyopt.Text == "Proxy") SettingsManager.Settings.ProxyOption = GhostLib.Network.ProxyType.Proxy;
                else SettingsManager.Settings.ProxyOption = GhostLib.Network.ProxyType.None;

                //if (SettingsManager.Settings.Speech != spcheck.Checked && spcheck.Checked && !Speech.Initialized)
                //    Speech.Initialize();
                SettingsManager.Settings.Speech = spcheck.Checked;
                SettingsManager.Settings.RecordingDirectory = recdir.Text;
                RiotSharp.Region reg = RiotSharp.Region.euw;

                switch (serverbox.Text.ToUpper())
                {
                    case "EUNE":
                        reg = RiotSharp.Region.eune;
                        break;
                    case "NA":
                        reg = RiotSharp.Region.na;
                        break;
                    case "OCE":
                        reg = RiotSharp.Region.oce;
                        break;
                    case "RU":
                        reg = RiotSharp.Region.ru;
                        break;
                    case "TR":
                        reg = RiotSharp.Region.tr;
                        break;
                    case "KR":
                        reg = RiotSharp.Region.kr;
                        break;
                    case "LAN":
                        reg = RiotSharp.Region.lan;
                        break;
                    case "LAS":
                        reg = RiotSharp.Region.las;
                        break;
                    case "BR":
                        reg = RiotSharp.Region.br;
                        break;
                }
                if (API == null && frm != null)
                    frm.API = RiotSharp.RiotApi.GetInstance(api.Text);
                else if (API == null)
                    API = RiotSharp.RiotApi.GetInstance(api.Text);


                // Accounts
                foreach (object item in accidbox.Items)
                {
                    string sitem = item.ToString();
                    if (sitem.Length > 0 && sitem != "Add new summoner")
                    {
                        bool add = false;
                        GBAccount gbacc = FindAccount(int.Parse(sitem));
                        if (gbacc == null)
                        {
                            gbacc = new GBAccount();
                            add = true;
                        }

                        Summoner sum = API.GetSummoner(gbacc.Region, gbacc.SummonerName,false);
                 
                        gbacc.PlayerID = sum.Id;
                        if (add)
                        {
                            gbacc.PbeSummonerName = "";
                            gbacc.PbePlayerID = 0;
                        }
                        // find player tier
                        gbacc.SummonerTier =GetTier(sum,gbacc.Region);
                    

                        gbacc.SummonerIconId = sum.ProfileIconId;
                      GhostbladeInstance.DataDragonInstance.GetIcon(sum.ProfileIconId);
                        gbacc.SummonerLevel = sum.Level;

                        if (SettingsManager.Settings.Accounts == null)
                            SettingsManager.Settings.Accounts = new List<GBAccount>();

                        if (add)
                            SettingsManager.Settings.Accounts.Insert(0, gbacc);
                        else SettingsManager.Settings.Accounts[int.Parse(sitem)] = gbacc;

                        // make it default
                        if (accidbox.Text == sitem)
                        {
                            SettingsManager.Settings.Accounts.RemoveAt(int.Parse(sitem));
                            SettingsManager.Settings.Accounts.Insert(0, gbacc);
                        }
                    }
                }

                SettingsManager.Save();
                if (frm == null)
                {
                    MessageBox.Show("Settings saved\n You should restart Ghostblade", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(Application.StartupPath + @"\Ghostblade.exe", "-restart");
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    MetroMessageBox.Show(frm, "Settings saved", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frm.SelectedAccount = SettingsManager.Settings.Accounts[0];
                    frm.MainTopBanner.Switch(frm.SelectedAccount);
                    frm.RefillTheFollowerQueue();
                }
                //File.WriteAllText(Application.StartupPath + @"\API.dat", api.Text);
                NetworkManager.Init();
      
            }
            catch(Exception ex)

            {
                //Log.LogEx(ex);
                MessageBox.Show(this, ex.Message + "\n" + ex.StackTrace, "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
   
        private void metroLink1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sign-in using your League of legends account, agree on Riot Terms\n And u'll find your Api Key in home page https://developer.riotgames.com/ \nWhy ? \nApi key needed to perform operations like find summoner, game, current game infos...", "Api Key", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("https://developer.riotgames.com/sign-in");
        }

        private void netintEnabled_CheckedChanged(object sender, EventArgs e)
        {
            netint.Enabled = netintEnabled.Checked;
        }
        void SetStat()
        {
            if (SettingsManager.Settings.ProxyOption == GhostLib.Network.ProxyType.Network)
            {
                netintEnabled.Checked = true;
                proxuser.Enabled = false;
                proxpass.Enabled = false;
                proxhost.Enabled = false;
                proxport.Enabled = false;
            }
            else if (SettingsManager.Settings.ProxyOption == GhostLib.Network.ProxyType.Proxy)
            {
                netintEnabled.Checked = false;
                proxuser.Enabled = true;
                proxpass.Enabled = true;
                proxhost.Enabled = true;
                proxport.Enabled = true;
            }
            else
            {
                netintEnabled.Checked = false;
                proxuser.Enabled = false;
                proxpass.Enabled = false;
                proxhost.Enabled = false;
                proxport.Enabled = false;
            }
        }
        private void proxyopt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (proxyopt.Text == "Network")
            {
                netintEnabled.Checked = true;
                proxuser.Enabled = false;
                proxpass.Enabled = false;
                proxhost.Enabled = false;
                proxport.Enabled = false;
            }
            else if (proxyopt.Text == "Proxy")
            {
                netintEnabled.Checked = false;
                proxuser.Enabled = true;
                proxpass.Enabled = true;
                proxhost.Enabled = true;
                proxport.Enabled = true;
            }
            else
            {
                netintEnabled.Checked = false;
                proxuser.Enabled = false;
                proxpass.Enabled = false;
                proxhost.Enabled = false;
                proxport.Enabled = false;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                RiotSharp.ApiCache.ClearCache();
                RiotSharp.ApiCache.ClearSummoners();
            }
            catch
            {

            }
        }


        private void summoner_SelectedIndexChanged(object sender, EventArgs e)
        {
            GBAccount gbacc = null;
            try

            {
                if (accidbox.Text == "Add new summoner")
                {
                    accidbox.Items.Add((accidbox.Items.Count-1).ToString());
                    accidbox.SelectedIndex = accidbox.Items.Count - 1;
                    gbacc = new GBAccount();
                    gbacc.SummonerName = summoners.Text;
                    gbacc.Region = RiotSharp.Region.euw;
                    gbacc.PbeSummonerName = "";

                    SettingsManager.Settings.Accounts.Add(gbacc);

                    pslb.Visible = (accidbox.Text.Length > 0);
                    rlb.Visible = (accidbox.Text.Length > 0);
                    summoners.Visible = (accidbox.Text.Length > 0);

                    snlb.Visible = (accidbox.Text.Length > 0);
                    serverbox.Visible = (accidbox.Text.Length > 0);
                    pbesumbox.Visible = (accidbox.Text.Length > 0); 
                    return;
                }
                gbacc = FindAccount(int.Parse(accidbox.Text));
                if (gbacc != null)
                {
                    summoners.Text = gbacc.SummonerName;
                    serverbox.Text = gbacc.Region.ToString().ToUpper();
                    if (SettingsManager.Settings.HasPBE && !string.IsNullOrEmpty(gbacc.PbeSummonerName))
                        pbesumbox.Text = gbacc.PbeSummonerName;
                    else  if (SettingsManager.Settings.HasPBE)
                        pbesumbox.Text = "Not configured";
                    else pbesumbox.Text = "Not detected";

                    pslb.Visible = (accidbox.Text.Length > 0);
                    rlb.Visible = (accidbox.Text.Length > 0);
                    summoners.Visible = (accidbox.Text.Length > 0);

                    snlb.Visible = (accidbox.Text.Length > 0);
                    serverbox.Visible = (accidbox.Text.Length > 0);
                    pbesumbox.Visible = (accidbox.Text.Length > 0); 
                }

            }
            catch
            {

            }
        }

        private void summoners_TextChanged(object sender, EventArgs e)
        {try{
             GBAccount   gbacc = FindAccount(int.Parse(accidbox.Text));
                if (gbacc != null)
                {
                    gbacc.SummonerName = summoners.Text;
             
                    SettingsManager.Settings.Accounts[int.Parse(accidbox.Text)] = gbacc;
                }

            }
            catch
            {

            }
        }

        private void serverbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GBAccount gbacc = FindAccount(int.Parse(accidbox.Text));
                if (gbacc != null)
                {
                    RiotSharp.Region reg = RiotSharp.Region.euw;

                    switch (serverbox.Text.ToUpper())
                    {
                        case "EUNE":
                            reg = RiotSharp.Region.eune;
                            break;
                        case "NA":
                            reg = RiotSharp.Region.na;
                            break;
                        case "OCE":
                            reg = RiotSharp.Region.oce;
                            break;
                        case "RU":
                            reg = RiotSharp.Region.ru;
                            break;
                        case "TR":
                            reg = RiotSharp.Region.tr;
                            break;
                        case "KR":
                            reg = RiotSharp.Region.kr;
                            break;
                        case "LAN":
                            reg = RiotSharp.Region.lan;
                            break;
                        case "LAS":
                            reg = RiotSharp.Region.las;
                            break;
                        case "BR":
                            reg = RiotSharp.Region.br;
                            break;
                    }
                    gbacc.Region = reg;

                    SettingsManager.Settings.Accounts[int.Parse(accidbox.Text)] = gbacc;
                }

            }
            catch
            {

            }
        }

    
    }
}
