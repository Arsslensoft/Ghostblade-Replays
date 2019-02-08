using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Ghostblade
{
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();
           // Create your private font collection object.
            PrivateFontCollection pfc = new PrivateFontCollection();

            pfc.AddFontFile(Application.StartupPath + @"\TRANS.ttf");

            label2.Font = new Font(pfc.Families[0], 24, FontStyle.Bold);
            label1.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
