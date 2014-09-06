using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class CoolCat: UserControl
    {
        // This control isn't really designed for use outside FtCG. For example, it
        // essentially ignores its own Font property. Kind of disrespectful.

        private List<CoolCatItem> Items = new List<CoolCatItem>();

        public CoolCat()
        {
            InitializeComponent();

            ExistingCategoryNames = new Dictionary<string, bool>(600);
        }

        private void CoolCat_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                toolTips.SetToolTip(btnAdd, Localization.GetString("AddCategory_Tooltip"));
        }

        /// <summary>
        /// Gets the set of categories currently shown in the control.
        /// </summary>
        [Browsable(false)]
        public string[] Categories
        {
            get
            {
                return Array.ConvertAll<CoolCatItem, string>(Items.ToArray(), x => x.Text);
            }
        }

        public void ClearCategories()
        {
            foreach (CoolCatItem i in Items)
                flowLayout.Controls.Remove(i);
            Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CoolCatItem item = new CoolCatItem(this);
            Items.Add(item);
            flowLayout.Controls.Add(item);
            flowLayout.Controls.SetChildIndex(item, flowLayout.Controls.Count - 2);

            item.TabIndex = Items.Count;
            item.RemoveClicked += new EventHandler(item_RemoveClicked);
            item.LeftEditingMode += new EventHandler(item_LeftEditingMode);
            item.Focus();
        }

        void item_RemoveClicked(object sender, EventArgs e)
        {
            CoolCatItem item = sender as CoolCatItem;
            Items.Remove(item);
            flowLayout.Controls.Remove(item);
        }

        void item_LeftEditingMode(object sender, EventArgs e)
        {
            btnAdd.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            flowLayout.MaximumSize = new Size(Width, flowLayout.MaximumSize.Height);
            flowLayout.Width = flowLayout.MaximumSize.Width;
            base.OnResize(e);
        }

        // Cache of existing category names. Has to be somewhat thread-safe

        private Dictionary<string, bool> ExistingCategoryNames;

        public static string NormalizeCatName(string category)
        {
            string result = category.Trim().Replace("_", " ");
            if (result.IndexOf("Category:") == 0)
                result = result.Substring("Category:".Length);
            return result.Length < 2 ? result.ToUpperInvariant() : 
                (result.Substring(0, 1).ToUpperInvariant() + result.Substring(1));
        }

        // true: exists, false: does not exist, null: unknown
        internal bool? DoesCategoryExist(string category)
        {
            category = NormalizeCatName(category);
            return ExistingCategoryNames.ContainsKey(category) ? (bool?) ExistingCategoryNames[category] : null;
        }

        internal void AddCategoryExistence(string category, bool exists)
        {
            category = NormalizeCatName(category);
            // always write to ExistingCategoryNames from the control's thread
            Invoke((MethodInvoker) delegate() {
                if (!ExistingCategoryNames.ContainsKey(category))
                    ExistingCategoryNames.Add(category, exists);
            });
        }

        internal void AddCategoryExistenceRange(string[] categories, bool exists)
        {
            // always write to ExistingCategoryNames from the control's thread
            Invoke((MethodInvoker) delegate()
            {
                foreach (string i in categories)
                {
                    string keyToAdd = NormalizeCatName(i);
                    if (!ExistingCategoryNames.ContainsKey(keyToAdd))
                        ExistingCategoryNames.Add(keyToAdd, exists);
                }
            });
        }
    }
}
