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
using System.IO;
using Microsoft.Win32;
using GhostLib;

namespace Ghostblade
{
    public partial class ChampionCtrl : MetroUserControl
    {
      internal  RiotLib riotLib = new RiotLib();
        /// <summary>
        /// Location of LoL assets folder
        /// </summary>
        private string LoLAssetsPath = null;
        /// <summary>
        /// Returns Riot's assets folder location
        /// </summary>
        /// <param name="installationPath">Installation path of LoL</param>
        /// <returns>Riot's assets folder</returns>
        /// 

        #region Init

        /// <summary>
        /// Inits application, first function that is executed when program starts
        /// </summary>
        private void InitAll()
        {
            LoadData();

     
        }

        /// <summary>
        /// Loads all the required data.
        /// Called when application starts or when settings were changed.
        /// </summary>
        public void LoadData()
        {
            // clear everything
            riotLib.Data.Clear();
            riotLib.Abilities.Clear();
            riotLib.Skins.Clear();

       


            // todo: what if DB is not found
            InitRiotData();

            //LoadDbList();

            // load champions list
            foreach (RiotDataStruct data in riotLib.Data)
                infoList.Items.Add(data.DisplayName);
        }


        /// <summary>
        /// Reads data from Riot SQLite database and setups all images/paths
        /// </summary>
        void InitRiotData()
        {
            riotLib.DbLocation = LoLAssetsPath + @"data\gameStats\gameStats_en_US.sqlite";
            riotLib.GetChampionsData();
            riotLib.GetSkinsData();
            riotLib.GetAbilitiesData();

            // sort, so our list would appear alphabetic
            riotLib.Data.Sort((x, y) => string.Compare(x.Name, y.Name));


            // load images and champion lists
            List<ChampListStruct> champs = new List<ChampListStruct>();
            ImageList imgs = new ImageList();
            imgs.ColorDepth = ColorDepth.Depth32Bit;
            imgs.ImageSize = new Size(32, 32);
            foreach (RiotDataStruct champion in riotLib.Data)
            {
                champs.Add(new ChampListStruct() { Name = champion.Name, DisplayName = champion.DisplayName, Tag = champion.Tags });
             
                imgs.Images.Add(champion.Name, GetImageFile(LoLAssetsPath + @"images\champions\" + champion.IconPath));
             
            }

   
        }


        /// <summary>
        /// Retrieves LoL installation path. Returns NULL if can't be located in the registry
        /// </summary>
        /// <returns>Path to LoL installation folder or NULL</returns>
        private string GetRiotPath()
        {
            RegistryKey rk = Registry.LocalMachine, candidate;
            rk = rk.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            foreach (string key in rk.GetSubKeyNames())
            {
                candidate = rk.OpenSubKey(key);

                // make sure the key belongs to LoL
                if (candidate.GetValue("DisplayName") != null && candidate.GetValue("DisplayName").ToString().CompareTo("League of Legends") == 0)
                {
                    // try to get installation location
                    if (candidate.GetValue("InstallLocation") != null)
                        return candidate.GetValue("InstallLocation").ToString();
                }
            }

            return null;
        }

     
     
        #endregion
        private string GetRiotAssetsPath(string installationPath)
        {
            if (!Directory.Exists(installationPath + @"\RADS\projects\lol_air_client\releases"))
                return String.Empty;

            string[] directories = Directory.GetDirectories(installationPath + @"\RADS\projects\lol_air_client\releases");

            // check for the newest release
            DateTime newest = new DateTime(0);
            string resultPath = "";
            foreach (string dir in directories)
            {
                if (Directory.GetCreationTime(dir) > newest)
                {
                    newest = Directory.GetCreationTime(dir);
                    resultPath = dir;
                }
            }

            return resultPath + @"\deploy\assets\";
        }
        public ChampionCtrl()
        {
            InitializeComponent();
            try
            {


                if (SettingsManager.Settings != null)
                {
                    LoLAssetsPath = GetRiotAssetsPath(SettingsManager.Settings.GameDirectory);
                    LoadData();
                }

            }
            catch { }
        }

        private void infoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChampionInformation(riotLib.GetChampionByDisplayName(infoList.SelectedItem.ToString()).Name);
        }
        internal void LoadChampionInformation(int id)
        {
            RiotDataStruct data = riotLib.GetChampionById(id);

            infoList.SelectedItem = data.DisplayName;

            // fill general data
            infoDisplayName.Text = data.DisplayName;
            infoTitle.Text = data.Title;
            infoTags.Text = data.Tags.Replace(",", ", ");
            infoDescription.Text = data.Description;
            infoTips.Text = data.Tips.Replace("*", "\r\n\r\n* ").Trim();
            infoOpponentTips.Text = data.OpponentTips.Replace("*", "\r\n\r\n* ").Trim();
         
         
                infoIcon.Image = GetImageFile(LoLAssetsPath + @"images\champions\" + data.IconPath);
             


            // populate stats table
            infoStats.AutoGenerateColumns = false;
            List<CompareStatsStruct> list = new List<CompareStatsStruct>();
            list.Add(new CompareStatsStruct()
            {
                Name = "Range",
                Base1 = data.Range.ToString("F1"),
                Level1 = "",
                Level18 = data.Range.ToString("F1")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Move Speed",
                Base1 = data.MoveSpeed.ToString("F1"),
                Level1 = "",
                Level18 = data.MoveSpeed.ToString("F1")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Attack",
                Base1 = data.AttackBase.ToString("F1"),
                Level1 = data.AttackLevel.ToString("F1"),
                Level18 = (data.AttackBase + (data.AttackLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Armor",
                Base1 = data.ArmorBase.ToString("F1"),
                Level1 = data.ArmorLevel.ToString("F1"),
                Level18 = (data.ArmorBase + (data.ArmorLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Magic Resist",
                Base1 = data.MagicResistBase.ToString("F1"),
                Level1 = data.MagicResistLevel.ToString("F1"),
                Level18 = (data.MagicResistBase + (data.MagicResistLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Health",
                Base1 = data.HealthBase.ToString("F1"),
                Level1 = data.HealthLevel.ToString("F1"),
                Level18 = (data.HealthBase + (data.HealthLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Health Regen",
                Base1 = data.HealthRegenBase.ToString("F1"),
                Level1 = data.HealthRegenLevel.ToString("F1"),
                Level18 = (data.HealthRegenBase + (data.HealthRegenLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Mana",
                Base1 = data.ManaBase.ToString("F1"),
                Level1 = data.ManaLevel.ToString("F1"),
                Level18 = (data.ManaBase + (data.ManaLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Mana Regen",
                Base1 = data.ManaRegenBase.ToString("F1"),
                Level1 = data.ManaRegenLevel.ToString("F1"),
                Level18 = (data.ManaRegenBase + (data.ManaRegenLevel * 18)).ToString("F")
            });

            infoStats.DataSource = list;

            // ratings
            infoRatingAttack.Value = (int)data.RatingAttack;
            infoRatingDefense.Value = (int)data.RatingDefense;
            infoRatingMagic.Value = (int)data.RatingMagic;
            infoRatingDifficulty.Value = (int)data.RatingDifficulty;

            // skins
            infoSkinsList.Items.Clear();
            foreach (RiotSkinsStruct skin in riotLib.Skins)
            {
                if (skin.ChampionId == data.Id)
                {
                    if (String.IsNullOrEmpty(skin.DisplayName))
                        infoSkinsList.Items.Add("Base");
                    else
                        infoSkinsList.Items.Add(skin.DisplayName);
                }
            }
            infoSkinsList.SelectedItem = "Base";
            LoadSkin();

            // abilities
            foreach (RiotAbilitiesStruct ability in riotLib.Abilities)
            {
                if (ability.ChampionId != data.Id)
                    continue;

                if (ability.Rank == 1)
                {
                    infoAbilityNameP.Text = "(Passive) " + ability.Name;
                    infoAbilityEffectP.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconP.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                }
                else if (ability.Rank == 2)
                {
                    infoAbilityNameQ.Text = "(Q) " + ability.Name;
                    infoAbilityEffectQ.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconQ.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostQ.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownQ.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
                else if (ability.Rank == 3)
                {
                    infoAbilityNameW.Text = "(W) " + ability.Name;
                    infoAbilityEffectW.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconW.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostW.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownW.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
                else if (ability.Rank == 4)
                {
                    infoAbilityNameE.Text = "(E) " + ability.Name;
                    infoAbilityEffectE.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconE.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostE.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownE.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
                else if (ability.Rank == 5)
                {
                    infoAbilityNameR.Text = "(R) " + ability.Name;
                    infoAbilityEffectR.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconR.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostR.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownR.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
            }

            gInfoInformation.Visible = gInfoAbilities.Visible = gInfoSkins.Visible = gInfoStatistics.Visible = true;
        }
        Image GetImageFile(string file)
        {
            if (File.Exists(file))
                return Image.FromFile(file);
            else return Image.FromFile(GhostBase.GhostbladeInstance.DataDragonInstance.ChampionsDirectory + @"\Unknown.png");

        }
        internal void LoadChampionInformation(string champName)
        {
            RiotDataStruct data = riotLib.GetChampionByName(champName);

            infoList.SelectedItem = data.DisplayName;

            // fill general data
            infoDisplayName.Text = data.DisplayName;
            infoTitle.Text = data.Title;
            infoTags.Text = data.Tags.Replace(",", ", ");
            infoDescription.Text = data.Description;
            infoTips.Text = data.Tips.Replace("*", "\r\n\r\n* ").Trim();
            infoOpponentTips.Text = data.OpponentTips.Replace("*", "\r\n\r\n* ").Trim();
            infoIcon.Image = GetImageFile(LoLAssetsPath + @"images\champions\" + data.IconPath);

            // populate stats table
            infoStats.AutoGenerateColumns = false;
            List<CompareStatsStruct> list = new List<CompareStatsStruct>();
            list.Add(new CompareStatsStruct()
            {
                Name = "Range",
                Base1 = data.Range.ToString("F1"),
                Level1 = "",
                Level18 = data.Range.ToString("F1")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Move Speed",
                Base1 = data.MoveSpeed.ToString("F1"),
                Level1 = "",
                Level18 = data.MoveSpeed.ToString("F1")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Attack",
                Base1 = data.AttackBase.ToString("F1"),
                Level1 = data.AttackLevel.ToString("F1"),
                Level18 = (data.AttackBase + (data.AttackLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Armor",
                Base1 = data.ArmorBase.ToString("F1"),
                Level1 = data.ArmorLevel.ToString("F1"),
                Level18 = (data.ArmorBase + (data.ArmorLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Magic Resist",
                Base1 = data.MagicResistBase.ToString("F1"),
                Level1 = data.MagicResistLevel.ToString("F1"),
                Level18 = (data.MagicResistBase + (data.MagicResistLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Health",
                Base1 = data.HealthBase.ToString("F1"),
                Level1 = data.HealthLevel.ToString("F1"),
                Level18 = (data.HealthBase + (data.HealthLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Health Regen",
                Base1 = data.HealthRegenBase.ToString("F1"),
                Level1 = data.HealthRegenLevel.ToString("F1"),
                Level18 = (data.HealthRegenBase + (data.HealthRegenLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Mana",
                Base1 = data.ManaBase.ToString("F1"),
                Level1 = data.ManaLevel.ToString("F1"),
                Level18 = (data.ManaBase + (data.ManaLevel * 18)).ToString("F")
            });
            list.Add(new CompareStatsStruct()
            {
                Name = "Mana Regen",
                Base1 = data.ManaRegenBase.ToString("F1"),
                Level1 = data.ManaRegenLevel.ToString("F1"),
                Level18 = (data.ManaRegenBase + (data.ManaRegenLevel * 18)).ToString("F")
            });

            infoStats.DataSource = list;

            // ratings
            infoRatingAttack.Value = (int)data.RatingAttack;
            infoRatingDefense.Value = (int)data.RatingDefense;
            infoRatingMagic.Value = (int)data.RatingMagic;
            infoRatingDifficulty.Value = (int)data.RatingDifficulty;

            // skins
            infoSkinsList.Items.Clear();
            foreach (RiotSkinsStruct skin in riotLib.Skins)
            {
                if (skin.ChampionId == data.Id)
                {
                    if (String.IsNullOrEmpty(skin.DisplayName))
                        infoSkinsList.Items.Add("Base");
                    else
                        infoSkinsList.Items.Add(skin.DisplayName);
                }
            }
            infoSkinsList.SelectedItem = "Base";
            LoadSkin();

            // abilities
            foreach (RiotAbilitiesStruct ability in riotLib.Abilities)
            {
                if (ability.ChampionId != data.Id)
                    continue;

                if (ability.Rank == 1)
                {
                    infoAbilityNameP.Text = "(Passive) " + ability.Name;
                    infoAbilityEffectP.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconP.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                }
                else if (ability.Rank == 2)
                {
                    infoAbilityNameQ.Text = "(Q) " + ability.Name;
                    infoAbilityEffectQ.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconQ.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostQ.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownQ.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
                else if (ability.Rank == 3)
                {
                    infoAbilityNameW.Text = "(W) " + ability.Name;
                    infoAbilityEffectW.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconW.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostW.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownW.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
                else if (ability.Rank == 4)
                {
                    infoAbilityNameE.Text = "(E) " + ability.Name;
                    infoAbilityEffectE.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconE.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostE.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownE.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
                else if (ability.Rank == 5)
                {
                    infoAbilityNameR.Text = "(R) " + ability.Name;
                    infoAbilityEffectR.Text = ability.Effect.Replace("<br>", "\n");
                    infoAbilityIconR.Image = GetImageFile(LoLAssetsPath + @"images\abilities\" + ability.IconPath);
                    infoAbilityCostR.Text = String.IsNullOrEmpty(ability.Cost) ? "" : "Cost: " + ability.Cost;
                    infoAbilityCooldownR.Text = String.IsNullOrEmpty(ability.Cooldown) ? "" : "Cooldown: " + ability.Cooldown;
                }
            }

            gInfoInformation.Visible = gInfoAbilities.Visible = gInfoSkins.Visible = gInfoStatistics.Visible = true;
        }
   private void LoadSkin()
        {
            // get skin's display name (if it's Base, than it's an empty string in DB)
            string skinDisplayName = infoSkinsList.SelectedItem.ToString() == "Base" ? "" : infoSkinsList.SelectedItem.ToString();

            RiotDataStruct data = riotLib.GetChampionByDisplayName(infoDisplayName.Text);

            foreach (RiotSkinsStruct skin in riotLib.Skins)
            {
                if (skin.DisplayName == skinDisplayName && skin.ChampionId == data.Id)
                {
                    infoPortrait.Image = GetImageFile(LoLAssetsPath + @"images\champions\" + skin.PortraitPath);
                    break;
                }
            }
        }

   private void infoSkinsList_SelectedIndexChanged(object sender, EventArgs e)
   {
       LoadSkin();
   }

    }
}
