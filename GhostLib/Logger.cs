using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib
{
   public class Logger
    {
       static Logger _instance = null;
       public static Logger Instance { 
           get {
               if (_instance == null)
                   _instance = new Logger();
               return _instance; 
           } 
       }

       private readonly ILog log = LogManager.GetLogger(typeof(Logger));
       public string LogFileName { get; set; }
       public Logger()
       {
     //      LogFileName = "log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml";
           LogFileName = "All-logs.xml";
           log4net.GlobalContext.Properties["LogFileName"] = LogFileName;
           log4net.Config.XmlConfigurator.Configure();

        
       }

       public ILog Log
       { get { return log; } }


    }
}
