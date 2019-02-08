using GBCrash;
using GBReplay.Replays;
using GhostBase;
using GhostLib;
using GhostLib.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Ghostblade
{
    static public class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;
        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }
        static public void ShowFirstInstance()
        {
            WinApi.PostMessage(
                (IntPtr)WinApi.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }
        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }



    static public class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }


    static public class ProgramInfo
    {
        static public string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
        static public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
    }
    static class Program
    {
        internal static MainForm MainFormInstance { get; set; }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try{

                if (!SingleInstance.Start())
                {
                    File.WriteAllLines(Application.StartupPath + @"\ARGS.t", Environment.GetCommandLineArgs());
                    SingleInstance.ShowFirstInstance();
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                GhostBase.GhostbladeInstance.Init();
                RiotSharp.ApiCache.CacheEnabled = true;
                RiotSharp.ApiCache.Initialize();
                // Register App
                ApplicationInstance.Instance.Register("CID");
           
            if (!File.Exists(Application.StartupPath + @"\Config.txt"))
            {
                SettingsManager.Default();
         
                SettingsForm frm = new SettingsForm();
                frm.ShowDialog();
            }
            else
            {
                SettingsManager.Init();
                if (System.Windows.Forms.SystemInformation.Network)
                {
                    Thread thr = new Thread(new ThreadStart(CheckForUpdates));
                    thr.Start();
                    thr.Join();
                }
            }
            // Api cache
            RiotSharp.ApiCache.CacheEnabled = SettingsManager.Settings.ApiCacheEnabled;
       

 

       
         



                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                // Load Interfaces
             
                    MethodInvoker mi = new MethodInvoker(NetworkManager.Init);
                    mi.BeginInvoke(null, null);

                // GHOSTOVERLAY
                GhostOverlay.StartServer();

                MainFormInstance = new MainForm();
                GhostbladeInstance.MainForm = MainFormInstance;
                Application.Run(MainFormInstance);
        }
                        catch (Exception ex)
            {

                SendCrashReport(ex);
                Logger.Instance.Log.Fatal("Global fail", ex);
            }
            finally
            {
                if (RiotTool.pi != null)
                    RiotTool.pi.Dispose();

                if (SettingsManager.Settings.ProxyOption == ProxyType.Network &&   NetworkManager.DefaultProxy != null)
                    ((NetworkInterfaceProxy)NetworkManager.DefaultProxy).Stop();

                // Api cache
                RiotSharp.ApiCache.Close
                                      ();
                GhostOverlay.Stop();
                StreamManager.Stop();
                SingleInstance.Stop();
               
       
            }
        }
        static void LoadReplay(string[] args)
        {

            if (args.Length == 2)
            {
                GhostReplay rep = new GhostReplay(args[1], SettingsManager.Settings.GameVersion, false, Application.StartupPath + @"\Temp");
                rep.ReadReplay(ReadMode.AllExceptData);
                foreach (Process proc in Process.GetProcessesByName("GhostReplay"))
                    proc.Kill();
                foreach (Process proc in Process.GetProcessesByName("League of Legends"))
                    proc.Kill();


                if (rep.Platform != "PBE1")
                {

                    if (SettingsManager.Settings.GameVersion != rep.GameVersion)
                    {
                        Version rv = new Version(rep.GameVersion);
                        Version gv = new Version(SettingsManager.Settings.GameVersion);
                        if (gv.Major != rv.Major || rv.Minor != gv.Minor)
                        {
                            if (MessageBox.Show("The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.GameVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                                return;
                        }
                        else
                        {
                            if (MessageBox.Show("The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.GameVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                return;
                        }
                    }
                }
                else if (SettingsManager.Settings.HasPBE)
                {
                    if (SettingsManager.Settings.PbeVersion != rep.GameVersion)
                    {
                        Version rv = new Version(rep.GameVersion);
                        Version gv = new Version(SettingsManager.Settings.PbeVersion);
                        if (gv.Major != rv.Major || rv.Minor != gv.Minor)
                        {
                            if (MessageBox.Show("The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.PbeVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                                return;
                        }
                        else
                        {
                            if (MessageBox.Show("The replay was recorded on a different League of Legends version \nGame Version : " + SettingsManager.Settings.PbeVersion + "\nReplay Version : " + rep.GameVersion + "\nThe replay may not work correctly, Do you want to play it ? ", "Version Mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                return;
                        }
                    }
                }
                if (String.IsNullOrEmpty(SettingsManager.Settings.GameDirectory))
                {
                    MessageBox.Show("You need to set your League of Legends installation location in settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProcessStartInfo Replay = new ProcessStartInfo();
                Replay.FileName = Application.StartupPath + @"\GhostReplay.exe";




                if (SettingsManager.Settings.HasPBE && (rep.Platform == "PBE1"))
                    Replay.Arguments = rep.MetaData.gameKey.gameId.ToString() + " " + rep.MetaData.gameKey.platformId + " \"" + RiotTool.Instance.InstalledPbeProfile.GameDirectory + "\"" + " \"" + RiotTool.Instance.InstalledPbeProfile.ReleaseDeploy + "\" \"" + rep.FileName + "\"";
                else
                    Replay.Arguments = rep.MetaData.gameKey.gameId.ToString() + " " + rep.MetaData.gameKey.platformId + " \"" + RiotTool.Instance.InstalledGameProfile.GameDirectory + "\"" + " \"" + RiotTool.Instance.InstalledGameProfile.ReleaseDeploy + "\" \"" + rep.FileName + "\"";

                Process.Start(Replay);
           
            }
        }
        public static void ParseArgs(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[1] != "-restart" && args[1] != "patcher")
                {
                    LoadReplay(args);

                    return;
                }

            }
        }
        static void CheckForUpdates()
        {
            string v = SettingsManager.Settings.DragonVersion;
                GhostbladeInstance.DoUpdate(ref v);
                SettingsManager.Settings.DragonVersion = v;
                SettingsManager.Save();
           
        }

        public static void SendCrashReport(Exception exception, string developerMessage = "Error")
        {
            try
            {
                var reportCrash = new ReportCrash
                {
                    AnalyzeWithDoctorDump = true,

                    DeveloperMessage = File.ReadAllText(Application.StartupPath + @"\" + Logger.Instance.LogFileName),
                    ToEmail = "crash@ghostblade.gq",
                    DoctorDumpSettings = new DoctorDumpSettings
                    {
                        ApplicationID = new Guid("c037d9f5-9e78-41be-8931-64212ff14b83"),
                        OpenReportInBrowser = true
                    },
                };

                reportCrash.Send(exception);
            }
            catch
            {

            }
        }
        //public static bool CheckForUpdate(string server, string application, long versionint)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server + "/update.php?status=check&version=" + versionint.ToString());
        //        request.Method = "POST";
        //        request.Accept = "gzip, deflate";
        //        request.Timeout = 10000;
        //        request.KeepAlive = true;
        //        request.UserAgent = "Arsslensoft/UserAgent";
        //        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


        //        request.Proxy = GlobalProxySelection.GetEmptyWebProxy();
        //        // Get the response.
        //        WebResponse response = request.GetResponse();
        //        Stream dataStream = response.GetResponseStream();

        //        StreamReader reader = new StreamReader(dataStream);

        //        string responseFromServer = reader.ReadToEnd();
        //        if (responseFromServer.Contains("AVAILABLE_"))
        //        {

        //            reader.Close();
        //            dataStream.Close();
        //            response.Close();
        //            return true;

        //        }
        //        reader.Close();
        //        dataStream.Close();
        //        response.Close();

        //        return false;
        //    }
        //    catch
        //    {

        //    }
        //    return false;
        //}

        //static void CheckForUpdates()
        //{
        //    try
        //    {
        //        if (DataDragon.CheckUpdateDDragon(SettingsManager.Settings.ApiKey, SettingsManager.Settings.Region, File.ReadAllText(Application.StartupPath + @"\DD.dat")))
        //        {
        //            // Download
        //            DataDragon.UpdateStaticData(SettingsManager.Settings.ApiKey, SettingsManager.Settings.Region);
        //        }
        //        //if (CheckForUpdate("https://update.ghostblade.tk/", "GHOSTBLADE", VersionInt))
        //        //{
        //        //    if (MessageBox.Show("Update was found \n Do you want to update Ghostblade ? \nNote : Update process will close current Ghostblade instance", "Ghostblade Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
        //        //    {
        //        //        Process.Start(Application.StartupPath + @"\GhostbladeUpdater.exe");
        //        //        Application.Exit();
        //        //    }
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Instance.Log.Error("Failed to check for update", ex);
        //    }

        //}
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //         SendCrashReport((Exception)e.ExceptionObject);



            Logger.Instance.Log.Fatal("Fail", (e.ExceptionObject as Exception));

        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            SendCrashReport(e.Exception);
        }
    }
}
