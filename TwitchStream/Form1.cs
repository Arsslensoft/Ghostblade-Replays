using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TwitchStream
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public void LoadStream(string chanel)
        {
            axShockwaveFlash1.FlashVars = "channel="+chanel+"&auto_play=true&start_volume=50";
        }
        public Form1(string chanel)
        {
            InitializeComponent();
            LoadStream(chanel);
        }
    }
}
