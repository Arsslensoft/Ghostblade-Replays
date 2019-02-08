using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using RiotSharp.MatchEndpoint;
using RiotSharp.ChampionEndpoint;
using System.IO;
using RiotSharp.LeagueEndpoint;
using System.Threading;
using GhostLib;
using System.Media;

namespace Ghostblade
{
    public partial class CurrentGame : MetroUserControl
    {
  
      
        public static string GetChampion(int id)
        {
            return GhostRessources.Instance.GetChampion(id);

        }
        public static int GetChampionID(string id)
        {
            return GhostRessources.Instance.GetChampionID(id);

        }
        internal AnimatorNS.Animator frm;
        public CurrentGame()
        {
            InitializeComponent();
  
        }



        public void LoadGame(RootObject gameinfo, RiotSharp.RiotApi api, RiotSharp.Region reg, long sid)
        {
            try
            {

                BluePanel.BeginInvoke(new MethodInvoker(delegate
                {

                    BluePanel.Controls.Clear();
                    RedPanel.Controls.Clear();
                    ginfo.Text = RiotTool.ToMapString(gameinfo.MapType) + ", " + RiotTool.ToQueueString(gameinfo.gameQueueConfigId) + " - " + RiotTool.PlatformToString(gameinfo.platformId);
                    metroProgressSpinner1.Visible = true;


                    B1.Visible = (gameinfo.bannedChampions.Count != 0);
                    B2.Visible = (gameinfo.bannedChampions.Count != 0);
                    B3.Visible = (gameinfo.bannedChampions.Count != 0);
                    B4.Visible = (gameinfo.bannedChampions.Count != 0);
                    B5.Visible = (gameinfo.bannedChampions.Count != 0);
                    B6.Visible = (gameinfo.bannedChampions.Count != 0);


                    foreach (BannedChampion b in gameinfo.bannedChampions)
                    {
                        Image champ = Image.FromFile(Application.StartupPath + @"\Champions\" + CurrentGame.GetChampion(b.ChampionId) + ".png");
                        switch (b.PickTurn)
                        {
                            case 1:
                                B1.BackgroundImage = champ;
                                break;
                            case 3:
                                B2.BackgroundImage = champ;
                                break;
                            case 5:
                                B3.BackgroundImage = champ;
                                break;

                            case 2:
                                B4.BackgroundImage = champ;
                                break;
                            case 4:
                                B5.BackgroundImage = champ;
                                break;
                            case 6:
                                B6.BackgroundImage = champ;
                                break;
                        }
                    }
                }));
                int c = 0;

                frm.AnimationType = AnimatorNS.AnimationType.Mosaic;
                //foreach (Participant p in gameinfo.participants)
                //{
                ParallelLoopResult pr = Parallel.ForEach(gameinfo.participants, p =>
                   {

                       Player pl = new Player(p.TeamId == 200);


                       pl.Dock = DockStyle.Top;
                       LeaguePlayer pls = new LeaguePlayer();
                       pls.LoadPlayer(reg, api, p, CurrentGame.GetChampion(p.ChampionId));
                       if (pls.SummonerName == "Error occured")
                           pls.SummonerName = p.Name;
                       pl.LoadSummonerData(pls);
                       MetroPanel panel = null;
                       if (p.TeamId == 100)
                           panel = BluePanel;
                       else
                           panel = RedPanel;
                       pls.Dispose();
                       panel.Invoke(new MethodInvoker(delegate
                       {
                           //if (panel == BluePanel && p.TeamId != 100)
                           //    panel = RedPanel;
                           c++;
                           metroProgressSpinner1.Value = (int)((double)((double)c / (double)gameinfo.participants.Count) * 100);

                           panel.Controls.Add(pl);
                           //pl.Visible = false;

                           //frm.Show(pl, Program.MainFormInstance.MainTabControl.SelectedTab == Program.MainFormInstance.GameInfoTab);

                       }));

                       //   }


                   });
                while (!pr.IsCompleted)

                    Thread.Sleep(100);

                RedPanel.Invoke(new MethodInvoker(delegate
                {

                    this.RedPanel.Controls.Add(this.RedBan);
                    this.BluePanel.Controls.Add(this.BlueBan);
                    metroProgressSpinner1.Visible = false;
                    metroProgressSpinner1.Value = 1;
                }));

                Program.MainFormInstance.ShowInfo("Game info", "The current game info are ready to be viewed");
                if (SettingsManager.Settings.Speech)
                {
                    SoundPlayer sp = new SoundPlayer(Application.StartupPath + @"\Data\done.wav");
                    sp.Load();
                    sp.PlaySync();
                    sp.Dispose();
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error("Failed to load game", ex);
            }
        }
  
    }
}
