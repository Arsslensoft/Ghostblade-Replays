using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLib
{
  
   public class ApplicationInstance
    {
       static ApplicationInstance _instance = null;
       public static ApplicationInstance Instance
        {

            get
            {
                if (_instance == null)
                    _instance = new ApplicationInstance();

                return _instance;
            }
        }

       public const string ApplicationName = "Ghostblade";
       public const string Developer = "Arsslensoft";
       public const string ApplicationVersion = "0.9.1.0";
       public const string ReplayExt = ".lgr";


       public string BaseDirectory
       {
           get { return GetRegistryProperty("MainDirectory").ToString(); }
       }

       public object GetRegistryProperty(string name)
       {
           try
           {
               RegistryKey key;
               if ((key=Registry.LocalMachine.OpenSubKey(string.Format(@"SOFTWARE\{0}\{1}\", Developer, ApplicationName))) != null)
                return   key.GetValue(name);
          
               else return null;
           }
           catch (Exception ex)
           {
               Logger.Instance.Log.Error("Could not get the property "+name, ex);
               return null;
           }
       }
       public void Register(string clientid)
       {
           try{
               if (Registry.LocalMachine.OpenSubKey(string.Format(@"SOFTWARE\{0}\{1}\", Developer, ApplicationName)) != null)
               {
                   // Modify
                   using (RegistryKey Key = Registry.LocalMachine.OpenSubKey(string.Format(@"SOFTWARE\{0}\{1}\", Developer, ApplicationName), true))
                   {
                       Key.SetValue("MainDirectory", AppDomain.CurrentDomain.BaseDirectory);
                       Key.SetValue("Version", ApplicationVersion);
                       Key.SetValue("ClientId", clientid);

                   }

               }
               else
               {
                   // CREATE
                   using (RegistryKey Key = Registry.LocalMachine.CreateSubKey(string.Format(@"SOFTWARE\{0}\{1}\", Developer, ApplicationName)))
                   {
                       Key.SetValue("MainDirectory", AppDomain.CurrentDomain.BaseDirectory);
                       Key.SetValue("Version", ApplicationVersion);
                       Key.SetValue("ClientId", clientid);
                   }
               }


          
           }
       catch(Exception ex)
           {
                   Logger.Instance.Log.Error("Could not register the application", ex);
            }
       }
       public void AssociateExtension()
       {
           try
           {
               FileAssociationInfo fai = new FileAssociationInfo(ReplayExt);
               if (!fai.Exists)
               {
                   fai.Create("GhostbladeReplays");

                   //Specify MIME type (optional)
                   fai.ContentType = "application/ghostblade-replay";

                   //Programs automatically displayed in open with list
                   fai.OpenWithList = new string[] { "Ghostblade.exe" };

               }

               ProgramAssociationInfo pai = new ProgramAssociationInfo(fai.ProgID);
               if (!pai.Exists)
               {
                   pai.Create
                   (
                       //Description of program/file type
                   "Ghostblade Replay",

                   new ProgramVerb
                        (
                       //Verb name
                        "Open",
                       //Path and arguments to use
                        AppDomain.CurrentDomain.BaseDirectory + "Ghostblade.exe %1"
                        )
                      );

                   //optional
                   pai.DefaultIcon = new ProgramIcon(AppDomain.CurrentDomain.BaseDirectory + "LGR.ico");
               }
               else if (pai.Verbs[0].Command != (AppDomain.CurrentDomain.BaseDirectory + "Ghostblade.exe %1"))
               {
                   pai.Verbs[0] = new ProgramVerb
                        (
                       //Verb name
                        "Open",
                       //Path and arguments to use
                                  AppDomain.CurrentDomain.BaseDirectory + "Ghostblade.exe %1"
                        );

                   pai.DefaultIcon = new ProgramIcon(AppDomain.CurrentDomain.BaseDirectory + "LGR.ico");
               }

           }
           catch
           {

           }
       }


    }
}
