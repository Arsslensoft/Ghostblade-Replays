using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Globalization;

namespace RiotSharp
{
 
    public static class ApiCache
    {
        public static bool CacheEnabled = true;
        internal static SQLiteConnection DTB;
        internal static string BuildConString(string dbfile, int cachesize, int version, int maxpagecount, int pagesize, bool pooling, bool Sync)
        {
            SQLiteConnectionStringBuilder wdbcons = new SQLiteConnectionStringBuilder();
            wdbcons.CacheSize = cachesize;
            wdbcons.DataSource = dbfile;
            wdbcons.Version = version;
         //   wdbcons.MaxPageCount = maxpagecount;
            wdbcons.PageSize = pagesize;
            wdbcons.Pooling = pooling;
            wdbcons.JournalMode = SQLiteJournalModeEnum.Off;
            if (Sync)
            {
                wdbcons.SyncMode = SynchronizationModes.Full;
            }
            else
            {
                wdbcons.SyncMode = SynchronizationModes.Off;
            }
            wdbcons.FailIfMissing = true;
            return wdbcons.ConnectionString;
        }
        public static void Initialize()
        {
            try
            {
                DTB = new SQLiteConnection(BuildConString(Application.StartupPath + @"\Data\Cache.dat", 8192, 3, 8192, 8192, false, false));
                DTB.Open();
            }
            catch
            {

            }
        }
        public static void Close()
        {
            try
            {
                DTB.Close();
            }
            catch
            {

            }
        }
        static string Normalize(string text, bool reverse)
        {
            if (reverse)
                return text.Replace("<arsslensoft-quote>", "'");
            else
                return text.Replace("'", "<arsslensoft-quote>");
        }
        public static void AddSummoner(SummonerEndpoint.Summoner sum,string json, Region reg)
        {
            try
            {
                using (SQLiteTransaction trans = DTB.BeginTransaction())
                {
                   
                    using (SQLiteCommand addKeysCmd = new SQLiteCommand(DTB))
                    {
                        string sqlIns = "INSERT INTO Summoners (id, name, region, json) VALUES(" + sum.Id + ", '" + Normalize(sum.Name, false) + "', '" + reg.ToString() + "', '" + Normalize(json, false) + "');";
                        addKeysCmd.CommandText = sqlIns;
                        addKeysCmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
            }
            catch
            {

            }
        }
        public static string GetSummoner(string name, Region reg)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(DTB))
            {
                cmd.CommandText = "SELECT json FROM Summoners WHERE name = '" +Normalize(name,false) + "' AND region='"+reg.ToString()+"';";
               object o = cmd.ExecuteScalar();

               if (o != null)
                   return Normalize((string)o, true);
               else return null;



            }
        }
        public static string GetSummoner(long id, Region reg)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(DTB))
            {
                cmd.CommandText = "SELECT json FROM Summoners WHERE id = " + id.ToString()+ " AND region='" + reg.ToString() + "';";
                object o = cmd.ExecuteScalar();

                if (o != null)
                    return Normalize((string)o, true);
                else return null;



            }
        }
         public static void ClearSummoners()
        {
            try
            {
                using (SQLiteTransaction trans = DTB.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(DTB))
                    {
                        cmd.CommandText = "DELETE FROM Summoners;";
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
            }
            catch
            {

            }
        }

         public static bool IsExpired(string type, DateTime dt)
         {
             if (type == "RS"  || type == "CS")
                 return (DateTime.Now.Subtract(dt).TotalDays >= 1);
             else return (DateTime.Now.Subtract(dt).TotalDays >= 2);
         }
        public static void AddCache(long sid, string region, string json, string type)
        {
            try
            {
                bool update = false;
                using (SQLiteCommand cmd = new SQLiteCommand(DTB))
                {
                    cmd.CommandText = "SELECT json FROM cached WHERE sid = " + sid.ToString() + " AND region='" + region + "' AND type='" + type + "';";
                 
                    update = (cmd.ExecuteScalar() != null);

                }

                if (update)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(DTB))
                    {
                        cmd.CommandText = "DELETE FROM cached WHERE sid = " + sid.ToString() + " AND region='" + region + "' AND type='" + type + "';";
                        cmd.ExecuteNonQuery();

                    }
                }

                    using (SQLiteTransaction trans = DTB.BeginTransaction())
                    {

                        using (SQLiteCommand addKeysCmd = new SQLiteCommand(DTB))
                        {
                            string sqlIns = "INSERT INTO cached (sid, type, region,json,date) VALUES(" + sid.ToString() + ", '" + type + "', '" + region + "','" + Normalize(json, false) + "', '" + DateTime.Now.ToString() + "');";
                            addKeysCmd.CommandText = sqlIns;
                            addKeysCmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                
            }
            catch
            {

            }
        }
        public static List<long> ForceDodge = new List<long>();
        public static string GetCache(long sid, Region reg,string type)
        {
            string result = null;
            if (ForceDodge.Contains(sid))
                return null;

            using (SQLiteCommand cmd = new SQLiteCommand(DTB))
            {
                cmd.CommandText = "SELECT json,date FROM cached WHERE sid = " + sid.ToString() + " AND region='" + reg.ToString() + "' AND type='"+type+"';";
                object o1 = cmd.ExecuteScalar();
         using (SQLiteDataReader o = cmd.ExecuteReader())
                {
                    while (o.Read())
                    {
                        if (!IsExpired(type, DateTime.Parse((string)o["date"])))
                            result = Normalize((string)o["json"], true);

                    }
                }

            }

                return result;
        }
        public static void ClearCache()
        {
            try
            {
                using (SQLiteTransaction trans = DTB.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(DTB))
                    {
                        cmd.CommandText = "DELETE FROM cached;";
                        cmd.ExecuteNonQuery();
                  
                    }
                    trans.Commit();
                }
            }
            catch
            {

            }
        }

   
    }
}
