using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GhostLib.Network
{
   public interface IProxy
    {
       IPAddress Host { get; set; }
       int Port { get; set; }
      

      IWebProxy GetWebProxy();



    }
}
