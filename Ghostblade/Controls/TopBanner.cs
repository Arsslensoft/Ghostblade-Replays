using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GhostLib;
using System.Diagnostics;
using MetroFramework;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Ghostblade
{
    public partial class TopBanner : UserControl
    {
        Thread CollectorThread;
        public TopBanner()
        {
            InitializeComponent();
            CollectorThread = new Thread(new ThreadStart(Collect));
            timer1.Enabled = true;
        }
        int GCStack = 5;
        void Collect()
        {
            try
            {
                GCStack = 0;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {

            }
        }
        public void Switch(GBAccount acc)
        {
            summoner1.Text = acc.SummonerName;
            summoner2.Text = acc.SummonerName;
            lvl1.Text ="Level " +acc.SummonerLevel.ToString();
            lvl2.Text = "Level " + acc.SummonerLevel.ToString();

            SwitchTierIcon(acc.SummonerTier, acc.SummonerIconId);
        }
        public void SwitchTierIcon(GhostLib.AccountTier tier,int iconid)
        {
            panel1.Visible = false;
            panel3.Visible = false;
            if (tier == GhostLib.AccountTier.Challenger || tier == GhostLib.AccountTier.Master)
            {
                panel1.Visible = true;
                this.panel1.BackgroundImage = (tier == GhostLib.AccountTier.Challenger) ? Ghostblade.Properties.Resources.challenger : Ghostblade.Properties.Resources.master;
                profileicon.Size = new Size(58, 53);
                profileicon.Location = new Point(33, 21);
                this.Height = 86;
            }
            else if (tier == GhostLib.AccountTier.Diamond)
            {
                panel1.Visible = true;
                this.panel1.BackgroundImage = Ghostblade.Properties.Resources.diamond;
                profileicon.Size = new Size(58, 50);
                profileicon.Location = new Point(33, 19);
                this.Height = 86;
            }
            else if (tier == GhostLib.AccountTier.Plat)
            {
                panel1.Visible = true;
                this.panel1.BackgroundImage = Ghostblade.Properties.Resources.platinum;
                profileicon.Size = new Size(58, 55);
                profileicon.Location = new Point(33, 15);
                this.Height = 86;
            }
            else if (tier == GhostLib.AccountTier.Gold)
            {
                panel1.Visible = true;
                this.panel1.BackgroundImage = Ghostblade.Properties.Resources.gold1;
                profileicon.Size = new Size(58, 55);
                profileicon.Location = new Point(33, 17);
                this.Height = 86;
            }
            else if (tier == GhostLib.AccountTier.Silver)
            {
                panel1.Visible = true;
                this.panel1.BackgroundImage = Ghostblade.Properties.Resources.silver;
                profileicon.Size = new Size(58, 55);
                profileicon.Location = new Point(33, 17);
                this.Height = 86;
            }
            else
            {
              
                panel3.Visible = true;
                this.Height = 60;
            }

            _banner = null;
            if (File.Exists(GhostBase.GhostbladeInstance.DataDragonInstance.IconsDirectory + @"\" + iconid.ToString() + ".png"))
                {
                    profilevox.BackgroundImage = Bitmap.FromFile(GhostBase.GhostbladeInstance.DataDragonInstance.IconsDirectory + @"\" + iconid.ToString() + ".png");
                    profileicon.BackgroundImage = Bitmap.FromFile(GhostBase.GhostbladeInstance.DataDragonInstance.IconsDirectory + @"\" + iconid.ToString() + ".png");
                }
                
        }
        Image _banner = null;
        Image BannerBg
        {
            get
            {
                if (_banner != null)
                    return _banner;

                else if (File.Exists(Application.StartupPath + @"\Data\"+SettingsManager.Settings.TopBannerBg))
                {
                    _banner = Image.FromFile(Application.StartupPath + @"\Data\"+SettingsManager.Settings.TopBannerBg);
                    return _banner;
                }
                else return null;

            }
        }
        public void DrawBg(Image image, Graphics graphics)
        {
            var destRect = new Rectangle(0, 0, this.Width, this.Height);
      
 
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            

       
        }
        protected override void OnPaint(PaintEventArgs e)
        {
          if(BannerBg != null)
               //set the graphics interpolation mode to high
              DrawBg(BannerBg, e.Graphics);
            base.OnPaint(e);
          
        }
        private void TopBanner_Resize(object sender, EventArgs e)
        {
            //this.BackgroundImage = global::Ghostblade.Properties.Resources.Default_Profile_Banner;
            //this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        }
        private void metroLink1_Click(object sender, EventArgs e)
        {
            SettingsForm frm = new SettingsForm();
            frm.ShowDialog();
        }
        Process[] procs = null;
        private void timer1_Tick(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroLink pinglabel = (panel3.Visible)?metroLink5:pinglb;
         

            try
            {
              
                procs = Process.GetProcessesByName("League of Legends");
                if (procs.Length == 0)
                {

                    if (Program.MainFormInstance.Visible)
                    {
                        long ping = RiotTool.PingServer(Program.MainFormInstance.SelectedAccount.Region);
                        pinglabel.BeginInvoke(new MethodInvoker(delegate
                        {

                          

                            if (ping != -1)
                            {
                                if (ping >= 150 && ping <= 300)
                                    pinglabel.Style = MetroColorStyle.Yellow;
                                else if (ping > 300)
                                    pinglabel.Style = MetroColorStyle.Red;
                                else pinglabel.Style = MetroColorStyle.Green;
                                pinglabel.Text = ping.ToString() + " ms";



                            }
                            else
                            {
                                pinglabel.Text = ping.ToString() + " ms";
                                pinglabel.Style = MetroColorStyle.Purple;
                            }

                        }));

                    }
                }
                else if(RiotTool.Instance.NeedsToCheckLol(procs[0]))
                    RiotTool.Instance.LolStarted(this, EventArgs.Empty);
                
                procs = null;
                if (!CollectorThread.IsAlive)
                {
                    GCStack++;
                    if (GCStack >= 5)
                        CollectorThread.Start();
                }

            }
            catch
            {
                pinglabel.Style = MetroColorStyle.Magenta;
            }
        }
        private void metroLink5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;

            MessageBox.Show("Ping " + (timer1.Enabled ? "Enabled" : "Disabled"), "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/Arsslensoft");
        }

        private void pinglb_MouseEnter(object sender, EventArgs e)
        {
            Helper.GetToolTip(pinglb, "Ping", "This is your average ping based on the region you set.\nYou can disable or enable this feature by clicking on it [while in-game this feature is automatically disabled] ");
        }

     
    }
}
