using System;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmPreview: Form
    {
        public frmPreview()
        {
            InitializeComponent();

            Text = Localization.GetString("PreviewCommonsWikitext_Hyperlink");
            button1.Text = Localization.GetString("Close_Button");

            SetContent("<center style=\"font: 10pt Tahoma;\"><b>Loading...</b></center>");
        }

        public void SetContent(string html)
        {
            html = html.Replace("<head>", "<head>" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />" +
                "<base href=\"http://" + MorebitsDotNet.GetDomain(Wiki.Commons) + "/wiki/x\" target=\"_blank\" />")
                .Replace("</head>", "</head>" +
                "<style>body { font-size: 0.85em; background-color: white; color: black; padding: 0.8em; }</style>");
            if (webBrowser1.Document == null)
                webBrowser1.DocumentText = html;
            else
                webBrowser1.Document.Write(html);
        }
    }
}
