using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class CoolCatItem: UserControl
    {
        /// <summary>
        /// Is the user currently editing the category name?
        /// </summary>
        private bool IsEditing = false;

        /// <summary>
        /// Is this a "brand new" CoolCatItem control in its first editing session?
        /// </summary>
        private bool IsInitialEditing = true;

        private CoolCat ParentCoolCat;

        public CoolCatItem(CoolCat parentCoolCat)
        {
            ParentCoolCat = parentCoolCat;

            InitializeComponent();

            btnOK.Text = Localization.GetString("OK_Button");
            btnCancel.Text = Localization.GetString("Cancel_Button");

            EnterEditingMode();
            SetCategoryExistenceIcon(false);

            toolTips.SetToolTip(btnRemove, Localization.GetString("RemoveCategory_Tooltip"));
            toolTips.SetToolTip(btnModify, Localization.GetString("ModifyCategory_Tooltip"));
            //toolTips.SetToolTip(btnParentCat, "Explore parent categories");
            //toolTips.SetToolTip(btnSubCat, "Explore subcategories");
        }

        private void CoolCatItem_Load(object sender, EventArgs e)
        {
        }

        private void EnterEditingMode()
        {
            cboCatName.Text = lnkCatLink.Text;

            panView.Hide();
            panEdit.Show();
            Width = btnCancel.Right + 1;
            cboCatName.Focus();

            IsEditing = true;
        }

        public event EventHandler LeftEditingMode;

        private void LeaveEditingMode()
        {
            IsEditing = false;
            IsInitialEditing = false;

            panEdit.Hide();
            panView.Show();
            Width = panView.Width;
            panEdit.Dock = DockStyle.None;

            if (LeftEditingMode != null)
                LeftEditingMode(this, EventArgs.Empty);
        }

        private void SetCategoryExistenceIcon(bool exists)
        {
            picIcon.Image = exists ? Properties.Resources.Small_check : Properties.Resources.Small_cross;
            toolTips.SetToolTip(picIcon, Localization.GetString(exists ? "CategoryExists_Tooltip" : "CategoryDoesNotExist_Tooltip"));
        }

        public override bool AutoSize
        {
            get
            {
                return false;
            }
        }

        public override string Text
        {
            get
            {
                if (InvokeRequired)
                {
                    string result = null;
                    Invoke((MethodInvoker) delegate() { result = IsEditing ? cboCatName.Text : lnkCatLink.Text; });
                    return result;
                }
                else
                {
                    return IsEditing ? cboCatName.Text : lnkCatLink.Text;
                }
            }
            set
            {
                int oldWidth = lnkCatLink.Width;
                lnkCatLink.Text = value;
                toolTips.SetToolTip(lnkCatLink, "Category:" + value);
                Width = panView.Width;
            }
        }

        /// <summary>
        /// Occurs when the "Remove" button is clicked by the user.
        /// </summary>
        public event EventHandler RemoveClicked;

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (RemoveClicked != null)
                RemoveClicked(this, EventArgs.Empty);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            EnterEditingMode();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            LeaveEditingMode();
            Text = cboCatName.Text.Trim();
            if (Text == "" && RemoveClicked != null)
                RemoveClicked(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsInitialEditing && RemoveClicked != null)
                RemoveClicked(this, EventArgs.Empty);
            LeaveEditingMode();
        }

        private void lnkCatLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://" + Settings.CommonsDomain + ".org/wiki/Category:" + lnkCatLink.Text);
            }
            catch (Exception)
            {
                //ErrorHandler(Localization.GetString("LinkVisitFailed"));
            }
        }

        private void cboCatName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                if (cboCatName.DroppedDown)
                    cboCatName.DroppedDown = false;
                else
                    btnCancel_Click(sender, e);
            }
            if (e.KeyData == Keys.Return)
                btnOK_Click(sender, e);

        }

        string textUpdateTarget;
        DateTime latestDate;

        private void cboCatName_TextChanged(object sender, EventArgs e)
        {
            if (cboCatName.Text.Length == 0 || textUpdateTarget != cboCatName.Text)
                return;

            // needed to avoid overzealous mouse pointer hiding on Windows
            PlatformSpecific.FakeMouseMovement();

            DateTime now = DateTime.Now;
            latestDate = now;

            // use same queries as HotCat (via GET), to take advantage of server-side caching
            MorebitsDotNet.GetApi(Wiki.Commons,
                "format=json&action=opensearch&namespace=14&limit=30&search=Category:" + Uri.EscapeDataString(cboCatName.Text),
                now, delegate(string json, object date)
                {
                    if (latestDate > ((DateTime) date))
                        return;

                    // we take advantage of the fact that [ is not a legal character in wiki page titles
                    // To be honest, this whole thing is crass, but I don't want to bring in a full JSON
                    // parser just for this
                    string resultsPart = json.Substring(json.IndexOf('[', 2) + 2);
                    string[] results;
                    if (resultsPart.Length < 2)
                    {
                        // no results
                        results = new string[0];
                    }
                    else
                    {
                        // we have results, so parse out character escapes
                        resultsPart.Replace("\\\\", "#");
                        int location;
                        string charValue;
                        while ((location = resultsPart.IndexOf("\\u")) != -1)
                        {
                            charValue = resultsPart.Substring(location + 2, 4);
                            resultsPart = resultsPart.Remove(location, 6).Insert(location,
                                Char.ConvertFromUtf32(int.Parse(charValue, NumberStyles.AllowHexSpecifier)));
                        }
                        resultsPart = resultsPart.Replace("\\\"", "\"").Replace("#", "\\");
                        results = resultsPart.Substring(0, resultsPart.Length - 3).Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    }
                    //MessageBox.Show(json + "\n\n====================\n\n" + string.Join("\n", results));

                    ParentCoolCat.AddCategoryExistenceRange(results, true);

                    if (ParentCoolCat.DoesCategoryExist(cboCatName.Text) != true)
                    {
                        // does it exist? Go looking
                        MorebitsDotNet.GetApi(Wiki.Commons,
                            "format=json&action=query&titles=Category:" + Uri.EscapeDataString(cboCatName.Text),
                            cboCatName.Text, delegate(string innerJson, object catName)
                            {
                                // more crassness
                                bool exists = innerJson.IndexOf("\"-1\":") == -1;
                                SetCategoryExistenceIcon(exists);
                                ParentCoolCat.AddCategoryExistence((string) catName, exists);

                                FinishCategoryAutoComplete(results, exists ? (string) catName : null);
                            }
                        );
                    }
                    else
                    {
                        SetCategoryExistenceIcon(true);
                        FinishCategoryAutoComplete(results, null);
                    }
                }
            );
        }

        private void FinishCategoryAutoComplete(Array results, string extraResult)
        {
            // This code causes hangs under Mono. Need to fix
            if (PlatformSpecific.IsMono())
                return;
            
            string text = cboCatName.Text;

            cboCatName.Items.Clear();
            if (results.Length > 0)
            {
                //Array.Sort(results);
                foreach (string i in results)
                    cboCatName.Items.Add(i.Substring("Category:".Length));
            }
            if (extraResult != null)
                cboCatName.Items.Add(CoolCat.NormalizeCatName(extraResult));

            if (cboCatName.Items.Count == 0)
            {
                cboCatName.Items.Add("");
            }

            if (!cboCatName.DroppedDown && cboCatName.Visible)
            {
                cboCatName.DroppedDown = true;
                cboCatName.Text = text;
            }
            cboCatName.SelectionStart = text.Length;
        }

        private void cboCatName_TextUpdate(object sender, EventArgs e)
        {
            textUpdateTarget = cboCatName.Text;
        }

        private void cboCatName_DropDownClosed(object sender, EventArgs e)
        {
            textUpdateTarget = null;
            bool? exists = ParentCoolCat.DoesCategoryExist(cboCatName.Text);
            if (exists != null)
                SetCategoryExistenceIcon(exists.Value);
        }
    }
}
