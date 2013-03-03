namespace ForTheCommonGood
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblOriginalFilename = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.lblExifNotice = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblLocalFileDesc = new System.Windows.Forms.Label();
            this.lblCommonsFileDesc = new System.Windows.Forms.Label();
            this.lblNormName = new System.Windows.Forms.Label();
            this.txtNormName = new System.Windows.Forms.TextBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.chkIgnoreWarnings = new System.Windows.Forms.CheckBox();
            this.lnkLocalFile = new System.Windows.Forms.LinkLabel();
            this.lnkCommonsFile = new System.Windows.Forms.LinkLabel();
            this.btnRandomFile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkDeleteAfter = new System.Windows.Forms.CheckBox();
            this.optCategory1 = new System.Windows.Forms.RadioButton();
            this.optCategory2 = new System.Windows.Forms.RadioButton();
            this.lblName = new System.Windows.Forms.Label();
            this.lblRevision = new System.Windows.Forms.Label();
            this.lblDimensions = new System.Windows.Forms.Label();
            this.lblPastRevisions = new System.Windows.Forms.Label();
            this.btnPastRevisions = new System.Windows.Forms.Button();
            this.panWarning = new System.Windows.Forms.Panel();
            this.panWarningTexts = new System.Windows.Forms.FlowLayoutPanel();
            this.icoWarning = new System.Windows.Forms.PictureBox();
            this.lblWarningHeading = new System.Windows.Forms.Label();
            this.optOther = new System.Windows.Forms.RadioButton();
            this.panRoot = new System.Windows.Forms.Panel();
            this.toolBarLinks = new System.Windows.Forms.ToolStrip();
            this.btnPreview = new System.Windows.Forms.ToolStripButton();
            this.btnLinkify = new System.Windows.Forms.ToolStripButton();
            this.lnkGoogleImageSearch = new System.Windows.Forms.LinkLabel();
            this.panFileLinks = new System.Windows.Forms.Panel();
            this.lblFileLinks = new System.Windows.Forms.Label();
            this.lnkGoToFileLink = new System.Windows.Forms.LinkLabel();
            this.lstFileLinks = new System.Windows.Forms.ListBox();
            this.optCategory3 = new System.Windows.Forms.RadioButton();
            this.btnViewExif = new System.Windows.Forms.Button();
            this.lblViewExif = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblDeclineTransfer = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panStatus = new System.Windows.Forms.Panel();
            this.icoInfo = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoWarning)).BeginInit();
            this.panRoot.SuspendLayout();
            this.toolBarLinks.SuspendLayout();
            this.panFileLinks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOriginalFilename
            // 
            this.lblOriginalFilename.Location = new System.Drawing.Point(8, 21);
            this.lblOriginalFilename.Name = "lblOriginalFilename";
            this.lblOriginalFilename.Size = new System.Drawing.Size(44, 29);
            this.lblOriginalFilename.TabIndex = 0;
            this.lblOriginalFilename.Text = "<name>";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(56, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(224, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(288, 17);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(71, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "<go>";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(3, 16);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(474, 224);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "<blurb>";
            this.textBox2.WordWrap = false;
            // 
            // textBox3
            // 
            this.textBox3.AcceptsReturn = true;
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(483, 16);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(474, 224);
            this.textBox3.TabIndex = 3;
            this.textBox3.WordWrap = false;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransfer.Enabled = false;
            this.btnTransfer.Location = new System.Drawing.Point(856, 399);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(112, 23);
            this.btnTransfer.TabIndex = 26;
            this.btnTransfer.Text = "<transfer>";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.DoTransfer);
            // 
            // lblExifNotice
            // 
            this.lblExifNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblExifNotice.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExifNotice.Location = new System.Drawing.Point(320, 499);
            this.lblExifNotice.Name = "lblExifNotice";
            this.lblExifNotice.Size = new System.Drawing.Size(304, 13);
            this.lblExifNotice.TabIndex = 15;
            this.lblExifNotice.Text = "<exif rotation warning>";
            this.lblExifNotice.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblLocalFileDesc, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCommonsFileDesc, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 56);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(960, 243);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // lblLocalFileDesc
            // 
            this.lblLocalFileDesc.AutoSize = true;
            this.lblLocalFileDesc.Location = new System.Drawing.Point(3, 0);
            this.lblLocalFileDesc.Name = "lblLocalFileDesc";
            this.lblLocalFileDesc.Size = new System.Drawing.Size(113, 13);
            this.lblLocalFileDesc.TabIndex = 0;
            this.lblLocalFileDesc.Text = "<local file desc page>";
            // 
            // lblCommonsFileDesc
            // 
            this.lblCommonsFileDesc.AutoSize = true;
            this.lblCommonsFileDesc.Location = new System.Drawing.Point(483, 0);
            this.lblCommonsFileDesc.Name = "lblCommonsFileDesc";
            this.lblCommonsFileDesc.Size = new System.Drawing.Size(136, 13);
            this.lblCommonsFileDesc.TabIndex = 2;
            this.lblCommonsFileDesc.Text = "<commons file desc page>";
            // 
            // lblNormName
            // 
            this.lblNormName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNormName.AutoSize = true;
            this.lblNormName.Location = new System.Drawing.Point(712, 307);
            this.lblNormName.Name = "lblNormName";
            this.lblNormName.Size = new System.Drawing.Size(86, 13);
            this.lblNormName.TabIndex = 22;
            this.lblNormName.Text = "<new filename>";
            // 
            // txtNormName
            // 
            this.txtNormName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNormName.Enabled = false;
            this.txtNormName.Location = new System.Drawing.Point(712, 323);
            this.txtNormName.Name = "txtNormName";
            this.txtNormName.Size = new System.Drawing.Size(256, 21);
            this.txtNormName.TabIndex = 23;
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(884, 8);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(84, 23);
            this.btnSettings.TabIndex = 31;
            this.btnSettings.Text = "<settings>";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // chkIgnoreWarnings
            // 
            this.chkIgnoreWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIgnoreWarnings.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkIgnoreWarnings.Enabled = false;
            this.chkIgnoreWarnings.Location = new System.Drawing.Point(715, 350);
            this.chkIgnoreWarnings.Name = "chkIgnoreWarnings";
            this.chkIgnoreWarnings.Size = new System.Drawing.Size(253, 18);
            this.chkIgnoreWarnings.TabIndex = 24;
            this.chkIgnoreWarnings.Text = "<ignore warnings>";
            this.chkIgnoreWarnings.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkIgnoreWarnings.UseVisualStyleBackColor = true;
            // 
            // lnkLocalFile
            // 
            this.lnkLocalFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkLocalFile.Enabled = false;
            this.lnkLocalFile.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkLocalFile.Location = new System.Drawing.Point(712, 428);
            this.lnkLocalFile.Name = "lnkLocalFile";
            this.lnkLocalFile.Size = new System.Drawing.Size(256, 13);
            this.lnkLocalFile.TabIndex = 27;
            this.lnkLocalFile.TabStop = true;
            this.lnkLocalFile.Text = "<view on local wiki>";
            this.lnkLocalFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLocalFile_LinkClicked);
            // 
            // lnkCommonsFile
            // 
            this.lnkCommonsFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkCommonsFile.Enabled = false;
            this.lnkCommonsFile.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkCommonsFile.Location = new System.Drawing.Point(712, 448);
            this.lnkCommonsFile.Name = "lnkCommonsFile";
            this.lnkCommonsFile.Size = new System.Drawing.Size(256, 13);
            this.lnkCommonsFile.TabIndex = 28;
            this.lnkCommonsFile.TabStop = true;
            this.lnkCommonsFile.Text = "<view on commons>";
            this.lnkCommonsFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCommonsFile_LinkClicked);
            // 
            // btnRandomFile
            // 
            this.btnRandomFile.Location = new System.Drawing.Point(372, 17);
            this.btnRandomFile.Name = "btnRandomFile";
            this.btnRandomFile.Size = new System.Drawing.Size(108, 23);
            this.btnRandomFile.TabIndex = 4;
            this.btnRandomFile.Text = "<random/next>";
            this.btnRandomFile.UseVisualStyleBackColor = true;
            this.btnRandomFile.Click += new System.EventHandler(this.RandomImage);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(365, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 40);
            this.panel1.TabIndex = 3;
            // 
            // chkDeleteAfter
            // 
            this.chkDeleteAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDeleteAfter.Checked = true;
            this.chkDeleteAfter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeleteAfter.Enabled = false;
            this.chkDeleteAfter.Location = new System.Drawing.Point(715, 369);
            this.chkDeleteAfter.Name = "chkDeleteAfter";
            this.chkDeleteAfter.Size = new System.Drawing.Size(253, 30);
            this.chkDeleteAfter.TabIndex = 25;
            this.chkDeleteAfter.Text = "<tag/delete after transfer>";
            this.chkDeleteAfter.UseVisualStyleBackColor = true;
            // 
            // optCategory1
            // 
            this.optCategory1.AutoSize = true;
            this.optCategory1.Location = new System.Drawing.Point(488, 4);
            this.optCategory1.Name = "optCategory1";
            this.optCategory1.Size = new System.Drawing.Size(93, 17);
            this.optCategory1.TabIndex = 5;
            this.optCategory1.Text = "<category 1>";
            this.optCategory1.UseVisualStyleBackColor = true;
            this.optCategory1.CheckedChanged += new System.EventHandler(this.optCopyFoo_CheckedChanged);
            // 
            // optCategory2
            // 
            this.optCategory2.AutoSize = true;
            this.optCategory2.Location = new System.Drawing.Point(488, 20);
            this.optCategory2.Name = "optCategory2";
            this.optCategory2.Size = new System.Drawing.Size(93, 17);
            this.optCategory2.TabIndex = 6;
            this.optCategory2.Text = "<category 2>";
            this.optCategory2.UseVisualStyleBackColor = true;
            this.optCategory2.CheckedChanged += new System.EventHandler(this.optCopyFoo_CheckedChanged);
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoEllipsis = true;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(320, 307);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(376, 13);
            this.lblName.TabIndex = 11;
            this.lblName.UseMnemonic = false;
            // 
            // lblRevision
            // 
            this.lblRevision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRevision.Location = new System.Drawing.Point(320, 325);
            this.lblRevision.Name = "lblRevision";
            this.lblRevision.Size = new System.Drawing.Size(236, 13);
            this.lblRevision.TabIndex = 12;
            // 
            // lblDimensions
            // 
            this.lblDimensions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDimensions.Location = new System.Drawing.Point(320, 343);
            this.lblDimensions.Name = "lblDimensions";
            this.lblDimensions.Size = new System.Drawing.Size(236, 13);
            this.lblDimensions.TabIndex = 13;
            // 
            // lblPastRevisions
            // 
            this.lblPastRevisions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPastRevisions.Location = new System.Drawing.Point(560, 325);
            this.lblPastRevisions.Name = "lblPastRevisions";
            this.lblPastRevisions.Size = new System.Drawing.Size(130, 13);
            this.lblPastRevisions.TabIndex = 16;
            this.lblPastRevisions.Text = "<X earlier versions>";
            this.lblPastRevisions.Visible = false;
            // 
            // btnPastRevisions
            // 
            this.btnPastRevisions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPastRevisions.Location = new System.Drawing.Point(560, 340);
            this.btnPastRevisions.Name = "btnPastRevisions";
            this.btnPastRevisions.Size = new System.Drawing.Size(112, 23);
            this.btnPastRevisions.TabIndex = 17;
            this.btnPastRevisions.Text = "<select version>";
            this.btnPastRevisions.UseVisualStyleBackColor = true;
            this.btnPastRevisions.Visible = false;
            this.btnPastRevisions.Click += new System.EventHandler(this.SelectVersion);
            // 
            // panWarning
            // 
            this.panWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panWarning.Controls.Add(this.panWarningTexts);
            this.panWarning.Controls.Add(this.icoWarning);
            this.panWarning.Controls.Add(this.lblWarningHeading);
            this.panWarning.Location = new System.Drawing.Point(320, 367);
            this.panWarning.Name = "panWarning";
            this.panWarning.Size = new System.Drawing.Size(232, 126);
            this.panWarning.TabIndex = 14;
            this.panWarning.Visible = false;
            this.panWarning.Paint += new System.Windows.Forms.PaintEventHandler(this.FmboxLookalike);
            // 
            // panWarningTexts
            // 
            this.panWarningTexts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panWarningTexts.AutoScroll = true;
            this.panWarningTexts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panWarningTexts.Location = new System.Drawing.Point(6, 45);
            this.panWarningTexts.Name = "panWarningTexts";
            this.panWarningTexts.Size = new System.Drawing.Size(219, 74);
            this.panWarningTexts.TabIndex = 2;
            this.panWarningTexts.WrapContents = false;
            // 
            // icoWarning
            // 
            this.icoWarning.Image = global::ForTheCommonGood.Properties.Resources.Imbox_content;
            this.icoWarning.Location = new System.Drawing.Point(8, 6);
            this.icoWarning.Name = "icoWarning";
            this.icoWarning.Size = new System.Drawing.Size(32, 32);
            this.icoWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoWarning.TabIndex = 1;
            this.icoWarning.TabStop = false;
            // 
            // lblWarningHeading
            // 
            this.lblWarningHeading.AutoSize = true;
            this.lblWarningHeading.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarningHeading.ForeColor = System.Drawing.Color.Firebrick;
            this.lblWarningHeading.Location = new System.Drawing.Point(42, 13);
            this.lblWarningHeading.Name = "lblWarningHeading";
            this.lblWarningHeading.Size = new System.Drawing.Size(48, 16);
            this.lblWarningHeading.TabIndex = 0;
            this.lblWarningHeading.Text = "Notice";
            // 
            // optOther
            // 
            this.optOther.AutoSize = true;
            this.optOther.Location = new System.Drawing.Point(744, 4);
            this.optOther.Name = "optOther";
            this.optOther.Size = new System.Drawing.Size(102, 17);
            this.optOther.TabIndex = 8;
            this.optOther.Text = "<other source>";
            this.optOther.UseVisualStyleBackColor = true;
            this.optOther.CheckedChanged += new System.EventHandler(this.optCopyFoo_CheckedChanged);
            this.optOther.Click += new System.EventHandler(this.optOther_CheckedChanged);
            // 
            // panRoot
            // 
            this.panRoot.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.panRoot.Controls.Add(this.toolBarLinks);
            this.panRoot.Controls.Add(this.lnkGoogleImageSearch);
            this.panRoot.Controls.Add(this.lblExifNotice);
            this.panRoot.Controls.Add(this.panFileLinks);
            this.panRoot.Controls.Add(this.optCategory3);
            this.panRoot.Controls.Add(this.optOther);
            this.panRoot.Controls.Add(this.lblOriginalFilename);
            this.panRoot.Controls.Add(this.panWarning);
            this.panRoot.Controls.Add(this.textBox1);
            this.panRoot.Controls.Add(this.btnViewExif);
            this.panRoot.Controls.Add(this.btnPastRevisions);
            this.panRoot.Controls.Add(this.btnGo);
            this.panRoot.Controls.Add(this.lblViewExif);
            this.panRoot.Controls.Add(this.lblPastRevisions);
            this.panRoot.Controls.Add(this.pictureBox1);
            this.panRoot.Controls.Add(this.lblDeclineTransfer);
            this.panRoot.Controls.Add(this.btnTransfer);
            this.panRoot.Controls.Add(this.tableLayoutPanel1);
            this.panRoot.Controls.Add(this.lblDimensions);
            this.panRoot.Controls.Add(this.lblNormName);
            this.panRoot.Controls.Add(this.lblRevision);
            this.panRoot.Controls.Add(this.txtNormName);
            this.panRoot.Controls.Add(this.lblName);
            this.panRoot.Controls.Add(this.btnSettings);
            this.panRoot.Controls.Add(this.optCategory2);
            this.panRoot.Controls.Add(this.chkIgnoreWarnings);
            this.panRoot.Controls.Add(this.optCategory1);
            this.panRoot.Controls.Add(this.chkDeleteAfter);
            this.panRoot.Controls.Add(this.panel2);
            this.panRoot.Controls.Add(this.panel1);
            this.panRoot.Controls.Add(this.lnkLocalFile);
            this.panRoot.Controls.Add(this.btnRandomFile);
            this.panRoot.Controls.Add(this.lnkCommonsFile);
            this.panRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRoot.Location = new System.Drawing.Point(0, 0);
            this.panRoot.Name = "panRoot";
            this.panRoot.Size = new System.Drawing.Size(977, 522);
            this.panRoot.TabIndex = 0;
            this.panRoot.Paint += new System.Windows.Forms.PaintEventHandler(this.panRoot_Paint);
            // 
            // toolBarLinks
            // 
            this.toolBarLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolBarLinks.AutoSize = false;
            this.toolBarLinks.CanOverflow = false;
            this.toolBarLinks.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBarLinks.Enabled = false;
            this.toolBarLinks.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBarLinks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPreview,
            this.btnLinkify});
            this.toolBarLinks.Location = new System.Drawing.Point(917, 46);
            this.toolBarLinks.Name = "toolBarLinks";
            this.toolBarLinks.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBarLinks.Size = new System.Drawing.Size(48, 25);
            this.toolBarLinks.TabIndex = 32;
            // 
            // btnPreview
            // 
            this.btnPreview.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
            this.btnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(23, 22);
            this.btnPreview.Text = "<preview>";
            this.btnPreview.Click += new System.EventHandler(this.lnkPreviewWikitext_LinkClicked);
            // 
            // btnLinkify
            // 
            this.btnLinkify.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnLinkify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLinkify.Image = ((System.Drawing.Image)(resources.GetObject("btnLinkify.Image")));
            this.btnLinkify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLinkify.Name = "btnLinkify";
            this.btnLinkify.Size = new System.Drawing.Size(23, 22);
            this.btnLinkify.Text = "<linkify>";
            this.btnLinkify.Click += new System.EventHandler(this.lnkLinkify_LinkClicked);
            // 
            // lnkGoogleImageSearch
            // 
            this.lnkGoogleImageSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkGoogleImageSearch.Enabled = false;
            this.lnkGoogleImageSearch.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkGoogleImageSearch.Location = new System.Drawing.Point(712, 468);
            this.lnkGoogleImageSearch.Name = "lnkGoogleImageSearch";
            this.lnkGoogleImageSearch.Size = new System.Drawing.Size(256, 13);
            this.lnkGoogleImageSearch.TabIndex = 29;
            this.lnkGoogleImageSearch.TabStop = true;
            this.lnkGoogleImageSearch.Text = "<search Google>";
            this.lnkGoogleImageSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGoogleImageSearch_LinkClicked);
            // 
            // panFileLinks
            // 
            this.panFileLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panFileLinks.Controls.Add(this.lblFileLinks);
            this.panFileLinks.Controls.Add(this.lnkGoToFileLink);
            this.panFileLinks.Controls.Add(this.lstFileLinks);
            this.panFileLinks.Location = new System.Drawing.Point(560, 408);
            this.panFileLinks.MaximumSize = new System.Drawing.Size(240, 1000);
            this.panFileLinks.Name = "panFileLinks";
            this.panFileLinks.Size = new System.Drawing.Size(136, 108);
            this.panFileLinks.TabIndex = 20;
            // 
            // lblFileLinks
            // 
            this.lblFileLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileLinks.AutoSize = true;
            this.lblFileLinks.Location = new System.Drawing.Point(0, 0);
            this.lblFileLinks.Name = "lblFileLinks";
            this.lblFileLinks.Size = new System.Drawing.Size(124, 13);
            this.lblFileLinks.TabIndex = 0;
            this.lblFileLinks.Text = "<file links/image usage>";
            // 
            // lnkGoToFileLink
            // 
            this.lnkGoToFileLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkGoToFileLink.Enabled = false;
            this.lnkGoToFileLink.Location = new System.Drawing.Point(75, 88);
            this.lnkGoToFileLink.Name = "lnkGoToFileLink";
            this.lnkGoToFileLink.Size = new System.Drawing.Size(61, 16);
            this.lnkGoToFileLink.TabIndex = 2;
            this.lnkGoToFileLink.TabStop = true;
            this.lnkGoToFileLink.Text = "<go> →";
            this.lnkGoToFileLink.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkGoToFileLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGoToFileLink_LinkClicked);
            // 
            // lstFileLinks
            // 
            this.lstFileLinks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFileLinks.Items.AddRange(new object[] {
            " "});
            this.lstFileLinks.Location = new System.Drawing.Point(0, 16);
            this.lstFileLinks.Name = "lstFileLinks";
            this.lstFileLinks.Size = new System.Drawing.Size(136, 69);
            this.lstFileLinks.TabIndex = 1;
            this.lstFileLinks.SelectedIndexChanged += new System.EventHandler(this.lstFileLinks_SelectedIndexChanged);
            this.lstFileLinks.ForeColorChanged += new System.EventHandler(this.lstFileLinks_SelectedIndexChanged);
            // 
            // optCategory3
            // 
            this.optCategory3.AutoSize = true;
            this.optCategory3.Checked = true;
            this.optCategory3.Location = new System.Drawing.Point(488, 36);
            this.optCategory3.Name = "optCategory3";
            this.optCategory3.Size = new System.Drawing.Size(93, 17);
            this.optCategory3.TabIndex = 7;
            this.optCategory3.TabStop = true;
            this.optCategory3.Text = "<category 3>";
            this.optCategory3.UseVisualStyleBackColor = true;
            // 
            // btnViewExif
            // 
            this.btnViewExif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewExif.Location = new System.Drawing.Point(560, 382);
            this.btnViewExif.Name = "btnViewExif";
            this.btnViewExif.Size = new System.Drawing.Size(112, 23);
            this.btnViewExif.TabIndex = 19;
            this.btnViewExif.Text = "<view metadata>";
            this.btnViewExif.UseVisualStyleBackColor = true;
            this.btnViewExif.Visible = false;
            this.btnViewExif.Click += new System.EventHandler(this.ViewExifData);
            // 
            // lblViewExif
            // 
            this.lblViewExif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblViewExif.Location = new System.Drawing.Point(560, 367);
            this.lblViewExif.Name = "lblViewExif";
            this.lblViewExif.Size = new System.Drawing.Size(130, 13);
            this.lblViewExif.TabIndex = 18;
            this.lblViewExif.Text = "<contains EXIF>";
            this.lblViewExif.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackgroundImage = global::ForTheCommonGood.Properties.Resources.Checker_16x16;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 307);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(304, 208);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblDeclineTransfer
            // 
            this.lblDeclineTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeclineTransfer.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDeclineTransfer.Location = new System.Drawing.Point(712, 488);
            this.lblDeclineTransfer.Name = "lblDeclineTransfer";
            this.lblDeclineTransfer.Size = new System.Drawing.Size(255, 28);
            this.lblDeclineTransfer.TabIndex = 30;
            this.lblDeclineTransfer.Text = "<if ineligible, edit file page manually>";
            this.lblDeclineTransfer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(704, 308);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 204);
            this.panel2.TabIndex = 21;
            // 
            // panStatus
            // 
            this.panStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
            this.panStatus.Controls.Add(this.icoInfo);
            this.panStatus.Controls.Add(this.lblStatus);
            this.panStatus.Location = new System.Drawing.Point(388, 232);
            this.panStatus.Name = "panStatus";
            this.panStatus.Size = new System.Drawing.Size(200, 54);
            this.panStatus.TabIndex = 1;
            this.panStatus.Visible = false;
            this.panStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.FmboxLookalike);
            // 
            // icoInfo
            // 
            this.icoInfo.Image = global::ForTheCommonGood.Properties.Resources.Ambox_notice;
            this.icoInfo.Location = new System.Drawing.Point(17, 7);
            this.icoInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.icoInfo.Name = "icoInfo";
            this.icoInfo.Size = new System.Drawing.Size(40, 40);
            this.icoInfo.TabIndex = 4;
            this.icoInfo.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(69, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(123, 24);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "<loading>";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(977, 522);
            this.Controls.Add(this.panStatus);
            this.Controls.Add(this.panRoot);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(993, 560);
            this.Name = "frmMain";
            this.Text = "For the Common Good";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panWarning.ResumeLayout(false);
            this.panWarning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoWarning)).EndInit();
            this.panRoot.ResumeLayout(false);
            this.panRoot.PerformLayout();
            this.toolBarLinks.ResumeLayout(false);
            this.toolBarLinks.PerformLayout();
            this.panFileLinks.ResumeLayout(false);
            this.panFileLinks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.icoInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblOriginalFilename;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Label lblExifNotice;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblNormName;
        private System.Windows.Forms.TextBox txtNormName;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.CheckBox chkIgnoreWarnings;
        private System.Windows.Forms.LinkLabel lnkLocalFile;
        private System.Windows.Forms.LinkLabel lnkCommonsFile;
        private System.Windows.Forms.Button btnRandomFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkDeleteAfter;
        private System.Windows.Forms.RadioButton optCategory1;
        private System.Windows.Forms.RadioButton optCategory2;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblRevision;
        private System.Windows.Forms.Label lblDimensions;
        private System.Windows.Forms.Label lblPastRevisions;
        private System.Windows.Forms.Button btnPastRevisions;
        private System.Windows.Forms.Panel panWarning;
        private System.Windows.Forms.PictureBox icoWarning;
        private System.Windows.Forms.Label lblWarningHeading;
        private System.Windows.Forms.Label lblLocalFileDesc;
        private System.Windows.Forms.Label lblCommonsFileDesc;
        private System.Windows.Forms.RadioButton optOther;
        private System.Windows.Forms.Panel panRoot;
        private System.Windows.Forms.Panel panStatus;
        private System.Windows.Forms.PictureBox icoInfo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.RadioButton optCategory3;
        private System.Windows.Forms.Button btnViewExif;
        private System.Windows.Forms.Label lblViewExif;
        private System.Windows.Forms.ListBox lstFileLinks;
        private System.Windows.Forms.Label lblFileLinks;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lnkGoToFileLink;
        private System.Windows.Forms.Panel panFileLinks;
        private System.Windows.Forms.LinkLabel lnkGoogleImageSearch;
        private System.Windows.Forms.Label lblDeclineTransfer;
        private System.Windows.Forms.ToolStrip toolBarLinks;
        private System.Windows.Forms.ToolStripButton btnLinkify;
        private System.Windows.Forms.ToolStripButton btnPreview;
        private System.Windows.Forms.FlowLayoutPanel panWarningTexts;
    }
}

