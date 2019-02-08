﻿using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GBCrash
{
    /// <summary>
    /// Set SMTP server details and receiver email fields of this class instance to send crash reports directly in your inbox.
    /// </summary>
    public class ReportCrash
    {
        /// <summary>
        /// Gets or Sets name or IP address of the Host used for SMTP transactions.
        /// </summary>
        public String SmtpHost;

        /// <summary>
        /// Specify whether the SMTP client uses the Secure Socket Layer (SSL) to encrypt the connection.
        /// </summary>
        public Boolean EnableSSL;

        /// <summary>
        /// Gets or Sets the port used for SMTP transactions.
        /// </summary>
        public int Port = 25;

        /// <summary>
        /// Gets or Sets the username used for SMTP transactions.
        /// </summary>
        public String UserName = "";

        /// <summary>
        /// Gets or Sets the password used for SMTP transactions. 
        /// </summary>
        public String Password = "";

        /// <summary>
        /// Gets or Sets email address where you want to receive crash reports.
        /// </summary>
        public String ToEmail;

        /// <summary>
        /// Gets or Sets email address used by crash reporter if user don't provide her email address.
        /// </summary>
        public String FromEmail;

        /// <summary>
        /// Gets or Sets exception that occur during application execution.
        /// </summary>
        public Exception Exception;

        /// <summary>
        /// Specify whether CrashReporter.NET should take screen shot of whole screen or not.
        /// </summary>
        public bool CaptureScreen = false;

        /// <summary>
        /// Gets or Sets custom message developer wants to send. It can be something like value of variables or other details you want to send.
        /// </summary>
        public String DeveloperMessage = "";

        /// <summary>
        ///  Gets or Sets if email is required to send the crash report.
        /// </summary>
        public bool EmailRequired = false;

        /// <summary>
        /// Gets or Sets "Include screenshot" start value.
        /// </summary>
        public bool IncludeScreenshot = true;

        /// <summary>
        /// Gets or Sets the current culture to use by the library.
        /// </summary>
        public CultureInfo CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        
        /// <summary>
        /// Specify whether CrashReporter.NET should send crash reports only for new problems (duplicates detected by Doctor Dump free cloud service).
        /// </summary>
        public bool AnalyzeWithDoctorDump = true;

        /// <summary>
        /// Specify Doctor Dump processing settings. Used only when AnalyzeWithDoctorDump is true.
        /// </summary>
        public DoctorDumpSettings DoctorDumpSettings = new DoctorDumpSettings();

        internal string ApplicationTitle;

        internal string ApplicationVersion;

        internal string ScreenShot;

        /// <summary>
        /// Sends exception report directly to receiver email address provided in ToEmail.
        /// </summary>
        public void Send()
        {
            if (Exception != null)
            {
                Send(Exception);
            }
        }

        /// <summary>
        /// Sends exception report directly to receiver email address provided in ToEmail.
        /// </summary>
        /// <param name="exception">Exception object that contains details of the exception.</param>
        public void Send(Exception exception)
        {
            Exception = exception;

            var mainAssembly = Assembly.GetEntryAssembly();
            string appTitle = null;
            var attributes = mainAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), true);
            if (attributes.Length > 0)
            {
                appTitle = ((AssemblyTitleAttribute) attributes[0]).Title;
            }
            ApplicationTitle = !string.IsNullOrEmpty(appTitle) ? appTitle : mainAssembly.GetName().Name;
            ApplicationVersion = mainAssembly.GetName().Version.ToString();

            try
            {
                ScreenShot = string.Format(@"{0}\{1} Crash Screenshot.png", Path.GetTempPath(),
                                           ApplicationTitle);
                if (CaptureScreen)
                    CaptureScreenshot.CaptureScreen(ScreenShot, ImageFormat.Png);
                else
                    CaptureScreenshot.CaptureActiveWindow(ScreenShot, ImageFormat.Png);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            if (String.IsNullOrEmpty(ToEmail) || !AnalyzeWithDoctorDump && (String.IsNullOrEmpty(FromEmail) || String.IsNullOrEmpty(SmtpHost)))
                return;

            var parameterizedThreadStart = new ParameterizedThreadStart(ShowUI);
            var thread = new Thread(parameterizedThreadStart) {IsBackground = false};
            thread.CurrentCulture = thread.CurrentUICulture = CurrentCulture ?? System.Windows.Forms.Application.CurrentCulture;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(this);
            thread.Join();
        }

        private static void ShowUI(object reportCrash)
        {
            var crashReport = new CrashReport((ReportCrash) reportCrash);
            crashReport.ShowDialog();
        }
    }
    
    /// <summary>
    /// Set Doctor Dump processing settings.
    /// </summary>
    public class DoctorDumpSettings
    {
        /// <summary>
        /// Gets or Sets application ID.
        /// </summary>
        public Guid? ApplicationID;

        /// <summary>
        /// Specify whether CrashReporter.NET should send anonymous crash report to Doctor Dump that doesn't contain private information.
        /// Only about 1/10 of users press "Send" button on crash reporting dialogs. And even less if there are required fields to fill.
        /// Without sending anonymous reports most of the problems are hidden from the developer.
        /// </summary>
        public bool SendAnonymousReportSilently = true;

        /// <summary>
        /// Specify whether CrashReporter.NET should open the web page in browser about crash report that contains report ID and may contain steps to fix the problem.
        /// </summary>
        public bool OpenReportInBrowser = true;
    }
}