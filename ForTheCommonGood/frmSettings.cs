using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmSettings: Form
    {
        string localWikiDataFile = "";
        string localWikiDataDomain = "en.wikipedia";

        bool loginOnly;

        private struct HostedLocalWikiDataEntry
        {
            public string displayName;
            public string uri;

            public HostedLocalWikiDataEntry(string displayName, string uri)
            {
                this.displayName = displayName;
                this.uri = uri;
            }

            public override string ToString()
            {
                return displayName;
            }

            public string GetFull()
            {
                return displayName + "|" + uri;
            }
        }

        public frmSettings(bool loginOnly, bool showInTaskbar)
        {
            this.loginOnly = loginOnly;
            
            InitializeComponent();
            lblVersion.Text = Localization.GetString("Version_Format", Application.ProductVersion.ToString());

            grpLocal.Text = Localization.GetString("LocalLoginDetails_Label");
            lblCommonsUserName.Text = lblLocalUserName.Text = Localization.GetString("UserName_TextBox");
            lblCommonsPassword.Text = lblLocalPassword.Text = Localization.GetString("Password_TextBox");
            lblLocalDomain.Text = Localization.GetString("LocalWiki_TextBox");
            chkLocalSysop.Text = Localization.GetString("IAmLocalAdministrator_CheckBox");
            lblLocalSysopHint.Text = Localization.GetString("LocalAdministratorExplanation_Label");
            chkSaveCreds.Text = Localization.GetString("SavePasswordsToDisk_CheckBox");
            chkUseHttps.Text = Localization.GetString("UseHttps_CheckBox");
            chkAutoUpdate.Text = Localization.GetString("CheckForUpdates_CheckBox");
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
            optLocalDataFile.Tag = Localization.GetString("LocalWikiDataUseLoaded_Option").TrimEnd() + " ";
            optLocalDataFile.Text = (string) optLocalDataFile.Tag + Localization.GetString("LocalWikiDataNoneSelected_Option");
            optLocalDataHosted.Text = Localization.GetString("LocalWikiDataUseHosted_Option");
            optLocalDataDefault.Text = Localization.GetString("LocalWikiDataUseDefault_Option");
            btnOK.Text = Localization.GetString("OK_Button");
            btnCancel.Text = Localization.GetString("Cancel_Button");

            if (Settings.CommonsDomain == Settings.DefaultCommonsDomain)
            {
                grpCommons.Text = Localization.GetString("CommonsLoginDetails_Label");
                chkLocalSameAsCommons.Text = Localization.GetString("SameCredentialsAsCommons_CheckBox");
                chkOpenBrowserAutomatically.Text = Localization.GetString("OpenFilePageAutomatically_CheckBox");
            }
            else
            {
                grpCommons.Text = Localization.GetString("TargetLoginDetails_Label", Settings.CommonsDomain);
                chkLocalSameAsCommons.Text = Localization.GetString("SameCredentialsAsTarget_CheckBox", Settings.CommonsDomain);
                chkOpenBrowserAutomatically.Text = Localization.GetString("OpenTargetFilePageAutomatically_CheckBox", Settings.CommonsDomain);
            }

            if (loginOnly)
            {
                txtCommonsUserName.Enabled = txtLocalDomain.Enabled = txtLocalUserName.Enabled =
                    chkLocalSameAsCommons.Enabled = lblLocalSysopHint.Visible = chkLocalSysop.Visible =
                    panRightSide.Visible = panVertLine.Visible = false;
                Text = Localization.GetString("LogIn_WindowTitle");
                ClientSize = new Size(grpCommons.Width + 18, ClientSize.Height + 40);
                btnCancel.Top = btnOK.Top = grpLocal.Bottom + 8;
            }
            else
            {
                Text = Localization.GetString("Settings_WindowTitle");
            }

            if (showInTaskbar)
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                ShowInTaskbar = true;
                Text += " - " + Application.ProductName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!loginOnly && localWikiDataDomain != "" && localWikiDataDomain != txtLocalDomain.Text)
            {
                if (MessageBox.Show(this, Localization.GetString("LocalWikiDataWrongDomain", localWikiDataDomain) + "\n\n" +
                    Localization.GetString("ContinueAnyway"), Application.ProductName, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            }

            Settings.LocalDomain = txtLocalDomain.Text;
            Settings.LocalUserName = chkLocalSameAsCommons.Checked ? txtCommonsUserName.Text : txtLocalUserName.Text;
            Settings.LocalPassword = chkLocalSameAsCommons.Checked ? txtCommonsPassword.Text : txtLocalPassword.Text;
            Settings.LocalSysop = chkLocalSysop.Checked;
            Settings.CommonsUserName = txtCommonsUserName.Text;
            Settings.CommonsPassword = txtCommonsPassword.Text;
            Settings.SaveCreds = chkSaveCreds.Checked;

            if (!loginOnly)
            {
                Settings.UseHttps = chkUseHttps.Checked;
                Settings.LogTransfers = chkLogTransfers.Checked;
                Settings.OpenBrowserAutomatically = chkOpenBrowserAutomatically.Checked;
                Settings.OpenBrowserLocal = chkOpenBrowserLocal.Checked;
                Settings.AutoUpdate = chkAutoUpdate.Checked;
                Settings.LocalWikiData = optLocalDataFile.Checked ? localWikiDataFile : "";
                if (optLocalDataHosted.Checked && cboLocalDataHosted.SelectedIndex != -1 &&
                    cboLocalDataHosted.SelectedItem is HostedLocalWikiDataEntry)
                {
                    Settings.LocalWikiDataHosted = ((HostedLocalWikiDataEntry) cboLocalDataHosted.SelectedItem).GetFull();
                }
                else
                {
                    Settings.LocalWikiDataHosted = "";
                }
            }

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

            if (!loginOnly)
            {
                chkUseHttps.Checked = Settings.UseHttps;
                chkLogTransfers.Checked = Settings.LogTransfers;
                chkOpenBrowserAutomatically.Checked = Settings.OpenBrowserAutomatically;
                chkOpenBrowserLocal.Checked = Settings.OpenBrowserLocal;
                chkAutoUpdate.Checked = Settings.AutoUpdate;

                if (Settings.LocalWikiData != "")
                {
                    optLocalDataFile.Checked = true;
                    localWikiDataFile = Settings.LocalWikiData;
                    localWikiDataDomain = LocalWikiData.LocalDomain;
                    optLocalDataFile.Text = (string) optLocalDataFile.Tag + localWikiDataDomain;
                }
                else if (Settings.LocalWikiDataHosted != "")
                {
                    optLocalDataHosted.Checked = true;
                    localWikiDataDomain = Settings.LocalWikiDataHosted.Substring(0, Settings.LocalWikiDataHosted.IndexOf("|"));
                }
                else
                {
                    optLocalDataDefault.Checked = true;
                }

                optLocalDataXyz_CheckChange(null, null);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://en.wikipedia.org/wiki/WP:FTCG");
            }
            catch (Exception)
            {
                MessageBox.Show(this, Localization.GetString("LinkVisitFailed"), Application.ProductName);
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
                MessageBox.Show(this, Localization.GetString("LinkVisitFailed"), Application.ProductName);
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
                MessageBox.Show(this, Localization.GetString("FileNotFoundLocal", openFileDialog1.FileName), Application.ProductName);
                return;
            }
            localWikiDataFile = File.ReadAllText(openFileDialog1.FileName, Encoding.UTF8);

            localWikiDataDomain = Regex.Match(localWikiDataFile, "LocalDomain=([^\r\n]+)[\r\n]").Groups[1].Value;
            optLocalDataFile.Text = (string) optLocalDataFile.Tag + localWikiDataDomain;
            if (localWikiDataDomain != txtLocalDomain.Text)
            {
                MessageBox.Show(this, Localization.GetString("LocalWikiDataWrongDomain", localWikiDataDomain.Trim()), Application.ProductName);
            }
        }

        private void optLocalDataXyz_CheckChange(object sender, EventArgs e)
        {
            btnLocalDataLoad.Enabled = optLocalDataFile.Checked;
            cboLocalDataHosted.Enabled = optLocalDataHosted.Checked;

            if (optLocalDataFile.Checked && localWikiDataFile != "")
            {
                localWikiDataDomain = Regex.Match(localWikiDataFile, "LocalDomain=([^\r\n]+)[\r\n]").Groups[1].Value;
            }
            else if (optLocalDataHosted.Checked)
            {
                if (cboLocalDataHosted.Items.Count == 0)
                {
                    cboLocalDataHosted.ForeColor = SystemColors.GrayText;
                    cboLocalDataHosted.Items.Add("Loading...");
                    cboLocalDataHosted.SelectedIndex = 0;
                    hostedListLoader.RunWorkerAsync();
                }
                else if (cboLocalDataHosted.SelectedIndex != -1)
                {
                    localWikiDataDomain = cboLocalDataHosted.SelectedItem.ToString();
                }
                else
                {
                    localWikiDataDomain = "";
                }
            }
            else
            {
                localWikiDataDomain = "";
            }
        }

        private void hostedListLoader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            WebClient loader = new WebClient();
            loader.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
            try
            {
                string result = loader.DownloadString(new Uri("http://en.wikipedia.org/w/index.php?action=raw&ctype=text/css&title=User:This,%20that%20and%20the%20other/FtCG%20local%20wiki%20data%20files.css"));
                Invoke((MethodInvoker) delegate()
                {
                    try
                    {
                        cboLocalDataHosted.Items.Clear();
                        cboLocalDataHosted.ForeColor = SystemColors.WindowText;
                        foreach (string line in result.Split('\n'))
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length == 2)
                            {
                                HostedLocalWikiDataEntry item = new HostedLocalWikiDataEntry(parts[0], parts[1]);
                                cboLocalDataHosted.Items.Add(item);
                                if (Settings.LocalWikiDataHosted.StartsWith(item.displayName + "|"))
                                    cboLocalDataHosted.SelectedItem = item;
                            }
                        }
                        if (cboLocalDataHosted.Items.Count > 0 && cboLocalDataHosted.SelectedIndex == -1)
                            cboLocalDataHosted.SelectedIndex = 0;
                    }
                    catch (Exception)
                    {
                        MorebitsDotNet.DefaultErrorHandler(Localization.GetString("FailedToLoadHostedLocalWikiDataList"));
                    }
                });
            }
            catch (Exception ex)
            {
                if (!(ex is InvalidOperationException))
                    MorebitsDotNet.DefaultErrorHandler(Localization.GetString("FailedToLoadHostedLocalWikiDataList"));
            }
        }

        private void cboLocalDataHosted_SelectedIndexChanged(object sender, EventArgs e)
        {
            localWikiDataDomain = cboLocalDataHosted.SelectedItem.ToString();
        }
    }

    internal static class Settings
    {
        public static string LocalDomain { get; set; }
        public static string LocalUserName { get; set; }
        public static string LocalPassword { get; set; }
        public static bool LocalSysop { get; set; }

        public static string CommonsDomain { get; set; }
        public static string CommonsUserName { get; set; }
        public static string CommonsPassword { get; set; }
        public static bool SaveCreds { get; set; }

        public static bool UseHttps { get; set; }
        public static bool LogTransfers { get; set; }
        public static bool OpenBrowserAutomatically { get; set; }
        public static bool OpenBrowserLocal { get; set; }
        public static bool AutoUpdate { get; set; }

        public static string LocalWikiData { get; set; }
        public static string LocalWikiDataHosted { get; set; }

        public static string CurrentSourceOption { get; set; }
        public static string SourceCategory { get; set; }
        public static string SourceTextFile { get; set; }

        const string settingsFileName = "ForTheCommonGood.cfg";

        public static string DefaultLocalDomain { get { return "en.wikipedia"; } }
        public static string DefaultCommonsDomain { get { return "commons.wikimedia"; } }

        static Settings()
        {
            LocalDomain = DefaultLocalDomain;
            CommonsDomain = DefaultCommonsDomain;
            LocalUserName = LocalPassword = CommonsPassword = CommonsUserName =
                LocalWikiData = LocalWikiDataHosted =
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
                    if (l.StartsWith("CommonsDomain="))
                        Settings.CommonsDomain = l.Substring("CommonsDomain=".Length);
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
                    if (l.StartsWith("LocalWikiDataHosted="))
                        Settings.LocalWikiDataHosted = l.Substring("LocalWikiDataHosted=".Length);
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
                    "LocalWikiDataHosted=" + Settings.LocalWikiDataHosted,
                    "CurrentSourceOption=" + Settings.CurrentSourceOption,
                    "SourceCategory=" + Settings.SourceCategory,
                    "SourceTextFile=" + Settings.SourceTextFile,
                });
                if (Settings.CommonsDomain != Settings.DefaultCommonsDomain)
                    lines.Add("CommonsDomain=" + Settings.CommonsDomain);
                File.WriteAllLines(settingsFileName, lines.ToArray(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localization.GetString("SaveSettingsError") + "\n\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
