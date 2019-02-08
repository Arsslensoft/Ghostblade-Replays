using System.Windows.Forms;
namespace Ghostblade
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            AnimatorNS.Animation animation1 = new AnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainTabControl = new GhostLib.Gui.NSTabControl();
            this.HomeTab = new System.Windows.Forms.TabPage();
            this.recordingbx = new GhostLib.Gui.NSGroupBox();
            this.RecordingPanelHost = new System.Windows.Forms.Panel();
            this.svstatbx = new GhostLib.Gui.NSGroupBox();
            this.shardInfoControl1 = new Ghostblade.ShardInfoControl();
            this.featuredbx = new GhostLib.Gui.NSGroupBox();
            this.FeaturedGamesPanel = new System.Windows.Forms.Panel();
            this.recfeaturedbtn = new MetroFramework.Controls.MetroButton();
            this.specfeaturedbtn = new MetroFramework.Controls.MetroButton();
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.featuredgamespaginator = new GhostLib.Gui.NSPaginator();
            this.CurrentFeaturedGamePanel = new System.Windows.Forms.Panel();
            this.ginfo = new MetroFramework.Controls.MetroLabel();
            this.ReplaysTab = new System.Windows.Forms.TabPage();
            this.replaybx = new GhostLib.Gui.NSGroupBox();
            this.ReplayPanelHost = new System.Windows.Forms.Panel();
            this.StreamsTab = new System.Windows.Forms.TabPage();
            this.streambx = new GhostLib.Gui.NSGroupBox();
            this.StreamsHostPanel = new System.Windows.Forms.Panel();
            this.GameInfoTab = new System.Windows.Forms.TabPage();
            this.gameinfobx = new GhostLib.Gui.NSGroupBox();
            this.GameInfoPanelHost = new System.Windows.Forms.Panel();
            this.currentGame1 = new Ghostblade.CurrentGame();
            this.gamesearchbx = new GhostLib.Gui.NSGroupBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.regiontxt = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.summonertxt = new MetroFramework.Controls.MetroTextBox();
            this.EsportsTab = new System.Windows.Forms.TabPage();
            this.esportsControl1 = new Ghostblade.EsportsControl();
            this.ChampionsTab = new System.Windows.Forms.TabPage();
            this.championsbx = new GhostLib.Gui.NSGroupBox();
            this.championCtrl1 = new Ghostblade.ChampionCtrl();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.settingsControl1 = new Ghostblade.SettingsControl();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.aboutControl1 = new Ghostblade.AboutControl();

            this.MainTopBanner = new Ghostblade.TopBanner();
            this.metroStyleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.animator1 = new AnimatorNS.Animator(this.components);
            this.nsContextMenu1 = new GhostLib.Gui.NSContextMenu();
            this.homeitem = new System.Windows.Forms.ToolStripMenuItem();
            this.replayitem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsitem = new System.Windows.Forms.ToolStripMenuItem();
            this.Aboutitem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.leagueOfLegendsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playPBEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitmenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.featuredtimer = new System.Windows.Forms.Timer(this.components);
            this.followtimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainPanel.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.HomeTab.SuspendLayout();
            this.recordingbx.SuspendLayout();
            this.svstatbx.SuspendLayout();
            this.featuredbx.SuspendLayout();
            this.FeaturedGamesPanel.SuspendLayout();
            this.CurrentFeaturedGamePanel.SuspendLayout();
            this.ReplaysTab.SuspendLayout();
            this.replaybx.SuspendLayout();
            this.StreamsTab.SuspendLayout();
            this.streambx.SuspendLayout();
            this.GameInfoTab.SuspendLayout();
            this.gameinfobx.SuspendLayout();
            this.GameInfoPanelHost.SuspendLayout();
            this.gamesearchbx.SuspendLayout();
            this.EsportsTab.SuspendLayout();
            this.ChampionsTab.SuspendLayout();
            this.championsbx.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            this.AboutTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).BeginInit();
            this.nsContextMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.MainPanel.Controls.Add(this.MainTabControl);
            this.MainPanel.Controls.Add(this.MainTopBanner);
            this.animator1.SetDecoration(this.MainPanel, AnimatorNS.DecorationType.None);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MainPanel.Location = new System.Drawing.Point(8, 50);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1135, 599);
            this.MainPanel.TabIndex = 0;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.MainTabControl.Controls.Add(this.HomeTab);
            this.MainTabControl.Controls.Add(this.ReplaysTab);
            this.MainTabControl.Controls.Add(this.StreamsTab);
            this.MainTabControl.Controls.Add(this.GameInfoTab);
            this.MainTabControl.Controls.Add(this.EsportsTab);
            this.MainTabControl.Controls.Add(this.ChampionsTab);
            this.MainTabControl.Controls.Add(this.SettingsTab);
            this.MainTabControl.Controls.Add(this.AboutTab);
            this.animator1.SetDecoration(this.MainTabControl, AnimatorNS.DecorationType.None);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.MainTabControl.ItemSize = new System.Drawing.Size(28, 115);
            this.MainTabControl.Location = new System.Drawing.Point(0, 86);
            this.MainTabControl.Multiline = true;
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1135, 513);
            this.MainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.MainTabControl.TabIndex = 6;
            this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.nsTabControl1_SelectedIndexChanged);
            // 
            // HomeTab
            // 
            this.HomeTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.HomeTab.Controls.Add(this.recordingbx);
            this.HomeTab.Controls.Add(this.svstatbx);
            this.HomeTab.Controls.Add(this.featuredbx);
            this.animator1.SetDecoration(this.HomeTab, AnimatorNS.DecorationType.None);
            this.HomeTab.Location = new System.Drawing.Point(119, 4);
            this.HomeTab.Name = "HomeTab";
            this.HomeTab.Padding = new System.Windows.Forms.Padding(3);
            this.HomeTab.Size = new System.Drawing.Size(1012, 505);
            this.HomeTab.TabIndex = 0;
            this.HomeTab.Text = "Home";
            // 
            // recordingbx
            // 
            this.recordingbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.recordingbx.Controls.Add(this.RecordingPanelHost);
            this.animator1.SetDecoration(this.recordingbx, AnimatorNS.DecorationType.None);
            this.recordingbx.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.recordingbx.DrawSeperator = true;
            this.recordingbx.Location = new System.Drawing.Point(3, 268);
            this.recordingbx.Name = "recordingbx";
            this.recordingbx.Size = new System.Drawing.Size(1006, 234);
            this.recordingbx.SubTitle = "0 Games recording";
            this.recordingbx.TabIndex = 4;
            this.recordingbx.Title = "Recording";
            // 
            // RecordingPanelHost
            // 
            this.RecordingPanelHost.AutoScroll = true;
            this.animator1.SetDecoration(this.RecordingPanelHost, AnimatorNS.DecorationType.None);
            this.RecordingPanelHost.Location = new System.Drawing.Point(3, 43);
            this.RecordingPanelHost.Name = "RecordingPanelHost";
            this.RecordingPanelHost.Size = new System.Drawing.Size(1000, 188);
            this.RecordingPanelHost.TabIndex = 6;
            // 
            // svstatbx
            // 
            this.svstatbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.svstatbx.Controls.Add(this.shardInfoControl1);
            this.animator1.SetDecoration(this.svstatbx, AnimatorNS.DecorationType.None);
            this.svstatbx.DrawSeperator = false;
            this.svstatbx.Location = new System.Drawing.Point(471, 6);
            this.svstatbx.Name = "svstatbx";
            this.svstatbx.Size = new System.Drawing.Size(533, 256);
            this.svstatbx.SubTitle = "Server Status";
            this.svstatbx.TabIndex = 3;
            this.svstatbx.Title = "Europe West";
            // 
            // shardInfoControl1
            // 
            this.shardInfoControl1.AutoScroll = true;
            this.shardInfoControl1.BackColor = System.Drawing.Color.Transparent;
            this.animator1.SetDecoration(this.shardInfoControl1, AnimatorNS.DecorationType.None);
            this.shardInfoControl1.Location = new System.Drawing.Point(3, 49);
            this.shardInfoControl1.Name = "shardInfoControl1";
            this.shardInfoControl1.Size = new System.Drawing.Size(525, 204);
            this.shardInfoControl1.TabIndex = 0;
            // 
            // featuredbx
            // 
            this.featuredbx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.featuredbx.Controls.Add(this.FeaturedGamesPanel);
            this.animator1.SetDecoration(this.featuredbx, AnimatorNS.DecorationType.None);
            this.featuredbx.DrawSeperator = false;
            this.featuredbx.Location = new System.Drawing.Point(3, 6);
            this.featuredbx.Name = "featuredbx";
            this.featuredbx.Size = new System.Drawing.Size(462, 256);
            this.featuredbx.SubTitle = "";
            this.featuredbx.TabIndex = 1;
            this.featuredbx.Title = "Featured Games";
            // 
            // FeaturedGamesPanel
            // 
            this.FeaturedGamesPanel.Controls.Add(this.recfeaturedbtn);
            this.FeaturedGamesPanel.Controls.Add(this.specfeaturedbtn);
            this.FeaturedGamesPanel.Controls.Add(this.metroComboBox1);
            this.FeaturedGamesPanel.Controls.Add(this.featuredgamespaginator);
            this.FeaturedGamesPanel.Controls.Add(this.CurrentFeaturedGamePanel);
            this.animator1.SetDecoration(this.FeaturedGamesPanel, AnimatorNS.DecorationType.None);
            this.FeaturedGamesPanel.Location = new System.Drawing.Point(8, 38);
            this.FeaturedGamesPanel.Name = "FeaturedGamesPanel";
            this.FeaturedGamesPanel.Size = new System.Drawing.Size(448, 215);
            this.FeaturedGamesPanel.TabIndex = 0;
            // 
            // recfeaturedbtn
            // 
            this.animator1.SetDecoration(this.recfeaturedbtn, AnimatorNS.DecorationType.None);
            this.recfeaturedbtn.Location = new System.Drawing.Point(377, 181);
            this.recfeaturedbtn.Name = "recfeaturedbtn";
            this.recfeaturedbtn.Size = new System.Drawing.Size(68, 28);
            this.recfeaturedbtn.TabIndex = 8;
            this.recfeaturedbtn.Text = "Record";
            this.recfeaturedbtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.recfeaturedbtn.UseSelectable = true;
            this.recfeaturedbtn.Click += new System.EventHandler(this.recfeaturedbtn_Click);
            // 
            // specfeaturedbtn
            // 
            this.animator1.SetDecoration(this.specfeaturedbtn, AnimatorNS.DecorationType.None);
            this.specfeaturedbtn.Location = new System.Drawing.Point(312, 181);
            this.specfeaturedbtn.Name = "specfeaturedbtn";
            this.specfeaturedbtn.Size = new System.Drawing.Size(68, 28);
            this.specfeaturedbtn.TabIndex = 9;
            this.specfeaturedbtn.Text = "Spectate";
            this.specfeaturedbtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.specfeaturedbtn.UseSelectable = true;
            this.specfeaturedbtn.Click += new System.EventHandler(this.specfeaturedbtn_Click);
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.animator1.SetDecoration(this.metroComboBox1, AnimatorNS.DecorationType.None);
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 23;
            this.metroComboBox1.Items.AddRange(new object[] {
            "NA",
            "EUW",
            "BR",
            "KR",
            "TR",
            "EUNE",
            "RU",
            "OCE",
            "LAN",
            "LAS",
            "TW",
            "PBE1",
            "VN",
            "PH",
            "TH",
            "ID1",
            "TRSA",
            "TRNA",
            "TRTW",
            "HN1_NEW"});
            this.metroComboBox1.Location = new System.Drawing.Point(3, 180);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(94, 29);
            this.metroComboBox1.TabIndex = 7;
            this.metroComboBox1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroComboBox1.UseSelectable = true;
            this.metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            // 
            // featuredgamespaginator
            // 
            this.animator1.SetDecoration(this.featuredgamespaginator, AnimatorNS.DecorationType.None);
            this.featuredgamespaginator.Location = new System.Drawing.Point(103, 181);
            this.featuredgamespaginator.Name = "featuredgamespaginator";
            this.featuredgamespaginator.NumberOfPages = 5;
            this.featuredgamespaginator.SelectedIndex = 0;
            this.featuredgamespaginator.Size = new System.Drawing.Size(203, 26);
            this.featuredgamespaginator.TabIndex = 1;
            this.featuredgamespaginator.Text = "Featured Games";
            this.featuredgamespaginator.SelectedIndexChanged += new GhostLib.Gui.NSPaginator.SelectedIndexChangedEventHandler(this.featuredgamespaginator_SelectedIndexChanged);
            // 
            // CurrentFeaturedGamePanel
            // 
            this.CurrentFeaturedGamePanel.Controls.Add(this.ginfo);
            this.animator1.SetDecoration(this.CurrentFeaturedGamePanel, AnimatorNS.DecorationType.None);
            this.CurrentFeaturedGamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CurrentFeaturedGamePanel.Location = new System.Drawing.Point(0, 0);
            this.CurrentFeaturedGamePanel.Name = "CurrentFeaturedGamePanel";
            this.CurrentFeaturedGamePanel.Size = new System.Drawing.Size(448, 174);
            this.CurrentFeaturedGamePanel.TabIndex = 0;
            // 
            // ginfo
            // 
            this.ginfo.AutoSize = true;
            this.animator1.SetDecoration(this.ginfo, AnimatorNS.DecorationType.None);
            this.ginfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ginfo.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.ginfo.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.ginfo.Location = new System.Drawing.Point(0, 0);
            this.ginfo.Name = "ginfo";
            this.ginfo.Size = new System.Drawing.Size(0, 0);
            this.ginfo.TabIndex = 10;
            this.ginfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ginfo.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // ReplaysTab
            // 
            this.ReplaysTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ReplaysTab.Controls.Add(this.replaybx);
            this.animator1.SetDecoration(this.ReplaysTab, AnimatorNS.DecorationType.None);
            this.ReplaysTab.Location = new System.Drawing.Point(119, 4);
            this.ReplaysTab.Name = "ReplaysTab";
            this.ReplaysTab.Padding = new System.Windows.Forms.Padding(3);
            this.ReplaysTab.Size = new System.Drawing.Size(1012, 505);
            this.ReplaysTab.TabIndex = 1;
            this.ReplaysTab.Text = "Replays";
            // 
            // replaybx
            // 
            this.replaybx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.replaybx.Controls.Add(this.ReplayPanelHost);
            this.animator1.SetDecoration(this.replaybx, AnimatorNS.DecorationType.None);
            this.replaybx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.replaybx.DrawSeperator = true;
            this.replaybx.Location = new System.Drawing.Point(3, 3);
            this.replaybx.Name = "replaybx";
            this.replaybx.Size = new System.Drawing.Size(1006, 499);
            this.replaybx.SubTitle = "1 Replay - 1 Win 0 Losses";
            this.replaybx.TabIndex = 5;
            this.replaybx.Title = "Replays";
            // 
            // ReplayPanelHost
            // 
            this.ReplayPanelHost.AutoScroll = true;
            this.animator1.SetDecoration(this.ReplayPanelHost, AnimatorNS.DecorationType.None);
            this.ReplayPanelHost.Location = new System.Drawing.Point(64, 43);
            this.ReplayPanelHost.Name = "ReplayPanelHost";
            this.ReplayPanelHost.Size = new System.Drawing.Size(837, 211);
            this.ReplayPanelHost.TabIndex = 6;
            // 
            // StreamsTab
            // 
            this.StreamsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.StreamsTab.Controls.Add(this.streambx);
            this.animator1.SetDecoration(this.StreamsTab, AnimatorNS.DecorationType.None);
            this.StreamsTab.Location = new System.Drawing.Point(119, 4);
            this.StreamsTab.Name = "StreamsTab";
            this.StreamsTab.Padding = new System.Windows.Forms.Padding(3);
            this.StreamsTab.Size = new System.Drawing.Size(1012, 505);
            this.StreamsTab.TabIndex = 6;
            this.StreamsTab.Text = "Streams";
            // 
            // streambx
            // 
            this.streambx.Controls.Add(this.StreamsHostPanel);
            this.animator1.SetDecoration(this.streambx, AnimatorNS.DecorationType.None);
            this.streambx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.streambx.DrawSeperator = true;
            this.streambx.Location = new System.Drawing.Point(3, 3);
            this.streambx.Name = "streambx";
            this.streambx.Size = new System.Drawing.Size(1006, 499);
            this.streambx.SubTitle = "0 Streams online";
            this.streambx.TabIndex = 0;
            this.streambx.Text = "nsGroupBox1";
            this.streambx.Title = "Streams";
            // 
            // StreamsHostPanel
            // 
            this.StreamsHostPanel.AutoScroll = true;
            this.animator1.SetDecoration(this.StreamsHostPanel, AnimatorNS.DecorationType.None);
            this.StreamsHostPanel.Location = new System.Drawing.Point(3, 45);
            this.StreamsHostPanel.Name = "StreamsHostPanel";
            this.StreamsHostPanel.Size = new System.Drawing.Size(800, 293);
            this.StreamsHostPanel.TabIndex = 0;
            // 
            // GameInfoTab
            // 
            this.GameInfoTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.GameInfoTab.Controls.Add(this.gameinfobx);
            this.GameInfoTab.Controls.Add(this.gamesearchbx);
            this.animator1.SetDecoration(this.GameInfoTab, AnimatorNS.DecorationType.None);
            this.GameInfoTab.Location = new System.Drawing.Point(119, 4);
            this.GameInfoTab.Name = "GameInfoTab";
            this.GameInfoTab.Padding = new System.Windows.Forms.Padding(3);
            this.GameInfoTab.Size = new System.Drawing.Size(1012, 505);
            this.GameInfoTab.TabIndex = 2;
            this.GameInfoTab.Text = "Games infos";
            // 
            // gameinfobx
            // 
            this.gameinfobx.Controls.Add(this.GameInfoPanelHost);
            this.animator1.SetDecoration(this.gameinfobx, AnimatorNS.DecorationType.None);
            this.gameinfobx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameinfobx.DrawSeperator = true;
            this.gameinfobx.Location = new System.Drawing.Point(3, 100);
            this.gameinfobx.Name = "gameinfobx";
            this.gameinfobx.Size = new System.Drawing.Size(1006, 402);
            this.gameinfobx.SubTitle = "";
            this.gameinfobx.TabIndex = 1;
            this.gameinfobx.Text = "nsGroupBox2";
            this.gameinfobx.Title = "Game Info";
            // 
            // GameInfoPanelHost
            // 
            this.GameInfoPanelHost.AutoScroll = true;
            this.GameInfoPanelHost.Controls.Add(this.currentGame1);
            this.animator1.SetDecoration(this.GameInfoPanelHost, AnimatorNS.DecorationType.None);
            this.GameInfoPanelHost.Location = new System.Drawing.Point(3, 31);
            this.GameInfoPanelHost.Name = "GameInfoPanelHost";
            this.GameInfoPanelHost.Size = new System.Drawing.Size(1000, 368);
            this.GameInfoPanelHost.TabIndex = 0;
            // 
            // currentGame1
            // 
            this.currentGame1.AutoScroll = true;
            this.currentGame1.BackColor = System.Drawing.Color.Transparent;
            this.animator1.SetDecoration(this.currentGame1, AnimatorNS.DecorationType.None);
            this.currentGame1.Dock = System.Windows.Forms.DockStyle.Top;
            this.currentGame1.Location = new System.Drawing.Point(0, 0);
            this.currentGame1.Name = "currentGame1";
            this.currentGame1.Size = new System.Drawing.Size(983, 700);
            this.currentGame1.TabIndex = 5;
            this.currentGame1.UseSelectable = true;
            // 
            // gamesearchbx
            // 
            this.gamesearchbx.Controls.Add(this.metroButton1);
            this.gamesearchbx.Controls.Add(this.regiontxt);
            this.gamesearchbx.Controls.Add(this.metroLabel2);
            this.gamesearchbx.Controls.Add(this.metroButton2);
            this.gamesearchbx.Controls.Add(this.summonertxt);
            this.animator1.SetDecoration(this.gamesearchbx, AnimatorNS.DecorationType.None);
            this.gamesearchbx.Dock = System.Windows.Forms.DockStyle.Top;
            this.gamesearchbx.DrawSeperator = true;
            this.gamesearchbx.Location = new System.Drawing.Point(3, 3);
            this.gamesearchbx.Name = "gamesearchbx";
            this.gamesearchbx.Size = new System.Drawing.Size(1006, 97);
            this.gamesearchbx.SubTitle = "Find a game using summoner\'s name";
            this.gamesearchbx.TabIndex = 0;
            this.gamesearchbx.Text = "nsGroupBox1";
            this.gamesearchbx.Title = "Game Search";
            // 
            // metroButton1
            // 
            this.animator1.SetDecoration(this.metroButton1, AnimatorNS.DecorationType.None);
            this.metroButton1.Location = new System.Drawing.Point(798, 67);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(116, 23);
            this.metroButton1.TabIndex = 8;
            this.metroButton1.Text = "Record";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // regiontxt
            // 
            this.regiontxt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.animator1.SetDecoration(this.regiontxt, AnimatorNS.DecorationType.None);
            this.regiontxt.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.regiontxt.FormattingEnabled = true;
            this.regiontxt.ItemHeight = 19;
            this.regiontxt.Items.AddRange(new object[] {
            "NA",
            "EUW",
            "BR",
            "KR",
            "TR",
            "EUNE",
            "RU",
            "OCE",
            "LAN",
            "LAS",
            "PBE",
            "Spectator",
            "Cache"});
            this.regiontxt.Location = new System.Drawing.Point(559, 67);
            this.regiontxt.Name = "regiontxt";
            this.regiontxt.Size = new System.Drawing.Size(104, 25);
            this.regiontxt.TabIndex = 7;
            this.regiontxt.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.regiontxt.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.animator1.SetDecoration(this.metroLabel2, AnimatorNS.DecorationType.None);
            this.metroLabel2.Location = new System.Drawing.Point(42, 46);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(111, 19);
            this.metroLabel2.TabIndex = 6;
            this.metroLabel2.Text = "Summoner name";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroButton2
            // 
            this.animator1.SetDecoration(this.metroButton2, AnimatorNS.DecorationType.None);
            this.metroButton2.Location = new System.Drawing.Point(676, 67);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(116, 23);
            this.metroButton2.TabIndex = 5;
            this.metroButton2.Text = "Find game";
            this.metroButton2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton2.UseSelectable = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // summonertxt
            // 
            this.animator1.SetDecoration(this.summonertxt, AnimatorNS.DecorationType.None);
            this.summonertxt.Lines = new string[0];
            this.summonertxt.Location = new System.Drawing.Point(42, 68);
            this.summonertxt.MaxLength = 32767;
            this.summonertxt.Name = "summonertxt";
            this.summonertxt.PasswordChar = '\0';
            this.summonertxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.summonertxt.SelectedText = "";
            this.summonertxt.Size = new System.Drawing.Size(511, 23);
            this.summonertxt.TabIndex = 4;
            this.summonertxt.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.summonertxt.UseSelectable = true;
            // 
            // EsportsTab
            // 
            this.EsportsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.EsportsTab.Controls.Add(this.esportsControl1);
            this.animator1.SetDecoration(this.EsportsTab, AnimatorNS.DecorationType.None);
            this.EsportsTab.Location = new System.Drawing.Point(119, 4);
            this.EsportsTab.Name = "EsportsTab";
            this.EsportsTab.Padding = new System.Windows.Forms.Padding(3);
            this.EsportsTab.Size = new System.Drawing.Size(1012, 505);
            this.EsportsTab.TabIndex = 5;
            this.EsportsTab.Text = "Esports";
            // 
            // esportsControl1
            // 
            this.esportsControl1.BackColor = System.Drawing.Color.Transparent;
            this.animator1.SetDecoration(this.esportsControl1, AnimatorNS.DecorationType.None);
            this.esportsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.esportsControl1.Location = new System.Drawing.Point(3, 3);
            this.esportsControl1.Name = "esportsControl1";
            this.esportsControl1.Size = new System.Drawing.Size(1006, 499);
            this.esportsControl1.TabIndex = 0;
            // 
            // ChampionsTab
            // 
            this.ChampionsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ChampionsTab.Controls.Add(this.championsbx);
            this.animator1.SetDecoration(this.ChampionsTab, AnimatorNS.DecorationType.None);
            this.ChampionsTab.Location = new System.Drawing.Point(119, 4);
            this.ChampionsTab.Name = "ChampionsTab";
            this.ChampionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ChampionsTab.Size = new System.Drawing.Size(1012, 505);
            this.ChampionsTab.TabIndex = 3;
            this.ChampionsTab.Text = "Champions";
            // 
            // championsbx
            // 
            this.championsbx.Controls.Add(this.championCtrl1);
            this.animator1.SetDecoration(this.championsbx, AnimatorNS.DecorationType.None);
            this.championsbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.championsbx.DrawSeperator = true;
            this.championsbx.Location = new System.Drawing.Point(3, 3);
            this.championsbx.Name = "championsbx";
            this.championsbx.Size = new System.Drawing.Size(1006, 499);
            this.championsbx.SubTitle = "Find all champions stats";
            this.championsbx.TabIndex = 0;
            this.championsbx.Text = "nsGroupBox1";
            this.championsbx.Title = "Champions";
            // 
            // championCtrl1
            // 
            this.championCtrl1.AutoScroll = true;
            this.championCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.animator1.SetDecoration(this.championCtrl1, AnimatorNS.DecorationType.None);
            this.championCtrl1.Location = new System.Drawing.Point(3, 44);
            this.championCtrl1.Name = "championCtrl1";
            this.championCtrl1.Size = new System.Drawing.Size(891, 433);
            this.championCtrl1.TabIndex = 0;
            this.championCtrl1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.championCtrl1.UseSelectable = true;
            // 
            // SettingsTab
            // 
            this.SettingsTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SettingsTab.Controls.Add(this.settingsControl1);
            this.animator1.SetDecoration(this.SettingsTab, AnimatorNS.DecorationType.None);
            this.SettingsTab.Location = new System.Drawing.Point(119, 4);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(1012, 505);
            this.SettingsTab.TabIndex = 4;
            this.SettingsTab.Text = "Settings";
            // 
            // settingsControl1
            // 
            this.settingsControl1.AutoScroll = true;
            this.settingsControl1.BackColor = System.Drawing.Color.Transparent;
            this.animator1.SetDecoration(this.settingsControl1, AnimatorNS.DecorationType.None);
            this.settingsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsControl1.Location = new System.Drawing.Point(3, 3);
            this.settingsControl1.Name = "settingsControl1";
            this.settingsControl1.Size = new System.Drawing.Size(1006, 499);
            this.settingsControl1.TabIndex = 1;
            // 
            // AboutTab
            // 
            this.AboutTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.AboutTab.Controls.Add(this.aboutControl1);
            this.animator1.SetDecoration(this.AboutTab, AnimatorNS.DecorationType.None);
            this.AboutTab.Location = new System.Drawing.Point(119, 4);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTab.Size = new System.Drawing.Size(1012, 505);
            this.AboutTab.TabIndex = 5;
            this.AboutTab.Text = "About";
            // 
            // aboutControl1
            // 
            this.aboutControl1.BackColor = System.Drawing.Color.Transparent;
            this.animator1.SetDecoration(this.aboutControl1, AnimatorNS.DecorationType.None);
            this.aboutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutControl1.Location = new System.Drawing.Point(3, 3);
            this.aboutControl1.Name = "aboutControl1";
            this.aboutControl1.Size = new System.Drawing.Size(1006, 499);
            this.aboutControl1.TabIndex = 0;
            // 
            // MainTopBanner
            // 
            this.MainTopBanner.BackColor = System.Drawing.Color.Transparent;
            this.MainTopBanner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.animator1.SetDecoration(this.MainTopBanner, AnimatorNS.DecorationType.None);
            this.MainTopBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainTopBanner.Location = new System.Drawing.Point(0, 0);
            this.MainTopBanner.Name = "MainTopBanner";
            this.MainTopBanner.Size = new System.Drawing.Size(1135, 86);
            this.MainTopBanner.TabIndex = 5;
            // 
            // metroStyleManager1
            // 
            this.metroStyleManager1.Owner = this;
            // 
            // animator1
            // 
            this.animator1.AnimationType = AnimatorNS.AnimationType.Particles;
            this.animator1.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 1;
            animation1.Padding = new System.Windows.Forms.Padding(100, 50, 100, 150);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 2F;
            animation1.TransparencyCoeff = 0F;
            this.animator1.DefaultAnimation = animation1;
            this.animator1.HostForm = this;
            // 
            // nsContextMenu1
            // 
            this.animator1.SetDecoration(this.nsContextMenu1, AnimatorNS.DecorationType.None);
            this.nsContextMenu1.ForeColor = System.Drawing.Color.White;
            this.nsContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeitem,
            this.leagueOfLegendsToolStripMenuItem,
            this.replayitem,
            this.settingsitem,
            this.Aboutitem,
            this.toolStripSeparator1,
            this.exitmenuitem});
            this.nsContextMenu1.Name = "nsContextMenu1";
            this.nsContextMenu1.Size = new System.Drawing.Size(167, 120);
            this.nsContextMenu1.Opening += new  System.ComponentModel.CancelEventHandler(this.nsContextMenu1_Opening);
            // 
            // homeitem
            // 
            this.homeitem.Name = "homeitem";
            this.homeitem.Size = new System.Drawing.Size(166, 22);
            this.homeitem.Text = "Show Ghostblade";
            this.homeitem.Click += new System.EventHandler(this.homeitem_Click);
            // 
            // leagueOfLegendsToolStripMenuItem
            // 
            this.leagueOfLegendsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.playPBEToolStripMenuItem});
            this.leagueOfLegendsToolStripMenuItem.Name = "leagueOfLegendsToolStripMenuItem";
            this.leagueOfLegendsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.leagueOfLegendsToolStripMenuItem.Text = "League of Legends";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.play_Click);
            // 
            // playPBEToolStripMenuItem
            // 
            this.playPBEToolStripMenuItem.Name = "playPBEToolStripMenuItem";
            this.playPBEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.playPBEToolStripMenuItem.Text = "Play PBE";
            this.playPBEToolStripMenuItem.Click += new System.EventHandler(this.playpbe_Click);
            // 
            // replayitem
            // 
            this.replayitem.Name = "replayitem";
            this.replayitem.Size = new System.Drawing.Size(166, 22);
            this.replayitem.Text = "Replays";
            this.replayitem.Click += new System.EventHandler(this.replayitem_Click);
            // 
            // settingsitem
            // 
            this.settingsitem.Name = "settingsitem";
            this.settingsitem.Size = new System.Drawing.Size(166, 22);
            this.settingsitem.Text = "Settings";
            this.settingsitem.Click += new System.EventHandler(this.settingsitem_Click);
            // 
            // Aboutitem
            // 
            this.Aboutitem.Name = "Aboutitem";
            this.Aboutitem.Size = new System.Drawing.Size(166, 22);
            this.Aboutitem.Text = "About";
            this.Aboutitem.Click += new System.EventHandler(this.Aboutitem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // exitmenuitem
            // 
            this.exitmenuitem.Name = "exitmenuitem";
            this.exitmenuitem.Size = new System.Drawing.Size(166, 22);
            this.exitmenuitem.Text = "Exit";
            this.exitmenuitem.Click += new System.EventHandler(this.exitmenuitem_Click);
            // 
            // featuredtimer
            // 
            this.featuredtimer.Interval = 10000;
            this.featuredtimer.Tick += new System.EventHandler(this.featuredtimer_Tick);
            // 
            // folow
            // 
            this.followtimer.Interval = 10000;
            this.followtimer.Tick += new System.EventHandler(this.followtimer_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.nsContextMenu1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Ghostblade Replays";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 657);
            this.Controls.Add(this.MainPanel);
            this.animator1.SetDecoration(this, AnimatorNS.DecorationType.None);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MenuIconInnerBorder = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.MainPanel.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.HomeTab.ResumeLayout(false);
            this.recordingbx.ResumeLayout(false);
            this.svstatbx.ResumeLayout(false);
            this.featuredbx.ResumeLayout(false);
            this.FeaturedGamesPanel.ResumeLayout(false);
            this.CurrentFeaturedGamePanel.ResumeLayout(false);
            this.CurrentFeaturedGamePanel.PerformLayout();
            this.ReplaysTab.ResumeLayout(false);
            this.replaybx.ResumeLayout(false);
            this.StreamsTab.ResumeLayout(false);
            this.streambx.ResumeLayout(false);
            this.GameInfoTab.ResumeLayout(false);
            this.gameinfobx.ResumeLayout(false);
            this.GameInfoPanelHost.ResumeLayout(false);
            this.gamesearchbx.ResumeLayout(false);
            this.gamesearchbx.PerformLayout();
            this.EsportsTab.ResumeLayout(false);
            this.ChampionsTab.ResumeLayout(false);
            this.championsbx.ResumeLayout(false);
            this.SettingsTab.ResumeLayout(false);
            this.AboutTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager1)).EndInit();
            this.nsContextMenu1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Components.MetroStyleManager metroStyleManager1;
        private System.Windows.Forms.Panel MainPanel;
        internal TopBanner MainTopBanner;
        private System.Windows.Forms.TabPage HomeTab;
        private System.Windows.Forms.TabPage ReplaysTab;
        private System.Windows.Forms.Panel FeaturedGamesPanel;
        private System.Windows.Forms.Panel CurrentFeaturedGamePanel;
        private GhostLib.Gui.NSPaginator featuredgamespaginator;
        private MetroFramework.Controls.MetroComboBox metroComboBox1;
        private MetroFramework.Controls.MetroLabel ginfo;
        private AnimatorNS.Animator animator1;
        private MetroFramework.Controls.MetroButton recfeaturedbtn;
        private MetroFramework.Controls.MetroButton specfeaturedbtn;
        private GhostLib.Gui.NSGroupBox featuredbx;
        private GhostLib.Gui.NSGroupBox svstatbx;
        private ShardInfoControl shardInfoControl1;
        private GhostLib.Gui.NSGroupBox recordingbx;
        private Panel RecordingPanelHost;
        private GhostLib.Gui.NSGroupBox replaybx;
        private Panel ReplayPanelHost;
        private System.Windows.Forms.TabPage ChampionsTab;
        private GhostLib.Gui.NSGroupBox gamesearchbx;
        private GhostLib.Gui.NSGroupBox gameinfobx;
        private MetroFramework.Controls.MetroComboBox regiontxt;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroTextBox summonertxt;
        private Panel GameInfoPanelHost;
        internal CurrentGame currentGame1;
        private GhostLib.Gui.NSGroupBox championsbx;
        private ChampionCtrl championCtrl1;
        internal GhostLib.Gui.NSTabControl MainTabControl;
        internal TabPage GameInfoTab;
        private TabPage SettingsTab;
        private SettingsControl settingsControl1;
        private TabPage AboutTab;
        private TabPage EsportsTab;
        private AboutControl aboutControl1;
        private Ghostblade.EsportsControl esportsControl1;
        private GhostLib.Gui.NSContextMenu nsContextMenu1;
        private ToolStripMenuItem homeitem;
        private ToolStripMenuItem replayitem;
        private ToolStripMenuItem settingsitem;
        private ToolStripMenuItem Aboutitem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitmenuitem;
        private System.Windows.Forms.ToolStripMenuItem leagueOfLegendsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playPBEToolStripMenuItem;
        private NotifyIcon notifyIcon1;
        private MetroFramework.Controls.MetroButton metroButton1;
        private TabPage StreamsTab;
        private GhostLib.Gui.NSGroupBox streambx;
        private Panel StreamsHostPanel;
        private Timer featuredtimer;
        private Timer followtimer;

    }
}

