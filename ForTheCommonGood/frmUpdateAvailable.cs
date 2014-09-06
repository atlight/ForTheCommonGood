using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmUpdateAvailable: Form
    {
        const string DownloadSite = "http://atlight.github.io";
        
        public frmUpdateAvailable(Version newVersion)
        {
            InitializeComponent();

            panBack.Tag = Color.FromArgb(30, 144, 255);

            Text = lblTitle.Text = Localization.GetString("NewVersionTitle");
            lblNoticePre.Text = Localization.GetString("NewVersionNotice1", newVersion.ToString());
            button1.Text = Localization.GetString("OK_Button");

            lblNoticeLink.Text = Localization.GetString("NewVersionNotice2", DownloadSite);
            if (Localization.GetString("NewVersionNotice2").Contains("{0}"))
                lblNoticeLink.LinkArea = new LinkArea(Localization.GetString("NewVersionNotice2").IndexOf("{0}"),
                    DownloadSite.Length);

            string watchlink = Localization.GetString("NewVersionNoticeWatchLink");
            lblNoticePost.Text = Localization.GetString("NewVersionNotice3", watchlink);
            if (Localization.GetString("NewVersionNotice3").Contains("{0}"))
                lblNoticePost.LinkArea = new LinkArea(Localization.GetString("NewVersionNotice3").IndexOf("{0}"),
                    watchlink.Length);
        }

        private void lblNoticeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(DownloadSite);
            }
            catch (Exception)
            {
                MessageBox.Show(this, Localization.GetString("LinkVisitFailed"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(MorebitsDotNet.GetProtocol() + "://en.wikipedia.org/w/index.php?title=User_talk:This,_that_and_the_other/For_the_Common_Good&action=watch");
            }
            catch (Exception)
            {
                MessageBox.Show(this, Localization.GetString("LinkVisitFailed"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void panBack_Paint(object sender, PaintEventArgs e)
        {
            Control ctl = (Control) sender;
            e.Graphics.Clear(ctl.BackColor);
            e.Graphics.DrawRectangle(new Pen((Color) ctl.Tag, 6),
                Rectangle.FromLTRB(0, 0, ctl.Width, ctl.Height));
        }
    }
}
