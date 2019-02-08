using ManagedUPnP;
using ManagedUPnP.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Ghostblade
{
    internal class HttpServerManager
    {
        public static ushort ListenPort { get; set; }
        public static IPEndPoint WanEndPoint { get; set; }
        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        public static string GetExternalIP(Services lsServices)
        {
            foreach (Service lsService in lsServices)
            {
                try
                {
                    // Check for action if required
                    ServiceDescription svcd = lsService.Description();
                    if (svcd.Actions.ContainsKey("GetExternalIPAddress"))
                    {
                        // Invoke the action
                        string lsIP;
                        lsService.InvokeAction<string>("GetExternalIPAddress", out lsIP);

                        return lsIP;
                    }
                }
                catch
                {
                }

            }

            return null;
        }
        public static bool ForwardPort(Services lsServices, int port)
        {


            foreach (Service lsService in lsServices)
            {
                try
                {

                    // Check for action if required
                    ServiceDescription svcd = lsService.Description();
                    if (svcd.Actions.ContainsKey("AddPortMapping"))
                    {


                        try
                        {
                            string local = LocalIPAddress();
                            // Add the port mapping
                            object[] loObj = new object[] { "", port, "TCP", port,local, true, "Ghostblade Stream Server", 0 };

                            lsService.InvokeAction("AddPortMapping", loObj);
                  
                            //// Remove the port mapping


                            //loObj = new object[] { "", 9068, "TCP" };
                            //lsService.InvokeAction("DeletePortMapping", loObj);
                            //Console.WriteLine("Removed");
                        }
                        catch (Exception loE)
                        {
                         
                        }
                        return true;
                    }
                }
                catch
                {

                }
             
            }
            return false;
        }
        public static bool RemovePort(Services lsServices, int port)
        {


            foreach (Service lsService in lsServices)
            {
                try
                {

                    // Check for action if required
                    ServiceDescription svcd = lsService.Description();
                    if (svcd.Actions.ContainsKey("DeletePortMapping"))
                    {


                        try
                        {
                            // Add the port mapping
                       object[]     loObj = new object[] { "", port, "TCP" };
                            lsService.InvokeAction("DeletePortMapping", loObj);
                       
                            //// Remove the port mapping


                            //loObj = new object[] { "", 9068, "TCP" };
                            //lsService.InvokeAction("DeletePortMapping", loObj);
                            //Console.WriteLine("Removed");
                        }
                        catch (Exception loE)
                        {
                      
                        }
                        return true;
                    }
                }
                catch
                {
                }

            }
            return false;
        }

        public static void ForwardPort()
        {
            try
            {
                bool lbCompleted;
                Services lsServices = Discovery.FindServices(
                   null,
                   2000, 0,
                   out lbCompleted,
                   AddressFamilyFlags.IPvBoth);
                string ip = GetExternalIP(lsServices);
                if (ip != null)
                    WanEndPoint = new IPEndPoint(IPAddress.Parse(ip), ListenPort);
                RemovePort(lsServices, ListenPort);
                ForwardPort(lsServices,ListenPort);
            }
            catch (Exception ex)
            {

            }
        }
    }
  
}
