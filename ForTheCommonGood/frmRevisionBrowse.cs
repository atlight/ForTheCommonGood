using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmRevisionBrowse: Form
    {
        public frmRevisionBrowse()
        {
            InitializeComponent();

            Text = Localization.GetString("SelectVersion_WindowTitle");
            colComment.Text = Localization.GetString("Comment_Column");
            colDateTime.Text = Localization.GetString("DateTime_Column");
            colDimensions.Text = Localization.GetString("Dimensions_Column");
            colPreview.Text = Localization.GetString("Preview_Column");
            colUser.Text = Localization.GetString("UserName_Column");
            button1.Text = Localization.GetString("OK_Button");
            button2.Text = Localization.GetString("Cancel_Button");
        }
    }
}
