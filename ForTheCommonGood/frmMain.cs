using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace ForTheCommonGood
{
    public partial class frmMain: Form
    {
        // A tool strip renderer with no UI footprint (no bottom border)
        class SimpleToolStripRenderer: ToolStripSystemRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // do nothing
            }
        }

        // The old local filename of the currently loaded image, without the namespace prefix
        string CurrentFileName;

        // The indexes of each selected revision in the ImageInfos and ImageDatas arrays
        int[] SelectedRevisions;

        // The <ii> elements received from the API
        XmlNodeList ImageInfos;

        // Holds the binary data of each image to be transferred
        byte[][] ImageDatas;

        // Downloads the file into ImageDatas!
        WebClient ImageDataDownloader;

        // Stuff to support the "random file" feature
        enum FileSources
        {
            ManualInput,
            Category,
            TextFile
        }
        FileSources CurrentFileSource;
        FileSources RandomFileSource;
        string SourceTag;  // category name or name of text file

        List<string> TextFileCache = new List<string>();
        int RandomCurrentIndex;

        // Cached global resources
        Cursor ZoomInCursor;
        TextureBrush CheckerBrush;

        public frmMain()
        {
            InitializeComponent();

            // nicer default height - we need to keep it at MinimumSize in design view
            // to stop the main text boxes from being cut off
            Height = 720;

            panStatus.Tag = Color.FromArgb(30, 144, 255);
            panWarning.Tag = Color.FromArgb(178, 34, 34);

            ZoomInCursor = new Cursor(new MemoryStream(Properties.Resources.zoomin_cur));
            CheckerBrush = new TextureBrush(Properties.Resources.Checker_16x16, System.Drawing.Drawing2D.WrapMode.Tile);

            // load language file, if any
            Localization.Init();

            lblOriginalFilename.Text = Localization.GetString("OriginalFileName_TextBox");
            btnGo.Text = Localization.GetString("Go_Button");
            btnRandomFile.Text = Localization.GetString("RandomFile_Button");
            optOther.Text = Localization.GetString("OtherSource_Option");
            btnSettings.Text = Localization.GetString("Settings_Button");
            lblCommonsFileDesc.Text = Localization.GetString("FilePageOnCommons_TextBox");
            btnLinkify.Text = Localization.GetString("MakeSelectedTextIntoWikilink_Hyperlink");
            btnPreview.Text = Localization.GetString("PreviewCommonsWikitext_Hyperlink");
            btnPastRevisions.Text = Localization.GetString("SelectVersion_Button");
            lblViewExif.Text = Localization.GetString("ContainsExifMetadata_Label");
            btnViewExif.Text = Localization.GetString("ViewMetadata_Button");
            lblFileLinks.Text = Localization.GetString("ImageUsage_Label");
            lnkGoToFileLink.Text = Localization.GetString("Go_Button") + " >";
            lblNormName.Text = Localization.GetString("NewFilenameOnCommons_TextBox");
            chkIgnoreWarnings.Text = Localization.GetString("IgnoreWarnings_CheckBox");
            chkDeleteAfter.Text = Localization.GetString("TagLocalFileWithNowCommons_Label");
            btnTransfer.Text = Localization.GetString("Transfer_Button");
            lnkCommonsFile.Text = Localization.GetString("ViewFilePageOnWikimediaCommons_Hyperlink");
            //lblCategoryHint.Text = Localization.GetString("DontForgetToCategorize_Label") + " " + Localization.GetString("HotcatHint_Label");
            lnkGoogleImageSearch.Text = Localization.GetString("GoogleCheck_Hyperlink");
            lblDeclineTransfer.Text = Localization.GetString("IfIneligibleEditManually_Label");
            lblExifNotice.Text = Localization.GetString("NoExifRotation_Label");
            lblStatus.Text = Localization.GetString("Loading");

            // prepare welcome text
            StringBuilder welcome = new StringBuilder();
            welcome.AppendLine("");
            welcome.AppendLine(" == " + Localization.GetString("WelcomeToFtcg_Title") + " ==");
            welcome.AppendLine("");
            welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_IsGood1"));
            if (Localization.GetString("WelcomeToFtcg_IsGood2") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_IsGood2"));
            welcome.AppendLine("");
            welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_GetStarted1"));
            if (Localization.GetString("WelcomeToFtcg_GetStarted2") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_GetStarted2"));
            if (Localization.GetString("WelcomeToFtcg_GetStarted3") != "")
                welcome.AppendLine("    " + Localization.GetString("WelcomeToFtcg_GetStarted3"));
            if (Localization.GetString("WelcomeToFtcg_GetStarted4") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_GetStarted4"));
            if (Localization.GetString("WelcomeToFtcg_GetStarted5") != "")
                welcome.AppendLine("    " + Localization.GetString("WelcomeToFtcg_GetStarted5"));
            welcome.AppendLine("");
            welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_NotABludge1"));
            if (Localization.GetString("WelcomeToFtcg_NotABludge2") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_NotABludge2"));
            if (Localization.GetString("WelcomeToFtcg_NotABludge3") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_NotABludge3"));
            if (Localization.GetString("WelcomeToFtcg_NotABludge4") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_NotABludge4"));
            if (Localization.GetString("WelcomeToFtcg_NotABludge5") != "")
                welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_NotABludge5"));
            welcome.AppendLine("");
            welcome.AppendLine(" " + Localization.GetString("WelcomeToFtcg_Enjoy"));
            txtLocalText.Text = welcome.ToString();

            toolBarLinks.Renderer = new SimpleToolStripRenderer();

            // time to load settings
            if (File.Exists("ForTheCommonGood.cfg"))
            {
                Settings.ReadSettings();
                if (Settings.SaveCreds == false)
                {
                    frmSettings set = new frmSettings(Settings.LocalUserName != "", true);
                    set.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show(Localization.GetString("Welcome1") + "\n\n" + Localization.GetString("Welcome2"),
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                new frmSettings(false, true).ShowDialog(this);
            }

            InitSettings();

            if (PlatformSpecific.IsMono())
            {
                tableLayoutPanel1.Resize += delegate(object sender, EventArgs e)
                {
                    tableLayoutPanel1.SuspendLayout();
                    txtLocalText.Height = panCommonsText.Height = 0;
                    tableLayoutPanel1.ResumeLayout(true);
                };
                btnViewExif.Visible = lblViewExif.Visible = btnPastRevisions.Visible =
                    lblPastRevisions.Visible = panWarning.Visible = true;
                Load += delegate(object sender, EventArgs e)
                {
                    btnViewExif.Visible = lblViewExif.Visible = btnPastRevisions.Visible =
                         lblPastRevisions.Visible = panWarning.Visible = false;
                };
            }
        }

        // Miscellaneous helper methods
        // ============================

        void ErrorHandler(String msg)
        {
            MorebitsDotNet.DefaultErrorHandler(msg);
            EnableForm(true);
        }

        void ErrorHandler(String msg, MessageBoxIcon icon)
        {
            MorebitsDotNet.DefaultErrorHandler(msg, icon);
            EnableForm(true);
        }

        void SetTransferButtonDownloading(bool downloading)
        {
            Invoke((MethodInvoker) delegate()
            {
                btnTransfer.Enabled = !downloading;
                btnTransfer.Text = downloading ? Localization.GetString("Downloading") :
                    Localization.GetString("Transfer_Button");
            });
        }

        static string FormatTimestamp(XmlNode n)
        {
            return DateTime.Parse(n.Attributes["timestamp"].Value).ToUniversalTime().ToString("HH:mm, d MMMM yyyy", DateTimeFormatInfo.InvariantInfo);
        }

        static string FormatIsoDate(XmlNode n)
        {
            return DateTime.Parse(n.Attributes["timestamp"].Value).ToUniversalTime().ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
        }

        static string FormatDimensions(XmlNode n)
        {
            if (n.Attributes["width"].Value == "0")
                return n.Attributes["size"].Value + " bytes";  // not formatted, since it may be some weird format .NET doesn't know about
            else
                return int.Parse(n.Attributes["width"].Value).ToString("n0", CultureInfo.InvariantCulture) + " × " +
                    int.Parse(n.Attributes["height"].Value).ToString("n0", CultureInfo.InvariantCulture) + " (" +
                    int.Parse(n.Attributes["size"].Value).ToString("n0", CultureInfo.InvariantCulture) + " bytes)";
        }

        static string GetCurrentLanguageCode()
        {
            string result = Settings.LocalDomain.Substring(0, Settings.LocalDomain.IndexOf('.'));
            if (result == "simple")
                return "en";
            if (result.Length > 3 && !result.Contains("-"))  // meta, species, etc
                return "";  // don't emit a language code template
            return result;
        }

        static string GetCurrentInterwikiPrefix(bool forUvTemplate)
        {
            string result = null;
            switch (Settings.LocalDomain.Substring(Settings.LocalDomain.IndexOf('.') + 1))
            {
                case "wikipedia":
                    result = forUvTemplate ? "w:" : ":";
                    break;
                case "wiktionary":
                    result = "wikt:";
                    break;
                case "wikiquote":
                    result = "q:";
                    break;
                case "wikinews":
                    result = "n:";
                    break;
                case "wikibooks":
                    result = "b:";
                    break;
                case "wikisource":
                    result = "s:";
                    break;
                case "wikiversity":
                    result = "v:";
                    break;
                case "wikivoyage":
                    result = "voy:";
                    break;
                case "wikimedia":
                    break;
                case "mediawiki":
                    return "mw";
            }
            return result + Settings.LocalDomain.Substring(0, Settings.LocalDomain.IndexOf('.'));
        }

        // Potential problem/success box infrastructure
        // ============================================

        enum WarningBoxType
        {
            Warning,
            Success
        }

        void AddWarningCore(Control ctl, WarningBoxType type)
        {
            Invoke((MethodInvoker) delegate()
            {
                if (!panWarning.Visible)
                {
                    panWarning.Visible = true;
                    if (type == WarningBoxType.Success)
                    {
                        panRoot.BackColor = Color.FromArgb(236, 255, 236);
                        icoWarning.Image = Properties.Resources.Information_icon4;
                        lblWarningHeading.ForeColor = Color.Green;
                        lblWarningHeading.Text = Localization.GetString("Successful_Label");
                        panWarning.BackColor = Color.FromArgb(222, 245, 222);
                        panWarning.Tag = Color.FromArgb(28, 134, 28);
                    }
                    else
                    {
                        panRoot.BackColor = Color.FromArgb(246, 255, 192);
                        icoWarning.Image = Properties.Resources.Imbox_content;
                        lblWarningHeading.ForeColor = Color.Firebrick;
                        lblWarningHeading.Text = Localization.GetString("PotentialProblems_Label");
                        panWarning.BackColor = Color.FromArgb(255, 238, 238);
                        panWarning.Tag = Color.FromArgb(178, 34, 34);
                    }
                }
                ctl.Margin = new Padding(3, 0, 3, 3);
                panWarningTexts.Controls.Add(ctl);
            });
        }

        void AddWarning(string text, WarningBoxType type)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Text = text;
            AddWarningCore(label, type);
        }

        void AddWarningLink(string formatText, string linkText, LinkLabelLinkClickedEventHandler linkEvent, WarningBoxType type)
        {
            LinkLabel label = new LinkLabel();
            label.AutoSize = true;
            label.Text = String.Format(formatText, linkText);
            label.Links.Clear();
            label.Links.Add(formatText.IndexOf("{0}"), linkText.Length);
            label.LinkBehavior = LinkBehavior.HoverUnderline;
            label.LinkClicked += linkEvent;
            AddWarningCore(label, type);
        }

        void ClearWarnings()
        {
            panWarning.Visible = false;
            panRoot.BackColor = DefaultBackColor;
            panWarningTexts.Controls.Clear();
        }

        // Fundamentals of FtCG
        // ====================

        void EnableForm(bool enabled)
        {
            Invoke((MethodInvoker) delegate()
            {
                panRoot.Enabled = enabled;
                panStatus.Visible = !enabled;
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentFileSource = FileSources.ManualInput;
            try
            {
                DownloadAndProcess();
            }
            finally
            {
                EnableForm(true);
            }
        }

        void DownloadAndProcess()
        {
            Invoke((MethodInvoker) delegate()
            {
                // enable those controls which are initially disabled
                btnTransfer.Enabled = txtNormName.Enabled = chkDeleteAfter.Enabled =
                    chkIgnoreWarnings.Enabled = toolBarLinks.Enabled = coolCat.Enabled = true;

                txtLocalText.Text = txtCommonsText.Text = lblName.Text = lblRevision.Text = lblDimensions.Text = "";
                coolCat.ClearCategories();
                pictureBox1.Image = null;
                pictureBox1.Cursor = Cursors.Default;
                lblPastRevisions.Visible = btnPastRevisions.Visible = lblViewExif.Visible =
                    btnViewExif.Visible = false;
                lstFileLinks.ForeColor = SystemColors.GrayText;
                lstFileLinks.Items.Clear();
                chkIgnoreWarnings.Checked = false;
                ClearWarnings();
                lnkCommonsFile.Enabled = lnkLocalFile.Enabled = lnkGoogleImageSearch.Enabled =
                    lnkGoToFileLink.Enabled = false;
                SetTransferButtonDownloading(true);

                textBox1.Text = textBox1.Text.Trim();
            });
            if (ImageDataDownloader != null)
                ImageDataDownloader.CancelAsync();

            CurrentFileName = Regex.Replace(textBox1.Text, @"^\w+:", "", RegexOptions.IgnoreCase);

            EnableForm(false);

            ImageInfos = null;
            ImageDatas = null;
            string text = "";
            MorebitsDotNet.ActionCompleted sentry = new MorebitsDotNet.ActionCompleted(2);
            sentry.Done += new MorebitsDotNet.ActionCompleted.Action(delegate()
            {
                if (ImageInfos == null)
                    return;  // too much on at once

                // identify potential problems
                string textLowercase = text.ToLower();
                List<string> potentialProblems = new List<string>();
                if (Regex.IsMatch(text, "{{((db-)?now ?commons|" + Regex.Escape(LocalWikiData.NowCommonsTag) + ")", RegexOptions.IgnoreCase))
                {
                    potentialProblems.Add("• " + Localization.GetString("NowCommonsPotentialProblem"));
                    if (CurrentFileSource == FileSources.Category)
                    {
                        RandomBlacklist.Add(RandomCurrentIndex);  // don't turn up this file again
                        RandomImage(null, null);
                        return;
                    }
                }
                foreach (LocalWikiData.PotentialProblem problem in LocalWikiData.PotentialProblems)
                {
                    try
                    {
                        if (problem.IsRegex ?
                            Regex.IsMatch(text, problem.Test, RegexOptions.IgnoreCase) :
                            textLowercase.Contains(problem.Test.ToLower()))
                        {
                            potentialProblems.Add("• " + problem.Message);
                        }
                    }
                    catch (ArgumentException e)
                    {
                        ErrorHandler(Localization.GetString("LocalWikiDataError") + "\n\n" +
                            Localization.GetString("LocalWikiDataRegexError", "PotentialProblem IfRegex") + "\n\n" + e.Message);
                    }
                }

                if (potentialProblems.Count > 0)
                {
                    Invoke((MethodInvoker) delegate()
                    {
                        foreach (string i in potentialProblems)
                            AddWarning(i, WarningBoxType.Warning);
                    });
                }

                // start building the new file description page
                string origUploader = ImageInfos[ImageInfos.Count - 1].Attributes["user"].Value;

                // clean up local templates, etc.
                string prefix = GetCurrentInterwikiPrefix(false);
                try
                {
                    text = Regex.Replace(text, LocalWikiData.CopyToCommonsRegex, "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
                catch (ArgumentException e)
                {
                    ErrorHandler(Localization.GetString("LocalWikiDataError") + "\n\n" +
                        Localization.GetString("LocalWikiDataRegexError", "CopyToCommonsRegex") + "\n\n" + e.Message);
                }
                //text = Regex.Replace(text, "== ?(Summary|Licensing:?) ?== *\n", "\n");
                text = Regex.Replace(text, "== ?((" + LocalWikiData.Summary + ")|{{int:filedesc}}) ?== *\n", "", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, "\n?\n?== ?((" + LocalWikiData.Licensing + ")|{{int:license}}) ?== *\n", "\n\n== {{int:license-header}} ==\n", RegexOptions.IgnoreCase);
                text = Regex.Replace(text, @"\[\[:?", "[[" + prefix + ":", RegexOptions.Compiled);
                text = Regex.Replace(text, @"\[\[" + prefix + @":([^\|\]]+)\]\]", new MatchEvaluator(delegate(Match m)
                    {
                        string linktext = m.Groups[1].Value;
                        string linktextLower = linktext.ToLower();
                        if (!(linktextLower.StartsWith("category:")) && !(linktextLower.StartsWith(LocalWikiData.CategoryNamespace.ToLower())))
                            return "[[" + prefix + ":" + linktext + "|" + linktext + "]]";
                        return "<!-- [[" + linktext + "]] -->";  // comment out categories
                    }), RegexOptions.Compiled);
                // this next one is redundant to the above BUT still needed for categories with sortkeys (?!)
                text = Regex.Replace(text, @"\[\[" + prefix + @":(Category:[^\]]+\]\])", "<!-- [[$1 -->", RegexOptions.Compiled);

                // per-wiki cleanup
                foreach (KeyValuePair<string, string> replacement in LocalWikiData.Replacements)
                {
                    try
                    {
                        text = Regex.Replace(text, replacement.Key, replacement.Value.Replace("\\n", "\n"),
                            RegexOptions.IgnoreCase);
                    }
                    catch (ArgumentException e)
                    {
                        ErrorHandler(Localization.GetString("LocalWikiDataError") + "\n\n" +
                            Localization.GetString("LocalWikiDataRegexError", "Replacement") + "\n\n" + e.Message);
                    }
                }

                // amend self-license tags
                string beforeSelfTagCheck = text;
                foreach (KeyValuePair<string, string> replacement in LocalWikiData.SelfLicenseReplacements)
                {
                    try
                    {
                        text = Regex.Replace(text, replacement.Key, replacement.Value.Replace("\\n", "\n")
                            .Replace("%%OriginalUploader%%", origUploader).Replace("%%InterwikiLinkPrefix%%", prefix),
                            RegexOptions.IgnoreCase);
                    }
                    catch (ArgumentException e)
                    {
                        ErrorHandler(Localization.GetString("LocalWikiDataError") + "\n\n" +
                            Localization.GetString("LocalWikiDataRegexError", "SelfLicenseReplacement") + "\n\n" + e.Message);
                    }
                }
                bool selfLicense = (text != beforeSelfTagCheck);

                text = text.Trim();

                // the character index at which the information tag finishes (doesn't have to be exact)
                int infoTagEnd = 0;

                string languageCode = GetCurrentLanguageCode();

                if (!textLowercase.Contains("{{information") &&
                    !Regex.IsMatch(text, "{{" + LocalWikiData.Information, RegexOptions.IgnoreCase))
                {
                    string detectedDesc = Regex.Replace(text, "{{[^}]*}}", "", RegexOptions.IgnoreCase);
                    detectedDesc = Regex.Replace(detectedDesc, "==[^=]*==", "", RegexOptions.IgnoreCase);
                    detectedDesc = detectedDesc.Split('\n')[0];
                    if (detectedDesc.Length > 0)
                        text = text.Replace(detectedDesc, "");

                    XmlNode exifDateNode = ImageInfos[ImageInfos.Count - 1].SelectSingleNode("metadata/metadata[@name=\"DateTime\"]");
                    string exifDate = null;
                    if (exifDateNode != null)
                    {
                        exifDate = exifDateNode.Attributes["value"].Value;
                        if (Regex.IsMatch(exifDate, @"^\d\d\d\d:\d\d:\d\d") && !exifDate.StartsWith("0000"))
                            exifDate = exifDate.Substring(0, 10).Replace(':', '-');
                        else
                            exifDate = null;
                    }

                    // Note: pipe replacement was commented out because it caused problems with piped wikilinks in the
                    // detected description. Of course, now any literal pipes will cause problems...
                    var infoTag =
"== {{int:filedesc}} ==\n" +
"{{Information\n" +
"|Description    = " + (languageCode.Length > 0 ? ("{{" + languageCode + "|1=" + detectedDesc.Trim()/*.Replace("|", "&#124;")*/ + "}}") : detectedDesc.Trim().Replace("|", "&#124;")) + "\n" +
"|Date           = " + (exifDate != null ? "{{according to EXIF data|" + exifDate + "}}\n" : "{{original upload date|" + FormatIsoDate(ImageInfos[ImageInfos.Count - 1]) + "}}\n") +
"|Source         = {{own work by original uploader}} <!-- " + Localization.GetString("ChangeIfNotOwnWork") + " -->\n" +
"|Author         = " + (selfLicense ? ("[[" + prefix + ":User:" + ImageInfos[ImageInfos.Count - 1].Attributes["user"].Value + "|]]\n") : "\n") +
"|Permission     = \n" +
"|Other_versions = \n" +
"}}\n\n";
                    text = infoTag + text;

                    infoTagEnd = infoTag.Length - 8;  // -8 for sanity
                }
                else
                {
                    text = "== {{int:filedesc}} ==\n" + text;

                    string errorTopicText = "";
                    try
                    {
                        errorTopicText = "Information";
                        text = Regex.Replace(text, @"{{\s*(" + LocalWikiData.Information + @")", "{{Information", RegexOptions.IgnoreCase);

                        string paramStart = @"({{Information({{[^{}]*}}|[^{}])*)\|\s*(";
                        string paramEnd = @")\s*= *";
                        errorTopicText = "Description";
                        text = Regex.Replace(text, paramStart + LocalWikiData.Description + paramEnd, "$1|Description    = ", RegexOptions.IgnoreCase);
                        errorTopicText = "Date";
                        text = Regex.Replace(text, paramStart + LocalWikiData.Date + paramEnd, "$1|Date           = ", RegexOptions.IgnoreCase);
                        errorTopicText = "Source";
                        text = Regex.Replace(text, paramStart + LocalWikiData.Source + paramEnd, "$1|Source         = ", RegexOptions.IgnoreCase);
                        errorTopicText = "Author";
                        text = Regex.Replace(text, paramStart + LocalWikiData.Author + paramEnd, "$1|Author         = ", RegexOptions.IgnoreCase);
                        errorTopicText = "Permission";
                        text = Regex.Replace(text, paramStart + LocalWikiData.Permission + paramEnd, "$1|Permission     = ", RegexOptions.IgnoreCase);
                        errorTopicText = "Other_versions";
                        text = Regex.Replace(text, paramStart + LocalWikiData.Other_versions + paramEnd, "$1|Other_versions = ", RegexOptions.IgnoreCase);
                    }
                    catch (ArgumentException e)
                    {
                        ErrorHandler(Localization.GetString("LocalWikiDataError") + "\n\n" +
                            Localization.GetString("LocalWikiDataRegexError", errorTopicText) + "\n\n" + e.Message);
                    }

                    if (languageCode.Length > 0 && !text.Contains("{{" + languageCode + "|"))
                        text = Regex.Replace(text, @"({{Information[\r\n]* *\| ?Description *= *)([^\r\n ][^\r\n]+)([\r\n])",
                            "$1{{" + languageCode + "|1=$2}}$3", RegexOptions.IgnoreCase);

                    Match infoTagMatch = Regex.Match(text, @"{{\s*information\s*(\|({{[^{}]*}}|[^{}])*)?}}", RegexOptions.IgnoreCase);
                    infoTagEnd = infoTagMatch.Index + infoTagMatch.Length - 8;  // -8 for sanity
                }


                // assume first template is a license tag
                if (!text.Contains("{{int:license-header}}"))
                {
                    bool hadAnySuccessYet = false;
                    text = Regex.Replace(text, "\n?\n?{{", delegate(Match m)
                    {
                        if (m.Index < infoTagEnd || hadAnySuccessYet)
                            return m.Groups[0].Value;
                        hadAnySuccessYet = true;
                        return "\n\n== {{int:license-header}} ==\n{{";
                    });
                }

                text += "\n\n== {{Original upload log}} ==";
                if (Settings.CommonsDomain == Settings.DefaultCommonsDomain)
                {
                    text += "\n\n{{transferred from|" + Settings.LocalDomain + "||ftcg}} {{original description page|" +
                        Settings.LocalDomain + "|" + Uri.EscapeDataString(CurrentFileName.Replace(' ', '_')) + "}}";
                }

                text += "\n\n{| class=\"wikitable\"\n! {{int:filehist-datetime}} !! {{int:filehist-dimensions}} !! {{int:filehist-user}} !! {{int:filehist-comment}}";
                foreach (XmlNode n in ImageInfos)
                {
                    text += "\n|-\n| " + FormatTimestamp(n) + " || " + FormatDimensions(n) + " || ";

                    // check if the username has been RevDel'd (for admins, the user attribute
                    // will be present, and we don't want to copy the hidden username to Commons,
                    // so we need to check the userhidden attribute)
                    if (n.Attributes["userhidden"] != null || n.Attributes["user"] == null)
                        text += "<span class=\"history-deleted\">{{int:rev-deleted-user}}</span>";
                    else
                        text += "{{uv|" + n.Attributes["user"].Value + "|" + GetCurrentInterwikiPrefix(true) + ":}}";

                    text += " || ";

                    // same deal for comment/commenthidden
                    if (n.Attributes["commenthidden"] != null || n.Attributes["comment"] == null)
                        text += "<span class=\"history-deleted\">{{int:rev-deleted-comment}}</span>";
                    else
                        text += "<nowiki>" + n.Attributes["comment"].Value + "</nowiki>";
                }
                text += "\n|}";

                // remove multiple line breaks
                text = Regex.Replace(text, @"[\r\n]{3,}", "\n\n");

                Invoke((MethodInvoker) delegate()
                    {
                        txtCommonsText.Text = text.Replace("\n", "\r\n");
                        lnkLocalFile.Enabled = lnkGoogleImageSearch.Enabled = true;
                        lnkCommonsFile.Enabled = false;
                        lblFileLinks.Visible = lstFileLinks.Visible = true;
                        if (lstFileLinks.ForeColor == SystemColors.GrayText)
                        {
                            lstFileLinks.Items.Clear();
                            lstFileLinks.Items.Add(Localization.GetString("Loading"));
                        }
                        EnableForm(true);
                    });
            });
            sentry.Finally += new MorebitsDotNet.ActionCompleted.Action(delegate()
            {
                Invoke((MethodInvoker) delegate()
                {
                    EnableForm(true);
                });
            });

            // download image file
            StringDictionary query = new StringDictionary 
            {
                { "action", "query" },
                { "prop", "imageinfo|info" },
                { "iiprop", "comment|timestamp|user|url|size|mime|metadata" },
                { "iilimit", "500" },
                { "iiurlwidth", pictureBox1.Width.ToString() },
                { "iiurlheight", pictureBox1.Height.ToString() },
                { "titles", "File:" + CurrentFileName + "|File talk:" + CurrentFileName },
                { "redirects", "true" }
            };
            MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
            {
                XmlNode filePage = doc.SelectSingleNode("//page[@ns=6]");  // was doc.GetElementsByTagName("page")[0]

                switch (filePage.Attributes["imagerepository"].Value)
                {
                    case "shared":
                        ErrorHandler(Localization.GetString("AlreadyCommons"));
                        sentry.Fail();
                        return;
                    case "":
                        if (filePage.Attributes["missing"] != null)
                            ErrorHandler(Localization.GetString("ImageMissing"));
                        else
                            ErrorHandler(Localization.GetString("NoFile"));
                        sentry.Fail();
                        return;
                }

                ImageInfos = doc.GetElementsByTagName("ii");
                ImageDatas = new byte[ImageInfos.Count][];

                Invoke((MethodInvoker) delegate()
                {
                    lblName.Text = filePage.Attributes["title"].Value;

                    if (ImageInfos.Count > 1)
                    {
                        lblPastRevisions.Visible = btnPastRevisions.Visible = true;
                        lblPastRevisions.Text = ((ImageInfos.Count == 2) ?
                            Localization.GetString("OneEarlierVersion_Label") :
                            Localization.GetString("EarlierVersions_Format", (ImageInfos.Count - 1).ToString()));
                    }
                    else
                        lblPastRevisions.Visible = btnPastRevisions.Visible = false;
                });

                // notify about presence of file talk page (if it is over 120 bytes in size)
                XmlNode fileTalkPage = doc.SelectSingleNode("//page[@ns=7]");
                if (fileTalkPage.Attributes["missing"] == null)
                {
                    string warningtext = "• " + Localization.GetString("ContentOnTalkPage");
                    if (fileTalkPage.Attributes["length"] != null &&
                        int.Parse(fileTalkPage.Attributes["length"].Value) > int.Parse(LocalWikiData.FileTalkMinimumSize))
                    {
                        warningtext = warningtext.Replace("{1}", " (" + int.Parse(fileTalkPage.Attributes["length"].Value).
                            ToString("n0", CultureInfo.InvariantCulture) + " bytes)");
                        AddWarningLink(warningtext, Localization.GetString("TalkPage"), delegate(object sender, LinkLabelLinkClickedEventArgs e)
                        {
                            try
                            {
                                Process.Start(MorebitsDotNet.GetProtocol() + "://" + Settings.LocalDomain + ".org/wiki/File_talk:" +
                                    CurrentFileName);
                            }
                            catch (Exception)
                            {
                                ErrorHandler(Localization.GetString("LinkVisitFailed"));
                            }
                        }, WarningBoxType.Warning);
                    }
                }

                // download the file and display a thumbnail (also display metadata)
                SelectedRevisions = new int[] { 0 };
                DownloadFileAndDisplayThumb();

                sentry.DoneOne();
            }, ErrorHandler, WebRequestMethods.Http.Get);

            // get wikitext of file description page
            query = new StringDictionary 
            {
                { "action", "query" },
                { "prop", "revisions" },
                { "rvprop", "content" },
                { "titles", "File:" + CurrentFileName },
                { "redirects", "true" }
            };
            MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
            {
                if (doc.GetElementsByTagName("page")[0].Attributes["missing"] != null)
                {
                    // MessageBox.Show("Image does not exist on enwiki");    don't need to tell user twice
                    sentry.Fail();
                    return;
                }

                text = doc.GetElementsByTagName("rev")[0].InnerText;

                Invoke((MethodInvoker) delegate()
                {
                    txtLocalText.Text = text.Replace("\n", "\r\n");
                });

                XmlNodeList ns = doc.GetElementsByTagName("n");
                if (ns.Count > 0)
                    Invoke((MethodInvoker) delegate()
                    {
                        CurrentFileName = ns[0].Attributes["to"].Value;
                        CurrentFileName = CurrentFileName.Substring(CurrentFileName.IndexOf(':') + 1);
                    });
                Invoke((MethodInvoker) delegate()
                {
                    txtNormName.Text = "File:" + CurrentFileName;
                });

                sentry.DoneOne();
            }, ErrorHandler, WebRequestMethods.Http.Get);

            // get file links (not in sentry)
            query = new StringDictionary 
            {
                { "action", "query" },
                { "list", "imageusage" },
                { "iulimit", "20" },
                { "iutitle", "File:" + CurrentFileName },
                { "rawcontinue", "" },
            };
            // prevent race conditions
            object current = new object();
            Invoke((MethodInvoker) delegate() { lstFileLinks.Tag = current; });
            MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
            {
                if (lstFileLinks.Tag != current)
                    return;
                Invoke((MethodInvoker) delegate()
                {
                    lstFileLinks.Items.Clear();
                    XmlNodeList ius = doc.GetElementsByTagName("iu");
                    if (ius.Count == 0)
                    {
                        lstFileLinks.Items.Add(Localization.GetString("NoImageUsages_Label"));
                        return;
                    }

                    lstFileLinks.ForeColor = SystemColors.ControlText;
                    foreach (XmlNode i in ius)
                        lstFileLinks.Items.Add(i.Attributes["title"].Value);
                    if (doc.GetElementsByTagName("query-continue").Count > 0)
                        lstFileLinks.Items.Add("<<" + Localization.GetString("SeeWikiForFullList_Label") + ">>");
                });
            }, ErrorHandler, WebRequestMethods.Http.Get);
        }

        // per-revision logic
        private void DownloadFileAndDisplayThumb()
        {
            XmlNode topImage = ImageInfos[SelectedRevisions[0]];
            Invoke((MethodInvoker) delegate()
            {
                if (SelectedRevisions.Length > 1)
                    lblRevision.Text = Localization.GetString("MultipleVersions_Label", SelectedRevisions.Length.ToString(),
                        ImageInfos.Count.ToString());
                else if (SelectedRevisions[0] == 0)
                    lblRevision.Text = Localization.GetString("CurrentVersion_Label") + " (" + FormatTimestamp(topImage) + ")";
                else
                    lblRevision.Text = Localization.GetString("OldVersion_Label") + " (" + FormatTimestamp(topImage) + ")";

                lblDimensions.Text = FormatDimensions(topImage);
                lblRevision.ForeColor = lblName.ForeColor = (SelectedRevisions[0] == 0 && SelectedRevisions.Length == 1 ? SystemColors.ControlText : Color.Red);

                XmlNodeList metadatas = topImage.SelectNodes("metadata/metadata");
                if (metadatas.Count > 4)
                {
                    lblViewExif.Visible = btnViewExif.Visible = true;
                    btnViewExif.Tag = metadatas;
                }
                else
                    lblViewExif.Visible = btnViewExif.Visible = false;

                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox1.Image = pictureBox1.InitialImage;
                pictureBox1.Cursor = Cursors.Default;
            });

            // decide whether PictureBox can display this image
            bool previewDownloadedFile = false;
            if (topImage.Attributes["thumburl"].Value == "")
            {
                // probably an OGG or something
                Invoke((MethodInvoker) delegate()
                {
                    pictureBox1.Image = pictureBox1.ErrorImage;
                });
            }
            else
            {
                switch (topImage.Attributes["mime"].Value)
                {
                    // GDI+ natively supports only these formats
                    case "image/gif":
                    case "image/jpeg":
                    case "image/png":
                    case "image/tiff":
                        previewDownloadedFile = true;
                        break;
                    default:
                        // try to download PNG thumb
                        // ImageLocation doesn't work, because of the useragent
                        WebClient clThumb = new WebClient();
                        clThumb.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
                        clThumb.DownloadDataCompleted += new DownloadDataCompletedEventHandler(
                            delegate(object s, DownloadDataCompletedEventArgs v)
                            {
                                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                                Invoke((MethodInvoker) delegate()
                                {
                                    try
                                    {
                                        pictureBox1.Image = Image.FromStream(new MemoryStream(v.Result, false));
                                        pictureBox1.Cursor = ZoomInCursor;
                                    }
                                    catch (Exception)
                                    {
                                        pictureBox1.Image = pictureBox1.ErrorImage;
                                    }
                                });

                            });
                        clThumb.DownloadDataAsync(new Uri(topImage.Attributes["thumburl"].Value));
                        break;
                }
            }

            // download files
            SetTransferButtonDownloading(true);
            int currentIndexIndex = 0;  // the index into SelectedRevisions (an array of indexes). So meta!
            DownloadDataCompletedEventHandler displayThumbHandler = null;
            displayThumbHandler = new DownloadDataCompletedEventHandler(
                delegate(object s, DownloadDataCompletedEventArgs v)
                {
                    if (v.Cancelled)
                    {
                        SetTransferButtonDownloading(false);
                        return;
                    }
                    if (v.Error != null)
                    {
                        ErrorHandler(Localization.GetString("FailedToDownload") + "\n\n" + v.Error.Message);
                        SetTransferButtonDownloading(false);
                        return;
                    }

                    ImageDatas[SelectedRevisions[currentIndexIndex]] = v.Result;
                    if (previewDownloadedFile && currentIndexIndex == 0)
                    {
                        Invoke((MethodInvoker) delegate()
                        {
                            try
                            {
                                Image img = Image.FromStream(new MemoryStream(ImageDatas[SelectedRevisions[currentIndexIndex]], false));
                                if (img.Height > pictureBox1.Height || img.Width > pictureBox1.Width)
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                else
                                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                                pictureBox1.Image = img;
                                pictureBox1.Cursor = ZoomInCursor;
                            }
                            catch (Exception)
                            {
                                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                                pictureBox1.Image = pictureBox1.ErrorImage;
                            }
                        });
                    }

                    if (++currentIndexIndex < SelectedRevisions.Length)
                    {
                        ImageDataDownloader = new WebClient();
                        ImageDataDownloader.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
                        ImageDataDownloader.DownloadDataCompleted += displayThumbHandler;
                        ImageDataDownloader.DownloadDataAsync(new Uri(ImageInfos[SelectedRevisions[currentIndexIndex]].Attributes["url"].Value));
                    }
                    else
                    {
                        SetTransferButtonDownloading(false);
                    }
                });
            ImageDataDownloader = new WebClient();
            ImageDataDownloader.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
            ImageDataDownloader.DownloadDataCompleted += displayThumbHandler;
            ImageDataDownloader.DownloadDataAsync(new Uri(ImageInfos[SelectedRevisions[currentIndexIndex]].Attributes["url"].Value));
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                button1_Click(sender, e);
        }

        // Random File feature
        // ===================

        List<string> RandomImageCache = new List<string>(500);
        Random rand = new Random();
        string RandomContinue;
        List<int> RandomBlacklist = new List<int>();  // to avoid weird infinite loops and other silliness

        private void RandomImage(object sender, EventArgs e)
        {
            EnableForm(false);

            if (RandomFileSource == FileSources.TextFile)
            {
                if (TextFileCache.Count == 0)
                {
                    try
                    {
                        TextFileCache.AddRange(File.ReadAllLines(SourceTag));
                    }
                    catch (Exception x)
                    {
                        ErrorHandler(Localization.GetString("FailedToReadTextFile", SourceTag) + "\n\n" + x.Message);
                        EnableForm(true);
                        return;
                    }
                    TextFileCache.RemoveAll(str => String.IsNullOrEmpty(str));
                }
                RandomImageTextFileCore();
            }
            else if (RandomFileSource == FileSources.Category)
            {
                if (RandomImageCache.Count == 0)
                {
                    EnableForm(false);
                    StringDictionary query = new StringDictionary 
                    {
                        { "action", "query" },
                        { "list", "categorymembers" },
                        { "cmtitle", "Category:" + SourceTag },
                        { "cmnamespace", "6" },  // File:, obviously
                        { "cmsort", "timestamp" },
                        { "cmprop", "title" },
                        { "cmlimit", "500" },
                        { "rawcontinue", "" },
                    };
                    if (RandomContinue != null)
                        query.Add("cmcontinue", RandomContinue);
                    MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
                    {
                        foreach (XmlNode i in doc.GetElementsByTagName("cm"))
                            RandomImageCache.Add(i.Attributes["title"].Value);
                        if (RandomImageCache.Count == 0)
                        {
                            ErrorHandler(Localization.GetString("NoMoreFiles"));
                            return;
                        }

                        XmlNodeList continues = doc.GetElementsByTagName("query-continue");
                        if (continues.Count > 0)
                            RandomContinue = continues[0].FirstChild.Attributes["cmcontinue"].Value;
                        else
                            RandomContinue = null;

                        RandomImageCategoryCore();
                    }, ErrorHandler, WebRequestMethods.Http.Get);
                }
                else
                    RandomImageCategoryCore();
            }
            else
            {
                EnableForm(true);
                throw new Exception("Internal error: the RandomFileSource is invalid.");
            }
        }

        private void RandomImageCategoryCore()
        {
            if (RandomBlacklist.Count >= RandomImageCache.Count)
            {
                ErrorHandler(Localization.GetString("RandomCacheEmpty1") + "\n\n" +
                    Localization.GetString("RandomCacheEmpty2") + "\n\n" +
                    Localization.GetString("RandomCacheEmpty3"));
                RandomImageCache.Clear();
                return;
            }

            int randIndex;
            do
            {
                randIndex = rand.Next(RandomImageCache.Count);
            } while (RandomBlacklist.Contains(randIndex));
            Invoke((MethodInvoker) delegate()
            {
                textBox1.Text = RandomImageCache[randIndex];
            });
            RandomImageCache.RemoveAt(randIndex);

            CurrentFileSource = FileSources.Category;
            RandomCurrentIndex = randIndex;
            try
            {
                DownloadAndProcess();
            }
            finally
            {
                EnableForm(true);
            }
        }

        private void RandomImageTextFileCore()
        {
            // not really random - works through the file sequentially
            Invoke((MethodInvoker) delegate()
            {
                textBox1.Text = TextFileCache[0];
            });
            TextFileCache.RemoveAt(0);

            CurrentFileSource = FileSources.TextFile;
            RandomCurrentIndex = 0;
            try
            {
                DownloadAndProcess();
            }
            finally
            {
                EnableForm(true);
            }
        }

        private void optCopyFoo_CheckedChanged(object sender, EventArgs e)
        {
            RandomImageCache.Clear();
            RandomContinue = null;
            RandomBlacklist = new List<int>();

            if (optCategory1.Checked)
            {
                RandomFileSource = FileSources.Category;
                Settings.CurrentSourceOption = "Category1";
                SourceTag = LocalWikiData.Category1;
            }
            else if (optCategory2.Checked)
            {
                RandomFileSource = FileSources.Category;
                Settings.CurrentSourceOption = "Category2";
                SourceTag = LocalWikiData.Category2;
            }
            else if (optCategory3.Checked)
            {
                RandomFileSource = FileSources.Category;
                Settings.CurrentSourceOption = "Category3";
                SourceTag = LocalWikiData.Category3;
            }
            else
            {
                // do nothing - "other source" is handled elsewhere
            }

            btnRandomFile.Text = RandomFileSource == FileSources.TextFile ?
                Localization.GetString("NextFileInList_Button") :
                Localization.GetString("RandomFile_Button");
        }

        // Source selection
        // ================

        private void optOther_CheckedChanged(object sender, EventArgs e)
        {
            if (!optOther.Checked)
                return;

            frmRandomSource form = new frmRandomSource();
            if (!String.IsNullOrEmpty(Settings.SourceTextFile))
            {
                form.optTextFile.Checked = true;
                form.txtFileName.Text = Settings.SourceTextFile;
            }
            else if (!String.IsNullOrEmpty(Settings.SourceCategory))
                form.txtCategory.Text = Settings.SourceCategory;

            if (form.ShowDialog(this) == DialogResult.Cancel)
            {
                switch (LocalWikiData.DefaultCategory)
                {
                    case "1":
                        optCategory1.Checked = true;
                        SourceTag = LocalWikiData.Category1;
                        break;
                    case "2":
                        optCategory2.Checked = true;
                        SourceTag = LocalWikiData.Category2;
                        break;
                    case "3":
                        optCategory3.Checked = true;
                        SourceTag = LocalWikiData.Category3;
                        break;
                }
                RandomFileSource = FileSources.Category;
                return;
            }

            if (form.optTextFile.Checked)
            {
                RandomFileSource = FileSources.TextFile;
                Settings.CurrentSourceOption = "TextFile";
                SourceTag = Settings.SourceTextFile = form.txtFileName.Text;
                Settings.SourceCategory = "";
                TextFileCache.Clear();
            }
            else
            {
                RandomFileSource = FileSources.Category;
                Settings.CurrentSourceOption = "CustomCategory";
                SourceTag = Settings.SourceCategory = form.txtCategory.Text.Substring(
                    form.txtCategory.Text.ToLower(CultureInfo.InvariantCulture).StartsWith("category:", StringComparison.InvariantCulture)
                    ? "category:".Length : 0);
                Settings.SourceTextFile = "";
                RandomImageCache.Clear();
            }

            Settings.WriteSettings();
            btnRandomFile.Text = RandomFileSource == FileSources.TextFile ?
                Localization.GetString("NextFileInList_Button") :
                Localization.GetString("RandomFile_Button");
        }

        // Transfer to Commons
        // ===================

        private void DoTransfer(object sender, EventArgs e)
        {
            if (panWarning.Visible && MessageBox.Show(Localization.GetString("PotentialProblemsGoAhead"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            if (Array.Exists<int>(SelectedRevisions, i => ImageDatas[i] == null))
            {
                ErrorHandler(Localization.GetString("StillDownloading"));
                return;
            }

            if (SelectedRevisions.Length == 1 && SelectedRevisions[0] != 0 &&
                MessageBox.Show(Localization.GetString("OldVersionTransferAdvice"), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;
            if (SelectedRevisions.Length > 1 &&
                MessageBox.Show(Localization.GetString("MultipleVersionTransferAdvice"), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;

            EnableForm(false);

            if (!MorebitsDotNet.LoginSessions[Wiki.Commons].LoggedIn)
                MorebitsDotNet.LogIn(Wiki.Commons, Settings.CommonsUserName, Settings.CommonsPassword,
                    PrepareTransfer, ErrorHandler);
            else
                PrepareTransfer();
        }

        private void PrepareTransfer()
        {
            StringDictionary query = new StringDictionary 
            {
                { "action", "query" },
                { "prop", "info" },
                { "intoken", "edit" },
                { "titles", txtNormName.Text }
            };
            MorebitsDotNet.PostApi(Wiki.Commons, query, delegate(XmlDocument doc)
            {
                if (doc.GetElementsByTagName("page")[0].Attributes["invalid"] != null)
                {
                    ErrorHandler(Localization.GetString("InvalidFilenameCommons"));
                    return;
                }

                if (doc.GetElementsByTagName("page")[0].Attributes["missing"] == null && !chkIgnoreWarnings.Checked)
                {
                    ErrorHandler(Localization.GetString("FilenameClash") + "\n\n" + Localization.GetString("Warnings2"));
                    return;
                }

                string newFilename = Regex.Replace(txtNormName.Text, "^(Image|File):", "");
                string token = doc.GetElementsByTagName("page")[0].Attributes["edittoken"].Value;

                // add categories from CoolCat
                string textForCommons = txtCommonsText.Text.Trim()
                    .Replace("<!-- " + Localization.GetString("ChangeIfNotOwnWork") + " -->", "");
                IEnumerable<string> categories = coolCat.Categories;
                bool haveAddedCats = false;
                foreach (string i in categories)
                {
                    if (!haveAddedCats)
                    {
                        textForCommons += "\n";
                        haveAddedCats = true;
                    }
                    textForCommons += "\n[[Category:" + i + "]]";
                }

                // Note: the upload comment is not localised, since Commons uses English as lingua franca
                UploadEachRevision(newFilename, SelectedRevisions.Length - 1, true, textForCommons, token);
            }, ErrorHandler);
        }

        private void UploadEachRevision(string filename, int selectedRevisionIndex,
            bool isEarliestRevision, string text, string token)
        {
            StringDictionary uploadQuery = new StringDictionary 
            {
                { "action", "upload" },
                { "filename", filename }
            };

            if (isEarliestRevision)
                uploadQuery.Add("text", text);

            if (selectedRevisionIndex == 0)  // latest revision?
                uploadQuery.Add("comment", "Transferred from " + Settings.LocalDomain + ": see original upload log above");
            else
                uploadQuery.Add("comment", "Old revision as of " + FormatTimestamp(ImageInfos[SelectedRevisions[selectedRevisionIndex]]) +
                    ": see original upload log above");

            if (chkIgnoreWarnings.Checked || !isEarliestRevision)
                uploadQuery.Add("ignorewarnings", "true");

            uploadQuery.Add("token", token);  // token always last

            MorebitsDotNetPostSuccess callbackAfterEach = delegate(XmlDocument doc)
            {
                // assuming the upload was successful...

                // any warnings?
                XmlNodeList xmlWarnings = doc.GetElementsByTagName("warnings");
                List<string> warnings = new List<string>();
                // TODO: get rid of isEarliestRevision here, change warnings dialog to a yes/no type thing
                if (xmlWarnings.Count > 0 && !chkIgnoreWarnings.Checked && isEarliestRevision)
                {
                    foreach (XmlAttribute i in xmlWarnings[0].Attributes)
                    {
                        switch (i.LocalName)
                        {
                            case "badfilename":
                                // <warnings badfilename="new, corrected filename" />
                                warnings.Add(Localization.GetString("UploadWarning_BadFilename", i.Value));
                                break;
                            case "bad-prefix":
                                // <warnings bad-prefix="DSCN1004.JPG" />
                                warnings.Add(Localization.GetString("UploadWarning_BadPrefix", i.Value));
                                break;
                            case "duplicate-archive":
                                // <warnings duplicate-archive="Deleted_file.png" />
                                warnings.Add(Localization.GetString("UploadWarning_DuplicateArchive", i.Value));
                                break;
                            case "exists":
                                // <warnings exists="File.png" />
                                // Should be handled when getting the edit token
                                warnings.Add(Localization.GetString("FilenameClash"));
                                break;
                            case "exists-normalized":
                                // <warnings exists-normalized="A.jpeg" />
                                warnings.Add(Localization.GetString("UploadWarning_ExistsNormalized", i.Value));
                                break;
                            case "thumb": // Not sure exactly what this is...
                            case "thumb-name":
                                // <warnings thumb-name="180px-File.png" />
                                warnings.Add(Localization.GetString("UploadWarning_ThumbName", i.Value));
                                break;
                            //case "was-deleted":
                                // Let's not bother with this warning.
                                // <warnings was-deleted="Deleted_file.png" />
                                //warnings.Add(Localization.GetString("UploadWarning_WasDeleted", i.Value));
                                //break;
                            // Not handled: page-exists, ...
                            default:
                                warnings.Add(Localization.GetString("UploadWarning_Unknown", i.LocalName, i.Value));
                                break;
                        }
                    }
                    foreach (XmlNode i in xmlWarnings[0].ChildNodes)
                    {
                        if (i.NodeType != XmlNodeType.Element)
                            continue;
                        switch (i.LocalName)
                        {
                            case "duplicate":
                                // <warnings>
                                //   <duplicate>
                                //     <duplicate>First file name</duplicate>
                                //     <duplicate>Second file name</duplicate>
                                //     ...
                                //   </duplicate>
                                // </warnings>
                                string warning = Localization.GetString("UploadWarning_Duplicate") + "\n";
                                foreach (XmlNode j in i.ChildNodes)
                                    if (j.NodeType == XmlNodeType.Element)
                                        warning += j.InnerText + "\n";
                                warnings.Add(warning.TrimEnd());
                                break;
                            default:
                                warnings.Add(i.OuterXml);
                                break;
                        }
                    }
                }
                if (warnings.Count > 0)
                {
                    ErrorHandler(Localization.GetString("Warnings1") + "\n\n• " + String.Join("\n• ", warnings.ToArray()) + 
                        "\n\n" + Localization.GetString("Warnings2"), MessageBoxIcon.Warning);
                    return;
                }

                // more to upload?
                if (--selectedRevisionIndex >= 0)
                    UploadEachRevision(filename, selectedRevisionIndex, false, null, token);
                else
                    UploadSuccess();
            };

            MorebitsDotNet.UploadFile(Wiki.Commons, uploadQuery, ImageDatas[SelectedRevisions[selectedRevisionIndex]],
                filename, "file", callbackAfterEach, ErrorHandler);
        }

        private void UploadSuccess()
        {
            Invoke((MethodInvoker) delegate()
            {
                lnkCommonsFile.Enabled = true;
                lnkCommonsFile.Tag = txtNormName.Text;
            });

            // log the transfer, if requested
            if (Settings.LogTransfers)
            {
                string logText = "# " +
                    DateTime.UtcNow.ToString(Localization.GetString("LogFileTimestampFormat"), System.Globalization.DateTimeFormatInfo.InvariantInfo) +
                    " (UTC): ";
                if (CurrentFileName == txtNormName.Text.Substring(5))
                    logText += "[[:" + txtNormName.Text + "]]\r\n";
                else
                    logText += "[[:File:" + CurrentFileName + "]] → [[:" + txtNormName.Text + "]]\r\n";
                try
                {
                    File.AppendAllText("CommonsTransfers.log", logText, Encoding.UTF8);
                }
                catch (Exception x)
                {
                    ErrorHandler(Localization.GetString("LogFileWriteFailed") + "\n\n" + x.Message);
                    EnableForm(true);
                    return;
                }
            }

            // this is invoked when all is finished
            MethodInvoker showSuccess = delegate()
            {
                Invoke((MethodInvoker) ClearWarnings);
                AddWarning(Localization.GetString("Success_Label",
                    Settings.CommonsDomain == Settings.DefaultCommonsDomain ?
                    Localization.GetString("ViewFilePageOnWikimediaCommons_Hyperlink") :
                    Localization.GetString("ViewFilePageOnLocalWiki_Hyperlink_Format", Settings.CommonsDomain)),
                    WarningBoxType.Success);
                if (Settings.OpenBrowserLocal)
                    lnkLocalFile_LinkClicked(null, null);
                if (Settings.OpenBrowserAutomatically)
                    lnkCommonsFile_LinkClicked(null, null);
                if (CurrentFileSource != FileSources.Category)
                    Invoke((MethodInvoker) delegate() { btnRandomFile.Focus(); });
            };

            // finished?
            if (!chkDeleteAfter.Checked)
            {
                EnableForm(true);
                showSuccess();
                return;
            }

            // continue with deleting/tagging with {{now Commons}}

            MorebitsDotNetLoginSuccess innerAction = delegate()
            {
                if (Settings.LocalSysop)
                {
                    DeleteLocal();
                    return;
                }

                StringDictionary enTokenQuery = new StringDictionary 
                    {
                        { "action", "query" },
                        { "prop", "info|revisions" },
                        { "intoken", "edit" },
                        { "titles", "File:" + CurrentFileName },  // old filename
                        { "rvprop", "content" }
                    };
                MorebitsDotNet.PostApi(Wiki.Local, enTokenQuery, delegate(XmlDocument enDoc)
                {
                    if (enDoc.GetElementsByTagName("page")[0].Attributes["missing"] != null)
                    {
                        ErrorHandler(Localization.GetString("LocalFileDeleted"));
                        return;
                    }

                    string enToken = enDoc.GetElementsByTagName("page")[0].Attributes["edittoken"].Value;

                    string enText = Regex.Replace(enDoc.GetElementsByTagName("rev")[0].FirstChild.Value, "{{orphan image[^}]*}}", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    string nowcommonsTag = "{{" + LocalWikiData.NowCommonsTag + "|" + txtNormName.Text + "|date=" + DateTime.UtcNow.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + "}}\n";
                    string newText = Regex.Replace(enText, LocalWikiData.CopyToCommonsRegex, nowcommonsTag, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    bool replaced = true;
                    if (enText == newText)
                    {
                        newText = nowcommonsTag + newText;
                        replaced = false;
                    }

                    StringDictionary enEditQuery = new StringDictionary 
                        {
                            { "action", "edit" },
                            { "token", enToken },
                            { "title", "File:" + CurrentFileName },
                            { "text", newText },
                            { "summary", (replaced ? LocalWikiData.NowCommonsReplacingTagEditSummary : LocalWikiData.NowCommonsAddingTagEditSummary) +
                                " ([[" + LocalWikiData.LocalFtcgPage + "|FtCG]])" },
                            { "nocreate", "true" },
                            { "redirects", "true" }
                        };
                    MorebitsDotNet.PostApi(Wiki.Local, enEditQuery, delegate(XmlDocument enInnerDoc)
                    {
                        EnableForm(true);
                        string editResult = enInnerDoc.GetElementsByTagName("edit")[0].Attributes["result"].Value.ToLower();
                        if (editResult != "success")
                        {
                            ErrorHandler(Localization.GetString("NowCommonsFailed") + " " + editResult, MessageBoxIcon.Information);
                            return;
                        }
                        showSuccess();
                    }, ErrorHandler);
                }, ErrorHandler);
            };

            if (!MorebitsDotNet.LoginSessions[Wiki.Local].LoggedIn)
                MorebitsDotNet.LogIn(Wiki.Local, Settings.LocalUserName,
                    Settings.LocalPassword, innerAction, ErrorHandler);
            else
                innerAction();
        }

        private void DeleteLocal()
        {
            EnableForm(false);

            MorebitsDotNetLoginSuccess action = delegate()
            {
                StringDictionary query = new StringDictionary 
                {
                    { "action", "query" },
                    { "prop", "info" },
                    { "intoken", "delete" },
                    { "titles", "File:" + CurrentFileName },
                    { "redirects", "true" }
                };
                MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
                {
                    if (doc.GetElementsByTagName("page")[0].Attributes["missing"] != null)
                    {
                        ErrorHandler(Localization.GetString("AlreadyDeleted"));
                        return;
                    }

                    string token = doc.GetElementsByTagName("page")[0].Attributes["deletetoken"].Value;
                    StringDictionary deleteQuery = new StringDictionary 
                    {
                        { "action", "delete" },
                        { "reason", LocalWikiData.NowCommonsDeletionSummary + ": [[" + lnkCommonsFile.Tag.ToString() + "]]" },
                        { "token", token },
                        { "title", "File:" + CurrentFileName },
                        { "redirects", "true" }
                    };

                    MorebitsDotNet.PostApi(Wiki.Local, deleteQuery, delegate(XmlDocument innerDoc)
                    {
                        EnableForm(true);
                        AddWarning(Localization.GetString("LooksGood"), WarningBoxType.Success);
                        if (Settings.CommonsDomain == Settings.DefaultCommonsDomain)
                            AddWarning(Localization.GetString("DontForgetToCategorize_Label"), WarningBoxType.Success);
                        if (Settings.OpenBrowserLocal)
                            lnkLocalFile_LinkClicked(null, null);
                        if (Settings.OpenBrowserAutomatically)
                            lnkCommonsFile_LinkClicked(null, null);
                        if (CurrentFileSource != FileSources.Category)
                            Invoke((MethodInvoker) delegate() { btnRandomFile.Focus(); });
                    }, ErrorHandler);
                }, ErrorHandler);
            };

            if (!MorebitsDotNet.LoginSessions[Wiki.Local].LoggedIn)
                MorebitsDotNet.LogIn(Wiki.Local, Settings.LocalUserName, Settings.LocalPassword,
                    action, ErrorHandler);
            else
                action();
        }

        // "Decline transfer" feature removed, due to crashes
        // Also, it was useless anyway, since bots would re-add {{copy to Commons}} regardless

        //private void DeclineTransfer(object sender, EventArgs e)
        //{
        //    Action action = delegate()
        //    {
        //        string summary = frmPrompt.Prompt(Localization.GetString("DeclineReasonPrompt1") + "\n" + Localization.GetString("DeclineReasonPrompt2"));
        //        if (summary == null)
        //            return;

        //        EnableForm(false);

        //        StringDictionary enTokenQuery = new StringDictionary 
        //        {
        //            { "action", "query" },
        //            { "prop", "info|revisions" },
        //            { "intoken", "edit" },
        //            { "titles", filename },  // old filename
        //            { "rvprop", "content" }
        //        };
        //        MorebitsDotNet.PostApi(Wiki.Local, enTokenQuery, delegate(XmlDocument enDoc)
        //        {
        //            if (enDoc.GetElementsByTagName("page")[0].Attributes["missing"] != null)
        //            {
        //                ErrorHandler(Localization.GetString("LocalFileDeleted"));
        //                return;
        //            }

        //            string enToken = enDoc.GetElementsByTagName("page")[0].Attributes["edittoken"].Value;

        //            string enText = enDoc.GetElementsByTagName("rev")[0].FirstChild.Value;
        //            string newText =  Regex.Replace(enText, LocalWikiData.CopyToCommonsRegex, "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //            if (enText == newText)
        //            {
        //                ErrorHandler(Localization.GetString("CouldNotFindTag"));
        //                return;
        //            }

        //            StringDictionary enEditQuery = new StringDictionary 
        //            {
        //                { "action", "edit" },
        //                { "token", enToken },
        //                { "title", filename },
        //                { "text", newText },
        //                { "summary", "Declining {{Copy to Commons}} request: " + summary + " ([[WP:FTCG|FtCG]])" },
        //                { "nocreate", "true" }
        //            };
        //            MorebitsDotNet.PostApi(Wiki.Local, enEditQuery, delegate(XmlDocument enInnerDoc)
        //            {
        //                EnableForm(true);
        //                string editResult = enInnerDoc.GetElementsByTagName("edit")[0].Attributes["result"].Value.ToLower();
        //                if (editResult == "success")
        //                    ShowWarningBox(true, "");
        //                else
        //                    ErrorHandler(Localization.GetString("FailedPlus") + " " + editResult, MessageBoxIcon.Information);
        //            }, ErrorHandler, true);
        //        }, ErrorHandler, true);
        //    };

        //    if (!MorebitsDotNet.LoginSessions[Wiki.Local].LoggedIn)
        //        MorebitsDotNet.LogIn(Wiki.Local, Settings.LocalUserName, Settings.LocalPassword,
        //            action, ErrorHandler);
        //    else
        //        action();
        //}

        // Misc. UI backing code
        // =====================

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings set = new frmSettings(false, false);
            if (set.ShowDialog(this) == DialogResult.Cancel)
                return;
            foreach (Wiki w in MorebitsDotNet.LoginSessions.Keys)
                MorebitsDotNet.LoginSessions[w].LoggedIn = false;

            InitSettings();
        }

        void InitSettings()
        {
            chkDeleteAfter.Text = Settings.LocalSysop ?
                Localization.GetString("DeleteLocalFile_Label") :
                Localization.GetString("TagLocalFileWithNowCommons_Label");

            if (Settings.LocalDomain == "en.wikipedia" && Localization.GetString("LanguageCode") == "en")
            {
                lnkLocalFile.Text = "View file page on &English Wikipedia";
                lblLocalFileDesc.Text = "File description page on English Wiki&pedia";
            }
            else
            {
                lnkLocalFile.Text = Localization.GetString("ViewFilePageOnLocalWiki_Hyperlink_Format", Settings.LocalDomain);
                lblLocalFileDesc.Text = Localization.GetString("FilePageOnLocalWiki_TextBox_Format", Settings.LocalDomain);
            }

            if (Settings.CommonsDomain == Settings.DefaultCommonsDomain)
            {
                lnkCommonsFile.Text = Localization.GetString("ViewFilePageOnWikimediaCommons_Hyperlink");
                lblCommonsFileDesc.Text = Localization.GetString("FilePageOnCommons_TextBox");
                lblNormName.Text = Localization.GetString("NewFilenameOnCommons_TextBox");
            }
            else
            {
                lnkCommonsFile.Text = Localization.GetString("ViewFilePageOnLocalWiki_Hyperlink_Format", Settings.CommonsDomain);
                lblCommonsFileDesc.Text = Localization.GetString("FilePageOnLocalWiki_TextBox_Format", Settings.CommonsDomain);
                lblNormName.Text = Localization.GetString("NewFilenameOnTarget_TextBox", Settings.CommonsDomain);
            }

            // local wiki data
            if (Settings.LocalWikiData != "")
            {
                LocalWikiData.LoadWikiData(Settings.LocalWikiData);
            }
            else if (Settings.LocalWikiDataHosted != "" && Settings.LocalWikiDataHosted.IndexOf("|") != -1 &&
                Settings.LocalWikiDataHosted.IndexOf("|") == Settings.LocalWikiDataHosted.LastIndexOf("|"))
            {
                if (!LocalWikiData.LoadWikiDataHosted(Settings.LocalWikiDataHosted.Substring(Settings.LocalWikiDataHosted.IndexOf("|") + 1)))
                    LocalWikiData.LoadWikiData(Properties.Resources.en_wikipedia);
            }
            else
            {
                LocalWikiData.LoadWikiData(Properties.Resources.en_wikipedia);
            }

            optCategory1.Text = LocalWikiData.CategoryNamespace + ":" + LocalWikiData.Category1;
            optCategory2.Visible = LocalWikiData.Category2 != "";
            optCategory2.Text = LocalWikiData.CategoryNamespace + ":" + LocalWikiData.Category2;
            optCategory3.Visible = LocalWikiData.Category3 != "";
            optCategory3.Text = LocalWikiData.CategoryNamespace + ":" + LocalWikiData.Category3;

            string source = Settings.CurrentSourceOption.ToLower();
            if (!source.StartsWith("category"))
                source = "category" + LocalWikiData.DefaultCategory;
            switch (source)
            {
                case "category2":
                    if (!optCategory2.Visible)
                        goto case "category1";
                    optCategory2.Checked = true;
                    SourceTag = LocalWikiData.Category2;
                    break;
                case "category3":
                    if (!optCategory3.Visible)
                        goto case "category2";
                    optCategory3.Checked = true;
                    SourceTag = LocalWikiData.Category3;
                    break;
                case "category1":
                default:
                    optCategory1.Checked = true;
                    SourceTag = LocalWikiData.Category1;
                    break;
            }
            RandomFileSource = FileSources.Category;
            RandomImageCache.Clear();
        }

        private void lnkLocalFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://" + MorebitsDotNet.GetDomain(Wiki.Local) + "/wiki/File:" + CurrentFileName);
            }
            catch (Exception)
            {
                ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }

        private void lnkCommonsFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://" + MorebitsDotNet.GetDomain(Wiki.Commons) + "/wiki/" + lnkCommonsFile.Tag.ToString());
            }
            catch (Exception)
            {
                ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }

        private Image GetThumbnailFailImage()
        {
            Bitmap img = new Bitmap(150, 150);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(Localization.GetString("FailedToGenerateThumbnail1_Label") + "\n" +
                    Localization.GetString("FailedToGenerateThumbnail2_Label") + "\n" +
                    Localization.GetString("FailedToGenerateThumbnail3_Label"), Font, Brushes.Black, 5, 5);
            }
            return img;
        }

        private void SelectVersion(object sender, EventArgs e)
        {
            frmRevisionBrowse form = new frmRevisionBrowse();
            bool first = true;

            for (int i = 0; i < ImageInfos.Count; i++)
            {
                XmlNode n = ImageInfos[i];
                ListViewItem item = new ListViewItem(new string[] {
                    "", 
                    (first ? (Localization.GetString("CurrentVersion_Label") + "\n") : "") + FormatTimestamp(n),
                    FormatDimensions(n),
                    n.Attributes["user"] == null ? Localization.GetString("UserNameHidden") : n.Attributes["user"].Value,
                    n.Attributes["comment"] == null ? Localization.GetString("CommentHidden") : n.Attributes["comment"].Value
                });
                item.Tag = i;

                // download thumbnail
                if (n.Attributes["thumburl"] != null && n.Attributes["thumburl"].Value != "")
                {
                    WebClient cl = new WebClient();
                    cl.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
                    cl.DownloadDataCompleted += new DownloadDataCompletedEventHandler(
                        delegate(object s, DownloadDataCompletedEventArgs v)
                        {
                            // sometimes thumbnails can't be generated and the server gives a 404
                            if (v.Error != null)
                            {
                                Invoke((MethodInvoker) delegate()
                                {
                                    lock (form)
                                    {
                                        form.imageList.Images.Add(GetThumbnailFailImage());
                                        item.ImageIndex = form.imageList.Images.Count - 1;
                                    }
                                });
                                return;
                            }

                            byte[] data = v.Result;
                            Invoke((MethodInvoker) delegate()
                            {
                                Image original = Image.FromStream(new MemoryStream(data, false));
                                Bitmap img = new Bitmap(150, 150, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                using (Graphics g = Graphics.FromImage(img))
                                {
                                    g.FillRectangle(CheckerBrush, 0, 0, img.Width, img.Height);
                                    if (original.Width > original.Height)
                                    {
                                        float newHeight = ((float) original.Height) / ((float) original.Width / 150);
                                        g.DrawImage(original, 0f, (150f - newHeight) / 2f, 150f, newHeight);
                                    }
                                    else
                                    {
                                        float newWidth = ((float) original.Width) / ((float) original.Height / 150f);
                                        g.DrawImage(original, (150f - newWidth) / 2f, 0f, newWidth, 150f);
                                    }
                                }
                                lock (form)
                                {
                                    form.imageList.Images.Add(img);
                                    item.ImageIndex = form.imageList.Images.Count - 1;
                                }
                            });
                        });
                    cl.DownloadDataAsync(new Uri(n.Attributes["thumburl"].Value));
                }
                else
                {
                    lock (form)
                    {
                        form.imageList.Images.Add(GetThumbnailFailImage());
                        item.ImageIndex = form.imageList.Images.Count - 1;
                    }
                }

                if (first)
                {
                    item.UseItemStyleForSubItems = false;
                    item.SubItems[1].Font = new Font(form.Font, FontStyle.Bold);
                }
                form.listView.Items.Add(item);

                if (Array.IndexOf(SelectedRevisions, i) != -1)
                    item.Selected = true;

                first = false;
            }

            if (form.ShowDialog() == DialogResult.Cancel)
                return;

            if (form.listView.SelectedItems.Count == 0)
            {
                ErrorHandler(Localization.GetString("NoRevisionSelected"));
                return;
            }

            SelectedRevisions = new int[form.listView.SelectedItems.Count];
            for (int i = 0; i < form.listView.SelectedItems.Count; i++)
                SelectedRevisions[i] = (int) form.listView.SelectedItems[i].Tag;
            DownloadFileAndDisplayThumb();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmMain_Resize(sender, e);

            // check for updates
            if (Settings.AutoUpdate)
                updateChecker.RunWorkerAsync();
        }

        private void updateChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            WebClient autoupdate = new WebClient();
            autoupdate.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
            autoupdate.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            try
            {
                string result = autoupdate.DownloadString(new Uri("http://en.wikipedia.org/w/index.php?action=raw&ctype=text/css&title=User:This,%20that%20and%20the%20other/FtCG%20current%20version.css"));
                Version newVersion = new Version(result.Split('\n')[1]);
                if (newVersion > new Version(Application.ProductVersion))
                {
                    Invoke((MethodInvoker) delegate() { new frmUpdateAvailable(newVersion).ShowDialog(this); });
                }
            }
            catch (Exception)
            {
                // don't really care - fail silently
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (optCategory1.Checked)
            {
                Settings.CurrentSourceOption = "Category1";
            }
            else if (optCategory2.Checked)
            {
                Settings.CurrentSourceOption = "Category2";
            }
            else if (optCategory3.Checked)
            {
                Settings.CurrentSourceOption = "Category3";
            }
            else if (RandomFileSource == FileSources.Category)
            {
                Settings.CurrentSourceOption = "CustomCategory";
            }
            else if (RandomFileSource == FileSources.TextFile)
            {
                Settings.CurrentSourceOption = "TextFile";
            }
            else
            {
                throw new Exception("invalid source");
            }
            Settings.WriteSettings();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RandomFileSource == FileSources.TextFile && TextFileCache.Count > 0)
            {
                switch (MessageBox.Show(Localization.GetString("TextFileAutoClean") + "\n\n" +
                    Localization.GetString("SaveChanges", Path.GetFileName(SourceTag)),
                    Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        try
                        {
                            File.WriteAllLines(SourceTag, TextFileCache.ToArray());
                        }
                        catch (Exception x)
                        {
                            ErrorHandler(Localization.GetString("TextFileSaveFailed") + "\n\n" + x.Message);
                            e.Cancel = true;
                            return;
                        }
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            panStatus.Location = new Point((ClientSize.Width - panStatus.Width) / 2,
                (ClientSize.Height - panStatus.Height) / 2);
        }

        private void ImboxLookalike(object sender, PaintEventArgs e)
        {
            Control ctl = (Control) sender;
            e.Graphics.Clear(ctl.BackColor);
            e.Graphics.DrawRectangle(new Pen((Color) ctl.Tag, 6),
                Rectangle.FromLTRB(0, 0, ctl.Width, ctl.Height));
        }

        private void ViewExifData(object sender, EventArgs e)
        {
            frmViewExif form = new frmViewExif();
            form.lblRevision.Text = lblRevision.Text;
            form.lblRevision.ForeColor = lblRevision.ForeColor;
            bool failures = false;

            foreach (XmlNode i in (XmlNodeList) btnViewExif.Tag)
            {
                try
                {
                    if (i.Attributes["name"].Value == "metadata")
                        foreach (XmlNode j in i.SelectNodes("value/metadata"))
                            form.lstExif.Items.Add(new ListViewItem(new string[]
                        { 
                            j.Attributes["name"].Value,
                            j.Attributes["value"].Value 
                        }));
                    else
                        form.lstExif.Items.Add(new ListViewItem(new string[]
                    { 
                        i.Attributes["name"].Value,
                        i.Attributes["value"].Value 
                    }));
                }
                catch (Exception)
                {
                    if (!failures)
                        form.lstExif.Items.Add(new ListViewItem(new string[]
                        { 
                            Localization.GetString("EXIFFailedNotice"),
                            Localization.GetString("EXIFFailedMessage")
                        }) { ForeColor = SystemColors.GrayText });
                    failures = true;
                }
            }

            form.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Cursor.Handle == ZoomInCursor.Handle)
            {
                Image original = null;
                try
                {
                    original = Image.FromStream(new MemoryStream(ImageDatas[SelectedRevisions[0]]));
                }
                catch (Exception)
                {
                    ErrorHandler(Localization.GetString("LightboxFailed"));
                    return;
                }
                Bitmap img = new Bitmap(original.Width, original.Height);
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.FillRectangle(CheckerBrush, 0, 0, img.Width, img.Height);
                    g.DrawImage(original, 0, 0, img.Width, img.Height);
                }
                BackgroundImage = img;
                if (img.Height > ClientSize.Height || img.Width > ClientSize.Width)
                    BackgroundImageLayout = ImageLayout.Zoom;
                else
                    BackgroundImageLayout = ImageLayout.Center;
                BackColor = Color.DimGray;

                Click += AbandonLightbox;
                panRoot.Visible = panRoot.Enabled = false;
            }
        }

        void AbandonLightbox(object s, EventArgs a)
        {
            BackgroundImage = null;
            BackColor = DefaultBackColor;
            panRoot.Visible = panRoot.Enabled = true;
            Click -= AbandonLightbox;
        }

        private void lstFileLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkGoToFileLink.Enabled = (lstFileLinks.SelectedIndex != -1 &&
                !lstFileLinks.SelectedItem.ToString().StartsWith("<<", StringComparison.InvariantCulture));
        }

        private void lnkGoToFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://" + Settings.LocalDomain + ".org/wiki/" + ((string) lstFileLinks.SelectedItem).Replace(" ", "_"));
            }
            catch (Exception)
            {
                ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }

        private void lnkLinkify_LinkClicked(object sender, EventArgs e)
        {
            int oldSelectionStart = txtCommonsText.SelectionStart;
            int oldSelectionLength = txtCommonsText.SelectionLength;
            string prefix = "[[" + GetCurrentInterwikiPrefix(false) + ":";
            string suffix = "|]]";
            txtCommonsText.Text = txtCommonsText.Text.Insert(oldSelectionStart + oldSelectionLength, suffix)
                .Insert(oldSelectionStart, prefix);
            txtCommonsText.Focus();
            txtCommonsText.Select(oldSelectionStart, oldSelectionLength + prefix.Length + suffix.Length);
        }

        private void lnkPreviewWikitext_LinkClicked(object sender, EventArgs e)
        {
            frmPreview prv = new frmPreview();

            prv.CreateControl();
            IntPtr bogus = prv.Handle;  // seems needed, to force WinForms to actually create the form

            prv.BeginInvoke((MethodInvoker) delegate()
            {
                prv.ShowDialog();
            });

            StringDictionary query = new StringDictionary 
            {
                { "action", "parse" },
                { "prop", "text" },
                { "pst", "true" },
                { "text", txtCommonsText.Text },
                { "title", txtNormName.Text },
                { "disabletoc", "true" },
                { "disableeditsection", "true" },
                { "uselang", Localization.GetString("LanguageCode") }
            };
            MorebitsDotNet.PostApi(Wiki.Commons, query, delegate(XmlDocument doc)
            {
                XmlNodeList l = doc.GetElementsByTagName("parse");
                if (l.Count < 1)
                {
                    ErrorHandler(Localization.GetString("ParsePageFailed"), MessageBoxIcon.Information); // TODO - error message better
                    prv.Invoke((MethodInvoker) prv.Close);
                    return;
                }

                string pageHtml = l[0].InnerText;
                prv.Invoke((MethodInvoker) delegate() { prv.SetContent(pageHtml); });
            }, delegate(string msg)
            {
                ErrorHandler(msg);
                prv.Invoke((MethodInvoker) prv.Close);
            });
        }

        private void lnkGoogleImageSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://www.google.com/searchbyimage?hl=" + Localization.GetString("LanguageCode") + "&image_url=" + Uri.EscapeDataString(ImageInfos[SelectedRevisions[0]].Attributes["url"].Value));
            }
            catch (Exception)
            {
                ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }
    }
}
