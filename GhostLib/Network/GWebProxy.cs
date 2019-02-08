using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GhostLib.Network
{
   public class GWebProxy : IProxy
    {
        IPAddress _ip;
        int _port;
        string _username;
        string _pass;
       string _host;

        public int Port { get { return _port; } set { _port = value; } }
        public IPAddress Host { get { return _ip; } set { _ip = value; } }

        public string PHost { get { return _host; } set { _host = value; } }


        public IWebProxy GetWebProxy()
        {
            WebProxy wbp = new WebProxy(PHost, Port);
            if (!string.IsNullOrEmpty(_username))
                wbp.Credentials = new NetworkCredential(_username, _pass);
            else
            {
                wbp.UseDefaultCredentials = true;
                wbp.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            return wbp;

        }

        public GWebProxy()
        {
            _pass = SettingsManager.Settings.ProxyPass;
            _username = SettingsManager.Settings.ProxyUser;
            _port = SettingsManager.Settings.ProxyPort;
            _ip = IPAddress.Any;
            _host = SettingsManager.Settings.ProxyHost;
        }
    }
}
