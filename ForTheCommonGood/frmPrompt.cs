using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ForTheCommonGood
{
    public partial class frmPrompt: Form
    {
        public frmPrompt()
        {
            InitializeComponent();
            Text = Application.ProductName;
            button2.Text = Localization.GetString("OK_Button");
            button1.Text = Localization.GetString("Cancel_Button");
        }

        public static string Prompt(string question)
        {
            return Prompt(question, "");
        }

        public static string Prompt(string question, string initialText)
        {
            frmPrompt prompt = new frmPrompt();
            prompt.lblPrompt.Text = question;
            prompt.txtReply.Text = initialText;
            prompt.txtReply.Focus();
            prompt.txtReply.SelectAll();
            if (prompt.ShowDialog() == DialogResult.Cancel)
                return null;
            return prompt.txtReply.Text;
        }
    }
}
