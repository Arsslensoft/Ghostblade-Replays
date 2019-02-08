using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostblade
{
    public partial class FeaturedPlayer : UserControl
    {
        public FeaturedPlayer() : this(false)
        {

        }
        public FeaturedPlayer(bool red)
        {
           
            InitializeComponent();
            if (red)
                metroLabel1.Style = MetroFramework.MetroColorStyle.Red;
        }
        int cid = 0;
        public void Load(string playername, int champion)
        {
            cid = champion;

          this.Champion.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Champions\" + CurrentGame.GetChampion(champion) + ".png");
            metroLabel1.Text = playername;
        }

        private void Champion_Click(object sender, EventArgs e)
        {
            Program.MainFormInstance.ShowChampInfo(cid);
        }
    }
}
