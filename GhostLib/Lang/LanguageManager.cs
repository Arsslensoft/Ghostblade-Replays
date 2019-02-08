using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib.Lang
{
   public class LanguageManager
    {

       static LanguageManager _instance = null;
       public static LanguageManager Instance
        {

            get
            {
                if (_instance == null)
                    _instance = new LanguageManager();

                return _instance;
            }
        }

       public LanguageManager()
       {

           Lang = new GhostLocale();
       }

       public GhostLocale Lang { get; set; }

       public void LoadLanguage(string file)
       {
           try
           {
               string local = File.ReadAllText(file);
               Lang = JsonConvert.DeserializeObject<GhostLocale>(local);
               local = null;
           }
           catch (Exception ex)
           {
               Logger.Instance.Log.Error("Unable to load language file " + file, ex);

           }
           finally
           {
               GC.Collect();
           }
       }

       
    }

   public enum GhostLanguage : byte
   {
       Default = 0,
       US = 1,
       FR = 2,
       DE = 3,
       AR = 4,
       RU = 5,
       SP = 6,
       TR = 7,
       KR = 8

   }
}
