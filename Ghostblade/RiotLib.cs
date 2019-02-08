using Ghostblade.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Ghostblade
{


    public class CompareStatsStruct
    {
        public string Name { set; get; }
        public string Base1 { set; get; }
        public string Level1 { set; get; }
        public string Diff { set; get; }
        public string Base2 { set; get; }
        public string Level2 { set; get; }
        public string Level18 { set; get; }
    }
    public class OverviewStruct
    {
        public string Name;
    }
    #region Structs
    public struct RiotDataStruct
    {
        public Int64 Id;
        public string Name, DisplayName, Title, IconPath, PortraitPath, SplashPath, Tags, Description, Tips, OpponentTips;
        public double Range, MoveSpeed, ArmorBase, ArmorLevel, ManaBase, ManaLevel, CriticalChanceBase, CriticalChanceLevel, ManaRegenBase, ManaRegenLevel, HealthRegenBase, HealthRegenLevel,
            MagicResistBase, MagicResistLevel, HealthBase, HealthLevel, AttackBase, AttackLevel;
        public Int64 RatingDefense, RatingMagic, RatingDifficulty, RatingAttack;
    }

    public struct RiotSkinsStruct
    {
        public Int64 Id;
        public Int64 IsBase, ChampionId;
        public string Name, DisplayName, PortraitPath;
    }

    public struct RiotAbilitiesStruct
    {
        public Int64 Id, ChampionId, Rank;
        public string Name, Cost, Cooldown, IconPath, Effect, Hotkey;
    }

    public class ChampListStruct
    {
        public string Name, DisplayName, Tag;
    }


    public class ChampCountersListStruct
    {
        public string Name, DisplayName;
        public int Score;
    }
    #endregion

    /// <summary>
    /// Class for accessing Riot LOL database
    /// </summary>
   internal class RiotLib
    {
     
        public List<RiotDataStruct> Data = new List<RiotDataStruct>();
        public List<RiotSkinsStruct> Skins = new List<RiotSkinsStruct>();
        public List<RiotAbilitiesStruct> Abilities = new List<RiotAbilitiesStruct>();

        /// <summary>
        /// Full path of SQLite file
        /// </summary>
        public string DbLocation = String.Empty;


        /// <summary>
        /// Gets struct of champion by its display name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RiotDataStruct GetChampionByDisplayName(string name)
        {
            foreach (RiotDataStruct s in Data)
                if (s.DisplayName == name)
                    return s;

            return Data[0];
        }
        /// <summary>
        /// Gets struct of champion by its name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RiotDataStruct GetChampionById(int id)
        {
            foreach (RiotDataStruct s in Data)
                if (s.Id == id)
                    return s;

            return Data[0];
        }
        /// <summary>
        /// Gets struct of champion by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RiotDataStruct GetChampionByName(string name)
        {
            foreach (RiotDataStruct s in Data)
                if (s.Name == name)
                    return s;

            return Data[0];
        }

        /// <summary>
        /// Load champions data (from "champions" table)
        /// </summary>
        public void GetChampionsData()
        {
            if (!File.Exists(DbLocation))
                return;

	        using (SQLiteConnection conn = new SQLiteConnection(@"data source=""" + DbLocation + @""""))
	        {
	            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM champions", conn);
	            SQLiteDataReader rdr = null;
	            conn.Open();
	            try
	            {
                    RiotDataStruct dataStruct;

	                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
	                while (rdr.Read())
	                {   
                        dataStruct.Id = (Int64)rdr["id"];

                        dataStruct.Name = (string)rdr["name"];
                        dataStruct.DisplayName = (string)rdr["displayName"];
                        dataStruct.Title = (string)rdr["title"];
                        dataStruct.IconPath = (string)rdr["iconPath"];
                        dataStruct.PortraitPath = (string)rdr["portraitPath"];
                        dataStruct.SplashPath = (string)rdr["splashPath"];
                        dataStruct.Tags = (string)rdr["tags"];
                        dataStruct.Description = (string)rdr["description"];
                        dataStruct.Tips = (string)rdr["tips"];
                        dataStruct.OpponentTips = (string)rdr["opponentTips"];

                        dataStruct.Range = float.Parse(rdr["range"].ToString());
                        dataStruct.MoveSpeed = float.Parse(rdr["MoveSpeed"].ToString());
                        dataStruct.ArmorBase = float.Parse(rdr["ArmorBase"].ToString());
                        dataStruct.ArmorLevel = float.Parse(rdr["ArmorLevel"].ToString());
                        dataStruct.ManaBase = float.Parse(rdr["ManaBase"].ToString());
                        dataStruct.ManaLevel = float.Parse(rdr["ManaLevel"].ToString());
                        dataStruct.CriticalChanceBase = float.Parse(rdr["CriticalChanceBase"].ToString());
                        dataStruct.CriticalChanceLevel = float.Parse(rdr["CriticalChanceLevel"].ToString());
                        dataStruct.ManaRegenBase = float.Parse(rdr["ManaRegenBase"].ToString());
                        dataStruct.ManaRegenLevel = float.Parse(rdr["ManaRegenLevel"].ToString());
                        dataStruct.HealthRegenBase = float.Parse(rdr["HealthRegenBase"].ToString());
                        dataStruct.HealthRegenLevel = float.Parse(rdr["HealthRegenLevel"].ToString());
                        dataStruct.MagicResistBase = float.Parse(rdr["MagicResistBase"].ToString());
                        dataStruct.MagicResistLevel = float.Parse(rdr["MagicResistLevel"].ToString());
                        dataStruct.HealthBase = float.Parse(rdr["HealthBase"].ToString());
                        dataStruct.HealthLevel = float.Parse(rdr["HealthLevel"].ToString());
                        dataStruct.AttackBase = float.Parse(rdr["AttackBase"].ToString());
                        dataStruct.AttackLevel = float.Parse(rdr["AttackLevel"].ToString());

                        dataStruct.RatingDefense = (Int64)rdr["RatingDefense"];
                        dataStruct.RatingMagic = (Int64)rdr["RatingMagic"];
                        dataStruct.RatingDifficulty = (Int64)rdr["RatingDifficulty"];
                        dataStruct.RatingAttack = (Int64)rdr["RatingAttack"];

                        Data.Add(dataStruct);
	                }
	            }
	            finally
	            {
	                rdr.Close();
	            }
	        }
        }


        /// <summary>
        /// Load skins data (from "championSkins" table)
        /// </summary>
        public void GetSkinsData()
        {
            if (!File.Exists(DbLocation))
                return;

            using (SQLiteConnection conn = new SQLiteConnection(@"data source=""" + DbLocation + @""""))
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM championSkins", conn);
                SQLiteDataReader rdr = null;
                conn.Open();
                try
                {
                    RiotSkinsStruct dataStruct;

                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (rdr.Read())
                    {
                        dataStruct.Id = (Int64)rdr["id"];
                        dataStruct.IsBase = (Int64)rdr["isBase"];
                        dataStruct.ChampionId = (Int64)rdr["championId"];

                        dataStruct.Name = (string)rdr["name"];
                        dataStruct.DisplayName = (string)rdr["displayName"];
                        dataStruct.PortraitPath = (string)rdr["portraitPath"];

                        Skins.Add(dataStruct);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
        }


        /// <summary>
        /// Load abilities data (from "championSkins" table)
        /// </summary>
        public void GetAbilitiesData()
        {
            if (!File.Exists(DbLocation))
                return;

            using (SQLiteConnection conn = new SQLiteConnection(@"data source=""" + DbLocation + @""""))
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM championAbilities", conn);
                SQLiteDataReader rdr = null;
                conn.Open();
                try
                {
                    RiotAbilitiesStruct dataStruct;

                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (rdr.Read())
                    {
                        dataStruct.Id = (Int64)rdr["id"];
                        dataStruct.Rank = (Int64)rdr["rank"];
                        dataStruct.ChampionId = (Int64)rdr["championId"];

                        dataStruct.Name = (string)rdr["name"];
                        dataStruct.Cost = (rdr["cost"] is DBNull) ? String.Empty : (string)rdr["cost"];
                        dataStruct.Cooldown = (rdr["cooldown"] is DBNull) ? String.Empty : (string)rdr["cooldown"];
                        dataStruct.IconPath = (string)rdr["iconPath"];
                        dataStruct.Effect = (string)rdr["description"];
                        // todo: consider:
                        // dataStruct.Effect = (rdr["effect"] is DBNull) ? (string)rdr["description"] : (string)rdr["effect"];
                        dataStruct.Hotkey = (rdr["hotkey"] is DBNull) ? String.Empty : (string)rdr["hotkey"];

                        Abilities.Add(dataStruct);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
        }
    }
}
