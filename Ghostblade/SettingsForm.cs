using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ghostblade
{
    public partial class SettingsForm :  XCoolForm.XCoolForm
    {
        void LoadStyle()
        {
            this.TitleBar.TitleBarCaption = "Ghostblade Settings";
            this.TitleBar.TitleBarType = XCoolForm.XTitleBar.XTitleBarType.Rounded;
            this.TitleBar.TitleBarFill = XCoolForm.XTitleBar.XTitleBarFill.LinearRendering;


            this.MenuIcon = Ghostblade.Properties.Resources.ghost_white.GetThumbnailImage(24, 24, null, IntPtr.Zero);
            //  this.TitleBar.TitleBarFill = XCoolForm.XTitleBar.XTitleBarFill.AdvancedRendering;
            TitleBar.InnerTitleBarColor = Color.FromArgb(255, 17, 17, 17);
            //   TitleBar.OuterTitleBarColor = Color.FromArgb(255, 240, 230, 235);
            TitleBar.TitleBarMixColors.Add(Color.FromArgb(255, 17, 17, 17));

            this.TitleBar.TitleBarButtons[0].ButtonStyle = XCoolForm.XTitleBarButton.XTitleBarButtonStyle.Pixeled;
            this.TitleBar.TitleBarButtons[0].ButtonFillMode = XCoolForm.XTitleBarButton.XButtonFillMode.UpperGlow;


            this.TitleBar.TitleBarButtons[1].ButtonStyle = XCoolForm.XTitleBarButton.XTitleBarButtonStyle.Pixeled;
            this.TitleBar.TitleBarButtons[1].ButtonFillMode = XCoolForm.XTitleBarButton.XButtonFillMode.UpperGlow;

            this.TitleBar.TitleBarButtons[2].ButtonStyle = XCoolForm.XTitleBarButton.XTitleBarButtonStyle.Pixeled;
            this.TitleBar.TitleBarButtons[2].ButtonFillMode = XCoolForm.XTitleBarButton.XButtonFillMode.UpperGlow;



        }
        public SettingsForm()
        {
            InitializeComponent();
            settingsControl1.LoadSettings();
            LoadStyle();
        
        }
    }
}
