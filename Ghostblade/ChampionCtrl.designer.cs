using MetroFramework.Controls;
using System.Windows.Forms;
namespace Ghostblade
{
    partial class ChampionCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gInfoInformation = new System.Windows.Forms.GroupBox();
            this.infoTitle = new MetroFramework.Controls.MetroLabel();
            this.infoDisplayName = new MetroFramework.Controls.MetroLabel();
            this.infoOpponentTips = new MetroFramework.Controls.MetroTextBox();
            this.infoTips = new MetroFramework.Controls.MetroTextBox();
            this.label30 = new MetroFramework.Controls.MetroLabel();
            this.label29 = new MetroFramework.Controls.MetroLabel();
            this.label28 = new MetroFramework.Controls.MetroLabel();
            this.infoDescription = new MetroFramework.Controls.MetroTextBox();
            this.infoTags = new MetroFramework.Controls.MetroLabel();
            this.infoIcon = new System.Windows.Forms.PictureBox();
            this.infoList = new MetroFramework.Controls.MetroComboBox();
            this.gInfoAbilities = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.infoAbilityEffectR = new MetroFramework.Controls.MetroTextBox();
            this.infoAbilityNameR = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityCooldownR = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityIconR = new System.Windows.Forms.PictureBox();
            this.infoAbilityCostR = new MetroFramework.Controls.MetroLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.infoAbilityEffectE = new MetroFramework.Controls.MetroTextBox();
            this.infoAbilityNameE = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityCooldownE = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityIconE = new System.Windows.Forms.PictureBox();
            this.infoAbilityCostE = new MetroFramework.Controls.MetroLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.infoAbilityEffectW = new MetroFramework.Controls.MetroTextBox();
            this.infoAbilityNameW = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityCooldownW = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityIconW = new System.Windows.Forms.PictureBox();
            this.infoAbilityCostW = new MetroFramework.Controls.MetroLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.infoAbilityEffectQ = new MetroFramework.Controls.MetroTextBox();
            this.infoAbilityNameQ = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityCooldownQ = new MetroFramework.Controls.MetroLabel();
            this.infoAbilityIconQ = new System.Windows.Forms.PictureBox();
            this.infoAbilityCostQ = new MetroFramework.Controls.MetroLabel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.infoAbilityEffectP = new MetroFramework.Controls.MetroTextBox();
            this.infoAbilityIconP = new System.Windows.Forms.PictureBox();
            this.infoAbilityNameP = new MetroFramework.Controls.MetroLabel();
            this.gInfoSkins = new System.Windows.Forms.GroupBox();
            this.infoPortrait = new System.Windows.Forms.PictureBox();
            this.infoSkinsList = new MetroFramework.Controls.MetroComboBox();
            this.gInfoStatistics = new System.Windows.Forms.GroupBox();
            this.infoRatingDifficulty = new MetroFramework.Controls.MetroProgressBar();
            this.label35 = new MetroFramework.Controls.MetroLabel();
            this.infoRatingMagic = new MetroFramework.Controls.MetroProgressBar();
            this.label34 = new MetroFramework.Controls.MetroLabel();
            this.infoRatingDefense = new MetroFramework.Controls.MetroProgressBar();
            this.label33 = new MetroFramework.Controls.MetroLabel();
            this.infoRatingAttack = new MetroFramework.Controls.MetroProgressBar();
            this.label31 = new MetroFramework.Controls.MetroLabel();
            this.infoStats = new MetroGrid();
            this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.gInfoInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoIcon)).BeginInit();
            this.gInfoAbilities.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconR)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconE)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconW)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconQ)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconP)).BeginInit();
            this.gInfoSkins.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoPortrait)).BeginInit();
            this.gInfoStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoStats)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.gInfoInformation);
            this.panel1.Controls.Add(this.infoList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 1353);
            this.panel1.TabIndex = 0;
            // 
            // gInfoInformation
            // 
            this.gInfoInformation.Controls.Add(this.infoTitle);
            this.gInfoInformation.Controls.Add(this.infoDisplayName);
            this.gInfoInformation.Controls.Add(this.infoOpponentTips);
            this.gInfoInformation.Controls.Add(this.infoTips);
            this.gInfoInformation.Controls.Add(this.label30);
            this.gInfoInformation.Controls.Add(this.label29);
            this.gInfoInformation.Controls.Add(this.label28);
            this.gInfoInformation.Controls.Add(this.infoDescription);
            this.gInfoInformation.Controls.Add(this.infoTags);
            this.gInfoInformation.Controls.Add(this.infoIcon);
            this.gInfoInformation.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.gInfoInformation.Location = new System.Drawing.Point(0, 65);
            this.gInfoInformation.Name = "gInfoInformation";
            this.gInfoInformation.Size = new System.Drawing.Size(379, 661);
            this.gInfoInformation.TabIndex = 2;
            this.gInfoInformation.TabStop = false;
            this.gInfoInformation.Text = "Champion Information";
            this.gInfoInformation.Visible = false;
            // 
            // infoTitle
            // 
            this.infoTitle.AutoSize = true;
            this.infoTitle.Location = new System.Drawing.Point(135, 60);
            this.infoTitle.Name = "infoTitle";
            this.infoTitle.Size = new System.Drawing.Size(98, 19);
            this.infoTitle.TabIndex = 11;
            this.infoTitle.Text = "Champion Title";
            // 
            // infoDisplayName
            // 
            this.infoDisplayName.AutoSize = true;
            this.infoDisplayName.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.infoDisplayName.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.infoDisplayName.Location = new System.Drawing.Point(135, 25);
            this.infoDisplayName.Name = "infoDisplayName";
            this.infoDisplayName.Size = new System.Drawing.Size(153, 25);
            this.infoDisplayName.TabIndex = 10;
            this.infoDisplayName.Text = "Champion Name";
            // 
            // infoOpponentTips
            // 
            this.infoOpponentTips.Lines = new string[0];
            this.infoOpponentTips.Location = new System.Drawing.Point(6, 515);
            this.infoOpponentTips.MaxLength = 32767;
            this.infoOpponentTips.Multiline = true;
            this.infoOpponentTips.Name = "infoOpponentTips";
            this.infoOpponentTips.PasswordChar = '\0';
            this.infoOpponentTips.ReadOnly = true;
            this.infoOpponentTips.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoOpponentTips.SelectedText = "";
            this.infoOpponentTips.Size = new System.Drawing.Size(367, 140);
            this.infoOpponentTips.TabIndex = 8;
            this.infoOpponentTips.UseSelectable = true;
            // 
            // infoTips
            // 
            this.infoTips.Lines = new string[0];
            this.infoTips.Location = new System.Drawing.Point(6, 341);
            this.infoTips.MaxLength = 32767;
            this.infoTips.Multiline = true;
            this.infoTips.Name = "infoTips";
            this.infoTips.PasswordChar = '\0';
            this.infoTips.ReadOnly = true;
            this.infoTips.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoTips.SelectedText = "";
            this.infoTips.Size = new System.Drawing.Size(367, 140);
            this.infoTips.TabIndex = 6;
            this.infoTips.UseSelectable = true;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 493);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(101, 19);
            this.label30.TabIndex = 9;
            this.label30.Text = "Playing Against:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 322);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(35, 19);
            this.label29.TabIndex = 7;
            this.label29.Text = "Tips:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 154);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(77, 19);
            this.label28.TabIndex = 6;
            this.label28.Text = "Description:";
            // 
            // infoDescription
            // 
            this.infoDescription.Lines = new string[0];
            this.infoDescription.Location = new System.Drawing.Point(6, 171);
            this.infoDescription.MaxLength = 32767;
            this.infoDescription.Multiline = true;
            this.infoDescription.Name = "infoDescription";
            this.infoDescription.PasswordChar = '\0';
            this.infoDescription.ReadOnly = true;
            this.infoDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoDescription.SelectedText = "";
            this.infoDescription.Size = new System.Drawing.Size(367, 140);
            this.infoDescription.TabIndex = 5;
            this.infoDescription.UseSelectable = true;
            // 
            // infoTags
            // 
            this.infoTags.AutoSize = true;
            this.infoTags.Location = new System.Drawing.Point(132, 127);
            this.infoTags.Name = "infoTags";
            this.infoTags.Size = new System.Drawing.Size(100, 19);
            this.infoTags.TabIndex = 4;
            this.infoTags.Text = "Tag1, tag2, tag3";
            // 
            // infoIcon
            // 
            this.infoIcon.Location = new System.Drawing.Point(6, 21);
            this.infoIcon.Name = "infoIcon";
            this.infoIcon.Size = new System.Drawing.Size(120, 120);
            this.infoIcon.TabIndex = 1;
            this.infoIcon.TabStop = false;
            // 
            // infoList
            // 
            this.infoList.FormattingEnabled = true;
            this.infoList.ItemHeight = 23;
            this.infoList.Location = new System.Drawing.Point(35, 24);
            this.infoList.Name = "infoList";
            this.infoList.Size = new System.Drawing.Size(297, 29);
            this.infoList.TabIndex = 0;
            this.infoList.UseSelectable = true;
            this.infoList.SelectedIndexChanged += new System.EventHandler(this.infoList_SelectedIndexChanged);
            // 
            // gInfoAbilities
            // 
            this.gInfoAbilities.Controls.Add(this.panel5);
            this.gInfoAbilities.Controls.Add(this.panel4);
            this.gInfoAbilities.Controls.Add(this.panel3);
            this.gInfoAbilities.Controls.Add(this.panel2);
            this.gInfoAbilities.Controls.Add(this.panel6);
            this.gInfoAbilities.ForeColor = System.Drawing.Color.White;
            this.gInfoAbilities.Location = new System.Drawing.Point(391, 347);
            this.gInfoAbilities.Name = "gInfoAbilities";
            this.gInfoAbilities.Size = new System.Drawing.Size(502, 1006);
            this.gInfoAbilities.TabIndex = 7;
            this.gInfoAbilities.TabStop = false;
            this.gInfoAbilities.Text = "Abilities";
            this.gInfoAbilities.Visible = false;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.infoAbilityEffectR);
            this.panel5.Controls.Add(this.infoAbilityNameR);
            this.panel5.Controls.Add(this.infoAbilityCooldownR);
            this.panel5.Controls.Add(this.infoAbilityIconR);
            this.panel5.Controls.Add(this.infoAbilityCostR);
            this.panel5.Location = new System.Drawing.Point(6, 803);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(487, 196);
            this.panel5.TabIndex = 14;
            // 
            // infoAbilityEffectR
            // 
            this.infoAbilityEffectR.Lines = new string[0];
            this.infoAbilityEffectR.Location = new System.Drawing.Point(96, 36);
            this.infoAbilityEffectR.MaxLength = 32767;
            this.infoAbilityEffectR.Multiline = true;
            this.infoAbilityEffectR.Name = "infoAbilityEffectR";
            this.infoAbilityEffectR.PasswordChar = '\0';
            this.infoAbilityEffectR.ReadOnly = true;
            this.infoAbilityEffectR.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoAbilityEffectR.SelectedText = "";
            this.infoAbilityEffectR.Size = new System.Drawing.Size(378, 125);
            this.infoAbilityEffectR.TabIndex = 10;
            this.infoAbilityEffectR.UseSelectable = true;
            // 
            // infoAbilityNameR
            // 
            this.infoAbilityNameR.AutoSize = true;
            this.infoAbilityNameR.Location = new System.Drawing.Point(17, 10);
            this.infoAbilityNameR.Name = "infoAbilityNameR";
            this.infoAbilityNameR.Size = new System.Drawing.Size(25, 19);
            this.infoAbilityNameR.TabIndex = 6;
            this.infoAbilityNameR.Text = "(R)";
            // 
            // infoAbilityCooldownR
            // 
            this.infoAbilityCooldownR.AutoSize = true;
            this.infoAbilityCooldownR.Location = new System.Drawing.Point(231, 169);
            this.infoAbilityCooldownR.Name = "infoAbilityCooldownR";
            this.infoAbilityCooldownR.Size = new System.Drawing.Size(226, 19);
            this.infoAbilityCooldownR.TabIndex = 9;
            this.infoAbilityCooldownR.Text = "Cooldown: 1000/1000/1000/1000/1000";
            // 
            // infoAbilityIconR
            // 
            this.infoAbilityIconR.Location = new System.Drawing.Point(21, 36);
            this.infoAbilityIconR.Name = "infoAbilityIconR";
            this.infoAbilityIconR.Size = new System.Drawing.Size(64, 64);
            this.infoAbilityIconR.TabIndex = 5;
            this.infoAbilityIconR.TabStop = false;
            // 
            // infoAbilityCostR
            // 
            this.infoAbilityCostR.AutoSize = true;
            this.infoAbilityCostR.Location = new System.Drawing.Point(8, 169);
            this.infoAbilityCostR.Name = "infoAbilityCostR";
            this.infoAbilityCostR.Size = new System.Drawing.Size(192, 19);
            this.infoAbilityCostR.TabIndex = 8;
            this.infoAbilityCostR.Text = "Cost: 1000/1000/1000/1000/1000";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.infoAbilityEffectE);
            this.panel4.Controls.Add(this.infoAbilityNameE);
            this.panel4.Controls.Add(this.infoAbilityCooldownE);
            this.panel4.Controls.Add(this.infoAbilityIconE);
            this.panel4.Controls.Add(this.infoAbilityCostE);
            this.panel4.Location = new System.Drawing.Point(6, 602);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(487, 196);
            this.panel4.TabIndex = 13;
            // 
            // infoAbilityEffectE
            // 
            this.infoAbilityEffectE.Lines = new string[0];
            this.infoAbilityEffectE.Location = new System.Drawing.Point(96, 36);
            this.infoAbilityEffectE.MaxLength = 32767;
            this.infoAbilityEffectE.Multiline = true;
            this.infoAbilityEffectE.Name = "infoAbilityEffectE";
            this.infoAbilityEffectE.PasswordChar = '\0';
            this.infoAbilityEffectE.ReadOnly = true;
            this.infoAbilityEffectE.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoAbilityEffectE.SelectedText = "";
            this.infoAbilityEffectE.Size = new System.Drawing.Size(378, 125);
            this.infoAbilityEffectE.TabIndex = 10;
            this.infoAbilityEffectE.UseSelectable = true;
            // 
            // infoAbilityNameE
            // 
            this.infoAbilityNameE.AutoSize = true;
            this.infoAbilityNameE.Location = new System.Drawing.Point(17, 10);
            this.infoAbilityNameE.Name = "infoAbilityNameE";
            this.infoAbilityNameE.Size = new System.Drawing.Size(24, 19);
            this.infoAbilityNameE.TabIndex = 6;
            this.infoAbilityNameE.Text = "(E)";
            // 
            // infoAbilityCooldownE
            // 
            this.infoAbilityCooldownE.AutoSize = true;
            this.infoAbilityCooldownE.Location = new System.Drawing.Point(231, 169);
            this.infoAbilityCooldownE.Name = "infoAbilityCooldownE";
            this.infoAbilityCooldownE.Size = new System.Drawing.Size(226, 19);
            this.infoAbilityCooldownE.TabIndex = 9;
            this.infoAbilityCooldownE.Text = "Cooldown: 1000/1000/1000/1000/1000";
            // 
            // infoAbilityIconE
            // 
            this.infoAbilityIconE.Location = new System.Drawing.Point(21, 36);
            this.infoAbilityIconE.Name = "infoAbilityIconE";
            this.infoAbilityIconE.Size = new System.Drawing.Size(64, 64);
            this.infoAbilityIconE.TabIndex = 5;
            this.infoAbilityIconE.TabStop = false;
            // 
            // infoAbilityCostE
            // 
            this.infoAbilityCostE.AutoSize = true;
            this.infoAbilityCostE.Location = new System.Drawing.Point(8, 169);
            this.infoAbilityCostE.Name = "infoAbilityCostE";
            this.infoAbilityCostE.Size = new System.Drawing.Size(192, 19);
            this.infoAbilityCostE.TabIndex = 8;
            this.infoAbilityCostE.Text = "Cost: 1000/1000/1000/1000/1000";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.infoAbilityEffectW);
            this.panel3.Controls.Add(this.infoAbilityNameW);
            this.panel3.Controls.Add(this.infoAbilityCooldownW);
            this.panel3.Controls.Add(this.infoAbilityIconW);
            this.panel3.Controls.Add(this.infoAbilityCostW);
            this.panel3.Location = new System.Drawing.Point(6, 400);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(487, 196);
            this.panel3.TabIndex = 12;
            // 
            // infoAbilityEffectW
            // 
            this.infoAbilityEffectW.Lines = new string[0];
            this.infoAbilityEffectW.Location = new System.Drawing.Point(96, 36);
            this.infoAbilityEffectW.MaxLength = 32767;
            this.infoAbilityEffectW.Multiline = true;
            this.infoAbilityEffectW.Name = "infoAbilityEffectW";
            this.infoAbilityEffectW.PasswordChar = '\0';
            this.infoAbilityEffectW.ReadOnly = true;
            this.infoAbilityEffectW.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoAbilityEffectW.SelectedText = "";
            this.infoAbilityEffectW.Size = new System.Drawing.Size(378, 125);
            this.infoAbilityEffectW.TabIndex = 10;
            this.infoAbilityEffectW.UseSelectable = true;
            // 
            // infoAbilityNameW
            // 
            this.infoAbilityNameW.AutoSize = true;
            this.infoAbilityNameW.Location = new System.Drawing.Point(17, 10);
            this.infoAbilityNameW.Name = "infoAbilityNameW";
            this.infoAbilityNameW.Size = new System.Drawing.Size(30, 19);
            this.infoAbilityNameW.TabIndex = 6;
            this.infoAbilityNameW.Text = "(W)";
            // 
            // infoAbilityCooldownW
            // 
            this.infoAbilityCooldownW.AutoSize = true;
            this.infoAbilityCooldownW.Location = new System.Drawing.Point(231, 169);
            this.infoAbilityCooldownW.Name = "infoAbilityCooldownW";
            this.infoAbilityCooldownW.Size = new System.Drawing.Size(226, 19);
            this.infoAbilityCooldownW.TabIndex = 9;
            this.infoAbilityCooldownW.Text = "Cooldown: 1000/1000/1000/1000/1000";
            // 
            // infoAbilityIconW
            // 
            this.infoAbilityIconW.Location = new System.Drawing.Point(21, 36);
            this.infoAbilityIconW.Name = "infoAbilityIconW";
            this.infoAbilityIconW.Size = new System.Drawing.Size(64, 64);
            this.infoAbilityIconW.TabIndex = 5;
            this.infoAbilityIconW.TabStop = false;
            // 
            // infoAbilityCostW
            // 
            this.infoAbilityCostW.AutoSize = true;
            this.infoAbilityCostW.Location = new System.Drawing.Point(8, 169);
            this.infoAbilityCostW.Name = "infoAbilityCostW";
            this.infoAbilityCostW.Size = new System.Drawing.Size(192, 19);
            this.infoAbilityCostW.TabIndex = 8;
            this.infoAbilityCostW.Text = "Cost: 1000/1000/1000/1000/1000";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.infoAbilityEffectQ);
            this.panel2.Controls.Add(this.infoAbilityNameQ);
            this.panel2.Controls.Add(this.infoAbilityCooldownQ);
            this.panel2.Controls.Add(this.infoAbilityIconQ);
            this.panel2.Controls.Add(this.infoAbilityCostQ);
            this.panel2.Location = new System.Drawing.Point(6, 198);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(487, 196);
            this.panel2.TabIndex = 11;
            // 
            // infoAbilityEffectQ
            // 
            this.infoAbilityEffectQ.Lines = new string[0];
            this.infoAbilityEffectQ.Location = new System.Drawing.Point(96, 36);
            this.infoAbilityEffectQ.MaxLength = 32767;
            this.infoAbilityEffectQ.Multiline = true;
            this.infoAbilityEffectQ.Name = "infoAbilityEffectQ";
            this.infoAbilityEffectQ.PasswordChar = '\0';
            this.infoAbilityEffectQ.ReadOnly = true;
            this.infoAbilityEffectQ.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoAbilityEffectQ.SelectedText = "";
            this.infoAbilityEffectQ.Size = new System.Drawing.Size(378, 125);
            this.infoAbilityEffectQ.TabIndex = 10;
            this.infoAbilityEffectQ.UseSelectable = true;
            // 
            // infoAbilityNameQ
            // 
            this.infoAbilityNameQ.AutoSize = true;
            this.infoAbilityNameQ.Location = new System.Drawing.Point(17, 10);
            this.infoAbilityNameQ.Name = "infoAbilityNameQ";
            this.infoAbilityNameQ.Size = new System.Drawing.Size(28, 19);
            this.infoAbilityNameQ.TabIndex = 6;
            this.infoAbilityNameQ.Text = "(Q)";
            // 
            // infoAbilityCooldownQ
            // 
            this.infoAbilityCooldownQ.AutoSize = true;
            this.infoAbilityCooldownQ.Location = new System.Drawing.Point(231, 169);
            this.infoAbilityCooldownQ.Name = "infoAbilityCooldownQ";
            this.infoAbilityCooldownQ.Size = new System.Drawing.Size(226, 19);
            this.infoAbilityCooldownQ.TabIndex = 9;
            this.infoAbilityCooldownQ.Text = "Cooldown: 1000/1000/1000/1000/1000";
            // 
            // infoAbilityIconQ
            // 
            this.infoAbilityIconQ.Location = new System.Drawing.Point(21, 36);
            this.infoAbilityIconQ.Name = "infoAbilityIconQ";
            this.infoAbilityIconQ.Size = new System.Drawing.Size(64, 64);
            this.infoAbilityIconQ.TabIndex = 5;
            this.infoAbilityIconQ.TabStop = false;
            // 
            // infoAbilityCostQ
            // 
            this.infoAbilityCostQ.AutoSize = true;
            this.infoAbilityCostQ.Location = new System.Drawing.Point(8, 169);
            this.infoAbilityCostQ.Name = "infoAbilityCostQ";
            this.infoAbilityCostQ.Size = new System.Drawing.Size(192, 19);
            this.infoAbilityCostQ.TabIndex = 8;
            this.infoAbilityCostQ.Text = "Cost: 1000/1000/1000/1000/1000";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.infoAbilityEffectP);
            this.panel6.Controls.Add(this.infoAbilityIconP);
            this.panel6.Controls.Add(this.infoAbilityNameP);
            this.panel6.Location = new System.Drawing.Point(9, 21);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(484, 171);
            this.panel6.TabIndex = 10;
            // 
            // infoAbilityEffectP
            // 
            this.infoAbilityEffectP.Lines = new string[0];
            this.infoAbilityEffectP.Location = new System.Drawing.Point(92, 33);
            this.infoAbilityEffectP.MaxLength = 32767;
            this.infoAbilityEffectP.Multiline = true;
            this.infoAbilityEffectP.Name = "infoAbilityEffectP";
            this.infoAbilityEffectP.PasswordChar = '\0';
            this.infoAbilityEffectP.ReadOnly = true;
            this.infoAbilityEffectP.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoAbilityEffectP.SelectedText = "";
            this.infoAbilityEffectP.Size = new System.Drawing.Size(378, 125);
            this.infoAbilityEffectP.TabIndex = 2;
            this.infoAbilityEffectP.UseSelectable = true;
            // 
            // infoAbilityIconP
            // 
            this.infoAbilityIconP.Location = new System.Drawing.Point(17, 33);
            this.infoAbilityIconP.Name = "infoAbilityIconP";
            this.infoAbilityIconP.Size = new System.Drawing.Size(64, 64);
            this.infoAbilityIconP.TabIndex = 0;
            this.infoAbilityIconP.TabStop = false;
            // 
            // infoAbilityNameP
            // 
            this.infoAbilityNameP.AutoSize = true;
            this.infoAbilityNameP.Location = new System.Drawing.Point(14, 7);
            this.infoAbilityNameP.Name = "infoAbilityNameP";
            this.infoAbilityNameP.Size = new System.Drawing.Size(57, 19);
            this.infoAbilityNameP.TabIndex = 1;
            this.infoAbilityNameP.Text = "(Passive)";
            // 
            // gInfoSkins
            // 
            this.gInfoSkins.Controls.Add(this.infoPortrait);
            this.gInfoSkins.Controls.Add(this.infoSkinsList);
            this.gInfoSkins.ForeColor = System.Drawing.Color.White;
            this.gInfoSkins.Location = new System.Drawing.Point(724, 6);
            this.gInfoSkins.Name = "gInfoSkins";
            this.gInfoSkins.Size = new System.Drawing.Size(169, 335);
            this.gInfoSkins.TabIndex = 6;
            this.gInfoSkins.TabStop = false;
            this.gInfoSkins.Text = "Skins";
            this.gInfoSkins.Visible = false;
            // 
            // infoPortrait
            // 
            this.infoPortrait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPortrait.Location = new System.Drawing.Point(6, 49);
            this.infoPortrait.Name = "infoPortrait";
            this.infoPortrait.Size = new System.Drawing.Size(154, 280);
            this.infoPortrait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.infoPortrait.TabIndex = 1;
            this.infoPortrait.TabStop = false;
            // 
            // infoSkinsList
            // 
            this.infoSkinsList.FormattingEnabled = true;
            this.infoSkinsList.ItemHeight = 23;
            this.infoSkinsList.Location = new System.Drawing.Point(6, 21);
            this.infoSkinsList.Name = "infoSkinsList";
            this.infoSkinsList.Size = new System.Drawing.Size(154, 29);
            this.infoSkinsList.TabIndex = 0;
            this.infoSkinsList.UseSelectable = true;
            this.infoSkinsList.SelectedIndexChanged += new System.EventHandler(this.infoSkinsList_SelectedIndexChanged);
            // 
            // gInfoStatistics
            // 
            this.gInfoStatistics.Controls.Add(this.infoRatingDifficulty);
            this.gInfoStatistics.Controls.Add(this.label35);
            this.gInfoStatistics.Controls.Add(this.infoRatingMagic);
            this.gInfoStatistics.Controls.Add(this.label34);
            this.gInfoStatistics.Controls.Add(this.infoRatingDefense);
            this.gInfoStatistics.Controls.Add(this.label33);
            this.gInfoStatistics.Controls.Add(this.infoRatingAttack);
            this.gInfoStatistics.Controls.Add(this.label31);
            this.gInfoStatistics.Controls.Add(this.infoStats);
            this.gInfoStatistics.ForeColor = System.Drawing.Color.White;
            this.gInfoStatistics.Location = new System.Drawing.Point(391, 6);
            this.gInfoStatistics.Name = "gInfoStatistics";
            this.gInfoStatistics.Size = new System.Drawing.Size(327, 335);
            this.gInfoStatistics.TabIndex = 5;
            this.gInfoStatistics.TabStop = false;
            this.gInfoStatistics.Text = "Statistics";
            this.gInfoStatistics.Visible = false;
            // 
            // infoRatingDifficulty
            // 
            this.infoRatingDifficulty.Location = new System.Drawing.Point(98, 85);
            this.infoRatingDifficulty.Maximum = 10;
            this.infoRatingDifficulty.Name = "infoRatingDifficulty";
            this.infoRatingDifficulty.Size = new System.Drawing.Size(222, 15);
            this.infoRatingDifficulty.Step = 1;
            this.infoRatingDifficulty.TabIndex = 17;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 86);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(58, 19);
            this.label35.TabIndex = 16;
            this.label35.Text = "Difficulty";
            // 
            // infoRatingMagic
            // 
            this.infoRatingMagic.Location = new System.Drawing.Point(98, 62);
            this.infoRatingMagic.Maximum = 10;
            this.infoRatingMagic.Name = "infoRatingMagic";
            this.infoRatingMagic.Size = new System.Drawing.Size(222, 15);
            this.infoRatingMagic.Step = 1;
            this.infoRatingMagic.TabIndex = 15;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(6, 63);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(45, 19);
            this.label34.TabIndex = 14;
            this.label34.Text = "Magic";
            // 
            // infoRatingDefense
            // 
            this.infoRatingDefense.Location = new System.Drawing.Point(98, 41);
            this.infoRatingDefense.Maximum = 10;
            this.infoRatingDefense.Name = "infoRatingDefense";
            this.infoRatingDefense.Size = new System.Drawing.Size(222, 15);
            this.infoRatingDefense.Step = 1;
            this.infoRatingDefense.TabIndex = 13;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(6, 42);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(55, 19);
            this.label33.TabIndex = 12;
            this.label33.Text = "Defense";
            // 
            // infoRatingAttack
            // 
            this.infoRatingAttack.Location = new System.Drawing.Point(98, 20);
            this.infoRatingAttack.Maximum = 10;
            this.infoRatingAttack.Name = "infoRatingAttack";
            this.infoRatingAttack.Size = new System.Drawing.Size(222, 15);
            this.infoRatingAttack.Step = 1;
            this.infoRatingAttack.TabIndex = 11;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 21);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(45, 19);
            this.label31.TabIndex = 10;
            this.label31.Text = "Attack";
            // 
            // infoStats
            // 
            this.infoStats.AllowUserToAddRows = false;
            this.infoStats.AllowUserToDeleteRows = false;
            this.infoStats.AllowUserToResizeColumns = false;
            this.infoStats.AllowUserToResizeRows = false;
            this.infoStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoStats.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            this.infoStats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.infoStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.infoStats.Columns.AddRange(new DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn6});
            this.infoStats.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.infoStats.Location = new System.Drawing.Point(6, 106);
            this.infoStats.MultiSelect = false;
            this.infoStats.Name = "infoStats";
            this.infoStats.ReadOnly = true;
            this.infoStats.RowHeadersVisible = false;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.infoStats.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.infoStats.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.infoStats.ShowCellErrors = false;
            this.infoStats.ShowCellToolTips = false;
            this.infoStats.ShowEditingIcon = false;
            this.infoStats.ShowRowErrors = false;
            this.infoStats.Size = new System.Drawing.Size(314, 223);
            this.infoStats.TabIndex = 9;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Base1";
            dataGridViewCellStyle5.NullValue = null;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn2.HeaderText = "Base";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Level1";
            this.dataGridViewTextBoxColumn3.HeaderText = "+Level";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Level18";
            this.dataGridViewTextBoxColumn6.HeaderText = "Level 18";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 70;
            // 
            // ChampionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gInfoAbilities);
            this.Controls.Add(this.gInfoSkins);
            this.Controls.Add(this.gInfoStatistics);
            this.Controls.Add(this.panel1);
            this.Name = "ChampionCtrl";
            this.Size = new System.Drawing.Size(891, 483);
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.panel1.ResumeLayout(false);
            this.gInfoInformation.ResumeLayout(false);
            this.gInfoInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoIcon)).EndInit();
            this.gInfoAbilities.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconR)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconE)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconW)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconQ)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoAbilityIconP)).EndInit();
            this.gInfoSkins.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.infoPortrait)).EndInit();
            this.gInfoStatistics.ResumeLayout(false);
            this.gInfoStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoStats)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetroFramework.Controls.MetroComboBox infoList;
        private System.Windows.Forms.GroupBox gInfoInformation;
        private MetroTextBox infoOpponentTips;
        private MetroTextBox infoTips;
        private MetroLabel label30;
        private MetroLabel label29;
        private MetroLabel label28;
        private MetroTextBox infoDescription;
        private MetroLabel infoTags;
        private System.Windows.Forms.PictureBox infoIcon;
        private MetroFramework.Controls.MetroLabel infoDisplayName;
        private MetroFramework.Controls.MetroLabel infoTitle;
        private System.Windows.Forms.GroupBox gInfoAbilities;
        private System.Windows.Forms.Panel panel5;
        private MetroTextBox infoAbilityEffectR;
        private MetroLabel infoAbilityNameR;
        private MetroLabel infoAbilityCooldownR;
        private System.Windows.Forms.PictureBox infoAbilityIconR;
        private MetroLabel infoAbilityCostR;
        private System.Windows.Forms.Panel panel4;
        private MetroTextBox infoAbilityEffectE;
        private MetroLabel infoAbilityNameE;
        private MetroLabel infoAbilityCooldownE;
        private System.Windows.Forms.PictureBox infoAbilityIconE;
        private MetroLabel infoAbilityCostE;
        private System.Windows.Forms.Panel panel3;
        private MetroTextBox infoAbilityEffectW;
        private MetroLabel infoAbilityNameW;
        private MetroLabel infoAbilityCooldownW;
        private System.Windows.Forms.PictureBox infoAbilityIconW;
        private MetroLabel infoAbilityCostW;
        private System.Windows.Forms.Panel panel2;
        private MetroTextBox infoAbilityEffectQ;
        private MetroLabel infoAbilityNameQ;
        private MetroLabel infoAbilityCooldownQ;
        private System.Windows.Forms.PictureBox infoAbilityIconQ;
        private MetroLabel infoAbilityCostQ;
        private System.Windows.Forms.Panel panel6;
        private MetroTextBox infoAbilityEffectP;
        private System.Windows.Forms.PictureBox infoAbilityIconP;
        private MetroLabel infoAbilityNameP;
        private System.Windows.Forms.GroupBox gInfoSkins;
        private System.Windows.Forms.PictureBox infoPortrait;
        private MetroComboBox infoSkinsList;
        private System.Windows.Forms.GroupBox gInfoStatistics;
        private MetroProgressBar infoRatingDifficulty;
        private MetroLabel label35;
        private MetroProgressBar infoRatingMagic;
        private MetroLabel label34;
        private MetroProgressBar infoRatingDefense;
        private MetroLabel label33;
        private MetroProgressBar infoRatingAttack;
        private MetroLabel label31;
        private MetroGrid infoStats;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

    }
}
