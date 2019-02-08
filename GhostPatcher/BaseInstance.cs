using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GhostBase
{
    public delegate void LogExceptionEventHandler(string message,Exception ex);


    public static class BaseInstance
    {
       public static event LogExceptionEventHandler OnExceptionOccured;
       public static void Log(string message, Exception ex)
       {
           if (OnExceptionOccured != null)
               OnExceptionOccured(message, ex);
       }
    }
}
