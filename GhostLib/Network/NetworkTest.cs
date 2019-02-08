using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GhostLib.Network
{
   public static  class NetworkTest
    {
       public static bool PingTest(IPAddress ip)
       {
           System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

           System.Net.NetworkInformation.PingReply pingStatus =
               ping.Send(ip, 1000);

           if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       public static bool DnsTest(out IPAddress ip)
       {
           try
           {
               System.Net.IPHostEntry ipHe =
                   System.Net.Dns.GetHostByName("www.google.com");
               ip = ipHe.AddressList[0];
               return true;
           }
           catch
           {
               ip = IPAddress.Any;
               return false;
           }
       }
       public static bool TcpSocketTest()
       {
           try
           {
               System.Net.Sockets.TcpClient client =
                   new System.Net.Sockets.TcpClient("www.google.com", 80);
               client.Close();
               return true;
           }
           catch (System.Exception ex)
           {
               return false;
           }
       }
       public static bool WebRequestTest()
       {
           string url = "http://www.google.com";
           try
           {
               System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
               System.Net.WebResponse myResponse = myRequest.GetResponse();
           }
           catch (System.Net.WebException)
           {
               return false;
           }
           return true;
       }

       [DllImport("wininet.dll")]
       private extern static bool InternetGetConnectedState(out int connDescription, int ReservedValue);

       //check if a connection to the Internet can be established 
       public static bool IsConnectionAvailable()
       {
           int Desc;
           return InternetGetConnectedState(out Desc, 0);
       }

       public static bool IPV6Test()
       {
           return System.Net.Sockets.Socket.OSSupportsIPv6; 
       }

       public static bool Tracert(out string trace, string region)
       {

           IcmpRequest myRequest = new IcmpRequest(new ToolsCommandRequest(RiotTool.PingServers[region.ToUpper()],
                                "Hello", CommandType.tracert, 4, 32, 1000, 1000, 128));
           trace = myRequest.Cns.ToString();
           return true;

       }

       public static bool GeneralTest(out string rep)
       {
           try
           {
               System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
               proc.FileName = "cmd.exe";
               proc.Arguments = "/c "+"(ipconfig /all & ping www.google.com & netsh firewall show config & netsh interface ipv4 show subinterfaces & netsh interface ipv4 show ipstats) > " +"\"" + Application.StartupPath + @"\Temp\NetInfo.txt" + "\"";
             Process p =  System.Diagnostics.Process.Start(proc);
             p.WaitForExit();

             rep = File.ReadAllText(Application.StartupPath + @"\Temp\NetInfo.txt");
             return true;
              

           }
           catch
           {
               rep = "";
               return false;
           }
       }

      
    }
}
