using GhostLib.Network.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace GhostLib.Network
{
    public class TransparentProxy : ProxyLogic
    {
        public TransparentProxy(HttpSocket clientSocket)
            : base(clientSocket) { }

        static new public TransparentProxy CreateProxy(HttpSocket clientSocket)
        {
            return new TransparentProxy(clientSocket);
        }

        protected override void OnReceiveRequest()
        {

        }

        protected override void OnReceiveResponse()
        {

        }
    }
    public  class NetworkInterfaceProxy : IProxy
    {
      IPAddress _ip;
      int _port;
      readonly TcpServer _server;
      public NetworkInterface Interface { get; set; }
      public int Port { get { return _port; } set { _port = value; } }
      public IPAddress Host { get { return _ip; } set { _ip = value; } }

      public IWebProxy GetWebProxy()
      {
          if (IsReady)
              return new WebProxy(Host.ToString(), Port);
          else return null;
      }


      public IPAddress ExternalIp { get; set; }
      public bool IsReady { get; set; }

      public bool IsRuning
      {
          get { return (_server == null) ? false : _server.IsListening; }

      }

   

      public bool Start()
      {
          _server.BindAddress = Host;
          _server.Start(TransparentProxy.CreateProxy);
          _server.BindAddress = Host;
          _server.InitListenFinished.WaitOne();
          if (_server.InitListenException != null)
              throw _server.InitListenException;
          return true;
      }
      public bool Stop()
      {
          if(_server != null)
          _server.Stop();
          return true;
      }


      private string GetExternalIP()
      {
          if (_port != -1)
          {
              Random rd = new Random();
              int port = rd.Next(9070, 9900);
              const bool bUseIPv6 = false;

              var Server = new TcpServer(port, bUseIPv6 );
              try
              {
                  Server.BindAddress = Host;
                  Server.Start(TransparentProxy.CreateProxy);
                  //Server.Start(RedirectingProxy.CreateProxy);
                  //Server.Start(RewritingProxy.CreateProxy);
                  Server.BindAddress = Host;
                  Server.InitListenFinished.WaitOne();
                  if (Server.InitListenException != null)
                      throw Server.InitListenException;


                  using (SmartWebClient wbc = new SmartWebClient(4000))
                  {
                      wbc.Proxy = new WebProxy(Host.ToString(), port);
                      string ip = wbc.DownloadString("https://api.ipify.org/?format=text");
                      Server.Stop();
                      return ip;
                  }

              }
              catch (Exception ex)
              {
                  if (ex.Message.ToLower().Contains("port"))
                      return GetExternalIP();
                  Server.Stop();
                  return null;
              }
          }
          else
          {
              try
              {
                  using (SmartWebClient wbc = new SmartWebClient(4000))
                  {
                   
                      string ip = wbc.DownloadString("https://api.ipify.org/?format=text");
                   
                      return ip;
                  }
              }
              catch
              {
                  return "0.0.0.0";
              }
          }

      }
    public int Index { get; set; }
      public NetworkInterfaceProxy(NetworkInterface nic, int port,int idx)
      {
          Index = idx;
          if (nic.OperationalStatus != OperationalStatus.Up)
              return;
          Interface = nic;
          IsReady = (nic.OperationalStatus == OperationalStatus.Up);

          foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
              Host = ip.Address;

          Port = port;

          _server = new TcpServer(Port, false);

          ExternalIp = IPAddress.Parse(GetExternalIP());
          
      }
      public NetworkInterfaceProxy()
      {
          Interface = null;
          Index = -1;
          Port = -1;
          ExternalIp = IPAddress.Parse(GetExternalIP());
          IsReady = false;
      }

    }
}
