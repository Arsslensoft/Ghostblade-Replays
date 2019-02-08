using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GhostBase
{
    public class GhostbladeInstance
    {
        public static object MainForm { get; set; }
        public static RiotSharp.RiotApi Api { get; set; }
        public static DataDragonUpdater DataDragonInstance { get; set; }
        //TODO:Add Init with setting reg and api
        public static void Init()
        {
            // INSTALL CERTS
            SecurityUtils.TrustRoot(global::GhostBase.Properties.Resources.Root);
            SecurityUtils.TrustRoot(global::GhostBase.Properties.Resources.gbca);
            SecurityUtils.TrustRoot(global::GhostBase.Properties.Resources.ASPRCA);


            SecurityUtils.TrustIntermediate(global::GhostBase.Properties.Resources.CSCA);
            SecurityUtils.TrustIntermediate(global::GhostBase.Properties.Resources.CS);

            SecurityUtils.TrustIntermediate(global::GhostBase.Properties.Resources.EVCA);
            // Create Directories
            CheckDir(Application.StartupPath + @"\Champions");
            CheckDir(Application.StartupPath + @"\Data");
            CheckDir(Application.StartupPath + @"\Icons");
            CheckDir(Application.StartupPath + @"\Items");
            CheckDir(Application.StartupPath + @"\Rank");
            CheckDir(Application.StartupPath + @"\Spells");
            CheckDir(Application.StartupPath + @"\Temp");
            CheckDir(Application.StartupPath + @"\Logs");

            // Create Base Files
            CheckFile(Application.StartupPath + @"\TRANS.ttf", GhostBase.Properties.Resources.TRANS);
            CheckFile(Application.StartupPath + @"\Champions\Unknown.png", GhostBase.Properties.Resources.Unknown);
            CheckFile(Application.StartupPath + @"\Data\Cache.dat", GhostBase.Properties.Resources.Cache);

            DataDragonInstance = new DataDragonUpdater("a912bd06-0928-470e-8ff6-debaa96fd0a9", RiotSharp.Region.euw);
            Api = RiotSharp.RiotApi.GetInstance("a912bd06-0928-470e-8ff6-debaa96fd0a9");
        }
        public static void CheckFile(string file, byte[] data)
        {
            if (!File.Exists(file))
                File.WriteAllBytes(file, data);
        }
        public static void CheckFile(string file, Bitmap data)
        {
            if (!File.Exists(file))
                data.Save(file, System.Drawing.Imaging.ImageFormat.Png);
        }
        static bool ForceUpdate = false;
      public  static void CheckDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                ForceUpdate = true;
                Directory.CreateDirectory(dir);
               

            }
        }
        public static void DoUpdate(ref string ver)
        {
            try
            {
                // INSTALL CERTS
                SecurityUtils.TrustRoot(global::GhostBase.Properties.Resources.Root);
                SecurityUtils.TrustRoot(global::GhostBase.Properties.Resources.gbca);
                SecurityUtils.TrustRoot(global::GhostBase.Properties.Resources.ASPRCA);


                SecurityUtils.TrustIntermediate(global::GhostBase.Properties.Resources.CSCA);
                SecurityUtils.TrustIntermediate(global::GhostBase.Properties.Resources.CS);

                SecurityUtils.TrustIntermediate(global::GhostBase.Properties.Resources.EVCA);
                // Create Directories
                CheckDir(Application.StartupPath + @"\Champions");
                CheckDir(Application.StartupPath + @"\Data");
                CheckDir(Application.StartupPath + @"\Icons");
                CheckDir(Application.StartupPath + @"\Items");
                CheckDir(Application.StartupPath + @"\Rank");
                CheckDir(Application.StartupPath + @"\Spells");
                CheckDir(Application.StartupPath + @"\Temp");
                CheckDir(Application.StartupPath + @"\Logs");
               
                // Create Base Files
                CheckFile(Application.StartupPath + @"\TRANS.ttf", GhostBase.Properties.Resources.TRANS);
                CheckFile(Application.StartupPath + @"\Champions\Unknown.png", GhostBase.Properties.Resources.Unknown);
                CheckFile(Application.StartupPath + @"\Data\Cache.dat", GhostBase.Properties.Resources.Cache);


                if (System.Windows.Forms.SystemInformation.Network && ( !DataDragonInstance.IsUpToDate(ver) || ForceUpdate))
                {
                    UpdaterForm updfrm = new UpdaterForm();
                    updfrm.version = ver;
                    updfrm.ShowDialog();
                    ver = DataDragonInstance.DataDragonRealm.V;

                }
            }
            catch(Exception ex)

            {
                BaseInstance.Log("Failed to update", ex);
            }
            
        }
    }
}
