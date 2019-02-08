using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GhostLib.Network
{
    public class SmartWebClient : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public IWebProxy GetDefaulProxy()
        {
            if (NetworkManager.DefaultProxy == null)
                return null;
            else return NetworkManager.DefaultProxy.GetWebProxy();
        }

        public SmartWebClient() : this(60000) { }

        public SmartWebClient(int timeout)
        {

            this.Timeout = timeout;
            this.Proxy = null;
            ServicePointManager.DefaultConnectionLimit = 6;
            ServicePointManager.Expect100Continue = false;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
             //   this.Proxy = GetDefaulProxy();
                request.Timeout = this.Timeout;

            }
            return request;
        }


    }
}
