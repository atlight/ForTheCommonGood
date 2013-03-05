namespace ForTheCommonGood
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCommonsUserName = new System.Windows.Forms.Label();
            this.lblCommonsPassword = new System.Windows.Forms.Label();
            this.txtCommonsUserName = new System.Windows.Forms.TextBox();
            this.txtCommonsPassword = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpCommons = new System.Windows.Forms.GroupBox();
            this.grpLocal = new System.Windows.Forms.GroupBox();
            this.txtLocalDomain = new System.Windows.Forms.TextBox();
            this.txtLocalUserName = new System.Windows.Forms.TextBox();
            this.txtLocalPassword = new System.Windows.Forms.TextBox();
            this.lblDotOrg = new System.Windows.Forms.Label();
            this.lblLocalSysopHint = new System.Windows.Forms.Label();
            this.chkLocalSameAsCommons = new System.Windows.Forms.CheckBox();
            this.chkLocalSysop = new System.Windows.Forms.CheckBox();
            this.lblLocalDomain = new System.Windows.Forms.Label();
            this.lblLocalUserName = new System.Windows.Forms.Label();
            this.lblLocalPassword = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lnkThisThatOther = new System.Windows.Forms.LinkLabel();
            this.lblLicense = new System.Windows.Forms.Label();
            this.lnkAppName = new System.Windows.Forms.LinkLabel();
            this.chkOpenBrowserLocal = new System.Windows.Forms.CheckBox();
            this.chkOpenBrowserAutomatically = new System.Windows.Forms.CheckBox();
            this.chkAutoUpdate = new System.Windows.Forms.CheckBox();
            this.chkSaveCreds = new System.Windows.Forms.CheckBox();
            this.lblLocalDataHint = new System.Windows.Forms.Label();
            this.grpLocalData = new System.Windows.Forms.GroupBox();
            this.lblLocalCurrent = new System.Windows.Forms.Label();
            this.lblLocalCurrentLabel = new System.Windows.Forms.Label();
            this.btnLocalDataReset = new System.Windows.Forms.Button();
            this.btnLocalDataLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkUseHttps = new System.Windows.Forms.CheckBox();
            this.panRightSide = new System.Windows.Forms.Panel();
            this.chkLogTransfers = new System.Windows.Forms.CheckBox();
            this.grpCommons.SuspendLayout();
            this.grpLocal.SuspendLayout();
            this.grpLocalData.SuspendLayout();
            this.panRightSide.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCommonsUserName
            // 
            this.lblCommonsUserName.AutoSize = true;
            this.lblCommonsUserName.Location = new System.Drawing.Point(8, 20);
            this.lblCommonsUserName.Name = "lblCommonsUserName";
            this.lblCommonsUserName.Size = new System.Drawing.Size(70, 13);
            this.lblCommonsUserName.TabIndex = 0;
            this.lblCommonsUserName.Text = "<username>";
            // 
            // lblCommonsPassword
            // 
            this.lblCommonsPassword.AutoSize = true;
            this.lblCommonsPassword.Location = new System.Drawing.Point(8, 44);
            this.lblCommonsPassword.Name = "lblCommonsPassword";
            this.lblCommonsPassword.Size = new System.Drawing.Size(69, 13);
            this.lblCommonsPassword.TabIndex = 2;
            this.lblCommonsPassword.Text = "<password>";
            // 
            // txtCommonsUserName
            // 
            this.txtCommonsUserName.Location = new System.Drawing.Point(96, 16);
            this.txtCommonsUserName.Name = "txtCommonsUserName";
            this.txtCommonsUserName.Size = new System.Drawing.Size(208, 21);
            this.txtCommonsUserName.TabIndex = 1;
            // 
            // txtCommonsPassword
            // 
            this.txtCommonsPassword.AcceptsReturn = true;
            this.txtCommonsPassword.Location = new System.Drawing.Point(96, 40);
            this.txtCommonsPassword.Name = "txtCommonsPassword";
            this.txtCommonsPassword.Size = new System.Drawing.Size(208, 21);
            this.txtCommonsPassword.TabIndex = 3;
            this.txtCommonsPassword.UseSystemPasswordChar = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(615, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "<cancel>";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(535, 248);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "<ok>";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.button2_Click);
            // 
            // grpCommons
            // 
            this.grpCommons.Controls.Add(this.txtCommonsUserName);
            this.grpCommons.Controls.Add(this.lblCommonsUserName);
            this.grpCommons.Controls.Add(this.lblCommonsPassword);
            this.grpCommons.Controls.Add(this.txtCommonsPassword);
            this.grpCommons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpCommons.Location = new System.Drawing.Point(8, 8);
            this.grpCommons.Name = "grpCommons";
            this.grpCommons.Size = new System.Drawing.Size(312, 72);
            this.grpCommons.TabIndex = 0;
            this.grpCommons.TabStop = false;
            this.grpCommons.Text = "<commons>";
            // 
            // grpLocal
            // 
            this.grpLocal.Controls.Add(this.txtLocalDomain);
            this.grpLocal.Controls.Add(this.txtLocalUserName);
            this.grpLocal.Controls.Add(this.txtLocalPassword);
            this.grpLocal.Controls.Add(this.lblDotOrg);
            this.grpLocal.Controls.Add(this.lblLocalSysopHint);
            this.grpLocal.Controls.Add(this.chkLocalSameAsCommons);
            this.grpLocal.Controls.Add(this.chkLocalSysop);
            this.grpLocal.Controls.Add(this.lblLocalDomain);
            this.grpLocal.Controls.Add(this.lblLocalUserName);
            this.grpLocal.Controls.Add(this.lblLocalPassword);
            this.grpLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpLocal.Location = new System.Drawing.Point(8, 88);
            this.grpLocal.Name = "grpLocal";
            this.grpLocal.Size = new System.Drawing.Size(312, 175);
            this.grpLocal.TabIndex = 1;
            this.grpLocal.TabStop = false;
            this.grpLocal.Text = "<local>";
            // 
            // txtLocalDomain
            // 
            this.txtLocalDomain.Location = new System.Drawing.Point(96, 17);
            this.txtLocalDomain.Name = "txtLocalDomain";
            this.txtLocalDomain.Size = new System.Drawing.Size(88, 21);
            this.txtLocalDomain.TabIndex = 1;
            this.txtLocalDomain.Text = "en.wikipedia";
            this.txtLocalDomain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLocalUserName
            // 
            this.txtLocalUserName.Location = new System.Drawing.Point(96, 61);
            this.txtLocalUserName.Name = "txtLocalUserName";
            this.txtLocalUserName.Size = new System.Drawing.Size(208, 21);
            this.txtLocalUserName.TabIndex = 5;
            // 
            // txtLocalPassword
            // 
            this.txtLocalPassword.Location = new System.Drawing.Point(96, 85);
            this.txtLocalPassword.Name = "txtLocalPassword";
            this.txtLocalPassword.Size = new System.Drawing.Size(208, 21);
            this.txtLocalPassword.TabIndex = 7;
            this.txtLocalPassword.UseSystemPasswordChar = true;
            // 
            // lblDotOrg
            // 
            this.lblDotOrg.AutoSize = true;
            this.lblDotOrg.Location = new System.Drawing.Point(184, 20);
            this.lblDotOrg.Margin = new System.Windows.Forms.Padding(0);
            this.lblDotOrg.Name = "lblDotOrg";
            this.lblDotOrg.Size = new System.Drawing.Size(27, 13);
            this.lblDotOrg.TabIndex = 2;
            this.lblDotOrg.Text = ".org";
            // 
            // lblLocalSysopHint
            // 
            this.lblLocalSysopHint.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalSysopHint.Location = new System.Drawing.Point(24, 127);
            this.lblLocalSysopHint.Name = "lblLocalSysopHint";
            this.lblLocalSysopHint.Size = new System.Drawing.Size(280, 40);
            this.lblLocalSysopHint.TabIndex = 9;
            this.lblLocalSysopHint.Text = "<admindesctext>";
            // 
            // chkLocalSameAsCommons
            // 
            this.chkLocalSameAsCommons.AutoSize = true;
            this.chkLocalSameAsCommons.Location = new System.Drawing.Point(10, 42);
            this.chkLocalSameAsCommons.Name = "chkLocalSameAsCommons";
            this.chkLocalSameAsCommons.Size = new System.Drawing.Size(154, 17);
            this.chkLocalSameAsCommons.TabIndex = 3;
            this.chkLocalSameAsCommons.Text = "<same creds as commons>";
            this.chkLocalSameAsCommons.UseVisualStyleBackColor = true;
            this.chkLocalSameAsCommons.CheckedChanged += new System.EventHandler(this.chkLocalSameAsCommons_CheckedChanged);
            // 
            // chkLocalSysop
            // 
            this.chkLocalSysop.AutoSize = true;
            this.chkLocalSysop.Location = new System.Drawing.Point(10, 109);
            this.chkLocalSysop.Name = "chkLocalSysop";
            this.chkLocalSysop.Size = new System.Drawing.Size(66, 17);
            this.chkLocalSysop.TabIndex = 8;
            this.chkLocalSysop.Text = "<admin>";
            this.chkLocalSysop.UseVisualStyleBackColor = true;
            // 
            // lblLocalDomain
            // 
            this.lblLocalDomain.AutoSize = true;
            this.lblLocalDomain.Location = new System.Drawing.Point(8, 21);
            this.lblLocalDomain.Name = "lblLocalDomain";
            this.lblLocalDomain.Size = new System.Drawing.Size(57, 13);
            this.lblLocalDomain.TabIndex = 0;
            this.lblLocalDomain.Text = "<domain>";
            // 
            // lblLocalUserName
            // 
            this.lblLocalUserName.AutoSize = true;
            this.lblLocalUserName.Location = new System.Drawing.Point(8, 65);
            this.lblLocalUserName.Name = "lblLocalUserName";
            this.lblLocalUserName.Size = new System.Drawing.Size(70, 13);
            this.lblLocalUserName.TabIndex = 4;
            this.lblLocalUserName.Text = "<username>";
            // 
            // lblLocalPassword
            // 
            this.lblLocalPassword.AutoSize = true;
            this.lblLocalPassword.Location = new System.Drawing.Point(8, 89);
            this.lblLocalPassword.Name = "lblLocalPassword";
            this.lblLocalPassword.Size = new System.Drawing.Size(69, 13);
            this.lblLocalPassword.TabIndex = 6;
            this.lblLocalPassword.Text = "<password>";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(8, 278);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 1);
            this.panel1.TabIndex = 5;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(8, 302);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(56, 18);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "<version>";
            this.lblVersion.UseCompatibleTextRendering = true;
            // 
            // lnkThisThatOther
            // 
            this.lnkThisThatOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkThisThatOther.AutoSize = true;
            this.lnkThisThatOther.Location = new System.Drawing.Point(8, 322);
            this.lnkThisThatOther.Name = "lnkThisThatOther";
            this.lnkThisThatOther.Size = new System.Drawing.Size(52, 13);
            this.lnkThisThatOther.TabIndex = 8;
            this.lnkThisThatOther.TabStop = true;
            this.lnkThisThatOther.Text = "<by tto>";
            this.lnkThisThatOther.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lblLicense
            // 
            this.lblLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLicense.AutoSize = true;
            this.lblLicense.Location = new System.Drawing.Point(8, 338);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(90, 18);
            this.lblLicense.TabIndex = 9;
            this.lblLicense.Text = "<public domain>";
            this.lblLicense.UseCompatibleTextRendering = true;
            // 
            // lnkAppName
            // 
            this.lnkAppName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkAppName.AutoSize = true;
            this.lnkAppName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkAppName.Location = new System.Drawing.Point(8, 286);
            this.lnkAppName.Name = "lnkAppName";
            this.lnkAppName.Size = new System.Drawing.Size(131, 18);
            this.lnkAppName.TabIndex = 6;
            this.lnkAppName.TabStop = true;
            this.lnkAppName.Text = "For the Common Good";
            this.lnkAppName.UseCompatibleTextRendering = true;
            this.lnkAppName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // chkOpenBrowserLocal
            // 
            this.chkOpenBrowserLocal.AutoSize = true;
            this.chkOpenBrowserLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkOpenBrowserLocal.Location = new System.Drawing.Point(2, 94);
            this.chkOpenBrowserLocal.Name = "chkOpenBrowserLocal";
            this.chkOpenBrowserLocal.Size = new System.Drawing.Size(140, 18);
            this.chkOpenBrowserLocal.TabIndex = 4;
            this.chkOpenBrowserLocal.Text = "<open local file page>";
            this.chkOpenBrowserLocal.UseVisualStyleBackColor = true;
            // 
            // chkOpenBrowserAutomatically
            // 
            this.chkOpenBrowserAutomatically.AutoSize = true;
            this.chkOpenBrowserAutomatically.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkOpenBrowserAutomatically.Location = new System.Drawing.Point(2, 72);
            this.chkOpenBrowserAutomatically.Name = "chkOpenBrowserAutomatically";
            this.chkOpenBrowserAutomatically.Size = new System.Drawing.Size(163, 18);
            this.chkOpenBrowserAutomatically.TabIndex = 3;
            this.chkOpenBrowserAutomatically.Text = "<open commons file page>";
            this.chkOpenBrowserAutomatically.UseMnemonic = false;
            this.chkOpenBrowserAutomatically.UseVisualStyleBackColor = true;
            // 
            // chkAutoUpdate
            // 
            this.chkAutoUpdate.AutoSize = true;
            this.chkAutoUpdate.Checked = true;
            this.chkAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAutoUpdate.Location = new System.Drawing.Point(2, 44);
            this.chkAutoUpdate.Name = "chkAutoUpdate";
            this.chkAutoUpdate.Size = new System.Drawing.Size(107, 18);
            this.chkAutoUpdate.TabIndex = 2;
            this.chkAutoUpdate.Text = "<auto update>";
            this.chkAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // chkSaveCreds
            // 
            this.chkSaveCreds.AutoSize = true;
            this.chkSaveCreds.Checked = true;
            this.chkSaveCreds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveCreds.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkSaveCreds.Location = new System.Drawing.Point(2, 0);
            this.chkSaveCreds.Name = "chkSaveCreds";
            this.chkSaveCreds.Size = new System.Drawing.Size(159, 18);
            this.chkSaveCreds.TabIndex = 0;
            this.chkSaveCreds.Text = "<save passwords to disk>";
            this.chkSaveCreds.UseVisualStyleBackColor = true;
            // 
            // lblLocalDataHint
            // 
            this.lblLocalDataHint.Location = new System.Drawing.Point(10, 20);
            this.lblLocalDataHint.Name = "lblLocalDataHint";
            this.lblLocalDataHint.Size = new System.Drawing.Size(198, 56);
            this.lblLocalDataHint.TabIndex = 0;
            this.lblLocalDataHint.Text = "<local data hint>";
            // 
            // grpLocalData
            // 
            this.grpLocalData.Controls.Add(this.lblLocalCurrent);
            this.grpLocalData.Controls.Add(this.lblLocalCurrentLabel);
            this.grpLocalData.Controls.Add(this.btnLocalDataReset);
            this.grpLocalData.Controls.Add(this.btnLocalDataLoad);
            this.grpLocalData.Controls.Add(this.lblLocalDataHint);
            this.grpLocalData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpLocalData.Location = new System.Drawing.Point(0, 146);
            this.grpLocalData.Name = "grpLocalData";
            this.grpLocalData.Size = new System.Drawing.Size(360, 84);
            this.grpLocalData.TabIndex = 6;
            this.grpLocalData.TabStop = false;
            this.grpLocalData.Text = "<local wiki data>";
            // 
            // lblLocalCurrent
            // 
            this.lblLocalCurrent.Location = new System.Drawing.Point(211, 61);
            this.lblLocalCurrent.Name = "lblLocalCurrent";
            this.lblLocalCurrent.Size = new System.Drawing.Size(144, 16);
            this.lblLocalCurrent.TabIndex = 4;
            this.lblLocalCurrent.Text = "<wiki domain>";
            this.lblLocalCurrent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLocalCurrentLabel
            // 
            this.lblLocalCurrentLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLocalCurrentLabel.Location = new System.Drawing.Point(211, 44);
            this.lblLocalCurrentLabel.Name = "lblLocalCurrentLabel";
            this.lblLocalCurrentLabel.Size = new System.Drawing.Size(144, 16);
            this.lblLocalCurrentLabel.TabIndex = 3;
            this.lblLocalCurrentLabel.Text = "<currently using>";
            this.lblLocalCurrentLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnLocalDataReset
            // 
            this.btnLocalDataReset.Location = new System.Drawing.Point(288, 16);
            this.btnLocalDataReset.Name = "btnLocalDataReset";
            this.btnLocalDataReset.Size = new System.Drawing.Size(67, 23);
            this.btnLocalDataReset.TabIndex = 2;
            this.btnLocalDataReset.Text = "<reset>";
            this.btnLocalDataReset.UseVisualStyleBackColor = true;
            this.btnLocalDataReset.Click += new System.EventHandler(this.btnLocalDataReset_Click);
            // 
            // btnLocalDataLoad
            // 
            this.btnLocalDataLoad.Location = new System.Drawing.Point(208, 16);
            this.btnLocalDataLoad.Name = "btnLocalDataLoad";
            this.btnLocalDataLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLocalDataLoad.TabIndex = 1;
            this.btnLocalDataLoad.Text = "<load>";
            this.btnLocalDataLoad.UseVisualStyleBackColor = true;
            this.btnLocalDataLoad.Click += new System.EventHandler(this.btnLocalDataLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Wiki data files (*.wiki)|*.wiki|All files (*.*)|*.*";
            this.openFileDialog1.Title = "Select wiki";
            // 
            // chkUseHttps
            // 
            this.chkUseHttps.AutoSize = true;
            this.chkUseHttps.Checked = true;
            this.chkUseHttps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseHttps.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkUseHttps.Location = new System.Drawing.Point(2, 22);
            this.chkUseHttps.Name = "chkUseHttps";
            this.chkUseHttps.Size = new System.Drawing.Size(93, 18);
            this.chkUseHttps.TabIndex = 1;
            this.chkUseHttps.Text = "<use https>";
            this.chkUseHttps.UseVisualStyleBackColor = true;
            // 
            // panRightSide
            // 
            this.panRightSide.Controls.Add(this.chkSaveCreds);
            this.panRightSide.Controls.Add(this.grpLocalData);
            this.panRightSide.Controls.Add(this.chkUseHttps);
            this.panRightSide.Controls.Add(this.chkLogTransfers);
            this.panRightSide.Controls.Add(this.chkOpenBrowserLocal);
            this.panRightSide.Controls.Add(this.chkOpenBrowserAutomatically);
            this.panRightSide.Controls.Add(this.chkAutoUpdate);
            this.panRightSide.Location = new System.Drawing.Point(328, 8);
            this.panRightSide.Name = "panRightSide";
            this.panRightSide.Size = new System.Drawing.Size(360, 232);
            this.panRightSide.TabIndex = 2;
            // 
            // chkLogTransfers
            // 
            this.chkLogTransfers.AutoSize = true;
            this.chkLogTransfers.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkLogTransfers.Location = new System.Drawing.Point(2, 116);
            this.chkLogTransfers.Name = "chkLogTransfers";
            this.chkLogTransfers.Size = new System.Drawing.Size(109, 18);
            this.chkLogTransfers.TabIndex = 5;
            this.chkLogTransfers.Text = "<log transfers>";
            this.chkLogTransfers.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(696, 358);
            this.Controls.Add(this.panRightSide);
            this.Controls.Add(this.lnkAppName);
            this.Controls.Add(this.lblLicense);
            this.Controls.Add(this.lnkThisThatOther);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpLocal);
            this.Controls.Add(this.grpCommons);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "<settings>";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.grpCommons.ResumeLayout(false);
            this.grpCommons.PerformLayout();
            this.grpLocal.ResumeLayout(false);
            this.grpLocal.PerformLayout();
            this.grpLocalData.ResumeLayout(false);
            this.panRightSide.ResumeLayout(false);
            this.panRightSide.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCommonsUserName;
        private System.Windows.Forms.Label lblCommonsPassword;
        private System.Windows.Forms.TextBox txtCommonsUserName;
        private System.Windows.Forms.TextBox txtCommonsPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grpCommons;
        private System.Windows.Forms.GroupBox grpLocal;
        private System.Windows.Forms.TextBox txtLocalUserName;
        private System.Windows.Forms.Label lblLocalUserName;
        private System.Windows.Forms.Label lblLocalPassword;
        private System.Windows.Forms.TextBox txtLocalPassword;
        private System.Windows.Forms.Label lblLocalSysopHint;
        private System.Windows.Forms.CheckBox chkLocalSysop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel lnkThisThatOther;
        private System.Windows.Forms.Label lblLicense;
        private System.Windows.Forms.LinkLabel lnkAppName;
        private System.Windows.Forms.CheckBox chkOpenBrowserLocal;
        private System.Windows.Forms.CheckBox chkOpenBrowserAutomatically;
        private System.Windows.Forms.CheckBox chkAutoUpdate;
        private System.Windows.Forms.Label lblDotOrg;
        private System.Windows.Forms.TextBox txtLocalDomain;
        private System.Windows.Forms.Label lblLocalDomain;
        private System.Windows.Forms.CheckBox chkSaveCreds;
        private System.Windows.Forms.CheckBox chkLocalSameAsCommons;
        private System.Windows.Forms.Label lblLocalDataHint;
        private System.Windows.Forms.GroupBox grpLocalData;
        private System.Windows.Forms.Label lblLocalCurrent;
        private System.Windows.Forms.Label lblLocalCurrentLabel;
        private System.Windows.Forms.Button btnLocalDataLoad;
        private System.Windows.Forms.Button btnLocalDataReset;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkUseHttps;
        private System.Windows.Forms.Panel panRightSide;
        private System.Windows.Forms.CheckBox chkLogTransfers;
    }
}