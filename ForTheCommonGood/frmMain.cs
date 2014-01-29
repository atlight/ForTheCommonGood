using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        byte[] ImageData;

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

        // global resources
        Cursor ZoomInCursor;
        TextureBrush checker;

        class SimpleToolStripRenderer: ToolStripSystemRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // do nothing
            }
        }

        public frmMain()
        {
            InitializeComponent();

            panStatus.Tag = Color.FromArgb(30, 144, 255);
            panWarning.Tag = Color.FromArgb(178, 34, 34);

            ZoomInCursor = new Cursor(new MemoryStream(Properties.Resources.zoomin_cur));
            checker = new TextureBrush(Properties.Resources.Checker_16x16, System.Drawing.Drawing2D.WrapMode.Tile);

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
            lnkGoToFileLink.Text = Localization.GetString("Go_Button") + " →";
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
            textBox2.Text = welcome.ToString();

            toolBarLinks.Renderer = new SimpleToolStripRenderer();

            // time to load settings
            if (File.Exists("ForTheCommonGood.cfg"))
            {
                Settings.ReadSettings();
                if (Settings.SaveCreds == false)
                {
                    frmSettings set = new frmSettings(Settings.LocalUserName != "");
                    set.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show(Localization.GetString("Welcome1") + "\n\n" + Localization.GetString("Welcome2"),
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSettings_Click(this, null);
            }

            InitSettings();
        }

        void ErrorHandler(String msg)
        {
            MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            EnableForm(true);
        }

        string FormatTimestamp(XmlNode n)
        {
            return DateTime.Parse(n.Attributes["timestamp"].Value).ToUniversalTime().ToString("HH:mm, d MMMM yyyy", DateTimeFormatInfo.InvariantInfo);
        }

        string FormatIsoDate(XmlNode n)
        {
            return DateTime.Parse(n.Attributes["timestamp"].Value).ToUniversalTime().ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
        }

        string FormatDimensions(XmlNode n)
        {
            if (n.Attributes["width"].Value == "0")
                return n.Attributes["size"].Value + " bytes";  // not formatted, since it may be some weird format .NET doesn't know about
            else
                return int.Parse(n.Attributes["width"].Value).ToString("n0", CultureInfo.InvariantCulture) + " × " +
                    int.Parse(n.Attributes["height"].Value).ToString("n0", CultureInfo.InvariantCulture) + " (" +
                    int.Parse(n.Attributes["size"].Value).ToString("n0", CultureInfo.InvariantCulture) + " bytes)";
        }

        string GetCurrentLanguageCode()
        {
            string result = Settings.LocalDomain.Substring(0, Settings.LocalDomain.IndexOf('.'));
            if (result == "simple")
                return "en";
            if (result.Length > 3 && !result.Contains("-"))  // meta, species, etc
                return "";  // don't emit a language code template
            return result;
        }

        string GetCurrentInterwikiPrefix(bool forUvTemplate)
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

        enum WarningBoxType
        {
            Warning,
            Success
        }

        void AddWarningCore(Control ctl, WarningBoxType type)
        {
            Invoke(new Action(delegate()
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
            }));
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

        void EnableForm(bool enabled)
        {
            Invoke(new Action(delegate()
            {
                panRoot.Enabled = enabled;
                panStatus.Visible = !enabled;
            }));
        }

        string filename;  // the old local filename
        XmlNodeList iis;

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
            Invoke(new Action(delegate()
            {
                // enable those controls which are initially disabled
                btnTransfer.Enabled = txtNormName.Enabled = chkDeleteAfter.Enabled =
                    chkIgnoreWarnings.Enabled = toolBarLinks.Enabled = true;

                textBox2.Text = textBox3.Text = lblName.Text = lblRevision.Text = lblDimensions.Text = "";
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

                textBox1.Text = textBox1.Text.Trim();
            }));
            if (cl != null)
                cl.CancelAsync();

            filename = "File:" + Regex.Replace(textBox1.Text, @"^\w+:", "", RegexOptions.IgnoreCase);

            EnableForm(false);

            iis = null;
            ImageData = null;
            string text = "";
            MorebitsDotNet.ActionCompleted sentry = new MorebitsDotNet.ActionCompleted(2);
            sentry.Done += new Action(delegate()
            {
                if (iis == null)
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
                    if (problem.IsRegex ?
                        Regex.IsMatch(text, problem.Test, RegexOptions.IgnoreCase) :
                        textLowercase.Contains(problem.Test.ToLower()))
                    {
                        potentialProblems.Add("• " + problem.Message);
                    }
                }

                if (potentialProblems.Count > 0)
                {
                    Invoke(new Action(delegate()
                    {
                        foreach (string i in potentialProblems)
                            AddWarning(i, WarningBoxType.Warning);
                    }));
                }

                // start building the new file description page
                string origUploader = iis[iis.Count - 1].Attributes["user"].Value;

                // clean up local templates, etc.
                string prefix = GetCurrentInterwikiPrefix(false);
                text = Regex.Replace(text, LocalWikiData.CopyToCommonsRegex, "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                //text = Regex.Replace(text, "== ?(Summary|Licensing:?) ?== *\n", "\n");
                text = Regex.Replace(text, "== ?(" + LocalWikiData.Summary + ") ?== *\n", "", RegexOptions.IgnoreCase);//"== {{int:filedesc}} ==\n");
                text = Regex.Replace(text, "\n?\n?== ?(" + LocalWikiData.Licensing + ") ?== *\n", "\n\n== {{int:license-header}} ==\n", RegexOptions.IgnoreCase);
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
                    text = Regex.Replace(text, replacement.Key, replacement.Value.Replace("\\n", "\n"),
                        RegexOptions.IgnoreCase);
                }
                
                // amend self-license tags
                string beforeSelfTagCheck = text;
                foreach (KeyValuePair<string, string> replacement in LocalWikiData.SelfLicenseReplacements)
                {
                    text = Regex.Replace(text, replacement.Key, replacement.Value.Replace("\\n", "\n")
                        .Replace("%%OriginalUploader%%", origUploader).Replace("%%InterwikiLinkPrefix%%", prefix),
                        RegexOptions.IgnoreCase);
                }
                bool selfLicense = (text != beforeSelfTagCheck);

                text = text.Trim();

                // the character index at which the information tag finishes (doesn't have to be exact)
                int infoTagEnd = 0;

                if (!textLowercase.Contains("{{information") &&
                    !Regex.IsMatch(text, "{{" + LocalWikiData.Information, RegexOptions.IgnoreCase))
                {
                    string detectedDesc = Regex.Replace(text, "{{[^}]*}}", "", RegexOptions.IgnoreCase);
                    detectedDesc = Regex.Replace(detectedDesc, "==[^=]*==", "", RegexOptions.IgnoreCase);
                    detectedDesc = detectedDesc.Split('\n')[0];
                    if (detectedDesc.Length > 0)
                        text = text.Replace(detectedDesc, "");

                    XmlNode exifDateNode = iis[iis.Count - 1].SelectSingleNode("metadata/metadata[@name=\"DateTime\"]");
                    string exifDate = null;
                    if (exifDateNode != null)
                    {
                        exifDate = exifDateNode.Attributes["value"].Value;
                        if (Regex.IsMatch(exifDate, @"^\d\d\d\d:\d\d:\d\d") && !exifDate.StartsWith("0000"))
                            exifDate = exifDate.Substring(0, 10).Replace(':', '-');
                        else
                            exifDate = null;
                    }

                    string languageCode = GetCurrentLanguageCode();
                    // Note: pipe replacement was commented out because it caused problems with piped wikilinks in the
                    // detected description. Of course, now any literal pipes will cause problems...
                    var infoTag =
"== {{int:filedesc}} ==\n" +
"{{Information\n" +
"|Description    = " + (languageCode.Length > 0 ? ("{{" + languageCode + "|1=" + detectedDesc.Trim()/*.Replace("|", "&#124;")*/ + "}}") : detectedDesc.Trim().Replace("|", "&#124;")) + "\n" +
"|Date           = " + (exifDate != null ? "{{according to EXIF data|" + exifDate + "}}\n" : "{{original upload date|" + FormatIsoDate(iis[iis.Count - 1]) + "}}\n") +
"|Source         = {{own work by original uploader}} <!-- " + Localization.GetString("ChangeIfNotOwnWork") + " -->\n" +
"|Author         = " + (selfLicense ? ("[[" + prefix + ":User:" + iis[iis.Count - 1].Attributes["user"].Value + "|]]\n") : "\n") +
"|Permission     = \n" +
"|Other_versions = \n" +
"}}\n\n";
                    text = infoTag + text;

                    infoTagEnd = infoTag.Length - 8;  // -8 for sanity
                }
                else
                {
                    text = "== {{int:filedesc}} ==\n" + text;

                    if (!text.Contains("{{" + GetCurrentLanguageCode() + "|"))
                        text = Regex.Replace(text, @"({{Information[\r\n]* *\| ?Description *= *)([^\r\n ][^\r\n]+)([\r\n])",
                            "$1{{" + GetCurrentLanguageCode() + "|1=$2}}$3", RegexOptions.IgnoreCase);

                    if (!LocalWikiData.LocalDomain.StartsWith("en.wikipedia"))  // speed boost - this is unneeded on enwiki
                    {
                        text = Regex.Replace(text, @"{{\s*" + LocalWikiData.Information, "{{Information", RegexOptions.IgnoreCase);
                        text = Regex.Replace(text, @"\|\s*" + LocalWikiData.Description + @"\s*=", "|Description    =", RegexOptions.IgnoreCase);
                        text = Regex.Replace(text, @"\|\s*" + LocalWikiData.Date + @"\s*=", "|Date           =", RegexOptions.IgnoreCase);
                        text = Regex.Replace(text, @"\|\s*" + LocalWikiData.Source + @"\s*=", "|Source         =", RegexOptions.IgnoreCase);
                        text = Regex.Replace(text, @"\|\s*" + LocalWikiData.Author + @"\s*=", "|Author         =", RegexOptions.IgnoreCase);
                        text = Regex.Replace(text, @"\|\s*" + LocalWikiData.Permission + @"\s*=", "|Permission     =", RegexOptions.IgnoreCase);
                        text = Regex.Replace(text, @"\|\s*" + LocalWikiData.Other_versions + @"\s*=", "|Other_versions =", RegexOptions.IgnoreCase);
                    }

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

                text += "\n\n== {{Original upload log}} ==\n\n{{transferred from|" +
                    Settings.LocalDomain + "||ftcg}} {{original description page|" +
                    Settings.LocalDomain + "|" + Uri.EscapeDataString(filename.Substring(filename.IndexOf(':') + 1).Replace(' ', '_')) + "}}";

                text += "\n\n{| class=\"wikitable\"\n! {{int:filehist-datetime}} !! {{int:filehist-dimensions}} !! {{int:filehist-user}} !! {{int:filehist-comment}}";
                foreach (XmlNode n in iis)
                {
                    text += "\n|-\n| " + FormatTimestamp(n) + " || " + FormatDimensions(n);
                    text += " || {{uv|" + n.Attributes["user"].Value + "|" + GetCurrentInterwikiPrefix(true) +
                        ":}} || <nowiki>" + n.Attributes["comment"].Value + "</nowiki>";
                }
                text += "\n|}";

                // remove multiple line breaks
                text = Regex.Replace(text, @"[\r\n]{3,}", "\n\n");

                Invoke(new Action(delegate()
                    {
                        textBox3.Text = text.Replace("\n", "\r\n");
                        lnkLocalFile.Enabled = lnkGoogleImageSearch.Enabled = true;
                        lnkCommonsFile.Enabled = false;
                        lblFileLinks.Visible = lstFileLinks.Visible = true;
                        if (lstFileLinks.ForeColor == SystemColors.GrayText)
                        {
                            lstFileLinks.Items.Clear();
                            lstFileLinks.Items.Add(Localization.GetString("Loading"));
                        }
                        EnableForm(true);
                    }));
            });
            sentry.Finally += new Action(delegate()
            {
                Invoke(new Action(delegate()
                {
                    EnableForm(true);
                }));
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
                { "titles", filename + "|File talk:" + filename.Substring(5) },
                { "redirects", "true" }
            };
            MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
            {
                XmlNode filePage = doc.SelectSingleNode("//page[@ns=6]");  // was doc.GetElementsByTagName("page")[0]

                switch (filePage.Attributes["imagerepository"].Value)
                {
                    case "shared":
                        MessageBox.Show(Localization.GetString("AlreadyCommons"));
                        sentry.Fail();
                        return;
                    case "":
                        if (filePage.Attributes["missing"] != null)
                            MessageBox.Show(Localization.GetString("ImageMissing"));
                        else
                            MessageBox.Show(Localization.GetString("NoFile"));
                        sentry.Fail();
                        return;
                }

                iis = doc.GetElementsByTagName("ii");

                Invoke(new Action(delegate()
                {
                    lblName.Text = filePage.Attributes["title"].Value;

                    if (iis.Count > 1)
                    {
                        lblPastRevisions.Visible = btnPastRevisions.Visible = true;
                        lblPastRevisions.Text = ((iis.Count == 2) ?
                            Localization.GetString("OneEarlierVersion_Label") :
                            Localization.GetString("EarlierVersions_Format", (iis.Count - 1).ToString()));
                    }
                    else
                        lblPastRevisions.Visible = btnPastRevisions.Visible = false;
                }));

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
                                    filename.Substring(filename.IndexOf(':') + 1));
                            }
                            catch (Exception)
                            {
                                ErrorHandler(Localization.GetString("LinkVisitFailed"));
                            }
                        }, WarningBoxType.Warning);
                    }
                }

                // download the file and display a thumbnail (also display metadata)
                DownloadFileAndDisplayThumb(iis[0]);

                sentry.DoneOne();
            }, ErrorHandler);

            // get wikitext of file description page
            query = new StringDictionary 
            {
                { "action", "query" },
                { "prop", "revisions" },
                { "rvprop", "content" },
                { "titles", filename },
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

                Invoke(new Action(delegate()
                {
                    textBox2.Text = text.Replace("\n", "\r\n");
                }));

                XmlNodeList ns = doc.GetElementsByTagName("n");
                if (ns.Count > 0)
                    Invoke(new Action(delegate()
                    {
                        filename = ns[0].Attributes["to"].Value;
                    }));
                Invoke(new Action(delegate()
                {
                    txtNormName.Text = Regex.Replace(filename, @"^\w+:", "File:");
                }));

                sentry.DoneOne();
            }, ErrorHandler);

            // get file links (not in sentry)
            query = new StringDictionary 
            {
                { "action", "query" },
                { "list", "imageusage" },
                { "iulimit", "20" },
                { "iutitle", filename }
            };
            // prevent race conditions
            object current = new object();
            Invoke(new Action(delegate() { lstFileLinks.Tag = current; }));
            MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
            {
                if (lstFileLinks.Tag != current)
                    return;
                Invoke(new Action(delegate()
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
                }));
            }, ErrorHandler);
        }

        WebClient cl;

        // per-revision logic
        private void DownloadFileAndDisplayThumb(XmlNode n)
        {
            Invoke(new Action(delegate()
            {
                lblRevision.Text = (n.PreviousSibling == null ? Localization.GetString("CurrentVersion_Label") : Localization.GetString("OldVersion_Label")) +
                    " (" + FormatTimestamp(n) + ")";
                lblDimensions.Text = FormatDimensions(n);
                lblRevision.ForeColor = lblName.ForeColor = (n.PreviousSibling == null ? SystemColors.ControlText : Color.Red);

                XmlNodeList metadatas = n.SelectNodes("metadata/metadata");
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
            }));

            // decide whether PictureBox can display this image
            bool previewDownloadedFile = false;
            if (n.Attributes["thumburl"].Value == "")
            {
                // probably an OGG or something
                Invoke(new Action(delegate()
                {
                    pictureBox1.Image = pictureBox1.ErrorImage;
                }));
            }
            else
            {
                switch (n.Attributes["mime"].Value)
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
                                Invoke(new Action(delegate()
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
                                }));  // , true, true

                            });
                        clThumb.DownloadDataAsync(new Uri(n.Attributes["thumburl"].Value));
                        break;
                }
            }

            // download file
            cl = new WebClient();
            cl.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
            cl.DownloadDataCompleted += new DownloadDataCompletedEventHandler(
                delegate(object s, DownloadDataCompletedEventArgs v)
                {
                    if (v.Cancelled)
                        return;
                    if (v.Error != null)
                    {
                        MessageBox.Show(Localization.GetString("FailedToDownload") + "\n\n" + v.Error.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ImageData = v.Result;
                    if (previewDownloadedFile)
                    {
                        Invoke(new Action(delegate()
                        {
                            try
                            {
                                Image img = Image.FromStream(new MemoryStream(ImageData, false));
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
                        }));  // , true, true
                    }
                });
            cl.DownloadDataAsync(new Uri(n.Attributes["url"].Value));
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
                    StringDictionary query = new StringDictionary 
                    {
                        { "action", "query" },
                        { "list", "categorymembers" },
                        { "cmtitle", "Category:" + SourceTag },
                        { "cmnamespace", "6" },  // File:, obviously
                        { "cmsort", "timestamp" },
                        { "cmprop", "title" },
                        { "cmlimit", "500" },
                    };
                    if (RandomContinue != null)
                        query.Add("cmstart", RandomContinue);
                    MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
                    {
                        foreach (XmlNode i in doc.GetElementsByTagName("cm"))
                            RandomImageCache.Add(i.Attributes["title"].Value);
                        if (RandomImageCache.Count == 0)
                        {
                            MessageBox.Show(Localization.GetString("NoMoreFiles"));
                            EnableForm(true);
                            return;
                        }

                        XmlNodeList continues = doc.GetElementsByTagName("query-continue");
                        if (continues.Count > 0)
                            RandomContinue = continues[0].FirstChild.Attributes["cmstart"].Value;
                        else
                            RandomContinue = null;

                        RandomImageCategoryCore();
                    }, ErrorHandler);
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
                MessageBox.Show(Localization.GetString("RandomCacheEmpty1") + "\n\n" +
                    Localization.GetString("RandomCacheEmpty2") + "\n\n" +
                    Localization.GetString("RandomCacheEmpty3"));
                RandomImageCache.Clear();
                EnableForm(true);
                return;
            }

            int randIndex;
            do
            {
                randIndex = rand.Next(RandomImageCache.Count);
            } while (RandomBlacklist.Contains(randIndex));
            Invoke(new Action(delegate()
            {
                textBox1.Text = RandomImageCache[randIndex];
            }));
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
            Invoke(new Action(delegate()
            {
                textBox1.Text = TextFileCache[0];
            }));
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
                    form.txtCategory.Text.ToLower().StartsWith("category:") ? "category:".Length : 0);
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

            if (ImageData == null)
            {
                MessageBox.Show(Localization.GetString("StillDownloading"));
                return;
            }

            if (lblRevision.ForeColor == Color.Red && MessageBox.Show(Localization.GetString("OldVersionTransferAdvice"), Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;

            EnableForm(false);

            Action action = delegate()
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
                    if (doc.GetElementsByTagName("page")[0].Attributes["missing"] == null && !chkIgnoreWarnings.Checked)
                    {
                        MessageBox.Show(Localization.GetString("FilenameClash"));
                        EnableForm(true);
                        return;
                    }

                    string newFilename = Regex.Replace(txtNormName.Text, "^(Image|File):", "");
                    string token = doc.GetElementsByTagName("page")[0].Attributes["edittoken"].Value;

                    StringDictionary uploadQuery = new StringDictionary 
                    {
                        { "action", "upload" },
                        { "filename", newFilename },
                        { "text", textBox3.Text.Replace("<!-- " + Localization.GetString("ChangeIfNotOwnWork") + " -->", "") },
                        // Note: this upload comment is not localised, since Commons uses English as lingua franca
                        { "comment", "Transferred from " + Settings.LocalDomain + ": see original upload log above" },
                        { "token", token }
                    };
                    if (chkIgnoreWarnings.Checked)
                        uploadQuery.Add("ignorewarnings", "true");
                    MorebitsDotNet.UploadFile(Wiki.Commons, uploadQuery, ImageData, newFilename, "file", delegate(XmlDocument innerDoc)
                    {
                        // assuming success...

                        Invoke(new Action(delegate()
                        {
                            lnkCommonsFile.Enabled = true;
                            lnkCommonsFile.Tag = txtNormName.Text;
                        }));

                        XmlNodeList warnings = innerDoc.GetElementsByTagName("warnings");
                        if (warnings.Count > 0 && !chkIgnoreWarnings.Checked)
                        {
                            MessageBox.Show(Localization.GetString("Warnings1") + "\n\n" + warnings[0].OuterXml + "\n\n" + Localization.GetString("Warnings2"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            EnableForm(true);
                            return;
                        }

                        // log the transfer, if requested
                        if (Settings.LogTransfers)
                        {
                            string logText = "# " +
                                DateTime.UtcNow.ToString(Localization.GetString("LogFileTimestampFormat"), System.Globalization.DateTimeFormatInfo.InvariantInfo) +
                                " (UTC): ";
                            if (filename == txtNormName.Text)
                                logText += "[[:" + txtNormName.Text + "]]\r\n";
                            else
                                logText += "[[:" + filename + "]] → [[:" + txtNormName.Text + "]]\r\n";
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
                        Action showSuccess = delegate()
                        {
                            AddWarning(Localization.GetString("DontForgetToCategorize_Label"), WarningBoxType.Success);
                            AddWarning(Localization.GetString("HotcatHint_Label"), WarningBoxType.Success);
                            if (Settings.OpenBrowserLocal)
                                lnkLocalFile_LinkClicked(null, null);
                            if (Settings.OpenBrowserAutomatically)
                                lnkCommonsFile_LinkClicked(null, null);
                            if (CurrentFileSource != FileSources.Category)
                                Invoke(new Action(delegate() { btnRandomFile.Focus(); }));
                        };

                        // finished?
                        if (!chkDeleteAfter.Checked)
                        {
                            EnableForm(true);
                            showSuccess();
                            return;
                        }

                        // continue with deleting/tagging with {{now Commons}}

                        Action innerAction = delegate()
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
                                { "titles", filename },  // old filename
                                { "rvprop", "content" }
                            };
                            MorebitsDotNet.PostApi(Wiki.Local, enTokenQuery, delegate(XmlDocument enDoc)
                            {
                                if (enDoc.GetElementsByTagName("page")[0].Attributes["missing"] != null)
                                {
                                    MessageBox.Show(Localization.GetString("LocalFileDeleted"));
                                    EnableForm(true);
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
                                    { "title", filename },
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
                                        MessageBox.Show(Localization.GetString("NowCommonsFailed") + " " + editResult, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    }, ErrorHandler);
                }, ErrorHandler);
            };

            if (!MorebitsDotNet.LoginSessions[Wiki.Commons].LoggedIn)
                MorebitsDotNet.LogIn(Wiki.Commons, Settings.CommonsUserName, Settings.CommonsPassword,
                    action, ErrorHandler);
            else
                action();
        }

        private void DeleteLocal()
        {
            EnableForm(false);

            Action action = delegate()
            {
                StringDictionary query = new StringDictionary 
                {
                    { "action", "query" },
                    { "prop", "info" },
                    { "intoken", "delete" },
                    { "titles", filename },
                    { "redirects", "true" }
                };
                MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
                {
                    if (doc.GetElementsByTagName("page")[0].Attributes["missing"] != null)
                    {
                        MessageBox.Show(Localization.GetString("AlreadyDeleted"));
                        EnableForm(true);
                        return;
                    }

                    string token = doc.GetElementsByTagName("page")[0].Attributes["deletetoken"].Value;
                    StringDictionary deleteQuery = new StringDictionary 
                    {
                        { "action", "delete" },
                        { "reason", LocalWikiData.NowCommonsDeletionSummary + ": [[" + lnkCommonsFile.Tag.ToString() + "]]" },
                        { "token", token },
                        { "title", filename },
                        { "redirects", "true" }
                    };

                    MorebitsDotNet.PostApi(Wiki.Local, deleteQuery, delegate(XmlDocument innerDoc)
                    {
                        EnableForm(true);
                        AddWarning(Localization.GetString("LooksGood"), WarningBoxType.Success);
                        AddWarning(Localization.GetString("DontForgetToCategorize_Label"), WarningBoxType.Success);
                        if (Settings.OpenBrowserLocal)
                            lnkLocalFile_LinkClicked(null, null);
                        if (Settings.OpenBrowserAutomatically)
                            lnkCommonsFile_LinkClicked(null, null);
                        if (CurrentFileSource != FileSources.Category)
                            Invoke(new Action(delegate() { btnRandomFile.Focus(); }));
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
        //                MessageBox.Show(Localization.GetString("LocalFileDeleted"));
        //                EnableForm(true);
        //                return;
        //            }

        //            string enToken = enDoc.GetElementsByTagName("page")[0].Attributes["edittoken"].Value;

        //            string enText = enDoc.GetElementsByTagName("rev")[0].FirstChild.Value;
        //            string newText =  Regex.Replace(enText, LocalWikiData.CopyToCommonsRegex, "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //            if (enText == newText)
        //            {
        //                MessageBox.Show(Localization.GetString("CouldNotFindTag"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //                EnableForm(true);
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
        //                    MessageBox.Show(Localization.GetString("FailedPlus") + " " + editResult, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            frmSettings set = new frmSettings(false);
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

            // local wiki data
            LocalWikiData.LoadWikiData((Settings.LocalWikiData == "" ? Properties.Resources.en_wikipedia : Settings.LocalWikiData)
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
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
                Process.Start(MorebitsDotNet.GetProtocol() + "://" + Settings.LocalDomain + ".org/wiki/" + filename);
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
                Process.Start(MorebitsDotNet.GetProtocol() + "://commons.wikimedia.org/wiki/" + lnkCommonsFile.Tag.ToString());
            }
            catch (Exception)
            {
                ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }

        private void SelectVersion(object sender, EventArgs e)
        {
            frmRevisionBrowse form = new frmRevisionBrowse();
            bool first = true;

            foreach (XmlNode i in iis)
            {
                ListViewItem item = new ListViewItem(new string[] {
                    "", 
                    (first ? (Localization.GetString("CurrentVersion_Label") + "\n") : "") + FormatTimestamp(i),
                    FormatDimensions(i),
                    i.Attributes["user"].Value,
                    i.Attributes["comment"].Value
                });
                item.Tag = i;

                // download thumbnail
                if (i.Attributes["thumburl"].Value != "")
                {
                    WebClient cl = new WebClient();
                    cl.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
                    cl.DownloadDataCompleted += new DownloadDataCompletedEventHandler(
                        delegate(object s, DownloadDataCompletedEventArgs v)
                        {
                            // sometimes thumbnails can't be generated and the server gives a 404
                            if (v.Error != null)
                            {
                                Invoke(new Action(delegate()
                                {
                                    Bitmap img = new Bitmap(150, 150);
                                    using (Graphics g = Graphics.FromImage(img))
                                    {
                                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                                        g.DrawString(Localization.GetString("FailedToGenerateThumbnail1_Label") + "\n" +
                                            Localization.GetString("FailedToGenerateThumbnail2_Label") + "\n" +
                                            Localization.GetString("FailedToGenerateThumbnail3_Label"), Font, Brushes.Black, 5, 5);
                                    }
                                    lock (form)
                                    {
                                        form.imageList.Images.Add(img);
                                        item.ImageIndex = form.imageList.Images.Count - 1;
                                    }
                                }));
                                return;
                            }

                            byte[] data = v.Result;
                            Invoke(new Action(delegate()
                            {
                                Image original = Image.FromStream(new MemoryStream(data, false));
                                Bitmap img = new Bitmap(150, 150, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                using (Graphics g = Graphics.FromImage(img))
                                {
                                    g.FillRectangle(checker, 0, 0, img.Width, img.Height);
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
                            }));
                        });
                    cl.DownloadDataAsync(new Uri(i.Attributes["thumburl"].Value));
                }
                if (first)
                {
                    item.UseItemStyleForSubItems = false;
                    item.SubItems[1].Font = new Font(form.Font, FontStyle.Bold);
                }
                form.listView.Items.Add(item);

                first = false;
            }

            if (form.ShowDialog() == DialogResult.Cancel)
                return;

            if (form.listView.SelectedItems.Count != 1)
            {
                MessageBox.Show(Localization.GetString("NoRevisionSelected"));
                return;
            }

            ImageData = null;
            DownloadFileAndDisplayThumb((XmlNode) form.listView.SelectedItems[0].Tag);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmMain_Resize(sender, e);

            // check for updates
            if (Settings.AutoUpdate)
            {
                WebClient autoupdate = new WebClient();
                autoupdate.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
                autoupdate.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                autoupdate.DownloadStringCompleted += delegate(object obj, DownloadStringCompletedEventArgs resp)
                {
                    if (resp.Error != null)
                        return;   // fail silently for the moment

                    try
                    {
                        Version newVersion = new Version(resp.Result.Split('\n')[1]);
                        if (newVersion > new Version(Application.ProductVersion))
                        {
                            new frmUpdateAvailable(newVersion).ShowDialog();
                        }
                    }
                    catch (Exception)
                    {
                        // don't really care - fail silently
                    }
                };
                autoupdate.DownloadStringAsync(new Uri("http://en.wikipedia.org/w/index.php?action=raw&ctype=text/css&title=User:This,%20that%20and%20the%20other/FtCG%20current%20version.css"));
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
                    original = Image.FromStream(new MemoryStream(ImageData));
                }
                catch (Exception)
                {
                    ErrorHandler(Localization.GetString("LightboxFailed"));
                    return;
                }
                Bitmap img = new Bitmap(original.Width, original.Height);
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.FillRectangle(checker, 0, 0, img.Width, img.Height);
                    g.DrawImage(original, 0, 0, img.Width, img.Height);
                }
                BackgroundImage = img;
                if (img.Height > ClientSize.Height || img.Width > ClientSize.Width)
                    BackgroundImageLayout = ImageLayout.Zoom;
                else
                    BackgroundImageLayout = ImageLayout.Center;
                BackColor = Color.DimGray;

                Click += AbandonLightbox;
                panRoot.Visible = false;
            }
        }

        void AbandonLightbox(object s, EventArgs a)
        {
            BackgroundImage = null;
            BackColor = DefaultBackColor;
            panRoot.Visible = true;
            Click -= AbandonLightbox;
        }

        private void lstFileLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkGoToFileLink.Enabled = (lstFileLinks.SelectedIndex != -1 &&
                !lstFileLinks.SelectedItem.ToString().StartsWith("<<"));
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
            int oldSelectionStart = textBox3.SelectionStart;
            int oldSelectionLength = textBox3.SelectionLength;
            textBox3.Text = textBox3.Text.Insert(oldSelectionStart + oldSelectionLength, "|]]")
                .Insert(oldSelectionStart, "[[" + GetCurrentInterwikiPrefix(false) + ":");
            textBox3.Focus();
            textBox3.Select(oldSelectionStart, oldSelectionLength + 9);
        }

        private void lnkPreviewWikitext_LinkClicked(object sender, EventArgs e)
        {
            frmPreview prv = new frmPreview();

            prv.CreateControl();
            IntPtr bogus = prv.Handle;  // seems needed, to force WinForms to actually create the form

            prv.BeginInvoke(new Action(delegate()
            {
                prv.ShowDialog();
            }));

            StringDictionary query = new StringDictionary 
            {
                { "action", "parse" },
                { "prop", "text" },
                { "pst", "true" },
                { "text", textBox3.Text },
                { "title", txtNormName.Text }
            };
            MorebitsDotNet.PostApi(Wiki.Commons, query, delegate(XmlDocument doc)
            {
                XmlNodeList l = doc.GetElementsByTagName("parse");
                if (l.Count < 1)
                {
                    MessageBox.Show(Localization.GetString("ParsePageFailed"),
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); // TODO - error message better
                    prv.Invoke(new Action(prv.Close));
                    return;
                }

                // remove section-edit links
                string pageHtml = l[0].InnerText;
                pageHtml = Regex.Replace(pageHtml, @"<span class=""editsection"">\[(.+)\]</span>\s*", "");

                prv.Invoke(new Action(delegate()
                    {
                        prv.webBrowser1.Document.Write(frmPreview.css + pageHtml);
                    }));

            }, delegate(string msg)
            {
                ErrorHandler(msg);
                prv.Invoke(new Action(prv.Close));
            });
        }

        private void lnkGoogleImageSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://www.google.com/searchbyimage?hl=" + Localization.GetString("LanguageCode") + "&image_url=" + Uri.EscapeDataString(iis[0].Attributes["url"].Value));
            }
            catch (Exception)
            {
                ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }
    }
}
