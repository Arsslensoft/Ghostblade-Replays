using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TwitchStream
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if(arg.StartsWith("c="))
                    Application.Run(new Form1(arg.Remove(0,2)));
            }
        }
    }
}
