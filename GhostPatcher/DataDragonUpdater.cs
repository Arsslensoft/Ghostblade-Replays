using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GhostBase
{
    public delegate void StatusMessageEventHandler(string status);
    public delegate void ProgressEventHandler(int progress);

    public class DataDragonUpdater
    {
        Realm _realm = null;
        Region _basereg = Region.euw;
        WebClient _wbc;
        internal RiotSharp.StaticRiotApi StaticApi { get; set; }  
        internal Realm DataDragonRealm
        {
            get
            {
                if (_realm == null)
                    _realm = GetRealm(_basereg);

                return _realm;
            }
        }
        public string SpellsDirectory
        {
            get { return System.Windows.Forms.Application.StartupPath + @"\Spells"; }
        }
        public string ChampionsDirectory
        {
            get { return System.Windows.Forms.Application.StartupPath + @"\Champions"; }
        }
        public string RankDirectory
        {
            get { return System.Windows.Forms.Application.StartupPath + @"\Rank"; }
        }
        public string ItemsDirectory
        {
            get { return System.Windows.Forms.Application.StartupPath + @"\Items"; }
        }
        public string IconsDirectory
        {
            get { return System.Windows.Forms.Application.StartupPath + @"\Icons"; }
        }


        public event StatusMessageEventHandler OnStatusMessage;
        public event ProgressEventHandler OnProgress;
        
        void UpdateStatus(string msg)
        {
            if (OnStatusMessage != null)
                OnStatusMessage(msg);
        }
        void UpdateProgress(int prog)
        {
            if (OnProgress != null)
                OnProgress(prog);
        }
        Realm GetRealm(Region reg)
        {
            try
            {
                return StaticApi.GetRealm(reg);
            }
            catch (Exception ex)
            {
                BaseInstance.Log("Failed to get DataDragon Realm", ex);
            }
            return null;
        }
        void PrepareDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
        void ExportRank(System.Drawing.Bitmap bmp,string name)
        {
            if (!File.Exists(RankDirectory + @"\" + name + ".png"))
            {
                UpdateStatus(name);
                bmp.Save(RankDirectory + @"\" + name + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public DataDragonUpdater(Region reg) : this("//TODO:BASE API KEY",reg) 
        {

        }
        public DataDragonUpdater(string apikey, Region region)
        {
            StaticApi=RiotSharp.StaticRiotApi.GetInstance(apikey);
            _basereg = region;
            _wbc = new WebClient();
        }


        public void Prepare()
        {
            PrepareDirectory(ChampionsDirectory);
            PrepareDirectory(SpellsDirectory);
            PrepareDirectory(ItemsDirectory);
            PrepareDirectory(IconsDirectory);
            PrepareDirectory(RankDirectory);

        }
        public void UpdateChampions()
        {
            UpdateStatus("Updating champions...");
            int i = 0;
            double prog = 0;
            using (StreamWriter str = new StreamWriter(ChampionsDirectory + @"\Champions.txt", true))
            {
                ChampionListStatic champs = StaticApi.GetChampions(_basereg, ChampionData.image);
                foreach (KeyValuePair<string, ChampionStatic> cs in champs.Champions)
                {
                    i++;
                    prog = (double)i / champs.Champions.Count;
                   
                        UpdateProgress((int)(prog * 100));

                    if (!File.Exists(ChampionsDirectory + @"\" + cs.Key.ToString() + ".png"))
                    {
                        _wbc.DownloadFile(DataDragonRealm.Cdn + "/" + DataDragonRealm.Dd + "/img/champion/" + cs.Value.Image.Full, ChampionsDirectory + @"\" + cs.Key + ".png");
                        Console.WriteLine(cs.Key);
                        str.WriteLine(cs.Value.Id.ToString() + "," + cs.Key);
                      UpdateStatus("Champion " + cs.Key.ToString() + ".png");
                    }
                }
            }
            UpdateStatus("Champions Updated");

        }
        public void UpdateItems()
        {
            UpdateStatus("Updating items...");
            int i = 0;
            double prog = 0;
            ItemListStatic items  = StaticApi.GetItems(_basereg, ItemData.image);
            foreach (KeyValuePair<int, ItemStatic> it in items.Items)
            {
                i++;
                prog = (double)i / items.Items.Count;
                UpdateProgress((int)(prog * 100));

                if (!File.Exists(ItemsDirectory + @"\" + it.Value.Id.ToString() + ".png"))
                {
                    _wbc.DownloadFile(DataDragonRealm.Cdn + "/" + DataDragonRealm.Dd + "/img/item/" + it.Value.Image.Full, ItemsDirectory + @"\" + it.Value.Id.ToString() + ".png");
                   UpdateStatus("Item " + it.Value.Id.ToString() + ".png");
                }

            }
            UpdateStatus("Items Updated");
        }
        public void UpdateSpells()
        {
            UpdateStatus("Updating spells...");
            SummonerSpellListStatic spells = StaticApi.GetSummonerSpells(_basereg, SummonerSpellData.image);
            int i = 0;
            double prog = 0;
            foreach (KeyValuePair<string, SummonerSpellStatic> sp in spells.SummonerSpells)
            {
                i++;
                prog = (double)i / spells.SummonerSpells.Count;
                UpdateProgress((int)(prog * 100));

                if (!File.Exists(SpellsDirectory+ @"\" + sp.Value.Id.ToString() + ".png"))
                {
                     UpdateStatus("Spell " + sp.Value.Id.ToString() + ".png");
                     _wbc.DownloadFile(DataDragonRealm.Cdn + "/" + DataDragonRealm.Dd + "/img/spell/" + sp.Value.Image.Full, SpellsDirectory + @"\" + sp.Value.Id.ToString() + ".png");
                }
            }
            UpdateStatus("Spells Updated");
        }
        public void UpdateRanks()
        {
            UpdateStatus("Exporting ranks...");
            ExportRank(GhostBase.Properties.Resources.BRONZE, "BRONZE");
            ExportRank(GhostBase.Properties.Resources.SILVER, "SILVER");
            ExportRank(GhostBase.Properties.Resources.GOLD, "GOLD");
            ExportRank(GhostBase.Properties.Resources.PLATINUM, "PLATINUM");
            ExportRank(GhostBase.Properties.Resources.DIAMOND, "DIAMOND");
            ExportRank(GhostBase.Properties.Resources.MASTER, "MASTER");
            ExportRank(GhostBase.Properties.Resources.CHALLENGER, "CHALLENGER");
            UpdateStatus("Ranks exported...");
        }

        public void GetIcon(int id)
        {
            if (!File.Exists(IconsDirectory + @"\" + id.ToString() + ".png"))
            _wbc.DownloadFile(string.Format("http://ddragon.leagueoflegends.com/cdn/{0}/img/profileicon/{1}.png",DataDragonRealm.V ,id),IconsDirectory + @"\"+id.ToString()+".png");
        }

        public bool UpdateAll(string version)
        {
            try
            {
                UpdateStatus("Prepare");
                Prepare();
                UpdateStatus("Getting DataDragon Realm...");
                if (DataDragonRealm == null)
                {
                    UpdateStatus("Failed to update");
                    return false;

                }

                //if (DataDragonRealm.V == version)
                //{
                //    UpdateStatus("Up to date");
                //    return true;
                //}


                UpdateRanks();
                UpdateSpells();
                UpdateItems();
                UpdateChampions();
                UpdateStatus("Successfully Updated");
                return true;
            }
            catch(Exception ex)
            {
                UpdateStatus("Failed to update");
                BaseInstance.Log("Failed to update", ex);
                return false;
            }
        }
        public bool IsUpToDate(string version)
        {
            if (DataDragonRealm == null)
                return true;

            

            if (DataDragonRealm.V == version)
                return true;

            return false;
        }

    }
}
