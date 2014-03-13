using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ForTheCommonGood
{
    public partial class frmSettings: Form
    {
        string localWikiDataFile = "";

        public frmSettings(bool loginOnly)
        {
            InitializeComponent();
            lblVersion.Text = Localization.GetString("Version_Format", Application.ProductVersion.ToString());

            grpCommons.Text = Localization.GetString("CommonsLoginDetails_Label");
            grpLocal.Text = Localization.GetString("LocalLoginDetails_Label");
            lblCommonsUserName.Text = lblLocalUserName.Text = Localization.GetString("UserName_TextBox");
            lblCommonsPassword.Text = lblLocalPassword.Text = Localization.GetString("Password_TextBox");
            lblLocalDomain.Text = Localization.GetString("LocalWiki_TextBox");
            chkLocalSameAsCommons.Text = Localization.GetString("SameCredentialsAsCommons_CheckBox");
            chkLocalSysop.Text = Localization.GetString("IAmLocalAdministrator_CheckBox");
            lblLocalSysopHint.Text = Localization.GetString("LocalAdministratorExplanation_Label");
            chkSaveCreds.Text = Localization.GetString("SavePasswordsToDisk_CheckBox");
            chkUseHttps.Text = Localization.GetString("UseHttps_CheckBox");
            chkAutoUpdate.Text = Localization.GetString("CheckForUpdates_CheckBox");
            chkOpenBrowserAutomatically.Text = Localization.GetString("OpenFilePageAutomatically_CheckBox");
            chkOpenBrowserLocal.Text = Localization.GetString("OpenLocalFilePageAutomatically_CheckBox");
            chkLogTransfers.Text = Localization.GetString("LogTransfers_CheckBox");
            lnkThisThatOther.Text = Localization.GetString("Author_Label") + " This, that and the other";
            lnkThisThatOther.LinkArea = new LinkArea(lnkThisThatOther.Text.Length - "This, that and the other".Length,
                lnkThisThatOther.Text.Length - 1);
            lblLicense.Text = Localization.GetString("PublicDomain_Label");
            openFileDialog1.Title = Localization.GetString("OpenTextFile_WindowTitle");
            grpLocalData.Text = Localization.GetString("LocalWikiData_Label");
            lblLocalDataHint.Text = Localization.GetString("LocalWikiDataHint_Label");
            btnLocalDataLoad.Text = Localization.GetString("LocalWikiDataLoad_Button");
            btnLocalDataReset.Text = Localization.GetString("LocalWikiDataReset_Button");
            lblLocalCurrentLabel.Text = Localization.GetString("LocalWikiDataCurrent_Label");
            btnOK.Text = Localization.GetString("OK_Button");
            btnCancel.Text = Localization.GetString("Cancel_Button");

            if (loginOnly)
            {
                txtCommonsUserName.Enabled = txtLocalDomain.Enabled = txtLocalUserName.Enabled =
                    chkLocalSameAsCommons.Enabled = lblLocalSysopHint.Visible = chkLocalSysop.Visible = 
                    panRightSide.Visible = false;
                Text = Localization.GetString("LogIn_WindowTitle") + " - " + Application.ProductName;
                ClientSize = new Size(grpCommons.Width + 18, ClientSize.Height + 32);
                ShowInTaskbar = true;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            }
            else
            {
                Text = Localization.GetString("Settings_WindowTitle");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.LocalDomain = txtLocalDomain.Text;
            Settings.LocalUserName = chkLocalSameAsCommons.Checked ? txtCommonsUserName.Text : txtLocalUserName.Text;
            Settings.LocalPassword = chkLocalSameAsCommons.Checked ? txtCommonsPassword.Text : txtLocalPassword.Text;
            Settings.LocalSysop = chkLocalSysop.Checked;
            Settings.CommonsUserName = txtCommonsUserName.Text;
            Settings.CommonsPassword = txtCommonsPassword.Text;
            Settings.SaveCreds = chkSaveCreds.Checked;
            Settings.UseHttps = chkUseHttps.Checked;
            Settings.LogTransfers = chkLogTransfers.Checked;
            Settings.OpenBrowserAutomatically = chkOpenBrowserAutomatically.Checked;
            Settings.OpenBrowserLocal = chkOpenBrowserLocal.Checked;
            Settings.AutoUpdate = chkAutoUpdate.Checked;
            Settings.LocalWikiData = localWikiDataFile;

            // save settings
            Settings.WriteSettings();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Settings.LocalDomain != null)
                txtLocalDomain.Text = Settings.LocalDomain;
            if (Settings.LocalUserName == Settings.CommonsUserName &&
                Settings.LocalPassword == Settings.CommonsPassword)// && Settings.LocalUserName != "")
            {
                chkLocalSameAsCommons.Checked = true;
                chkLocalSameAsCommons_CheckedChanged(null, null);
            }
            else
            {
                if (Settings.LocalUserName != null)
                    txtLocalUserName.Text = Settings.LocalUserName;
                if (Settings.LocalPassword != null)
                    txtLocalPassword.Text = Settings.LocalPassword;
            }
            chkLocalSysop.Checked = Settings.LocalSysop;
            if (Settings.CommonsUserName != null)
                txtCommonsUserName.Text = Settings.CommonsUserName;
            if (Settings.CommonsPassword != null)
                txtCommonsPassword.Text = Settings.CommonsPassword;
            chkSaveCreds.Checked = Settings.SaveCreds;
            chkUseHttps.Checked = Settings.UseHttps;
            chkLogTransfers.Checked = Settings.LogTransfers;
            chkOpenBrowserAutomatically.Checked = Settings.OpenBrowserAutomatically;
            chkOpenBrowserLocal.Checked = Settings.OpenBrowserLocal;
            chkAutoUpdate.Checked = Settings.AutoUpdate;

            if (Settings.LocalWikiData != "")
            {
                localWikiDataFile = Settings.LocalWikiData;
                lblLocalCurrent.Text = LocalWikiData.LocalDomain;
            }
            else
                lblLocalCurrent.Text = "en.wikipedia (Default)"; // TODO: localize
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://en.wikipedia.org/wiki/WP:FTCG");
            }
            catch (Exception)
            {
                MessageBox.Show(Localization.GetString("LinkVisitFailed"));
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://en.wikipedia.org/wiki/User_talk:This,_that_and_the_other");
            }
            catch (Exception)
            {
                MessageBox.Show(Localization.GetString("LinkVisitFailed"));
            }
        }

        private void chkLocalSameAsCommons_CheckedChanged(object sender, EventArgs e)
        {
            txtLocalUserName.Enabled = txtLocalPassword.Enabled = !chkLocalSameAsCommons.Checked;
            if (!chkLocalSameAsCommons.Checked)
            {
                txtLocalUserName.Text = txtCommonsUserName.Text;
                txtLocalPassword.Text = txtCommonsPassword.Text;
            }
            else
            {
                txtLocalUserName.Text = txtLocalPassword.Text = "";
            }
        }

        private void btnLocalDataLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.Cancel)
                return;
            if (!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show(Localization.GetString("FileNotFoundLocal", openFileDialog1.FileName));
                return;
            }
            localWikiDataFile = File.ReadAllText(openFileDialog1.FileName, Encoding.UTF8);
            lblLocalCurrent.Text = Regex.Match(localWikiDataFile, "LocalDomain=([^\r\n]+)[\r\n]").Groups[1].Value;
            if (lblLocalCurrent.Text != txtLocalDomain.Text)
            {
                MessageBox.Show(Localization.GetString("LocalWikiDataWrongDomain", lblLocalCurrent.Text.Trim()));
            }
        }

        private void btnLocalDataReset_Click(object sender, EventArgs e)
        {
            localWikiDataFile = "";
            lblLocalCurrent.Text = "en.wikipedia (Default)";
            if ("en.wikipedia" != txtLocalDomain.Text)
            {
                MessageBox.Show(Localization.GetString("LocalWikiDataReset", "en.wikipedia"));
            }
        }
    }

    internal static class Settings
    {
        public static string LocalDomain { get; set; }
        public static string LocalUserName { get; set; }
        public static string LocalPassword { get; set; }
        public static bool LocalSysop { get; set; }
        public static string CommonsUserName { get; set; }
        public static string CommonsPassword { get; set; }
        public static bool SaveCreds { get; set; }

        public static bool UseHttps { get; set; }
        public static bool LogTransfers { get; set; }
        public static bool OpenBrowserAutomatically { get; set; }
        public static bool OpenBrowserLocal { get; set; }
        public static bool AutoUpdate { get; set; }

        public static string LocalWikiData { get; set; }

        public static string CurrentSourceOption { get; set; }
        public static string SourceCategory { get; set; }
        public static string SourceTextFile { get; set; }

        const string settingsFileName = "ForTheCommonGood.cfg";

        static Settings()
        {
            LocalDomain = "en.wikipedia";
            LocalUserName = LocalPassword = CommonsPassword = CommonsUserName =
                LocalWikiData =
                CurrentSourceOption = SourceCategory = SourceTextFile = "";
            LocalSysop = LogTransfers = OpenBrowserAutomatically = OpenBrowserLocal = false;
            SaveCreds = UseHttps = AutoUpdate = true;
        }

        public static void ReadSettings()
        {
            try
            {
                foreach (string l in File.ReadAllLines(settingsFileName, Encoding.UTF8))
                {
                    if (l.StartsWith("LocalDomain="))
                        Settings.LocalDomain = l.Substring("LocalDomain=".Length);
                    if (l.StartsWith("LocalUserName="))
                        Settings.LocalUserName = l.Substring("LocalUserName=".Length);
                    if (l.StartsWith("LocalPassword="))
                        Settings.LocalPassword = Encoding.UTF8.GetString(Convert.FromBase64String(l.Substring("LocalPassword=".Length)));
                    if (l.StartsWith("LocalSysop="))
                        Settings.LocalSysop = l.Substring("LocalSysop=".Length) == "true";
                    if (l.StartsWith("CommonsUserName="))
                        Settings.CommonsUserName = l.Substring("CommonsUserName=".Length);
                    if (l.StartsWith("CommonsPassword="))
                        Settings.CommonsPassword = Encoding.UTF8.GetString(Convert.FromBase64String(l.Substring("CommonsPassword=".Length)));
                    if (l.StartsWith("UseHttps="))
                        Settings.UseHttps = l.Substring("UseHttps=".Length) != "false";
                    if (l.StartsWith("LogTransfers="))
                        Settings.LogTransfers = l.Substring("LogTransfers=".Length) == "true";
                    if (l.StartsWith("OpenBrowserAutomatically="))
                        Settings.OpenBrowserAutomatically = l.Substring("OpenBrowserAutomatically=".Length) == "true";
                    if (l.StartsWith("OpenBrowserLocal="))
                        Settings.OpenBrowserLocal = l.Substring("OpenBrowserLocal=".Length) == "true";
                    if (l.StartsWith("AutoUpdate="))
                        Settings.AutoUpdate = l.Substring("AutoUpdate=".Length) != "false";
                    if (l.StartsWith("LocalWikiData="))
                        Settings.LocalWikiData = Encoding.UTF8.GetString(Convert.FromBase64String(l.Substring("LocalWikiData=".Length)));
                    if (l.StartsWith("CurrentSourceOption="))
                        Settings.CurrentSourceOption = l.Substring("CurrentSourceOption=".Length);
                    if (l.StartsWith("SourceCategory="))
                        Settings.SourceCategory = l.Substring("SourceCategory=".Length);
                    if (l.StartsWith("SourceTextFile="))
                        Settings.SourceTextFile = l.Substring("SourceTextFile=".Length);
                }
                Settings.SaveCreds = !(Settings.LocalPassword == "" && Settings.CommonsPassword == "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localization.GetString("ReadSettingsError") + "\n\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public static void WriteSettings()
        {
            try
            {
                List<String> lines = new List<string>(15);
                if (Settings.SaveCreds)
                    lines.AddRange(new string[] {
                        "LocalPassword=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Settings.LocalPassword)),
                        "CommonsPassword=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Settings.CommonsPassword)),
                    });
                lines.AddRange(new string[] {
                    "LocalDomain=" + Settings.LocalDomain,
                    "LocalUserName=" + Settings.LocalUserName,
                    "LocalSysop=" + (Settings.LocalSysop ? "true" : "false"),
                    "CommonsUserName=" + Settings.CommonsUserName,
                    "UseHttps=" + (Settings.UseHttps ? "true" : "false"),
                    "LogTransfers=" + (Settings.LogTransfers ? "true" : "false"),
                    "OpenBrowserAutomatically=" + (Settings.OpenBrowserAutomatically ? "true" : "false"),
                    "OpenBrowserLocal=" + (Settings.OpenBrowserLocal ? "true" : "false"),
                    "AutoUpdate=" + (Settings.AutoUpdate ? "true" : "false"),
                    "LocalWikiData=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(Settings.LocalWikiData)),
                    "CurrentSourceOption=" + Settings.CurrentSourceOption,
                    "SourceCategory=" + Settings.SourceCategory,
                    "SourceTextFile=" + Settings.SourceTextFile,
                });
                File.WriteAllLines(settingsFileName, lines.ToArray(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localization.GetString("SaveSettingsError") + "\n\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
