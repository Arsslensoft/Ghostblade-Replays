using GBCrash;
using GhostLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace GhostReplays
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
     
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            SettingsManager.Init();
          //  //  [DISABLED] Console.Title = "Ghostblade Replay Server";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 6)
            {

                string GameId = args[1];
                string Region = args[2];
                string gpath = args[3];
                string lol = args[4];
                string rep = args[5];

                //     ReplayServer server = new ReplayServer(GameId, Region);
                GhostReplayServer sv = new GhostReplayServer(9068, GameId, Region, gpath, lol, rep);
                sv.listen();
            }
            else if (args.Length == 7)
            {

                string GameId = args[1];
                string Region = args[2];
                string gpath = args[3];
                string lol = args[4];
                string rep = args[5];
                string ext = args[6];
               
                if (int.TryParse(ext, out port))
                {
                    //     ReplayServer server = new ReplayServer(GameId, Region);


                    GhostReplayServer sv = new GhostReplayServer(port, GameId, Region, gpath, lol, rep);
                    sv.listen();
                   

                }
            }
            
        }
        static int port;
     //public  static INatDevice device;
        public static void SendCrashReport(Exception exception, string developerMessage ="")
        {
            try
            {
                var reportCrash = new ReportCrash
                {
                    AnalyzeWithDoctorDump = true,
                    DeveloperMessage = developerMessage + "\nUser = " + Environment.UserName + "@" + Environment.MachineName+"\nOS = "+Environment.OSVersion.ToString(),
                    ToEmail = "crash@ghostblade.tk",
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

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SendCrashReport((Exception)e.ExceptionObject);

        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            SendCrashReport(e.Exception);
        }
    }
}
