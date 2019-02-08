using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace GhostLib.Network
{
   public static class NetworkManager
    {
       public static IProxy DefaultProxy { get; set; }

       public static void Init()
       {
           if (DefaultProxy != null)
               ((NetworkInterfaceProxy)DefaultProxy).Stop();
           if (SettingsManager.Settings.ProxyOption == ProxyType.Network )
           {
               if (SettingsManager.Settings.NetworkInterface == "default")
               {
                  DefaultProxy = new NetworkInterfaceProxy();
                   return;
               }
               NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
               NetworkChange.NetworkAvailabilityChanged+= NetworkChange_NetworkAvailabilityChanged;
               int idx = 0;
               foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
               {
                   if (ni.Name == SettingsManager.Settings.NetworkInterface)
                   {
                       DefaultProxy = new NetworkInterfaceProxy(ni, SettingsManager.Settings.NetPort,idx);
                       ((NetworkInterfaceProxy)DefaultProxy).Start();
                   }
                   idx++;
               }
           }
           else if (SettingsManager.Settings.ProxyOption == ProxyType.Proxy)
               DefaultProxy = new GWebProxy();
           else DefaultProxy = null;




       }
       static void NetworkChange_NetworkAvailabilityChanged(object sender, EventArgs e)
       {
           try
           {
               if (DefaultProxy != null)
                   ((NetworkInterfaceProxy)DefaultProxy).Stop();
               if (SettingsManager.Settings.ProxyOption == ProxyType.Network)
               {
                   if (SettingsManager.Settings.NetworkInterface == "default")
                   {
                       DefaultProxy = new NetworkInterfaceProxy();
                       return;
                   }
                   int idx = 0;
                   foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                   {
                       if (ni.Name == SettingsManager.Settings.NetworkInterface)
                       {

                           DefaultProxy = new NetworkInterfaceProxy(ni, SettingsManager.Settings.NetPort, idx);
                           ((NetworkInterfaceProxy)DefaultProxy).Start();
                       }
                       idx++;
                   }
               }
               else if (SettingsManager.Settings.ProxyOption == ProxyType.None)
                   DefaultProxy = null;
           }
           catch
           {

           }
       }
       static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
       {
           try
           {
               if(DefaultProxy != null)
                  ((NetworkInterfaceProxy)DefaultProxy).Stop();

               if (SettingsManager.Settings.NetworkInterface == "default")
               {
                   DefaultProxy = new NetworkInterfaceProxy();
                   return;
               }
               int idx = 0;
               foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
               {
                   if (ni.Name == SettingsManager.Settings.NetworkInterface)
                   {
                       DefaultProxy = new NetworkInterfaceProxy(ni, SettingsManager.Settings.NetPort,idx);
                       ((NetworkInterfaceProxy)DefaultProxy).Start();
                   }
                   idx++;
               }
           }
           catch
           {

           }
       }

    }
}
