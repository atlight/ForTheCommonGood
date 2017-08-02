using System;
using System.IO;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmRandomSource: Form
    {
        public frmRandomSource()
        {
            InitializeComponent();

            Text = Localization.GetString("RandomSource_WindowTitle");
            optCategory.Text = Localization.GetString("FilesInCategory_Option");
            optUserUpload.Text = Localization.GetString("FilesUploadedByUser_Option");
            optTextFile.Text = Localization.GetString("FilesFromTextFile_Option");
            btnFileNameBrowse.Text = Localization.GetString("Browse_Button");
            btnOK.Text = Localization.GetString("OK_Button");
            btnCancel.Text = Localization.GetString("Cancel_Button");
            openFileDialog1.Title = Localization.GetString("OpenTextFile_WindowTitle");

            CheckChange(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.Cancel)
                return;
            txtFileName.Text = openFileDialog1.FileName;
        }

        private void CheckChange(object sender, EventArgs e)
        {
            txtCategory.Enabled = optCategory.Checked;
            txtUserName.Enabled = optUserUpload.Checked;
            txtFileName.Enabled = btnFileNameBrowse.Enabled = optTextFile.Checked;
        }

        private void frmRandomSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (optTextFile.Checked && DialogResult == DialogResult.OK && !File.Exists(txtFileName.Text) &&
                MessageBox.Show(Localization.GetString("FileNotFound_Label", txtFileName.Text),
                Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                e.Cancel = true;
        }
    }
}
