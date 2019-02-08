using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib
{
   public class GhostRessources
    {
 
       static GhostRessources _instance = null;
       public static GhostRessources Instance
        {

            get
            {
                if (_instance == null)
                {
                    _instance = new GhostRessources();
                   _instance. LoadChampions();
                }
                return _instance;
            }
        }


       public void LoadChampions()
       {
           try
           {
               string[] lines = File.ReadAllLines(ApplicationInstance.Instance.BaseDirectory + @"Champions\Champions.txt");
               foreach (string ln in lines)
               {
                   string[] l = ln.Split(',');
                   Champs.Add(int.Parse(l[0]), l[1]);
                   RevChamps.Add(l[1].ToLower(), int.Parse(l[0]));
               }
           }
           catch (Exception ex)
           {

           }
       }
       public Dictionary<int, string> Champs = new Dictionary<int, string>();
       public Dictionary<string, int> RevChamps = new Dictionary<string, int>();
       public string GetChampion(int id)
       {
           if (Champs.ContainsKey(id))
               return Champs[id];
           else return "Unknown";

       }
       public int GetChampionID(string id)
       {
           return RevChamps[id.ToLower()];

       }

       public static Image GetChampionIcon(string name)
       {
           //return (Image)GBData.Properties.Resources.ResourceManager.GetObject(name);
           return new Bitmap(ApplicationInstance.Instance.BaseDirectory + @"Champions\" + name + ".png");
       }

       public static Image GetItemIcon(string name)
       {
           //return (Image)GBData.Properties.Resources.ResourceManager.GetObject("_"+name);
           if (File.Exists(ApplicationInstance.Instance.BaseDirectory + @"Items\" + name + ".png"))
               return new Bitmap(ApplicationInstance.Instance.BaseDirectory + @"Items\" + name + ".png");
           else
               return null;
       }
       public static Image GetSpellIcon(string name)
       {
           //return (Image)GBData.Properties.Resources.ResourceManager.GetObject("_" + name);

           return new Bitmap(ApplicationInstance.Instance.BaseDirectory + @"Spells\" + name + ".png");

       }


    }
}
