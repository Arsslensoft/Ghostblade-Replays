using GhostLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ghostblade
{
    public static class Helper
    {

        public static bool GetToolTip(Control ctrl, string title,string help)
        {
            if (!SettingsManager.Settings.HelperEnabled)
                return false;
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.UseFading = true;
            toolTip1.UseAnimation = true;
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.ToolTipTitle = title;
          
  
      
            toolTip1.SetToolTip(ctrl,help);

        

            return true;
        }
    }

    

}
