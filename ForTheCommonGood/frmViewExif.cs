using System;
using System.Collections;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmViewExif: Form
    {
        public frmViewExif()
        {
            InitializeComponent();
            lstExif_Resize(null, null);

            Text = Localization.GetString("ExifViewer_WindowTitle");
            label1.Text = Localization.GetString("ExifDataForVersion_Label");
            colName.Text = Localization.GetString("Name_Column");
            colValue.Text = Localization.GetString("Value_Column");
            button1.Text = Localization.GetString("Close_Button");
        }

        private void lstExif_Resize(object sender, EventArgs e)
        {
            colName.Width = colValue.Width = ((lstExif.Width - SystemInformation.VerticalScrollBarWidth) / 2) - 6;
        }

        class ListViewItemComparer: IComparer
        {
            int col;
            bool asc;
            public ListViewItemComparer(int column, bool ascending)
            {
                col = column;
                asc = ascending;
            }
            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewItem) (asc ? x : y)).SubItems[col].Text, ((ListViewItem) (asc ? y : x)).SubItems[col].Text);
            }
        }

        int prevCol;
        bool ascending;

        private void lstExif_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == prevCol)
            {
                ascending = !ascending;
            }
            else
            {
                prevCol = e.Column;
                ascending = true;
            }
            lstExif.ListViewItemSorter = new ListViewItemComparer(e.Column, ascending);
            lstExif.Sort();
        }
    }
}
