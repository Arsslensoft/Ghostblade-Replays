using GBReplay.Replays;
using GhostBase;
using GhostLib;
using MetroFramework;
using RiotSharp.MatchEndpoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostblade
{
    public partial class MainForm : XCoolForm.XCoolForm
    {
        internal RiotSharp.StatusRiotApi SAPI;
        internal RiotSharp.RiotApi API
        {
            get { return GhostbladeInstance.Api; }
                set { GhostbladeInstance.Api = value; }
        }
        public GBAccount SelectedAccount { get; set; }


        private readonly SynchronizationContext syncContext;
        Dictionary<string, ReplayPanel> RepCache = new Dictionary<string, ReplayPanel>();

        List<string> Recorded = new List<string>();
        List<string> Recording = new List<string>();


        void LoadStyle()
        {
            this.TitleBar.TitleBarCaption = "Ghostblade Replays";
            this.TitleBar.TitleBarType = XCoolForm.XTitleBar.XTitleBarType.Rounded;
            this.TitleBar.TitleBarFill = XCoolForm.XTitleBar.XTitleBarFill.LinearRendering;
      

            this.MenuIcon = Ghostblade.Properties.Resources.ghost_white.GetThumbnailImage(24, 24, null, IntPtr.Zero);
            //  this.TitleBar.TitleBarFill = XCoolForm.XTitleBar.XTitleBarFill.AdvancedRendering;
            TitleBar.InnerTitleBarColor = Color.FromArgb(255, 17, 17, 17);
            //   TitleBar.OuterTitleBarColor = Color.FromArgb(255, 240, 230, 235);
            TitleBar.TitleBarMixColors.Add(Color.FromArgb(255, 17, 17, 17));

            this.TitleBar.TitleBarButtons[0].ButtonStyle = XCoolForm.XTitleBarButton.XTitleBarButtonStyle.Pixeled;
            this.TitleBar.TitleBarButtons[0].ButtonFillMode = XCoolForm.XTitleBarButton.XButtonFillMode.UpperGlow;


            this.TitleBar.TitleBarButtons[1].ButtonStyle = XCoolForm.XTitleBarButton.XTitleBarButtonStyle.Pixeled;
            this.TitleBar.TitleBarButtons[1].ButtonFillMode = XCoolForm.XTitleBarButton.XButtonFillMode.UpperGlow;

            this.TitleBar.TitleBarButtons[2].ButtonStyle = XCoolForm.XTitleBarButton.XTitleBarButtonStyle.Pixeled;
            this.TitleBar.TitleBarButtons[2].ButtonFillMode = XCoolForm.XTitleBarButton.XButtonFillMode.UpperGlow;



        }


        public MainForm()
        {
         
            if (string.IsNullOrEmpty(SettingsManager.Settings.ApiKey))
            {
                MessageBox.Show("Please set your Riot Games api key in settings", "Riot Games Api", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();

            }
            else API = RiotSharp.RiotApi.GetInstance(SettingsManager.Settings.ApiKey);
            if (SettingsManager.Settings.Accounts.Count == 0)
            {

                MessageBox.Show("You must define an account", "Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SettingsManager.Default();

                SettingsForm frm = new SettingsForm();
                frm.ShowDialog();
            }
            SelectedAccount = SettingsManager.Settings.Accounts[0];
            

            InitializeComponent();

            settingsControl1.frm = this;

          //  RiotTool.Instance.SetDirectory(SettingsManager.Settings.GameDirectory);
        
            LoadStyle();
    

            MainTopBanner.Switch(SelectedAccount);
            currentGame1.frm = animator1;

            ApplicationInstance.Instance.AssociateExtension();
            svstatbx.Title = SelectedAccount.Region.ToString().ToUpper();
      
            if (!File.Exists(Application.StartupPath + @"\CS.pfx"))
                ReplaySignature.GenerateCertificate(Application.StartupPath + @"\CS.pfx", SelectedAccount.SummonerName, RiotTool.RegionToString(SelectedAccount.Region.ToString()));


            // GAME VERSION IS NOW UPDATED BY THE LOADINSTALLEDLOL IN RIOTTOOL
            //// Game Version
            //string v = SettingsManager.Settings.GameVersion;
            //if ((SettingsManager.Settings.GameVersion = RiotTool.Instance.GetGameVersion()) != null)
            //    SettingsManager.Save();
            //else SettingsManager.Settings.GameVersion = v;

            //// PBE Version
            //v = SettingsManager.Settings.PbeVersion;
            //if (SettingsManager.Settings.HasPBE && (SettingsManager.Settings.PbeVersion = RiotTool.Instance.GetGameVersion(RiotTool.Instance.GetLatestGameReleaseDeploy(SettingsManager.Settings.PbeDirectory) + @"\League of Legends.exe")) != null)
            //    SettingsManager.Save();
            //else SettingsManager.Settings.PbeVersion = v;

            this.syncContext = SynchronizationContext.Current;
            if (!Directory.Exists(ReplayTask.ReplayDir))
            {
                Directory.CreateDirectory(ReplayTask.ReplayDir);
                Directory.CreateDirectory(ReplayTask.ReplayDir + @"\Replays");

            }

            LoadWatcher();

            RiotTool.Instance.OnGameClientLaunch += Instance_OnGameClientLaunch;

        }

        #region Resize
        void StreamResize()
        {
            StreamsHostPanel.Width = streambx.Width - 6;
            StreamsHostPanel.Height = streambx.Height - 48;
            StreamsHostPanel.Location = new Point(3, 45);
        }
        void ReplayResize()
        {

            ReplayPanelHost.Width = replaybx.Width - 6;
            ReplayPanelHost.Height = replaybx.Height - 46;
            ReplayPanelHost.Location = new Point(3, 43);

        }
        void HomeResize()
        {
            recordingbx.Height =HomeTab.Height - 271;
            svstatbx.Width = recordingbx.Width - 471;
            shardInfoControl1.Width = svstatbx.Width - 8;
       
            RecordingPanelHost.Width = recordingbx.Width - 6;
            RecordingPanelHost.Height = recordingbx.Height - 46;
            RecordingPanelHost.Location = new Point(3, 43);


        }
        void CurrentGameResize()
        {

            GameInfoPanelHost.Width = gameinfobx.Width - 6;
            GameInfoPanelHost.Height = gameinfobx.Height - 28;
            GameInfoPanelHost.Location = new Point(3, 31);
        }
        void ChampionsResize()
        {

            championCtrl1.Width = championsbx.Width - 6;
            championCtrl1.Height = championsbx.Height - 43;
            championCtrl1.Location = new Point(3, 46);
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.MainPanel.Size = new Size(this.MainPanel.Size.Width, this.Height - 44);
            HomeResize(); 
            ReplayResize();
            CurrentGameResize();
            ChampionsResize();
            StreamResize();
        }
        private void nsTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (MainTabControl.SelectedTab == SettingsTab)
                    settingsControl1.LoadSettings();

                MainForm_Resize(this, EventArgs.Empty);

            }
            catch
            {

            }
        }

        #endregion
        
        #region Featured Games
        private void featuredtimer_Tick(object sender, EventArgs e)
        {
            if (!isingame)
            {
                featuredtimer.Interval = 300000;
                UpdateInvoker asyncrep = new UpdateInvoker(LoadFeatured);
                asyncrep.BeginInvoke(null, null);
            }
            else featuredtimer.Interval = 10000;

        }

        List<FeaturedGameControl> FeaturedGames = new List<FeaturedGameControl>();
        public FeaturedGameControl SelectedFeaturedGame { get; set; }
        void LoadFeatured()
        {
            try
            {
                // Remove all featured games
                FeaturedGames.Clear();
                if (!System.Windows.Forms.SystemInformation.Network && !ginfo.Visible)
                {
                    CurrentFeaturedGamePanel.BeginInvoke(new MethodInvoker(delegate
                {
                    ginfo.Text = "No network connection";
                    ginfo.Visible = false;
                    animator1.AnimationType = AnimatorNS.AnimationType.VertSlide;
                    CurrentFeaturedGamePanel.Controls.Add(ginfo);
                    animator1.Show(ginfo, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                
                }));


                }


                SelectedFeaturedGame = null;

                // Find Games
                string server = SelectedAccount.Region.ToString().ToUpper();


                string plat = RiotTool.RegionToPlatformId(server);


                if (RiotTool.Servers.ContainsKey(plat))
                {

                    RiotSharp.Featured.FeaturedGames fg = GhostBase.GhostbladeInstance.Api.GetFeaturedGames(RiotTool.Servers[plat]);
                    // Remove all Replays from control
               CurrentFeaturedGamePanel.BeginInvoke(new MethodInvoker(delegate
                    {
                        metroComboBox1.Text = server;
                        CurrentFeaturedGamePanel.Controls.Clear();
                    
                    featuredgamespaginator.NumberOfPages = fg.gameList.Count;
                    specfeaturedbtn.Visible = (fg.gameList.Count != 0);
                    recfeaturedbtn.Visible = (fg.gameList.Count != 0);
              
                    if (fg.gameList.Count == 0)
                    {
             
                        ginfo.Text = "Unable to get featured games from this server";
                        ginfo.Visible = false;
                        animator1.AnimationType = AnimatorNS.AnimationType.VertSlide;
                        CurrentFeaturedGamePanel.Controls.Add(ginfo);
                        animator1.Show(ginfo, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                    
                    }
                    else
                    {

                        foreach (RiotSharp.Featured.GameList gl in fg.gameList)
                        {
                            FeaturedGameControl ctrl = new FeaturedGameControl();
                            ctrl.LoadFeatured(gl);
                            ctrl.Dock = DockStyle.Fill;
                            //ctrl.BorderStyle = BorderStyle.Fixed3D;
               
                            FeaturedGames.Add(ctrl);

                  

                        }
                            featuredgamespaginator.SelectedIndex = 0;
                       SelectGame(0);
                         
                    }

                    }));

                }
                 
                    
                
            }
            catch
            {
            
            }
            finally
            {
                //    metroTabPage3.Controls.Add(panel3);
            }
        }
        Random rd = new Random();
        void SelectGame(int id)
        {
        
            if (FeaturedGames.Count == 0 || id >= FeaturedGames.Count)
                return;
            
            if (SelectedFeaturedGame != null)
                SelectedFeaturedGame.IsSelected = false;


            SelectedFeaturedGame = FeaturedGames[id];

            specfeaturedbtn.Visible = (SelectedFeaturedGame != null);
            recfeaturedbtn.Visible = (SelectedFeaturedGame != null);

            SelectedFeaturedGame.IsSelected = true;
            CurrentFeaturedGamePanel.Controls.Clear();

            SelectedFeaturedGame.Visible = false;
            animator1.AnimationType = AnimatorNS.AnimationType.ScaleAndHorizSlide;
            CurrentFeaturedGamePanel.Controls.Add(SelectedFeaturedGame);
            animator1.Show(SelectedFeaturedGame, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);

          
        }
        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               // Remove all featured games
                FeaturedGames.Clear();
                CurrentFeaturedGamePanel.Controls.Clear();
                SelectedFeaturedGame = null;

                // Find Games
                string server = metroComboBox1.Text;


                string plat = RiotTool.RegionToPlatformId(server);


                if (RiotTool.Servers.ContainsKey(plat))
                {

                    RiotSharp.Featured.FeaturedGames fg = GhostBase.GhostbladeInstance.Api.GetFeaturedGames(RiotTool.Servers[plat]);
                    featuredgamespaginator.NumberOfPages = fg.gameList.Count;
                    specfeaturedbtn.Visible = (fg.gameList.Count != 0);
                    recfeaturedbtn.Visible = (fg.gameList.Count != 0);
                    if (fg.gameList.Count == 0)
                    {
                  
                        ginfo.Text = "Unable to get featured games from this server";
                        ginfo.Visible = false;
                        animator1.AnimationType = AnimatorNS.AnimationType.VertSlide;
                        CurrentFeaturedGamePanel.Controls.Add(ginfo);
                        animator1.Show(ginfo, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                    }
                    else
                    {

                        foreach (RiotSharp.Featured.GameList gl in fg.gameList)
                        {
                            FeaturedGameControl ctrl = new FeaturedGameControl();
                            ctrl.LoadFeatured(gl);
                            ctrl.Dock = DockStyle.Fill;
                            //ctrl.BorderStyle = BorderStyle.Fixed3D;
                            FeaturedGames.Add(ctrl);

                           

                        }
                        SelectGame(0);
                    }
                }


            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message + " \n" + ex.StackTrace);

                ginfo.Text = ex.Message;
                ginfo.Visible = false;
                animator1.AnimationType = AnimatorNS.AnimationType.VertSlide;
                CurrentFeaturedGamePanel.Controls.Add(ginfo);
                animator1.Show(ginfo, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
            }
            finally
            {
            //    metroTabPage3.Controls.Add(panel3);
            }
        }

     

        private void featuredgamespaginator_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGame(featuredgamespaginator.SelectedIndex);
        }
        private void recfeaturedbtn_Click(object sender, EventArgs e)
        {

            if (SelectedFeaturedGame != null)
            {
                if (RiotTool.Servers.ContainsKey(SelectedFeaturedGame.CurrentGame.platformId.ToUpper()))
                    this.SimulateRecordFeatured(SelectedFeaturedGame.CurrentGame, RiotTool.Servers[SelectedFeaturedGame.CurrentGame.platformId.ToUpper()]);
                else
                    MetroMessageBox.Show(this, "Could not record this game", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MetroMessageBox.Show(this, "Could not record this game", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void specfeaturedbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedFeaturedGame == null)
                    return;
                if (Process.GetProcessesByName("League of Legends").Length != 0)
                {
                    MetroMessageBox.Show(this, "Could not open League of Legends.\nAnother instance is running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                RiotTool.Instance.LaunchGame(SelectedFeaturedGame.CurrentGame.platformId == "PBE1", SelectedFeaturedGame.CurrentGame.gameId.ToString(), RiotTool.Servers[SelectedFeaturedGame.CurrentGame.platformId.ToUpper()], SelectedFeaturedGame.CurrentGame.observers.encryptionKey, SelectedFeaturedGame.CurrentGame.platformId);
                //string[] details = new string[2] { SelectedFeaturedGame.CurrentGame.gameId.ToString(), SelectedFeaturedGame.CurrentGame.platformId };
                //Process p = new System.Diagnostics.Process();
                //p.StartInfo.WorkingDirectory = RiotTool.Instance.GetLatestGameReleaseDeploy();
                //p.StartInfo.FileName = RiotTool.Instance.GameExecutable;
                //p.StartInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"spectator "
                //    + RiotTool.Servers[SelectedFeaturedGame.CurrentGame.platformId.ToUpper()] + " "
                //    + SelectedFeaturedGame.CurrentGame.observers.encryptionKey + " "
                //    + details[0] + " "
                //    + details[1] + "\"";

                //p.Start();
                //GhostOverlay.ShowOverlay(p.StartInfo.WorkingDirectory);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Warn("Failed to spectate record", ex);
            }
        }
        #endregion

        #region Game Instance
        bool isingame = false;
        Process GameProc = null;
        public bool IsInGame
        {
            get
            {
                if (!isingame)
                    return false;
                else if (GameProc != null)
                    return GameProc.HasExited;
                else return (Process.GetProcessesByName("League of Legends").Length > 0);



            }
        }

        void Instance_OnGameClientLaunch(object sender, LoLProcessEventArgs e)
        {
            if (!SettingsManager.Settings.AutoRecordGame)
                return;

            try
            {

                isingame = !e.IsSpectator;
                if (isingame && streams.Count > 0)
                {
                    foreach (StreamControl str in streams)
                        str.CancelStream();

                    ShowInfo("Streaming disabled", "All streams has been automatically canceled. Reason : Game was detected");
                }
                GameProc = null;

                // PBE
                if (e.IsPBE)
                {
                    if (!e.IsSpectator)
                    {
                        RootObject rg = null;


                        long sid = e.Player;

                        if (sid != -1)
                        {
                            //  RiotSharp.Region lolregsharp = RiotTool.PlatformToRegion(RiotTool.RegionToPlatformId(lolreg));
                            rg = API.GetCurrentGamePBE(RiotSharp.Region.pbe, sid, "PBE1".ToUpper());


                            this.BeginInvoke(new UpdateCurrentGameInvoker(this.SimulateRecord), rg, this.API, RiotSharp.Region.pbe, sid);
                            //UpdateCurrentGameInvoker u = new UpdateCurrentGameInvoker(this.currentGame1.LoadGame);
                            //u.BeginInvoke(rg, API, RiotSharp.Region.pbe, 0, null, null);
                        }

                        //  this.BeginInvoke(new UpdateCurrentGameInvoker(currentGame1.LoadGame), rg, API, RiotSharp.Region.euw);
                        GameProc = e.LolProc;
                        //if (SettingsManager.Settings.Speech)
                        //    Speech.SpeakAsync("Recording you");
                    }
                    else
                        this.BeginInvoke(new UpdateReplayRecording(this.SimulateRecord), e.GameID, e.Server, e.EncryptionKey, e.PlatformId);

                }
                else
                {
                    if (!e.IsSpectator)
                    {
                        RootObject rg = null;
                        string lolreg = RiotTool.Instance.DefaultRegion;

                        long sid = e.Player;

                        if (sid != -1)
                        {
                            RiotSharp.Region lolregsharp = RiotTool.PlatformToRegion(RiotTool.RegionToPlatformId(lolreg));
                            rg = API.GetCurrentGame(lolregsharp, sid, RiotTool.RegionToPlatformId(lolreg).ToUpper());
                            this.BeginInvoke(new UpdateCurrentGameInvoker(this.SimulateRecord), rg, this.API, lolregsharp, sid);
                            if (SettingsManager.Settings.AutoGameInfo)
                            {
                                UpdateCurrentGameInvoker u = new UpdateCurrentGameInvoker(this.currentGame1.LoadGame);
                                u.BeginInvoke(rg, API, lolregsharp, 0, null, null);
                            }
                        }
                        else
                        {
                            rg = API.GetCurrentGame(SelectedAccount.Region, SelectedAccount.PlayerID, RiotTool.RegionToPlatformId(SelectedAccount.Region.ToString().ToUpper()));

                            this.BeginInvoke(new UpdateCurrentGameInvoker(this.SimulateRecord), rg, this.API, SelectedAccount.Region, SelectedAccount.PlayerID);
                            if (SettingsManager.Settings.AutoGameInfo)
                            {
                                UpdateCurrentGameInvoker u = new UpdateCurrentGameInvoker(currentGame1.LoadGame);
                                u.BeginInvoke(rg, API, SelectedAccount.Region, 0, null, null);
                            }
                        }
                        //  this.BeginInvoke(new UpdateCurrentGameInvoker(currentGame1.LoadGame), rg, API, RiotSharp.Region.euw);
                        GameProc = e.LolProc;
                        //if (SettingsManager.Settings.Speech)
                        //    Speech.SpeakAsync("Recording you");
                    }
                    else
                    {

                        this.BeginInvoke(new UpdateReplayRecording(this.SimulateRecord), e.GameID, e.Server, e.EncryptionKey, e.PlatformId);
                        GameProc = e.LolProc;
                        //if (SettingsManager.Settings.Speech)
                        //    Speech.SpeakAsync("Recording spectator");
                    }
                }

            }
            catch (RiotSharp.RiotSharpException rex)
            {
                if (!rex.Message.Contains("404"))
                {
                    if (sender != this)
                        Instance_OnGameClientLaunch(this, e);

                    notifyIcon1.ShowBalloonTip(3000, "Failed to record", rex.Message, ToolTipIcon.Error);
                    Logger.Instance.Log.Error("Failed to trigger", rex);
                }
                else notifyIcon1.ShowBalloonTip(3000, "Game not found", "May be you are trying to record a co-op vs AI game. Only normal, aram, featured, ranked and custom games can be recorded", ToolTipIcon.Warning);

            }
            catch (Exception ex)
            {
                if (sender != this)
                    Instance_OnGameClientLaunch(this, e);
                Logger.Instance.Log.Error("Failed to trigger", ex);
                notifyIcon1.ShowBalloonTip(3000, "Failed to record", ex.Message, ToolTipIcon.Error);
            }

        }

        #endregion

        #region Replays

        void LoadWatcher()
        {
            try
            {

                FileSystemWatcher fsw = new FileSystemWatcher(ReplayTask.ReplayDir + @"\Replays");
                fsw.Filter = "*.lgr";
                fsw.Created += fsw_Deleted;
                fsw.Deleted += fsw_Deleted;
                fsw.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to init watcher", ex);
            }
        }
        void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                this.BeginInvoke(new UpdateInvoker(LoadReplays));

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to trigger watcher", ex);
            }
        }
        void ClearPanel(Control panel)
        {
            List<Control> l = new List<Control>();
            foreach (Control c in panel.Controls)
            {
                if ((c is MetroFramework.Controls.MetroScrollBar))
                    l.Add(c);
            }
            panel.Controls.Clear();
            foreach (Control c in l)
            {
             
                panel.Controls.Add(c);
            }


        }
        internal bool CheckMove(GhostReplay rep)
        {
            try
            {
                if (SettingsManager.Settings.HasPBE && (rep.Platform == "PBE1"))
                    return false;
                else
                {
                    VersionResult vr = RiotTool.Instance.CompareVersions(SettingsManager.Settings.GameVersion, rep.GameVersion);
                
                        if (vr == VersionResult.OUTDATED)
                            return RiotTool.Instance.MoveReplay(rep.FileName, rep.GameVersion);
                    
                }
            }
            catch
            {

            }
            return false;
        }
        void Sort(List<ReplayPanel> list, int d, int f, byte lvl)
        {
            if (d < f && lvl <= 3)
            {
                int right = d;
                if (lvl == 3)
                {
                    do
                    {
                        right = 0;
                        for (int i = d; i < f - 1; i++)
                        {
                            if (list[i].rep.RecordDate < list[i + 1].rep.RecordDate)
                            {
                                ReplayPanel aux = list[i];
                                list[i] = list[i + 1];
                                list[i + 1] = aux;
                                right = 1;
                            }
                        }

                    }
                    while (right != 0);

                }
                else
                {

                    for (int i = d; i < f; i++)
                    {
                        // Compare Win
                        if (lvl == 1)
                        {
                            if (list[i].GameWon)
                            {
                                list.Insert(d, list[i]);
                                list.RemoveAt(i + 1);
                                right++;
                            }

                        }
                        else
                        {
                            if (list[i].rep.PlayerInfos.SummonerName == SelectedAccount.SummonerName)
                            {
                                list.Insert(d, list[i]);
                                list.RemoveAt(i + 1);
                                right++;
                            }
                        }



                    }
                }
                Sort(list, d, right, (byte)(lvl + 1));
                Sort(list, right, f, (byte)(lvl + 1));


            }
        }
        internal void LoadReplays()
        {
      
      
            try
            {
                int wins = 0;
                List<ReplayPanel> SortedReplays = new List<ReplayPanel>();
                ReplayPanelHost.AutoScroll = true;

                // Remove all Replays from control
                ReplayPanelHost.BeginInvoke(new MethodInvoker(delegate
                {
                    Recorded.Clear();
                    ClearPanel(ReplayPanelHost);

                }));

                Parallel.ForEach(Directory.GetFiles(ReplayTask.ReplayDir + @"\Replays", "*.lgr", SearchOption.TopDirectoryOnly), file =>
                {

                    string name = Path.GetFileNameWithoutExtension(file);
                    if (RepCache.ContainsKey(name))
                    {
                        ReplayPanelHost.BeginInvoke(new MethodInvoker(delegate
                        {

                            Recorded.Add(name);
                            ReplayPanel repc = RepCache[name];
                            repc.Dock = DockStyle.Top;
                            SortedReplays.Add(repc);
                            //ReplayPanelHost.Controls.Add(repc);

                            //if (!repc.GameWon)
                            //    ReplayPanelHost.Controls.SetChildIndex(repc, 0);
                            //else wins++;


                            // AddReplays(ref panels, 1, repc);

                        }));
                    }
                    else
                    {

                        GhostReplay rep = new GhostReplay(file, SettingsManager.Settings.GameVersion, false, Application.StartupPath + @"\Temp");
                        if (rep.ReadReplay(ReadMode.AllExceptData))
                        {

                            if (!(SettingsManager.Settings.AutoMoveOld && CheckMove(rep)))
                            {
                                //if (InvokeRequired)
                                //    this.BeginInvoke(new AddControlInvoker(AddControl), repc);
                                //else ReplaysTab.Controls.Add(repc);

                                // ReplaysTab.Controls.Add(repc);
                                ReplayPanelHost.BeginInvoke(new MethodInvoker(delegate
                                {

                                    Recorded.Add(name);
                                    ReplayPanel repc = new ReplayPanel(rep);
                                    repc.Dock = DockStyle.Top;
                                  //  ReplayPanelHost.Controls.Add(repc);

                                    SortedReplays.Add(repc);
                              

                                    //if (!repc.GameWon)

                                    //    ReplayPanelHost.Controls.SetChildIndex(repc, 0);
                                    //else wins++;

                                    if (!RepCache.ContainsKey(name))
                                        RepCache.Add(name, repc);
                                    // AddReplays(ref panels, 1, repc);

                                }));
                            }
                        }


                    }
                });
            
          
               Sort(SortedReplays, 0, SortedReplays.Count, 1);
        
             //  SortedReplays.Reverse();
                       ReplayPanelHost.BeginInvoke(new MethodInvoker(delegate
                       {
                           foreach (ReplayPanel re in SortedReplays)
                {
             
                           ReplayPanelHost.Controls.Add(re);
                           ReplayPanelHost.Controls.SetChildIndex(re, 0);
                  
                }
                       }));
                // Remove all Replays from control
                ReplayPanelHost.BeginInvoke(new MethodInvoker(delegate
                {
                    replaybx.SubTitle = Recorded.Count.ToString() + " Replays - " + wins.ToString() + " Wins " + (Recorded.Count - wins).ToString() + " Losses";

                }));
            }
            catch (IOException ioex)
            {
             
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to load replay", ex);
            }
           
        }
    
        #endregion

        #region Recording
        void AddRecord(RecordingPanel recp)
        {
            recp.Dock = DockStyle.Top;
            recp.Visible = false;
            RecordingPanelHost.Controls.Add(recp);
       //     this.RecordingPanelHost.Refresh();
            recordingbx.SubTitle =( Recording.Count+1).ToString() + " Games recording";
        }
        void RemoveRecord(RecordingPanel recp)
        {
            RecordingPanelHost.Controls.Remove(recp);
            recordingbx.SubTitle = Recording.Count.ToString() + " Games recording";
            //     this.RecordingPanelHost.Refresh();
        }


        internal void ReplayRecorded(ReplayTask rep, RecordingPanel gui)
        {
            try
            {
             
                Recording.Remove(Path.GetFileNameWithoutExtension(rep.ReplayRecording.FileName));
                  RemoveRecord(gui);
                this.Refresh();

                LoadReplays();
                if (rep.Player != null)
                    ShowInfo("Replay Recorded", "The replay of " + rep.Player + " was successfully recorded");
                else ShowInfo("Replay Recorded", "The replay " + rep.GameID.ToString() + " on " + rep.Platform + " was successfully recorded");

                if (SettingsManager.Settings.Speech)
                {
                    SoundPlayer sp = new SoundPlayer(Application.StartupPath + @"\Data\recorded.wav");
                    sp.Load();
                    sp.PlaySync();
                    sp.Dispose();
                }
                rep.Dispose();
                //if (SettingsManager.Settings.Speech)
                //    Speech.SpeakAsync("The game of "+rep.SummonerName  +" was successfully recorded");

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to save replay", ex);
            }
        }
         internal void CancelReplayRecorded(ReplayTask rep, RecordingPanel gui)
        {
            try
            {
                Recording.Remove(gui.RepTask.recorder.GameId.ToString() + "-" + gui.RepTask.recorder.Region);
                RemoveRecord(gui);
                this.Refresh();
                rep.Dispose();
                //if (SettingsManager.Settings.Speech)
                //    Speech.SpeakAsync("Recording cancelled");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to cancel recording", ex);
            }
        }
  
        public void SimulateRecordFeatured(RiotSharp.Featured.GameList game, string server)
        {
            try
            {


                string platform = game.platformId;

                if (!Recorded.Contains(game.gameId.ToString() + "-" + platform.ToUpper()) && !Recording.Contains(game.gameId.ToString() + "-" + platform.ToUpper()))
                {

                    MainTabControl.SelectedTab = HomeTab;
                    RecordingPanel rec = new RecordingPanel();
                    rec.Record(game.gameId, game.platformId, game.observers.encryptionKey, server, game.participants[0].summonerName, CurrentGame.GetChampion(game.participants[0].championId));
                    rec.Dock = DockStyle.Top;
                    AddRecord(rec);
                    Recording.Add(game.gameId.ToString() + "-" + platform.ToUpper());
                    // }
                    rec.Visible = false;
                    animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                    animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);

                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to record featured", ex);
            }
        }

        void SimulateRecord(RootObject game, RiotSharp.RiotApi api, RiotSharp.Region reg, long sid)
        {
            try
            {


                string platform = RiotTool.RegionToPlatformId(reg.ToString());

                if (!Recorded.Contains(game.gameId.ToString() + "-" + platform.ToUpper()) && !Recording.Contains(game.gameId.ToString() + "-" + platform.ToUpper()))
                {
                    if (sid == -1)
                        sid = SelectedAccount.PlayerID;




                    // record
                    RecordingPanel rec = new RecordingPanel();

                    rec.Record(sid, game, game.platformId);
                    if (reg == RiotSharp.Region.pbe)
                    {
                        SettingsManager.Settings.Accounts[0].PbePlayerID = sid;
                        SettingsManager.Settings.Accounts[0].PbeSummonerName = rec.RepTask.Player;
                        SettingsManager.Save();
                    }
                    rec.Dock = DockStyle.Top;
                    AddRecord(rec);
                    Recording.Add(game.gameId.ToString() + "-" + platform.ToUpper());
                    rec.Visible = false;
                    animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                    animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);

                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to record", ex);
            }
        }
        public void SimulateRecord(long GameId, string server, string enc, string platform)
        {
            try
            {

                this.MainTabControl.SelectedTab = HomeTab;

                // record


                if (!Program.MainFormInstance.Recorded.Contains(GameId.ToString() + "-" + platform.ToUpper()) && !Recording.Contains(GameId.ToString() + "-" + platform.ToUpper()))
                {
                    RecordingPanel rec = new RecordingPanel();


                    rec.Record(GameId, platform, enc, server, "Spectator");
                    rec.Dock = DockStyle.Top;
                    AddRecord(rec);
                    Recording.Add(GameId.ToString() + "-" + platform.ToUpper());
                    rec.Visible = false;
                    animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                    animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                }

            }
            catch (Exception ex)
            {
                //  MetroMessageBox.Show(Program.MainFormInstance, "Failed to find player", "Player not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.Instance.Log.Error("Failed to record", ex);
            }
        }
        void SimulateRecord(string specmd)
        {
            try
            {



                // record
                string[] RemoveExcessInfo = specmd.Split(new string[1] { "spectator " }, StringSplitOptions.None);

                if (RemoveExcessInfo.Length != 2)
                {
                    MetroMessageBox.Show(this, "Corrupt spectator command", "Unable to record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                string[] Info = RemoveExcessInfo[1].Replace("\"", "").Split(' ');

                if (Info.Length != 5)
                {
                    MetroMessageBox.Show(this, "Corrupt spectator command", "Unable to record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }



                long GameId = Convert.ToInt64(Info[2]);
                if (!Recorded.Contains(Info[2] + "-" + Info[3].ToUpper()) && !Recording.Contains(Info[2] + "-" + Info[3].ToUpper()))
                {
                    RecordingPanel rec = new RecordingPanel();

                    rec.Record(GameId, Info[3], Info[1], Info[0], "Spectator");
                    rec.Dock = DockStyle.Top;
                    AddRecord(rec);
                    Recording.Add(Info[2] + "-" + Info[3].ToUpper());
                    rec.Visible = false;
                    animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                    animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                }

            }
            catch (Exception ex)
            {
                //   MetroMessageBox.Show(this, "Failed to find player", "Player not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.Instance.Log.Error("Failed to record", ex);
            }
        }


        #endregion

        #region Champions
        public void ShowChampInfo(int id)
        {
            try
            {
                championCtrl1.LoadChampionInformation(id);
             MainTabControl .SelectedTab = ChampionsTab;
            }
            catch
            {

            }

        }


        #endregion
    


    
       
        #region Game Info

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string server = regiontxt.Text;
                RiotSharp.Region reg = RiotSharp.Region.euw;

                switch (server.ToUpper())
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
                    case "PBE":
                        reg = RiotSharp.Region.pbe;
                        break;
                }

                if (server == "Cache")
                {
                    // record
                    long gid, cid;
                    string[] d = summonertxt.Text.Split('-');
                    gid = long.Parse(d[0]);
                    if (Directory.Exists(Application.StartupPath + @"\Temp\" + summonertxt.Text))
                    {
                        string play = null;
                        string ke = File.ReadAllText(Application.StartupPath + @"\Temp\" + summonertxt.Text + @"\cache.dat");
                        if (File.Exists(Application.StartupPath + @"\Temp\" + summonertxt.Text + @"\summoner.dat"))
                            play = File.ReadAllText(Application.StartupPath + @"\Temp\" + summonertxt.Text + @"\summoner.dat");
                        reg = RiotTool.PlatformToRegion(d[1]);
                        if (!Recorded.Contains(summonertxt.Text.ToUpper()) && !Recording.Contains(summonertxt.Text.ToUpper()))
                        {
                            long sid = API.GetSummoner(reg, play).Id;
                            RootObject game = null;
                            string platform = d[1];
                            if (RiotTool.IsPlayerInGame(API, out game, sid, platform, reg))
                            {

                                RecordingPanel rec = new RecordingPanel();

                                rec.Record(gid, d[1], ke, RiotTool.Servers[d[1].ToUpper()], play, game, sid);
                                rec.Dock = DockStyle.Top;
                                AddRecord(rec);
                                Recording.Add(summonertxt.Text);
                                rec.Visible = false;
                                animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                                animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                            }
                            else
                            {
                                RecordingPanel rec = new RecordingPanel();

                                rec.Record(gid, d[1], ke, RiotTool.Servers[d[1].ToUpper()], play);
                              rec.Dock = DockStyle.Top;
                                AddRecord(rec);
                                Recording.Add(summonertxt.Text);
                                rec.Visible = false;
                                animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                                animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);

                       
                          
                       
                            }
                        }
                        else MetroMessageBox.Show(this, "Already recorded or recording", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                        MetroMessageBox.Show(this, "No cache detected", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                else if (server == "Spectator")
                {
                    // record
                    string[] RemoveExcessInfo = summonertxt.Text.Split(new string[1] { "spectator " }, StringSplitOptions.None);

                    if (RemoveExcessInfo.Length != 2)
                    {
                        MetroMessageBox.Show(this, "Corrupt spectator command", "Unable to record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;
                    }

                    string[] Info = RemoveExcessInfo[1].Replace("\"", "").Split(' ');

                    if (Info.Length != 4)
                    {
                        MetroMessageBox.Show(this, "Corrupt spectator command", "Unable to record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;
                    }

                    if (!Recorded.Contains(Info[2] + "-" + Info[3].ToUpper()) && !Recording.Contains(Info[2] + "-" + Info[3].ToUpper()))
                    {

                        long GameId = Convert.ToInt64(Info[2]);
                        RecordingPanel rec = new RecordingPanel();

                        rec.Record(GameId, Info[3], Info[1], Info[0], "Spectator");
                        rec.Dock = DockStyle.Top;
                        AddRecord(rec);
                        Recording.Add(Info[2] + "-" + Info[3].ToUpper());
                        rec.Visible = false;
                        animator1.AnimationType = AnimatorNS.AnimationType.Particles;
                        animator1.Show(rec, MainTabControl.SelectedTab == HomeTab, SettingsManager.Settings.AnimatorEnabled);
                               
                  
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Already recorded or recording", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }


                }
                else
                {
                    long sid = 0;
                    if (reg == RiotSharp.Region.pbe && long.TryParse(summonertxt.Text,out sid))
                    {
                        RootObject game = null;

                        if (RiotTool.IsPlayerInGame(API, out game, sid, "PBE1", reg))
                        {

                            double seconds = game.gameLength % 60;
                            double minutes = game.gameLength / 60;
                            string dur = string.Format("{0:0}:{1:00}", minutes, seconds);
                            if (MetroMessageBox.Show(this, "The Player " + summonertxt.Text + " is in-game now \nGame ID : " + game.gameId.ToString() + "\nGame Time: " + dur + "\nDo you want to record it ?", "Game Found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (!Recorded.Contains(game.gameId.ToString() + "-" + "PBE1") && !Recording.Contains(game.gameId.ToString() + "-" + "PBE1"))
                                {

                                    // record
                                    MainTabControl.SelectedTab = HomeTab;
                                    this.BeginInvoke(new UpdateCurrentGameInvoker(this.SimulateRecord), game, this.API, reg, sid);
                                }
                                else
                                {
                                    MetroMessageBox.Show(this, "Already recorded or recording", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                            }
                        }
                        else
                            MetroMessageBox.Show(this, "Player is not in game", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if(reg != RiotSharp.Region.pbe)
                    {

                        string platform = RiotTool.RegionToPlatformId(reg.ToString());


                        sid = API.GetSummoner(reg, summonertxt.Text).Id;
                        RootObject game = null;

                        if (RiotTool.IsPlayerInGame(API, out game, sid, platform, reg))
                        {

                            double seconds = game.gameLength % 60;
                            double minutes = game.gameLength / 60;
                            string dur = string.Format("{0:0}:{1:00}", minutes, seconds);
                            if (MetroMessageBox.Show(this, "The Player " + summonertxt.Text + " is in-game now \nGame ID : " + game.gameId.ToString() + "\nGame Time: " + dur + "\nDo you want to record it ?", "Game Found", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (!Recorded.Contains(game.gameId.ToString() + "-" + platform.ToUpper()) && !Recording.Contains(game.gameId.ToString() + "-" + platform.ToUpper()))
                                {

                                    // record
                                    MainTabControl.SelectedTab = HomeTab;
                                    this.BeginInvoke(new UpdateCurrentGameInvoker(this.SimulateRecord), game, this.API, reg, sid);
                                }
                                else
                                {
                                    MetroMessageBox.Show(this, "Already recorded or recording", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                            }
                        }
                        else
                            MetroMessageBox.Show(this, "Player is not in game", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to get game info", ex);
            }
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string server = regiontxt.Text;
                if (server == "Cache" || server == "Spectator")
                    return;
                RiotSharp.Region reg = RiotSharp.Region.euw;

                switch (server.ToUpper())
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
                    case "PBE":
                        reg = RiotSharp.Region.pbe;
                        break;
                }
                long sid = 0;
                if (reg == RiotSharp.Region.pbe && long.TryParse(summonertxt.Text, out sid))
                {
                
                    RootObject game = null;
                    game = API.GetCurrentGamePBE(reg, sid, "PBE1");
                    UpdateCurrentGameInvoker u = new UpdateCurrentGameInvoker(currentGame1.LoadGame);
                    u.BeginInvoke(game, API, reg, 0, null, null);
                }
                else if(reg != RiotSharp.Region.pbe)
                {

                    string platform = RiotTool.RegionToPlatformId(reg.ToString());


                    sid = API.GetSummoner(reg, summonertxt.Text).Id;
                    RootObject game = null;
                    game = API.GetCurrentGame(reg, sid, platform);
                    UpdateCurrentGameInvoker u = new UpdateCurrentGameInvoker(currentGame1.LoadGame);
                    u.BeginInvoke(game, API, reg, 0, null, null);
                    // currentGame1.LoadGame(game, API, reg);
                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to get game info", ex);
            }
        }

        #endregion


        #region Notify Icon & Form Events
        void nsContextMenu1_Opening(object sender, CancelEventArgs e)
    {
    
                playPBEToolStripMenuItem.Visible = SettingsManager.Settings.HasPBE;
    }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Show();
            }
            catch
            {

            }
        }
        private void replayitem_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabControl.SelectedTab = ReplaysTab;
                this.Show();
            }
            catch
            {

            }
        }
        private void play_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(RiotTool.Instance.InstalledGameProfile.Launcher);
            }
            catch
            {

            }
        }
        private void playpbe_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(RiotTool.Instance.InstalledPbeProfile.Launcher);
            }
            catch
            {

            }
        }
        private void homeitem_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabControl.SelectedTab = HomeTab;
                this.Show();
            }
            catch
            {

            }
        }
        private void settingsitem_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabControl.SelectedTab = SettingsTab;
                this.Show();
            }
            catch
            {

            }
        }
        private void Aboutitem_Click(object sender, EventArgs e)
        {
            try
            {
                MainTabControl.SelectedTab = AboutTab;
                this.Show();
            }
            catch
            {

            }
        }
        bool canexit = false;
        private void exitmenuitem_Click(object sender, EventArgs e)
        {
            canexit = true;
            Application.Exit();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
         
       
            UpdateInvoker asyncrep = new UpdateInvoker(LoadReplays);
           asyncrep.BeginInvoke(null, null);
            featuredtimer.Enabled = true;
            //asyncrep = new UpdateInvoker(LoadFeatured);
            //asyncrep.BeginInvoke(null, null);
            followtimer.Enabled = true;
            RefillTheFollowerQueue();
            esportsControl1.LoadEsportsAsync();
            if (Process.GetProcessesByName("League of Legends").Length > 0 && SettingsManager.Settings.RecordIfLate)
                RiotTool.Instance.LolStarted(this, EventArgs.Empty);
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            e.Cancel = !canexit;
            if (e.Cancel && animator1.IsCompleted)
                this.Hide();
        }

      internal  void ShowInfo(string title, string message)
        {
            notifyIcon1.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }
        #endregion

      #region Stream
      internal void CancelStream(StreamControl gui)
        {
            try
            {
                StreamsHostPanel.Controls.Remove(gui);
                streambx.SubTitle = StreamManager.StreamCount.ToString() + " Streams online";
                streams.Remove(gui);
                
                this.Refresh();

                //if (SettingsManager.Settings.Speech)
                //    Speech.SpeakAsync("Recording cancelled");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to cancel recording", ex);
            }
        }
        List<StreamControl> streams = new List<StreamControl>();
        internal void AddStream(GhostReplay rep, string player, Image champ, int cid)
        {
            try
            {
                if (!StreamManager.IsStreaming(rep.GameId, rep.Platform))
                {
         
                    StreamControl str = new StreamControl();
                    str.SetUI(player, champ,cid);
                    if (!rep.IsPBE)
                        str.LoadAndStart(SettingsManager.Settings.GameDirectory, RiotTool.Instance.GetLatestGameReleaseDeploy(), rep);
                    else str.LoadAndStart(SettingsManager.Settings.PbeDirectory, RiotTool.Instance.GetLatestGameReleaseDeploy(SettingsManager.Settings.PbeDirectory), rep);
                    str.Dock = DockStyle.Top;
                    str.Visible = false;
                    StreamsHostPanel.Controls.Add(str);
                    MainTabControl.SelectedTab = StreamsTab;
                    animator1.AnimationType = AnimatorNS.AnimationType.Leaf;
                    animator1.Show(str, MainTabControl.SelectedTab == StreamsTab, SettingsManager.Settings.AnimatorEnabled);
                    streams.Add(str);
                   

                streambx.SubTitle = StreamManager.StreamCount.ToString() + " Streams online";
                }
            }
            catch
            {

            }
        }
      #endregion


        #region Followed Summoners
        void FindFollowedGame(string followed)
        {
            if (followed == null)
                return;
            try
            {

                string[] s = followed.Split('-');
                RiotSharp.Region reg = RiotSharp.Region.euw;

                switch (s[1].ToUpper())
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
                    case "PBE":
                        reg = RiotSharp.Region.pbe;
                        break;
                }
                
                long sid = API.GetSummoner(reg, s[0]).Id;
                RootObject game = null;
                string platform = RiotTool.RegionToPlatformId(s[1]);
    
             if (RiotTool.IsPlayerInGame(API, out game, sid, platform, reg) && (game.gameLength / 60) <= 5)
                    this.BeginInvoke(new UpdateCurrentGameInvoker(this.SimulateRecord), game, this.API, reg, sid);
                

            }
            catch
            {

            }
        }
        Queue<string> FollowQueue = new Queue<string>();
        void FollowedRoaming()
        {
            try
            {
                if (Process.GetProcessesByName("League of Legends.exe").Length > 0 || SettingsManager.Settings.FollowedSummoners == null )
                    return;
                
                if (FollowQueue.Count > 0)
                {

                    string follow = FollowQueue.Dequeue();
                                           FindFollowedGame(follow);

                   FollowQueue.Enqueue(follow);

                }
            }
            catch
            {

            }
        }
        void followtimer_Tick(object sender, EventArgs e)
        {
            FollowWork();
        }
        internal void RefillTheFollowerQueue()
        {
            try
            {
                FollowQueue.Clear();
                foreach (string follow in SettingsManager.Settings.FollowedSummoners)
                    FollowQueue.Enqueue(follow);
            }
            catch
            {

            }
        }
        int GetInterval()
        {
            if (SettingsManager.Settings.FollowedSummoners == null)
                return 10000;
            else
                return 60000 / SettingsManager.Settings.FollowedSummoners.Length;

        }
        void FollowWork()
        {
            try
            {
                      followtimer.Interval = GetInterval();
    // RO ASYNC
                      MethodInvoker mi = new MethodInvoker(FollowedRoaming);
                      mi.BeginInvoke(null, null);
              
                
            }
            catch
            {

            }
        }
        #endregion

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }
            base.WndProc(ref message);
        }
        bool minimizedToTray = false;
        public void ShowWindow()
        {
            try
            {
                if (minimizedToTray)
                {

                    this.Show();
                    this.WindowState = FormWindowState.Maximized;
                    minimizedToTray = false;
                }
                else
                {
                    WinApi.ShowToFront(this.Handle);
                }

            }
            catch
            {

            }
            try
            {
                if (File.Exists(Application.StartupPath + @"\ARGS.t"))
                {
                 Program.ParseArgs(File.ReadAllLines(Application.StartupPath + @"\ARGS.t"));
                    File.Delete(Application.StartupPath + @"\ARGS.t");
                }
            }
            catch
            {
            }
        }

     
    }
}
